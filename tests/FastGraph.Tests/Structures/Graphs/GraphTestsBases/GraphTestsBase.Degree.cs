#nullable enable

using JetBrains.Annotations;

namespace FastGraph.Tests.Structures
{
    internal partial class GraphTestsBase
    {
        #region Degree

        protected static void Degree_Test(
            IMutableBidirectionalGraph<int, Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(1, 4);
            var edge4 = new Edge<int>(2, 4);
            var edge5 = new Edge<int>(3, 2);
            var edge6 = new Edge<int>(3, 3);

            graph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });
            graph.AddVertex(5);

            graph.Degree(1).Should().Be(3);
            graph.Degree(2).Should().Be(3);
            graph.Degree(3).Should().Be(4); // Self edge
            graph.Degree(4).Should().Be(2);
            graph.Degree(5).Should().Be(0);
        }

        protected static void Degree_ImmutableGraph_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [InstantHandle] Func<IBidirectionalIncidenceGraph<int, Edge<int>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(1, 4);
            var edge4 = new Edge<int>(2, 4);
            var edge5 = new Edge<int>(3, 2);
            var edge6 = new Edge<int>(3, 3);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });
            wrappedGraph.AddVertex(5);
            IBidirectionalIncidenceGraph<int, Edge<int>> graph = createGraph();

            graph.Degree(1).Should().Be(3);
            graph.Degree(2).Should().Be(3);
            graph.Degree(3).Should().Be(4); // Self edge
            graph.Degree(4).Should().Be(2);
            graph.Degree(5).Should().Be(0);
        }

        protected static void Degree_ImmutableVertices_Test(
            BidirectionalMatrixGraph<Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(1, 4);
            var edge4 = new Edge<int>(2, 4);
            var edge5 = new Edge<int>(3, 2);
            var edge6 = new Edge<int>(3, 3);

            graph.AddEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });

            graph.Degree(0).Should().Be(0);
            graph.Degree(1).Should().Be(3);
            graph.Degree(2).Should().Be(3);
            graph.Degree(3).Should().Be(4); // Self edge
            graph.Degree(4).Should().Be(2);
        }

        protected static void Degree_ImmutableGraph_ReversedTest(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [InstantHandle] Func<IBidirectionalIncidenceGraph<int, SReversedEdge<int, Edge<int>>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(1, 4);
            var edge4 = new Edge<int>(2, 4);
            var edge5 = new Edge<int>(3, 2);
            var edge6 = new Edge<int>(3, 3);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });
            wrappedGraph.AddVertex(5);
            IBidirectionalIncidenceGraph<int, SReversedEdge<int, Edge<int>>> graph = createGraph();

            graph.Degree(1).Should().Be(3);
            graph.Degree(2).Should().Be(3);
            graph.Degree(3).Should().Be(4); // Self edge
            graph.Degree(4).Should().Be(2);
            graph.Degree(5).Should().Be(0);
        }

        protected static void Degree_Throws_Test<TVertex, TEdge>(
            IBidirectionalIncidenceGraph<TVertex, TEdge> graph)
            where TVertex : IEquatable<TVertex>, new()
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8604
            Invoking(() => graph.Degree(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8604
            Invoking(() => graph.Degree(new TVertex())).Should().Throw<VertexNotFoundException>();
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        protected static void Degree_Throws_Matrix_Test<TEdge>(
            BidirectionalMatrixGraph<TEdge> graph)
            where TEdge : class, IEdge<int>
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Invoking(() => graph.Degree(-1)).Should().Throw<VertexNotFoundException>();
            Invoking(() => graph.Degree(10)).Should().Throw<VertexNotFoundException>();
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        #endregion
    }
}
