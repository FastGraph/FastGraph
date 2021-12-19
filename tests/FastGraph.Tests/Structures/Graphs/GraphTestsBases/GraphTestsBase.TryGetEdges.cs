#nullable enable

using JetBrains.Annotations;
using static FastGraph.Tests.GraphTestHelpers;

namespace FastGraph.Tests.Structures
{
    internal partial class GraphTestsBase
    {
        #region Try Get Edges

        protected static void TryGetEdge_Test(
            IIncidenceGraph<int, Edge<int>> graph,
            [InstantHandle] Action<IEnumerable<Edge<int>>> addVerticesAndEdgeRange)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);

            addVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });

            graph.TryGetEdge(0, 10, out _).Should().BeFalse();
            graph.TryGetEdge(0, 1, out _).Should().BeFalse();

            graph.TryGetEdge(2, 4, out Edge<int>? gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge5);

            graph.TryGetEdge(2, 2, out gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge4);

            graph.TryGetEdge(1, 2, out gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge1);

            graph.TryGetEdge(2, 1, out _).Should().BeFalse();
        }

        protected static void TryGetEdge_Test(
            IMutableVertexAndEdgeListGraph<int, Edge<int>> graph)
        {
            TryGetEdge_Test(
                graph,
                edges => graph.AddVerticesAndEdgeRange(edges));
        }

        protected static void TryGetEdge_ImmutableGraph_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [InstantHandle] Func<IIncidenceGraph<int, Edge<int>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });
            IIncidenceGraph<int, Edge<int>> graph = createGraph();

            graph.TryGetEdge(0, 10, out _).Should().BeFalse();
            graph.TryGetEdge(0, 1, out _).Should().BeFalse();

            graph.TryGetEdge(2, 4, out Edge<int>? gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge5);

            graph.TryGetEdge(2, 2, out gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge4);

            graph.TryGetEdge(1, 2, out gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge1);

            graph.TryGetEdge(2, 1, out _).Should().BeFalse();
        }

        protected static void TryGetEdge_ImmutableGraph_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [InstantHandle] Func<IIncidenceGraph<int, SEquatableEdge<int>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });
            IIncidenceGraph<int, SEquatableEdge<int>> graph = createGraph();

            graph.TryGetEdge(0, 10, out _).Should().BeFalse();
            graph.TryGetEdge(0, 1, out _).Should().BeFalse();

            graph.TryGetEdge(2, 4, out SEquatableEdge<int> gotEdge).Should().BeTrue();
            gotEdge.Should().Be(new SEquatableEdge<int>(2, 4));

            graph.TryGetEdge(2, 2, out gotEdge).Should().BeTrue();
            gotEdge.Should().Be(new SEquatableEdge<int>(2, 2));

            graph.TryGetEdge(1, 2, out gotEdge).Should().BeTrue();
            gotEdge.Should().Be(new SEquatableEdge<int>(1, 2));

            graph.TryGetEdge(2, 1, out _).Should().BeFalse();
        }

        protected static void TryGetEdge_ImmutableVertices_Test(
            BidirectionalMatrixGraph<Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 2);
            var edge4 = new Edge<int>(2, 4);
            var edge5 = new Edge<int>(3, 1);

            graph.AddEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5 });

            graph.TryGetEdge(6, 10, out _).Should().BeFalse();
            graph.TryGetEdge(6, 1, out _).Should().BeFalse();

            graph.TryGetEdge(2, 4, out Edge<int>? gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge4);

            graph.TryGetEdge(2, 2, out gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge3);

            graph.TryGetEdge(1, 2, out gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge1);

            graph.TryGetEdge(2, 1, out _).Should().BeFalse();
        }

        protected static void TryGetEdge_ImmutableGraph_ReversedTest(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [InstantHandle] Func<IIncidenceGraph<int, SReversedEdge<int, Edge<int>>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });
            IIncidenceGraph<int, SReversedEdge<int, Edge<int>>> graph = createGraph();

            graph.TryGetEdge(0, 10, out _).Should().BeFalse();
            graph.TryGetEdge(0, 1, out _).Should().BeFalse();

            graph.TryGetEdge(4, 2, out SReversedEdge<int, Edge<int>> gotEdge).Should().BeTrue();
            AssertSameReversedEdge(edge5, gotEdge);

            graph.TryGetEdge(2, 2, out gotEdge).Should().BeTrue();
            AssertSameReversedEdge(edge4, gotEdge);

            graph.TryGetEdge(2, 1, out gotEdge).Should().BeTrue();
            AssertSameReversedEdge(edge1, gotEdge);

            graph.TryGetEdge(1, 2, out _).Should().BeFalse();
        }

        protected static void TryGetEdge_UndirectedGraph_Test(
            IMutableUndirectedGraph<int, Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);
            var edge7 = new Edge<int>(5, 2);

            graph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6, edge7 });

            graph.TryGetEdge(0, 10, out _).Should().BeFalse();
            graph.TryGetEdge(0, 1, out _).Should().BeFalse();

            graph.TryGetEdge(2, 4, out Edge<int>? gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge5);

            graph.TryGetEdge(1, 2, out gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge1);

            graph.TryGetEdge(2, 2, out gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge4);

            // 1 -> 2 is present in this undirected graph
            graph.TryGetEdge(2, 1, out gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge1);

            graph.TryGetEdge(5, 2, out gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge7);

            // 5 -> 2 is present in this undirected graph
            graph.TryGetEdge(2, 5, out gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge7);
        }

        protected static void TryGetEdge_ImmutableGraph_UndirectedGraph_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [InstantHandle] Func<IImplicitUndirectedGraph<int, Edge<int>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);
            var edge7 = new Edge<int>(5, 2);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6, edge7 });
            IImplicitUndirectedGraph<int, Edge<int>> graph = createGraph();

            graph.TryGetEdge(0, 10, out _).Should().BeFalse();
            graph.TryGetEdge(0, 1, out _).Should().BeFalse();

            graph.TryGetEdge(2, 4, out Edge<int>? gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge5);

            graph.TryGetEdge(1, 2, out gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge1);

            graph.TryGetEdge(2, 2, out gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge4);

            // 1 -> 2 is present in this undirected graph
            graph.TryGetEdge(2, 1, out gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge1);

            graph.TryGetEdge(5, 2, out gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge7);

            // 5 -> 2 is present in this undirected graph
            graph.TryGetEdge(2, 5, out gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge7);
        }

        protected static void TryGetEdge_Throws_Test<TVertex, TEdge>(
            IIncidenceGraph<TVertex, TEdge> graph)
            where TVertex : notnull, new()
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8604
            Invoking(() => graph.TryGetEdge(default, new TVertex(), out _)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.TryGetEdge(new TVertex(), default, out _)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.TryGetEdge(default, default, out _)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8604
            // ReSharper restore AssignNullToNotNullAttribute
        }

        protected static void TryGetEdge_Throws_UndirectedGraph_Test<TVertex, TEdge>(
            IImplicitUndirectedGraph<TVertex, TEdge> graph)
            where TVertex : notnull, new()
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8604
            Invoking(() => graph.TryGetEdge(default, new TVertex(), out _)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.TryGetEdge(new TVertex(), default, out _)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.TryGetEdge(default, default, out _)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8604
            // ReSharper restore AssignNullToNotNullAttribute
        }

        protected static void TryGetEdges_Test(
            IIncidenceGraph<int, Edge<int>> graph,
            [InstantHandle] Action<IEnumerable<Edge<int>>> addVerticesAndEdgeRange)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);

            addVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });

            graph.TryGetEdges(0, 10, out _).Should().BeFalse();
            graph.TryGetEdges(0, 1, out _).Should().BeFalse();

            graph.TryGetEdges(2, 2, out IEnumerable<Edge<int>>? gotEdges).Should().BeTrue();
            new[] { edge4 }.Should().BeEquivalentTo(gotEdges);

            graph.TryGetEdges(2, 4, out gotEdges).Should().BeTrue();
            new[] { edge5 }.Should().BeEquivalentTo(gotEdges);

            graph.TryGetEdges(1, 2, out gotEdges).Should().BeTrue();
            new[] { edge1, edge2 }.Should().BeEquivalentTo(gotEdges);

            graph.TryGetEdges(2, 1, out gotEdges).Should().BeTrue();
            gotEdges.Should().BeEmpty();
        }

        protected static void TryGetEdges_Test(
            IMutableVertexAndEdgeListGraph<int, Edge<int>> graph)
        {
            TryGetEdges_Test(
                graph,
                edges => graph.AddVerticesAndEdgeRange(edges));
        }

        protected static void TryGetEdges_ImmutableGraph_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [InstantHandle] Func<IIncidenceGraph<int, Edge<int>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });
            IIncidenceGraph<int, Edge<int>> graph = createGraph();

            graph.TryGetEdges(0, 10, out _).Should().BeFalse();
            graph.TryGetEdges(0, 1, out _).Should().BeFalse();

            graph.TryGetEdges(2, 2, out IEnumerable<Edge<int>>? gotEdges).Should().BeTrue();
            new[] { edge4 }.Should().BeEquivalentTo(gotEdges);

            graph.TryGetEdges(2, 4, out gotEdges).Should().BeTrue();
            new[] { edge5 }.Should().BeEquivalentTo(gotEdges);

            graph.TryGetEdges(1, 2, out gotEdges).Should().BeTrue();
            new[] { edge1, edge2 }.Should().BeEquivalentTo(gotEdges);

            graph.TryGetEdges(2, 1, out gotEdges).Should().BeTrue();
            gotEdges.Should().BeEmpty();
        }

        protected static void TryGetEdges_ImmutableGraph_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [InstantHandle] Func<IIncidenceGraph<int, SEquatableEdge<int>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });
            IIncidenceGraph<int, SEquatableEdge<int>> graph = createGraph();

            graph.TryGetEdges(0, 10, out _).Should().BeFalse();
            graph.TryGetEdges(0, 1, out _).Should().BeFalse();

            graph.TryGetEdges(2, 2, out IEnumerable<SEquatableEdge<int>>? gotEdges).Should().BeTrue();
            new[] { new SEquatableEdge<int>(2, 2) }.Should().BeEquivalentTo(gotEdges);

            graph.TryGetEdges(2, 4, out gotEdges).Should().BeTrue();
            new[] { new SEquatableEdge<int>(2, 4), }.Should().BeEquivalentTo(gotEdges);

            graph.TryGetEdges(1, 2, out gotEdges).Should().BeTrue();
            new[]
            {
                new SEquatableEdge<int>(1, 2),
                new SEquatableEdge<int>(1, 2)
            }.Should().BeEquivalentTo(gotEdges);

            graph.TryGetEdges(2, 1, out gotEdges).Should().BeTrue();
            gotEdges.Should().BeEmpty();
        }

        protected static void TryGetEdges_ImmutableVertices_Test(
            BidirectionalMatrixGraph<Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 2);
            var edge4 = new Edge<int>(2, 4);
            var edge5 = new Edge<int>(3, 1);

            graph.AddEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5 });

            graph.TryGetEdges(6, 10, out _).Should().BeFalse();
            graph.TryGetEdges(6, 1, out _).Should().BeFalse();

            graph.TryGetEdges(2, 2, out IEnumerable<Edge<int>>? gotEdges).Should().BeTrue();
            new[] { edge3 }.Should().BeEquivalentTo(gotEdges);

            graph.TryGetEdges(2, 4, out gotEdges).Should().BeTrue();
            new[] { edge4 }.Should().BeEquivalentTo(gotEdges);

            graph.TryGetEdges(1, 2, out gotEdges).Should().BeTrue();
            new[] { edge1 }.Should().BeEquivalentTo(gotEdges);

            graph.TryGetEdges(2, 1, out gotEdges).Should().BeTrue();
            gotEdges.Should().BeEmpty();
        }

        protected static void TryGetEdges_ImmutableGraph_ReversedTest(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [InstantHandle] Func<IIncidenceGraph<int, SReversedEdge<int, Edge<int>>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });
            IIncidenceGraph<int, SReversedEdge<int, Edge<int>>> graph = createGraph();

            graph.TryGetEdges(0, 10, out _).Should().BeFalse();
            graph.TryGetEdges(0, 1, out _).Should().BeFalse();

            graph.TryGetEdges(2, 2, out IEnumerable<SReversedEdge<int, Edge<int>>>? gotEdges).Should().BeTrue();
            AssertSameReversedEdges(new[] { edge4 }, gotEdges!);

            graph.TryGetEdges(4, 2, out gotEdges).Should().BeTrue();
            AssertSameReversedEdges(new[] { edge5 }, gotEdges!);

            graph.TryGetEdges(2, 1, out gotEdges).Should().BeTrue();
            AssertSameReversedEdges(new[] { edge1, edge2 }, gotEdges!);

            graph.TryGetEdges(1, 2, out gotEdges).Should().BeTrue();
            gotEdges.Should().BeEmpty();
        }

        protected static void TryGetEdges_Throws_Test<TEdge>(
            IIncidenceGraph<TestVertex, TEdge> graph)
            where TEdge : IEdge<TestVertex>
        {
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => graph.TryGetEdges(default, new TestVertex("v2"), out _)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.TryGetEdges(new TestVertex("v1"), default, out _)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.TryGetEdges(default, default, out _)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
        }

        protected static void TryGetOutEdges_Test(
            IImplicitGraph<int, Edge<int>> graph,
            [InstantHandle] Action<IEnumerable<Edge<int>>> addVerticesAndEdgeRange)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);
            var edge7 = new Edge<int>(4, 5);

            addVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6, edge7 });

            graph.TryGetOutEdges(0, out _).Should().BeFalse();

            graph.TryGetOutEdges(5, out IEnumerable<Edge<int>>? gotEdges).Should().BeTrue();
            gotEdges.Should().BeEmpty();

            graph.TryGetOutEdges(3, out gotEdges).Should().BeTrue();
            new[] { edge6 }.Should().BeEquivalentTo(gotEdges);

            graph.TryGetOutEdges(1, out gotEdges).Should().BeTrue();
            new[] { edge1, edge2, edge3 }.Should().BeEquivalentTo(gotEdges);
        }

        protected static void TryGetOutEdges_Test(
            IMutableVertexAndEdgeListGraph<int, Edge<int>> graph)
        {
            TryGetOutEdges_Test(
                graph,
                edges => graph.AddVerticesAndEdgeRange(edges));
        }

        protected static void TryGetOutEdges_ImmutableGraph_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [InstantHandle] Func<IImplicitGraph<int, Edge<int>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);
            var edge7 = new Edge<int>(4, 5);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6, edge7 });
            IImplicitGraph<int, Edge<int>> graph = createGraph();

            graph.TryGetOutEdges(0, out _).Should().BeFalse();

            graph.TryGetOutEdges(5, out IEnumerable<Edge<int>>? gotEdges).Should().BeTrue();
            gotEdges.Should().BeEmpty();

            graph.TryGetOutEdges(3, out gotEdges).Should().BeTrue();
            new[] { edge6 }.Should().BeEquivalentTo(gotEdges);

            graph.TryGetOutEdges(1, out gotEdges).Should().BeTrue();
            new[] { edge1, edge2, edge3 }.Should().BeEquivalentTo(gotEdges);
        }

        protected static void TryGetOutEdges_ImmutableGraph_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [InstantHandle] Func<IImplicitGraph<int, SEquatableEdge<int>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);
            var edge7 = new Edge<int>(4, 5);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6, edge7 });
            IImplicitGraph<int, SEquatableEdge<int>> graph = createGraph();

            graph.TryGetOutEdges(0, out _).Should().BeFalse();

            graph.TryGetOutEdges(5, out IEnumerable<SEquatableEdge<int>>? gotEdges).Should().BeTrue();
            gotEdges.Should().BeEmpty();

            graph.TryGetOutEdges(3, out gotEdges).Should().BeTrue();
            new[] { new SEquatableEdge<int>(3, 1) }.Should().BeEquivalentTo(gotEdges);

            graph.TryGetOutEdges(1, out gotEdges).Should().BeTrue();
            new[]
            {
                new SEquatableEdge<int>(1, 2),
                new SEquatableEdge<int>(1, 2),
                new SEquatableEdge<int>(1, 3)
            }.Should().BeEquivalentTo(gotEdges);
        }

        protected static void TryGetOutEdges_ImmutableVertices_Test(
            BidirectionalMatrixGraph<Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 2);
            var edge4 = new Edge<int>(2, 4);
            var edge5 = new Edge<int>(3, 1);
            var edge6 = new Edge<int>(4, 5);

            graph.AddEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });

            graph.TryGetOutEdges(6, out _).Should().BeFalse();

            graph.TryGetOutEdges(5, out IEnumerable<Edge<int>>? gotEdges).Should().BeTrue();
            gotEdges.Should().BeEmpty();

            graph.TryGetOutEdges(3, out gotEdges).Should().BeTrue();
            new[] { edge5 }.Should().BeEquivalentTo(gotEdges);

            graph.TryGetOutEdges(1, out gotEdges).Should().BeTrue();
            new[] { edge1, edge2 }.Should().BeEquivalentTo(gotEdges);
        }

        protected static void TryGetOutEdges_ImmutableGraph_ReversedTest(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [InstantHandle] Func<IImplicitGraph<int, SReversedEdge<int, Edge<int>>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);
            var edge7 = new Edge<int>(5, 4);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6, edge7 });
            IImplicitGraph<int, SReversedEdge<int, Edge<int>>> graph = createGraph();

            graph.TryGetOutEdges(0, out _).Should().BeFalse();

            graph.TryGetOutEdges(5, out IEnumerable<SReversedEdge<int, Edge<int>>>? gotEdges).Should().BeTrue();
            gotEdges.Should().BeEmpty();

            graph.TryGetOutEdges(3, out gotEdges).Should().BeTrue();
            AssertSameReversedEdges(new[] { edge3 }, gotEdges!);

            graph.TryGetOutEdges(2, out gotEdges).Should().BeTrue();
            AssertSameReversedEdges(new[] { edge1, edge2, edge4 }, gotEdges!);
        }

        protected static void TryGetOutEdges_Throws_Test<TVertex, TEdge>(
            IImplicitGraph<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8604
            Invoking(() => graph.TryGetOutEdges(default, out _)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8604
        }

        protected static void TryGetInEdges_Test(
            IBidirectionalIncidenceGraph<int, Edge<int>> graph,
            [InstantHandle] Action<IEnumerable<Edge<int>>> addVerticesAndEdgeRange)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);
            var edge7 = new Edge<int>(5, 3);

            addVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6, edge7 });

            graph.TryGetInEdges(0, out _).Should().BeFalse();

            graph.TryGetInEdges(5, out IEnumerable<Edge<int>>? gotEdges).Should().BeTrue();
            gotEdges.Should().BeEmpty();

            graph.TryGetInEdges(4, out gotEdges).Should().BeTrue();
            new[] { edge5 }.Should().BeEquivalentTo(gotEdges);

            graph.TryGetInEdges(2, out gotEdges).Should().BeTrue();
            new[] { edge1, edge2, edge4 }.Should().BeEquivalentTo(gotEdges);
        }

        protected static void TryGetInEdges_Test(
            IMutableBidirectionalGraph<int, Edge<int>> graph)
        {
            TryGetInEdges_Test(
                graph,
                edges => graph.AddVerticesAndEdgeRange(edges));
        }

        protected static void TryGetInEdges_ImmutableGraph_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [InstantHandle] Func<IBidirectionalIncidenceGraph<int, Edge<int>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);
            var edge7 = new Edge<int>(5, 3);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6, edge7 });
            IBidirectionalIncidenceGraph<int, Edge<int>> graph = createGraph();

            graph.TryGetInEdges(0, out _).Should().BeFalse();

            graph.TryGetInEdges(5, out IEnumerable<Edge<int>>? gotEdges).Should().BeTrue();
            gotEdges.Should().BeEmpty();

            graph.TryGetInEdges(4, out gotEdges).Should().BeTrue();
            new[] { edge5 }.Should().BeEquivalentTo(gotEdges);

            graph.TryGetInEdges(2, out gotEdges).Should().BeTrue();
            new[] { edge1, edge2, edge4 }.Should().BeEquivalentTo(gotEdges);
        }

        protected static void TryGetInEdges_ImmutableVertices_Test(
            BidirectionalMatrixGraph<Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 2);
            var edge4 = new Edge<int>(2, 4);
            var edge5 = new Edge<int>(3, 1);
            var edge6 = new Edge<int>(5, 3);

            graph.AddEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });

            graph.TryGetInEdges(6, out _).Should().BeFalse();

            graph.TryGetInEdges(5, out IEnumerable<Edge<int>>? gotEdges).Should().BeTrue();
            gotEdges.Should().BeEmpty();

            graph.TryGetInEdges(4, out gotEdges).Should().BeTrue();
            new[] { edge4 }.Should().BeEquivalentTo(gotEdges);

            graph.TryGetInEdges(2, out gotEdges).Should().BeTrue();
            new[] { edge1, edge3 }.Should().BeEquivalentTo(gotEdges);
        }

        protected static void TryGetInEdges_ImmutableGraph_ReversedTest(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [InstantHandle] Func<IBidirectionalIncidenceGraph<int, SReversedEdge<int, Edge<int>>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(1, 1);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);
            var edge7 = new Edge<int>(4, 5);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6, edge7 });
            IBidirectionalIncidenceGraph<int, SReversedEdge<int, Edge<int>>> graph = createGraph();

            graph.TryGetInEdges(0, out _).Should().BeFalse();

            graph.TryGetInEdges(5, out IEnumerable<SReversedEdge<int, Edge<int>>>? gotEdges).Should().BeTrue();
            gotEdges.Should().BeEmpty();

            graph.TryGetInEdges(4, out gotEdges).Should().BeTrue();
            AssertSameReversedEdges(new[] { edge7 }, gotEdges!);

            graph.TryGetInEdges(1, out gotEdges).Should().BeTrue();
            AssertSameReversedEdges(new[] { edge1, edge2, edge3, edge4 }, gotEdges!);
        }

        protected static void TryGetInEdges_Throws_Test<TVertex, TEdge>(
            IBidirectionalIncidenceGraph<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8604
            Invoking(() => graph.TryGetInEdges(default, out _)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8604
        }

        #endregion
    }
}
