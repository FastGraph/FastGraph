#nullable enable

using NUnit.Framework;
using FastGraph.Algorithms;
using FastGraph.Tests.Structures;
using static FastGraph.Tests.GraphTestHelpers;

namespace FastGraph.Tests.Algorithms
{
    /// <summary>
    /// Tests related to <see cref="RandomGraphFactory"/>
    /// </summary>
    [TestFixture]
    internal sealed class RandomGraphFactoryTests : GraphTestsBase
    {
        [Test]
        public void GetVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVertexRange(new[] { 1, 2, 3, 4, 5 });

            int vertex = RandomGraphFactory.GetVertex(graph, new Random(123456));
            vertex.Should().Be(2);

            vertex = RandomGraphFactory.GetVertex(graph, new Random(456789));
            vertex.Should().Be(5);

            vertex = RandomGraphFactory.GetVertex(graph.Vertices, graph.VertexCount, new Random(123456));
            vertex.Should().Be(2);

            vertex = RandomGraphFactory.GetVertex(graph.Vertices, graph.VertexCount, new Random(456789));
            vertex.Should().Be(5);

            vertex = RandomGraphFactory.GetVertex(graph.Vertices, 3, new Random(123));
            vertex.Should().Be(3);
        }

        [Test]
        public void GetVertex_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var graph2 = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var random = new Random();

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => RandomGraphFactory.GetVertex<int>(default, random)).Should().Throw<ArgumentNullException>();
            Invoking(() => RandomGraphFactory.GetVertex(graph2, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => RandomGraphFactory.GetVertex<int>(default, default)).Should().Throw<ArgumentNullException>();

            Invoking(() => RandomGraphFactory.GetVertex<int>(default, 1, random)).Should().Throw<ArgumentNullException>();
            Invoking(() => RandomGraphFactory.GetVertex(Enumerable.Empty<int>(), 1, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => RandomGraphFactory.GetVertex<int>(default, 1, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            Invoking(() => RandomGraphFactory.GetVertex(graph, random)).Should().Throw<ArgumentOutOfRangeException>();

            Invoking(() => RandomGraphFactory.GetVertex(Enumerable.Empty<int>(), -1, random)).Should().Throw<ArgumentOutOfRangeException>();
            Invoking(() => RandomGraphFactory.GetVertex(Enumerable.Empty<int>(), 0, random)).Should().Throw<ArgumentOutOfRangeException>();
            Invoking(() => RandomGraphFactory.GetVertex(Enumerable.Empty<int>(), 1, random)).Should().Throw<InvalidOperationException>();
            Invoking(() => RandomGraphFactory.GetVertex(new[] { 1, 2 }, 10, new Random(123456))).Should().Throw<InvalidOperationException>();
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
            edge.Should().BeSameAs(edge13);

            edge = RandomGraphFactory.GetEdge(graph, new Random(456789));
            edge.Should().BeSameAs(edge35);

            edge = RandomGraphFactory.GetEdge<int, Edge<int>>(graph.Edges, graph.VertexCount, new Random(123456));
            edge.Should().BeSameAs(edge13);

            edge = RandomGraphFactory.GetEdge<int, Edge<int>>(graph.Edges, graph.VertexCount, new Random(456789));
            edge.Should().BeSameAs(edge35);

            edge = RandomGraphFactory.GetEdge<int, Edge<int>>(graph.Edges, 3, new Random(123));
            edge.Should().BeSameAs(edge23);
        }

        [Test]
        public void GetEdge_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var graph2 = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var random = new Random();

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => RandomGraphFactory.GetEdge<int, Edge<int>>(default, random)).Should().Throw<ArgumentNullException>();
            Invoking(() => RandomGraphFactory.GetEdge(graph2, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => RandomGraphFactory.GetEdge<int, Edge<int>>(default, default)).Should().Throw<ArgumentNullException>();

            Invoking(() => RandomGraphFactory.GetEdge<int, Edge<int>>(default, 1, random)).Should().Throw<ArgumentNullException>();
            Invoking(() => RandomGraphFactory.GetEdge<int, Edge<int>>(Enumerable.Empty<Edge<int>>(), 1, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => RandomGraphFactory.GetEdge<int, Edge<int>>(default, 1, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            Invoking(() => RandomGraphFactory.GetVertex(graph, random)).Should().Throw<ArgumentOutOfRangeException>();

            Invoking(() => RandomGraphFactory.GetEdge<int, Edge<int>>(Enumerable.Empty<Edge<int>>(), -1, random)).Should().Throw<ArgumentOutOfRangeException>();
            Invoking(() => RandomGraphFactory.GetEdge<int, Edge<int>>(Enumerable.Empty<Edge<int>>(), 0, random)).Should().Throw<ArgumentOutOfRangeException>();
            Invoking(() => RandomGraphFactory.GetEdge<int, Edge<int>>(Enumerable.Empty<Edge<int>>(), 1, random)).Should().Throw<InvalidOperationException>();
            Invoking(() => RandomGraphFactory.GetEdge<int, Edge<int>>(
                new[] { new Edge<int>(1, 2), new Edge<int>(1, 3) },
                10,
                new Random(123456))).Should().Throw<InvalidOperationException>();
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void Create()
        {
            var graph = new AdjacencyGraph<int, EquatableEdge<int>>();

            // With isolated vertices only
            int v = 0;
            RandomGraphFactory.Create(
                graph,
                () => ++v,
                (source, target) => new EquatableEdge<int>(source, target),
                new Random(123456),
                2,
                0,
                true);
            AssertHasVertices(graph, new[] { 1, 2 });
            AssertNoEdge(graph);

            // With self edge
            v = 0;
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
#pragma warning disable CS8625
            Invoking(() => RandomGraphFactory.Create(
                (IMutableVertexAndEdgeListGraph<int, Edge<int>>?)default,
                () => 1,
                (source, target) => new Edge<int>(source, target),
                random,
                1, 1, false)).Should().Throw<ArgumentNullException>();
            Invoking(() => RandomGraphFactory.Create(
                graph,
                default,
                (source, target) => new Edge<int>(source, target),
                random,
                1, 1, false)).Should().Throw<ArgumentNullException>();
            Invoking(() => RandomGraphFactory.Create(
                graph,
                () => 1,
                default,
                random,
                1, 1, false)).Should().Throw<ArgumentNullException>();
            Invoking(() => RandomGraphFactory.Create(
                graph,
                () => 1,
                (source, target) => new Edge<int>(source, target),
                default,
                1, 1, false)).Should().Throw<ArgumentNullException>();
            Invoking(() => RandomGraphFactory.Create(
                (IMutableVertexAndEdgeListGraph<int, Edge<int>>?)default,
                default,
                (source, target) => new Edge<int>(source, target),
                random,
                1, 1, false)).Should().Throw<ArgumentNullException>();
            Invoking(() => RandomGraphFactory.Create(
                (IMutableVertexAndEdgeListGraph<int, Edge<int>>?)default,
                () => 1,
                default,
                random,
                1, 1, false)).Should().Throw<ArgumentNullException>();
            Invoking(() => RandomGraphFactory.Create(
                (IMutableVertexAndEdgeListGraph<int, Edge<int>>?)default,
                () => 1,
                (source, target) => new Edge<int>(source, target),
                default,
                1, 1, false)).Should().Throw<ArgumentNullException>();
            Invoking(() => RandomGraphFactory.Create(
                graph,
                default,
                default,
                random,
                1, 1, false)).Should().Throw<ArgumentNullException>();
            Invoking(() => RandomGraphFactory.Create(
                graph,
                default,
                (source, target) => new Edge<int>(source, target),
                default,
                1, 1, false)).Should().Throw<ArgumentNullException>();
            Invoking(() => RandomGraphFactory.Create(
                graph,
                () => 1,
                default,
                default,
                1, 1, false)).Should().Throw<ArgumentNullException>();
            Invoking(() => RandomGraphFactory.Create(
                graph,
                default,
                default,
                default,
                1, 1, false)).Should().Throw<ArgumentNullException>();
            Invoking(() => RandomGraphFactory.Create(
                (IMutableVertexAndEdgeListGraph<int, Edge<int>>?)default,
                () => 1,
                default,
                default,
                1, 1, false)).Should().Throw<ArgumentNullException>();
            Invoking(() => RandomGraphFactory.Create(
                (IMutableVertexAndEdgeListGraph<int, Edge<int>>?)default,
                default,
                (source, target) => new Edge<int>(source, target),
                default,
                1, 1, false)).Should().Throw<ArgumentNullException>();
            Invoking(() => RandomGraphFactory.Create(
                (IMutableVertexAndEdgeListGraph<int, Edge<int>>?)default,
                default,
                default,
                random,
                1, 1, false)).Should().Throw<ArgumentNullException>();
            Invoking(() => RandomGraphFactory.Create(
                (IMutableVertexAndEdgeListGraph<int, Edge<int>>?)default,
                default,
                default,
                default,
                1, 1, false)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            Invoking(() => RandomGraphFactory.Create(
                graph,
                () => 1,
                (source, target) => new Edge<int>(source, target),
                random,
                -1, 1, false)).Should().Throw<ArgumentOutOfRangeException>();
            Invoking(() => RandomGraphFactory.Create(
                graph,
                () => 1,
                (source, target) => new Edge<int>(source, target),
                random,
                0, 1, false)).Should().Throw<ArgumentOutOfRangeException>();
            Invoking(() => RandomGraphFactory.Create(
                graph,
                () => 1,
                (source, target) => new Edge<int>(source, target),
                random,
                1, -1, false)).Should().Throw<ArgumentOutOfRangeException>();
            Invoking(() => RandomGraphFactory.Create(
                graph,
                () => 1,
                (source, target) => new Edge<int>(source, target),
                random,
                0, 0, false)).Should().Throw<ArgumentOutOfRangeException>();
            Invoking(() => RandomGraphFactory.Create(
                graph,
                () => 1,
                (source, target) => new Edge<int>(source, target),
                random,
                -1, -1, false)).Should().Throw<ArgumentOutOfRangeException>();
        }

        [Test]
        public void Create_Undirected()
        {
            var graph = new UndirectedGraph<int, EquatableEdge<int>>();

            // With isolated vertices only
            int v = 0;
            RandomGraphFactory.Create(
                graph,
                () => ++v,
                (source, target) => new EquatableEdge<int>(source, target),
                new Random(123456),
                2,
                0,
                true);
            AssertHasVertices(graph, new[] { 1, 2 });
            AssertNoEdge(graph);

            // With self edge
            v = 0;
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
#pragma warning disable CS8625
            Invoking(() => RandomGraphFactory.Create(
                (IMutableUndirectedGraph<int, Edge<int>>?)default,
                () => 1,
                (source, target) => new Edge<int>(source, target),
                random,
                1, 1, false)).Should().Throw<ArgumentNullException>();
            Invoking(() => RandomGraphFactory.Create(
                graph,
                default,
                (source, target) => new Edge<int>(source, target),
                random,
                1, 1, false)).Should().Throw<ArgumentNullException>();
            Invoking(() => RandomGraphFactory.Create(
                graph,
                () => 1,
                default,
                random,
                1, 1, false)).Should().Throw<ArgumentNullException>();
            Invoking(() => RandomGraphFactory.Create(
                graph,
                () => 1,
                (source, target) => new Edge<int>(source, target),
                default,
                1, 1, false)).Should().Throw<ArgumentNullException>();
            Invoking(() => RandomGraphFactory.Create(
                (IMutableUndirectedGraph<int, Edge<int>>?)default,
                default,
                (source, target) => new Edge<int>(source, target),
                random,
                1, 1, false)).Should().Throw<ArgumentNullException>();
            Invoking(() => RandomGraphFactory.Create(
                (IMutableUndirectedGraph<int, Edge<int>>?)default,
                () => 1,
                default,
                random,
                1, 1, false)).Should().Throw<ArgumentNullException>();
            Invoking(() => RandomGraphFactory.Create(
                (IMutableUndirectedGraph<int, Edge<int>>?)default,
                () => 1,
                (source, target) => new Edge<int>(source, target),
                default,
                1, 1, false)).Should().Throw<ArgumentNullException>();
            Invoking(() => RandomGraphFactory.Create(
                graph,
                default,
                default,
                random,
                1, 1, false)).Should().Throw<ArgumentNullException>();
            Invoking(() => RandomGraphFactory.Create(
                graph,
                default,
                (source, target) => new Edge<int>(source, target),
                default,
                1, 1, false)).Should().Throw<ArgumentNullException>();
            Invoking(() => RandomGraphFactory.Create(
                graph,
                () => 1,
                default,
                default,
                1, 1, false)).Should().Throw<ArgumentNullException>();
            Invoking(() => RandomGraphFactory.Create(
                graph,
                default,
                default,
                default,
                1, 1, false)).Should().Throw<ArgumentNullException>();
            Invoking(() => RandomGraphFactory.Create(
                (IMutableUndirectedGraph<int, Edge<int>>?)default,
                () => 1,
                default,
                default,
                1, 1, false)).Should().Throw<ArgumentNullException>();
            Invoking(() => RandomGraphFactory.Create(
                (IMutableUndirectedGraph<int, Edge<int>>?)default,
                default,
                (source, target) => new Edge<int>(source, target),
                default,
                1, 1, false)).Should().Throw<ArgumentNullException>();
            Invoking(() => RandomGraphFactory.Create(
                (IMutableUndirectedGraph<int, Edge<int>>?)default,
                default,
                default,
                random,
                1, 1, false)).Should().Throw<ArgumentNullException>();
            Invoking(() => RandomGraphFactory.Create(
                (IMutableUndirectedGraph<int, Edge<int>>?)default,
                default,
                default,
                default,
                1, 1, false)).Should().Throw<ArgumentNullException>();
            // ReSharper restore AssignNullToNotNullAttribute
            Invoking(() => RandomGraphFactory.Create(
                graph,
                () => 1,
                (source, target) => new Edge<int>(source, target),
                random,
                -1, 1, false)).Should().Throw<ArgumentOutOfRangeException>();
            Invoking(() => RandomGraphFactory.Create(
                graph,
                () => 1,
                (source, target) => new Edge<int>(source, target),
                random,
                0, 1, false)).Should().Throw<ArgumentOutOfRangeException>();
            Invoking(() => RandomGraphFactory.Create(
                graph,
                () => 1,
                (source, target) => new Edge<int>(source, target),
                random,
                1, -1, false)).Should().Throw<ArgumentOutOfRangeException>();
            Invoking(() => RandomGraphFactory.Create(
                graph,
                () => 1,
                (source, target) => new Edge<int>(source, target),
                random,
                0, 0, false)).Should().Throw<ArgumentOutOfRangeException>();
            Invoking(() => RandomGraphFactory.Create(
                graph,
                () => 1,
                (source, target) => new Edge<int>(source, target),
                random,
                -1, -1, false)).Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}
