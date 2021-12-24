#nullable enable

using System.Collections.Immutable;
using NUnit.Framework;
using FastGraph.Algorithms;
using FastGraph.Algorithms.RankedShortestPath;
using static FastGraph.Tests.Algorithms.AlgorithmTestHelpers;
using static FastGraph.Tests.AssertHelpers;

namespace FastGraph.Tests.Algorithms.RankedShortestPath
{
    /// <summary>
    /// Tests for <see cref="HoffmanPavleyRankedShortestPathAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class HoffmanPavleyRankedShortestPathAlgorithmTests : RootedAlgorithmTestsBase
    {
        #region Test helpers

        private static void RunHoffmanPavleyRankedShortestPathAndCheck<TVertex, TEdge>(
            IBidirectionalGraph<TVertex, TEdge> graph,
            Dictionary<TEdge, double> edgeWeights,
            TVertex rootVertex,
            TVertex targetVertex,
            int pathCount)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            graph.Edges.Should().OnlyContain(e => edgeWeights.ContainsKey(e));

            var target = new HoffmanPavleyRankedShortestPathAlgorithm<TVertex, TEdge>(graph, e => edgeWeights[e])
            {
                ShortestPathCount = pathCount
            };
            target.Compute(rootVertex, targetVertex);

            double lastWeight = double.MinValue;
            foreach (TEdge[] path in target.ComputedShortestPaths.Select(p => p.ToArray()))
            {
                double weight = path.Sum(e => edgeWeights[e]);
                (lastWeight <= weight).Should().BeTrue();
                path.First().Source.Should().Be(rootVertex);
                path.Last().Target.Should().Be(targetVertex);
                path.IsPathWithoutCycles<TVertex, TEdge>().Should().BeTrue();

                lastWeight = weight;
            }
        }

        #endregion

        [Test]
        public void Constructor()
        {
            Func<Edge<int>, double> Weights = _ => 1.0;

            var graph = new BidirectionalGraph<int, Edge<int>>();
            var algorithm = new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(graph, Weights);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(graph, Weights, DistanceRelaxers.CriticalDistance);
            AssertAlgorithmProperties(algorithm, graph, DistanceRelaxers.CriticalDistance);

            algorithm = new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(default, graph, Weights, DistanceRelaxers.CriticalDistance);
            AssertAlgorithmProperties(algorithm, graph, DistanceRelaxers.CriticalDistance);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                HoffmanPavleyRankedShortestPathAlgorithm<TVertex, TEdge> algo,
                IBidirectionalGraph<TVertex, TEdge> g,
                IDistanceRelaxer? relaxer = default)
                where TVertex : notnull
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                algo.ShortestPathCount.Should().Be(3);
                algo.ComputedShortestPaths.Should().BeEmpty();
                algo.ComputedShortestPathCount.Should().Be(0);
                if (relaxer is null)
                    algo.DistanceRelaxer.Should().NotBeNull();
                else
                    algo.DistanceRelaxer.Should().BeSameAs(relaxer);
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            var graph = new BidirectionalGraph<int, Edge<int>>();

