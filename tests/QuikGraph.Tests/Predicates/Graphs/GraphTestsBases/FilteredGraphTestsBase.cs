using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Tests.Structures;
using static QuikGraph.Tests.AssertHelpers;
using static QuikGraph.Tests.GraphTestHelpers;

namespace QuikGraph.Tests.Predicates
{
    /// <summary>
    /// Base class for filtered graph tests.
    /// </summary>
    internal abstract class FilteredGraphTestsBase : GraphTestsBase
    {
        #region Vertices & Edges

        protected static void Vertices_Test<TGraph>(
            [NotNull] TGraph wrappedGraph,
            [NotNull] Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IVertexSet<int>> createFilteredGraph)
            where TGraph : IMutableVertexSet<int>, IMutableGraph<int, Edge<int>>
        {
            IVertexSet<int> filteredGraph = createFilteredGraph(vertex => true, edge => true);
            AssertNoVertex(filteredGraph);

            wrappedGraph.AddVertexRange(new[] { 1, 2, 3 });
            AssertHasVertices(filteredGraph, new[] { 1, 2, 3 });


            wrappedGraph.Clear();
            filteredGraph = createFilteredGraph(vertex => vertex < 3, edge => true);
            AssertNoVertex(filteredGraph);

            wrappedGraph.AddVertexRange(new[] { 1, 2, 3 });
            AssertHasVertices(filteredGraph, new[] { 1, 2 });
        }

        public void Edges_Test<TGraph>(
            [NotNull] TGraph wrappedGraph,
            [NotNull] Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IEdgeSet<int, Edge<int>>> createFilteredGraph)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>, IMutableGraph<int, Edge<int>>
        {
            IEdgeSet<int, Edge<int>> filteredGraph = createFilteredGraph(vertex => true, edge => true);
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
            filteredGraph = createFilteredGraph(vertex => vertex <= 3, edge => true);
            AssertNoEdge(filteredGraph);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge22, edge31, edge33, edge41 });
            AssertHasEdges(filteredGraph, new[] { edge12, edge13, edge22, edge31, edge33 });


