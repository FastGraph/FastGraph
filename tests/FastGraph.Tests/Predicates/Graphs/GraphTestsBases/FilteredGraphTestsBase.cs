#nullable enable

using FastGraph.Tests.Structures;
using static FastGraph.Tests.AssertHelpers;
using static FastGraph.Tests.GraphTestHelpers;

namespace FastGraph.Tests.Predicates
{
    /// <summary>
    /// Base class for filtered graph tests.
    /// </summary>
    internal abstract class FilteredGraphTestsBase : GraphTestsBase
    {
        #region Vertices & Edges

        protected static void Vertices_Test<TGraph>(
            TGraph wrappedGraph,
            Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IVertexSet<int>> createFilteredGraph)
            where TGraph : IMutableVertexSet<int>, IMutableGraph<int, Edge<int>>
        {
            IVertexSet<int> filteredGraph = createFilteredGraph(_ => true, _ => true);
            AssertNoVertex(filteredGraph);

            wrappedGraph.AddVertexRange(new[] { 1, 2, 3 });
            AssertHasVertices(filteredGraph, new[] { 1, 2, 3 });


            wrappedGraph.Clear();
            filteredGraph = createFilteredGraph(vertex => vertex < 3, _ => true);
            AssertNoVertex(filteredGraph);

            wrappedGraph.AddVertexRange(new[] { 1, 2, 3 });
            AssertHasVertices(filteredGraph, new[] { 1, 2 });
        }

        public void Edges_Test<TGraph>(
            TGraph wrappedGraph,
            Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IEdgeSet<int, Edge<int>>> createFilteredGraph)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>, IMutableGraph<int, Edge<int>>
        {
            IEdgeSet<int, Edge<int>> filteredGraph = createFilteredGraph(_ => true, _ => true);
            AssertNoEdge(filteredGraph);

            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge22 = new Edge<int>(2, 2);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            var edge41 = new Edge<int>(4, 1);
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge22, edge31, edge33, edge41 });
            AssertHasEdges(filteredGraph, new[] { edge12, edge13, edge22, edge31, edge33, edge41 });


