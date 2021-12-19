#nullable enable

using JetBrains.Annotations;
using static FastGraph.Tests.AssertHelpers;
using static FastGraph.Tests.GraphTestHelpers;

namespace FastGraph.Tests.Structures
{
    internal partial class GraphTestsBase
    {
        #region Out Edges

        protected static void OutEdge_Test(
            IImplicitGraph<int, Edge<int>> graph,
            [InstantHandle] Action<IEnumerable<Edge<int>>> addVerticesAndEdgeRange)
        {
            var edge11 = new Edge<int>(1, 1);
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge24 = new Edge<int>(2, 4);
            var edge33 = new Edge<int>(3, 3);
            var edge41 = new Edge<int>(4, 1);

            addVerticesAndEdgeRange(new[] { edge11, edge12, edge13, edge24, edge33, edge41 });

            graph.OutEdge(1, 0).Should().BeSameAs(edge11);
            graph.OutEdge(1, 2).Should().BeSameAs(edge13);
            graph.OutEdge(2, 0).Should().BeSameAs(edge24);
            graph.OutEdge(3, 0).Should().BeSameAs(edge33);
            graph.OutEdge(4, 0).Should().BeSameAs(edge41);
        }

        protected static void OutEdge_Test(
            IMutableVertexAndEdgeListGraph<int, Edge<int>> graph)
        {
            OutEdge_Test(
                graph,
                edges => graph.AddVerticesAndEdgeRange(edges));
        }

