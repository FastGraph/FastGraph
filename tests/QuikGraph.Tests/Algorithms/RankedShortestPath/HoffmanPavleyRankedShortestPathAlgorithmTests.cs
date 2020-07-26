using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.RankedShortestPath;
using QuikGraph.Tests.Algorithms.ShortestPath;
using static QuikGraph.Tests.Algorithms.AlgorithmTestHelpers;
using static QuikGraph.Tests.AssertHelpers;

namespace QuikGraph.Tests.Algorithms.RankedShortestPath
{
    /// <summary>
    /// Tests for <see cref="HoffmanPavleyRankedShortestPathAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class HoffmanPavleyRankedShortestPathAlgorithmTests : ShortestPathAlgorithmTestsBase
    {
        #region Test helpers

        private static void RunHoffmanPavleyRankedShortestPathAndCheck<TVertex, TEdge>(
            [NotNull] IBidirectionalGraph<TVertex, TEdge> graph,
            [NotNull] Dictionary<TEdge, double> edgeWeights,
            [NotNull] TVertex rootVertex,
            [NotNull] TVertex targetVertex,
            int pathCount)
            where TEdge : IEdge<TVertex>
        {
            QuikGraphAssert.TrueForAll(graph.Edges, edgeWeights.ContainsKey);

            var target = new HoffmanPavleyRankedShortestPathAlgorithm<TVertex, TEdge>(graph, e => edgeWeights[e])
            {
                ShortestPathCount = pathCount
            };
            target.Compute(rootVertex, targetVertex);

            double lastWeight = double.MinValue;
            foreach (TEdge[] path in target.ComputedShortestPaths.Select(p => p.ToArray()))
            {
                double weight = path.Sum(e => edgeWeights[e]);
                Assert.IsTrue(lastWeight <= weight, $"{lastWeight} <= {weight}");
                Assert.AreEqual(rootVertex, path.First().Source);
                Assert.AreEqual(targetVertex, path.Last().Target);
                Assert.IsTrue(path.IsPathWithoutCycles<TVertex, TEdge>());

                lastWeight = weight;
            }
        }

        #endregion

        [Test]
        public void Constructor()
        {
            Func<Edge<int>, double> Weights = e => 1.0;

            var graph = new BidirectionalGraph<int, Edge<int>>();
            var algorithm = new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(graph, Weights);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(graph, Weights, DistanceRelaxers.CriticalDistance);
            AssertAlgorithmProperties(algorithm, graph, DistanceRelaxers.CriticalDistance);

            algorithm = new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(null, graph, Weights, DistanceRelaxers.CriticalDistance);
            AssertAlgorithmProperties(algorithm, graph, DistanceRelaxers.CriticalDistance);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                HoffmanPavleyRankedShortestPathAlgorithm<TVertex, TEdge> algo,
                IBidirectionalGraph<TVertex, TEdge> g,
                IDistanceRelaxer relaxer = null)
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                Assert.AreEqual(3, algo.ShortestPathCount);
                CollectionAssert.IsEmpty(algo.ComputedShortestPaths);
                Assert.AreEqual(0, algo.ComputedShortestPathCount);
                if (relaxer is null)
                    Assert.IsNotNull(algo.DistanceRelaxer);
                else
                    Assert.AreSame(relaxer, algo.DistanceRelaxer);
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            var graph = new BidirectionalGraph<int, Edge<int>>();

            Func<Edge<int>, double> Weights = e => 1.0;

            Assert.Throws<ArgumentNullException>(
                () => new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(null, Weights));
            Assert.Throws<ArgumentNullException>(
                () => new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(graph, null));
            Assert.Throws<ArgumentNullException>(
                () => new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(null, null));

            Assert.Throws<ArgumentNullException>(
                () => new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(null, Weights, DistanceRelaxers.CriticalDistance));
            Assert.Throws<ArgumentNullException>(
                () => new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(graph, null, DistanceRelaxers.CriticalDistance));
            Assert.Throws<ArgumentNullException>(
                () => new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(graph, Weights, null));
            Assert.Throws<ArgumentNullException>(
                () => new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(null, null, DistanceRelaxers.CriticalDistance));
            Assert.Throws<ArgumentNullException>(
                () => new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(null, Weights, null));
            Assert.Throws<ArgumentNullException>(
                () => new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(graph, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(null, null, null));

            Assert.Throws<ArgumentNullException>(
                () => new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(null, null, Weights, DistanceRelaxers.CriticalDistance));
            Assert.Throws<ArgumentNullException>(
                () => new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(null, graph, null, DistanceRelaxers.CriticalDistance));
            Assert.Throws<ArgumentNullException>(
                () => new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(null, graph, Weights, null));
            Assert.Throws<ArgumentNullException>(
                () => new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(null, null, null, DistanceRelaxers.CriticalDistance));
            Assert.Throws<ArgumentNullException>(
                () => new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(null, null, Weights, null));
            Assert.Throws<ArgumentNullException>(
                () => new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(null, graph, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(null, null, null, null));

            var algorithm = new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(graph, Weights);
            Assert.Throws<ArgumentOutOfRangeException>(() => algorithm.ShortestPathCount = 0);
            Assert.Throws<ArgumentOutOfRangeException>(() => algorithm.ShortestPathCount = -1);
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        #region Rooted algorithm

        [Test]
        public void TryGetRootVertex()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            var algorithm = new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(graph, edge => 1.0);
            TryGetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            var algorithm = new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(graph, edge => 1.0);
            SetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            var algorithm = new HoffmanPavleyRankedShortestPathAlgorithm<TestVertex, Edge<TestVertex>>(graph, edge => 1.0);
            SetRootVertex_Throws_Test(algorithm);
        }

        [Test]
        public void ClearRootVertex()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            var algorithm = new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(graph, edge => 1.0);
            ClearRootVertex_Test(algorithm);
        }

        [Test]
        public void ComputeWithoutRoot_Throws()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            ComputeWithoutRoot_Throws_Test(
                () => new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(graph, edge => 1.0));

            var algorithm = new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(graph, edge => 1.0);
            Assert.Throws<InvalidOperationException>(algorithm.Compute);

            // Source (and target) vertex set but not to a vertex in the graph
            const int vertex1 = 1;
            algorithm = new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(graph, edge => 1.0);
            algorithm.SetRootVertex(vertex1);
            algorithm.SetTargetVertex(vertex1);
            Assert.Throws<VertexNotFoundException>(algorithm.Compute);

            const int vertex2 = 2;
            graph.AddVertex(vertex1);
            algorithm = new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(graph, edge => 1.0);
            algorithm.SetRootVertex(vertex1);
            algorithm.SetTargetVertex(vertex2);
            Assert.Throws<VertexNotFoundException>(algorithm.Compute);
        }

