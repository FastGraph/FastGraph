#nullable enable

using JetBrains.Annotations;
using static FastGraph.Tests.AssertHelpers;
using static FastGraph.Tests.GraphTestHelpers;

namespace FastGraph.Tests.Structures
{
    internal partial class GraphTestsBase
    {
        #region Adjacent Edges

        protected static void AdjacentEdge_Test(
            IMutableUndirectedGraph<int, Edge<int>> graph)
        {
            var edge11 = new Edge<int>(1, 1);
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge24 = new Edge<int>(2, 4);
            var edge33 = new Edge<int>(3, 3);
            var edge41 = new Edge<int>(4, 1);

            graph.AddVerticesAndEdgeRange(new[] { edge11, edge12, edge13, edge24, edge33, edge41 });

            graph.AdjacentEdge(1, 0).Should().BeSameAs(edge11);
            graph.AdjacentEdge(1, 2).Should().BeSameAs(edge13);
            graph.AdjacentEdge(1, 3).Should().BeSameAs(edge41);
            graph.AdjacentEdge(3, 0).Should().BeSameAs(edge13);
            graph.AdjacentEdge(3, 1).Should().BeSameAs(edge33);
            graph.AdjacentEdge(4, 0).Should().BeSameAs(edge24);
        }

        protected static void AdjacentEdge_ImmutableGraph_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [InstantHandle] Func<IImplicitUndirectedGraph<int, Edge<int>>> createGraph)
        {
            var edge11 = new Edge<int>(1, 1);
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge24 = new Edge<int>(2, 4);
            var edge33 = new Edge<int>(3, 3);
            var edge41 = new Edge<int>(4, 1);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge11, edge12, edge13, edge24, edge33, edge41 });
            IImplicitUndirectedGraph<int, Edge<int>> graph = createGraph();

            graph.AdjacentEdge(1, 0).Should().BeSameAs(edge11);
            graph.AdjacentEdge(1, 2).Should().BeSameAs(edge13);
            graph.AdjacentEdge(1, 3).Should().BeSameAs(edge41);
            graph.AdjacentEdge(3, 0).Should().BeSameAs(edge13);
            graph.AdjacentEdge(3, 1).Should().BeSameAs(edge33);
            graph.AdjacentEdge(4, 0).Should().BeSameAs(edge24);
        }

        protected static void AdjacentEdge_NullThrows_Test<TVertex, TEdge>(
            IImplicitUndirectedGraph<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8604
            Invoking(() => graph.AdjacentEdge(default, 0)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8604
        }

        protected static void AdjacentEdge_Throws_Test(
            IMutableUndirectedGraph<int, Edge<int>> graph)
        {
            const int vertex1 = 1;
            const int vertex2 = 2;

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Invoking(() => graph.AdjacentEdge(vertex1, 0)).Should().Throw<VertexNotFoundException>();

            graph.AddVertex(vertex1);
            graph.AddVertex(vertex2);
            AssertIndexOutOfRange(() => graph.AdjacentEdge(vertex1, 0));

            graph.AddEdge(new Edge<int>(1, 2));
            AssertIndexOutOfRange(() => graph.AdjacentEdge(vertex1, 5));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        protected static void AdjacentEdge_Throws_ImmutableGraph_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [InstantHandle] Func<IImplicitUndirectedGraph<int, Edge<int>>> createGraph)
        {
            const int vertex1 = 1;
            const int vertex2 = 2;

            IImplicitUndirectedGraph<int, Edge<int>> graph = createGraph();
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Invoking(() => graph.AdjacentEdge(vertex1, 0)).Should().Throw<VertexNotFoundException>();

            wrappedGraph.AddVertex(vertex1);
            wrappedGraph.AddVertex(vertex2);
            graph = createGraph();
            AssertIndexOutOfRange(() => graph.AdjacentEdge(vertex1, 0));

            wrappedGraph.AddEdge(new Edge<int>(1, 2));
            graph = createGraph();
            AssertIndexOutOfRange(() => graph.AdjacentEdge(vertex1, 5));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        protected static void AdjacentEdges_Test(
            IMutableUndirectedGraph<int, Edge<int>> graph)
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);

            graph.AddVertex(1);
            AssertNoAdjacentEdge(graph, 1);

            graph.AddVertex(5);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge14, edge24, edge31, edge33 });

            AssertHasAdjacentEdges(graph, 1, new[] { edge12, edge13, edge14, edge31 });
            AssertHasAdjacentEdges(graph, 2, new[] { edge12, edge24 });
            AssertHasAdjacentEdges(graph, 3, new[] { edge13, edge31, edge33 }, 4);  // Has self edge counting twice
            AssertHasAdjacentEdges(graph, 4, new[] { edge14, edge24 });
            AssertNoAdjacentEdge(graph, 5);
        }

        protected static void AdjacentEdges_ImmutableGraph_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [InstantHandle] Func<IImplicitUndirectedGraph<int, Edge<int>>> createGraph)
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);

            wrappedGraph.AddVertex(1);
            IImplicitUndirectedGraph<int, Edge<int>> graph = createGraph();
            AssertNoAdjacentEdge(graph, 1);

            wrappedGraph.AddVertex(5);
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge14, edge24, edge31, edge33 });
            graph = createGraph();

            AssertHasAdjacentEdges(graph, 1, new[] { edge12, edge13, edge14, edge31 });
            AssertHasAdjacentEdges(graph, 2, new[] { edge12, edge24 });
            AssertHasAdjacentEdges(graph, 3, new[] { edge13, edge31, edge33 }, 4);  // Has self edge counting twice
            AssertHasAdjacentEdges(graph, 4, new[] { edge14, edge24 });
            AssertNoAdjacentEdge(graph, 5);
        }

        protected static void AdjacentEdges_NullThrows_Test<TVertex, TEdge>(
            IImplicitUndirectedGraph<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8604
            Invoking(() => graph.IsAdjacentEdgesEmpty(default)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.AdjacentDegree(default)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.AdjacentEdges(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8604
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        protected static void AdjacentEdges_Throws_Test<TVertex>(
            IImplicitUndirectedGraph<TVertex, Edge<TVertex>> graph)
            where TVertex : IEquatable<TVertex>, new()
        {
            AdjacentEdges_NullThrows_Test(graph);

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            var vertex = new TVertex();
            Invoking(() => graph.IsAdjacentEdgesEmpty(vertex)).Should().Throw<VertexNotFoundException>();
            Invoking(() => graph.AdjacentDegree(vertex)).Should().Throw<VertexNotFoundException>();
            Invoking(() => graph.AdjacentEdges(vertex)).Should().Throw<VertexNotFoundException>();
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        #endregion
    }
}
