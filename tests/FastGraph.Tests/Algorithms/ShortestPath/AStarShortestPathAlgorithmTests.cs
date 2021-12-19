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
    /// Tests for <see cref="AStarShortestPathAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class AStartShortestPathAlgorithmTests : RootedAlgorithmTestsBase
    {
        #region Test helpers

        private static void RunAStarAndCheck<TVertex, TEdge>(
            IVertexAndEdgeListGraph<TVertex, TEdge> graph,
            TVertex root)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            var distances = new Dictionary<TEdge, double>();
            foreach (TEdge edge in graph.Edges)
                distances[edge] = graph.OutDegree(edge.Source) + 1;

            var algorithm = new AStarShortestPathAlgorithm<TVertex, TEdge>(
                graph,
                e => distances[e],
                _ => 0.0);

            algorithm.InitializeVertex += vertex =>
            {
                algorithm.VerticesColors![vertex].Should().Be(GraphColor.White);
            };

            algorithm.DiscoverVertex += vertex =>
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

            algorithm.GetDistances().Should<KeyValuePair<TVertex, double>>().NotBeEmpty();
            algorithm.GetDistances().Count().Should().Be(graph.VertexCount);

            Verify(algorithm, predecessors);
        }

        private static void Verify<TVertex, TEdge>(
            AStarShortestPathAlgorithm<TVertex, TEdge> algorithm,
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
            Func<int, double> Heuristic = _ => 1.0;
            Func<Edge<int>, double> Weights = _ => 1.0;

            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new AStarShortestPathAlgorithm<int, Edge<int>>(graph, Weights, Heuristic);
            AssertAlgorithmProperties(algorithm, graph, Heuristic, Weights);

            algorithm = new AStarShortestPathAlgorithm<int, Edge<int>>(graph, Weights, Heuristic, DistanceRelaxers.CriticalDistance);
            AssertAlgorithmProperties(algorithm, graph, Heuristic, Weights, DistanceRelaxers.CriticalDistance);

            algorithm = new AStarShortestPathAlgorithm<int, Edge<int>>(default, graph, Weights, Heuristic, DistanceRelaxers.CriticalDistance);
            AssertAlgorithmProperties(algorithm, graph, Heuristic, Weights, DistanceRelaxers.CriticalDistance);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                AStarShortestPathAlgorithm<TVertex, TEdge> algo,
                IVertexListGraph<TVertex, TEdge> g,
                Func<TVertex, double>? heuristic = default,
                Func<TEdge, double>? eWeights = default,
                IDistanceRelaxer? relaxer = default)
                where TVertex : notnull
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                algo.VerticesColors.Should().BeNull();
                if (heuristic is null)
                    algo.CostHeuristic.Should().NotBeNull();
                else
                    algo.CostHeuristic.Should().BeSameAs(heuristic);
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

            Func<int, double> Heuristic = _ => 1.0;
            Func<Edge<int>, double> Weights = _ => 1.0;