        protected static void OutEdge_ImmutableGraph_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [InstantHandle] Func<IImplicitGraph<int, Edge<int>>> createGraph)
        {
            var edge11 = new Edge<int>(1, 1);
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge24 = new Edge<int>(2, 4);
            var edge33 = new Edge<int>(3, 3);
            var edge41 = new Edge<int>(4, 1);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge11, edge12, edge13, edge24, edge33, edge41 });
            IImplicitGraph<int, Edge<int>> graph = createGraph();

            graph.OutEdge(1, 0).Should().BeSameAs(edge11);
            graph.OutEdge(1, 2).Should().BeSameAs(edge13);
            graph.OutEdge(2, 0).Should().BeSameAs(edge24);
            graph.OutEdge(3, 0).Should().BeSameAs(edge33);
            graph.OutEdge(4, 0).Should().BeSameAs(edge41);
        }

        protected static void OutEdge_ImmutableGraph_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [InstantHandle] Func<IImplicitGraph<int, SEquatableEdge<int>>> createGraph)
        {
            var edge11 = new Edge<int>(1, 1);
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge24 = new Edge<int>(2, 4);
            var edge33 = new Edge<int>(3, 3);
            var edge41 = new Edge<int>(4, 1);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge11, edge12, edge13, edge24, edge33, edge41 });
            IImplicitGraph<int, SEquatableEdge<int>> graph = createGraph();

            graph.OutEdge(1, 0).Should().Be(new SEquatableEdge<int>(1, 1));
            graph.OutEdge(1, 2).Should().Be(new SEquatableEdge<int>(1, 3));
            graph.OutEdge(2, 0).Should().Be(new SEquatableEdge<int>(2, 4));
            graph.OutEdge(3, 0).Should().Be(new SEquatableEdge<int>(3, 3));
            graph.OutEdge(4, 0).Should().Be(new SEquatableEdge<int>(4, 1));
        }

        protected static void OutEdge_ImmutableVertices_Test(
            BidirectionalMatrixGraph<Edge<int>> graph)
        {
            var edge11 = new Edge<int>(1, 1);
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge24 = new Edge<int>(2, 4);
            var edge33 = new Edge<int>(3, 3);
            var edge41 = new Edge<int>(4, 1);

            graph.AddEdgeRange(new[] { edge11, edge12, edge13, edge24, edge33, edge41 });

            graph.OutEdge(1, 0).Should().BeSameAs(edge11);
            graph.OutEdge(1, 2).Should().BeSameAs(edge13);
            graph.OutEdge(2, 0).Should().BeSameAs(edge24);
            graph.OutEdge(3, 0).Should().BeSameAs(edge33);
            graph.OutEdge(4, 0).Should().BeSameAs(edge41);
        }

        protected static void OutEdge_ImmutableGraph_ReversedTest(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [InstantHandle] Func<IImplicitGraph<int, SReversedEdge<int, Edge<int>>>> createGraph)
        {
            var edge11 = new Edge<int>(1, 1);
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge21 = new Edge<int>(2, 1);
            var edge24 = new Edge<int>(2, 4);
            var edge33 = new Edge<int>(3, 3);
            var edge41 = new Edge<int>(4, 1);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge11, edge12, edge13, edge21, edge24, edge33, edge41 });
            IImplicitGraph<int, SReversedEdge<int, Edge<int>>> graph = createGraph();

            AssertSameReversedEdge(edge11, graph.OutEdge(1, 0));
            AssertSameReversedEdge(edge41, graph.OutEdge(1, 2));
            AssertSameReversedEdge(edge12, graph.OutEdge(2, 0));
            AssertSameReversedEdge(edge13, graph.OutEdge(3, 0));
            AssertSameReversedEdge(edge24, graph.OutEdge(4, 0));
        }

        protected static void OutEdge_NullThrows_Test<TVertex, TEdge>(
            IImplicitGraph<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8604
            Invoking(() => graph.OutEdge(default, 0)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8604
        }

        protected static void OutEdge_Throws_Test(
            IImplicitGraph<int, Edge<int>> graph,
            [InstantHandle] Action<int> addVertex,
            [InstantHandle] Action<Edge<int>> addEdge)
        {
            const int vertex1 = 1;
            const int vertex2 = 2;

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Invoking(() => graph.OutEdge(vertex1, 0)).Should().Throw<VertexNotFoundException>();

            addVertex(vertex1);
            addVertex(vertex2);
            AssertIndexOutOfRange(() => graph.OutEdge(vertex1, 0));

            addEdge(new Edge<int>(1, 2));
            AssertIndexOutOfRange(() => graph.OutEdge(vertex1, 5));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        protected static void OutEdge_Throws_Test(
            IMutableVertexAndEdgeListGraph<int, Edge<int>> graph)
        {
            OutEdge_Throws_Test(
                graph,
                vertex => graph.AddVertex(vertex),
                edge => graph.AddEdge(edge));
        }

        protected static void OutEdge_Throws_ImmutableGraph_Test<TEdge>(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [InstantHandle] Func<IImplicitGraph<int, TEdge>> createGraph)
            where TEdge : IEdge<int>
        {
            IImplicitGraph<int, TEdge> graph = createGraph();

            const int vertex1 = 1;
            const int vertex2 = 2;

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Invoking(() => graph.OutEdge(vertex1, 0)).Should().Throw<VertexNotFoundException>();

            wrappedGraph.AddVertex(vertex1);
            wrappedGraph.AddVertex(vertex2);
            graph = createGraph();
            AssertIndexOutOfRange(() => graph.OutEdge(vertex1, 0));

            wrappedGraph.AddEdge(new Edge<int>(1, 2));
            graph = createGraph();
            AssertIndexOutOfRange(() => graph.OutEdge(vertex1, 5));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        protected static void OutEdge_Throws_ImmutableVertices_Test(
            BidirectionalMatrixGraph<Edge<int>> graph)
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Invoking(() => graph.OutEdge(-1, 0)).Should().Throw<VertexNotFoundException>();
            Invoking(() => graph.OutEdge(4, 0)).Should().Throw<VertexNotFoundException>();

            graph.AddEdge(new Edge<int>(1, 2));
            AssertIndexOutOfRange(() => graph.OutEdge(1, 5));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        protected static void OutEdge_Throws_ImmutableGraph_ReversedTest(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [InstantHandle] Func<IImplicitGraph<int, SReversedEdge<int, Edge<int>>>> createGraph)
        {
            IImplicitGraph<int, SReversedEdge<int, Edge<int>>> graph = createGraph();

            const int vertex1 = 1;
            const int vertex2 = 2;

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Invoking(() => graph.OutEdge(vertex1, 0)).Should().Throw<VertexNotFoundException>();

            wrappedGraph.AddVertex(vertex1);
            wrappedGraph.AddVertex(vertex2);
            graph = createGraph();
            AssertIndexOutOfRange(() => graph.OutEdge(vertex1, 0));

            wrappedGraph.AddEdge(new Edge<int>(1, 2));
            graph = createGraph();
            AssertIndexOutOfRange(() => graph.OutEdge(vertex1, 5));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        protected static void OutEdges_Test(
            IImplicitGraph<int, Edge<int>> graph,
            [InstantHandle] Action<int> addVertex,
            [InstantHandle] Action<IEnumerable<Edge<int>>> addVerticesAndEdgeRange)
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);

            addVertex(1);
            AssertNoOutEdge(graph, 1);

            addVerticesAndEdgeRange(new[] { edge12, edge13, edge14, edge24, edge31, edge33 });

            AssertHasOutEdges(graph, 1, new[] { edge12, edge13, edge14 });
            AssertHasOutEdges(graph, 2, new[] { edge24 });
            AssertHasOutEdges(graph, 3, new[] { edge31, edge33 });
            AssertNoOutEdge(graph, 4);
        }

        protected static void OutEdges_Test(
            IMutableVertexAndEdgeListGraph<int, Edge<int>> graph)
        {
            OutEdges_Test(
                graph,
                vertex => graph.AddVertex(vertex),
                edges => graph.AddVerticesAndEdgeRange(edges));
        }

        protected static void OutEdges_ImmutableGraph_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [InstantHandle] Func<IImplicitGraph<int, Edge<int>>> createGraph)
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);

            wrappedGraph.AddVertex(1);
            IImplicitGraph<int, Edge<int>> graph = createGraph();
            AssertNoOutEdge(graph, 1);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge14, edge24, edge31, edge33 });
            graph = createGraph();

            AssertHasOutEdges(graph, 1, new[] { edge12, edge13, edge14 });
            AssertHasOutEdges(graph, 2, new[] { edge24 });
            AssertHasOutEdges(graph, 3, new[] { edge31, edge33 });
            AssertNoOutEdge(graph, 4);
        }

        protected static void OutEdges_ImmutableGraph_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [InstantHandle] Func<IImplicitGraph<int, SEquatableEdge<int>>> createGraph)
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);

            wrappedGraph.AddVertex(1);
            IImplicitGraph<int, SEquatableEdge<int>> graph = createGraph();
            AssertNoOutEdge(graph, 1);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge14, edge24, edge31, edge33 });
            graph = createGraph();

            AssertHasOutEdges(
                graph,
                1,
                new[]
                {
                    new SEquatableEdge<int>(1, 2),
                    new SEquatableEdge<int>(1, 3),
                    new SEquatableEdge<int>(1, 4)
                });
            AssertHasOutEdges(
                graph,
                2,
                new[] { new SEquatableEdge<int>(2, 4) });
            AssertHasOutEdges(
                graph,
                3,
                new[]
                {
                    new SEquatableEdge<int>(3, 1),
                    new SEquatableEdge<int>(3, 3)
                });
            AssertNoOutEdge(graph, 4);
        }

        protected static void OutEdges_ImmutableVertices_Test(
            BidirectionalMatrixGraph<Edge<int>> graph)
        {
            var edge01 = new Edge<int>(0, 1);
            var edge02 = new Edge<int>(0, 2);
            var edge03 = new Edge<int>(0, 3);
            var edge13 = new Edge<int>(1, 3);
            var edge20 = new Edge<int>(2, 0);
            var edge22 = new Edge<int>(2, 2);

            AssertNoOutEdge(graph, 1);

            graph.AddEdgeRange(new[] { edge01, edge02, edge03, edge13, edge20, edge22 });

            AssertHasOutEdges(graph, 0, new[] { edge01, edge02, edge03 });
            AssertHasOutEdges(graph, 1, new[] { edge13 });
            AssertHasOutEdges(graph, 2, new[] { edge20, edge22 });
            AssertNoOutEdge(graph, 3);
        }

        protected static void OutEdges_ImmutableGraph_ReversedTest(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [InstantHandle] Func<IImplicitGraph<int, SReversedEdge<int, Edge<int>>>> createGraph)
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge33 = new Edge<int>(3, 3);
            var edge34 = new Edge<int>(3, 4);

            wrappedGraph.AddVertex(1);
            IImplicitGraph<int, SReversedEdge<int, Edge<int>>> graph = createGraph();
            AssertNoOutEdge(graph, 1);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge14, edge24, edge33, edge34 });
            graph = createGraph();

            AssertNoOutEdge(graph, 1);
            AssertHasReversedOutEdges(graph, 2, new[] { edge12 });
            AssertHasReversedOutEdges(graph, 3, new[] { edge13, edge33 });
            AssertHasReversedOutEdges(graph, 4, new[] { edge14, edge24, edge34 });
        }

        protected static void OutEdges_NullThrows_Test<TVertex, TEdge>(
            IImplicitGraph<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8604
            Invoking(() => graph.IsOutEdgesEmpty(default)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.OutDegree(default)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.OutEdges(default).ToArray()).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8604
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        protected static void OutEdges_Throws_Test<TVertex, TEdge>(
            IImplicitGraph<TVertex, TEdge> graph)
            where TVertex : IEquatable<TVertex>, new()
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            var vertex = new TVertex();
            Invoking(() => graph.IsOutEdgesEmpty(vertex)).Should().Throw<VertexNotFoundException>();
            Invoking(() => graph.OutDegree(vertex)).Should().Throw<VertexNotFoundException>();
            Invoking(() => graph.OutEdges(vertex).ToArray()).Should().Throw<VertexNotFoundException>();
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        protected static void OutEdges_Throws_Matrix_Test<TEdge>(
            BidirectionalMatrixGraph<TEdge> graph)
            where TEdge : class, IEdge<int>
        {
            const int vertex = 10;
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Invoking(() => graph.IsOutEdgesEmpty(vertex)).Should().Throw<VertexNotFoundException>();
            Invoking(() => graph.OutDegree(vertex)).Should().Throw<VertexNotFoundException>();
            Invoking(() => graph.OutEdges(vertex).ToArray()).Should().Throw<VertexNotFoundException>();
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        #endregion
    }
}
