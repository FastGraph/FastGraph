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
    /// Tests for <see cref="BellmanFordShortestPathAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class BellmanFordShortestPathAlgorithmTests : RootedAlgorithmTestsBase
    {
        #region Test helpers

        private static void RunBellmanFordAndCheck<TVertex, TEdge>(
            IVertexAndEdgeListGraph<TVertex, TEdge> graph,
            TVertex root)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            var distances = new Dictionary<TEdge, double>();
            foreach (TEdge edge in graph.Edges)
                distances[edge] = graph.OutDegree(edge.Source) + 1;

            var algorithm = new BellmanFordShortestPathAlgorithm<TVertex, TEdge>(
                graph,
                e => distances[e]);

            algorithm.InitializeVertex += vertex =>
            {
                algorithm.VerticesColors![vertex].Should().Be(GraphColor.White);
            };

            var predecessors = new VertexPredecessorRecorderObserver<TVertex, TEdge>();
            using (predecessors.Attach(algorithm))
                algorithm.Compute(root);

            algorithm.VerticesColors!.Count.Should().Be(graph.VertexCount);
            foreach (TVertex vertex in graph.Vertices)
            {
                algorithm.VerticesColors[vertex].Should().Be(GraphColor.Black);
            }

            algorithm.FoundNegativeCycle.Should().BeFalse();
            algorithm.GetDistances().Should<KeyValuePair<TVertex, double>>().NotBeEmpty();
            algorithm.GetDistances().Count().Should().Be(graph.VertexCount);

            Verify(algorithm, predecessors);
        }

        private static void Verify<TVertex, TEdge>(
            BellmanFordShortestPathAlgorithm<TVertex, TEdge> algorithm,
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
                currentDistance.Should().BeGreaterThanOrEqualTo(predecessorDistance);
            }
        }

        #endregion

        [Test]
        public void Constructor()
        {
            Func<Edge<int>, double> Weights = _ => 1.0;

            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new BellmanFordShortestPathAlgorithm<int, Edge<int>>(graph, Weights);
            AssertAlgorithmProperties(algorithm, graph, Weights);

            algorithm = new BellmanFordShortestPathAlgorithm<int, Edge<int>>(graph, Weights, DistanceRelaxers.CriticalDistance);
            AssertAlgorithmProperties(algorithm, graph, Weights, DistanceRelaxers.CriticalDistance);

            algorithm = new BellmanFordShortestPathAlgorithm<int, Edge<int>>(default, graph, Weights, DistanceRelaxers.CriticalDistance);
            AssertAlgorithmProperties(algorithm, graph, Weights, DistanceRelaxers.CriticalDistance);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                BellmanFordShortestPathAlgorithm<TVertex, TEdge> algo,
                IVertexAndEdgeListGraph<TVertex, TEdge> g,
                Func<TEdge, double>? eWeights = default,
                IDistanceRelaxer? relaxer = default)
                where TVertex : notnull
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                algo.VerticesColors.Should().BeNull();
                algo.FoundNegativeCycle.Should().BeFalse();
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
            Invoking(() => new BellmanFordShortestPathAlgorithm<int, Edge<int>>(default, Weights)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BellmanFordShortestPathAlgorithm<int, Edge<int>>(graph, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BellmanFordShortestPathAlgorithm<int, Edge<int>>(default, default)).Should().Throw<ArgumentNullException>();

            Invoking(() => new BellmanFordShortestPathAlgorithm<int, Edge<int>>(default, Weights, DistanceRelaxers.CriticalDistance)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BellmanFordShortestPathAlgorithm<int, Edge<int>>(graph, default, DistanceRelaxers.CriticalDistance)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BellmanFordShortestPathAlgorithm<int, Edge<int>>(graph, Weights, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BellmanFordShortestPathAlgorithm<int, Edge<int>>(default, default, DistanceRelaxers.CriticalDistance)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BellmanFordShortestPathAlgorithm<int, Edge<int>>(default, Weights, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BellmanFordShortestPathAlgorithm<int, Edge<int>>(graph, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BellmanFordShortestPathAlgorithm<int, Edge<int>>(default, default, default)).Should().Throw<ArgumentNullException>();

            Invoking(() => new BellmanFordShortestPathAlgorithm<int, Edge<int>>(default, default, Weights, DistanceRelaxers.CriticalDistance)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BellmanFordShortestPathAlgorithm<int, Edge<int>>(default, graph, default, DistanceRelaxers.CriticalDistance)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BellmanFordShortestPathAlgorithm<int, Edge<int>>(default, graph, Weights, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BellmanFordShortestPathAlgorithm<int, Edge<int>>(default, default, default, DistanceRelaxers.CriticalDistance)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BellmanFordShortestPathAlgorithm<int, Edge<int>>(default, default, Weights, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BellmanFordShortestPathAlgorithm<int, Edge<int>>(default, graph, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BellmanFordShortestPathAlgorithm<int, Edge<int>>(default, default, default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        #region Rooted algorithm

        [Test]
        public void TryGetRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new BellmanFordShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0);
            TryGetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new BellmanFordShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0);
            SetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var algorithm = new BellmanFordShortestPathAlgorithm<TestVertex, Edge<TestVertex>>(graph, _ => 1.0);
            SetRootVertex_Throws_Test(algorithm);
        }

        [Test]
        public void ClearRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new BellmanFordShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0);
            ClearRootVertex_Test(algorithm);
        }

        [Test]
        public void ComputeWithoutRoot_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            ComputeWithoutRoot_Throws_Test(
                () => new BellmanFordShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0));
        }

        [Test]
        public void ComputeWithRoot()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVertex(0);
            var algorithm = new BellmanFordShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0);
            ComputeWithRoot_Test(algorithm);
        }

        [Test]
        public void ComputeWithRoot_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            ComputeWithRoot_Throws_Test(
                () => new BellmanFordShortestPathAlgorithm<TestVertex, Edge<TestVertex>>(graph, _ => 1.0));
        }

        #endregion

        [Test]
        public void GetVertexColor()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdge(new Edge<int>(1, 2));

            var algorithm = new BellmanFordShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0);
            algorithm.Compute(1);

            algorithm.GetVertexColor(1).Should().Be(GraphColor.Black);
            algorithm.GetVertexColor(2).Should().Be(GraphColor.Black);
        }

        [Test]
        [Category(TestCategories.LongRunning)]
        public void BellmanFord()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs_SlowTests())
            {
                foreach (string root in graph.Vertices)
                    RunBellmanFordAndCheck(graph, root);
            }
        }

        [Test]
        public void BellmanFord_NegativeCycle()
        {
            // Without negative cycle
            var edge12 = new Edge<int>(1, 2);
            var edge23 = new Edge<int>(2, 3);
            var edge34 = new Edge<int>(3, 4);

            var negativeWeightGraph = new AdjacencyGraph<int, Edge<int>>();
            negativeWeightGraph.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge23, edge34
            });

            var algorithm = new BellmanFordShortestPathAlgorithm<int, Edge<int>>(
                negativeWeightGraph,
                e =>
                {
                    if (e == edge12)
                        return 12.0;
                    if (e == edge23)
                        return -23.0;
                    if (e == edge34)
                        return -34.0;
                    return 1.0;
                });
            Invoking(() => algorithm.Compute(1)).Should().NotThrow();
            algorithm.FoundNegativeCycle.Should().BeFalse();

            // With negative cycle
            var edge41 = new Edge<int>(4, 1);

            var negativeCycleGraph = new AdjacencyGraph<int, Edge<int>>();
            negativeCycleGraph.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge23, edge34, edge41
            });

            algorithm = new BellmanFordShortestPathAlgorithm<int, Edge<int>>(
                negativeCycleGraph,
                e =>
                {
                    if (e == edge12)
                        return 12.0;
                    if (e == edge23)
                        return -23.0;
                    if (e == edge34)
                        return -34.0;
                    if (e == edge41)
                        return 41.0;
                    return 1.0;
                });
            Invoking(() => algorithm.Compute(1)).Should().NotThrow();
            algorithm.FoundNegativeCycle.Should().BeTrue();
        }

        [Pure]
        public static BellmanFordShortestPathAlgorithm<T, Edge<T>> CreateAlgorithmAndMaybeDoComputation<T>(
            ContractScenario<T> scenario)
            where T : notnull
        {
            var graph = new AdjacencyGraph<T, Edge<T>>();
            graph.AddVerticesAndEdgeRange(scenario.EdgesInGraph.Select(e => new Edge<T>(e.Source, e.Target)));
            graph.AddVertexRange(scenario.SingleVerticesInGraph);

            double Weights(Edge<T> e) => 1.0;
            var algorithm = new BellmanFordShortestPathAlgorithm<T, Edge<T>>(graph, Weights);

            if (scenario.DoComputation)
                algorithm.Compute(scenario.Root!);
            return algorithm;
        }
    }
}