            Func<Edge<int>, double> Weights = _ => 1.0;

#pragma warning disable CS8625
            Invoking(() => new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(default, Weights)).Should().Throw<ArgumentNullException>();
            Invoking(() => new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(graph, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(default, default)).Should().Throw<ArgumentNullException>();

            Invoking(() => new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(default, Weights, DistanceRelaxers.CriticalDistance)).Should().Throw<ArgumentNullException>();
            Invoking(() => new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(graph, default, DistanceRelaxers.CriticalDistance)).Should().Throw<ArgumentNullException>();
            Invoking(() => new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(graph, Weights, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(default, default, DistanceRelaxers.CriticalDistance)).Should().Throw<ArgumentNullException>();
            Invoking(() => new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(default, Weights, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(graph, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(default, default, default)).Should().Throw<ArgumentNullException>();

            Invoking(() => new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(default, default, Weights, DistanceRelaxers.CriticalDistance)).Should().Throw<ArgumentNullException>();
            Invoking(() => new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(default, graph, default, DistanceRelaxers.CriticalDistance)).Should().Throw<ArgumentNullException>();
            Invoking(() => new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(default, graph, Weights, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(default, default, default, DistanceRelaxers.CriticalDistance)).Should().Throw<ArgumentNullException>();
            Invoking(() => new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(default, default, Weights, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(default, graph, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(default, default, default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625

            var algorithm = new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(graph, Weights);
            Invoking(() => algorithm.ShortestPathCount = 0).Should().Throw<ArgumentOutOfRangeException>();
            Invoking(() => algorithm.ShortestPathCount = -1).Should().Throw<ArgumentOutOfRangeException>();
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        #region Rooted algorithm

        [Test]
        public void TryGetRootVertex()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            var algorithm = new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0);
            TryGetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            var algorithm = new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0);
            SetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            var algorithm = new HoffmanPavleyRankedShortestPathAlgorithm<TestVertex, Edge<TestVertex>>(graph, _ => 1.0);
            SetRootVertex_Throws_Test(algorithm);
        }

        [Test]
        public void ClearRootVertex()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            var algorithm = new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0);
            ClearRootVertex_Test(algorithm);
        }

        [Test]
        public void ComputeWithoutRoot_Throws()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            ComputeWithoutRoot_Throws_Test(
                () => new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0));

            var algorithm = new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0);
            Invoking(algorithm.Compute).Should().Throw<InvalidOperationException>();

            // Source (and target) vertex set but not to a vertex in the graph
            const int vertex1 = 1;
            algorithm = new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0);
            algorithm.SetRootVertex(vertex1);
            algorithm.SetTargetVertex(vertex1);
            Invoking(algorithm.Compute).Should().Throw<VertexNotFoundException>();

            const int vertex2 = 2;
            graph.AddVertex(vertex1);
            algorithm = new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0);
            algorithm.SetRootVertex(vertex1);
            algorithm.SetTargetVertex(vertex2);
            Invoking(algorithm.Compute).Should().Throw<VertexNotFoundException>();
        }

        #endregion

        #region Shortest path algorithm

        [Test]
        public void TryGetTargetVertex()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            var algorithm = new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0);

            algorithm.TryGetTargetVertex(out _).Should().BeFalse();

            const int vertex = 0;
            algorithm.SetTargetVertex(vertex);
            algorithm.TryGetTargetVertex(out int target).Should().BeTrue();
            AssertEqual(vertex, target);
        }

        [Test]
        public void SetTargetVertex()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            var algorithm = new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0);

            const int vertex1 = 0;
            algorithm.SetTargetVertex(vertex1);
            algorithm.TryGetTargetVertex(out int target);
            target.Should().Be(vertex1);

            // Not changed
            algorithm.SetTargetVertex(vertex1);
            algorithm.TryGetTargetVertex(out target);
            target.Should().Be(vertex1);

            const int vertex2 = 1;
            algorithm.SetTargetVertex(vertex2);
            algorithm.TryGetTargetVertex(out target);
            target.Should().Be(vertex2);
        }

        [Test]
        public void SetTargetVertex_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            var algorithm = new HoffmanPavleyRankedShortestPathAlgorithm<TestVertex, Edge<TestVertex>>(graph, _ => 1.0);

            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => algorithm.SetTargetVertex(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        [Test]
        public void ComputeWithRootAndTarget()
        {
            const int start = 0;
            const int end = 1;

            var graph = new BidirectionalGraph<int, Edge<int>>();
            graph.AddVertexRange(new[] { start, end });
            var algorithm = new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0);

            Invoking(() => algorithm.Compute(start, end)).Should().NotThrow();
            algorithm.TryGetRootVertex(out int root).Should().BeTrue();
            algorithm.TryGetTargetVertex(out int target).Should().BeTrue();
            AssertEqual(start, root);
            AssertEqual(end, target);
        }

        [Test]
        public void ComputeWithRootAndTarget_Throws()
        {
            const int start1 = 1;
            const int end1 = 2;

            var graph1 = new BidirectionalGraph<int, Edge<int>>();
            var algorithm1 = new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(graph1, _ => 1.0);

            Invoking(() => algorithm1.Compute(start1)).Should().Throw<ArgumentException>();
            graph1.AddVertex(start1);

            Invoking(() => algorithm1.Compute(start1)).Should().Throw<InvalidOperationException>();
            Invoking(() => algorithm1.Compute(start1, end1)).Should().Throw<ArgumentException>();


            var start2 = new TestVertex("1");
            var end2 = new TestVertex("2");

            var graph2 = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            var algorithm2 = new HoffmanPavleyRankedShortestPathAlgorithm<TestVertex, Edge<TestVertex>>(graph2, _ => 1.0);

            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => algorithm2.Compute(default)).Should().Throw<ArgumentNullException>();
            Invoking(() => algorithm2.Compute(start2, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => algorithm2.Compute(default, end2)).Should().Throw<ArgumentNullException>();
            Invoking(() => algorithm2.Compute(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
        }

        #endregion

        [TestCaseSource(nameof(GetBidirectionalGraphs_SlowTests))]
        [Category(TestCategories.LongRunning)]
        public void HoffmanPavleyRankedShortestPath(TestGraphInstance<BidirectionalGraph<string, Edge<string>>, string> testGraph)
        {
            var graph = testGraph.Instance;

            var weights = new Dictionary<Edge<string>, double>();
            foreach (Edge<string> edge in graph.Edges)
                weights.Add(edge, graph.OutDegree(edge.Source) + 1);

            RunHoffmanPavleyRankedShortestPathAndCheck(
                graph,
                weights,
                graph.Vertices.First(),
                graph.Vertices.Last(),
                graph.VertexCount);
        }

        [Test]
        public void HoffmanPavleyRankedShortestPathNetwork()
        {
            // Create network graph
            var graph = new BidirectionalGraph<int, Edge<int>>();
            var weights = new Dictionary<Edge<int>, double>();
            int[] data =
            {
                1, 4, 3,
                4, 1, 3,

                1, 2, 1,
                2, 1, 1,

                2, 3, 3,
                3, 2, 3,

                4, 5, 1,
                5, 4, 1,

                1, 5, 2,
                5, 1, 2,

                2, 5, 2,
                5, 2, 3,

                2, 6, 5,
                6, 2, 5,

                2, 8, 2,
                8, 2, 2,

                6, 9, 2,
                9, 6, 2,

                6, 8, 4,
                8, 6, 4,

                5, 8, 2,
                8, 5, 2,

                5, 7, 2,
                7, 5, 2,

                4, 7, 3,
                7, 4, 3,

                7, 8, 4,
                8, 7, 4,

                9, 8, 5
            };

            int i = 0;
            for (; i + 2 < data.Length; i += 3)
            {
                var edge = new Edge<int>(data[i + 0], data[i + 1]);
                graph.AddVerticesAndEdge(edge);
                weights[edge] = data[i + 2];
            }
            i.Should().Be(data.Length);

            RunHoffmanPavleyRankedShortestPathAndCheck(graph, weights, 9, 1, 10);
        }

        [Test]
        public void NotEnoughPaths()
        {
            BidirectionalGraph<int, TaggedEdge<int, int>> graph = CreateGraph();

            var algorithm = new HoffmanPavleyRankedShortestPathAlgorithm<int, TaggedEdge<int, int>>(graph, _ => 1.0)
            {
                ShortestPathCount = 5
            };
            algorithm.Compute(1626, 1965);
            algorithm.ComputedShortestPathCount.Should().Be(4);

            #region Local function

            BidirectionalGraph<int, TaggedEdge<int, int>> CreateGraph()
            {
                int ii = 0;
                var g = new BidirectionalGraph<int, TaggedEdge<int, int>>();
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(493, 495, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(495, 493, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(497, 499, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(499, 497, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(499, 501, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(501, 499, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(501, 503, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(503, 501, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(503, 505, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(505, 503, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(505, 507, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(507, 505, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(507, 509, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(509, 507, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(509, 511, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(511, 509, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(2747, 2749, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(2749, 2747, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(2749, 2751, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(2751, 2749, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(2751, 2753, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(2753, 2751, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(2753, 2755, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(2755, 2753, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(2755, 2757, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(2757, 2755, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(2757, 2759, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(2759, 2757, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(2761, 2763, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(2763, 2761, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(2765, 2767, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(2767, 2765, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(2763, 2765, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(2765, 2763, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(654, 978, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(978, 654, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(978, 1302, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(1302, 978, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(1302, 1626, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(1626, 1302, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(1626, 1950, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(1950, 1626, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(1950, 2274, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(2274, 1950, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(2274, 2598, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(2598, 2274, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(513, 676, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(676, 513, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(2767, 2608, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(2608, 2767, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(2287, 2608, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(2608, 2287, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(676, 999, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(999, 676, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(1321, 1643, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(1643, 1321, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(1643, 1965, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(1965, 1643, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(1965, 2287, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(2287, 1965, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(999, 1321, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(1321, 999, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(2745, 2747, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(2747, 2745, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(650, 491, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(491, 650, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(650, 970, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(970, 650, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(2258, 2582, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(2582, 2258, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(970, 1291, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(1291, 970, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(1935, 2258, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(2258, 1935, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(1291, 1613, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(1613, 1291, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(1613, 1935, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(1935, 1613, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(2582, 2745, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(2745, 2582, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(495, 497, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(497, 495, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(511, 513, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(513, 511, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(491, 493, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(493, 491, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(491, 654, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(654, 491, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(2761, 2598, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(2598, 2761, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(2761, 2759, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(2759, 2761, ii));

                return g;
            }

            #endregion
        }

        [Test]
        public void InfiniteLoop()
        {
            BidirectionalGraph<int, TaggedEdge<int, int>> graph = CreateGraph();

            var algorithm = new HoffmanPavleyRankedShortestPathAlgorithm<int, TaggedEdge<int, int>>(graph, _ => 1.0)
            {
                ShortestPathCount = 5
            };
            Invoking(() => algorithm.Compute(5, 2)).Should().NotThrow();

            #region Local function

            BidirectionalGraph<int, TaggedEdge<int, int>> CreateGraph()
            {
                int ii = 0;
                var g = new BidirectionalGraph<int, TaggedEdge<int, int>>();

                g.AddVerticesAndEdge(new TaggedEdge<int, int>(0, 1, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(1, 2, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(2, 3, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(3, 4, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(4, 5, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(5, 0, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(1, 5, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(5, 1, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(2, 5, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(1, 0, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(2, 1, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(3, 2, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(4, 3, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(5, 4, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(0, 5, ii++));
                g.AddVerticesAndEdge(new TaggedEdge<int, int>(5, 2, ii));

                return g;
            }

            #endregion
        }

        private static IEnumerable<TestCaseData> GetBidirectionalGraphs_SlowTests() =>
            TestGraphFactory
                .SampleBidirectionalGraphs()
                .Where(t => t.VertexCount != 0)
                .Select(t => new TestCaseData(t) { TestName = t.DescribeForTestCase() })
                .Memoize();
    }
}