            wrappedGraph.Clear();
            filteredGraph = createFilteredGraph(vertex => vertex <= 3, _ => true);
            AssertNoEdge(filteredGraph);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge22, edge31, edge33, edge41 });
            AssertHasEdges(filteredGraph, new[] { edge12, edge13, edge22, edge31, edge33 });


            wrappedGraph.Clear();
            filteredGraph = createFilteredGraph(_ => true, edge => edge.Source != edge.Target);
            AssertNoEdge(filteredGraph);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge22, edge31, edge33, edge41 });
            AssertHasEdges(filteredGraph, new[] { edge12, edge13, edge31, edge41 });


            wrappedGraph.Clear();
            filteredGraph = createFilteredGraph(vertex => vertex <= 3, edge => edge.Source != edge.Target);
            AssertNoEdge(filteredGraph);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge22, edge31, edge33, edge41 });
            AssertHasEdges(filteredGraph, new[] { edge12, edge13, edge31 });
        }

        #endregion

        #region Contains Vertex

        protected static void ContainsVertex_Test<TGraph>(
            TGraph wrappedGraph,
            Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IImplicitVertexSet<int>> createFilteredGraph)
            where TGraph : IMutableVertexSet<int>, IMutableGraph<int, Edge<int>>
        {
            IImplicitVertexSet<int> filteredGraph = createFilteredGraph(
                _ => true,
                _ => true);

            filteredGraph.ContainsVertex(1).Should().BeFalse();
            filteredGraph.ContainsVertex(2).Should().BeFalse();

            wrappedGraph.AddVertex(1);
            filteredGraph.ContainsVertex(1).Should().BeTrue();
            filteredGraph.ContainsVertex(2).Should().BeFalse();

            wrappedGraph.AddVertex(2);
            filteredGraph.ContainsVertex(1).Should().BeTrue();
            filteredGraph.ContainsVertex(2).Should().BeTrue();

            wrappedGraph.RemoveVertex(1);
            filteredGraph.ContainsVertex(1).Should().BeFalse();
            filteredGraph.ContainsVertex(2).Should().BeTrue();


            wrappedGraph.Clear();
            filteredGraph = createFilteredGraph(
                vertex => vertex <= 2,
                _ => true);

            filteredGraph.ContainsVertex(1).Should().BeFalse();
            filteredGraph.ContainsVertex(2).Should().BeFalse();
            filteredGraph.ContainsVertex(3).Should().BeFalse();

            wrappedGraph.AddVertex(1);
            filteredGraph.ContainsVertex(1).Should().BeTrue();
            filteredGraph.ContainsVertex(2).Should().BeFalse();
            filteredGraph.ContainsVertex(3).Should().BeFalse();

            wrappedGraph.AddVertex(2);
            filteredGraph.ContainsVertex(1).Should().BeTrue();
            filteredGraph.ContainsVertex(2).Should().BeTrue();
            filteredGraph.ContainsVertex(3).Should().BeFalse();

            wrappedGraph.AddVertex(3);
            filteredGraph.ContainsVertex(1).Should().BeTrue();
            filteredGraph.ContainsVertex(2).Should().BeTrue();
            filteredGraph.ContainsVertex(3).Should().BeFalse();    // Filtered

            wrappedGraph.RemoveVertex(1);
            filteredGraph.ContainsVertex(1).Should().BeFalse();
            filteredGraph.ContainsVertex(2).Should().BeTrue();
            filteredGraph.ContainsVertex(3).Should().BeFalse();
        }

        #endregion

        #region Contains Edge

        protected static void ContainsEdge_Test<TGraph>(
            TGraph wrappedGraph,
            Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IEdgeSet<int, Edge<int>>> createFilteredGraph)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>, IMutableGraph<int, Edge<int>>
        {
            #region Part 1

            IEdgeSet<int, Edge<int>> filteredGraph = createFilteredGraph(
                _ => true,
                _ => true);

            ContainsEdge_Test(
                filteredGraph,
                edge => wrappedGraph.AddVerticesAndEdge(edge));

            #endregion

            #region Part 2

            wrappedGraph.Clear();
            filteredGraph = createFilteredGraph(
                vertex => vertex > 0 && vertex < 3,
                _ => true);
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 1);
            var edge4 = new Edge<int>(2, 2);
            var otherEdge1 = new Edge<int>(1, 2);

            filteredGraph.ContainsEdge(edge1).Should().BeFalse();
            filteredGraph.ContainsEdge(edge2).Should().BeFalse();
            filteredGraph.ContainsEdge(edge3).Should().BeFalse();
            filteredGraph.ContainsEdge(edge4).Should().BeFalse();
            filteredGraph.ContainsEdge(otherEdge1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(edge1);
            filteredGraph.ContainsEdge(edge1).Should().BeTrue();
            filteredGraph.ContainsEdge(edge2).Should().BeFalse();
            filteredGraph.ContainsEdge(edge3).Should().BeFalse();
            filteredGraph.ContainsEdge(edge4).Should().BeFalse();
            filteredGraph.ContainsEdge(otherEdge1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(edge2);
            filteredGraph.ContainsEdge(edge1).Should().BeTrue();
            filteredGraph.ContainsEdge(edge2).Should().BeFalse();  // Filtered
            filteredGraph.ContainsEdge(edge3).Should().BeFalse();
            filteredGraph.ContainsEdge(edge4).Should().BeFalse();
            filteredGraph.ContainsEdge(otherEdge1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(edge3);
            filteredGraph.ContainsEdge(edge1).Should().BeTrue();
            filteredGraph.ContainsEdge(edge2).Should().BeFalse();  // Filtered
            filteredGraph.ContainsEdge(edge3).Should().BeTrue();
            filteredGraph.ContainsEdge(edge4).Should().BeFalse();
            filteredGraph.ContainsEdge(otherEdge1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(edge4);
            filteredGraph.ContainsEdge(edge1).Should().BeTrue();
            filteredGraph.ContainsEdge(edge2).Should().BeFalse();  // Filtered
            filteredGraph.ContainsEdge(edge3).Should().BeTrue();
            filteredGraph.ContainsEdge(edge4).Should().BeTrue();
            filteredGraph.ContainsEdge(otherEdge1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(otherEdge1);
            filteredGraph.ContainsEdge(edge1).Should().BeTrue();
            filteredGraph.ContainsEdge(edge2).Should().BeFalse();  // Filtered
            filteredGraph.ContainsEdge(edge3).Should().BeTrue();
            filteredGraph.ContainsEdge(edge4).Should().BeTrue();
            filteredGraph.ContainsEdge(otherEdge1).Should().BeTrue();

            // Both vertices not in graph
            filteredGraph.ContainsEdge(new Edge<int>(0, 10)).Should().BeFalse();
            // Source not in graph
            filteredGraph.ContainsEdge(new Edge<int>(0, 1)).Should().BeFalse();
            // Target not in graph
            filteredGraph.ContainsEdge(new Edge<int>(1, 0)).Should().BeFalse();

            #endregion

            #region Part 3

            wrappedGraph.Clear();
            filteredGraph = createFilteredGraph(
                _ => true,
                edge => edge.Source != edge.Target);

            filteredGraph.ContainsEdge(edge1).Should().BeFalse();
            filteredGraph.ContainsEdge(edge2).Should().BeFalse();
            filteredGraph.ContainsEdge(edge3).Should().BeFalse();
            filteredGraph.ContainsEdge(edge4).Should().BeFalse();
            filteredGraph.ContainsEdge(otherEdge1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(edge1);
            filteredGraph.ContainsEdge(edge1).Should().BeTrue();
            filteredGraph.ContainsEdge(edge2).Should().BeFalse();
            filteredGraph.ContainsEdge(edge3).Should().BeFalse();
            filteredGraph.ContainsEdge(edge4).Should().BeFalse();
            filteredGraph.ContainsEdge(otherEdge1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(edge2);
            filteredGraph.ContainsEdge(edge1).Should().BeTrue();
            filteredGraph.ContainsEdge(edge2).Should().BeTrue();
            filteredGraph.ContainsEdge(edge3).Should().BeFalse();
            filteredGraph.ContainsEdge(edge4).Should().BeFalse();
            filteredGraph.ContainsEdge(otherEdge1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(edge3);
            filteredGraph.ContainsEdge(edge1).Should().BeTrue();
            filteredGraph.ContainsEdge(edge2).Should().BeTrue();
            filteredGraph.ContainsEdge(edge3).Should().BeTrue();
            filteredGraph.ContainsEdge(edge4).Should().BeFalse();
            filteredGraph.ContainsEdge(otherEdge1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(edge4);
            filteredGraph.ContainsEdge(edge1).Should().BeTrue();
            filteredGraph.ContainsEdge(edge2).Should().BeTrue();
            filteredGraph.ContainsEdge(edge3).Should().BeTrue();
            filteredGraph.ContainsEdge(edge4).Should().BeFalse();   // Filtered
            filteredGraph.ContainsEdge(otherEdge1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(otherEdge1);
            filteredGraph.ContainsEdge(edge1).Should().BeTrue();
            filteredGraph.ContainsEdge(edge2).Should().BeTrue();
            filteredGraph.ContainsEdge(edge3).Should().BeTrue();
            filteredGraph.ContainsEdge(edge4).Should().BeFalse();  // Filtered
            filteredGraph.ContainsEdge(otherEdge1).Should().BeTrue();

            // Both vertices not in graph
            filteredGraph.ContainsEdge(new Edge<int>(0, 10)).Should().BeFalse();
            // Source not in graph
            filteredGraph.ContainsEdge(new Edge<int>(0, 1)).Should().BeFalse();
            // Target not in graph
            filteredGraph.ContainsEdge(new Edge<int>(1, 0)).Should().BeFalse();

            #endregion

            #region Part 4

            wrappedGraph.Clear();
            filteredGraph = createFilteredGraph(
                vertex => vertex > 0 && vertex < 3,
                edge => edge.Source != edge.Target);

            filteredGraph.ContainsEdge(edge1).Should().BeFalse();
            filteredGraph.ContainsEdge(edge2).Should().BeFalse();
            filteredGraph.ContainsEdge(edge3).Should().BeFalse();
            filteredGraph.ContainsEdge(edge4).Should().BeFalse();
            filteredGraph.ContainsEdge(otherEdge1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(edge1);
            filteredGraph.ContainsEdge(edge1).Should().BeTrue();
            filteredGraph.ContainsEdge(edge2).Should().BeFalse();
            filteredGraph.ContainsEdge(edge3).Should().BeFalse();
            filteredGraph.ContainsEdge(edge4).Should().BeFalse();
            filteredGraph.ContainsEdge(otherEdge1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(edge2);
            filteredGraph.ContainsEdge(edge1).Should().BeTrue();
            filteredGraph.ContainsEdge(edge2).Should().BeFalse();  // Filtered
            filteredGraph.ContainsEdge(edge3).Should().BeFalse();
            filteredGraph.ContainsEdge(edge4).Should().BeFalse();
            filteredGraph.ContainsEdge(otherEdge1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(edge3);
            filteredGraph.ContainsEdge(edge1).Should().BeTrue();
            filteredGraph.ContainsEdge(edge2).Should().BeFalse();  // Filtered
            filteredGraph.ContainsEdge(edge3).Should().BeTrue();
            filteredGraph.ContainsEdge(edge4).Should().BeFalse();
            filteredGraph.ContainsEdge(otherEdge1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(edge4);
            filteredGraph.ContainsEdge(edge1).Should().BeTrue();
            filteredGraph.ContainsEdge(edge2).Should().BeFalse();  // Filtered
            filteredGraph.ContainsEdge(edge3).Should().BeTrue();
            filteredGraph.ContainsEdge(edge4).Should().BeFalse();  // Filtered
            filteredGraph.ContainsEdge(otherEdge1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(otherEdge1);
            filteredGraph.ContainsEdge(edge1).Should().BeTrue();
            filteredGraph.ContainsEdge(edge2).Should().BeFalse();  // Filtered
            filteredGraph.ContainsEdge(edge3).Should().BeTrue();
            filteredGraph.ContainsEdge(edge4).Should().BeFalse();  // Filtered
            filteredGraph.ContainsEdge(otherEdge1).Should().BeTrue();

            // Both vertices not in graph
            filteredGraph.ContainsEdge(new Edge<int>(0, 10)).Should().BeFalse();
            // Source not in graph
            filteredGraph.ContainsEdge(new Edge<int>(0, 1)).Should().BeFalse();
            // Target not in graph
            filteredGraph.ContainsEdge(new Edge<int>(1, 0)).Should().BeFalse();

            #endregion
        }

        protected static void ContainsEdge_EquatableEdge_Test<TGraph>(
            TGraph wrappedGraph,
            Func<VertexPredicate<int>, EdgePredicate<int, EquatableEdge<int>>, IEdgeSet<int, EquatableEdge<int>>> createFilteredGraph)
            where TGraph : IMutableVertexAndEdgeSet<int, EquatableEdge<int>>, IMutableGraph<int, EquatableEdge<int>>
        {
            #region Part 1

            IEdgeSet<int, EquatableEdge<int>> filteredGraph = createFilteredGraph(
                _ => true,
                _ => true);

            ContainsEdge_EquatableEdge_Test(
                filteredGraph,
                edge => wrappedGraph.AddVerticesAndEdge(edge));

            #endregion

            #region Part 2

            wrappedGraph.Clear();
            filteredGraph = createFilteredGraph(
                vertex => vertex > 0 && vertex < 3,
                _ => true);
            var edge1 = new EquatableEdge<int>(1, 2);
            var edge2 = new EquatableEdge<int>(1, 3);
            var edge3 = new EquatableEdge<int>(2, 1);
            var edge4 = new EquatableEdge<int>(2, 2);
            var otherEdge1 = new EquatableEdge<int>(1, 2);

            filteredGraph.ContainsEdge(edge1).Should().BeFalse();
            filteredGraph.ContainsEdge(edge2).Should().BeFalse();
            filteredGraph.ContainsEdge(edge3).Should().BeFalse();
            filteredGraph.ContainsEdge(edge4).Should().BeFalse();
            filteredGraph.ContainsEdge(otherEdge1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(edge1);
            filteredGraph.ContainsEdge(edge1).Should().BeTrue();
            filteredGraph.ContainsEdge(edge2).Should().BeFalse();
            filteredGraph.ContainsEdge(edge3).Should().BeFalse();
            filteredGraph.ContainsEdge(edge4).Should().BeFalse();
            filteredGraph.ContainsEdge(otherEdge1).Should().BeTrue();

            wrappedGraph.AddVerticesAndEdge(edge2);
            filteredGraph.ContainsEdge(edge1).Should().BeTrue();
            filteredGraph.ContainsEdge(edge2).Should().BeFalse();  // Filtered
            filteredGraph.ContainsEdge(edge3).Should().BeFalse();
            filteredGraph.ContainsEdge(edge4).Should().BeFalse();
            filteredGraph.ContainsEdge(otherEdge1).Should().BeTrue();

            wrappedGraph.AddVerticesAndEdge(edge3);
            filteredGraph.ContainsEdge(edge1).Should().BeTrue();
            filteredGraph.ContainsEdge(edge2).Should().BeFalse();  // Filtered
            filteredGraph.ContainsEdge(edge3).Should().BeTrue();
            filteredGraph.ContainsEdge(edge4).Should().BeFalse();
            filteredGraph.ContainsEdge(otherEdge1).Should().BeTrue();

            wrappedGraph.AddVerticesAndEdge(edge4);
            filteredGraph.ContainsEdge(edge1).Should().BeTrue();
            filteredGraph.ContainsEdge(edge2).Should().BeFalse();  // Filtered
            filteredGraph.ContainsEdge(edge3).Should().BeTrue();
            filteredGraph.ContainsEdge(edge4).Should().BeTrue();
            filteredGraph.ContainsEdge(otherEdge1).Should().BeTrue();

            wrappedGraph.AddVerticesAndEdge(otherEdge1);
            filteredGraph.ContainsEdge(edge1).Should().BeTrue();
            filteredGraph.ContainsEdge(edge2).Should().BeFalse();  // Filtered
            filteredGraph.ContainsEdge(edge3).Should().BeTrue();
            filteredGraph.ContainsEdge(edge4).Should().BeTrue();
            filteredGraph.ContainsEdge(otherEdge1).Should().BeTrue();

            // Both vertices not in graph
            filteredGraph.ContainsEdge(new EquatableEdge<int>(0, 10)).Should().BeFalse();
            // Source not in graph
            filteredGraph.ContainsEdge(new EquatableEdge<int>(0, 1)).Should().BeFalse();
            // Target not in graph
            filteredGraph.ContainsEdge(new EquatableEdge<int>(1, 0)).Should().BeFalse();

            #endregion

            #region Part 3

            wrappedGraph.Clear();
            filteredGraph = createFilteredGraph(
                _ => true,
                edge => edge.Source != edge.Target);

            filteredGraph.ContainsEdge(edge1).Should().BeFalse();
            filteredGraph.ContainsEdge(edge2).Should().BeFalse();
            filteredGraph.ContainsEdge(edge3).Should().BeFalse();
            filteredGraph.ContainsEdge(edge4).Should().BeFalse();
            filteredGraph.ContainsEdge(otherEdge1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(edge1);
            filteredGraph.ContainsEdge(edge1).Should().BeTrue();
            filteredGraph.ContainsEdge(edge2).Should().BeFalse();
            filteredGraph.ContainsEdge(edge3).Should().BeFalse();
            filteredGraph.ContainsEdge(edge4).Should().BeFalse();
            filteredGraph.ContainsEdge(otherEdge1).Should().BeTrue();

            wrappedGraph.AddVerticesAndEdge(edge2);
            filteredGraph.ContainsEdge(edge1).Should().BeTrue();
            filteredGraph.ContainsEdge(edge2).Should().BeTrue();
            filteredGraph.ContainsEdge(edge3).Should().BeFalse();
            filteredGraph.ContainsEdge(edge4).Should().BeFalse();
            filteredGraph.ContainsEdge(otherEdge1).Should().BeTrue();

            wrappedGraph.AddVerticesAndEdge(edge3);
            filteredGraph.ContainsEdge(edge1).Should().BeTrue();
            filteredGraph.ContainsEdge(edge2).Should().BeTrue();
            filteredGraph.ContainsEdge(edge3).Should().BeTrue();
            filteredGraph.ContainsEdge(edge4).Should().BeFalse();
            filteredGraph.ContainsEdge(otherEdge1).Should().BeTrue();

            wrappedGraph.AddVerticesAndEdge(edge4);
            filteredGraph.ContainsEdge(edge1).Should().BeTrue();
            filteredGraph.ContainsEdge(edge2).Should().BeTrue();
            filteredGraph.ContainsEdge(edge3).Should().BeTrue();
            filteredGraph.ContainsEdge(edge4).Should().BeFalse();   // Filtered
            filteredGraph.ContainsEdge(otherEdge1).Should().BeTrue();

            wrappedGraph.AddVerticesAndEdge(otherEdge1);
            filteredGraph.ContainsEdge(edge1).Should().BeTrue();
            filteredGraph.ContainsEdge(edge2).Should().BeTrue();
            filteredGraph.ContainsEdge(edge3).Should().BeTrue();
            filteredGraph.ContainsEdge(edge4).Should().BeFalse();  // Filtered
            filteredGraph.ContainsEdge(otherEdge1).Should().BeTrue();

            // Both vertices not in graph
            filteredGraph.ContainsEdge(new EquatableEdge<int>(0, 10)).Should().BeFalse();
            // Source not in graph
            filteredGraph.ContainsEdge(new EquatableEdge<int>(0, 1)).Should().BeFalse();
            // Target not in graph
            filteredGraph.ContainsEdge(new EquatableEdge<int>(1, 0)).Should().BeFalse();

            #endregion

            #region Part 4

            wrappedGraph.Clear();
            filteredGraph = createFilteredGraph(
                vertex => vertex > 0 && vertex < 3,
                edge => edge.Source != edge.Target);

            filteredGraph.ContainsEdge(edge1).Should().BeFalse();
            filteredGraph.ContainsEdge(edge2).Should().BeFalse();
            filteredGraph.ContainsEdge(edge3).Should().BeFalse();
            filteredGraph.ContainsEdge(edge4).Should().BeFalse();
            filteredGraph.ContainsEdge(otherEdge1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(edge1);
            filteredGraph.ContainsEdge(edge1).Should().BeTrue();
            filteredGraph.ContainsEdge(edge2).Should().BeFalse();
            filteredGraph.ContainsEdge(edge3).Should().BeFalse();
            filteredGraph.ContainsEdge(edge4).Should().BeFalse();
            filteredGraph.ContainsEdge(otherEdge1).Should().BeTrue();

            wrappedGraph.AddVerticesAndEdge(edge2);
            filteredGraph.ContainsEdge(edge1).Should().BeTrue();
            filteredGraph.ContainsEdge(edge2).Should().BeFalse();  // Filtered
            filteredGraph.ContainsEdge(edge3).Should().BeFalse();
            filteredGraph.ContainsEdge(edge4).Should().BeFalse();
            filteredGraph.ContainsEdge(otherEdge1).Should().BeTrue();

            wrappedGraph.AddVerticesAndEdge(edge3);
            filteredGraph.ContainsEdge(edge1).Should().BeTrue();
            filteredGraph.ContainsEdge(edge2).Should().BeFalse();  // Filtered
            filteredGraph.ContainsEdge(edge3).Should().BeTrue();
            filteredGraph.ContainsEdge(edge4).Should().BeFalse();
            filteredGraph.ContainsEdge(otherEdge1).Should().BeTrue();

            wrappedGraph.AddVerticesAndEdge(edge4);
            filteredGraph.ContainsEdge(edge1).Should().BeTrue();
            filteredGraph.ContainsEdge(edge2).Should().BeFalse();  // Filtered
            filteredGraph.ContainsEdge(edge3).Should().BeTrue();
            filteredGraph.ContainsEdge(edge4).Should().BeFalse();  // Filtered
            filteredGraph.ContainsEdge(otherEdge1).Should().BeTrue();

            wrappedGraph.AddVerticesAndEdge(otherEdge1);
            filteredGraph.ContainsEdge(edge1).Should().BeTrue();
            filteredGraph.ContainsEdge(edge2).Should().BeFalse();  // Filtered
            filteredGraph.ContainsEdge(edge3).Should().BeTrue();
            filteredGraph.ContainsEdge(edge4).Should().BeFalse();  // Filtered
            filteredGraph.ContainsEdge(otherEdge1).Should().BeTrue();

            // Both vertices not in graph
            filteredGraph.ContainsEdge(new EquatableEdge<int>(0, 10)).Should().BeFalse();
            // Source not in graph
            filteredGraph.ContainsEdge(new EquatableEdge<int>(0, 1)).Should().BeFalse();
            // Target not in graph
            filteredGraph.ContainsEdge(new EquatableEdge<int>(1, 0)).Should().BeFalse();

            #endregion
        }

        protected static void ContainsEdge_SourceTarget_Test<TGraph>(
            TGraph wrappedGraph,
            Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IIncidenceGraph<int, Edge<int>>> createFilteredGraph)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>, IMutableGraph<int, Edge<int>>
        {
            #region Part 1

            ContainsEdge_SourceTarget_ImmutableGraph_Test(
                wrappedGraph,
                () => createFilteredGraph(_ => true, _ => true));

            #endregion

            #region Part 2

            wrappedGraph.Clear();
            IIncidenceGraph<int, Edge<int>> filteredGraph = createFilteredGraph(
                vertex => vertex > 0 && vertex < 3,
                _ => true);

            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 2);

            filteredGraph.ContainsEdge(1, 2).Should().BeFalse();
            filteredGraph.ContainsEdge(2, 1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(edge1);
            filteredGraph.ContainsEdge(1, 2).Should().BeTrue();
            filteredGraph.ContainsEdge(2, 1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(edge2);
            filteredGraph.ContainsEdge(1, 3).Should().BeFalse();   // Filtered
            filteredGraph.ContainsEdge(3, 1).Should().BeFalse();   // Filtered

            wrappedGraph.AddVerticesAndEdge(edge3);
            filteredGraph.ContainsEdge(2, 2).Should().BeTrue();

            // Vertices is not present in the graph
            filteredGraph.ContainsEdge(0, 4).Should().BeFalse();
            filteredGraph.ContainsEdge(1, 4).Should().BeFalse();
            filteredGraph.ContainsEdge(4, 1).Should().BeFalse();

            #endregion

            #region Part 3

            wrappedGraph.Clear();
            filteredGraph = createFilteredGraph(
                _ => true,
                edge => edge.Source != edge.Target);

            filteredGraph.ContainsEdge(1, 2).Should().BeFalse();
            filteredGraph.ContainsEdge(2, 1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(edge1);
            filteredGraph.ContainsEdge(1, 2).Should().BeTrue();
            filteredGraph.ContainsEdge(2, 1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(edge2);
            filteredGraph.ContainsEdge(1, 3).Should().BeTrue();
            filteredGraph.ContainsEdge(3, 1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(edge3);
            filteredGraph.ContainsEdge(2, 2).Should().BeFalse();   // Filtered

            // Vertices is not present in the graph
            filteredGraph.ContainsEdge(0, 4).Should().BeFalse();
            filteredGraph.ContainsEdge(1, 4).Should().BeFalse();
            filteredGraph.ContainsEdge(4, 1).Should().BeFalse();

            #endregion

            #region Part 4

            wrappedGraph.Clear();
            filteredGraph = createFilteredGraph(
                vertex => vertex > 0 && vertex < 3,
                edge => edge.Source != edge.Target);

            filteredGraph.ContainsEdge(1, 2).Should().BeFalse();
            filteredGraph.ContainsEdge(2, 1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(edge1);
            filteredGraph.ContainsEdge(1, 2).Should().BeTrue();
            filteredGraph.ContainsEdge(2, 1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(edge2);
            filteredGraph.ContainsEdge(1, 3).Should().BeFalse();   // Filtered
            filteredGraph.ContainsEdge(3, 1).Should().BeFalse();   // Filtered

            wrappedGraph.AddVerticesAndEdge(edge3);
            filteredGraph.ContainsEdge(2, 2).Should().BeFalse();   // Filtered

            // Vertices is not present in the graph
            filteredGraph.ContainsEdge(0, 4).Should().BeFalse();
            filteredGraph.ContainsEdge(1, 4).Should().BeFalse();
            filteredGraph.ContainsEdge(4, 1).Should().BeFalse();

            #endregion
        }

        protected static void ContainsEdge_SourceTarget_UndirectedGraph_Test<TGraph>(
            TGraph wrappedGraph,
            Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IImplicitUndirectedGraph<int, Edge<int>>> createFilteredGraph)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>, IMutableGraph<int, Edge<int>>
        {
            #region Part 1

            ContainsEdge_SourceTarget_ImmutableGraph_UndirectedGraph_Test(
                wrappedGraph,
                () => createFilteredGraph(_ => true, _ => true));

            #endregion

            #region Part 2

            wrappedGraph.Clear();
            IImplicitUndirectedGraph<int, Edge<int>> filteredGraph = createFilteredGraph(
                vertex => vertex > 0 && vertex < 3,
                _ => true);

            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 2);

            filteredGraph.ContainsEdge(1, 2).Should().BeFalse();
            filteredGraph.ContainsEdge(2, 1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(edge1);
            filteredGraph.ContainsEdge(1, 2).Should().BeTrue();
            filteredGraph.ContainsEdge(2, 1).Should().BeTrue();

            wrappedGraph.AddVerticesAndEdge(edge2);
            filteredGraph.ContainsEdge(1, 3).Should().BeFalse();   // Filtered
            filteredGraph.ContainsEdge(3, 1).Should().BeFalse();   // Filtered

            wrappedGraph.AddVerticesAndEdge(edge3);
            filteredGraph.ContainsEdge(2, 2).Should().BeTrue();

            // Vertices is not present in the graph
            filteredGraph.ContainsEdge(0, 4).Should().BeFalse();
            filteredGraph.ContainsEdge(1, 4).Should().BeFalse();
            filteredGraph.ContainsEdge(4, 1).Should().BeFalse();

            #endregion

            #region Part 3

            wrappedGraph.Clear();
            filteredGraph = createFilteredGraph(
                _ => true,
                edge => edge.Source != edge.Target);

            filteredGraph.ContainsEdge(1, 2).Should().BeFalse();
            filteredGraph.ContainsEdge(2, 1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(edge1);
            filteredGraph.ContainsEdge(1, 2).Should().BeTrue();
            filteredGraph.ContainsEdge(2, 1).Should().BeTrue();

            wrappedGraph.AddVerticesAndEdge(edge2);
            filteredGraph.ContainsEdge(1, 3).Should().BeTrue();
            filteredGraph.ContainsEdge(3, 1).Should().BeTrue();

            wrappedGraph.AddVerticesAndEdge(edge3);
            filteredGraph.ContainsEdge(2, 2).Should().BeFalse();   // Filtered

            // Vertices is not present in the graph
            filteredGraph.ContainsEdge(0, 4).Should().BeFalse();
            filteredGraph.ContainsEdge(1, 4).Should().BeFalse();
            filteredGraph.ContainsEdge(4, 1).Should().BeFalse();

            #endregion

            #region Part 4

            wrappedGraph.Clear();
            filteredGraph = createFilteredGraph(
                vertex => vertex > 0 && vertex < 3,
                edge => edge.Source != edge.Target);

            filteredGraph.ContainsEdge(1, 2).Should().BeFalse();
            filteredGraph.ContainsEdge(2, 1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(edge1);
            filteredGraph.ContainsEdge(1, 2).Should().BeTrue();
            filteredGraph.ContainsEdge(2, 1).Should().BeTrue();

            wrappedGraph.AddVerticesAndEdge(edge2);
            filteredGraph.ContainsEdge(1, 3).Should().BeFalse();   // Filtered
            filteredGraph.ContainsEdge(3, 1).Should().BeFalse();   // Filtered

            wrappedGraph.AddVerticesAndEdge(edge3);
            filteredGraph.ContainsEdge(2, 2).Should().BeFalse();   // Filtered

            // Vertices is not present in the graph
            filteredGraph.ContainsEdge(0, 4).Should().BeFalse();
            filteredGraph.ContainsEdge(1, 4).Should().BeFalse();
            filteredGraph.ContainsEdge(4, 1).Should().BeFalse();

            #endregion
        }

        #endregion

        #region Out Edges

        protected static void OutEdge_Test<TGraph>(
            TGraph wrappedGraph,
            Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IImplicitGraph<int, Edge<int>>> createFilteredGraph)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>, IMutableGraph<int, Edge<int>>
        {
            #region Part 1

            OutEdge_ImmutableGraph_Test(
                wrappedGraph,
                () => createFilteredGraph(_ => true, _ => true));

            #endregion

            #region Part 2

            wrappedGraph.Clear();
            var edge11 = new Edge<int>(1, 1);
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge24 = new Edge<int>(2, 4);
            var edge33 = new Edge<int>(3, 3);
            var edge34 = new Edge<int>(3, 4);
            var edge41 = new Edge<int>(4, 1);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge11, edge12, edge13, edge24, edge33, edge34, edge41 });
            IImplicitGraph<int, Edge<int>> filteredGraph = createFilteredGraph(
                vertex => vertex < 4,
                _ => true);

            filteredGraph.OutEdge(1, 0).Should().BeSameAs(edge11);
            filteredGraph.OutEdge(1, 2).Should().BeSameAs(edge13);
            filteredGraph.OutEdge(3, 0).Should().BeSameAs(edge33);
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Invoking(() => filteredGraph.OutEdge(4, 0)).Should().Throw<VertexNotFoundException>(); // Filtered

            #endregion

            #region Part 3

            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge11, edge12, edge13, edge24, edge33, edge34, edge41 });
            filteredGraph = createFilteredGraph(
                _ => true,
                edge => edge.Source != edge.Target);

            filteredGraph.OutEdge(1, 0).Should().BeSameAs(edge12);
            filteredGraph.OutEdge(1, 1).Should().BeSameAs(edge13);
            filteredGraph.OutEdge(3, 0).Should().BeSameAs(edge34);
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            AssertIndexOutOfRange(() => filteredGraph.OutEdge(3, 1));  // Filtered

            #endregion

            #region Part 4

            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge11, edge12, edge13, edge24, edge33, edge34, edge41 });
            filteredGraph = createFilteredGraph(
                vertex => vertex < 4,
                edge => edge.Source != edge.Target);

            filteredGraph.OutEdge(1, 0).Should().BeSameAs(edge12);
            filteredGraph.OutEdge(1, 1).Should().BeSameAs(edge13);
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            AssertIndexOutOfRange(() => filteredGraph.OutEdge(3, 0));  // Filtered
            Invoking(() => filteredGraph.OutEdge(4, 1)).Should().Throw<VertexNotFoundException>(); // Filtered
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            #endregion
        }

        protected static void OutEdge_Throws_Test<TGraph>(
            TGraph wrappedGraph,
            Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IImplicitGraph<int, Edge<int>>> createFilteredGraph)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>, IMutableGraph<int, Edge<int>>
        {
            #region Part 1

            OutEdge_Throws_ImmutableGraph_Test(
                wrappedGraph,
                () => createFilteredGraph(_ => true, _ => true));

            #endregion

            #region Part 2

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            const int vertex1 = 1;
            const int vertex2 = 2;
            const int vertex3 = 3;

            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(vertex1, vertex1),
                new Edge<int>(vertex1, vertex2),
                new Edge<int>(vertex1, vertex3),
                new Edge<int>(vertex2, vertex3),
                new Edge<int>(vertex3, vertex1)
            });
            IImplicitGraph<int, Edge<int>> filteredGraph = createFilteredGraph(
                vertex => vertex < 3,
                _ => true);

            AssertIndexOutOfRange(() => filteredGraph.OutEdge(vertex1, 2));
            Invoking(() => filteredGraph.OutEdge(vertex3, 0)).Should().Throw<VertexNotFoundException>();
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            #endregion

            #region Part 3

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            const int vertex4 = 4;

            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(vertex1, vertex1),
                new Edge<int>(vertex1, vertex2),
                new Edge<int>(vertex1, vertex3),
                new Edge<int>(vertex2, vertex3),
                new Edge<int>(vertex3, vertex1)
            });
            filteredGraph = createFilteredGraph(
                _ => true,
                edge => edge.Source != 1);

            AssertIndexOutOfRange(() => filteredGraph.OutEdge(vertex1, 0));
            Invoking(() => filteredGraph.OutEdge(vertex4, 0)).Should().Throw<VertexNotFoundException>();
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            #endregion

            #region Part 4

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(vertex1, vertex1),
                new Edge<int>(vertex1, vertex2),
                new Edge<int>(vertex1, vertex3),
                new Edge<int>(vertex2, vertex2),
                new Edge<int>(vertex2, vertex3),
                new Edge<int>(vertex3, vertex1)
            });
            filteredGraph = createFilteredGraph(
                vertex => vertex < 3,
                edge => edge.Source != 1);

            AssertIndexOutOfRange(() => filteredGraph.OutEdge(vertex1, 0));
            AssertIndexOutOfRange(() => filteredGraph.OutEdge(vertex2, 1));
            Invoking(() => filteredGraph.OutEdge(vertex4, 0)).Should().Throw<VertexNotFoundException>();
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            #endregion
        }

        protected static void OutEdges_Test<TGraph>(
            TGraph wrappedGraph,
            Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IImplicitGraph<int, Edge<int>>> createFilteredGraph)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>, IMutableGraph<int, Edge<int>>
        {
            #region Part 1

            OutEdges_ImmutableGraph_Test(
                wrappedGraph,
                () => createFilteredGraph(_ => true, _ => true));

            #endregion

            #region Part 2

            wrappedGraph.Clear();
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge15 = new Edge<int>(1, 5);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);

            IImplicitGraph<int, Edge<int>> filteredGraph = createFilteredGraph(
                vertex => vertex < 4,
                _ => true);

            wrappedGraph.AddVertex(1);
            AssertNoOutEdge(filteredGraph, 1);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge14, edge15, edge24, edge31, edge33 });

            AssertHasOutEdges(filteredGraph, 1, new[] { edge12, edge13 });  // Filtered
            AssertNoOutEdge(filteredGraph, 2);                                   // Filtered
            AssertHasOutEdges(filteredGraph, 3, new[] { edge31, edge33 });

            #endregion

            #region Part 3

            wrappedGraph.Clear();
            filteredGraph = createFilteredGraph(
                _ => true,
                edge => edge.Source != edge.Target);

            wrappedGraph.AddVertex(1);
            AssertNoOutEdge(filteredGraph, 1);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge14, edge15, edge24, edge31, edge33 });

            AssertHasOutEdges(filteredGraph, 1, new[] { edge12, edge13, edge14, edge15 });
            AssertHasOutEdges(filteredGraph, 2, new[] { edge24 });
            AssertHasOutEdges(filteredGraph, 3, new[] { edge31 });  // Filtered

            #endregion

            #region Part 4

            wrappedGraph.Clear();
            filteredGraph = createFilteredGraph(
                vertex => vertex < 4,
                edge => edge.Source != edge.Target);

            wrappedGraph.AddVertex(1);
            AssertNoOutEdge(filteredGraph, 1);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge14, edge15, edge24, edge31, edge33 });

            AssertHasOutEdges(filteredGraph, 1, new[] { edge12, edge13 });  // Filtered
            AssertNoOutEdge(filteredGraph, 2);                                   // Filtered
            AssertHasOutEdges(filteredGraph, 3, new[] { edge31 });          // Filtered

            #endregion
        }

        #endregion

        #region In Edges

        protected static void InEdge_Test<TGraph>(
            TGraph wrappedGraph,
            Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IBidirectionalIncidenceGraph<int, Edge<int>>> createFilteredGraph)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>, IMutableGraph<int, Edge<int>>
        {
            #region Part 1

            InEdge_ImmutableGraph_Test(
                wrappedGraph,
                () => createFilteredGraph(_ => true, _ => true));

            #endregion

            #region Part 2

            wrappedGraph.Clear();
            var edge11 = new Edge<int>(1, 1);
            var edge13 = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge21 = new Edge<int>(2, 1);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge11, edge13, edge14, edge21 });

            IBidirectionalIncidenceGraph<int, Edge<int>> filteredGraph = createFilteredGraph(
                vertex => vertex < 4,
                _ => true);

            filteredGraph.InEdge(1, 0).Should().BeSameAs(edge11);
            filteredGraph.InEdge(1, 1).Should().BeSameAs(edge21);
            filteredGraph.InEdge(3, 0).Should().BeSameAs(edge13);
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            AssertIndexOutOfRange(() => filteredGraph.InEdge(1, 2));    // Filtered
            Invoking(() => filteredGraph.InEdge(4, 0)).Should().Throw<VertexNotFoundException>(); // Filtered
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            #endregion

            #region Part 3

            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge11, edge13, edge14, edge21 });
            filteredGraph = createFilteredGraph(
                _ => true,
                edge => edge.Source != edge.Target);

            filteredGraph.InEdge(1, 0).Should().BeSameAs(edge21);    // Filtered
            filteredGraph.InEdge(3, 0).Should().BeSameAs(edge13);
            filteredGraph.InEdge(4, 0).Should().BeSameAs(edge14);

            #endregion

            #region Part 4

            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge11, edge13, edge14, edge21 });
            filteredGraph = createFilteredGraph(
                vertex => vertex < 4,
                edge => edge.Source != edge.Target);

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            filteredGraph.InEdge(1, 0).Should().BeSameAs(edge21); // Filtered
            filteredGraph.InEdge(3, 0).Should().BeSameAs(edge13);
            AssertIndexOutOfRange(() => filteredGraph.InEdge(1, 2));    // Filtered
            Invoking(() => filteredGraph.InEdge(4, 0)).Should().Throw<VertexNotFoundException>(); // Filtered
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            #endregion
        }

        protected static void InEdge_Throws_Test<TGraph>(
            TGraph wrappedGraph,
            Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IBidirectionalIncidenceGraph<int, Edge<int>>> createFilteredGraph)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>, IMutableGraph<int, Edge<int>>
        {
            #region Part 1

            InEdge_Throws_ImmutableGraph_Test(
                wrappedGraph,
                () => createFilteredGraph(_ => true, _ => true));

            #endregion

            #region Part 2

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            const int vertex1 = 1;
            const int vertex2 = 2;
            const int vertex3 = 3;

            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(vertex1, vertex1),
                new Edge<int>(vertex1, vertex3),
                new Edge<int>(vertex2, vertex1),
                new Edge<int>(vertex3, vertex1),
                new Edge<int>(vertex3, vertex2)
            });
            IBidirectionalIncidenceGraph<int, Edge<int>> filteredGraph = createFilteredGraph(
                vertex => vertex < 3,
                _ => true);

            AssertIndexOutOfRange(() => filteredGraph.InEdge(vertex1, 2));
            Invoking(() => filteredGraph.InEdge(vertex3, 0)).Should().Throw<VertexNotFoundException>();
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            #endregion

            #region Part 3

            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(vertex1, vertex1),
                new Edge<int>(vertex1, vertex3),
                new Edge<int>(vertex2, vertex1),
                new Edge<int>(vertex3, vertex1),
                new Edge<int>(vertex3, vertex2)
            });
            filteredGraph = createFilteredGraph(
                _ => true,
                edge => edge.Source != edge.Target);

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            AssertIndexOutOfRange(() => filteredGraph.InEdge(vertex1, 2));

            #endregion

            #region Part 4

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(vertex1, vertex1),
                new Edge<int>(vertex1, vertex3),
                new Edge<int>(vertex2, vertex1),
                new Edge<int>(vertex3, vertex1),
                new Edge<int>(vertex3, vertex2)
            });
            filteredGraph = createFilteredGraph(
                vertex => vertex < 3,
                edge => edge.Source != edge.Target);

            AssertIndexOutOfRange(() => filteredGraph.InEdge(vertex1, 1));
            Invoking(() => filteredGraph.InEdge(vertex3, 0)).Should().Throw<VertexNotFoundException>();
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            #endregion
        }

        protected static void InEdges_Test<TGraph>(
            TGraph wrappedGraph,
            Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IBidirectionalIncidenceGraph<int, Edge<int>>> createFilteredGraph)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>, IMutableGraph<int, Edge<int>>
        {
            #region Part 1

            InEdges_ImmutableGraph_Test(
                wrappedGraph,
                () => createFilteredGraph(_ => true, _ => true));

            #endregion

            #region Part 2

            wrappedGraph.Clear();
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge32 = new Edge<int>(3, 2);
            var edge33 = new Edge<int>(3, 3);

            IBidirectionalIncidenceGraph<int, Edge<int>> filteredGraph = createFilteredGraph(
                vertex => vertex < 4,
                _ => true);

            wrappedGraph.AddVertex(1);
            AssertNoInEdge(filteredGraph, 1);
            AssertNoOutEdge(filteredGraph, 1);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge14, edge24, edge32, edge33 });

            AssertHasOutEdges(filteredGraph, 1, new[] { edge12, edge13 });  // Filtered
            AssertNoOutEdge(filteredGraph, 2);  // Filtered
            AssertHasOutEdges(filteredGraph, 3, new[] { edge32, edge33 });

            AssertNoInEdge(filteredGraph, 1);
            AssertHasInEdges(filteredGraph, 2, new[] { edge12, edge32 });
            AssertHasInEdges(filteredGraph, 3, new[] { edge13, edge33 });

            #endregion

            #region Part 3

            wrappedGraph.Clear();
            filteredGraph = createFilteredGraph(
                _ => true,
                edge => edge.Source != edge.Target);

            wrappedGraph.AddVertex(1);
            AssertNoInEdge(filteredGraph, 1);
            AssertNoOutEdge(filteredGraph, 1);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge14, edge24, edge32, edge33 });

            AssertHasOutEdges(filteredGraph, 1, new[] { edge12, edge13, edge14 });
            AssertHasOutEdges(filteredGraph, 2, new[] { edge24 });
            AssertHasOutEdges(filteredGraph, 3, new[] { edge32 });  // Filtered

            AssertNoInEdge(filteredGraph, 1);
            AssertHasInEdges(filteredGraph, 2, new[] { edge12, edge32 });
            AssertHasInEdges(filteredGraph, 3, new[] { edge13 });   // Filtered

            #endregion

            #region Part 4

            wrappedGraph.Clear();
            filteredGraph = createFilteredGraph(
                vertex => vertex < 4,
                edge => edge.Source != edge.Target);

            wrappedGraph.AddVertex(1);
            AssertNoInEdge(filteredGraph, 1);
            AssertNoOutEdge(filteredGraph, 1);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge14, edge24, edge32, edge33 });

            AssertHasOutEdges(filteredGraph, 1, new[] { edge12, edge13 });  // Filtered
            AssertNoOutEdge(filteredGraph, 2);  // Filtered
            AssertHasOutEdges(filteredGraph, 3, new[] { edge32 });  // Filtered

            AssertNoInEdge(filteredGraph, 1);
            AssertHasInEdges(filteredGraph, 2, new[] { edge12, edge32 });
            AssertHasInEdges(filteredGraph, 3, new[] { edge13 });   // Filtered

            #endregion
        }

        #endregion

        #region Adjacent Edges

        protected static void AdjacentEdge_Test<TGraph>(
            TGraph wrappedGraph,
            Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IImplicitUndirectedGraph<int, Edge<int>>> createFilteredGraph)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>, IMutableGraph<int, Edge<int>>
        {
            #region Part 1

            AdjacentEdge_ImmutableGraph_Test(
                wrappedGraph,
                () => createFilteredGraph(_ => true, _ => true));

            #endregion

            #region Part 2

            wrappedGraph.Clear();
            var edge11 = new Edge<int>(1, 1);
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge24 = new Edge<int>(2, 4);
            var edge33 = new Edge<int>(3, 3);
            var edge41 = new Edge<int>(4, 1);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge11, edge12, edge13, edge24, edge33, edge41 });
            IImplicitUndirectedGraph<int, Edge<int>> filteredGraph = createFilteredGraph(
                vertex => vertex < 4,
                _ => true);

            filteredGraph.AdjacentEdge(1, 0).Should().BeSameAs(edge11);
            filteredGraph.AdjacentEdge(1, 2).Should().BeSameAs(edge13);
            filteredGraph.AdjacentEdge(3, 0).Should().BeSameAs(edge13);
            filteredGraph.AdjacentEdge(3, 1).Should().BeSameAs(edge33);
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Invoking(() => filteredGraph.AdjacentEdge(4, 1)).Should().Throw<VertexNotFoundException>(); // Filtered

            #endregion

            #region Part 3

            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge11, edge12, edge13, edge24, edge33, edge41 });
            filteredGraph = createFilteredGraph(
                _ => true,
                edge => edge.Source != edge.Target);

            filteredGraph.AdjacentEdge(1, 0).Should().BeSameAs(edge12);
            filteredGraph.AdjacentEdge(1, 1).Should().BeSameAs(edge13);
            filteredGraph.AdjacentEdge(3, 0).Should().BeSameAs(edge13);
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            AssertIndexOutOfRange(() => filteredGraph.AdjacentEdge(3, 1));  // Filtered

            #endregion

            #region Part 4

            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge11, edge12, edge13, edge24, edge33, edge41 });
            filteredGraph = createFilteredGraph(
                vertex => vertex < 4,
                edge => edge.Source != edge.Target);

            filteredGraph.AdjacentEdge(1, 0).Should().BeSameAs(edge12);
            filteredGraph.AdjacentEdge(1, 1).Should().BeSameAs(edge13);
            filteredGraph.AdjacentEdge(3, 0).Should().BeSameAs(edge13);
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            AssertIndexOutOfRange(() => filteredGraph.AdjacentEdge(3, 1));  // Filtered
            Invoking(() => filteredGraph.AdjacentEdge(4, 1)).Should().Throw<VertexNotFoundException>(); // Filtered
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            #endregion
        }

        protected static void AdjacentEdge_Throws_Test<TGraph>(
            TGraph wrappedGraph,
            Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IImplicitUndirectedGraph<int, Edge<int>>> createFilteredGraph)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>, IMutableGraph<int, Edge<int>>
        {
            #region Part 1

            AdjacentEdge_Throws_ImmutableGraph_Test(
                wrappedGraph,
                () => createFilteredGraph(_ => true, _ => true));

            #endregion

            #region Part 2

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            const int vertex1 = 1;
            const int vertex2 = 2;
            const int vertex3 = 3;

            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(vertex1, vertex1),
                new Edge<int>(vertex1, vertex2),
                new Edge<int>(vertex1, vertex3),
                new Edge<int>(vertex2, vertex3),
                new Edge<int>(vertex3, vertex1)
            });
            IImplicitUndirectedGraph<int, Edge<int>> filteredGraph = createFilteredGraph(
                vertex => vertex < 3,
                _ => true);

            AssertIndexOutOfRange(() => filteredGraph.AdjacentEdge(vertex1, 2));
            Invoking(() => filteredGraph.AdjacentEdge(vertex3, 0)).Should().Throw<VertexNotFoundException>();
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            #endregion

            #region Part 3

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            const int vertex4 = 4;
            const int vertex5 = 5;

            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(vertex1, vertex1),
                new Edge<int>(vertex1, vertex2),
                new Edge<int>(vertex1, vertex3),
                new Edge<int>(vertex2, vertex3),
                new Edge<int>(vertex3, vertex4)
            });
            filteredGraph = createFilteredGraph(
                _ => true,
                edge => edge.Source != 1);

            AssertIndexOutOfRange(() => filteredGraph.AdjacentEdge(vertex1, 0));
            AssertIndexOutOfRange(() => filteredGraph.AdjacentEdge(vertex2, 1));
            Invoking(() => filteredGraph.AdjacentEdge(vertex5, 0)).Should().Throw<VertexNotFoundException>();
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            #endregion

            #region Part 4

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(vertex1, vertex1),
                new Edge<int>(vertex1, vertex2),
                new Edge<int>(vertex1, vertex3),
                new Edge<int>(vertex2, vertex2),
                new Edge<int>(vertex2, vertex3),
                new Edge<int>(vertex3, vertex1)
            });
            filteredGraph = createFilteredGraph(
                vertex => vertex < 3,
                edge => edge.Source != 1);

            AssertIndexOutOfRange(() => filteredGraph.AdjacentEdge(vertex1, 0));
            AssertIndexOutOfRange(() => filteredGraph.AdjacentEdge(vertex2, 1));
            Invoking(() => filteredGraph.AdjacentEdge(vertex4, 0)).Should().Throw<VertexNotFoundException>();
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            #endregion
        }

        protected static void AdjacentEdges_Test<TGraph>(
            TGraph wrappedGraph,
            Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IImplicitUndirectedGraph<int, Edge<int>>> createFilteredGraph)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>, IMutableGraph<int, Edge<int>>
        {
            #region Part1

            AdjacentEdges_ImmutableGraph_Test(
                wrappedGraph,
                () => createFilteredGraph(_ => true, _ => true));

            #endregion

            #region Part 2

            wrappedGraph.Clear();
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);

            IImplicitUndirectedGraph<int, Edge<int>> filteredGraph = createFilteredGraph(
                vertex => vertex <= 4,
                _ => true);

            wrappedGraph.AddVertex(1);
            AssertNoAdjacentEdge(filteredGraph, 1);

            wrappedGraph.AddVertex(5);
            var edge15 = new Edge<int>(1, 5);
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge14, edge15, edge24, edge31, edge33 });

            AssertHasAdjacentEdges(filteredGraph, 1, new[] { edge12, edge13, edge14, edge31 });
            AssertHasAdjacentEdges(filteredGraph, 2, new[] { edge12, edge24 });
            AssertHasAdjacentEdges(filteredGraph, 3, new[] { edge13, edge31, edge33 }, 4);  // Has self edge counting twice
            AssertHasAdjacentEdges(filteredGraph, 4, new[] { edge14, edge24 });

            #endregion

            #region Part 3

            wrappedGraph.Clear();
            filteredGraph = createFilteredGraph(
                _ => true,
                edge => edge.Source != edge.Target);

            wrappedGraph.AddVertex(1);
            AssertNoAdjacentEdge(filteredGraph, 1);

            wrappedGraph.AddVertex(5);
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge14, edge15, edge24, edge31, edge33 });

            AssertHasAdjacentEdges(filteredGraph, 1, new[] { edge12, edge13, edge14, edge15, edge31 });
            AssertHasAdjacentEdges(filteredGraph, 2, new[] { edge12, edge24 });
            AssertHasAdjacentEdges(filteredGraph, 3, new[] { edge13, edge31 });
            AssertHasAdjacentEdges(filteredGraph, 4, new[] { edge14, edge24 });

            #endregion

            #region Part 4

            wrappedGraph.Clear();
            filteredGraph = createFilteredGraph(
                vertex => vertex <= 4,
                edge => edge.Source != edge.Target);

            wrappedGraph.AddVertex(1);
            AssertNoAdjacentEdge(filteredGraph, 1);

            wrappedGraph.AddVertex(5);
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge14, edge15, edge24, edge31, edge33 });

            AssertHasAdjacentEdges(filteredGraph, 1, new[] { edge12, edge13, edge14, edge31 });
            AssertHasAdjacentEdges(filteredGraph, 2, new[] { edge12, edge24 });
            AssertHasAdjacentEdges(filteredGraph, 3, new[] { edge13, edge31 });
            AssertHasAdjacentEdges(filteredGraph, 4, new[] { edge14, edge24 });

            #endregion
        }

        #endregion

        #region Degree

        protected static void Degree_Test<TGraph>(
            TGraph wrappedGraph,
            Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IBidirectionalIncidenceGraph<int, Edge<int>>> createFilteredGraph)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>, IMutableGraph<int, Edge<int>>
        {
            #region Part 1

            Degree_ImmutableGraph_Test(
                wrappedGraph,
                () => createFilteredGraph(_ => true, _ => true));

            #endregion

            #region Part 2

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            wrappedGraph.Clear();
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(1, 4);
            var edge4 = new Edge<int>(2, 4);
            var edge5 = new Edge<int>(3, 2);
            var edge6 = new Edge<int>(3, 3);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });
            wrappedGraph.AddVertex(5);

            IBidirectionalIncidenceGraph<int, Edge<int>> filteredGraph = createFilteredGraph(
                vertex => vertex < 4,
                _ => true);

            filteredGraph.Degree(1).Should().Be(2);    // Filtered
            filteredGraph.Degree(2).Should().Be(2);    // Filtered
            filteredGraph.Degree(3).Should().Be(4);    // Self edge
            Invoking(() => filteredGraph.Degree(4)).Should().Throw<VertexNotFoundException>();    // Filtered
            Invoking(() => filteredGraph.Degree(5)).Should().Throw<VertexNotFoundException>();    // Filtered
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            #endregion

            #region Part 3

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });
            wrappedGraph.AddVertex(5);

            filteredGraph = createFilteredGraph(
                _ => true,
                edge => edge.Source != edge.Target);

            filteredGraph.Degree(1).Should().Be(3);
            filteredGraph.Degree(2).Should().Be(3);
            filteredGraph.Degree(3).Should().Be(2);    // Filtered
            filteredGraph.Degree(4).Should().Be(2);
            filteredGraph.Degree(5).Should().Be(0);
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            #endregion

            #region Part 4

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });
            wrappedGraph.AddVertex(5);

            filteredGraph = createFilteredGraph(
                vertex => vertex < 4,
                edge => edge.Source != edge.Target);

            filteredGraph.Degree(1).Should().Be(2);    // Filtered
            filteredGraph.Degree(2).Should().Be(2);    // Filtered
            filteredGraph.Degree(3).Should().Be(2);    // Filtered
            Invoking(() => filteredGraph.Degree(4)).Should().Throw<VertexNotFoundException>();    // Filtered
            Invoking(() => filteredGraph.Degree(5)).Should().Throw<VertexNotFoundException>();    // Filtered
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            #endregion
        }

        #endregion

        #region Try Get Edges

        protected static void TryGetEdge_Test<TGraph>(
            TGraph wrappedGraph,
            Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IIncidenceGraph<int, Edge<int>>> createFilteredGraph)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>, IMutableGraph<int, Edge<int>>
        {
            #region Part 1

            TryGetEdge_ImmutableGraph_Test(
                wrappedGraph,
                () => createFilteredGraph(_ => true, _ => true));

            #endregion

            #region Part 2

            wrappedGraph.Clear();
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);
            var edge7 = new Edge<int>(5, 2);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6, edge7 });

            IIncidenceGraph<int, Edge<int>> filteredGraph = createFilteredGraph(
                vertex => vertex <= 4,
                _ => true);

            filteredGraph.TryGetEdge(0, 10, out _).Should().BeFalse();
            filteredGraph.TryGetEdge(0, 1, out _).Should().BeFalse();

            filteredGraph.TryGetEdge(2, 4, out Edge<int>? gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge5);

            filteredGraph.TryGetEdge(2, 2, out gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge4);

            filteredGraph.TryGetEdge(1, 2, out gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge1);

            filteredGraph.TryGetEdge(2, 1, out _).Should().BeFalse();

            filteredGraph.TryGetEdge(5, 2, out _).Should().BeFalse();  // Filtered
            filteredGraph.TryGetEdge(2, 5, out _).Should().BeFalse();  // Filtered

            #endregion

            #region Part 3

            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6, edge7 });

            filteredGraph = createFilteredGraph(
                _ => true,
                edge => edge.Source != edge.Target);

            filteredGraph.TryGetEdge(0, 10, out _).Should().BeFalse();
            filteredGraph.TryGetEdge(0, 1, out _).Should().BeFalse();

            filteredGraph.TryGetEdge(2, 4, out gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge5);

            filteredGraph.TryGetEdge(2, 2, out gotEdge).Should().BeFalse();    // Filtered

            filteredGraph.TryGetEdge(1, 2, out gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge1);

            filteredGraph.TryGetEdge(2, 1, out _).Should().BeFalse();

            filteredGraph.TryGetEdge(5, 2, out gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge7);

            #endregion

            #region Part 4

            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6, edge7 });

            filteredGraph = createFilteredGraph(
                vertex => vertex <= 4,
                edge => edge.Source != edge.Target);

            filteredGraph.TryGetEdge(0, 10, out _).Should().BeFalse();
            filteredGraph.TryGetEdge(0, 1, out _).Should().BeFalse();

            filteredGraph.TryGetEdge(2, 4, out gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge5);

            filteredGraph.TryGetEdge(2, 2, out _).Should().BeFalse();  // Filtered

            filteredGraph.TryGetEdge(1, 2, out gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge1);

            filteredGraph.TryGetEdge(2, 1, out _).Should().BeFalse();

            filteredGraph.TryGetEdge(5, 2, out _).Should().BeFalse();  // Filtered
            filteredGraph.TryGetEdge(2, 5, out _).Should().BeFalse();  // Filtered

            #endregion
        }

        protected static void TryGetEdges_Test<TGraph>(
            TGraph wrappedGraph,
            Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IIncidenceGraph<int, Edge<int>>> createFilteredGraph)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>, IMutableGraph<int, Edge<int>>
        {
            #region Part 1

            TryGetEdges_Test(
                createFilteredGraph(_ => true, _ => true),
                edges => wrappedGraph.AddVerticesAndEdgeRange(edges));

            #endregion

            #region Part 2

            wrappedGraph.Clear();
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });

            IIncidenceGraph<int, Edge<int>> filteredGraph = createFilteredGraph(
                vertex => vertex < 4,
                _ => true);

            filteredGraph.TryGetEdges(0, 10, out _).Should().BeFalse();
            filteredGraph.TryGetEdges(0, 1, out _).Should().BeFalse();

            filteredGraph.TryGetEdges(2, 2, out IEnumerable<Edge<int>>? gotEdges).Should().BeTrue();
            new[] { edge4 }.Should().BeEquivalentTo(gotEdges);

            filteredGraph.TryGetEdges(2, 4, out _).Should().BeFalse(); // Filtered

            filteredGraph.TryGetEdges(1, 2, out gotEdges).Should().BeTrue();
            new[] { edge1, edge2 }.Should().BeEquivalentTo(gotEdges);

            filteredGraph.TryGetEdges(2, 1, out gotEdges).Should().BeTrue();
            gotEdges.Should().BeEmpty();

            #endregion

            #region Part 3

            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });

            filteredGraph = createFilteredGraph(
                _ => true,
                edge => edge.Source != edge.Target);

            filteredGraph.TryGetEdges(0, 10, out _).Should().BeFalse();
            filteredGraph.TryGetEdges(0, 1, out _).Should().BeFalse();

            filteredGraph.TryGetEdges(2, 2, out gotEdges).Should().BeTrue(); // Filtered
            gotEdges.Should().BeEmpty();

            filteredGraph.TryGetEdges(2, 4, out gotEdges).Should().BeTrue();
            new[] { edge5 }.Should().BeEquivalentTo(gotEdges);

            filteredGraph.TryGetEdges(1, 2, out gotEdges).Should().BeTrue();
            new[] { edge1, edge2 }.Should().BeEquivalentTo(gotEdges);

            filteredGraph.TryGetEdges(2, 1, out gotEdges).Should().BeTrue();
            gotEdges.Should().BeEmpty();

            #endregion

            #region Part 4

            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });

            filteredGraph = createFilteredGraph(
                vertex => vertex < 4,
                edge => edge.Source != edge.Target);

            filteredGraph.TryGetEdges(0, 10, out _).Should().BeFalse();
            filteredGraph.TryGetEdges(0, 1, out _).Should().BeFalse();

            filteredGraph.TryGetEdges(2, 2, out gotEdges).Should().BeTrue(); // Filtered
            gotEdges.Should().BeEmpty();

            filteredGraph.TryGetEdges(2, 4, out _).Should().BeFalse(); // Filtered

            filteredGraph.TryGetEdges(1, 2, out gotEdges).Should().BeTrue();
            new[] { edge1, edge2 }.Should().BeEquivalentTo(gotEdges);

            filteredGraph.TryGetEdges(2, 1, out gotEdges).Should().BeTrue();
            gotEdges.Should().BeEmpty();

            #endregion
        }

        protected static void TryGetEdge_UndirectedGraph_Test<TGraph>(
            TGraph wrappedGraph,
            Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IImplicitUndirectedGraph<int, Edge<int>>> createFilteredGraph)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>, IMutableGraph<int, Edge<int>>
        {
            #region Part 1

            TryGetEdge_ImmutableGraph_UndirectedGraph_Test(
                wrappedGraph,
                () => createFilteredGraph(_ => true, _ => true));

            #endregion

            #region Part 2

            wrappedGraph.Clear();
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);
            var edge7 = new Edge<int>(5, 2);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6, edge7 });

            IImplicitUndirectedGraph<int, Edge<int>> filteredGraph = createFilteredGraph(
                vertex => vertex <= 4,
                _ => true);

            filteredGraph.TryGetEdge(0, 10, out _).Should().BeFalse();
            filteredGraph.TryGetEdge(0, 1, out _).Should().BeFalse();

            filteredGraph.TryGetEdge(2, 4, out Edge<int>? gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge5);

            filteredGraph.TryGetEdge(2, 2, out gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge4);

            filteredGraph.TryGetEdge(1, 2, out gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge1);

            // 1 -> 2 is present in this undirected graph
            filteredGraph.TryGetEdge(2, 1, out gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge1);

            filteredGraph.TryGetEdge(5, 2, out _).Should().BeFalse();  // Filtered
            filteredGraph.TryGetEdge(2, 5, out _).Should().BeFalse();  // Filtered

            #endregion

            #region Part 3

            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6, edge7 });

            filteredGraph = createFilteredGraph(
                _ => true,
                edge => edge.Source != edge.Target);

            filteredGraph.TryGetEdge(0, 10, out _).Should().BeFalse();
            filteredGraph.TryGetEdge(0, 1, out _).Should().BeFalse();

            filteredGraph.TryGetEdge(2, 4, out gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge5);

            filteredGraph.TryGetEdge(2, 2, out gotEdge).Should().BeFalse();    // Filtered

            filteredGraph.TryGetEdge(1, 2, out gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge1);

            // 1 -> 2 is present in this undirected graph
            filteredGraph.TryGetEdge(2, 1, out gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge1);

            filteredGraph.TryGetEdge(5, 2, out gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge7);

            filteredGraph.TryGetEdge(2, 5, out gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge7);

            #endregion

            #region Part 4

            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6, edge7 });

            filteredGraph = createFilteredGraph(
                vertex => vertex <= 4,
                edge => edge.Source != edge.Target);

            filteredGraph.TryGetEdge(0, 10, out _).Should().BeFalse();
            filteredGraph.TryGetEdge(0, 1, out _).Should().BeFalse();

            filteredGraph.TryGetEdge(2, 4, out gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge5);

            filteredGraph.TryGetEdge(2, 2, out _).Should().BeFalse();  // Filtered

            filteredGraph.TryGetEdge(1, 2, out gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge1);

            // 1 -> 2 is present in this undirected graph
            filteredGraph.TryGetEdge(2, 1, out gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge1);

            filteredGraph.TryGetEdge(5, 2, out _).Should().BeFalse();  // Filtered
            filteredGraph.TryGetEdge(2, 5, out _).Should().BeFalse();  // Filtered

            #endregion
        }

        protected static void TryGetOutEdges_Test<TGraph>(
            TGraph wrappedGraph,
            Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IImplicitGraph<int, Edge<int>>> createFilteredGraph)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>, IMutableGraph<int, Edge<int>>
        {
            #region Part 1

            TryGetOutEdges_Test(
                createFilteredGraph(_ => true, _ => true),
                edges => wrappedGraph.AddVerticesAndEdgeRange(edges));

            #endregion

            #region Part 2

            wrappedGraph.Clear();
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 3);
            var edge6 = new Edge<int>(2, 4);
            var edge7 = new Edge<int>(4, 3);
            var edge8 = new Edge<int>(4, 5);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6, edge7, edge8 });
            IImplicitGraph<int, Edge<int>> filteredGraph = createFilteredGraph(
                vertex => vertex <= 4,
                _ => true);

            filteredGraph.TryGetOutEdges(0, out _).Should().BeFalse();

            filteredGraph.TryGetOutEdges(5, out _).Should().BeFalse(); // Filtered

            filteredGraph.TryGetOutEdges(3, out IEnumerable<Edge<int>>? gotEdges).Should().BeTrue();
            gotEdges.Should().BeEmpty();

            filteredGraph.TryGetOutEdges(4, out gotEdges).Should().BeTrue();
            new[] { edge7 }.Should().BeEquivalentTo(gotEdges);   // Filtered

            filteredGraph.TryGetOutEdges(2, out gotEdges).Should().BeTrue();
            new[] { edge4, edge5, edge6 }.Should().BeEquivalentTo(gotEdges);

            #endregion

            #region Part 3

            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6, edge7, edge8 });
            filteredGraph = createFilteredGraph(
                _ => true,
                edge => edge.Source != edge.Target);

            filteredGraph.TryGetOutEdges(0, out _).Should().BeFalse();

            filteredGraph.TryGetOutEdges(5, out gotEdges).Should().BeTrue();
            gotEdges.Should().BeEmpty();

            filteredGraph.TryGetOutEdges(3, out gotEdges).Should().BeTrue();
            gotEdges.Should().BeEmpty();

            filteredGraph.TryGetOutEdges(4, out gotEdges).Should().BeTrue();
            new[] { edge7, edge8 }.Should().BeEquivalentTo(gotEdges);

            filteredGraph.TryGetOutEdges(2, out gotEdges).Should().BeTrue();
            new[] { edge5, edge6 }.Should().BeEquivalentTo(gotEdges);   // Filtered

            #endregion

            #region Part 4

            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6, edge7, edge8 });
            filteredGraph = createFilteredGraph(
                vertex => vertex <= 4,
                edge => edge.Source != edge.Target);

            filteredGraph.TryGetOutEdges(0, out _).Should().BeFalse();

            filteredGraph.TryGetOutEdges(5, out _).Should().BeFalse(); // Filtered

            filteredGraph.TryGetOutEdges(3, out gotEdges).Should().BeTrue();
            gotEdges.Should().BeEmpty();

            filteredGraph.TryGetOutEdges(4, out gotEdges).Should().BeTrue();
            new[] { edge7 }.Should().BeEquivalentTo(gotEdges);   // Filtered

            filteredGraph.TryGetOutEdges(2, out gotEdges).Should().BeTrue();
            new[] { edge5, edge6 }.Should().BeEquivalentTo(gotEdges);   // Filtered

            #endregion
        }

        protected static void TryGetInEdges_Test<TGraph>(
            TGraph wrappedGraph,
            Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IBidirectionalIncidenceGraph<int, Edge<int>>> createFilteredGraph)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>, IMutableGraph<int, Edge<int>>
        {
            #region Part 1

            TryGetInEdges_Test(
                createFilteredGraph(_ => true, _ => true),
                edges => wrappedGraph.AddVerticesAndEdgeRange(edges));

            #endregion

            #region Part 2

            wrappedGraph.Clear();
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);
            var edge7 = new Edge<int>(5, 3);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6, edge7 });
            IBidirectionalIncidenceGraph<int, Edge<int>> filteredGraph = createFilteredGraph(
                vertex => vertex <= 4,
                _ => true);

            filteredGraph.TryGetInEdges(0, out _).Should().BeFalse();

            filteredGraph.TryGetInEdges(5, out _).Should().BeFalse();  // Filtered

            filteredGraph.TryGetInEdges(4, out IEnumerable<Edge<int>>? gotEdges).Should().BeTrue();
            new[] { edge5 }.Should().BeEquivalentTo(gotEdges);

            filteredGraph.TryGetInEdges(2, out gotEdges).Should().BeTrue();
            new[] { edge1, edge2, edge4 }.Should().BeEquivalentTo(gotEdges);

            #endregion

            #region Part 3

            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6, edge7 });
            filteredGraph = createFilteredGraph(
                _ => true,
                edge => edge.Source != edge.Target);

            filteredGraph.TryGetInEdges(0, out _).Should().BeFalse();

            filteredGraph.TryGetInEdges(5, out gotEdges).Should().BeTrue();
            gotEdges.Should().BeEmpty();

            filteredGraph.TryGetInEdges(4, out gotEdges).Should().BeTrue();
            new[] { edge5 }.Should().BeEquivalentTo(gotEdges);

            filteredGraph.TryGetInEdges(2, out gotEdges).Should().BeTrue();
            new[] { edge1, edge2 }.Should().BeEquivalentTo(gotEdges);    // Filtered

            #endregion

            #region Part 4

            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6, edge7 });
            filteredGraph = createFilteredGraph(
                vertex => vertex <= 4,
                edge => edge.Source != edge.Target);

            filteredGraph.TryGetInEdges(0, out _).Should().BeFalse();

            filteredGraph.TryGetInEdges(5, out _).Should().BeFalse();  // Filtered

            filteredGraph.TryGetInEdges(4, out gotEdges).Should().BeTrue();
            new[] { edge5 }.Should().BeEquivalentTo(gotEdges);

            filteredGraph.TryGetInEdges(2, out gotEdges).Should().BeTrue();
            new[] { edge1, edge2 }.Should().BeEquivalentTo(gotEdges);    // Filtered

            #endregion
        }

        #endregion
    }
}
