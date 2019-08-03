using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms.RankedShortestPath;

namespace QuikGraph.Tests.Algorithms.RankedShortestPath
{
    /// <summary>
    /// Tests for <see cref="HoffmanPavleyRankedShortestPathAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class HoffmanPavleyRankedShortestPathAlgorithmTests
    {
        #region Helpers

        private static void RunHoffmanPavleyRankedShortestPathAndCheck<TVertex, TEdge>(
            [NotNull] IBidirectionalGraph<TVertex, TEdge> graph,
            [NotNull] Dictionary<TEdge, double> edgeWeights,
            [NotNull] TVertex rootVertex,
            [NotNull] TVertex goalVertex,
            int pathCount)
            where TEdge : IEdge<TVertex>
        {
            QuikGraphAssert.TrueForAll(graph.Edges, edgeWeights.ContainsKey);

            var target = new HoffmanPavleyRankedShortestPathAlgorithm<TVertex, TEdge>(graph, e => edgeWeights[e])
            {
                ShortestPathCount = pathCount
            };
            target.Compute(rootVertex, goalVertex);

            double lastWeight = double.MinValue;
            foreach (TEdge[] path in target.ComputedShortestPaths.Select(p => p.ToArray()))
            {
                double weight = path.Sum(e => edgeWeights[e]);
                Assert.IsTrue(lastWeight <= weight, $"{lastWeight} <= {weight}");
                Assert.AreEqual(rootVertex, path.First().Source);
                Assert.AreEqual(goalVertex, path.Last().Target);
                Assert.IsTrue(path.IsPathWithoutCycles<TVertex, TEdge>());

                lastWeight = weight;
            }
        }

        #endregion

        [Test]
        public void HoffmanPavleyRankedShortestPathAll()
        {
            foreach (BidirectionalGraph<string, Edge<string>> graph in TestGraphFactory.GetBidirectionalGraphs())
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
    }
}
