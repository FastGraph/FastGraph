#nullable enable

using JetBrains.Annotations;
using NUnit.Framework;
using FastGraph.Algorithms;
using FastGraph.Algorithms.MaximumFlow;
using FastGraph.Algorithms.RandomWalks;
using FastGraph.Algorithms.TopologicalSort;
using FastGraph.Collections;
using FastGraph.Tests.Structures;
using static FastGraph.Tests.GraphTestHelpers;

namespace FastGraph.Tests.Extensions
{
    /// <summary>
    /// Tests related to <see cref="AlgorithmExtensions"/>.
    /// </summary>
    internal sealed class AlgorithmExtensionsTests : GraphTestsBase
    {
        [Test]
        public void GetIndexer()
        {
            var dictionary1 = new Dictionary<int, double>();
            Func<int, double> indexer1 = AlgorithmExtensions.GetIndexer(dictionary1);

            Invoking(() => indexer1(12)).Should().Throw<KeyNotFoundException>();

            dictionary1[12] = 42.0;
            indexer1(12).Should().Be(42.0);

            var dictionary2 = new Dictionary<TestVertex, TestVertex>();
            Func<TestVertex, TestVertex> indexer2 = AlgorithmExtensions.GetIndexer(dictionary2);

            var key = new TestVertex("1");
            var keyBis = new TestVertex("1");
            Invoking(() => indexer2(key)).Should().Throw<KeyNotFoundException>();

            var value = new TestVertex("2");
            dictionary2[key] = value;
            indexer2(key).Should().BeSameAs(value);

            Invoking(() => indexer2(keyBis)).Should().Throw<KeyNotFoundException>();
        }

        [Test]
        public void GetIndexer_Throws()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => AlgorithmExtensions.GetIndexer<int, double>(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        [Test]
        public void GetVertexIdentity()
        {
            var graph1 = new AdjacencyGraph<int, Edge<int>>();
            VertexIdentity<int> vertexIdentity1 = graph1.GetVertexIdentity();

            vertexIdentity1(12).Should().Be("12");
            vertexIdentity1(42).Should().Be("42");
            // Check identity didn't change
            vertexIdentity1(12).Should().Be("12");
            vertexIdentity1(42).Should().Be("42");

            var graph2 = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            VertexIdentity<TestVertex> vertexIdentity2 = graph2.GetVertexIdentity();

            var vertex1 = new TestVertex("12");
            var vertex2 = new TestVertex("42");
            vertexIdentity2(vertex1).Should().Be("0");
            vertexIdentity2(vertex2).Should().Be("1");
            // Check identity didn't change
            vertexIdentity2(vertex1).Should().Be("0");
            vertexIdentity2(vertex2).Should().Be("1");
        }

        [Test]
        public void GetVertexIdentity_Throws()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => AlgorithmExtensions.GetVertexIdentity<int>(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        [Test]
        public void GetEdgeIdentity()
        {
            var graph1 = new AdjacencyGraph<int, Edge<int>>();
            EdgeIdentity<int, Edge<int>> edgeIdentity1 = graph1.GetEdgeIdentity();

            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(2, 3);
            var edge3 = new Edge<int>(1, 2);
            edgeIdentity1(edge1).Should().Be("0");
            edgeIdentity1(edge2).Should().Be("1");
            edgeIdentity1(edge3).Should().Be("2");
            // Check identity didn't change
            edgeIdentity1(edge1).Should().Be("0");
            edgeIdentity1(edge2).Should().Be("1");
            edgeIdentity1(edge3).Should().Be("2");

            var graph2 = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            EdgeIdentity<TestVertex, Edge<TestVertex>> edgeIdentity2 = graph2.GetEdgeIdentity();

            var vertex1 = new TestVertex("1");
            var vertex2 = new TestVertex("2");
            var vertex3 = new TestVertex("3");
            var edge4 = new Edge<TestVertex>(vertex1, vertex2);
            var edge5 = new Edge<TestVertex>(vertex2, vertex3);
            var edge6 = new Edge<TestVertex>(vertex1, vertex2);
            edgeIdentity2(edge4).Should().Be("0");
            edgeIdentity2(edge5).Should().Be("1");
            edgeIdentity2(edge6).Should().Be("2");
            // Check identity didn't change
            edgeIdentity2(edge4).Should().Be("0");
            edgeIdentity2(edge5).Should().Be("1");
            edgeIdentity2(edge6).Should().Be("2");
        }

        [Test]
        public void GetEdgeIdentity_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => AlgorithmExtensions.GetEdgeIdentity<int, Edge<int>>(default)).Should().Throw<ArgumentNullException>();
            Invoking(() => AlgorithmExtensions.GetEdgeIdentity<TestVertex, Edge<TestVertex>>(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void TreeBreadthFirstSearch()
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge23 = new Edge<int>(2, 3);
            var edge24 = new Edge<int>(2, 4);
            var edge35 = new Edge<int>(3, 5);
            var edge36 = new Edge<int>(3, 6);

            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge13, edge23, edge24, edge35, edge36
            });
            graph.AddVertex(7);

            TryFunc<int, IEnumerable<Edge<int>>> pathAccessor = graph.TreeBreadthFirstSearch(1);

            pathAccessor(7, out _).Should().BeFalse();

            pathAccessor(5, out IEnumerable<Edge<int>>? path).Should().BeTrue();
            new[] { edge13, edge35 }.Should().BeEquivalentTo(path);
        }

        [Test]
        public void TreeBreadthFirstSearch_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();