        #endregion

        #region Shortest path algorithm

        [Test]
        public void TryGetTargetVertex()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            var algorithm = new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(graph, e => 1.0);

            Assert.IsFalse(algorithm.TryGetTargetVertex(out _));

            const int vertex = 0;
            algorithm.SetTargetVertex(vertex);
            Assert.IsTrue(algorithm.TryGetTargetVertex(out int target));
            AssertEqual(vertex, target);
        }

        [Test]
        public void SetTargetVertex()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            var algorithm = new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(graph, e => 1.0);

            const int vertex1 = 0;
            algorithm.SetTargetVertex(vertex1);
            algorithm.TryGetTargetVertex(out int target);
            Assert.AreEqual(vertex1, target);

            // Not changed
            algorithm.SetTargetVertex(vertex1);
            algorithm.TryGetTargetVertex(out target);
            Assert.AreEqual(vertex1, target);

            const int vertex2 = 1;
            algorithm.SetTargetVertex(vertex2);
            algorithm.TryGetTargetVertex(out target);
            Assert.AreEqual(vertex2, target);
        }

        [Test]
        public void SetTargetVertex_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            var algorithm = new HoffmanPavleyRankedShortestPathAlgorithm<TestVertex, Edge<TestVertex>>(graph, e => 1.0);

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => algorithm.SetTargetVertex(null));
        }

        [Test]
        public void ComputeWithRootAndTarget()
        {
            const int start = 0;
            const int end = 1;

            var graph = new BidirectionalGraph<int, Edge<int>>();
            graph.AddVertexRange(new[] { start, end });
            var algorithm = new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(graph, edge => 1.0);

            Assert.DoesNotThrow(() => algorithm.Compute(start, end));
            Assert.IsTrue(algorithm.TryGetRootVertex(out int root));
            Assert.IsTrue(algorithm.TryGetTargetVertex(out int target));
            AssertEqual(start, root);
            AssertEqual(end, target);
        }

        [Test]
        public void ComputeWithRootAndTarget_Throws()
        {
            const int start1 = 1;
            const int end1 = 2;

            var graph1 = new BidirectionalGraph<int, Edge<int>>();
            var algorithm1 = new HoffmanPavleyRankedShortestPathAlgorithm<int, Edge<int>>(graph1, edge => 1.0);

            Assert.Throws<ArgumentException>(() => algorithm1.Compute(start1));
            graph1.AddVertex(start1);

            Assert.Throws<InvalidOperationException>(() => algorithm1.Compute(start1));
            Assert.Throws<ArgumentException>(() => algorithm1.Compute(start1, end1));


            var start2 = new TestVertex("1");
            var end2 = new TestVertex("2");

            var graph2 = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            var algorithm2 = new HoffmanPavleyRankedShortestPathAlgorithm<TestVertex, Edge<TestVertex>>(graph2, edge => 1.0);

            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => algorithm2.Compute(null));
            Assert.Throws<ArgumentNullException>(() => algorithm2.Compute(start2, null));
            Assert.Throws<ArgumentNullException>(() => algorithm2.Compute(null, end2));
            Assert.Throws<ArgumentNullException>(() => algorithm2.Compute(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
        }

        #endregion

        [Test]
        [Category(TestCategories.LongRunning)]
        public void HoffmanPavleyRankedShortestPath()
        {
            foreach (BidirectionalGraph<string, Edge<string>> graph in TestGraphFactory.GetBidirectionalGraphs_SlowTests())
            {
                if (graph.VertexCount == 0)
                    continue;

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
            Assert.AreEqual(data.Length, i);

            RunHoffmanPavleyRankedShortestPathAndCheck(graph, weights, 9, 1, 10);
        }

        [Test]
        public void NotEnoughPaths()
        {
            BidirectionalGraph<int, TaggedEdge<int, int>> graph = CreateGraph();

            var algorithm = new HoffmanPavleyRankedShortestPathAlgorithm<int, TaggedEdge<int, int>>(graph, e => 1.0)
            {
                ShortestPathCount = 5
            };
            algorithm.Compute(1626, 1965);
            Assert.AreEqual(4, algorithm.ComputedShortestPathCount);

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

            var algorithm = new HoffmanPavleyRankedShortestPathAlgorithm<int, TaggedEdge<int, int>>(graph, e => 1.0)
            {
                ShortestPathCount = 5
            };
            Assert.DoesNotThrow(() => algorithm.Compute(5, 2));

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
    }
}