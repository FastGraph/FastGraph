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
                    Invoking(() => RunDagShortestPathAndCheck(graph, root, relaxer)).Should().Throw<NonAcyclicGraphException>();
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
                algorithm.VerticesColors![vertex].Should().Be(GraphColor.White);
            };

            algorithm.DiscoverVertex += vertex =>
            {
                algorithm.VerticesColors![vertex].Should().Be(GraphColor.Gray);
            };

            algorithm.StartVertex += vertex =>
            {
                algorithm.VerticesColors![vertex].Should().NotBe(GraphColor.Black);
            };

            algorithm.ExamineVertex += vertex =>
            {
                algorithm.VerticesColors![vertex].Should().Be(GraphColor.Gray);
            };

            algorithm.FinishVertex += vertex =>
            {
                algorithm.VerticesColors![vertex].Should().Be(GraphColor.Black);
            };

            var predecessors = new VertexPredecessorRecorderObserver<TVertex, TEdge>();
            using (predecessors.Attach(algorithm))
                algorithm.Compute(root);

            algorithm.VerticesColors!.Count.Should().Be(graph.VertexCount);
            foreach (TVertex vertex in graph.Vertices)
            {
                algorithm.VerticesColors[vertex].Should().Be(GraphColor.Black);
            }

            algorithm.GetDistances().Should<KeyValuePair<TVertex, double>>().NotBeEmpty();
            algorithm.GetDistances().Count().Should().Be(graph.VertexCount);

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
                algorithm.TryGetDistance(predecessor.Source, out double predecessorDistance)
                    .Should().Be(algorithm.TryGetDistance(vertex, out double currentDistance));
                currentDistance.Should().Be(predecessorDistance + 1);
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
                algo.VerticesColors.Should().BeNull();
                if (eWeights is null)
                    algo.Weights.Should().NotBeNull();
                else
                    algo.Weights.Should().BeSameAs(eWeights);
                algo.GetDistances().Should<KeyValuePair<TVertex, double>>().BeEmpty();
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
            var graph = new AdjacencyGraph<int, Edge<int>>();

            Func<Edge<int>, double> Weights = _ => 1.0;


#pragma warning disable CS8625
            Invoking(() => new DagShortestPathAlgorithm<int, Edge<int>>(default, Weights)).Should().Throw<ArgumentNullException>();
            Invoking(() => new DagShortestPathAlgorithm<int, Edge<int>>(graph, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new DagShortestPathAlgorithm<int, Edge<int>>(default, default)).Should().Throw<ArgumentNullException>();

            Invoking(() => new DagShortestPathAlgorithm<int, Edge<int>>(default, Weights, DistanceRelaxers.CriticalDistance)).Should().Throw<ArgumentNullException>();
            Invoking(() => new DagShortestPathAlgorithm<int, Edge<int>>(graph, default, DistanceRelaxers.CriticalDistance)).Should().Throw<ArgumentNullException>();
            Invoking(() => new DagShortestPathAlgorithm<int, Edge<int>>(graph, Weights, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new DagShortestPathAlgorithm<int, Edge<int>>(default, default, DistanceRelaxers.CriticalDistance)).Should().Throw<ArgumentNullException>();
            Invoking(() => new DagShortestPathAlgorithm<int, Edge<int>>(default, Weights, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new DagShortestPathAlgorithm<int, Edge<int>>(graph, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new DagShortestPathAlgorithm<int, Edge<int>>(default, default, default)).Should().Throw<ArgumentNullException>();

            Invoking(() => new DagShortestPathAlgorithm<int, Edge<int>>(default, default, Weights, DistanceRelaxers.CriticalDistance)).Should().Throw<ArgumentNullException>();
            Invoking(() => new DagShortestPathAlgorithm<int, Edge<int>>(default, graph, default, DistanceRelaxers.CriticalDistance)).Should().Throw<ArgumentNullException>();
            Invoking(() => new DagShortestPathAlgorithm<int, Edge<int>>(default, graph, Weights, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new DagShortestPathAlgorithm<int, Edge<int>>(default, default, default, DistanceRelaxers.CriticalDistance)).Should().Throw<ArgumentNullException>();
            Invoking(() => new DagShortestPathAlgorithm<int, Edge<int>>(default, default, Weights, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new DagShortestPathAlgorithm<int, Edge<int>>(default, graph, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new DagShortestPathAlgorithm<int, Edge<int>>(default, default, default, default)).Should().Throw<ArgumentNullException>();
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

            algorithm.GetVertexColor(1).Should().Be(GraphColor.Black);
            algorithm.GetVertexColor(2).Should().Be(GraphColor.Black);
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
