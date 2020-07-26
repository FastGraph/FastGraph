using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.Observers;
using QuikGraph.Algorithms.Search;
using QuikGraph.Algorithms.ShortestPath;
using static QuikGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace QuikGraph.Tests.Algorithms.Search
{
    /// <summary>
    /// Tests for <see cref="BestFirstFrontierSearchAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class BestFirstFrontierSearchAlgorithmTests : SearchAlgorithmTestsBase
    {
        #region Test helpers

        private static void RunAndCheckSearch<TVertex, TEdge>(
            [NotNull] IBidirectionalGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            if (graph.VertexCount == 0)
                return;

            IDistanceRelaxer distanceRelaxer = DistanceRelaxers.ShortestDistance;

            var search = new BestFirstFrontierSearchAlgorithm<TVertex, TEdge>(
                graph,
                edge => 1.0,
                distanceRelaxer);
            bool targetReached = false;
            search.TargetReached += (sender, args) => targetReached = true;

            TVertex root = graph.Vertices.First();
            TVertex target = graph.Vertices.Last();

            var recorder = new VertexPredecessorRecorderObserver<TVertex, TEdge>();
            using (recorder.Attach(search))
                search.Compute(root, target);

            if (recorder.VerticesPredecessors.ContainsKey(target))
            {
                Assert.IsTrue(recorder.TryGetPath(target, out IEnumerable<TEdge> path));

                if (Equals(root, path.First().Source))
                    Assert.IsTrue(targetReached);
                else
                    Assert.IsFalse(targetReached);
            }
        }

        private static void CompareSearches<TVertex, TEdge>(
            [NotNull] IBidirectionalGraph<TVertex, TEdge> graph,
            [NotNull] TVertex root,
            [NotNull] TVertex target)
            where TEdge : IEdge<TVertex>
        {
            double EdgeWeights(TEdge edge) => 1.0;

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

            IDictionary<TVertex, double> bffsVerticesDistances = recorder.Distances;
            IDictionary<TVertex, double> dijkstraVerticesDistances = dijkstraRecorder.Distances;
            if (dijkstraVerticesDistances.TryGetValue(target, out double cost))
            {
                Assert.IsTrue(bffsVerticesDistances.ContainsKey(target), $"Target {target} not found, should be {cost}.");
                Assert.AreEqual(dijkstraVerticesDistances[target], bffsVerticesDistances[target]);
            }
        }

        #endregion

        [Test]
        public void Constructor()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            var algorithm = new BestFirstFrontierSearchAlgorithm<int, Edge<int>>(graph, edge => 1.0, DistanceRelaxers.ShortestDistance);
            AssertAlgorithmState(algorithm, graph);

            algorithm = new BestFirstFrontierSearchAlgorithm<int, Edge<int>>(null, graph, edge => 1.0, DistanceRelaxers.ShortestDistance);
            AssertAlgorithmState(algorithm, graph);
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            var graph = new BidirectionalGraph<int, Edge<int>>();

            Assert.Throws<ArgumentNullException>(
                () => new BestFirstFrontierSearchAlgorithm<int, Edge<int>>(
                    null, edge => 1.0, DistanceRelaxers.ShortestDistance));
            Assert.Throws<ArgumentNullException>(
                () => new BestFirstFrontierSearchAlgorithm<int, Edge<int>>(
                    graph, null, DistanceRelaxers.ShortestDistance));
            Assert.Throws<ArgumentNullException>(
                () => new BestFirstFrontierSearchAlgorithm<int, Edge<int>>(
                    graph, edge => 1.0, null));
            Assert.Throws<ArgumentNullException>(
                () => new BestFirstFrontierSearchAlgorithm<int, Edge<int>>(
                    null, null, DistanceRelaxers.ShortestDistance));
            Assert.Throws<ArgumentNullException>(
                () => new BestFirstFrontierSearchAlgorithm<int, Edge<int>>(
                    graph, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new BestFirstFrontierSearchAlgorithm<int, Edge<int>>(
                    null, null, null));

            Assert.Throws<ArgumentNullException>(
                () => new BestFirstFrontierSearchAlgorithm<int, Edge<int>>(
                    null, null, edge => 1.0, DistanceRelaxers.ShortestDistance));
            Assert.Throws<ArgumentNullException>(
                () => new BestFirstFrontierSearchAlgorithm<int, Edge<int>>(
                    null, graph, null, DistanceRelaxers.ShortestDistance));
            Assert.Throws<ArgumentNullException>(
                () => new BestFirstFrontierSearchAlgorithm<int, Edge<int>>(
                    null, graph, edge => 1.0, null));
            Assert.Throws<ArgumentNullException>(
                () => new BestFirstFrontierSearchAlgorithm<int, Edge<int>>(
                    null, null, null, DistanceRelaxers.ShortestDistance));
            Assert.Throws<ArgumentNullException>(
                () => new BestFirstFrontierSearchAlgorithm<int, Edge<int>>(
                    null, graph, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new BestFirstFrontierSearchAlgorithm<int, Edge<int>>(
                    null, null, null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        #region Rooted algorithm

        [Test]
        public void TryGetRootVertex()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            var algorithm = new BestFirstFrontierSearchAlgorithm<int, Edge<int>>(graph, edge => 1.0, DistanceRelaxers.EdgeShortestDistance);
            TryGetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            var algorithm = new BestFirstFrontierSearchAlgorithm<int, Edge<int>>(graph, edge => 1.0, DistanceRelaxers.EdgeShortestDistance);
            SetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            var algorithm = new BestFirstFrontierSearchAlgorithm<TestVertex, Edge<TestVertex>>(graph, edge => 1.0, DistanceRelaxers.EdgeShortestDistance);
            SetRootVertex_Throws_Test(algorithm);
        }

        [Test]
        public void ClearRootVertex()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            var algorithm = new BestFirstFrontierSearchAlgorithm<int, Edge<int>>(graph, edge => 1.0, DistanceRelaxers.EdgeShortestDistance);
            ClearRootVertex_Test(algorithm);
        }

        [Test]
        public void ComputeWithoutRoot_Throws()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            ComputeWithoutRoot_Throws_Test(
                graph,
                () => new BestFirstFrontierSearchAlgorithm<int, Edge<int>>(graph, edge => 1.0, DistanceRelaxers.EdgeShortestDistance));
        }

        #endregion

        #region Search algorithm

        [Test]
        public void TryGetTargetVertex()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            var algorithm = new BestFirstFrontierSearchAlgorithm<int, Edge<int>>(graph, edge => 1.0, DistanceRelaxers.EdgeShortestDistance);
            TryGetTargetVertex_Test(algorithm);
        }

        [Test]
        public void SetTargetVertex()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            var algorithm = new BestFirstFrontierSearchAlgorithm<int, Edge<int>>(graph, edge => 1.0, DistanceRelaxers.EdgeShortestDistance);
            SetTargetVertex_Test(algorithm);
        }

        [Test]
        public void SetTargetVertex_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            var algorithm = new BestFirstFrontierSearchAlgorithm<TestVertex, Edge<TestVertex>>(graph, edge => 1.0, DistanceRelaxers.EdgeShortestDistance);
            SetTargetVertex_Throws_Test(algorithm);
        }

        [Test]
        public void ClearTargetVertex()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            var algorithm = new BestFirstFrontierSearchAlgorithm<int, Edge<int>>(graph, edge => 1.0, DistanceRelaxers.EdgeShortestDistance);
            ClearTargetVertex_Test(algorithm);
        }

        [Test]
        public void ComputeWithRootAndTarget()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            graph.AddVertexRange(new[] { 0, 1 });
            var algorithm = new BestFirstFrontierSearchAlgorithm<int, Edge<int>>(graph, edge => 1.0, DistanceRelaxers.EdgeShortestDistance);
            ComputeWithRootAndTarget_Test(algorithm);
        }

        [Test]
        public void ComputeWithRootAndTarget_Throws()
        {
            var graph1 = new BidirectionalGraph<int, Edge<int>>();
            var algorithm1 = new BestFirstFrontierSearchAlgorithm<int, Edge<int>>(graph1, edge => 1.0, DistanceRelaxers.EdgeShortestDistance);
            ComputeWithRootAndTarget_Throws_Test(graph1, algorithm1);

            var graph2 = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            var algorithm2 = new BestFirstFrontierSearchAlgorithm<TestVertex, Edge<TestVertex>>(graph2, edge => 1.0, DistanceRelaxers.EdgeShortestDistance);
            ComputeWithRootAndTarget_Throws_Test(algorithm2);
        }

        #endregion

        [Test]
        public void SameStartAndEnd()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            graph.AddVerticesAndEdge(new Edge<int>(1, 3));
            graph.AddVerticesAndEdge(new Edge<int>(1, 2));
            graph.AddVerticesAndEdge(new Edge<int>(2, 5));
            graph.AddVerticesAndEdge(new Edge<int>(2, 4));
            graph.AddVerticesAndEdge(new Edge<int>(5, 6));
            graph.AddVerticesAndEdge(new Edge<int>(5, 7));

            var algorithm = new BestFirstFrontierSearchAlgorithm<int, Edge<int>>(
                graph, edge => 1.0, DistanceRelaxers.ShortestDistance);
            bool targetReached = false;
            algorithm.TargetReached += (sender, args) => targetReached = true;

            algorithm.Compute(1, 1);
            Assert.IsTrue(targetReached);
        }

        [Test]
        public void SimpleGraph()
        {
            var graph = new BidirectionalGraph<char, SEquatableEdge<char>>();
            graph.AddVerticesAndEdge(new SEquatableEdge<char>('A', 'C'));
            graph.AddVerticesAndEdge(new SEquatableEdge<char>('A', 'B'));
            graph.AddVerticesAndEdge(new SEquatableEdge<char>('B', 'E'));
            graph.AddVerticesAndEdge(new SEquatableEdge<char>('B', 'D'));
            graph.AddVerticesAndEdge(new SEquatableEdge<char>('E', 'F'));
            graph.AddVerticesAndEdge(new SEquatableEdge<char>('E', 'G'));

            RunAndCheckSearch(graph);
        }

        [Test]
        [Category(TestCategories.LongRunning)]
        public void BestFirstFrontierSearch()
        {
            foreach (BidirectionalGraph<string, Edge<string>> graph in TestGraphFactory.GetBidirectionalGraphs_SlowTests())
                RunAndCheckSearch(graph);
        }

        [Test]
        [Category(TestCategories.LongRunning)]
        public void BestFirstFrontierComparedToDijkstraSearch()
        {
            foreach (BidirectionalGraph<string, Edge<string>> graph in TestGraphFactory.GetBidirectionalGraphs_SlowTests())
            {
                if (graph.VertexCount == 0)
                    continue;

                string root = graph.Vertices.First();
                foreach (string vertex in graph.Vertices)
                {
                    if (!root.Equals(vertex))
                        CompareSearches(graph, root, vertex);
                }
            }
        }
    }
}