using System;
using System.Linq;
using NUnit.Framework;
using QuikGraph.Algorithms;
using QuikGraph.Tests.Structures;
using static QuikGraph.Tests.GraphTestHelpers;

namespace QuikGraph.Tests.Algorithms
{
    /// <summary>
    /// Tests related to <see cref="RandomGraphFactory"/>
    /// </summary>
    [TestFixture]
    internal class RandomGraphFactoryTests : GraphTestsBase
    {
        [Test]
        public void GetVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVertexRange(new[] { 1, 2, 3, 4, 5 });

            int vertex = RandomGraphFactory.GetVertex(graph, new Random(123456));
            Assert.AreEqual(2, vertex);

            vertex = RandomGraphFactory.GetVertex(graph, new Random(456789));
            Assert.AreEqual(5, vertex);

            vertex = RandomGraphFactory.GetVertex(graph.Vertices, graph.VertexCount, new Random(123456));
            Assert.AreEqual(2, vertex);

            vertex = RandomGraphFactory.GetVertex(graph.Vertices, graph.VertexCount, new Random(456789));
            Assert.AreEqual(5, vertex);

            vertex = RandomGraphFactory.GetVertex(graph.Vertices, 3, new Random(123));
            Assert.AreEqual(3, vertex);
        }

        [Test]
        public void GetVertex_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var graph2 = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var random = new Random();

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => RandomGraphFactory.GetVertex<int>(null, random));
            Assert.Throws<ArgumentNullException>(() => RandomGraphFactory.GetVertex(graph2, null));
            Assert.Throws<ArgumentNullException>(() => RandomGraphFactory.GetVertex<int>(null, null));

            Assert.Throws<ArgumentNullException>(() => RandomGraphFactory.GetVertex<int>(null, 1, random));
            Assert.Throws<ArgumentNullException>(() => RandomGraphFactory.GetVertex(Enumerable.Empty<int>(), 1, null));
            Assert.Throws<ArgumentNullException>(() => RandomGraphFactory.GetVertex<int>(null, 1, null));
            // ReSharper restore AssignNullToNotNullAttribute
            Assert.Throws<ArgumentOutOfRangeException>(() => RandomGraphFactory.GetVertex(graph, random));

            Assert.Throws<ArgumentOutOfRangeException>(() => RandomGraphFactory.GetVertex(Enumerable.Empty<int>(), -1, random));
            Assert.Throws<ArgumentOutOfRangeException>(() => RandomGraphFactory.GetVertex(Enumerable.Empty<int>(), 0, random));
            Assert.Throws<InvalidOperationException>(() => RandomGraphFactory.GetVertex(Enumerable.Empty<int>(), 1, random));
            Assert.Throws<InvalidOperationException>(() => RandomGraphFactory.GetVertex(new[] { 1, 2 }, 10, new Random(123456)));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void GetEdge()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge23 = new Edge<int>(2, 3);
            var edge24 = new Edge<int>(2, 4);
            var edge35 = new Edge<int>(3, 5);
            graph.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge13, edge23, edge24, edge35
            });

            Edge<int> edge = RandomGraphFactory.GetEdge(graph, new Random(123456));
            Assert.AreSame(edge13, edge);

            edge = RandomGraphFactory.GetEdge(graph, new Random(456789));
            Assert.AreSame(edge35, edge);

            edge = RandomGraphFactory.GetEdge<int, Edge<int>>(graph.Edges, graph.VertexCount, new Random(123456));
            Assert.AreSame(edge13, edge);

            edge = RandomGraphFactory.GetEdge<int, Edge<int>>(graph.Edges, graph.VertexCount, new Random(456789));
            Assert.AreSame(edge35, edge);

            edge = RandomGraphFactory.GetEdge<int, Edge<int>>(graph.Edges, 3, new Random(123));
            Assert.AreSame(edge23, edge);
        }

        [Test]
        public void GetEdge_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var graph2 = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var random = new Random();

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => RandomGraphFactory.GetEdge<int, Edge<int>>(null, random));
            Assert.Throws<ArgumentNullException>(() => RandomGraphFactory.GetEdge(graph2, null));
            Assert.Throws<ArgumentNullException>(() => RandomGraphFactory.GetEdge<int, Edge<int>>(null, null));

            Assert.Throws<ArgumentNullException>(() => RandomGraphFactory.GetEdge<int, Edge<int>>(null, 1, random));
            Assert.Throws<ArgumentNullException>(() => RandomGraphFactory.GetEdge<int, Edge<int>>(Enumerable.Empty<Edge<int>>(), 1, null));
            Assert.Throws<ArgumentNullException>(() => RandomGraphFactory.GetEdge<int, Edge<int>>(null, 1, null));
            // ReSharper restore AssignNullToNotNullAttribute
            Assert.Throws<ArgumentOutOfRangeException>(() => RandomGraphFactory.GetVertex(graph, random));

            Assert.Throws<ArgumentOutOfRangeException>(() => RandomGraphFactory.GetEdge<int, Edge<int>>(Enumerable.Empty<Edge<int>>(), -1, random));
            Assert.Throws<ArgumentOutOfRangeException>(() => RandomGraphFactory.GetEdge<int, Edge<int>>(Enumerable.Empty<Edge<int>>(), 0, random));
            Assert.Throws<InvalidOperationException>(() => RandomGraphFactory.GetEdge<int, Edge<int>>(Enumerable.Empty<Edge<int>>(), 1, random));
            Assert.Throws<InvalidOperationException>(
                () => RandomGraphFactory.GetEdge<int, Edge<int>>(
                    new[] { new Edge<int>(1, 2), new Edge<int>(1, 3) },
                    10,
                    new Random(123456)));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void Create()
        {
            var graph = new AdjacencyGraph<int , EquatableEdge<int>>();

            // With self edge
            int v = 0;
            RandomGraphFactory.Create(
                graph,
                () => ++v,
                (source, target) => new EquatableEdge<int>(source, target),
                new Random(123456),
                5,
                10,
                true);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4, 5 });
            AssertHasEdges(
                graph,
                new[]
                {
                    new EquatableEdge<int>(1, 2),
                    new EquatableEdge<int>(1, 2),
                    new EquatableEdge<int>(1, 2),
                    new EquatableEdge<int>(2, 1),
                    new EquatableEdge<int>(3, 5),
                    new EquatableEdge<int>(3, 1),
                    new EquatableEdge<int>(4, 5),
                    new EquatableEdge<int>(4, 4),
                    new EquatableEdge<int>(4, 1),
                    new EquatableEdge<int>(5, 3)
                });

            // Without self edge
            graph.Clear();
            v = 0;
            RandomGraphFactory.Create(
                graph,
                () => ++v,
                (source, target) => new EquatableEdge<int>(source, target),
                new Random(123456),
                5,
                10,
                false);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4, 5 });
            AssertHasEdges(
                graph,
                new[]
                {
                    new EquatableEdge<int>(1, 2),
                    new EquatableEdge<int>(1, 2),
                    new EquatableEdge<int>(1, 2),
                    new EquatableEdge<int>(2, 1),
                    new EquatableEdge<int>(3, 5),
                    new EquatableEdge<int>(3, 1),
                    new EquatableEdge<int>(3, 1),
                    new EquatableEdge<int>(4, 5),
                    new EquatableEdge<int>(4, 1),
                    new EquatableEdge<int>(5, 3)
                });

            // Different seed change generated graph
            graph.Clear();
            v = 0;
            RandomGraphFactory.Create(
                graph,
                () => ++v,
                (source, target) => new EquatableEdge<int>(source, target),
                new Random(456789),
                5,
                10,
                true);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4, 5 });
            AssertHasEdges(
                graph,
                new[]
                {
                    new EquatableEdge<int>(1, 2),
                    new EquatableEdge<int>(2, 2),
                    new EquatableEdge<int>(2, 5),
                    new EquatableEdge<int>(3, 4),
                    new EquatableEdge<int>(3, 2),
                    new EquatableEdge<int>(4, 5),
                    new EquatableEdge<int>(4, 2),
                    new EquatableEdge<int>(4, 2),
                    new EquatableEdge<int>(5, 2),
                    new EquatableEdge<int>(5, 3)
                });

            // On non empty graph, keep existing stuff
            graph.Clear();
            graph.AddVerticesAndEdge(new EquatableEdge<int>(6, 7));
            v = 0;
            RandomGraphFactory.Create(
                graph,
                () => ++v,
                (source, target) => new EquatableEdge<int>(source, target),
                new Random(123456),
                5,
                10,
                true);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4, 5, 6, 7 });
            AssertHasEdges(
                graph,
                new[]
                {
                    new EquatableEdge<int>(6, 7),
                    new EquatableEdge<int>(1, 2),
                    new EquatableEdge<int>(1, 2),
                    new EquatableEdge<int>(1, 2),
                    new EquatableEdge<int>(2, 1),
                    new EquatableEdge<int>(3, 5),
                    new EquatableEdge<int>(3, 1),
                    new EquatableEdge<int>(4, 5),
                    new EquatableEdge<int>(4, 4),
                    new EquatableEdge<int>(4, 1),
                    new EquatableEdge<int>(5, 3)
                });
        }

        [Test]
        public void Create_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var random = new Random();

            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => RandomGraphFactory.Create(
                    (IMutableVertexAndEdgeListGraph<int, Edge<int>>)null,
                    () => 1,
                    (source, target) => new Edge<int>(source, target),
                    random,
                    1, 1, false));
            Assert.Throws<ArgumentNullException>(
                () => RandomGraphFactory.Create(
                    graph,
                    null,
                    (source, target) => new Edge<int>(source, target),
                    random,
                    1, 1, false));
            Assert.Throws<ArgumentNullException>(
                () => RandomGraphFactory.Create(
                    graph,
                    () => 1,
                    null,
                    random,
                    1, 1, false));
            Assert.Throws<ArgumentNullException>(
                () => RandomGraphFactory.Create(
                    graph,
                    () => 1,
                    (source, target) => new Edge<int>(source, target),
                    null,
                    1, 1, false));
            Assert.Throws<ArgumentNullException>(
                () => RandomGraphFactory.Create(
                    (IMutableVertexAndEdgeListGraph<int, Edge<int>>)null,
                    null,
                    (source, target) => new Edge<int>(source, target),
                    random,
                    1, 1, false));
            Assert.Throws<ArgumentNullException>(
                () => RandomGraphFactory.Create(
                    (IMutableVertexAndEdgeListGraph<int, Edge<int>>)null,
                    () => 1,
                    null,
                    random,
                    1, 1, false));
            Assert.Throws<ArgumentNullException>(
                () => RandomGraphFactory.Create(
                    (IMutableVertexAndEdgeListGraph<int, Edge<int>>)null,
                    () => 1,
                    (source, target) => new Edge<int>(source, target),
                    null,
                    1, 1, false));
            Assert.Throws<ArgumentNullException>(
                () => RandomGraphFactory.Create(
                    graph,
                    null,
                    null,
                    random,
                    1, 1, false));
            Assert.Throws<ArgumentNullException>(
                () => RandomGraphFactory.Create(
                    graph,
                    null,
                    (source, target) => new Edge<int>(source, target),
                    null,
                    1, 1, false));
            Assert.Throws<ArgumentNullException>(
                () => RandomGraphFactory.Create(
                    graph,
                    () => 1,
                    null,
                    null,
                    1, 1, false));
            Assert.Throws<ArgumentNullException>(
                () => RandomGraphFactory.Create(
                    graph,
                    null,
                    null,
                    null,
                    1, 1, false));
            Assert.Throws<ArgumentNullException>(
                () => RandomGraphFactory.Create(
                    (IMutableVertexAndEdgeListGraph<int, Edge<int>>)null,
                    () => 1,
                    null,
                    null,
                    1, 1, false));
            Assert.Throws<ArgumentNullException>(
                () => RandomGraphFactory.Create(
                    (IMutableVertexAndEdgeListGraph<int, Edge<int>>)null,
                    null,
                    (source, target) => new Edge<int>(source, target),
                    null,
                    1, 1, false));
            Assert.Throws<ArgumentNullException>(
                () => RandomGraphFactory.Create(
                    (IMutableVertexAndEdgeListGraph<int, Edge<int>>)null,
                    null,
                    null,
                    random,
                    1, 1, false));
            Assert.Throws<ArgumentNullException>(
                () => RandomGraphFactory.Create(
                    (IMutableVertexAndEdgeListGraph<int, Edge<int>>)null,
                    null,
                    null,
                    null,
                    1, 1, false));
            // ReSharper restore AssignNullToNotNullAttribute
            Assert.Throws<ArgumentOutOfRangeException>(
                () => RandomGraphFactory.Create(
                    graph,
                    () => 1,
                    (source, target) => new Edge<int>(source, target),
                    random,
                    -1, 1, false));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => RandomGraphFactory.Create(
                    graph,
                    () => 1,
                    (source, target) => new Edge<int>(source, target),
                    random,
                    0, 1, false));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => RandomGraphFactory.Create(
                    graph,
                    () => 1,
                    (source, target) => new Edge<int>(source, target),
                    random,
                    1, -1, false));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => RandomGraphFactory.Create(
                    graph,
                    () => 1,
                    (source, target) => new Edge<int>(source, target),
                    random,
                    1, 0, false));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => RandomGraphFactory.Create(
                    graph,
                    () => 1,
                    (source, target) => new Edge<int>(source, target),
                    random,
                    0, 0, false));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => RandomGraphFactory.Create(
                    graph,
                    () => 1,
                    (source, target) => new Edge<int>(source, target),
                    random,
                    -1, -1, false));
        }

        [Test]
        public void Create_Undirected()
        {
            var graph = new UndirectedGraph<int, EquatableEdge<int>>();

            // With self edge
            int v = 0;
            RandomGraphFactory.Create(
                graph,
                () => ++v,
                (source, target) => new EquatableEdge<int>(source, target),
                new Random(123456),
                5,
                10,
                true);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4, 5 });
            AssertHasEdges(
                graph,
                new[]
                {
                    new EquatableEdge<int>(1, 2),
                    new EquatableEdge<int>(1, 2),
                    new EquatableEdge<int>(1, 2),
                    new EquatableEdge<int>(2, 1),
                    new EquatableEdge<int>(3, 5),
                    new EquatableEdge<int>(3, 1),
                    new EquatableEdge<int>(4, 5),
                    new EquatableEdge<int>(4, 4),
                    new EquatableEdge<int>(4, 1),
                    new EquatableEdge<int>(5, 3)
                });

            // Without self edge
            graph.Clear();
            v = 0;
            RandomGraphFactory.Create(
                graph,
                () => ++v,
                (source, target) => new EquatableEdge<int>(source, target),
                new Random(123456),
                5,
                10,
                false);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4, 5 });
            AssertHasEdges(
                graph,
                new[]
                {
                    new EquatableEdge<int>(1, 2),
                    new EquatableEdge<int>(1, 2),
                    new EquatableEdge<int>(1, 2),
                    new EquatableEdge<int>(2, 1),
                    new EquatableEdge<int>(3, 5),
                    new EquatableEdge<int>(3, 1),
                    new EquatableEdge<int>(3, 1),
                    new EquatableEdge<int>(4, 5),
                    new EquatableEdge<int>(4, 1),
                    new EquatableEdge<int>(5, 3)
                });

            // Different seed change generated graph
            graph.Clear();
            v = 0;
            RandomGraphFactory.Create(
                graph,
                () => ++v,
                (source, target) => new EquatableEdge<int>(source, target),
                new Random(456789),
                5,
                10,
                true);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4, 5 });
            AssertHasEdges(
                graph,
                new[]
                {
                    new EquatableEdge<int>(1, 2),
                    new EquatableEdge<int>(2, 2),
                    new EquatableEdge<int>(2, 5),
                    new EquatableEdge<int>(3, 4),
                    new EquatableEdge<int>(3, 2),
                    new EquatableEdge<int>(4, 5),
                    new EquatableEdge<int>(4, 2),
                    new EquatableEdge<int>(4, 2),
                    new EquatableEdge<int>(5, 2),
                    new EquatableEdge<int>(5, 3)
                });

            // On non empty graph, keep existing stuff
            graph.Clear();
            graph.AddVerticesAndEdge(new EquatableEdge<int>(6, 7));
            v = 0;
            RandomGraphFactory.Create(
                graph,
                () => ++v,
                (source, target) => new EquatableEdge<int>(source, target),
                new Random(123456),
                5,
                10,
                true);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4, 5, 6, 7 });
            AssertHasEdges(
                graph,
                new[]
                {
                    new EquatableEdge<int>(6, 7),
                    new EquatableEdge<int>(1, 2),
                    new EquatableEdge<int>(1, 2),
                    new EquatableEdge<int>(1, 2),
                    new EquatableEdge<int>(2, 1),
                    new EquatableEdge<int>(3, 5),
                    new EquatableEdge<int>(3, 1),
                    new EquatableEdge<int>(4, 5),
                    new EquatableEdge<int>(4, 4),
                    new EquatableEdge<int>(4, 1),
                    new EquatableEdge<int>(5, 3)
                });
        }

        [Test]
        public void Create_Undirected_Throws()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            var random = new Random();

            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => RandomGraphFactory.Create(
                    (IMutableUndirectedGraph<int, Edge<int>>)null,
                    () => 1,
                    (source, target) => new Edge<int>(source, target),
                    random,
                    1, 1, false));
            Assert.Throws<ArgumentNullException>(
                () => RandomGraphFactory.Create(
                    graph,
                    null,
                    (source, target) => new Edge<int>(source, target),
                    random,
                    1, 1, false));
            Assert.Throws<ArgumentNullException>(
                () => RandomGraphFactory.Create(
                    graph,
                    () => 1,
                    null,
                    random,
                    1, 1, false));
            Assert.Throws<ArgumentNullException>(
                () => RandomGraphFactory.Create(
                    graph,
                    () => 1,
                    (source, target) => new Edge<int>(source, target),
                    null,
                    1, 1, false));
            Assert.Throws<ArgumentNullException>(
                () => RandomGraphFactory.Create(
                    (IMutableUndirectedGraph<int, Edge<int>>)null,
                    null,
                    (source, target) => new Edge<int>(source, target),
                    random,
                    1, 1, false));
            Assert.Throws<ArgumentNullException>(
                () => RandomGraphFactory.Create(
                    (IMutableUndirectedGraph<int, Edge<int>>)null,
                    () => 1,
                    null,
                    random,
                    1, 1, false));
            Assert.Throws<ArgumentNullException>(
                () => RandomGraphFactory.Create(
                    (IMutableUndirectedGraph<int, Edge<int>>)null,
                    () => 1,
                    (source, target) => new Edge<int>(source, target),
                    null,
                    1, 1, false));
            Assert.Throws<ArgumentNullException>(
                () => RandomGraphFactory.Create(
                    graph,
                    null,
                    null,
                    random,
                    1, 1, false));
            Assert.Throws<ArgumentNullException>(
                () => RandomGraphFactory.Create(
                    graph,
                    null,
                    (source, target) => new Edge<int>(source, target),
                    null,
                    1, 1, false));
            Assert.Throws<ArgumentNullException>(
                () => RandomGraphFactory.Create(
                    graph,
                    () => 1,
                    null,
                    null,
                    1, 1, false));
            Assert.Throws<ArgumentNullException>(
                () => RandomGraphFactory.Create(
                    graph,
                    null,
                    null,
                    null,
                    1, 1, false));
            Assert.Throws<ArgumentNullException>(
                () => RandomGraphFactory.Create(
                    (IMutableUndirectedGraph<int, Edge<int>>)null,
                    () => 1,
                    null,
                    null,
                    1, 1, false));
            Assert.Throws<ArgumentNullException>(
                () => RandomGraphFactory.Create(
                    (IMutableUndirectedGraph<int, Edge<int>>)null,
                    null,
                    (source, target) => new Edge<int>(source, target),
                    null,
                    1, 1, false));
            Assert.Throws<ArgumentNullException>(
                () => RandomGraphFactory.Create(
                    (IMutableUndirectedGraph<int, Edge<int>>)null,
                    null,
                    null,
                    random,
                    1, 1, false));
            Assert.Throws<ArgumentNullException>(
                () => RandomGraphFactory.Create(
                    (IMutableUndirectedGraph<int, Edge<int>>)null,
                    null,
                    null,
                    null,
                    1, 1, false));
            // ReSharper restore AssignNullToNotNullAttribute
            Assert.Throws<ArgumentOutOfRangeException>(
                () => RandomGraphFactory.Create(
                    graph,
                    () => 1,
                    (source, target) => new Edge<int>(source, target),
                    random,
                    -1, 1, false));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => RandomGraphFactory.Create(
                    graph,
                    () => 1,
                    (source, target) => new Edge<int>(source, target),
                    random,
                    0, 1, false));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => RandomGraphFactory.Create(
                    graph,
                    () => 1,
                    (source, target) => new Edge<int>(source, target),
                    random,
                    1, -1, false));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => RandomGraphFactory.Create(
                    graph,
                    () => 1,
                    (source, target) => new Edge<int>(source, target),
                    random,
                    1, 0, false));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => RandomGraphFactory.Create(
                    graph,
                    () => 1,
                    (source, target) => new Edge<int>(source, target),
                    random,
                    0, 0, false));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => RandomGraphFactory.Create(
                    graph,
                    () => 1,
                    (source, target) => new Edge<int>(source, target),
                    random,
                    -1, -1, false));
        }
    }
}