            var vertex = new TestVertex("1");
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
#pragma warning disable CS8620
#pragma warning disable CS8714
            Invoking(() => ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>?)default).TreeBreadthFirstSearch(vertex)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.TreeBreadthFirstSearch(default)).Should().Throw<ArgumentNullException>();
            Invoking(() => ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>?)default).TreeBreadthFirstSearch(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8714
#pragma warning restore CS8620
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void TreeDepthFirstSearch()
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge23 = new Edge<int>(2, 3);
            var edge24 = new Edge<int>(2, 4);
            var edge35 = new Edge<int>(3, 5);
            var edge36 = new Edge<int>(3, 6);

            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge13, edge23, edge24, edge35, edge36
            });
            graph.AddVertex(7);

            TryFunc<int, IEnumerable<Edge<int>>> pathAccessor = graph.TreeDepthFirstSearch(1);

            pathAccessor(7, out _).Should().BeFalse();

            pathAccessor(5, out IEnumerable<Edge<int>>? path).Should().BeTrue();
            new[] { edge12, edge23, edge35 }.Should().BeEquivalentTo(path);
        }

        [Test]
        public void TreeDepthFirstSearch_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();

            var vertex = new TestVertex("1");
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
#pragma warning disable CS8620
#pragma warning disable CS8714
            Invoking(() => ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>?)default).TreeDepthFirstSearch(vertex)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.TreeDepthFirstSearch(default)).Should().Throw<ArgumentNullException>();
            Invoking(() => ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>?)default).TreeDepthFirstSearch(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8714
#pragma warning restore CS8620
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void TreeCyclePoppingRandom()
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 1);
            var edge4 = new Edge<int>(2, 3);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 2);
            var edge7 = new Edge<int>(3, 5);
            var edge8 = new Edge<int>(3, 6);
            var edge9 = new Edge<int>(4, 1);
            var edge10 = new Edge<int>(4, 2);
            var edge11 = new Edge<int>(4, 5);
            var edge12 = new Edge<int>(4, 6);
            var edge13 = new Edge<int>(5, 6);
            var edge14 = new Edge<int>(6, 2);
            var edge15 = new Edge<int>(6, 3);

            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                edge1, edge2, edge3, edge4, edge5, edge6,
                edge7, edge8, edge9, edge10, edge11,
                edge12, edge13, edge14, edge15
            });
            graph.AddVertex(7);

            TryFunc<int, IEnumerable<Edge<int>>> pathAccessor = graph.TreeCyclePoppingRandom(2);

            pathAccessor(7, out _).Should().BeFalse();

            // Would require more tests...
        }

        [Test]
        public void TreeCyclePoppingRandom_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();

            var vertex = new TestVertex("1");
            var chain = new NormalizedMarkovEdgeChain<TestVertex, Edge<TestVertex>>();
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
#pragma warning disable CS8620
#pragma warning disable CS8714
            Invoking(() => ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>?)default).TreeCyclePoppingRandom(vertex)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.TreeCyclePoppingRandom(default)).Should().Throw<ArgumentNullException>();
            Invoking(() => ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>?)default).TreeCyclePoppingRandom(default)).Should().Throw<ArgumentNullException>();

            Invoking(() => ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>?)default).TreeCyclePoppingRandom(vertex, chain)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.TreeCyclePoppingRandom(default, chain)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.TreeCyclePoppingRandom(vertex, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>?)default).TreeCyclePoppingRandom(default, chain)).Should().Throw<ArgumentNullException>();
            Invoking(() => ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>?)default).TreeCyclePoppingRandom(vertex, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.TreeCyclePoppingRandom(default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>?)default).TreeCyclePoppingRandom(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8714
#pragma warning restore CS8620
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        #region Shortest paths

        [Test]
        public void ShortestPaths_Dijkstra_AStar_BellmanFord_Dag()
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge18 = new Edge<int>(1, 8);
            var edge24 = new Edge<int>(2, 4);
            var edge25 = new Edge<int>(2, 5);
            var edge26 = new Edge<int>(2, 6);
            var edge34 = new Edge<int>(3, 4);
            var edge45 = new Edge<int>(4, 5);
            var edge46 = new Edge<int>(4, 6);
            var edge56 = new Edge<int>(5, 6);
            var edge67 = new Edge<int>(6, 7);
            var edge810 = new Edge<int>(8, 10);
            var edge95 = new Edge<int>(9, 5);
            var edge109 = new Edge<int>(10, 9);

            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge13, edge18, edge24, edge25,
                edge26, edge34, edge45, edge46, edge56,
                edge67, edge810, edge95, edge109
            });

            TryFunc<int, IEnumerable<Edge<int>>>[] algorithmResults =
            {
                graph.ShortestPathsDijkstra(_ => 1.0, 2),
                graph.ShortestPathsAStar(_ => 1.0, _ => 1.0, 2),
                graph.ShortestPathsBellmanFord(_ => 1.0, 2, out _),
                graph.ShortestPathsDag(_ => 1.0, 2)
            };

            foreach (TryFunc<int, IEnumerable<Edge<int>>> result in algorithmResults)
            {
                CheckResult(result);
            }

            #region Local function

            void CheckResult(TryFunc<int, IEnumerable<Edge<int>>> pathAccessor)
            {
                pathAccessor.Should().NotBeNull();

                pathAccessor(1, out _).Should().BeFalse();

                pathAccessor(7, out IEnumerable<Edge<int>>? path).Should().BeTrue();
                new[] { edge26, edge67 }.Should().BeEquivalentTo(path);

                pathAccessor(4, out path).Should().BeTrue();
                new[] { edge24 }.Should().BeEquivalentTo(path);
            }

            #endregion
        }

        [Test]
        public void ShortestPaths_BellmanFord_NegativeCycle()
        {
            var edge12 = new Edge<int>(1, 2);
            var edge24 = new Edge<int>(2, 4);
            var edge41 = new Edge<int>(4, 1);

            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge24, edge41
            });

            TryFunc<int, IEnumerable<Edge<int>>> pathAccessor = graph.ShortestPathsBellmanFord(
                edge =>
                {
                    if (edge == edge12)
                        return 12.0;
                    if (edge == edge24)
                        return -42.0;
                    if (edge == edge41)
                        return 22.0;
                    return 1.0;
                },
                1,
                out bool foundNegativeCycle);
            pathAccessor.Should().NotBeNull();
            foundNegativeCycle.Should().BeTrue();

            // Path accessors is usable but will generate a stack overflow
            // if accessing path using edge in the negative cycle.
        }

        [Test]
        public void ShortestPathsDijkstra_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();

            var vertex = new TestVertex("1");
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
#pragma warning disable CS8620
#pragma warning disable CS8714
            Invoking(() => ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>?)default).ShortestPathsDijkstra(_ => 1.0, vertex)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.ShortestPathsDijkstra(default, vertex)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.ShortestPathsDijkstra(_ => 1.0, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>?)default).ShortestPathsDijkstra(default, vertex)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.ShortestPathsDijkstra(default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() =>
                ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>?)default).ShortestPathsDijkstra(_ => 1.0, default)).Should().Throw<ArgumentNullException>();
            Invoking(() =>
                ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>?)default).ShortestPathsDijkstra(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8714
#pragma warning restore CS8620
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void ShortestPathsAStar_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();

            var vertex = new TestVertex("1");
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8600
#pragma warning disable CS8625
#pragma warning disable CS8620
#pragma warning disable CS8714
            Invoking(() => ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>)default).ShortestPathsAStar(_ => 1.0, _ => 1.0, vertex)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.ShortestPathsAStar(default, _ => 1.0, vertex)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.ShortestPathsAStar(_ => 1.0, default, vertex)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.ShortestPathsAStar(_ => 1.0, _ => 1.0, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>?)default).ShortestPathsAStar(default, _ => 1.0, vertex)).Should().Throw<ArgumentNullException>();
            Invoking(() => ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>?)default).ShortestPathsAStar(_ => 1.0, default, vertex)).Should().Throw<ArgumentNullException>();
            Invoking(() => ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>?)default).ShortestPathsAStar(_ => 1.0, _ => 1.0, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.ShortestPathsAStar(default, default, vertex)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.ShortestPathsAStar(default, _ => 1.0, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.ShortestPathsAStar(_ => 1.0, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() =>
                ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>?)default).ShortestPathsAStar(default, default, vertex)).Should().Throw<ArgumentNullException>();
            Invoking(() =>
                ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>?)default).ShortestPathsAStar(default, _ => 1.0, default)).Should().Throw<ArgumentNullException>();
            Invoking(() =>
                ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>?)default).ShortestPathsAStar(_ => 1.0, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.ShortestPathsAStar(default, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() =>
                ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>?)default).ShortestPathsAStar(default, default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8714
#pragma warning restore CS8620
#pragma warning restore CS8625
#pragma warning restore CS8600
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void ShortestPathsBellmanFord_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();

            var vertex = new TestVertex("1");
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
#pragma warning disable CS8620
#pragma warning disable CS8714
            Invoking(() => ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>?)default).ShortestPathsBellmanFord(_ => 1.0, vertex, out _)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.ShortestPathsBellmanFord(default, vertex, out _)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.ShortestPathsBellmanFord(_ => 1.0, default, out _)).Should().Throw<ArgumentNullException>();
            Invoking(() => ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>?)default).ShortestPathsBellmanFord(default, vertex, out _)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.ShortestPathsBellmanFord(default, default, out _)).Should().Throw<ArgumentNullException>();
            Invoking(() =>
                ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>?)default).ShortestPathsBellmanFord(_ => 1.0, default, out _)).Should().Throw<ArgumentNullException>();
            Invoking(() =>
                ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>?)default).ShortestPathsBellmanFord(default, default, out _)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8714
