#nullable enable

using JetBrains.Annotations;
using NUnit.Framework;
using FastGraph.Algorithms.Observers;
using FastGraph.Algorithms.Search;
using FastGraph.Collections;
using static FastGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace FastGraph.Tests.Algorithms.Search
{
    /// <summary>
    /// Tests for <see cref="UndirectedBreadthFirstSearchAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class UndirectedBreadthFirstAlgorithmSearchTests : RootedAlgorithmTestsBase
    {
        #region Test helpers

        private static void RunBFSAndCheck<TVertex, TEdge>(
            IUndirectedGraph<TVertex, TEdge> graph,
            TVertex sourceVertex)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            var parents = new Dictionary<TVertex, TVertex>();
            var distances = new Dictionary<TVertex, int>();
            TVertex? currentVertex = default;
            int currentDistance = 0;
            var algorithm = new UndirectedBreadthFirstSearchAlgorithm<TVertex, TEdge>(graph);

            algorithm.InitializeVertex += vertex =>
            {
                algorithm.VerticesColors[vertex].Should().Be(GraphColor.White);
            };

            algorithm.StartVertex += vertex =>
            {
                algorithm.VerticesColors[vertex].Should().Be(GraphColor.White);
            };

            algorithm.DiscoverVertex += vertex =>
            {
                algorithm.VerticesColors[vertex].Should().Be(GraphColor.Gray);
                if (vertex.Equals(sourceVertex))
                {
                    currentVertex = sourceVertex;
                }
                else
                {
                    currentVertex.Should().NotBeNull();
                    parents[vertex].Should().Be(currentVertex);
                    // ReSharper disable once AccessToModifiedClosure
                    distances[vertex].Should().Be(currentDistance + 1);
                    distances[vertex].Should().Be(distances[parents[vertex]] + 1);
                }
            };

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            algorithm.ExamineEdge += edge =>
            {
                (edge.Source.Equals(currentVertex) || edge.Target.Equals(currentVertex)).Should().BeTrue();
            };

            algorithm.ExamineVertex += vertex =>
            {
                TVertex u = vertex;
                currentVertex = u;
                // Ensure that the distances monotonically increase.
                // ReSharper disable AccessToModifiedClosure
                (distances[u] == currentDistance || distances[u] == currentDistance + 1).Should().BeTrue();

                if (distances[u] == currentDistance + 1) // New level
                    ++currentDistance;
                // ReSharper restore AccessToModifiedClosure
            };

            algorithm.TreeEdge += (_, args) =>
            {
                TVertex u = args.Edge.Source;
                TVertex v = args.Edge.Target;
                if (algorithm.VerticesColors[v] == GraphColor.Gray)
                {
                    TVertex temp = u;
                    u = v;
                    v = temp;
                }

                algorithm.VerticesColors[v].Should().Be(GraphColor.White);
                currentDistance.Should().Be(distances[u]);
                parents[v] = u;
                distances[v] = distances[u] + 1;
            };

            algorithm.NonTreeEdge += (_, args) =>
            {
                TVertex u = args.Edge.Source;
                TVertex v = args.Edge.Target;
                if (algorithm.VerticesColors[v] != GraphColor.White)
                {
                    TVertex temp = u;
                    u = v;
                    v = temp;
                }

                (algorithm.VerticesColors[v] == GraphColor.White).Should().BeFalse();

                if (algorithm.VisitedGraph.IsDirected)
                {
                    // Cross or back edge
                    (distances[v] <= distances[u] + 1).Should().BeTrue();
                }
                else
                {
                    // Cross edge (or going backwards on a tree edge)
                    (distances[v] == distances[u]
                     || distances[v] == distances[u] + 1
                     || distances[v] == distances[u] - 1).Should().BeTrue();
                }
            };

            algorithm.GrayTarget += (_, args) =>
            {
                algorithm.VerticesColors[args.Target].Should().Be(GraphColor.Gray);
            };

            algorithm.BlackTarget += (_, args) =>
            {
                algorithm.VerticesColors[args.Target].Should().Be(GraphColor.Black);

                foreach (TEdge edge in algorithm.VisitedGraph.AdjacentEdges(args.Target))
                    (algorithm.VerticesColors[edge.Target] == GraphColor.White).Should().BeFalse();
            };

            algorithm.FinishVertex += vertex =>
            {
                algorithm.VerticesColors[vertex].Should().Be(GraphColor.Black);
            };

            parents.Clear();
            distances.Clear();
            currentDistance = 0;

            foreach (TVertex vertex in graph.Vertices)
            {
                distances[vertex] = int.MaxValue;
                parents[vertex] = vertex;
            }

            distances[sourceVertex] = 0;

            var recorder = new UndirectedVertexPredecessorRecorderObserver<TVertex, TEdge>();
            using (recorder.Attach(algorithm))
            {
                algorithm.Compute(sourceVertex);
            }

            // All white vertices should be unreachable from the source.
            foreach (TVertex vertex in graph.Vertices)
            {
                if (algorithm.VerticesColors[vertex] == GraphColor.White)
                {
                    // Check !IsReachable(sourceVertex, vertex, graph);
                    if (recorder.TryGetPath(vertex, out IEnumerable<TEdge>? path))
                    {
                        foreach (TEdge edge in path)
                        {
                            edge.Source.Should().NotBe(sourceVertex);
                            edge.Target.Should().NotBe(sourceVertex);
                        }
                    }
                }
            }

            // The shortest path to a child should be one longer than
            // shortest path to the parent.
            foreach (TVertex vertex in graph.Vertices)
            {
                if (!parents[vertex].Equals(vertex)) // Not the root of the BFS tree
                {
                    distances[vertex].Should().Be(distances[parents[vertex]] + 1);
                }
            }
        }

        #endregion

        [Test]
        public void Constructor()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            var algorithm = new UndirectedBreadthFirstSearchAlgorithm<int, Edge<int>>(graph);
            AssertAlgorithmProperties(algorithm, graph);

            var verticesColors = new Dictionary<int, GraphColor>();
            var queue = new BinaryQueue<int, double>(_ => 1.0);
            algorithm = new UndirectedBreadthFirstSearchAlgorithm<int, Edge<int>>(graph, queue, verticesColors);
            AssertAlgorithmProperties(algorithm, graph, verticesColors);

            algorithm = new UndirectedBreadthFirstSearchAlgorithm<int, Edge<int>>(default, graph, queue, verticesColors);
            AssertAlgorithmProperties(algorithm, graph, verticesColors);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                UndirectedBreadthFirstSearchAlgorithm<TVertex, TEdge> algo,
                IUndirectedGraph<TVertex, TEdge> g,
                IDictionary<TVertex, GraphColor>? vColors = default)
                where TVertex : notnull
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                if (vColors is null)
                    algo.VerticesColors.Should().BeEmpty();
                else
                    algo.VerticesColors.Should().BeSameAs(vColors);
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            var graph = new UndirectedGraph<int, Edge<int>>();
            var verticesColors = new Dictionary<int, GraphColor>();
            var queue = new BinaryQueue<int, double>(_ => 1.0);

