#nullable enable

using JetBrains.Annotations;
using NUnit.Framework;
using FastGraph.Algorithms;
using FastGraph.Algorithms.Observers;
using FastGraph.Algorithms.ShortestPath;
using static FastGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace FastGraph.Tests.Algorithms.ShortestPath
{
    /// <summary>
    /// Tests for <see cref="DagShortestPathAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class DagShortestPathAlgorithmTests : RootedAlgorithmTestsBase
    {
        #region Test helpers

        private static void DagAlgorithm_Test<TVertex, TEdge>(
            IVertexListGraph<TVertex, TEdge> graph,
            IDistanceRelaxer relaxer)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            // Is this a dag?
            bool isDag = graph.IsDirectedAcyclicGraph();

            TVertex[] vertices = graph.Vertices.ToArray();
            foreach (TVertex root in vertices)
            {
                if (isDag)
                {
                    RunDagShortestPathAndCheck(graph, root, relaxer);
                }
                else
                {
                    Assert.Throws<NonAcyclicGraphException>(() => RunDagShortestPathAndCheck(graph, root, relaxer));
                }
            }
        }

        private static void DagShortestPath_Test<TVertex, TEdge>(IVertexListGraph<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            DagAlgorithm_Test(graph, DistanceRelaxers.ShortestDistance);
        }

        private static void DagCriticalPath_Test<TVertex, TEdge>(
            IVertexListGraph<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            DagAlgorithm_Test(graph, DistanceRelaxers.CriticalDistance);
        }

        private static void RunDagShortestPathAndCheck<TVertex, TEdge>(
            IVertexListGraph<TVertex, TEdge> graph,
            TVertex root,
            IDistanceRelaxer relaxer)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            var algorithm = new DagShortestPathAlgorithm<TVertex, TEdge>(
                graph,
                _ => 1.0,
                relaxer);

            algorithm.InitializeVertex += vertex =>
            {
                Assert.AreEqual(GraphColor.White, algorithm.VerticesColors![vertex]);
            };

            algorithm.DiscoverVertex += vertex =>
            {
                Assert.AreEqual(GraphColor.Gray, algorithm.VerticesColors![vertex]);
            };

            algorithm.StartVertex += vertex =>
            {
                Assert.AreNotEqual(GraphColor.Black, algorithm.VerticesColors![vertex]);
            };

            algorithm.ExamineVertex += vertex =>
            {
                Assert.AreEqual(GraphColor.Gray, algorithm.VerticesColors![vertex]);
            };

            algorithm.FinishVertex += vertex =>
            {
                Assert.AreEqual(GraphColor.Black, algorithm.VerticesColors![vertex]);
            };

            var predecessors = new VertexPredecessorRecorderObserver<TVertex, TEdge>();
            using (predecessors.Attach(algorithm))
                algorithm.Compute(root);

            Assert.AreEqual(graph.VertexCount, algorithm.VerticesColors!.Count);
            foreach (TVertex vertex in graph.Vertices)
            {
                Assert.AreEqual(GraphColor.Black, algorithm.VerticesColors[vertex]);
            }

            CollectionAssert.IsNotEmpty(algorithm.GetDistances());
            Assert.AreEqual(graph.VertexCount, algorithm.GetDistances().Count());

            Verify(algorithm, predecessors);
        }

        private static void Verify<TVertex, TEdge>(
            DagShortestPathAlgorithm<TVertex, TEdge> algorithm,
            VertexPredecessorRecorderObserver<TVertex, TEdge> predecessors)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            // Verify the result
            foreach (TVertex vertex in algorithm.VisitedGraph.Vertices)
            {
                if (!predecessors.VerticesPredecessors.TryGetValue(vertex, out TEdge? predecessor))
                    continue;
                if (predecessor.Source.Equals(vertex))
                    continue;
                Assert.AreEqual(
                    algorithm.TryGetDistance(vertex, out double currentDistance),
                    algorithm.TryGetDistance(predecessor.Source, out double predecessorDistance));
                Assert.AreEqual(predecessorDistance + 1, currentDistance);
            }
        }

        #endregion

        [Test]
        public void Constructor()
        {
            Func<Edge<int>, double> Weights = _ => 1.0;

            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new DagShortestPathAlgorithm<int, Edge<int>>(graph, Weights);
            AssertAlgorithmProperties(algorithm, graph, Weights);

            algorithm = new DagShortestPathAlgorithm<int, Edge<int>>(graph, Weights, DistanceRelaxers.CriticalDistance);
            AssertAlgorithmProperties(algorithm, graph, Weights, DistanceRelaxers.CriticalDistance);

            algorithm = new DagShortestPathAlgorithm<int, Edge<int>>(default, graph, Weights, DistanceRelaxers.CriticalDistance);
            AssertAlgorithmProperties(algorithm, graph, Weights, DistanceRelaxers.CriticalDistance);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                DagShortestPathAlgorithm<TVertex, TEdge> algo,
                IVertexListGraph<TVertex, TEdge> g,
                Func<TEdge, double>? eWeights = default,
                IDistanceRelaxer? relaxer = default)
                where TVertex : notnull
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                Assert.IsNull(algo.VerticesColors);
                if (eWeights is null)
                    Assert.IsNotNull(algo.Weights);
                else
                    Assert.AreSame(eWeights, algo.Weights);
                CollectionAssert.IsEmpty(algo.GetDistances());
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
            var graph = new AdjacencyGraph<int, Edge<int>>();

            Func<Edge<int>, double> Weights = _ => 1.0;


