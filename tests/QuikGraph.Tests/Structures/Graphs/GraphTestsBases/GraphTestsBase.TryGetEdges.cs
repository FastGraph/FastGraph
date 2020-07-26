using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using static QuikGraph.Tests.GraphTestHelpers;

namespace QuikGraph.Tests.Structures
{
    internal partial class GraphTestsBase
    {
        #region Try Get Edges

        protected static void TryGetEdge_Test(
            [NotNull] IIncidenceGraph<int, Edge<int>> graph,
            [NotNull, InstantHandle] Action<IEnumerable<Edge<int>>> addVerticesAndEdgeRange)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);

            addVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });

            Assert.IsFalse(graph.TryGetEdge(0, 10, out _));
            Assert.IsFalse(graph.TryGetEdge(0, 1, out _));

            Assert.IsTrue(graph.TryGetEdge(2, 4, out Edge<int> gotEdge));
            Assert.AreSame(edge5, gotEdge);

            Assert.IsTrue(graph.TryGetEdge(2, 2, out gotEdge));
            Assert.AreSame(edge4, gotEdge);

            Assert.IsTrue(graph.TryGetEdge(1, 2, out gotEdge));
            Assert.AreSame(edge1, gotEdge);

            Assert.IsFalse(graph.TryGetEdge(2, 1, out _));
        }

        protected static void TryGetEdge_Test(
            [NotNull] IMutableVertexAndEdgeListGraph<int, Edge<int>> graph)
        {
            TryGetEdge_Test(
                graph,
                edges => graph.AddVerticesAndEdgeRange(edges));
        }

        protected static void TryGetEdge_ImmutableGraph_Test(
            [NotNull] IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [NotNull, InstantHandle] Func<IIncidenceGraph<int, Edge<int>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });
            IIncidenceGraph<int, Edge<int>> graph = createGraph();

            Assert.IsFalse(graph.TryGetEdge(0, 10, out _));
            Assert.IsFalse(graph.TryGetEdge(0, 1, out _));

            Assert.IsTrue(graph.TryGetEdge(2, 4, out Edge<int> gotEdge));
            Assert.AreSame(edge5, gotEdge);

            Assert.IsTrue(graph.TryGetEdge(2, 2, out gotEdge));
            Assert.AreSame(edge4, gotEdge);

            Assert.IsTrue(graph.TryGetEdge(1, 2, out gotEdge));
            Assert.AreSame(edge1, gotEdge);

            Assert.IsFalse(graph.TryGetEdge(2, 1, out _));
        }

        protected static void TryGetEdge_ImmutableGraph_Test(
            [NotNull] IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [NotNull, InstantHandle] Func<IIncidenceGraph<int, SEquatableEdge<int>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });
            IIncidenceGraph<int, SEquatableEdge<int>> graph = createGraph();

            Assert.IsFalse(graph.TryGetEdge(0, 10, out _));
            Assert.IsFalse(graph.TryGetEdge(0, 1, out _));

            Assert.IsTrue(graph.TryGetEdge(2, 4, out SEquatableEdge<int> gotEdge));
            Assert.AreEqual(new SEquatableEdge<int>(2, 4), gotEdge);

            Assert.IsTrue(graph.TryGetEdge(2, 2, out gotEdge));
            Assert.AreEqual(new SEquatableEdge<int>(2, 2), gotEdge);

            Assert.IsTrue(graph.TryGetEdge(1, 2, out gotEdge));
            Assert.AreEqual(new SEquatableEdge<int>(1, 2), gotEdge);

            Assert.IsFalse(graph.TryGetEdge(2, 1, out _));
        }

        protected static void TryGetEdge_ImmutableVertices_Test(
            [NotNull] BidirectionalMatrixGraph<Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 2);
            var edge4 = new Edge<int>(2, 4);
            var edge5 = new Edge<int>(3, 1);

            graph.AddEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5 });

            Assert.IsFalse(graph.TryGetEdge(6, 10, out _));
            Assert.IsFalse(graph.TryGetEdge(6, 1, out _));

            Assert.IsTrue(graph.TryGetEdge(2, 4, out Edge<int> gotEdge));
            Assert.AreSame(edge4, gotEdge);

            Assert.IsTrue(graph.TryGetEdge(2, 2, out gotEdge));
            Assert.AreSame(edge3, gotEdge);

            Assert.IsTrue(graph.TryGetEdge(1, 2, out gotEdge));
            Assert.AreSame(edge1, gotEdge);

            Assert.IsFalse(graph.TryGetEdge(2, 1, out _));
        }

        protected static void TryGetEdge_ImmutableGraph_ReversedTest(
            [NotNull] IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [NotNull, InstantHandle] Func<IIncidenceGraph<int, SReversedEdge<int, Edge<int>>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });
            IIncidenceGraph<int, SReversedEdge<int, Edge<int>>> graph = createGraph();

            Assert.IsFalse(graph.TryGetEdge(0, 10, out _));
            Assert.IsFalse(graph.TryGetEdge(0, 1, out _));

            Assert.IsTrue(graph.TryGetEdge(4, 2, out SReversedEdge<int, Edge<int>> gotEdge));
            AssertSameReversedEdge(edge5, gotEdge);

            Assert.IsTrue(graph.TryGetEdge(2, 2, out gotEdge));
            AssertSameReversedEdge(edge4, gotEdge);

            Assert.IsTrue(graph.TryGetEdge(2, 1, out gotEdge));
            AssertSameReversedEdge(edge1, gotEdge);

            Assert.IsFalse(graph.TryGetEdge(1, 2, out _));
        }

        protected static void TryGetEdge_UndirectedGraph_Test(
            [NotNull] IMutableUndirectedGraph<int, Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);
            var edge7 = new Edge<int>(5, 2);

            graph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6, edge7 });

            Assert.IsFalse(graph.TryGetEdge(0, 10, out _));
            Assert.IsFalse(graph.TryGetEdge(0, 1, out _));

            Assert.IsTrue(graph.TryGetEdge(2, 4, out Edge<int> gotEdge));
            Assert.AreSame(edge5, gotEdge);

            Assert.IsTrue(graph.TryGetEdge(1, 2, out gotEdge));
            Assert.AreSame(edge1, gotEdge);

            Assert.IsTrue(graph.TryGetEdge(2, 2, out gotEdge));
            Assert.AreSame(edge4, gotEdge);

            // 1 -> 2 is present in this undirected graph
            Assert.IsTrue(graph.TryGetEdge(2, 1, out gotEdge));
            Assert.AreSame(edge1, gotEdge);

            Assert.IsTrue(graph.TryGetEdge(5, 2, out gotEdge));
            Assert.AreSame(edge7, gotEdge);

            // 5 -> 2 is present in this undirected graph
            Assert.IsTrue(graph.TryGetEdge(2, 5, out gotEdge));
            Assert.AreSame(edge7, gotEdge);
        }

        protected static void TryGetEdge_ImmutableGraph_UndirectedGraph_Test(
            [NotNull] IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [NotNull, InstantHandle] Func<IImplicitUndirectedGraph<int, Edge<int>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);
            var edge7 = new Edge<int>(5, 2);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6, edge7 });
            var graph = createGraph();

            Assert.IsFalse(graph.TryGetEdge(0, 10, out _));
            Assert.IsFalse(graph.TryGetEdge(0, 1, out _));

            Assert.IsTrue(graph.TryGetEdge(2, 4, out Edge<int> gotEdge));
            Assert.AreSame(edge5, gotEdge);

            Assert.IsTrue(graph.TryGetEdge(1, 2, out gotEdge));
            Assert.AreSame(edge1, gotEdge);

            Assert.IsTrue(graph.TryGetEdge(2, 2, out gotEdge));
            Assert.AreSame(edge4, gotEdge);

            // 1 -> 2 is present in this undirected graph
            Assert.IsTrue(graph.TryGetEdge(2, 1, out gotEdge));
            Assert.AreSame(edge1, gotEdge);

            Assert.IsTrue(graph.TryGetEdge(5, 2, out gotEdge));
            Assert.AreSame(edge7, gotEdge);

            // 5 -> 2 is present in this undirected graph
            Assert.IsTrue(graph.TryGetEdge(2, 5, out gotEdge));
            Assert.AreSame(edge7, gotEdge);
        }

        protected static void TryGetEdge_Throws_Test<TVertex, TEdge>(
            [NotNull] IIncidenceGraph<TVertex, TEdge> graph)
            where TVertex : class, new()
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.TryGetEdge(null, new TVertex(), out _));
            Assert.Throws<ArgumentNullException>(() => graph.TryGetEdge(new TVertex(), null, out _));
            Assert.Throws<ArgumentNullException>(() => graph.TryGetEdge(null, null, out _));
            // ReSharper restore AssignNullToNotNullAttribute
        }

        protected static void TryGetEdge_Throws_UndirectedGraph_Test<TVertex, TEdge>(
            [NotNull] IImplicitUndirectedGraph<TVertex, TEdge> graph)
            where TVertex : class, new()
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.TryGetEdge(null, new TVertex(), out _));
            Assert.Throws<ArgumentNullException>(() => graph.TryGetEdge(new TVertex(), null, out _));
            Assert.Throws<ArgumentNullException>(() => graph.TryGetEdge(null, null, out _));
            // ReSharper restore AssignNullToNotNullAttribute
        }

        protected static void TryGetEdges_Test(
            [NotNull] IIncidenceGraph<int, Edge<int>> graph,
            [NotNull, InstantHandle] Action<IEnumerable<Edge<int>>> addVerticesAndEdgeRange)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);

            addVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });

            Assert.IsFalse(graph.TryGetEdges(0, 10, out _));
            Assert.IsFalse(graph.TryGetEdges(0, 1, out _));

            Assert.IsTrue(graph.TryGetEdges(2, 2, out IEnumerable<Edge<int>> gotEdges));
            CollectionAssert.AreEqual(new[] { edge4 }, gotEdges);

            Assert.IsTrue(graph.TryGetEdges(2, 4, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge5 }, gotEdges);

            Assert.IsTrue(graph.TryGetEdges(1, 2, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge1, edge2 }, gotEdges);

            Assert.IsTrue(graph.TryGetEdges(2, 1, out gotEdges));
            CollectionAssert.IsEmpty(gotEdges);
        }

        protected static void TryGetEdges_Test(
            [NotNull] IMutableVertexAndEdgeListGraph<int, Edge<int>> graph)
        {
            TryGetEdges_Test(
                graph,
                edges => graph.AddVerticesAndEdgeRange(edges));
        }

        protected static void TryGetEdges_ImmutableGraph_Test(
            [NotNull] IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [NotNull, InstantHandle] Func<IIncidenceGraph<int, Edge<int>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });
            IIncidenceGraph<int, Edge<int>> graph = createGraph();

            Assert.IsFalse(graph.TryGetEdges(0, 10, out _));
            Assert.IsFalse(graph.TryGetEdges(0, 1, out _));

            Assert.IsTrue(graph.TryGetEdges(2, 2, out IEnumerable<Edge<int>> gotEdges));
            CollectionAssert.AreEqual(new[] { edge4 }, gotEdges);

            Assert.IsTrue(graph.TryGetEdges(2, 4, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge5 }, gotEdges);

            Assert.IsTrue(graph.TryGetEdges(1, 2, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge1, edge2 }, gotEdges);

            Assert.IsTrue(graph.TryGetEdges(2, 1, out gotEdges));
            CollectionAssert.IsEmpty(gotEdges);
        }

        protected static void TryGetEdges_ImmutableGraph_Test(
            [NotNull] IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [NotNull, InstantHandle] Func<IIncidenceGraph<int, SEquatableEdge<int>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });
            IIncidenceGraph<int, SEquatableEdge<int>> graph = createGraph();

            Assert.IsFalse(graph.TryGetEdges(0, 10, out _));
            Assert.IsFalse(graph.TryGetEdges(0, 1, out _));

            Assert.IsTrue(graph.TryGetEdges(2, 2, out IEnumerable<SEquatableEdge<int>> gotEdges));
            CollectionAssert.AreEqual(
                new[] { new SEquatableEdge<int>(2, 2) },
                gotEdges);

            Assert.IsTrue(graph.TryGetEdges(2, 4, out gotEdges));
            CollectionAssert.AreEqual(
                new[] { new SEquatableEdge<int>(2, 4), },
                gotEdges);

            Assert.IsTrue(graph.TryGetEdges(1, 2, out gotEdges));
            CollectionAssert.AreEqual(
                new[]
                {
                    new SEquatableEdge<int>(1, 2),
                    new SEquatableEdge<int>(1, 2)
                },
                gotEdges);

            Assert.IsTrue(graph.TryGetEdges(2, 1, out gotEdges));
            CollectionAssert.IsEmpty(gotEdges);
        }

        protected static void TryGetEdges_ImmutableVertices_Test(
            [NotNull] BidirectionalMatrixGraph<Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 2);
            var edge4 = new Edge<int>(2, 4);
            var edge5 = new Edge<int>(3, 1);

            graph.AddEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5 });

            Assert.IsFalse(graph.TryGetEdges(6, 10, out _));
            Assert.IsFalse(graph.TryGetEdges(6, 1, out _));

            Assert.IsTrue(graph.TryGetEdges(2, 2, out IEnumerable<Edge<int>> gotEdges));
            CollectionAssert.AreEqual(new[] { edge3 }, gotEdges);

            Assert.IsTrue(graph.TryGetEdges(2, 4, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge4 }, gotEdges);

            Assert.IsTrue(graph.TryGetEdges(1, 2, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge1 }, gotEdges);

            Assert.IsTrue(graph.TryGetEdges(2, 1, out gotEdges));
            CollectionAssert.IsEmpty(gotEdges);
        }

        protected static void TryGetEdges_ImmutableGraph_ReversedTest(
            [NotNull] IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [NotNull, InstantHandle] Func<IIncidenceGraph<int, SReversedEdge<int, Edge<int>>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });
            IIncidenceGraph<int, SReversedEdge<int, Edge<int>>> graph = createGraph();

            Assert.IsFalse(graph.TryGetEdges(0, 10, out _));
            Assert.IsFalse(graph.TryGetEdges(0, 1, out _));

            Assert.IsTrue(graph.TryGetEdges(2, 2, out IEnumerable<SReversedEdge<int, Edge<int>>> gotEdges));
            AssertSameReversedEdges(new[] { edge4 }, gotEdges);

            Assert.IsTrue(graph.TryGetEdges(4, 2, out gotEdges));
            AssertSameReversedEdges(new[] { edge5 }, gotEdges);

            Assert.IsTrue(graph.TryGetEdges(2, 1, out gotEdges));
            AssertSameReversedEdges(new[] { edge1, edge2 }, gotEdges);

            Assert.IsTrue(graph.TryGetEdges(1, 2, out gotEdges));
            CollectionAssert.IsEmpty(gotEdges);
        }

        protected static void TryGetEdges_Throws_Test<TEdge>(
            [NotNull] IIncidenceGraph<TestVertex, TEdge> graph)
            where TEdge : IEdge<TestVertex>
        {
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.TryGetEdges(null, new TestVertex("v2"), out _));
            Assert.Throws<ArgumentNullException>(() => graph.TryGetEdges(new TestVertex("v1"), null, out _));
            Assert.Throws<ArgumentNullException>(() => graph.TryGetEdges(null, null, out _));
            // ReSharper restore AssignNullToNotNullAttribute
        }

        protected static void TryGetOutEdges_Test(
            [NotNull] IImplicitGraph<int, Edge<int>> graph,
            [NotNull, InstantHandle] Action<IEnumerable<Edge<int>>> addVerticesAndEdgeRange)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);
            var edge7 = new Edge<int>(4, 5);

            addVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6, edge7 });

            Assert.IsFalse(graph.TryGetOutEdges(0, out _));

            Assert.IsTrue(graph.TryGetOutEdges(5, out IEnumerable<Edge<int>> gotEdges));
            CollectionAssert.IsEmpty(gotEdges);

            Assert.IsTrue(graph.TryGetOutEdges(3, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge6 }, gotEdges);

            Assert.IsTrue(graph.TryGetOutEdges(1, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge1, edge2, edge3 }, gotEdges);
        }

        protected static void TryGetOutEdges_Test(
            [NotNull] IMutableVertexAndEdgeListGraph<int, Edge<int>> graph)
        {
            TryGetOutEdges_Test(
                graph,
                edges => graph.AddVerticesAndEdgeRange(edges));
        }

        protected static void TryGetOutEdges_ImmutableGraph_Test(
            [NotNull] IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [NotNull, InstantHandle] Func<IImplicitGraph<int, Edge<int>>> createGraph)
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

            Assert.IsFalse(graph.TryGetOutEdges(0, out _));

            Assert.IsTrue(graph.TryGetOutEdges(5, out IEnumerable<Edge<int>> gotEdges));
            CollectionAssert.IsEmpty(gotEdges);

            Assert.IsTrue(graph.TryGetOutEdges(3, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge6 }, gotEdges);

            Assert.IsTrue(graph.TryGetOutEdges(1, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge1, edge2, edge3 }, gotEdges);
        }

        protected static void TryGetOutEdges_ImmutableGraph_Test(
            [NotNull] IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [NotNull, InstantHandle] Func<IImplicitGraph<int, SEquatableEdge<int>>> createGraph)
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

            Assert.IsFalse(graph.TryGetOutEdges(0, out _));

            Assert.IsTrue(graph.TryGetOutEdges(5, out IEnumerable<SEquatableEdge<int>> gotEdges));
            CollectionAssert.IsEmpty(gotEdges);

            Assert.IsTrue(graph.TryGetOutEdges(3, out gotEdges));
            CollectionAssert.AreEqual(
                new[] { new SEquatableEdge<int>(3, 1) },
                gotEdges);

            Assert.IsTrue(graph.TryGetOutEdges(1, out gotEdges));
            CollectionAssert.AreEqual(
                new[]
                {
                    new SEquatableEdge<int>(1, 2),
                    new SEquatableEdge<int>(1, 2),
                    new SEquatableEdge<int>(1, 3)
                },
                gotEdges);
        }

        protected static void TryGetOutEdges_ImmutableVertices_Test(
            [NotNull] BidirectionalMatrixGraph<Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 2);
            var edge4 = new Edge<int>(2, 4);
            var edge5 = new Edge<int>(3, 1);
            var edge6 = new Edge<int>(4, 5);

            graph.AddEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });

            Assert.IsFalse(graph.TryGetOutEdges(6, out _));

            Assert.IsTrue(graph.TryGetOutEdges(5, out IEnumerable<Edge<int>> gotEdges));
            CollectionAssert.IsEmpty(gotEdges);

            Assert.IsTrue(graph.TryGetOutEdges(3, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge5 }, gotEdges);

            Assert.IsTrue(graph.TryGetOutEdges(1, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge1, edge2 }, gotEdges);
        }

        protected static void TryGetOutEdges_ImmutableGraph_ReversedTest(
            [NotNull] IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [NotNull, InstantHandle] Func<IImplicitGraph<int, SReversedEdge<int, Edge<int>>>> createGraph)
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

            Assert.IsFalse(graph.TryGetOutEdges(0, out _));

            Assert.IsTrue(graph.TryGetOutEdges(5, out IEnumerable<SReversedEdge<int, Edge<int>>> gotEdges));
            CollectionAssert.IsEmpty(gotEdges);

            Assert.IsTrue(graph.TryGetOutEdges(3, out gotEdges));
            AssertSameReversedEdges(new[] { edge3 }, gotEdges);

            Assert.IsTrue(graph.TryGetOutEdges(2, out gotEdges));
            AssertSameReversedEdges(new[] { edge1, edge2, edge4 }, gotEdges);
        }

        protected static void TryGetOutEdges_Throws_Test<TVertex, TEdge>(
            [NotNull] IImplicitGraph<TVertex, TEdge> graph)
            where TVertex : class
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.TryGetOutEdges(null, out _));
        }

        protected static void TryGetInEdges_Test(
            [NotNull] IBidirectionalIncidenceGraph<int, Edge<int>> graph,
            [NotNull, InstantHandle] Action<IEnumerable<Edge<int>>> addVerticesAndEdgeRange)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);
            var edge7 = new Edge<int>(5, 3);

            addVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6, edge7 });

            Assert.IsFalse(graph.TryGetInEdges(0, out _));

            Assert.IsTrue(graph.TryGetInEdges(5, out IEnumerable<Edge<int>> gotEdges));
            CollectionAssert.IsEmpty(gotEdges);

            Assert.IsTrue(graph.TryGetInEdges(4, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge5 }, gotEdges);

            Assert.IsTrue(graph.TryGetInEdges(2, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge1, edge2, edge4 }, gotEdges);
        }

        protected static void TryGetInEdges_Test(
            [NotNull] IMutableBidirectionalGraph<int, Edge<int>> graph)
        {
            TryGetInEdges_Test(
                graph,
                edges => graph.AddVerticesAndEdgeRange(edges));
        }

        protected static void TryGetInEdges_ImmutableGraph_Test(
            [NotNull] IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [NotNull, InstantHandle] Func<IBidirectionalIncidenceGraph<int, Edge<int>>> createGraph)
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

            Assert.IsFalse(graph.TryGetInEdges(0, out _));

            Assert.IsTrue(graph.TryGetInEdges(5, out IEnumerable<Edge<int>> gotEdges));
            CollectionAssert.IsEmpty(gotEdges);

            Assert.IsTrue(graph.TryGetInEdges(4, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge5 }, gotEdges);

            Assert.IsTrue(graph.TryGetInEdges(2, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge1, edge2, edge4 }, gotEdges);
        }

        protected static void TryGetInEdges_ImmutableVertices_Test(
            [NotNull] BidirectionalMatrixGraph<Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 2);
            var edge4 = new Edge<int>(2, 4);
            var edge5 = new Edge<int>(3, 1);
            var edge6 = new Edge<int>(5, 3);

            graph.AddEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });

            Assert.IsFalse(graph.TryGetInEdges(6, out _));

            Assert.IsTrue(graph.TryGetInEdges(5, out IEnumerable<Edge<int>> gotEdges));
            CollectionAssert.IsEmpty(gotEdges);

            Assert.IsTrue(graph.TryGetInEdges(4, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge4 }, gotEdges);

            Assert.IsTrue(graph.TryGetInEdges(2, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge1, edge3 }, gotEdges);
        }

        protected static void TryGetInEdges_ImmutableGraph_ReversedTest(
            [NotNull] IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [NotNull, InstantHandle] Func<IBidirectionalIncidenceGraph<int, SReversedEdge<int, Edge<int>>>> createGraph)
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

            Assert.IsFalse(graph.TryGetInEdges(0, out _));

            Assert.IsTrue(graph.TryGetInEdges(5, out IEnumerable<SReversedEdge<int, Edge<int>>> gotEdges));
            CollectionAssert.IsEmpty(gotEdges);

            Assert.IsTrue(graph.TryGetInEdges(4, out gotEdges));
            AssertSameReversedEdges(new[] { edge7 }, gotEdges);

            Assert.IsTrue(graph.TryGetInEdges(1, out gotEdges));
            AssertSameReversedEdges(new[] { edge1, edge2, edge3, edge4 }, gotEdges);
        }

        protected static void TryGetInEdges_Throws_Test<TVertex, TEdge>(
            [NotNull] IBidirectionalIncidenceGraph<TVertex, TEdge> graph)
            where TVertex : class
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.TryGetInEdges(null, out _));
        }

        #endregion
    }
}