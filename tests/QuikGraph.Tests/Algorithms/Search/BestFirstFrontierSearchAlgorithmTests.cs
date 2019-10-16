using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.Observers;
using QuikGraph.Algorithms.Search;
using QuikGraph.Algorithms.ShortestPath;

namespace QuikGraph.Tests.Algorithms.Search
{
    /// <summary>
    /// Tests for <see cref="BestFirstFrontierSearchAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class BestFirstFrontierSearchAlgorithmTests
    {
        #region Helpers

        private static void RunSearch<TVertex, TEdge>(
            [NotNull] IBidirectionalGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            if (graph.VertexCount == 0)
                return;

            IDistanceRelaxer distanceRelaxer = DistanceRelaxers.ShortestDistance;

            var search = new BestFirstFrontierSearchAlgorithm<TVertex, TEdge>(
                graph,
                e => 1,
                distanceRelaxer);

            TVertex root = graph.Vertices.First();
            TVertex target = graph.Vertices.Last();

            var recorder = new VertexPredecessorRecorderObserver<TVertex, TEdge>();
            using (recorder.Attach(search))
                search.Compute(root, target);

            if (recorder.VerticesPredecessors.ContainsKey(target))
            {
                Assert.IsTrue(recorder.TryGetPath(target, out _));
            }
        }

        public void CompareSearch<TVertex, TEdge>(
            [NotNull] IBidirectionalGraph<TVertex, TEdge> graph,
            [NotNull] TVertex root,
            [NotNull] TVertex target)
            where TEdge : IEdge<TVertex>
        {
            double EdgeWeights(TEdge e) => 1;

            IDistanceRelaxer distanceRelaxer = DistanceRelaxers.ShortestDistance;

            var search = new BestFirstFrontierSearchAlgorithm<TVertex, TEdge>(
                graph,
                EdgeWeights,
                distanceRelaxer);
            var recorder = new VertexDistanceRecorderObserver<TVertex, TEdge>(EdgeWeights);
            using (recorder.Attach(search))
                search.Compute(root, target);

            var dijkstra = new DijkstraShortestPathAlgorithm<TVertex, TEdge>(graph, EdgeWeights, distanceRelaxer);
            var dijkstraRecorder = new VertexDistanceRecorderObserver<TVertex, TEdge>(EdgeWeights);
            using (dijkstraRecorder.Attach(dijkstra))
                dijkstra.Compute(root);

            IDictionary<TVertex, double> fvp = recorder.Distances;
            IDictionary<TVertex, double> dvp = dijkstraRecorder.Distances;
            if (dvp.TryGetValue(target, out double cost))
            {
                Assert.IsTrue(fvp.ContainsKey(target), $"Target {target} not found, should be {cost}");
                Assert.AreEqual(dvp[target], fvp[target]);
            }
        }

        #endregion

        [Test]
        public void KrokFFig2Example()
        {
            var graph = new BidirectionalGraph<char, SEquatableEdge<char>>();
            graph.AddVerticesAndEdge(new SEquatableEdge<char>('A', 'C'));
            graph.AddVerticesAndEdge(new SEquatableEdge<char>('A', 'B'));
            graph.AddVerticesAndEdge(new SEquatableEdge<char>('B', 'E'));
            graph.AddVerticesAndEdge(new SEquatableEdge<char>('B', 'D'));
            graph.AddVerticesAndEdge(new SEquatableEdge<char>('E', 'F'));
            graph.AddVerticesAndEdge(new SEquatableEdge<char>('E', 'G'));

            RunSearch(graph);
        }

        [Test]
        public void BestFirstFrontierSearchAllGraphs()
        {
            foreach (BidirectionalGraph<string, Edge<string>> graph in TestGraphFactory.GetBidirectionalGraphs_TMP())
                RunSearch(graph);
        }

        [Test]
        public void CompareBestFirstFrontierSearchAllGraphs()
        {
            foreach (BidirectionalGraph<string, Edge<string>> graph in TestGraphFactory.GetBidirectionalGraphs_TMP())
            {
                if (graph.VertexCount == 0)
                    continue;

                string root = graph.Vertices.First();
                foreach (string vertex in graph.Vertices)
                {
                    if (!root.Equals(vertex))
                        CompareSearch(graph, root, vertex);
                }
            }
        }
    }
}