            wrappedGraph.Clear();
            filteredGraph = createFilteredGraph(vertex => true, edge => edge.Source != edge.Target);
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
            [NotNull] TGraph wrappedGraph,
            [NotNull] Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IImplicitVertexSet<int>> createFilteredGraph)
            where TGraph : IMutableVertexSet<int>, IMutableGraph<int, Edge<int>>
        {
            IImplicitVertexSet<int> filteredGraph = createFilteredGraph(
                vertex => true,
                edge => true);

            Assert.IsFalse(filteredGraph.ContainsVertex(1));
            Assert.IsFalse(filteredGraph.ContainsVertex(2));

            wrappedGraph.AddVertex(1);
            Assert.IsTrue(filteredGraph.ContainsVertex(1));
            Assert.IsFalse(filteredGraph.ContainsVertex(2));

            wrappedGraph.AddVertex(2);
            Assert.IsTrue(filteredGraph.ContainsVertex(1));
            Assert.IsTrue(filteredGraph.ContainsVertex(2));

            wrappedGraph.RemoveVertex(1);
            Assert.IsFalse(filteredGraph.ContainsVertex(1));
            Assert.IsTrue(filteredGraph.ContainsVertex(2));


            wrappedGraph.Clear();
            filteredGraph = createFilteredGraph(
                vertex => vertex <= 2,
                edge => true);

            Assert.IsFalse(filteredGraph.ContainsVertex(1));
            Assert.IsFalse(filteredGraph.ContainsVertex(2));
            Assert.IsFalse(filteredGraph.ContainsVertex(3));

            wrappedGraph.AddVertex(1);
            Assert.IsTrue(filteredGraph.ContainsVertex(1));
            Assert.IsFalse(filteredGraph.ContainsVertex(2));
            Assert.IsFalse(filteredGraph.ContainsVertex(3));

            wrappedGraph.AddVertex(2);
            Assert.IsTrue(filteredGraph.ContainsVertex(1));
            Assert.IsTrue(filteredGraph.ContainsVertex(2));
            Assert.IsFalse(filteredGraph.ContainsVertex(3));

            wrappedGraph.AddVertex(3);
            Assert.IsTrue(filteredGraph.ContainsVertex(1));
            Assert.IsTrue(filteredGraph.ContainsVertex(2));
            Assert.IsFalse(filteredGraph.ContainsVertex(3));    // Filtered

            wrappedGraph.RemoveVertex(1);
            Assert.IsFalse(filteredGraph.ContainsVertex(1));
            Assert.IsTrue(filteredGraph.ContainsVertex(2));
            Assert.IsFalse(filteredGraph.ContainsVertex(3));
        }

        #endregion

        #region Contains Edge

        protected static void ContainsEdge_Test<TGraph>(
            [NotNull] TGraph wrappedGraph,
            [NotNull] Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IEdgeSet<int, Edge<int>>> createFilteredGraph)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>, IMutableGraph<int, Edge<int>>
        {
            #region Part 1

            IEdgeSet<int, Edge<int>> filteredGraph = createFilteredGraph(
                vertex => true,
                edge => true);

            ContainsEdge_Test(
                filteredGraph,
                edge => wrappedGraph.AddVerticesAndEdge(edge));

            #endregion

            #region Part 2

            wrappedGraph.Clear();
            filteredGraph = createFilteredGraph(
                vertex => vertex > 0 && vertex < 3,
                edge => true);
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 1);
            var edge4 = new Edge<int>(2, 2);
            var otherEdge1 = new Edge<int>(1, 2);

            Assert.IsFalse(filteredGraph.ContainsEdge(edge1));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge2));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge3));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge4));
            Assert.IsFalse(filteredGraph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge1);
            Assert.IsTrue(filteredGraph.ContainsEdge(edge1));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge2));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge3));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge4));
            Assert.IsFalse(filteredGraph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge2);
            Assert.IsTrue(filteredGraph.ContainsEdge(edge1));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge2));  // Filtered
            Assert.IsFalse(filteredGraph.ContainsEdge(edge3));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge4));
            Assert.IsFalse(filteredGraph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge3);
            Assert.IsTrue(filteredGraph.ContainsEdge(edge1));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge2));  // Filtered
            Assert.IsTrue(filteredGraph.ContainsEdge(edge3));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge4));
            Assert.IsFalse(filteredGraph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge4);
            Assert.IsTrue(filteredGraph.ContainsEdge(edge1));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge2));  // Filtered
            Assert.IsTrue(filteredGraph.ContainsEdge(edge3));
            Assert.IsTrue(filteredGraph.ContainsEdge(edge4));
            Assert.IsFalse(filteredGraph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(otherEdge1);
            Assert.IsTrue(filteredGraph.ContainsEdge(edge1));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge2));  // Filtered
            Assert.IsTrue(filteredGraph.ContainsEdge(edge3));
            Assert.IsTrue(filteredGraph.ContainsEdge(edge4));
            Assert.IsTrue(filteredGraph.ContainsEdge(otherEdge1));

            // Both vertices not in graph
            Assert.IsFalse(filteredGraph.ContainsEdge(new Edge<int>(0, 10)));
            // Source not in graph
            Assert.IsFalse(filteredGraph.ContainsEdge(new Edge<int>(0, 1)));
            // Target not in graph
            Assert.IsFalse(filteredGraph.ContainsEdge(new Edge<int>(1, 0)));

            #endregion

            #region Part 3

            wrappedGraph.Clear();
            filteredGraph = createFilteredGraph(
                vertex => true,
                edge => edge.Source != edge.Target);

            Assert.IsFalse(filteredGraph.ContainsEdge(edge1));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge2));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge3));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge4));
            Assert.IsFalse(filteredGraph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge1);
            Assert.IsTrue(filteredGraph.ContainsEdge(edge1));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge2));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge3));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge4));
            Assert.IsFalse(filteredGraph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge2);
            Assert.IsTrue(filteredGraph.ContainsEdge(edge1));
            Assert.IsTrue(filteredGraph.ContainsEdge(edge2));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge3));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge4));
            Assert.IsFalse(filteredGraph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge3);
            Assert.IsTrue(filteredGraph.ContainsEdge(edge1));
            Assert.IsTrue(filteredGraph.ContainsEdge(edge2));
            Assert.IsTrue(filteredGraph.ContainsEdge(edge3));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge4));
            Assert.IsFalse(filteredGraph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge4);
            Assert.IsTrue(filteredGraph.ContainsEdge(edge1));
            Assert.IsTrue(filteredGraph.ContainsEdge(edge2));
            Assert.IsTrue(filteredGraph.ContainsEdge(edge3));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge4));   // Filtered
            Assert.IsFalse(filteredGraph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(otherEdge1);
            Assert.IsTrue(filteredGraph.ContainsEdge(edge1));
            Assert.IsTrue(filteredGraph.ContainsEdge(edge2));
            Assert.IsTrue(filteredGraph.ContainsEdge(edge3));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge4));  // Filtered
            Assert.IsTrue(filteredGraph.ContainsEdge(otherEdge1));

            // Both vertices not in graph
            Assert.IsFalse(filteredGraph.ContainsEdge(new Edge<int>(0, 10)));
            // Source not in graph
            Assert.IsFalse(filteredGraph.ContainsEdge(new Edge<int>(0, 1)));
            // Target not in graph
            Assert.IsFalse(filteredGraph.ContainsEdge(new Edge<int>(1, 0)));

            #endregion

            #region Part 4

            wrappedGraph.Clear();
            filteredGraph = createFilteredGraph(
                vertex => vertex > 0 && vertex < 3,
                edge => edge.Source != edge.Target);

            Assert.IsFalse(filteredGraph.ContainsEdge(edge1));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge2));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge3));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge4));
            Assert.IsFalse(filteredGraph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge1);
            Assert.IsTrue(filteredGraph.ContainsEdge(edge1));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge2));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge3));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge4));
            Assert.IsFalse(filteredGraph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge2);
            Assert.IsTrue(filteredGraph.ContainsEdge(edge1));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge2));  // Filtered
            Assert.IsFalse(filteredGraph.ContainsEdge(edge3));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge4));
            Assert.IsFalse(filteredGraph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge3);
            Assert.IsTrue(filteredGraph.ContainsEdge(edge1));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge2));  // Filtered
            Assert.IsTrue(filteredGraph.ContainsEdge(edge3));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge4));
            Assert.IsFalse(filteredGraph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge4);
            Assert.IsTrue(filteredGraph.ContainsEdge(edge1));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge2));  // Filtered
            Assert.IsTrue(filteredGraph.ContainsEdge(edge3));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge4));  // Filtered
            Assert.IsFalse(filteredGraph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(otherEdge1);
            Assert.IsTrue(filteredGraph.ContainsEdge(edge1));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge2));  // Filtered
            Assert.IsTrue(filteredGraph.ContainsEdge(edge3));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge4));  // Filtered
            Assert.IsTrue(filteredGraph.ContainsEdge(otherEdge1));

            // Both vertices not in graph
            Assert.IsFalse(filteredGraph.ContainsEdge(new Edge<int>(0, 10)));
            // Source not in graph
            Assert.IsFalse(filteredGraph.ContainsEdge(new Edge<int>(0, 1)));
            // Target not in graph
            Assert.IsFalse(filteredGraph.ContainsEdge(new Edge<int>(1, 0)));

            #endregion
        }

        protected static void ContainsEdge_EquatableEdge_Test<TGraph>(
            [NotNull] TGraph wrappedGraph,
            [NotNull] Func<VertexPredicate<int>, EdgePredicate<int, EquatableEdge<int>>, IEdgeSet<int, EquatableEdge<int>>> createFilteredGraph)
            where TGraph : IMutableVertexAndEdgeSet<int, EquatableEdge<int>>, IMutableGraph<int, EquatableEdge<int>>
        {
            #region Part 1

            IEdgeSet<int, EquatableEdge<int>> filteredGraph = createFilteredGraph(
                vertex => true,
                edge => true);

            ContainsEdge_EquatableEdge_Test(
                filteredGraph,
                edge => wrappedGraph.AddVerticesAndEdge(edge));

            #endregion

            #region Part 2

            wrappedGraph.Clear();
            filteredGraph = createFilteredGraph(
                vertex => vertex > 0 && vertex < 3,
                edge => true);
            var edge1 = new EquatableEdge<int>(1, 2);
            var edge2 = new EquatableEdge<int>(1, 3);
            var edge3 = new EquatableEdge<int>(2, 1);
            var edge4 = new EquatableEdge<int>(2, 2);
            var otherEdge1 = new EquatableEdge<int>(1, 2);

            Assert.IsFalse(filteredGraph.ContainsEdge(edge1));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge2));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge3));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge4));
            Assert.IsFalse(filteredGraph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge1);
            Assert.IsTrue(filteredGraph.ContainsEdge(edge1));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge2));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge3));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge4));
            Assert.IsTrue(filteredGraph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge2);
            Assert.IsTrue(filteredGraph.ContainsEdge(edge1));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge2));  // Filtered
            Assert.IsFalse(filteredGraph.ContainsEdge(edge3));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge4));
            Assert.IsTrue(filteredGraph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge3);
            Assert.IsTrue(filteredGraph.ContainsEdge(edge1));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge2));  // Filtered
            Assert.IsTrue(filteredGraph.ContainsEdge(edge3));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge4));
            Assert.IsTrue(filteredGraph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge4);
            Assert.IsTrue(filteredGraph.ContainsEdge(edge1));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge2));  // Filtered
            Assert.IsTrue(filteredGraph.ContainsEdge(edge3));
            Assert.IsTrue(filteredGraph.ContainsEdge(edge4));
            Assert.IsTrue(filteredGraph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(otherEdge1);
            Assert.IsTrue(filteredGraph.ContainsEdge(edge1));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge2));  // Filtered
            Assert.IsTrue(filteredGraph.ContainsEdge(edge3));
            Assert.IsTrue(filteredGraph.ContainsEdge(edge4));
            Assert.IsTrue(filteredGraph.ContainsEdge(otherEdge1));

            // Both vertices not in graph
            Assert.IsFalse(filteredGraph.ContainsEdge(new EquatableEdge<int>(0, 10)));
            // Source not in graph
            Assert.IsFalse(filteredGraph.ContainsEdge(new EquatableEdge<int>(0, 1)));
            // Target not in graph
            Assert.IsFalse(filteredGraph.ContainsEdge(new EquatableEdge<int>(1, 0)));

            #endregion

            #region Part 3

            wrappedGraph.Clear();
            filteredGraph = createFilteredGraph(
                vertex => true,
                edge => edge.Source != edge.Target);

            Assert.IsFalse(filteredGraph.ContainsEdge(edge1));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge2));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge3));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge4));
            Assert.IsFalse(filteredGraph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge1);
            Assert.IsTrue(filteredGraph.ContainsEdge(edge1));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge2));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge3));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge4));
            Assert.IsTrue(filteredGraph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge2);
            Assert.IsTrue(filteredGraph.ContainsEdge(edge1));
            Assert.IsTrue(filteredGraph.ContainsEdge(edge2));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge3));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge4));
            Assert.IsTrue(filteredGraph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge3);
            Assert.IsTrue(filteredGraph.ContainsEdge(edge1));
            Assert.IsTrue(filteredGraph.ContainsEdge(edge2));
            Assert.IsTrue(filteredGraph.ContainsEdge(edge3));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge4));
            Assert.IsTrue(filteredGraph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge4);
            Assert.IsTrue(filteredGraph.ContainsEdge(edge1));
            Assert.IsTrue(filteredGraph.ContainsEdge(edge2));
            Assert.IsTrue(filteredGraph.ContainsEdge(edge3));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge4));   // Filtered
            Assert.IsTrue(filteredGraph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(otherEdge1);
            Assert.IsTrue(filteredGraph.ContainsEdge(edge1));
            Assert.IsTrue(filteredGraph.ContainsEdge(edge2));
            Assert.IsTrue(filteredGraph.ContainsEdge(edge3));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge4));  // Filtered
            Assert.IsTrue(filteredGraph.ContainsEdge(otherEdge1));

            // Both vertices not in graph
            Assert.IsFalse(filteredGraph.ContainsEdge(new EquatableEdge<int>(0, 10)));
            // Source not in graph
            Assert.IsFalse(filteredGraph.ContainsEdge(new EquatableEdge<int>(0, 1)));
            // Target not in graph
            Assert.IsFalse(filteredGraph.ContainsEdge(new EquatableEdge<int>(1, 0)));

            #endregion

            #region Part 4

            wrappedGraph.Clear();
            filteredGraph = createFilteredGraph(
                vertex => vertex > 0 && vertex < 3,
                edge => edge.Source != edge.Target);

            Assert.IsFalse(filteredGraph.ContainsEdge(edge1));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge2));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge3));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge4));
            Assert.IsFalse(filteredGraph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge1);
            Assert.IsTrue(filteredGraph.ContainsEdge(edge1));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge2));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge3));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge4));
            Assert.IsTrue(filteredGraph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge2);
            Assert.IsTrue(filteredGraph.ContainsEdge(edge1));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge2));  // Filtered
            Assert.IsFalse(filteredGraph.ContainsEdge(edge3));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge4));
            Assert.IsTrue(filteredGraph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge3);
            Assert.IsTrue(filteredGraph.ContainsEdge(edge1));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge2));  // Filtered
            Assert.IsTrue(filteredGraph.ContainsEdge(edge3));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge4));
            Assert.IsTrue(filteredGraph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge4);
            Assert.IsTrue(filteredGraph.ContainsEdge(edge1));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge2));  // Filtered
            Assert.IsTrue(filteredGraph.ContainsEdge(edge3));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge4));  // Filtered
            Assert.IsTrue(filteredGraph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(otherEdge1);
            Assert.IsTrue(filteredGraph.ContainsEdge(edge1));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge2));  // Filtered
            Assert.IsTrue(filteredGraph.ContainsEdge(edge3));
            Assert.IsFalse(filteredGraph.ContainsEdge(edge4));  // Filtered
            Assert.IsTrue(filteredGraph.ContainsEdge(otherEdge1));

            // Both vertices not in graph
            Assert.IsFalse(filteredGraph.ContainsEdge(new EquatableEdge<int>(0, 10)));
            // Source not in graph
            Assert.IsFalse(filteredGraph.ContainsEdge(new EquatableEdge<int>(0, 1)));
            // Target not in graph
            Assert.IsFalse(filteredGraph.ContainsEdge(new EquatableEdge<int>(1, 0)));

            #endregion
        }

        protected static void ContainsEdge_SourceTarget_Test<TGraph>(
            [NotNull] TGraph wrappedGraph,
            [NotNull] Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IIncidenceGraph<int, Edge<int>>> createFilteredGraph)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>, IMutableGraph<int, Edge<int>>
        {
            #region Part 1

            ContainsEdge_SourceTarget_ImmutableGraph_Test(
                wrappedGraph,
                () => createFilteredGraph(vertex => true, edge => true));

            #endregion

            #region Part 2

            wrappedGraph.Clear();
            IIncidenceGraph<int, Edge<int>> filteredGraph = createFilteredGraph(
                vertex => vertex > 0 && vertex < 3,
                edge => true);

            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 2);

            Assert.IsFalse(filteredGraph.ContainsEdge(1, 2));
            Assert.IsFalse(filteredGraph.ContainsEdge(2, 1));

            wrappedGraph.AddVerticesAndEdge(edge1);
            Assert.IsTrue(filteredGraph.ContainsEdge(1, 2));
            Assert.IsFalse(filteredGraph.ContainsEdge(2, 1));

            wrappedGraph.AddVerticesAndEdge(edge2);
            Assert.IsFalse(filteredGraph.ContainsEdge(1, 3));   // Filtered
            Assert.IsFalse(filteredGraph.ContainsEdge(3, 1));   // Filtered

            wrappedGraph.AddVerticesAndEdge(edge3);
            Assert.IsTrue(filteredGraph.ContainsEdge(2, 2));

            // Vertices is not present in the graph
            Assert.IsFalse(filteredGraph.ContainsEdge(0, 4));
            Assert.IsFalse(filteredGraph.ContainsEdge(1, 4));
            Assert.IsFalse(filteredGraph.ContainsEdge(4, 1));

            #endregion

            #region Part 3

            wrappedGraph.Clear();
            filteredGraph = createFilteredGraph(
                vertex => true,
                edge => edge.Source != edge.Target);

            Assert.IsFalse(filteredGraph.ContainsEdge(1, 2));
            Assert.IsFalse(filteredGraph.ContainsEdge(2, 1));

            wrappedGraph.AddVerticesAndEdge(edge1);
            Assert.IsTrue(filteredGraph.ContainsEdge(1, 2));
            Assert.IsFalse(filteredGraph.ContainsEdge(2, 1));

            wrappedGraph.AddVerticesAndEdge(edge2);
            Assert.IsTrue(filteredGraph.ContainsEdge(1, 3));
            Assert.IsFalse(filteredGraph.ContainsEdge(3, 1));

            wrappedGraph.AddVerticesAndEdge(edge3);
            Assert.IsFalse(filteredGraph.ContainsEdge(2, 2));   // Filtered

            // Vertices is not present in the graph
            Assert.IsFalse(filteredGraph.ContainsEdge(0, 4));
            Assert.IsFalse(filteredGraph.ContainsEdge(1, 4));
            Assert.IsFalse(filteredGraph.ContainsEdge(4, 1));

            #endregion

            #region Part 4

            wrappedGraph.Clear();
            filteredGraph = createFilteredGraph(
                vertex => vertex > 0 && vertex < 3,
                edge => edge.Source != edge.Target);

            Assert.IsFalse(filteredGraph.ContainsEdge(1, 2));
            Assert.IsFalse(filteredGraph.ContainsEdge(2, 1));

            wrappedGraph.AddVerticesAndEdge(edge1);
            Assert.IsTrue(filteredGraph.ContainsEdge(1, 2));
            Assert.IsFalse(filteredGraph.ContainsEdge(2, 1));

            wrappedGraph.AddVerticesAndEdge(edge2);
            Assert.IsFalse(filteredGraph.ContainsEdge(1, 3));   // Filtered
            Assert.IsFalse(filteredGraph.ContainsEdge(3, 1));   // Filtered

            wrappedGraph.AddVerticesAndEdge(edge3);
            Assert.IsFalse(filteredGraph.ContainsEdge(2, 2));   // Filtered

            // Vertices is not present in the graph
            Assert.IsFalse(filteredGraph.ContainsEdge(0, 4));
            Assert.IsFalse(filteredGraph.ContainsEdge(1, 4));
            Assert.IsFalse(filteredGraph.ContainsEdge(4, 1));

            #endregion
        }

        protected static void ContainsEdge_SourceTarget_UndirectedGraph_Test<TGraph>(
            [NotNull] TGraph wrappedGraph,
            [NotNull] Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IImplicitUndirectedGraph<int, Edge<int>>> createFilteredGraph)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>, IMutableGraph<int, Edge<int>>
        {
            #region Part 1

            ContainsEdge_SourceTarget_ImmutableGraph_UndirectedGraph_Test(
                wrappedGraph,
                () => createFilteredGraph(vertex => true, edge => true));

            #endregion

            #region Part 2

            wrappedGraph.Clear();
            IImplicitUndirectedGraph<int, Edge<int>> filteredGraph = createFilteredGraph(
                vertex => vertex > 0 && vertex < 3,
                edge => true);

            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 2);

            Assert.IsFalse(filteredGraph.ContainsEdge(1, 2));
            Assert.IsFalse(filteredGraph.ContainsEdge(2, 1));

            wrappedGraph.AddVerticesAndEdge(edge1);
            Assert.IsTrue(filteredGraph.ContainsEdge(1, 2));
            Assert.IsTrue(filteredGraph.ContainsEdge(2, 1));

            wrappedGraph.AddVerticesAndEdge(edge2);
            Assert.IsFalse(filteredGraph.ContainsEdge(1, 3));   // Filtered
            Assert.IsFalse(filteredGraph.ContainsEdge(3, 1));   // Filtered

            wrappedGraph.AddVerticesAndEdge(edge3);
            Assert.IsTrue(filteredGraph.ContainsEdge(2, 2));

            // Vertices is not present in the graph
            Assert.IsFalse(filteredGraph.ContainsEdge(0, 4));
            Assert.IsFalse(filteredGraph.ContainsEdge(1, 4));
            Assert.IsFalse(filteredGraph.ContainsEdge(4, 1));

            #endregion

            #region Part 3

            wrappedGraph.Clear();
            filteredGraph = createFilteredGraph(
                vertex => true,
                edge => edge.Source != edge.Target);

            Assert.IsFalse(filteredGraph.ContainsEdge(1, 2));
            Assert.IsFalse(filteredGraph.ContainsEdge(2, 1));

            wrappedGraph.AddVerticesAndEdge(edge1);
            Assert.IsTrue(filteredGraph.ContainsEdge(1, 2));
            Assert.IsTrue(filteredGraph.ContainsEdge(2, 1));

            wrappedGraph.AddVerticesAndEdge(edge2);
            Assert.IsTrue(filteredGraph.ContainsEdge(1, 3));
            Assert.IsTrue(filteredGraph.ContainsEdge(3, 1));

            wrappedGraph.AddVerticesAndEdge(edge3);
            Assert.IsFalse(filteredGraph.ContainsEdge(2, 2));   // Filtered

            // Vertices is not present in the graph
            Assert.IsFalse(filteredGraph.ContainsEdge(0, 4));
            Assert.IsFalse(filteredGraph.ContainsEdge(1, 4));
            Assert.IsFalse(filteredGraph.ContainsEdge(4, 1));

            #endregion

            #region Part 4

            wrappedGraph.Clear();
            filteredGraph = createFilteredGraph(
                vertex => vertex > 0 && vertex < 3,
                edge => edge.Source != edge.Target);

            Assert.IsFalse(filteredGraph.ContainsEdge(1, 2));
            Assert.IsFalse(filteredGraph.ContainsEdge(2, 1));

            wrappedGraph.AddVerticesAndEdge(edge1);
            Assert.IsTrue(filteredGraph.ContainsEdge(1, 2));
            Assert.IsTrue(filteredGraph.ContainsEdge(2, 1));

            wrappedGraph.AddVerticesAndEdge(edge2);
            Assert.IsFalse(filteredGraph.ContainsEdge(1, 3));   // Filtered
            Assert.IsFalse(filteredGraph.ContainsEdge(3, 1));   // Filtered

            wrappedGraph.AddVerticesAndEdge(edge3);
            Assert.IsFalse(filteredGraph.ContainsEdge(2, 2));   // Filtered

            // Vertices is not present in the graph
            Assert.IsFalse(filteredGraph.ContainsEdge(0, 4));
            Assert.IsFalse(filteredGraph.ContainsEdge(1, 4));
            Assert.IsFalse(filteredGraph.ContainsEdge(4, 1));

            #endregion
        }

        #endregion

        #region Out Edges

        protected static void OutEdge_Test<TGraph>(
            [NotNull] TGraph wrappedGraph,
            [NotNull] Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IImplicitGraph<int, Edge<int>>> createFilteredGraph)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>, IMutableGraph<int, Edge<int>>
        {
            #region Part 1

            OutEdge_ImmutableGraph_Test(
                wrappedGraph,
                () => createFilteredGraph(vertex => true, edge => true));

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
                edge => true);

            Assert.AreSame(edge11, filteredGraph.OutEdge(1, 0));
            Assert.AreSame(edge13, filteredGraph.OutEdge(1, 2));
            Assert.AreSame(edge33, filteredGraph.OutEdge(3, 0));
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<VertexNotFoundException>(() => filteredGraph.OutEdge(4, 0)); // Filtered

            #endregion

            #region Part 3

            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge11, edge12, edge13, edge24, edge33, edge34, edge41 });
            filteredGraph = createFilteredGraph(
                vertex => true,
                edge => edge.Source != edge.Target);

            Assert.AreSame(edge12, filteredGraph.OutEdge(1, 0));
            Assert.AreSame(edge13, filteredGraph.OutEdge(1, 1));
            Assert.AreSame(edge34, filteredGraph.OutEdge(3, 0));
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            AssertIndexOutOfRange(() => filteredGraph.OutEdge(3, 1));  // Filtered

            #endregion

            #region Part 4

            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge11, edge12, edge13, edge24, edge33, edge34, edge41 });
            filteredGraph = createFilteredGraph(
                vertex => vertex < 4,
                edge => edge.Source != edge.Target);

            Assert.AreSame(edge12, filteredGraph.OutEdge(1, 0));
            Assert.AreSame(edge13, filteredGraph.OutEdge(1, 1));
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            AssertIndexOutOfRange(() => filteredGraph.OutEdge(3, 0));  // Filtered
            Assert.Throws<VertexNotFoundException>(() => filteredGraph.OutEdge(4, 1)); // Filtered
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            #endregion
        }

        protected static void OutEdge_Throws_Test<TGraph>(
            [NotNull] TGraph wrappedGraph,
            [NotNull] Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IImplicitGraph<int, Edge<int>>> createFilteredGraph)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>, IMutableGraph<int, Edge<int>>
        {
            #region Part 1

            OutEdge_Throws_ImmutableGraph_Test(
                wrappedGraph,
                () => createFilteredGraph(vertex => true, edge => true));

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
                edge => true);

            AssertIndexOutOfRange(() => filteredGraph.OutEdge(vertex1, 2));
            Assert.Throws<VertexNotFoundException>(() => filteredGraph.OutEdge(vertex3, 0));
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
                vertex => true,
                edge => edge.Source != 1);

            AssertIndexOutOfRange(() => filteredGraph.OutEdge(vertex1, 0));
            Assert.Throws<VertexNotFoundException>(() => filteredGraph.OutEdge(vertex4, 0));
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
            Assert.Throws<VertexNotFoundException>(() => filteredGraph.OutEdge(vertex4, 0));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            #endregion
        }

        protected static void OutEdges_Test<TGraph>(
            [NotNull] TGraph wrappedGraph,
            [NotNull] Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IImplicitGraph<int, Edge<int>>> createFilteredGraph)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>, IMutableGraph<int, Edge<int>>
        {
            #region Part 1

            OutEdges_ImmutableGraph_Test(
                wrappedGraph,
                () => createFilteredGraph(vertex => true, edge => true));

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
                edge => true);

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
                vertex => true,
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
            [NotNull] TGraph wrappedGraph,
            [NotNull] Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IBidirectionalIncidenceGraph<int, Edge<int>>> createFilteredGraph)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>, IMutableGraph<int, Edge<int>>
        {
            #region Part 1

            InEdge_ImmutableGraph_Test(
                wrappedGraph,
                () => createFilteredGraph(vertex => true, edge => true));

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
                edge => true);

            Assert.AreSame(edge11, filteredGraph.InEdge(1, 0));
            Assert.AreSame(edge21, filteredGraph.InEdge(1, 1));
            Assert.AreSame(edge13, filteredGraph.InEdge(3, 0));
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            AssertIndexOutOfRange(() => filteredGraph.InEdge(1, 2));    // Filtered
            Assert.Throws<VertexNotFoundException>(() => filteredGraph.InEdge(4, 0)); // Filtered
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            #endregion

            #region Part 3

            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge11, edge13, edge14, edge21 });
            filteredGraph = createFilteredGraph(
                vertex => true,
                edge => edge.Source != edge.Target);

            Assert.AreSame(edge21, filteredGraph.InEdge(1, 0));    // Filtered
            Assert.AreSame(edge13, filteredGraph.InEdge(3, 0));
            Assert.AreSame(edge14, filteredGraph.InEdge(4, 0));

            #endregion

            #region Part 4

            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge11, edge13, edge14, edge21 });
            filteredGraph = createFilteredGraph(
                vertex => vertex < 4,
                edge => edge.Source != edge.Target);

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.AreSame(edge21, filteredGraph.InEdge(1, 0)); // Filtered
            Assert.AreSame(edge13, filteredGraph.InEdge(3, 0));
            AssertIndexOutOfRange(() => filteredGraph.InEdge(1, 2));    // Filtered
            Assert.Throws<VertexNotFoundException>(() => filteredGraph.InEdge(4, 0)); // Filtered
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            #endregion
        }

        protected static void InEdge_Throws_Test<TGraph>(
            [NotNull] TGraph wrappedGraph,
            [NotNull] Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IBidirectionalIncidenceGraph<int, Edge<int>>> createFilteredGraph)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>, IMutableGraph<int, Edge<int>>
        {
            #region Part 1

            InEdge_Throws_ImmutableGraph_Test(
                wrappedGraph,
                () => createFilteredGraph(vertex => true, edge => true));

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
                edge => true);

            AssertIndexOutOfRange(() => filteredGraph.InEdge(vertex1, 2));
            Assert.Throws<VertexNotFoundException>(() => filteredGraph.InEdge(vertex3, 0));
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
                vertex => true,
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
            Assert.Throws<VertexNotFoundException>(() => filteredGraph.InEdge(vertex3, 0));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            #endregion
        }

        protected static void InEdges_Test<TGraph>(
            [NotNull] TGraph wrappedGraph,
            [NotNull] Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IBidirectionalIncidenceGraph<int, Edge<int>>> createFilteredGraph)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>, IMutableGraph<int, Edge<int>>
        {
            #region Part 1

            InEdges_ImmutableGraph_Test(
                wrappedGraph,
                () => createFilteredGraph(vertex => true, edge => true));

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
                edge => true);

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
                vertex => true,
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
            [NotNull] TGraph wrappedGraph,
            [NotNull] Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IImplicitUndirectedGraph<int, Edge<int>>> createFilteredGraph)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>, IMutableGraph<int, Edge<int>>
        {
            #region Part 1

            AdjacentEdge_ImmutableGraph_Test(
                wrappedGraph,
                () => createFilteredGraph(vertex => true, edge => true));

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
                edge => true);

            Assert.AreSame(edge11, filteredGraph.AdjacentEdge(1, 0));
            Assert.AreSame(edge13, filteredGraph.AdjacentEdge(1, 2));
            Assert.AreSame(edge13, filteredGraph.AdjacentEdge(3, 0));
            Assert.AreSame(edge33, filteredGraph.AdjacentEdge(3, 1));
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<VertexNotFoundException>(() => filteredGraph.AdjacentEdge(4, 1)); // Filtered

            #endregion

            #region Part 3

            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge11, edge12, edge13, edge24, edge33, edge41 });
            filteredGraph = createFilteredGraph(
                vertex => true,
                edge => edge.Source != edge.Target);

            Assert.AreSame(edge12, filteredGraph.AdjacentEdge(1, 0));
            Assert.AreSame(edge13, filteredGraph.AdjacentEdge(1, 1));
            Assert.AreSame(edge13, filteredGraph.AdjacentEdge(3, 0));
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            AssertIndexOutOfRange(() => filteredGraph.AdjacentEdge(3, 1));  // Filtered

            #endregion

            #region Part 4

            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge11, edge12, edge13, edge24, edge33, edge41 });
            filteredGraph = createFilteredGraph(
                vertex => vertex < 4,
                edge => edge.Source != edge.Target);

            Assert.AreSame(edge12, filteredGraph.AdjacentEdge(1, 0));
            Assert.AreSame(edge13, filteredGraph.AdjacentEdge(1, 1));
            Assert.AreSame(edge13, filteredGraph.AdjacentEdge(3, 0));
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            AssertIndexOutOfRange(() => filteredGraph.AdjacentEdge(3, 1));  // Filtered
            Assert.Throws<VertexNotFoundException>(() => filteredGraph.AdjacentEdge(4, 1)); // Filtered
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            #endregion
        }

        protected static void AdjacentEdge_Throws_Test<TGraph>(
            [NotNull] TGraph wrappedGraph,
            [NotNull] Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IImplicitUndirectedGraph<int, Edge<int>>> createFilteredGraph)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>, IMutableGraph<int, Edge<int>>
        {
            #region Part 1

            AdjacentEdge_Throws_ImmutableGraph_Test(
                wrappedGraph,
                () => createFilteredGraph(vertex => true, edge => true));

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
                edge => true);

            AssertIndexOutOfRange(() => filteredGraph.AdjacentEdge(vertex1, 2));
            Assert.Throws<VertexNotFoundException>(() => filteredGraph.AdjacentEdge(vertex3, 0));
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
                vertex => true,
                edge => edge.Source != 1);

            AssertIndexOutOfRange(() => filteredGraph.AdjacentEdge(vertex1, 0));
            AssertIndexOutOfRange(() => filteredGraph.AdjacentEdge(vertex2, 1));
            Assert.Throws<VertexNotFoundException>(() => filteredGraph.AdjacentEdge(vertex5, 0));
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
            Assert.Throws<VertexNotFoundException>(() => filteredGraph.AdjacentEdge(vertex4, 0));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            #endregion
        }

        protected static void AdjacentEdges_Test<TGraph>(
            [NotNull] TGraph wrappedGraph,
            [NotNull] Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IImplicitUndirectedGraph<int, Edge<int>>> createFilteredGraph)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>, IMutableGraph<int, Edge<int>>
        {
            #region Part1

            AdjacentEdges_ImmutableGraph_Test(
                wrappedGraph,
                () => createFilteredGraph(vertex => true, edge => true));

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
                edge => true);

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
                vertex => true,
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
            [NotNull] TGraph wrappedGraph,
            [NotNull] Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IBidirectionalIncidenceGraph<int, Edge<int>>> createFilteredGraph)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>, IMutableGraph<int, Edge<int>>
        {
            #region Part 1

            Degree_ImmutableGraph_Test(
                wrappedGraph,
                () => createFilteredGraph(vertex => true, edge => true));

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
                edge => true);

            Assert.AreEqual(2, filteredGraph.Degree(1));    // Filtered
            Assert.AreEqual(2, filteredGraph.Degree(2));    // Filtered
            Assert.AreEqual(4, filteredGraph.Degree(3));    // Self edge
            Assert.Throws<VertexNotFoundException>(() => filteredGraph.Degree(4));    // Filtered
            Assert.Throws<VertexNotFoundException>(() => filteredGraph.Degree(5));    // Filtered
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            #endregion

            #region Part 3

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });
            wrappedGraph.AddVertex(5);

            filteredGraph = createFilteredGraph(
                vertex => true,
                edge => edge.Source != edge.Target);

            Assert.AreEqual(3, filteredGraph.Degree(1));
            Assert.AreEqual(3, filteredGraph.Degree(2));
            Assert.AreEqual(2, filteredGraph.Degree(3));    // Filtered
            Assert.AreEqual(2, filteredGraph.Degree(4));
            Assert.AreEqual(0, filteredGraph.Degree(5));
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

            Assert.AreEqual(2, filteredGraph.Degree(1));    // Filtered
            Assert.AreEqual(2, filteredGraph.Degree(2));    // Filtered
            Assert.AreEqual(2, filteredGraph.Degree(3));    // Filtered
            Assert.Throws<VertexNotFoundException>(() => filteredGraph.Degree(4));    // Filtered
            Assert.Throws<VertexNotFoundException>(() => filteredGraph.Degree(5));    // Filtered
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            #endregion
        }

        #endregion

        #region Try Get Edges

        protected static void TryGetEdge_Test<TGraph>(
            [NotNull] TGraph wrappedGraph,
            [NotNull] Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IIncidenceGraph<int, Edge<int>>> createFilteredGraph)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>, IMutableGraph<int, Edge<int>>
        {
            #region Part 1

            TryGetEdge_ImmutableGraph_Test(
                wrappedGraph,
                () => createFilteredGraph(vertex => true, edge => true));

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
                edge => true);

            Assert.IsFalse(filteredGraph.TryGetEdge(0, 10, out _));
            Assert.IsFalse(filteredGraph.TryGetEdge(0, 1, out _));

            Assert.IsTrue(filteredGraph.TryGetEdge(2, 4, out Edge<int> gotEdge));
            Assert.AreSame(edge5, gotEdge);

            Assert.IsTrue(filteredGraph.TryGetEdge(2, 2, out gotEdge));
            Assert.AreSame(edge4, gotEdge);

            Assert.IsTrue(filteredGraph.TryGetEdge(1, 2, out gotEdge));
            Assert.AreSame(edge1, gotEdge);

            Assert.IsFalse(filteredGraph.TryGetEdge(2, 1, out _));

            Assert.IsFalse(filteredGraph.TryGetEdge(5, 2, out _));  // Filtered
            Assert.IsFalse(filteredGraph.TryGetEdge(2, 5, out _));  // Filtered

            #endregion

            #region Part 3

            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6, edge7 });

            filteredGraph = createFilteredGraph(
                vertex => true,
                edge => edge.Source != edge.Target);

            Assert.IsFalse(filteredGraph.TryGetEdge(0, 10, out _));
            Assert.IsFalse(filteredGraph.TryGetEdge(0, 1, out _));

            Assert.IsTrue(filteredGraph.TryGetEdge(2, 4, out gotEdge));
            Assert.AreSame(edge5, gotEdge);

            Assert.IsFalse(filteredGraph.TryGetEdge(2, 2, out gotEdge));    // Filtered

            Assert.IsTrue(filteredGraph.TryGetEdge(1, 2, out gotEdge));
            Assert.AreSame(edge1, gotEdge);

            Assert.IsFalse(filteredGraph.TryGetEdge(2, 1, out _));

            Assert.IsTrue(filteredGraph.TryGetEdge(5, 2, out gotEdge));
            Assert.AreSame(edge7, gotEdge);

            #endregion

            #region Part 4

            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6, edge7 });

            filteredGraph = createFilteredGraph(
                vertex => vertex <= 4,
                edge => edge.Source != edge.Target);

            Assert.IsFalse(filteredGraph.TryGetEdge(0, 10, out _));
            Assert.IsFalse(filteredGraph.TryGetEdge(0, 1, out _));

            Assert.IsTrue(filteredGraph.TryGetEdge(2, 4, out gotEdge));
            Assert.AreSame(edge5, gotEdge);

            Assert.IsFalse(filteredGraph.TryGetEdge(2, 2, out _));  // Filtered

            Assert.IsTrue(filteredGraph.TryGetEdge(1, 2, out gotEdge));
            Assert.AreSame(edge1, gotEdge);

            Assert.IsFalse(filteredGraph.TryGetEdge(2, 1, out _));

            Assert.IsFalse(filteredGraph.TryGetEdge(5, 2, out _));  // Filtered
            Assert.IsFalse(filteredGraph.TryGetEdge(2, 5, out _));  // Filtered

            #endregion
        }

        protected static void TryGetEdges_Test<TGraph>(
            [NotNull] TGraph wrappedGraph,
            [NotNull] Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IIncidenceGraph<int, Edge<int>>> createFilteredGraph)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>, IMutableGraph<int, Edge<int>>
        {
            #region Part 1

            TryGetEdges_Test(
                createFilteredGraph(vertex => true, edge => true),
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
                edge => true);

            Assert.IsFalse(filteredGraph.TryGetEdges(0, 10, out _));
            Assert.IsFalse(filteredGraph.TryGetEdges(0, 1, out _));

            Assert.IsTrue(filteredGraph.TryGetEdges(2, 2, out IEnumerable<Edge<int>> gotEdges));
            CollectionAssert.AreEqual(new[] { edge4 }, gotEdges);

            Assert.IsFalse(filteredGraph.TryGetEdges(2, 4, out _)); // Filtered

            Assert.IsTrue(filteredGraph.TryGetEdges(1, 2, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge1, edge2 }, gotEdges);

            Assert.IsTrue(filteredGraph.TryGetEdges(2, 1, out gotEdges));
            CollectionAssert.IsEmpty(gotEdges);

            #endregion

            #region Part 3

            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });

            filteredGraph = createFilteredGraph(
                vertex => true,
                edge => edge.Source != edge.Target);

            Assert.IsFalse(filteredGraph.TryGetEdges(0, 10, out _));
            Assert.IsFalse(filteredGraph.TryGetEdges(0, 1, out _));

            Assert.IsTrue(filteredGraph.TryGetEdges(2, 2, out gotEdges)); // Filtered
            CollectionAssert.IsEmpty(gotEdges);

            Assert.IsTrue(filteredGraph.TryGetEdges(2, 4, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge5 }, gotEdges);

            Assert.IsTrue(filteredGraph.TryGetEdges(1, 2, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge1, edge2 }, gotEdges);

            Assert.IsTrue(filteredGraph.TryGetEdges(2, 1, out gotEdges));
            CollectionAssert.IsEmpty(gotEdges);

            #endregion

            #region Part 4

            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });

            filteredGraph = createFilteredGraph(
                vertex => vertex < 4,
                edge => edge.Source != edge.Target);

            Assert.IsFalse(filteredGraph.TryGetEdges(0, 10, out _));
            Assert.IsFalse(filteredGraph.TryGetEdges(0, 1, out _));

            Assert.IsTrue(filteredGraph.TryGetEdges(2, 2, out gotEdges)); // Filtered
            CollectionAssert.IsEmpty(gotEdges);

            Assert.IsFalse(filteredGraph.TryGetEdges(2, 4, out _)); // Filtered

            Assert.IsTrue(filteredGraph.TryGetEdges(1, 2, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge1, edge2 }, gotEdges);

            Assert.IsTrue(filteredGraph.TryGetEdges(2, 1, out gotEdges));
            CollectionAssert.IsEmpty(gotEdges);

            #endregion
        }

        protected static void TryGetEdge_UndirectedGraph_Test<TGraph>(
            [NotNull] TGraph wrappedGraph,
            [NotNull] Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IImplicitUndirectedGraph<int, Edge<int>>> createFilteredGraph)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>, IMutableGraph<int, Edge<int>>
        {
            #region Part 1

            TryGetEdge_ImmutableGraph_UndirectedGraph_Test(
                wrappedGraph,
                () => createFilteredGraph(vertex => true, edge => true));

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
                edge => true);

            Assert.IsFalse(filteredGraph.TryGetEdge(0, 10, out _));
            Assert.IsFalse(filteredGraph.TryGetEdge(0, 1, out _));

            Assert.IsTrue(filteredGraph.TryGetEdge(2, 4, out Edge<int> gotEdge));
            Assert.AreSame(edge5, gotEdge);

            Assert.IsTrue(filteredGraph.TryGetEdge(2, 2, out gotEdge));
            Assert.AreSame(edge4, gotEdge);

            Assert.IsTrue(filteredGraph.TryGetEdge(1, 2, out gotEdge));
            Assert.AreSame(edge1, gotEdge);

            // 1 -> 2 is present in this undirected graph
            Assert.IsTrue(filteredGraph.TryGetEdge(2, 1, out gotEdge));
            Assert.AreSame(edge1, gotEdge);

            Assert.IsFalse(filteredGraph.TryGetEdge(5, 2, out _));  // Filtered
            Assert.IsFalse(filteredGraph.TryGetEdge(2, 5, out _));  // Filtered

            #endregion

            #region Part 3

            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6, edge7 });

            filteredGraph = createFilteredGraph(
                vertex => true,
                edge => edge.Source != edge.Target);

            Assert.IsFalse(filteredGraph.TryGetEdge(0, 10, out _));
            Assert.IsFalse(filteredGraph.TryGetEdge(0, 1, out _));

            Assert.IsTrue(filteredGraph.TryGetEdge(2, 4, out gotEdge));
            Assert.AreSame(edge5, gotEdge);

            Assert.IsFalse(filteredGraph.TryGetEdge(2, 2, out gotEdge));    // Filtered

            Assert.IsTrue(filteredGraph.TryGetEdge(1, 2, out gotEdge));
            Assert.AreSame(edge1, gotEdge);

            // 1 -> 2 is present in this undirected graph
            Assert.IsTrue(filteredGraph.TryGetEdge(2, 1, out gotEdge));
            Assert.AreSame(edge1, gotEdge);

            Assert.IsTrue(filteredGraph.TryGetEdge(5, 2, out gotEdge));
            Assert.AreSame(edge7, gotEdge);

            Assert.IsTrue(filteredGraph.TryGetEdge(2, 5, out gotEdge));
            Assert.AreSame(edge7, gotEdge);

            #endregion

            #region Part 4

            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6, edge7 });

            filteredGraph = createFilteredGraph(
                vertex => vertex <= 4,
                edge => edge.Source != edge.Target);

            Assert.IsFalse(filteredGraph.TryGetEdge(0, 10, out _));
            Assert.IsFalse(filteredGraph.TryGetEdge(0, 1, out _));

            Assert.IsTrue(filteredGraph.TryGetEdge(2, 4, out gotEdge));
            Assert.AreSame(edge5, gotEdge);

            Assert.IsFalse(filteredGraph.TryGetEdge(2, 2, out _));  // Filtered

            Assert.IsTrue(filteredGraph.TryGetEdge(1, 2, out gotEdge));
            Assert.AreSame(edge1, gotEdge);

            // 1 -> 2 is present in this undirected graph
            Assert.IsTrue(filteredGraph.TryGetEdge(2, 1, out gotEdge));
            Assert.AreSame(edge1, gotEdge);

            Assert.IsFalse(filteredGraph.TryGetEdge(5, 2, out _));  // Filtered
            Assert.IsFalse(filteredGraph.TryGetEdge(2, 5, out _));  // Filtered

            #endregion
        }

        protected static void TryGetOutEdges_Test<TGraph>(
            [NotNull] TGraph wrappedGraph,
            [NotNull] Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IImplicitGraph<int, Edge<int>>> createFilteredGraph)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>, IMutableGraph<int, Edge<int>>
        {
            #region Part 1

            TryGetOutEdges_Test(
                createFilteredGraph(vertex => true, edge => true),
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
                edge => true);

            Assert.IsFalse(filteredGraph.TryGetOutEdges(0, out _));

            Assert.IsFalse(filteredGraph.TryGetOutEdges(5, out _)); // Filtered

            Assert.IsTrue(filteredGraph.TryGetOutEdges(3, out IEnumerable<Edge<int>> gotEdges));
            CollectionAssert.IsEmpty(gotEdges);

            Assert.IsTrue(filteredGraph.TryGetOutEdges(4, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge7 }, gotEdges);   // Filtered

            Assert.IsTrue(filteredGraph.TryGetOutEdges(2, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge4, edge5, edge6 }, gotEdges);

            #endregion

            #region Part 3

            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6, edge7, edge8 });
            filteredGraph = createFilteredGraph(
                vertex => true,
                edge => edge.Source != edge.Target);

            Assert.IsFalse(filteredGraph.TryGetOutEdges(0, out _));

            Assert.IsTrue(filteredGraph.TryGetOutEdges(5, out gotEdges));
            CollectionAssert.IsEmpty(gotEdges);

            Assert.IsTrue(filteredGraph.TryGetOutEdges(3, out gotEdges));
            CollectionAssert.IsEmpty(gotEdges);

            Assert.IsTrue(filteredGraph.TryGetOutEdges(4, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge7, edge8 }, gotEdges);

            Assert.IsTrue(filteredGraph.TryGetOutEdges(2, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge5, edge6 }, gotEdges);   // Filtered

            #endregion

            #region Part 4

            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6, edge7, edge8 });
            filteredGraph = createFilteredGraph(
                vertex => vertex <= 4,
                edge => edge.Source != edge.Target);

            Assert.IsFalse(filteredGraph.TryGetOutEdges(0, out _));

            Assert.IsFalse(filteredGraph.TryGetOutEdges(5, out _)); // Filtered

            Assert.IsTrue(filteredGraph.TryGetOutEdges(3, out gotEdges));
            CollectionAssert.IsEmpty(gotEdges);

            Assert.IsTrue(filteredGraph.TryGetOutEdges(4, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge7 }, gotEdges);   // Filtered

            Assert.IsTrue(filteredGraph.TryGetOutEdges(2, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge5, edge6 }, gotEdges);   // Filtered

            #endregion
        }

        protected static void TryGetInEdges_Test<TGraph>(
            [NotNull] TGraph wrappedGraph,
            [NotNull] Func<VertexPredicate<int>, EdgePredicate<int, Edge<int>>, IBidirectionalIncidenceGraph<int, Edge<int>>> createFilteredGraph)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>, IMutableGraph<int, Edge<int>>
        {
            #region Part 1

            TryGetInEdges_Test(
                createFilteredGraph(vertex => true, edge => true),
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
                edge => true);

            Assert.IsFalse(filteredGraph.TryGetInEdges(0, out _));

            Assert.IsFalse(filteredGraph.TryGetInEdges(5, out _));  // Filtered

            Assert.IsTrue(filteredGraph.TryGetInEdges(4, out IEnumerable<Edge<int>> gotEdges));
            CollectionAssert.AreEqual(new[] { edge5 }, gotEdges);

            Assert.IsTrue(filteredGraph.TryGetInEdges(2, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge1, edge2, edge4 }, gotEdges);

            #endregion

            #region Part 3

            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6, edge7 });
            filteredGraph = createFilteredGraph(
                vertex => true,
                edge => edge.Source != edge.Target);

            Assert.IsFalse(filteredGraph.TryGetInEdges(0, out _));

            Assert.IsTrue(filteredGraph.TryGetInEdges(5, out gotEdges));
            CollectionAssert.IsEmpty(gotEdges);

            Assert.IsTrue(filteredGraph.TryGetInEdges(4, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge5 }, gotEdges);

            Assert.IsTrue(filteredGraph.TryGetInEdges(2, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge1, edge2 }, gotEdges);    // Filtered

            #endregion

            #region Part 4

            wrappedGraph.Clear();
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6, edge7 });
            filteredGraph = createFilteredGraph(
                vertex => vertex <= 4,
                edge => edge.Source != edge.Target);

            Assert.IsFalse(filteredGraph.TryGetInEdges(0, out _));

            Assert.IsFalse(filteredGraph.TryGetInEdges(5, out _));  // Filtered

            Assert.IsTrue(filteredGraph.TryGetInEdges(4, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge5 }, gotEdges);

            Assert.IsTrue(filteredGraph.TryGetInEdges(2, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge1, edge2 }, gotEdges);    // Filtered

            #endregion
        }

        #endregion
    }
}