#pragma warning disable CS8625
            Invoking(() => new AStarShortestPathAlgorithm<int, Edge<int>>(default, Weights, Heuristic)).Should().Throw<ArgumentNullException>();
            Invoking(() => new AStarShortestPathAlgorithm<int, Edge<int>>(graph, default, Heuristic)).Should().Throw<ArgumentNullException>();
            Invoking(() => new AStarShortestPathAlgorithm<int, Edge<int>>(graph, Weights, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new AStarShortestPathAlgorithm<int, Edge<int>>(graph, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new AStarShortestPathAlgorithm<int, Edge<int>>(default, default, Heuristic)).Should().Throw<ArgumentNullException>();
            Invoking(() => new AStarShortestPathAlgorithm<int, Edge<int>>(default, Weights, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new AStarShortestPathAlgorithm<int, Edge<int>>(default, default, Heuristic)).Should().Throw<ArgumentNullException>();
            Invoking(() => new AStarShortestPathAlgorithm<int, Edge<int>>(default, default, default)).Should().Throw<ArgumentNullException>();

            Invoking(() => new AStarShortestPathAlgorithm<int, Edge<int>>(default, Weights, Heuristic, DistanceRelaxers.CriticalDistance)).Should().Throw<ArgumentNullException>();
            Invoking(() => new AStarShortestPathAlgorithm<int, Edge<int>>(graph, default, Heuristic, DistanceRelaxers.CriticalDistance)).Should().Throw<ArgumentNullException>();
            Invoking(() => new AStarShortestPathAlgorithm<int, Edge<int>>(graph, Weights, default, DistanceRelaxers.CriticalDistance)).Should().Throw<ArgumentNullException>();
            Invoking(() => new AStarShortestPathAlgorithm<int, Edge<int>>(graph, Weights, Heuristic, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new AStarShortestPathAlgorithm<int, Edge<int>>(graph, Weights, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new AStarShortestPathAlgorithm<int, Edge<int>>(graph, default, Heuristic, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new AStarShortestPathAlgorithm<int, Edge<int>>(graph, default, default, DistanceRelaxers.CriticalDistance)).Should().Throw<ArgumentNullException>();
            Invoking(() => new AStarShortestPathAlgorithm<int, Edge<int>>(default, Weights, Heuristic, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new AStarShortestPathAlgorithm<int, Edge<int>>(default, Weights, default, DistanceRelaxers.CriticalDistance)).Should().Throw<ArgumentNullException>();
            Invoking(() => new AStarShortestPathAlgorithm<int, Edge<int>>(default, default, Heuristic, DistanceRelaxers.CriticalDistance)).Should().Throw<ArgumentNullException>();
            Invoking(() => new AStarShortestPathAlgorithm<int, Edge<int>>(default, Weights, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new AStarShortestPathAlgorithm<int, Edge<int>>(default, default, Heuristic, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new AStarShortestPathAlgorithm<int, Edge<int>>(default, default, default, DistanceRelaxers.CriticalDistance)).Should().Throw<ArgumentNullException>();
            Invoking(() => new AStarShortestPathAlgorithm<int, Edge<int>>(default, default, default, default)).Should().Throw<ArgumentNullException>();

            Invoking(() => new AStarShortestPathAlgorithm<int, Edge<int>>(default, default, Weights, Heuristic, DistanceRelaxers.CriticalDistance)).Should().Throw<ArgumentNullException>();
            Invoking(() => new AStarShortestPathAlgorithm<int, Edge<int>>(default, graph, default, Heuristic, DistanceRelaxers.CriticalDistance)).Should().Throw<ArgumentNullException>();
            Invoking(() => new AStarShortestPathAlgorithm<int, Edge<int>>(default, graph, Weights, default, DistanceRelaxers.CriticalDistance)).Should().Throw<ArgumentNullException>();
            Invoking(() => new AStarShortestPathAlgorithm<int, Edge<int>>(default, graph, Weights, Heuristic, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new AStarShortestPathAlgorithm<int, Edge<int>>(default, graph, Weights, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new AStarShortestPathAlgorithm<int, Edge<int>>(default, graph, default, Heuristic, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new AStarShortestPathAlgorithm<int, Edge<int>>(default, graph, default, default, DistanceRelaxers.CriticalDistance)).Should().Throw<ArgumentNullException>();
            Invoking(() => new AStarShortestPathAlgorithm<int, Edge<int>>(default, default, Weights, Heuristic, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new AStarShortestPathAlgorithm<int, Edge<int>>(default, default, Weights, default, DistanceRelaxers.CriticalDistance)).Should().Throw<ArgumentNullException>();
            Invoking(() => new AStarShortestPathAlgorithm<int, Edge<int>>(default, default, default, Heuristic, DistanceRelaxers.CriticalDistance)).Should().Throw<ArgumentNullException>();
            Invoking(() => new AStarShortestPathAlgorithm<int, Edge<int>>(default, default, Weights, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new AStarShortestPathAlgorithm<int, Edge<int>>(default, default, default, Heuristic, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new AStarShortestPathAlgorithm<int, Edge<int>>(default, default, default, default, DistanceRelaxers.CriticalDistance)).Should().Throw<ArgumentNullException>();
            Invoking(() => new AStarShortestPathAlgorithm<int, Edge<int>>(default, default, default, default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        #region Rooted algorithm

        [Test]
        public void TryGetRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new AStarShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0, _ => 0.0);
            TryGetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new AStarShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0, _ => 0.0);
            SetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var algorithm = new AStarShortestPathAlgorithm<TestVertex, Edge<TestVertex>>(graph, _ => 1.0, _ => 0.0);
            SetRootVertex_Throws_Test(algorithm);
        }

        [Test]
        public void ClearRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new AStarShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0, _ => 0.0);
            ClearRootVertex_Test(algorithm);
        }

        [Test]
        public void ComputeWithoutRoot_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            ComputeWithoutRoot_NoThrows_Test(
                graph,
                () => new AStarShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0, _ => 0.0));
        }

        [Test]
        public void ComputeWithRoot()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVertex(0);
            var algorithm = new AStarShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0, _ => 0.0);
            ComputeWithRoot_Test(algorithm);
        }

        [Test]
        public void ComputeWithRoot_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            ComputeWithRoot_Throws_Test(
                () => new AStarShortestPathAlgorithm<TestVertex, Edge<TestVertex>>(graph, _ => 1.0, _ => 0.0));
        }

        #endregion

        [Test]
        public void GetVertexColor()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdge(new Edge<int>(1, 2));

            var algorithm = new AStarShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0, _ => 0.0);
            algorithm.Compute(1);

            algorithm.GetVertexColor(1).Should().Be(GraphColor.Black);
            algorithm.GetVertexColor(2).Should().Be(GraphColor.Black);
        }

        [Test]
        [Category(TestCategories.LongRunning)]
        public void AStar()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs_SlowTests())
            {
                foreach (string root in graph.Vertices)
                    RunAStarAndCheck(graph, root);
            }
        }

        [Test]
        public void AStar_Throws()
        {
            var edge12 = new Edge<int>(1, 2);
            var edge23 = new Edge<int>(2, 3);
            var edge34 = new Edge<int>(3, 4);

            var negativeWeightGraph = new AdjacencyGraph<int, Edge<int>>();
            negativeWeightGraph.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge23, edge34
            });

            var algorithm = new AStarShortestPathAlgorithm<int, Edge<int>>(
                negativeWeightGraph,
                e =>
                {
                    if (e == edge12)
                        return 12.0;
                    if (e == edge23)
                        return -23.0;
                    if (e == edge34)
                        return 34.0;
                    return 1.0;
                },
                _ => 0.0);
            Invoking(() => algorithm.Compute(1)).Should().Throw<NegativeWeightException>();
        }

        [Test]
        public void AStar_HeuristicCalls()
        {
            var edge01 = new Edge<int>(0, 1);
            var edge02 = new Edge<int>(0, 2);
            var edge03 = new Edge<int>(0, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge23 = new Edge<int>(2, 3);
            var edge34 = new Edge<int>(3, 4);

            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                edge01,
                edge02,
                edge03,
                edge23,
                edge14,
                edge34
            });

            const int root = 0;

            var colorUpdates = new HashSet<GraphColor>
            {
                GraphColor.White, GraphColor.Gray, GraphColor.Black
            };

            int heuristicCalls = 0;
            AStarShortestPathAlgorithm<int, Edge<int>>? algorithm = default;
            Func<int, double> heuristic = v =>
            {
                // ReSharper disable once PossibleNullReferenceException
                // ReSharper disable once AccessToModifiedClosure
                colorUpdates.Remove(algorithm!.GetVertexColor(v));
                ++heuristicCalls;
                return 10.0 / heuristicCalls;
            };

            algorithm = new AStarShortestPathAlgorithm<int, Edge<int>>(
                graph,
                e =>
                {
                    if (e == edge01)
                        return 8.0;
                    if (e == edge02)
                        return 6.0;
                    if (e == edge03)
                        return 20.0;
                    if (e == edge34)
                        return 5.0;
                    return 1.0;
                },
                heuristic);

            algorithm.Compute(root);

            colorUpdates.Should().BeEmpty();
        }

        [Test]
        public void AStar_HeuristicCallCount()
        {
            var lineGraph = new AdjacencyGraph<int, Edge<int>>();
            lineGraph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(2, 3),
                new Edge<int>(3, 4),
                new Edge<int>(2, 1),
                new Edge<int>(1, 0)
            });

            const int root = 2;

            var heuristicCalls = new List<int>();
            var algorithm = new AStarShortestPathAlgorithm<int, Edge<int>>(
                lineGraph,
                _ => 1.0,
                v =>
                {
                    // Goal is 2, h(v) = v
                    heuristicCalls.Add(v);
                    return v;
                });

            algorithm.Compute(root);

            // Heuristic function must be called at least 4 times
            4.Should().BeGreaterThanOrEqualTo(heuristicCalls.Count);

            // 0 must be expanded before 4
            heuristicCalls.Should().Contain(0);
            heuristicCalls.Should().Contain(4);
            heuristicCalls.IndexOf(0).Should().BeLessThan(heuristicCalls.IndexOf(4));
        }

        [Pure]
        public static AStarShortestPathAlgorithm<T, Edge<T>> CreateAlgorithmAndMaybeDoComputation<T>(
            ContractScenario<T> scenario)
            where T : notnull
        {
            var graph = new AdjacencyGraph<T, Edge<T>>();
            graph.AddVerticesAndEdgeRange(scenario.EdgesInGraph.Select(e => new Edge<T>(e.Source, e.Target)));
            graph.AddVertexRange(scenario.SingleVerticesInGraph);

            double Heuristic(T v) => 1.0;
            double Weights(Edge<T> e) => 1.0;
            var algorithm = new AStarShortestPathAlgorithm<T, Edge<T>>(graph, Weights, Heuristic);

            if (scenario.DoComputation)
                algorithm.Compute(scenario.Root!);
            return algorithm;
        }
    }
}