#pragma warning restore CS8620
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void ShortestPathsDag_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();

            var vertex = new TestVertex("1");
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
#pragma warning disable CS8620
#pragma warning disable CS8714
            Invoking(() => ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>?)default).ShortestPathsDag(_ => 1.0, vertex)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.ShortestPathsDag(default, vertex)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.ShortestPathsDag(_ => 1.0, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>?)default).ShortestPathsDag(default, vertex)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.ShortestPathsDag(default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() =>
                ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>?)default).ShortestPathsDag(_ => 1.0, default)).Should().Throw<ArgumentNullException>();
            Invoking(() =>
                ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>?)default).ShortestPathsDag(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8714
#pragma warning restore CS8620
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void ShortestPaths_UndirectedDijkstra()
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge18 = new Edge<int>(1, 8);
            var edge45 = new Edge<int>(4, 5);
            var edge46 = new Edge<int>(4, 6);
            var edge56 = new Edge<int>(5, 6);
            var edge67 = new Edge<int>(6, 7);
            var edge810 = new Edge<int>(8, 10);

            var graph = new UndirectedGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge13, edge18, edge45,
                edge46, edge56, edge67, edge810
            });
            graph.AddVertex(9);

            TryFunc<int, IEnumerable<Edge<int>>> pathAccessor = graph.ShortestPathsDijkstra(_ => 1.0, 2);
            pathAccessor.Should().NotBeNull();

            pathAccessor(9, out _).Should().BeFalse();

            pathAccessor(8, out IEnumerable<Edge<int>>? path).Should().BeTrue();
            new[] { edge12, edge18 }.Should().BeEquivalentTo(path);

            pathAccessor(1, out path).Should().BeTrue();
            new[] { edge12 }.Should().BeEquivalentTo(path);
        }

        [Test]
        public void ShortestPathsUndirectedDijkstra_Throws()
        {
            var graph = new UndirectedGraph<TestVertex, Edge<TestVertex>>();

            var vertex = new TestVertex("1");
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
#pragma warning disable CS8620
#pragma warning disable CS8714
            Invoking(() => ((IUndirectedGraph<TestVertex, Edge<TestVertex>>?)default).ShortestPathsDijkstra(_ => 1.0, vertex)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.ShortestPathsDijkstra(default, vertex)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.ShortestPathsDijkstra(_ => 1.0, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => ((IUndirectedGraph<TestVertex, Edge<TestVertex>>?)default).ShortestPathsDijkstra(default, vertex)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.ShortestPathsDijkstra(default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() =>
                ((IUndirectedGraph<TestVertex, Edge<TestVertex>>?)default).ShortestPathsDijkstra(_ => 1.0, default)).Should().Throw<ArgumentNullException>();
            Invoking(() =>
                ((IUndirectedGraph<TestVertex, Edge<TestVertex>>?)default).ShortestPathsDijkstra(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8714
#pragma warning restore CS8620
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        #endregion

        #region K-Shortest path

        [Test]
        public void RankedShortestPathHoffmanPavley()
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge18 = new Edge<int>(1, 8);
            var edge21 = new Edge<int>(2, 1);
            var edge24 = new Edge<int>(2, 4);
            var edge25 = new Edge<int>(2, 5);
            var edge26 = new Edge<int>(2, 6);
            var edge33 = new Edge<int>(3, 3);
            var edge34 = new Edge<int>(3, 4);
            var edge45 = new Edge<int>(4, 5);
            var edge46 = new Edge<int>(4, 6);
            var edge56 = new Edge<int>(5, 6);
            var edge67 = new Edge<int>(6, 7);
            var edge810 = new Edge<int>(8, 10);
            var edge95 = new Edge<int>(9, 5);
            var edge109 = new Edge<int>(10, 9);

            var graph = new BidirectionalGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge13, edge18, edge21, edge24,
                edge25, edge26, edge33, edge34, edge45,
                edge46, edge56, edge67, edge810, edge95,
                edge109
            });

            IEnumerable<IEnumerable<Edge<int>>> paths = graph.RankedShortestPathHoffmanPavley(_ => 1.0, 1, 5, 5);
            new[]
            {
                new[] { edge12, edge25 },
                new[] { edge13, edge34, edge45 },
                new[] { edge12, edge24, edge45 },
                new[] { edge18, edge810, edge109, edge95 }
            }.Should().BeEquivalentTo(paths);

            paths = graph.RankedShortestPathHoffmanPavley(_ => 1.0, 1, 5);
            new[]
            {
                new[] { edge12, edge25 },
                new[] { edge13, edge34, edge45 },
                new[] { edge12, edge24, edge45 }
            }.Should().BeEquivalentTo(paths);
        }

        [Test]
        public void RankedShortestPathHoffmanPavley_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();

            var vertex = new TestVertex("1");
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
#pragma warning disable CS8620
#pragma warning disable CS8714
            Invoking(() => ((IBidirectionalGraph<TestVertex, Edge<TestVertex>>?)default).RankedShortestPathHoffmanPavley(_ => 1.0, vertex, vertex, int.MaxValue)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.RankedShortestPathHoffmanPavley(default, vertex, vertex, int.MaxValue)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.RankedShortestPathHoffmanPavley(_ => 1.0, default, vertex, int.MaxValue)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.RankedShortestPathHoffmanPavley(_ => 1.0, vertex, default, int.MaxValue)).Should().Throw<ArgumentNullException>();
            Invoking(() => ((IBidirectionalGraph<TestVertex, Edge<TestVertex>>?)default).RankedShortestPathHoffmanPavley(default, vertex, vertex, int.MaxValue)).Should().Throw<ArgumentNullException>();
            Invoking(() => ((IBidirectionalGraph<TestVertex, Edge<TestVertex>>?)default).RankedShortestPathHoffmanPavley(_ => 1.0, default, vertex, int.MaxValue)).Should().Throw<ArgumentNullException>();
            Invoking(() => ((IBidirectionalGraph<TestVertex, Edge<TestVertex>>?)default).RankedShortestPathHoffmanPavley(_ => 1.0, vertex, default, int.MaxValue)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.RankedShortestPathHoffmanPavley(default, default, vertex, int.MaxValue)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.RankedShortestPathHoffmanPavley(default, vertex, default, int.MaxValue)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.RankedShortestPathHoffmanPavley(_ => 1.0, default, default, int.MaxValue)).Should().Throw<ArgumentNullException>();
            Invoking(() => ((IBidirectionalGraph<TestVertex, Edge<TestVertex>>?)default).RankedShortestPathHoffmanPavley(default, default, vertex, int.MaxValue)).Should().Throw<ArgumentNullException>();
            Invoking(() => ((IBidirectionalGraph<TestVertex, Edge<TestVertex>>?)default).RankedShortestPathHoffmanPavley(default, vertex, default, int.MaxValue)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.RankedShortestPathHoffmanPavley(default, default, default, int.MaxValue)).Should().Throw<ArgumentNullException>();
            Invoking(() => ((IBidirectionalGraph<TestVertex, Edge<TestVertex>>?)default).RankedShortestPathHoffmanPavley(default, default, default, int.MaxValue)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8714
#pragma warning restore CS8620
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute

            Invoking(() => graph.RankedShortestPathHoffmanPavley(_ => 1.0, vertex, vertex, 0)).Should().Throw<ArgumentOutOfRangeException>();
            Invoking(() => graph.RankedShortestPathHoffmanPavley(_ => 1.0, vertex, vertex, -1)).Should().Throw<ArgumentOutOfRangeException>();
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        #endregion

        [Pure]
        private static IEnumerable<TestCaseData> CreateSinksTestCases(
            [InstantHandle] Func<IMutableVertexAndEdgeSet<int, Edge<int>>> createGraph)
        {
            yield return new TestCaseData(
                createGraph(),
                Enumerable.Empty<int>());

            var edge12 = new Edge<int>(1, 2);
            var edge14 = new Edge<int>(1, 4);
            var edge22 = new Edge<int>(2, 2);
            var edge23 = new Edge<int>(2, 3);
            var edge24 = new Edge<int>(2, 4);
            var edge25 = new Edge<int>(2, 5);
            var edge35 = new Edge<int>(3, 5);
            var edge41 = new Edge<int>(4, 1);
            var edge45 = new Edge<int>(4, 5);
            var edge46 = new Edge<int>(4, 6);

            IMutableVertexAndEdgeSet<int, Edge<int>> cycleGraph = createGraph();
            cycleGraph.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge24, edge41
            });
            yield return new TestCaseData(
                cycleGraph,
                Enumerable.Empty<int>());

            IMutableVertexAndEdgeSet<int, Edge<int>> cycleGraph2 = createGraph();
            cycleGraph2.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge24, edge25, edge35, edge41, edge22
            });
            yield return new TestCaseData(
                cycleGraph2,
                new[] { 5 });

            IMutableVertexAndEdgeSet<int, Edge<int>> graph1 = createGraph();
            graph1.AddVerticesAndEdgeRange(new[]
            {
                edge22
            });
            yield return new TestCaseData(
                graph1,
                Enumerable.Empty<int>());

            IMutableVertexAndEdgeSet<int, Edge<int>> graph2 = createGraph();
            graph2.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge14, edge23, edge24, edge35, edge45
            });
            yield return new TestCaseData(
                graph2,
                new[] { 5 });

            IMutableVertexAndEdgeSet<int, Edge<int>> graph3 = createGraph();
            graph3.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge14, edge24, edge35, edge45, edge46
            });
            yield return new TestCaseData(
                graph3,
                new[] { 5, 6 });
        }

        private static IEnumerable<TestCaseData> SinksTestCases
        {
            [UsedImplicitly]
            get
            {
                IEnumerable<TestCaseData> testCases = CreateSinksTestCases(() => new AdjacencyGraph<int, Edge<int>>())
                    .Concat(CreateSinksTestCases(() => new BidirectionalGraph<int, Edge<int>>()));
                foreach (TestCaseData testCase in testCases)
                {
                    yield return testCase;
                }
            }
        }

        [TestCaseSource(nameof(SinksTestCases))]
        public void Sinks(
            IVertexListGraph<int, Edge<int>> graph,
            IEnumerable<int> expectedSinks)
        {
            graph.Sinks().Should().BeEquivalentTo(expectedSinks);
        }

        [Test]
        public void Sinks_Throws()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8600