#pragma warning disable CS8625
            Invoking(() => new UndirectedBreadthFirstSearchAlgorithm<int, Edge<int>>(default)).Should().Throw<ArgumentNullException>();

            Invoking(() => new UndirectedBreadthFirstSearchAlgorithm<int, Edge<int>>(default, queue, verticesColors)).Should().Throw<ArgumentNullException>();
            Invoking(() => new UndirectedBreadthFirstSearchAlgorithm<int, Edge<int>>(graph, default, verticesColors)).Should().Throw<ArgumentNullException>();
            Invoking(() => new UndirectedBreadthFirstSearchAlgorithm<int, Edge<int>>(graph, queue, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new UndirectedBreadthFirstSearchAlgorithm<int, Edge<int>>(default, default, verticesColors)).Should().Throw<ArgumentNullException>();
            Invoking(() => new UndirectedBreadthFirstSearchAlgorithm<int, Edge<int>>(default, queue, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new UndirectedBreadthFirstSearchAlgorithm<int, Edge<int>>(graph, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new UndirectedBreadthFirstSearchAlgorithm<int, Edge<int>>(default, default, default)).Should().Throw<ArgumentNullException>();

            Invoking(() => new UndirectedBreadthFirstSearchAlgorithm<int, Edge<int>>(default, default, queue, verticesColors)).Should().Throw<ArgumentNullException>();
            Invoking(() => new UndirectedBreadthFirstSearchAlgorithm<int, Edge<int>>(default, graph, default, verticesColors)).Should().Throw<ArgumentNullException>();
            Invoking(() => new UndirectedBreadthFirstSearchAlgorithm<int, Edge<int>>(default, graph, queue, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new UndirectedBreadthFirstSearchAlgorithm<int, Edge<int>>(default, default, default, verticesColors)).Should().Throw<ArgumentNullException>();
            Invoking(() => new UndirectedBreadthFirstSearchAlgorithm<int, Edge<int>>(default, default, queue, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new UndirectedBreadthFirstSearchAlgorithm<int, Edge<int>>(default, graph, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new UndirectedBreadthFirstSearchAlgorithm<int, Edge<int>>(default, default, default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        #region Rooted algorithm

        [Test]
        public void TryGetRootVertex()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            var algorithm = new UndirectedBreadthFirstSearchAlgorithm<int, Edge<int>>(graph);
            TryGetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            var algorithm = new UndirectedBreadthFirstSearchAlgorithm<int, Edge<int>>(graph);
            SetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex_Throws()
        {
            var graph = new UndirectedGraph<TestVertex, Edge<TestVertex>>();
            var algorithm = new UndirectedBreadthFirstSearchAlgorithm<TestVertex, Edge<TestVertex>>(graph);
            SetRootVertex_Throws_Test(algorithm);
        }

        [Test]
        public void ClearRootVertex()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            var algorithm = new UndirectedBreadthFirstSearchAlgorithm<int, Edge<int>>(graph);
            ClearRootVertex_Test(algorithm);
        }

        [Test]
        public void ComputeWithoutRoot_Throws()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            ComputeWithoutRoot_Throws_Test(
                () => new UndirectedBreadthFirstSearchAlgorithm<int, Edge<int>>(graph));
        }

        [Test]
        public void ComputeWithRoot()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            graph.AddVertex(0);
            var algorithm = new UndirectedBreadthFirstSearchAlgorithm<int, Edge<int>>(graph);
            ComputeWithRoot_Test(algorithm);
        }

        [Test]
        public void ComputeWithRoot_Throws()
        {
            var graph = new UndirectedGraph<TestVertex, Edge<TestVertex>>();
            ComputeWithRoot_Throws_Test(
                () => new UndirectedBreadthFirstSearchAlgorithm<TestVertex, Edge<TestVertex>>(graph));
        }

        #endregion

        [Test]
        public void GetVertexColor()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            graph.AddVerticesAndEdge(new Edge<int>(1, 2));

            var algorithm = new UndirectedBreadthFirstSearchAlgorithm<int, Edge<int>>(graph);
            // Algorithm not run
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Invoking(() => algorithm.GetVertexColor(1)).Should().Throw<VertexNotFoundException>();

            algorithm.Compute(1);

            algorithm.GetVertexColor(1).Should().Be(GraphColor.Black);
            algorithm.GetVertexColor(2).Should().Be(GraphColor.Black);
        }

        [Test]
        [Category(TestCategories.LongRunning)]
        public void UndirectedBreadthFirstSearch()
        {
            foreach (UndirectedGraph<string, Edge<string>> graph in TestGraphFactory.GetUndirectedGraphs_SlowTests(10))
            {
                foreach (string vertex in graph.Vertices)
                    RunBFSAndCheck(graph, vertex);
            }
        }

        [Pure]
        public static UndirectedBreadthFirstSearchAlgorithm<T, Edge<T>> CreateAlgorithmAndMaybeDoComputation<T>(
            ContractScenario<T> scenario)
            where T : notnull
        {
            var graph = new UndirectedGraph<T, Edge<T>>();
            graph.AddVerticesAndEdgeRange(scenario.EdgesInGraph.Select(e => new Edge<T>(e.Source, e.Target)));
            graph.AddVertexRange(scenario.SingleVerticesInGraph);

            var algorithm = new UndirectedBreadthFirstSearchAlgorithm<T, Edge<T>>(graph);

            if (scenario.DoComputation)
                algorithm.Compute(scenario.Root!);
            return algorithm;
        }
    }
}