#pragma warning disable CS8625
            Assert.Throws<ArgumentNullException>(
                () => new DagShortestPathAlgorithm<int, Edge<int>>(default, Weights));
            Assert.Throws<ArgumentNullException>(
                () => new DagShortestPathAlgorithm<int, Edge<int>>(graph, default));
            Assert.Throws<ArgumentNullException>(
                () => new DagShortestPathAlgorithm<int, Edge<int>>(default, default));

            Assert.Throws<ArgumentNullException>(
                () => new DagShortestPathAlgorithm<int, Edge<int>>(default, Weights, DistanceRelaxers.CriticalDistance));
            Assert.Throws<ArgumentNullException>(
                () => new DagShortestPathAlgorithm<int, Edge<int>>(graph, default, DistanceRelaxers.CriticalDistance));
            Assert.Throws<ArgumentNullException>(
                () => new DagShortestPathAlgorithm<int, Edge<int>>(graph, Weights, default));
            Assert.Throws<ArgumentNullException>(
                () => new DagShortestPathAlgorithm<int, Edge<int>>(default, default, DistanceRelaxers.CriticalDistance));
            Assert.Throws<ArgumentNullException>(
                () => new DagShortestPathAlgorithm<int, Edge<int>>(default, Weights, default));
            Assert.Throws<ArgumentNullException>(
                () => new DagShortestPathAlgorithm<int, Edge<int>>(graph, default, default));
            Assert.Throws<ArgumentNullException>(
                () => new DagShortestPathAlgorithm<int, Edge<int>>(default, default, default));

            Assert.Throws<ArgumentNullException>(
                () => new DagShortestPathAlgorithm<int, Edge<int>>(default, default, Weights, DistanceRelaxers.CriticalDistance));
            Assert.Throws<ArgumentNullException>(
                () => new DagShortestPathAlgorithm<int, Edge<int>>(default, graph, default, DistanceRelaxers.CriticalDistance));
            Assert.Throws<ArgumentNullException>(
                () => new DagShortestPathAlgorithm<int, Edge<int>>(default, graph, Weights, default));
            Assert.Throws<ArgumentNullException>(
                () => new DagShortestPathAlgorithm<int, Edge<int>>(default, default, default, DistanceRelaxers.CriticalDistance));
            Assert.Throws<ArgumentNullException>(
                () => new DagShortestPathAlgorithm<int, Edge<int>>(default, default, Weights, default));
            Assert.Throws<ArgumentNullException>(
                () => new DagShortestPathAlgorithm<int, Edge<int>>(default, graph, default, default));
            Assert.Throws<ArgumentNullException>(
                () => new DagShortestPathAlgorithm<int, Edge<int>>(default, default, default, default));
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        #region Rooted algorithm

        [Test]
        public void TryGetRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new DagShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0);
            TryGetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new DagShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0);
            SetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var algorithm = new DagShortestPathAlgorithm<TestVertex, Edge<TestVertex>>(graph, _ => 1.0);
            SetRootVertex_Throws_Test(algorithm);
        }

        [Test]
        public void ClearRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new DagShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0);
            ClearRootVertex_Test(algorithm);
        }

        [Test]
        public void ComputeWithoutRoot_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            ComputeWithoutRoot_Throws_Test(
                () => new DagShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0));
        }

        [Test]
        public void ComputeWithRoot()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVertex(0);
            var algorithm = new DagShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0);
            ComputeWithRoot_Test(algorithm);
        }

        [Test]
        public void ComputeWithRoot_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            ComputeWithRoot_Throws_Test(
                () => new DagShortestPathAlgorithm<TestVertex, Edge<TestVertex>>(graph, _ => 1.0));
        }

        #endregion

        [Test]
        public void GetVertexColor()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdge(new Edge<int>(1, 2));

            var algorithm = new DagShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0);
            algorithm.Compute(1);

            Assert.AreEqual(GraphColor.Black, algorithm.GetVertexColor(1));
            Assert.AreEqual(GraphColor.Black, algorithm.GetVertexColor(2));
        }

        [Test]
        [Category(TestCategories.LongRunning)]
        public void DagShortestPath()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs_SlowTests(50))
            {
                DagShortestPath_Test(graph);
                DagCriticalPath_Test(graph);
            }
        }

        [Pure]
        public static DagShortestPathAlgorithm<T, Edge<T>> CreateAlgorithmAndMaybeDoComputation<T>(
            ContractScenario<T> scenario)
            where T : notnull
        {
            var graph = new AdjacencyGraph<T, Edge<T>>();
            graph.AddVerticesAndEdgeRange(scenario.EdgesInGraph.Select(e => new Edge<T>(e.Source, e.Target)));
            graph.AddVertexRange(scenario.SingleVerticesInGraph);

            double Weights(Edge<T> e) => 1.0;
            var algorithm = new DagShortestPathAlgorithm<T, Edge<T>>(graph, Weights);

            if (scenario.DoComputation)
                algorithm.Compute(scenario.Root!);
            return algorithm;
        }
    }
}