#pragma warning disable CS8625
            Invoking(() => ((IVertexListGraph<int, Edge<int>>)default).Sinks().ToArray()).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
#pragma warning restore CS8600
        }

        [Pure]
        private static IEnumerable<TestCaseData> CreateRootsTestCases(
            [InstantHandle] Func<IMutableVertexAndEdgeSet<int, Edge<int>>> createGraph)
        {
            yield return new TestCaseData(
                createGraph(),
                Enumerable.Empty<int>());

            var edge12 = new Edge<int>(1, 2);
            var edge14 = new Edge<int>(1, 4);
            var edge22 = new Edge<int>(2, 2);
            var edge23 = new Edge<int>(2, 3);
            var edge24 = new Edge<int>(2, 4);
            var edge25 = new Edge<int>(2, 5);
            var edge35 = new Edge<int>(3, 5);
            var edge41 = new Edge<int>(4, 1);
            var edge45 = new Edge<int>(4, 5);
            var edge46 = new Edge<int>(4, 6);

            IMutableVertexAndEdgeSet<int, Edge<int>> cycleGraph = createGraph();
            cycleGraph.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge24, edge41
            });
            yield return new TestCaseData(
                cycleGraph,
                Enumerable.Empty<int>());

            IMutableVertexAndEdgeSet<int, Edge<int>> cycleGraph2 = createGraph();
            cycleGraph2.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge24, edge25, edge35, edge41, edge22
            });
            yield return new TestCaseData(
                cycleGraph2,
                new[] { 3 });

            IMutableVertexAndEdgeSet<int, Edge<int>> graph1 = createGraph();
            graph1.AddVerticesAndEdgeRange(new[]
            {
                edge22
            });
            yield return new TestCaseData(
                graph1,
                Enumerable.Empty<int>());

            IMutableVertexAndEdgeSet<int, Edge<int>> graph2 = createGraph();
            graph2.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge14, edge23, edge24, edge35, edge45
            });
            yield return new TestCaseData(
                graph2,
                new[] { 1 });

            IMutableVertexAndEdgeSet<int, Edge<int>> graph3 = createGraph();
            graph3.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge14, edge24, edge35, edge45, edge46
            });
            yield return new TestCaseData(
                graph3,
                new[] { 1, 3 });
        }

        private static IEnumerable<TestCaseData> RootsTestCases
        {
            [UsedImplicitly]
            get
            {
                return CreateRootsTestCases(() => new AdjacencyGraph<int, Edge<int>>());
            }
        }

        [TestCaseSource(nameof(RootsTestCases))]
        public void Roots_NotBidirectional(
            IVertexListGraph<int, Edge<int>> graph,
            IEnumerable<int> expectedRoots)
        {
            graph.Roots().Should().BeEquivalentTo(expectedRoots);
        }

        [Test]
        public void AdjacencyGraphRoots()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs_All())
                CheckRoots(graph);

            #region Local function

            void CheckRoots<T>(IVertexAndEdgeListGraph<T, Edge<T>> graph)
                where T : notnull
            {
                var roots = new HashSet<T>(graph.Roots());
                foreach (Edge<T> edge in graph.Edges)
                    roots.Contains(edge.Target).Should().BeFalse();
            }

            #endregion
        }

        private static IEnumerable<TestCaseData> BidirectionalRootsTestCases
        {
            [UsedImplicitly]
            get
            {
                return CreateRootsTestCases(() => new BidirectionalGraph<int, Edge<int>>());
            }
        }

        [TestCaseSource(nameof(BidirectionalRootsTestCases))]
        public void Roots_Bidirectional(
            IBidirectionalGraph<int, Edge<int>> graph,
            IEnumerable<int> expectedRoots)
        {
            graph.Roots().Should().BeEquivalentTo(expectedRoots);
        }

        [Test]
        public void Roots_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => ((IVertexListGraph<int, Edge<int>>?)default).Roots().ToArray()).Should().Throw<ArgumentNullException>();
            Invoking(() => ((IBidirectionalGraph<int, Edge<int>>?)default).Roots().ToArray()).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        private static IEnumerable<TestCaseData> IsolatedVerticesTestCases
        {
            [UsedImplicitly]
            get
            {
                yield return new TestCaseData(
                    new BidirectionalGraph<int, Edge<int>>(),
                    Enumerable.Empty<int>());

                var edge12 = new Edge<int>(1, 2);
                var edge14 = new Edge<int>(1, 4);
                var edge22 = new Edge<int>(2, 2);
                var edge23 = new Edge<int>(2, 3);
                var edge24 = new Edge<int>(2, 4);
                var edge26 = new Edge<int>(2, 6);
                var edge35 = new Edge<int>(3, 5);
                var edge36 = new Edge<int>(3, 6);
                var edge41 = new Edge<int>(4, 1);
                var edge45 = new Edge<int>(4, 5);
                var edge46 = new Edge<int>(4, 6);

                var cycleGraph = new BidirectionalGraph<int, Edge<int>>();
                cycleGraph.AddVerticesAndEdgeRange(new[]
                {
                    edge12, edge24, edge41
                });
                yield return new TestCaseData(
                    cycleGraph,
                    Enumerable.Empty<int>());

                var cycleGraph2 = new BidirectionalGraph<int, Edge<int>>();
                cycleGraph2.AddVerticesAndEdgeRange(new[]
                {
                    edge12, edge24, edge41, edge22
                });
                yield return new TestCaseData(
                    cycleGraph2,
                    Enumerable.Empty<int>());

                var cycleGraph3 = new BidirectionalGraph<int, Edge<int>>();
                cycleGraph3.AddVerticesAndEdgeRange(new[]
                {
                    edge22
                });
                yield return new TestCaseData(
                    cycleGraph3,
                    Enumerable.Empty<int>());

                var cycleGraph4 = new BidirectionalGraph<int, Edge<int>>();
                cycleGraph4.AddVerticesAndEdgeRange(new[]
                {
                    edge12, edge22, edge24, edge41
                });
                cycleGraph4.AddVertex(5);
                yield return new TestCaseData(
                    cycleGraph4,
                    new[] { 5 });

                var graph1 = new BidirectionalGraph<int, Edge<int>>();
                graph1.AddVertexRange(new[] { 4, 5 });
                graph1.AddVerticesAndEdgeRange(new[]
                {
                    edge12, edge23, edge26, edge36
                });
                yield return new TestCaseData(
                    graph1,
                    new[] { 4, 5 });

                var graph2 = new BidirectionalGraph<int, Edge<int>>();
                graph2.AddVerticesAndEdgeRange(new[]
                {
                    edge12, edge14, edge23, edge24, edge26, edge35, edge45, edge46
                });
                yield return new TestCaseData(
                    graph2,
                    Enumerable.Empty<int>());
            }
        }

        [TestCaseSource(nameof(IsolatedVerticesTestCases))]
        public void IsolatedVertices(
            IBidirectionalGraph<int, Edge<int>> graph,
            IEnumerable<int> expectedIsolatedVertices)
        {
            graph.IsolatedVertices().Should().BeEquivalentTo(expectedIsolatedVertices);
        }

        [Test]
        public void IsolatedVertices_Throws()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => ((BidirectionalGraph<int, Edge<int>>?)default).IsolatedVertices()).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        #region Topological sort

        [Test]
        public void TopologicalSort()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(2, 4),
                new Edge<int>(3, 1),
                new Edge<int>(3, 5),
                new Edge<int>(5, 7),
                new Edge<int>(6, 3),
                new Edge<int>(6, 7)
            });

            new[] { 6, 3, 5, 7, 1, 2, 4 }.Should().BeEquivalentTo(graph.TopologicalSort());
        }

        [Test]
        public void TopologicalSort_Undirected()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(2, 4),
                new Edge<int>(3, 1),
                new Edge<int>(3, 5),
                new Edge<int>(5, 7),
                new Edge<int>(6, 7)
            });

            new[] { 1, 3, 5, 7, 6, 2, 4 }.Should().BeEquivalentTo(graph.TopologicalSort());
        }

        [Test]
        public void TopologicalSort_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => ((IVertexListGraph<int, Edge<int>>?)default).TopologicalSort()).Should().Throw<ArgumentNullException>();

            Invoking(() => ((IUndirectedGraph<int, Edge<int>>?)default).TopologicalSort()).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void SourceFirstTopologicalSort()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(2, 4),
                new Edge<int>(3, 1),
                new Edge<int>(3, 5),
                new Edge<int>(5, 7),
                new Edge<int>(6, 3),
                new Edge<int>(6, 7)
            });

            new[] { 6, 3, 1, 5, 2, 7, 4 }.Should().BeEquivalentTo(graph.SourceFirstTopologicalSort());
        }

        [Test]
        public void SourceFirstTopologicalSort_Undirected()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(2, 4),
                new Edge<int>(3, 1),
                new Edge<int>(3, 5),
                new Edge<int>(5, 7),
                new Edge<int>(6, 7)
            });

            new[] { 4, 6, 2, 7, 1, 5, 3 }.Should().BeEquivalentTo(graph.SourceFirstTopologicalSort());
        }

        [Test]
        public void SourceFirstTopologicalSort_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => ((IVertexAndEdgeListGraph<int, Edge<int>>?)default).SourceFirstTopologicalSort()).Should().Throw<ArgumentNullException>();

            Invoking(() => ((IUndirectedGraph<int, Edge<int>>?)default).SourceFirstTopologicalSort()).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void SourceFirstBidirectionalTopologicalSort()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(2, 4),
                new Edge<int>(3, 1),
                new Edge<int>(3, 5),
                new Edge<int>(5, 7),
                new Edge<int>(6, 3),
                new Edge<int>(6, 7)
            });

            graph.SourceFirstBidirectionalTopologicalSort().Should().BeEquivalentTo(new[] { 6, 3, 1, 5, 2, 7, 4 });

            graph.SourceFirstBidirectionalTopologicalSort(TopologicalSortDirection.Forward).Should().BeEquivalentTo(new[] { 6, 3, 1, 5, 2, 7, 4 });

            graph.SourceFirstBidirectionalTopologicalSort(TopologicalSortDirection.Backward).Should().BeEquivalentTo(new[] { 4, 7, 2, 5, 1, 3, 6 });
        }

        [Test]
        public void SourceFirstBidirectionalTopologicalSort_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => ((IBidirectionalGraph<int, Edge<int>>?)default).SourceFirstBidirectionalTopologicalSort()).Should().Throw<ArgumentNullException>();

            Invoking(() => ((IBidirectionalGraph<int, Edge<int>>?)default).SourceFirstBidirectionalTopologicalSort(TopologicalSortDirection.Forward)).Should().Throw<ArgumentNullException>();
            Invoking(() => ((IBidirectionalGraph<int, Edge<int>>?)default).SourceFirstBidirectionalTopologicalSort(TopologicalSortDirection.Backward)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        #endregion

        #region Connected components

        [Test]
        public void ConnectedComponents()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(1, 4),
                new Edge<int>(2, 3),
                new Edge<int>(3, 1),
                new Edge<int>(4, 5),
                new Edge<int>(5, 6),
                new Edge<int>(6, 7),
                new Edge<int>(7, 5),

                new Edge<int>(8, 9)
            });

            var components = new Dictionary<int, int>();

            graph.ConnectedComponents(components).Should().Be(2);
            components.Should().BeEquivalentTo(new Dictionary<int, int>
            {
                [1] = 0,
                [2] = 0,
                [3] = 0,
                [4] = 0,
                [5] = 0,
                [6] = 0,
                [7] = 0,
                [8] = 1,
                [9] = 1
            });
        }

        [Test]
        public void ConnectedComponents_Throws()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            var components = new Dictionary<int, int>();

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => graph.ConnectedComponents(default)).Should().Throw<ArgumentNullException>();
            Invoking(() => AlgorithmExtensions.ConnectedComponents<int, Edge<int>>(default, components)).Should().Throw<ArgumentNullException>();
            Invoking(() => AlgorithmExtensions.ConnectedComponents<int, Edge<int>>(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void IncrementalConnectedComponent()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVertexRange(new[] { 0, 1, 2, 3 });
            using (graph.IncrementalConnectedComponents(
                out Func<KeyValuePair<int, IDictionary<int, int>>> getComponents))
            {
                KeyValuePair<int, IDictionary<int, int>> current = getComponents();
                current.Key.Should().Be(4);

                graph.AddEdge(new Edge<int>(0, 1));
                current = getComponents();
                current.Key.Should().Be(3);

                graph.AddEdge(new Edge<int>(2, 3));
                current = getComponents();
                current.Key.Should().Be(2);

                graph.AddEdge(new Edge<int>(1, 3));
                current = getComponents();
                current.Key.Should().Be(1);

                graph.AddVertex(4);
                current = getComponents();
                current.Key.Should().Be(2);
            }
        }

        [Test]
        public void IncrementalConnectedComponent_Throws()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => AlgorithmExtensions.IncrementalConnectedComponents<int, Edge<int>>(default, out _)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        [Test]
        public void StronglyConnectedComponents()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(1, 4),
                new Edge<int>(2, 3),
                new Edge<int>(3, 1),
                new Edge<int>(4, 5),
                new Edge<int>(5, 6),
                new Edge<int>(6, 7),
                new Edge<int>(7, 5)
            });

            var components = new Dictionary<int, int>();

            graph.StronglyConnectedComponents(components).Should().Be(3);
            components.Should().BeEquivalentTo(new Dictionary<int, int>
            {
                [1] = 2,
                [2] = 2,
                [3] = 2,
                [4] = 1,
                [5] = 0,
                [6] = 0,
                [7] = 0
            });
        }

        [Test]
        public void StronglyConnectedComponents_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var components = new Dictionary<int, int>();

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => graph.StronglyConnectedComponents(default)).Should().Throw<ArgumentNullException>();
            Invoking(() => AlgorithmExtensions.StronglyConnectedComponents<int, Edge<int>>(default, components)).Should().Throw<ArgumentNullException>();
            Invoking(() => AlgorithmExtensions.StronglyConnectedComponents<int, Edge<int>>(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void WeaklyConnectedComponents()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(1, 4),
                new Edge<int>(2, 3),
                new Edge<int>(3, 1),
                new Edge<int>(4, 5),
                new Edge<int>(5, 6),
                new Edge<int>(6, 7),
                new Edge<int>(7, 5),

                new Edge<int>(8, 9)
            });

            var components = new Dictionary<int, int>();

            graph.WeaklyConnectedComponents(components).Should().Be(2);
            components.Should().BeEquivalentTo(new Dictionary<int, int>
            {
                [1] = 0,
                [2] = 0,
                [3] = 0,
                [4] = 0,
                [5] = 0,
                [6] = 0,
                [7] = 0,
                [8] = 1,
                [9] = 1
            });
        }

        [Test]
        public void WeaklyConnectedComponents_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var components = new Dictionary<int, int>();

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => graph.WeaklyConnectedComponents(default)).Should().Throw<ArgumentNullException>();
            Invoking(() => AlgorithmExtensions.WeaklyConnectedComponents<int, Edge<int>>(default, components)).Should().Throw<ArgumentNullException>();
            Invoking(() => AlgorithmExtensions.WeaklyConnectedComponents<int, Edge<int>>(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void StronglyCondensedGraph_Throws()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => AlgorithmExtensions.CondensateStronglyConnected<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        [Test]
        public void WeaklyCondensedGraph_Throws()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => AlgorithmExtensions.CondensateWeaklyConnected<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        [Test]
        public void EdgesCondensedGraph_Throws()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => ((IBidirectionalGraph<int, Edge<int>>?)default).CondensateEdges(_ => true)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.CondensateEdges(default)).Should().Throw<ArgumentNullException>();
            Invoking(() => ((IBidirectionalGraph<int, Edge<int>>?)default).CondensateEdges(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        #endregion

        private static IEnumerable<TestCaseData> OddVerticesTestCases
        {
            [UsedImplicitly]
            get
            {
                yield return new TestCaseData(
                    new AdjacencyGraph<int, Edge<int>>(),
                    Enumerable.Empty<int>());

                var edge12 = new Edge<int>(1, 2);
                var edge14 = new Edge<int>(1, 4);
                var edge22 = new Edge<int>(2, 2);
                var edge23 = new Edge<int>(2, 3);
                var edge24 = new Edge<int>(2, 4);
                var edge25 = new Edge<int>(2, 5);
                var edge26 = new Edge<int>(2, 6);
                var edge35 = new Edge<int>(3, 5);
                var edge41 = new Edge<int>(4, 1);
                var edge45 = new Edge<int>(4, 5);
                var edge46 = new Edge<int>(4, 6);

                var cycleGraph = new AdjacencyGraph<int, Edge<int>>();
                cycleGraph.AddVerticesAndEdgeRange(new[]
                {
                    edge12, edge24, edge41
                });
                yield return new TestCaseData(
                    cycleGraph,
                    Enumerable.Empty<int>());

                var cycleGraph2 = new AdjacencyGraph<int, Edge<int>>();
                cycleGraph2.AddVerticesAndEdgeRange(new[]
                {
                    edge12, edge24, edge41, edge22
                });
                yield return new TestCaseData(
                    cycleGraph2,
                    Enumerable.Empty<int>());

                var cycleGraph3 = new AdjacencyGraph<int, Edge<int>>();
                cycleGraph3.AddVerticesAndEdgeRange(new[]
                {
                    edge12, edge24, edge25, edge35, edge41, edge22
                });
                yield return new TestCaseData(
                    cycleGraph3,
                    new[] { 2, 3 });

                var cycleGraph4 = new AdjacencyGraph<int, Edge<int>>();
                cycleGraph4.AddVerticesAndEdgeRange(new[]
                {
                    edge12, edge22, edge24, edge25, edge35, edge41, edge45
                });
                yield return new TestCaseData(
                    cycleGraph4,
                    new[] { 2, 3, 4, 5 });

                var graph1 = new AdjacencyGraph<int, Edge<int>>();
                graph1.AddVerticesAndEdgeRange(new[]
                {
                    edge12, edge14, edge23, edge24, edge35, edge45
                });
                yield return new TestCaseData(
                    graph1,
                    new[] { 2, 4 });

                var graph2 = new AdjacencyGraph<int, Edge<int>>();
                graph2.AddVerticesAndEdgeRange(new[]
                {
                    edge12, edge14, edge23, edge24, edge26, edge35, edge45, edge46
                });
                yield return new TestCaseData(
                    graph2,
                    Enumerable.Empty<int>());
            }
        }

        [TestCaseSource(nameof(OddVerticesTestCases))]
        public void OddVertices(
            IVertexAndEdgeListGraph<int, Edge<int>> graph,
            IEnumerable<int> expectedOddVertices)
        {
            graph.OddVertices().Should().BeEquivalentTo(expectedOddVertices);
        }

        [Test]
        public void OddVertices_Throws()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => ((AdjacencyGraph<int, Edge<int>>?)default).OddVertices()).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        [Pure]
        private static IEnumerable<TestCaseData> CreateIsDirectedAcyclicGraphTestCases(
            [InstantHandle] Func<IMutableVertexAndEdgeSet<int, Edge<int>>> createGraph)
        {
            // Empty graph
            yield return new TestCaseData(createGraph())
            {
                ExpectedResult = true
            };

            var edge12 = new Edge<int>(1, 2);
            var edge14 = new Edge<int>(1, 4);
            var edge23 = new Edge<int>(2, 3);
            var edge24 = new Edge<int>(2, 4);

            // Not empty acyclic
            var adjacencyGraph = createGraph();
            adjacencyGraph.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge14, edge23, edge24
            });
            yield return new TestCaseData(adjacencyGraph)
            {
                ExpectedResult = true
            };

            // Not acyclic
            var edge22 = new Edge<int>(2, 2);
            var cyclicGraph1 = createGraph();
            cyclicGraph1.AddVerticesAndEdge(edge22);
            yield return new TestCaseData(cyclicGraph1)
            {
                ExpectedResult = false
            };

            var cyclicGraph2 = createGraph();
            cyclicGraph2.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge14, edge22, edge23, edge24
            });
            yield return new TestCaseData(cyclicGraph2)
            {
                ExpectedResult = false
            };

            var edge41 = new Edge<int>(4, 1);
            var cyclicGraph3 = createGraph();
            cyclicGraph3.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge14, edge23, edge24, edge41
            });
            yield return new TestCaseData(cyclicGraph3)
            {
                ExpectedResult = false
            };
        }

        private static IEnumerable<TestCaseData> IsDirectedAcyclicGraphTestCases
        {
            [UsedImplicitly]
            get
            {
                IEnumerable<TestCaseData> testCases = CreateIsDirectedAcyclicGraphTestCases(() => new AdjacencyGraph<int, Edge<int>>())
                    .Concat(CreateIsDirectedAcyclicGraphTestCases(() => new BidirectionalGraph<int, Edge<int>>()));
                foreach (TestCaseData testCase in testCases)
                {
                    yield return testCase;
                }
            }
        }

        [TestCaseSource(nameof(IsDirectedAcyclicGraphTestCases))]
        public bool IsDirectedAcyclicGraph(IVertexListGraph<int, Edge<int>> graph)
        {
            return graph.IsDirectedAcyclicGraph();
        }

        [Test]
        public void IsDirectedAcyclicGraph_Throws()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => ((AdjacencyGraph<int, Edge<int>>?)default).IsDirectedAcyclicGraph()).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        [Test]
        public void ComputePredecessorCost()
        {
            var predecessors = new Dictionary<int, Edge<int>>();
            var edgeCosts = new Dictionary<Edge<int>, double>();

            AlgorithmExtensions.ComputePredecessorCost(predecessors, edgeCosts, 1).Should().Be(0);

            var edge12 = new Edge<int>(1, 2);
            predecessors[2] = edge12;
            edgeCosts[edge12] = 12;
            AlgorithmExtensions.ComputePredecessorCost(predecessors, edgeCosts, 1).Should().Be(0);
            AlgorithmExtensions.ComputePredecessorCost(predecessors, edgeCosts, 2).Should().Be(12);

            var edge31 = new Edge<int>(3, 1);
            predecessors[1] = edge31;
            edgeCosts[edge31] = -5;
            var edge34 = new Edge<int>(3, 4);
            predecessors[4] = edge34;
            edgeCosts[edge34] = 42;

            AlgorithmExtensions.ComputePredecessorCost(predecessors, edgeCosts, 1).Should().Be(-5);
            AlgorithmExtensions.ComputePredecessorCost(predecessors, edgeCosts, 2).Should().Be(7);
            AlgorithmExtensions.ComputePredecessorCost(predecessors, edgeCosts, 3).Should().Be(0);
            AlgorithmExtensions.ComputePredecessorCost(predecessors, edgeCosts, 4).Should().Be(42);
        }

        [Test]
        public void ComputePredecessorCost_Throws()
        {
            var predecessors = new Dictionary<TestVertex, Edge<TestVertex>>();
            var edgeCosts = new Dictionary<Edge<TestVertex>, double>();
            var vertex1 = new TestVertex("1");
            var vertex2 = new TestVertex("2");

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
#pragma warning disable CS8620
#pragma warning disable CS8714
            Invoking(() => AlgorithmExtensions.ComputePredecessorCost(default, edgeCosts, vertex1)).Should().Throw<ArgumentNullException>();
            Invoking(() => AlgorithmExtensions.ComputePredecessorCost(predecessors, default, vertex1)).Should().Throw<ArgumentNullException>();
            Invoking(() => AlgorithmExtensions.ComputePredecessorCost(predecessors, edgeCosts, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => AlgorithmExtensions.ComputePredecessorCost<TestVertex, Edge<TestVertex>>(default, default, vertex1)).Should().Throw<ArgumentNullException>();
            Invoking(() => AlgorithmExtensions.ComputePredecessorCost(predecessors, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => AlgorithmExtensions.ComputePredecessorCost<TestVertex, Edge<TestVertex>>(default, default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8714
#pragma warning restore CS8620
#pragma warning restore CS8625

            // Wrong usage
            predecessors[vertex2] = new Edge<TestVertex>(vertex1, vertex2);
            Invoking(() => AlgorithmExtensions.ComputePredecessorCost(predecessors, edgeCosts, vertex2)).Should().Throw<KeyNotFoundException>();
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void ComputeDisjointSet()
        {
            var emptyGraph = new UndirectedGraph<int, Edge<int>>();
            IDisjointSet<int> disjointSet = emptyGraph.ComputeDisjointSet();
            disjointSet.ElementCount.Should().Be(0);
            disjointSet.SetCount.Should().Be(0);

            var graph = new UndirectedGraph<int, Edge<int>>();
            graph.AddVertexRange(new[] { 1, 2, 3, 4 });
            disjointSet = graph.ComputeDisjointSet();
            disjointSet.ElementCount.Should().Be(4);
            disjointSet.SetCount.Should().Be(4);

            graph.AddEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(2, 3),
                new Edge<int>(1, 4)
            });
            graph.AddVertex(5);
            disjointSet = graph.ComputeDisjointSet();
            disjointSet.ElementCount.Should().Be(5);
            disjointSet.SetCount.Should().Be(2);
            disjointSet.AreInSameSet(1, 2).Should().BeTrue();
            disjointSet.AreInSameSet(1, 3).Should().BeTrue();
            disjointSet.AreInSameSet(1, 4).Should().BeTrue();
            disjointSet.AreInSameSet(1, 5).Should().BeFalse();
        }

        [Test]
        public void ComputeDisjointSet_Throws()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => ((UndirectedGraph<int, Edge<int>>?)default).ComputeDisjointSet()).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        [Test]
        public void MinimumSpanningTreePrim_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new UndirectedGraph<int, Edge<int>>().MinimumSpanningTreePrim(default)).Should().Throw<ArgumentNullException>();
            Invoking(() => ((UndirectedGraph<int, Edge<int>>?)default).MinimumSpanningTreePrim(_ => 1.0)).Should().Throw<ArgumentNullException>();
            Invoking(() => ((UndirectedGraph<int, Edge<int>>?)default).MinimumSpanningTreePrim(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void MinimumSpanningTreeKruskal_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new UndirectedGraph<int, Edge<int>>().MinimumSpanningTreeKruskal(default)).Should().Throw<ArgumentNullException>();
            Invoking(() => ((UndirectedGraph<int, Edge<int>>?)default).MinimumSpanningTreeKruskal(_ => 1.0)).Should().Throw<ArgumentNullException>();
            Invoking(() => ((UndirectedGraph<int, Edge<int>>?)default).MinimumSpanningTreeKruskal(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void OfflineLeastCommonAncestor_Throws()
        {
            var vertex1 = new TestVertex("1");
            var vertex2 = new TestVertex("2");
            var graph1 = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            graph1.AddVertexRange(new[] { vertex1, vertex2 });
            var pairs1 = new[] { new SEquatableEdge<TestVertex>(vertex1, vertex2) };

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
#pragma warning disable CS8620
#pragma warning disable CS8714
            Invoking(() => ((IVertexListGraph<TestVertex, Edge<TestVertex>>?)default).OfflineLeastCommonAncestor(vertex1, pairs1)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph1.OfflineLeastCommonAncestor(default, pairs1)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph1.OfflineLeastCommonAncestor(vertex1, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => ((IVertexListGraph<TestVertex, Edge<TestVertex>>?)default).OfflineLeastCommonAncestor(default, pairs1)).Should().Throw<ArgumentNullException>();
            Invoking(() => ((IVertexListGraph<TestVertex, Edge<TestVertex>>?)default).OfflineLeastCommonAncestor(vertex1, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph1.OfflineLeastCommonAncestor(default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => ((IVertexListGraph<TestVertex, Edge<TestVertex>>?)default).OfflineLeastCommonAncestor(default, default)).Should().Throw<ArgumentNullException>();
            var pairs2 = new[] { new SEquatableEdge<int>(1, 2) };
            var graph2 = new AdjacencyGraph<int, Edge<int>>();
            Invoking(() => graph2.OfflineLeastCommonAncestor(1, pairs2)).Should().Throw<ArgumentException>();

            var graph3 = new AdjacencyGraph<int, Edge<int>>();
            graph3.AddVertex(1);
            Invoking(() => graph3.OfflineLeastCommonAncestor(1, pairs2)).Should().Throw<ArgumentException>();
#pragma warning restore CS8714
#pragma warning restore CS8620
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void MaximumFlow_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVertexRange(new[] { 1, 2 });
            Func<Edge<int>, double> capacities = _ => 1.0;
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);
            var reverseEdgesAlgorithm = new ReversedEdgeAugmentorAlgorithm<int, Edge<int>>(graph, edgeFactory);

            Invoking(() => graph.MaximumFlow(capacities, 1, 1, out _, edgeFactory, reverseEdgesAlgorithm)).Should().Throw<ArgumentException>();

            Invoking(() => graph.MaximumFlow(capacities, 1, 2, out _, edgeFactory, reverseEdgesAlgorithm)).Should().Throw<InvalidOperationException>();
        }

        private static IEnumerable<TestCaseData> CloneTestCases
        {
            [UsedImplicitly]
            get
            {
                yield return new TestCaseData(new AdjacencyGraph<int, EquatableEdge<int>>());
                yield return new TestCaseData(new BidirectionalGraph<int, EquatableEdge<int>>());
            }
        }

        [TestCaseSource(nameof(CloneTestCases))]
        public void Clone(IMutableVertexAndEdgeSet<int, EquatableEdge<int>> cloned)
        {
            var emptyGraph1 = new AdjacencyGraph<int, EquatableEdge<int>>();
            emptyGraph1.Clone(v => v, (_, v1, v2) => new EquatableEdge<int>(v1, v2), cloned);
            AssertEmptyGraph(cloned);

            cloned.Clear();
            var notEmptyGraph = new AdjacencyGraph<int, EquatableEdge<int>>();
            notEmptyGraph.AddVerticesAndEdgeRange(new[]
            {
                new EquatableEdge<int>(1, 2),
                new EquatableEdge<int>(2, 2),
                new EquatableEdge<int>(2, 3),
                new EquatableEdge<int>(3, 1)
            });
            notEmptyGraph.Clone(v => v, (_, v1, v2) => new EquatableEdge<int>(v1, v2), cloned);
            AssertHasVertices(cloned, new[] { 1, 2, 3 });
            AssertHasEdges(
                cloned,
                new[]
                {
                    new EquatableEdge<int>(1, 2),
                    new EquatableEdge<int>(2, 2),
                    new EquatableEdge<int>(2, 3),
                    new EquatableEdge<int>(3, 1)
                });

            // Clone is not empty
            cloned.Clear();
            cloned.AddVerticesAndEdge(new EquatableEdge<int>(1, 4));
            notEmptyGraph.Clone(v => v, (_, v1, v2) => new EquatableEdge<int>(v1, v2), cloned);
            // Clone has been cleaned and then re-filled
            AssertHasVertices(cloned, new[] { 1, 2, 3 });
            AssertHasEdges(
                cloned,
                new[]
                {
                    new EquatableEdge<int>(1, 2),
                    new EquatableEdge<int>(2, 2),
                    new EquatableEdge<int>(2, 3),
                    new EquatableEdge<int>(3, 1)
                });
        }

        [Test]
        public void Clone_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var clone = new AdjacencyGraph<int, Edge<int>>();

            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => AlgorithmExtensions.Clone(default, v => v, (e, _, _) => e, clone)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.Clone(default, (e, _, _) => e, clone)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.Clone(v => v, default, clone)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.Clone(v => v, (e, _, _) => e, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => AlgorithmExtensions.Clone(default, default, (e, _, _) => e, clone)).Should().Throw<ArgumentNullException>();
            Invoking(() => AlgorithmExtensions.Clone(default, v => v, default, clone)).Should().Throw<ArgumentNullException>();
            Invoking(() => AlgorithmExtensions.Clone<int, Edge<int>>(default, v => v, (e, _, _) => e, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.Clone(default, default, clone)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.Clone(default, (e, _, _) => e, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.Clone(v => v, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => AlgorithmExtensions.Clone(default, default, default, clone)).Should().Throw<ArgumentNullException>();
            Invoking(() => AlgorithmExtensions.Clone<int, Edge<int>>(default, default, (e, _, _) => e, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.Clone(default, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => AlgorithmExtensions.Clone<int, Edge<int>>(default, default, default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
        }
    }
}
