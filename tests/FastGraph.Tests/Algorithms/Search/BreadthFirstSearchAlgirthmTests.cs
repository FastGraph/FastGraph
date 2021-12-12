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
    /// Tests for <see cref="BreadthFirstSearchAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class BreadthFirstAlgorithmSearchTests : RootedAlgorithmTestsBase
    {
        #region Test helpers

        public void RunBFSAndCheck<TVertex, TEdge>(
            IVertexAndEdgeListGraph<TVertex, TEdge> graph,
            TVertex sourceVertex)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            var parents = new Dictionary<TVertex, TVertex>();
            var distances = new Dictionary<TVertex, int>();
            TVertex? currentVertex = default;
            int currentDistance = 0;
            var algorithm = new BreadthFirstSearchAlgorithm<TVertex, TEdge>(graph);

            algorithm.InitializeVertex += vertex =>
            {
                Assert.AreEqual(GraphColor.White, algorithm.VerticesColors[vertex]);
            };

            algorithm.StartVertex += vertex =>
            {
                Assert.AreEqual(GraphColor.White, algorithm.VerticesColors[vertex]);
            };

            algorithm.DiscoverVertex += vertex =>
            {
                Assert.AreEqual(GraphColor.Gray, algorithm.VerticesColors[vertex]);
                if (vertex.Equals(sourceVertex))
                {
                    currentVertex = sourceVertex;
                }
                else
                {
                    Assert.IsNotNull(currentVertex);
                    Assert.AreEqual(parents[vertex], currentVertex);
                    // ReSharper disable once AccessToModifiedClosure
                    Assert.AreEqual(distances[vertex], currentDistance + 1);
                    Assert.AreEqual(distances[vertex], distances[parents[vertex]] + 1);
                }
            };

            algorithm.ExamineEdge += edge =>
            {
                Assert.AreEqual(edge.Source, currentVertex);
            };

            algorithm.ExamineVertex += vertex =>
            {
                TVertex u = vertex;
                currentVertex = u;
                // Ensure that the distances monotonically increase.
                // ReSharper disable AccessToModifiedClosure
                Assert.IsTrue(distances[u] == currentDistance || distances[u] == currentDistance + 1);

                if (distances[u] == currentDistance + 1) // New level
                    ++currentDistance;
                // ReSharper restore AccessToModifiedClosure
            };

            algorithm.TreeEdge += edge =>
            {
                TVertex u = edge.Source;
                TVertex v = edge.Target;

                Assert.AreEqual(GraphColor.White, algorithm.VerticesColors[v]);
                Assert.AreEqual(distances[u], currentDistance);
                parents[v] = u;
                distances[v] = distances[u] + 1;
            };

            algorithm.NonTreeEdge += edge =>
            {
                TVertex u = edge.Source;
                TVertex v = edge.Target;

                Assert.IsFalse(algorithm.VerticesColors[v] == GraphColor.White);

                if (algorithm.VisitedGraph.IsDirected)
                {
                    // Cross or back edge
                    Assert.IsTrue(distances[v] <= distances[u] + 1);
                }
                else
                {
                    // Cross edge (or going backwards on a tree edge)
                    Assert.IsTrue(
                        distances[v] == distances[u]
                        || distances[v] == distances[u] + 1
                        || distances[v] == distances[u] - 1);
                }
            };

            algorithm.GrayTarget += edge =>
            {
                Assert.AreEqual(GraphColor.Gray, algorithm.VerticesColors[edge.Target]);
            };

            algorithm.BlackTarget += edge =>
            {
                Assert.AreEqual(GraphColor.Black, algorithm.VerticesColors[edge.Target]);

                foreach (TEdge outEdge in algorithm.VisitedGraph.OutEdges(edge.Target))
                    Assert.IsFalse(algorithm.VerticesColors[outEdge.Target] == GraphColor.White);
            };

            algorithm.FinishVertex += vertex =>
            {
                Assert.AreEqual(GraphColor.Black, algorithm.VerticesColors[vertex]);
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

            var recorder = new VertexPredecessorRecorderObserver<TVertex, TEdge>();
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
                            Assert.AreNotEqual(sourceVertex, edge.Source);
                            Assert.AreNotEqual(sourceVertex, edge.Target);
                        }
                    }
                }
            }

            // The shortest path to a child should be one longer than
            // shortest path to the parent.
            foreach (TVertex vertex in graph.Vertices)
            {
                if (!parents[vertex].Equals(vertex)) // Not the root of the BFS tree
                    Assert.AreEqual(distances[vertex], distances[parents[vertex]] + 1);
            }
        }

        #endregion

        [Test]
        public void Constructor()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new BreadthFirstSearchAlgorithm<int, Edge<int>>(graph);
            AssertAlgorithmProperties(algorithm, graph);

            var verticesColors = new Dictionary<int, GraphColor>();
            var queue = new BinaryQueue<int, double>(_ => 1.0);
            algorithm = new BreadthFirstSearchAlgorithm<int, Edge<int>>(graph, queue, verticesColors);
            AssertAlgorithmProperties(algorithm, graph, verticesColors);

            algorithm = new BreadthFirstSearchAlgorithm<int, Edge<int>>(default, graph, queue, verticesColors);
            AssertAlgorithmProperties(algorithm, graph, verticesColors);

            algorithm = new BreadthFirstSearchAlgorithm<int, Edge<int>>(default, graph, queue, verticesColors, edges => edges.Where(e => e != default));
            AssertAlgorithmProperties(algorithm, graph, verticesColors);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                BreadthFirstSearchAlgorithm<TVertex, TEdge> algo,
                IVertexListGraph<TVertex, TEdge> g,
                IDictionary<TVertex, GraphColor>? vColors = default)
                where TVertex : notnull
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                if (vColors is null)
                    CollectionAssert.IsEmpty(algo.VerticesColors);
                else
                    Assert.AreSame(vColors, algo.VerticesColors);
                Assert.IsNotNull(algo.OutEdgesFilter);
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var verticesColors = new Dictionary<int, GraphColor>();
            var queue = new BinaryQueue<int, double>(_ => 1.0);
            IEnumerable<Edge<int>> Filter(IEnumerable<Edge<int>> edges) => edges.Where(e => e != default);

            Assert.Throws<ArgumentNullException>(
                () => new BreadthFirstSearchAlgorithm<int, Edge<int>>(default));

            Assert.Throws<ArgumentNullException>(
                () => new BreadthFirstSearchAlgorithm<int, Edge<int>>(default, queue, verticesColors));
            Assert.Throws<ArgumentNullException>(
                () => new BreadthFirstSearchAlgorithm<int, Edge<int>>(graph, default, verticesColors));
            Assert.Throws<ArgumentNullException>(
                () => new BreadthFirstSearchAlgorithm<int, Edge<int>>(graph, queue, default));
            Assert.Throws<ArgumentNullException>(
                () => new BreadthFirstSearchAlgorithm<int, Edge<int>>(default, default, verticesColors));
            Assert.Throws<ArgumentNullException>(
                () => new BreadthFirstSearchAlgorithm<int, Edge<int>>(default, queue, default));
            Assert.Throws<ArgumentNullException>(
                () => new BreadthFirstSearchAlgorithm<int, Edge<int>>(graph, default, default));
            Assert.Throws<ArgumentNullException>(
                () => new BreadthFirstSearchAlgorithm<int, Edge<int>>(default, default, default));

            Assert.Throws<ArgumentNullException>(
                () => new BreadthFirstSearchAlgorithm<int, Edge<int>>(default, default, queue, verticesColors));
            Assert.Throws<ArgumentNullException>(
                () => new BreadthFirstSearchAlgorithm<int, Edge<int>>(default, graph, default, verticesColors));
            Assert.Throws<ArgumentNullException>(
                () => new BreadthFirstSearchAlgorithm<int, Edge<int>>(default, graph, queue, default));
            Assert.Throws<ArgumentNullException>(
                () => new BreadthFirstSearchAlgorithm<int, Edge<int>>(default, default, default, verticesColors));
            Assert.Throws<ArgumentNullException>(
                () => new BreadthFirstSearchAlgorithm<int, Edge<int>>(default, default, queue, default));
            Assert.Throws<ArgumentNullException>(
                () => new BreadthFirstSearchAlgorithm<int, Edge<int>>(default, graph, default, default));
            Assert.Throws<ArgumentNullException>(
                () => new BreadthFirstSearchAlgorithm<int, Edge<int>>(default, default, default, default));

            Assert.Throws<ArgumentNullException>(
                () => new BreadthFirstSearchAlgorithm<int, Edge<int>>(default, default, queue, verticesColors, Filter));
            Assert.Throws<ArgumentNullException>(
                () => new BreadthFirstSearchAlgorithm<int, Edge<int>>(default, graph, default, verticesColors, Filter));
            Assert.Throws<ArgumentNullException>(
                () => new BreadthFirstSearchAlgorithm<int, Edge<int>>(default, graph, queue, default, Filter));
            Assert.Throws<ArgumentNullException>(
                () => new BreadthFirstSearchAlgorithm<int, Edge<int>>(default, graph, queue, verticesColors, default));
            Assert.Throws<ArgumentNullException>(
                () => new BreadthFirstSearchAlgorithm<int, Edge<int>>(default, default, default, verticesColors, Filter));
            Assert.Throws<ArgumentNullException>(
                () => new BreadthFirstSearchAlgorithm<int, Edge<int>>(default, default, queue, default, Filter));
            Assert.Throws<ArgumentNullException>(
                () => new BreadthFirstSearchAlgorithm<int, Edge<int>>(default, default, queue, verticesColors, default));
            Assert.Throws<ArgumentNullException>(
                () => new BreadthFirstSearchAlgorithm<int, Edge<int>>(default, graph, default, default, Filter));
            Assert.Throws<ArgumentNullException>(
                () => new BreadthFirstSearchAlgorithm<int, Edge<int>>(default, graph, default, verticesColors, default));
            Assert.Throws<ArgumentNullException>(
                () => new BreadthFirstSearchAlgorithm<int, Edge<int>>(default, graph, queue, default, default));
            Assert.Throws<ArgumentNullException>(
                () => new BreadthFirstSearchAlgorithm<int, Edge<int>>(default, graph, default, default, default));
            Assert.Throws<ArgumentNullException>(
                () => new BreadthFirstSearchAlgorithm<int, Edge<int>>(default, default, queue, default, default));
            Assert.Throws<ArgumentNullException>(
                () => new BreadthFirstSearchAlgorithm<int, Edge<int>>(default, default, default, verticesColors, default));
            Assert.Throws<ArgumentNullException>(
                () => new BreadthFirstSearchAlgorithm<int, Edge<int>>(default, default, default, default, Filter));
            Assert.Throws<ArgumentNullException>(
                () => new BreadthFirstSearchAlgorithm<int, Edge<int>>(default, default, default, default, default));
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        #region Rooted algorithm

        [Test]
        public void TryGetRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new BreadthFirstSearchAlgorithm<int, Edge<int>>(graph);
            TryGetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new BreadthFirstSearchAlgorithm<int, Edge<int>>(graph);
            SetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var algorithm = new BreadthFirstSearchAlgorithm<TestVertex, Edge<TestVertex>>(graph);
            SetRootVertex_Throws_Test(algorithm);
        }

        [Test]
        public void ClearRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new BreadthFirstSearchAlgorithm<int, Edge<int>>(graph);
            ClearRootVertex_Test(algorithm);
        }

        [Test]
        public void ComputeWithoutRoot_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            ComputeWithoutRoot_NoThrows_Test(
                graph,
                () => new BreadthFirstSearchAlgorithm<int, Edge<int>>(graph));
        }

        [Test]
        public void ComputeWithRoot()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVertex(0);
            var algorithm = new BreadthFirstSearchAlgorithm<int, Edge<int>>(graph);
            ComputeWithRoot_Test(algorithm);
        }

        [Test]
        public void ComputeWithRoot_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            ComputeWithRoot_Throws_Test(
                () => new BreadthFirstSearchAlgorithm<TestVertex, Edge<TestVertex>>(graph));
        }

        #endregion

        [Test]
        public void GetVertexColor()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdge(new Edge<int>(1, 2));

            var algorithm = new BreadthFirstSearchAlgorithm<int, Edge<int>>(graph);
            // Algorithm not run
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<VertexNotFoundException>(() => algorithm.GetVertexColor(1));

            algorithm.Compute();

            Assert.AreEqual(GraphColor.Black, algorithm.GetVertexColor(1));
            Assert.AreEqual(GraphColor.Black, algorithm.GetVertexColor(2));
        }

        [Test]
        [Category(TestCategories.LongRunning)]
        public void BreadthFirstSearch()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs_SlowTests())
            {
                foreach (string vertex in graph.Vertices)
                    RunBFSAndCheck(graph, vertex);
            }
        }

        [Pure]
        public static BreadthFirstSearchAlgorithm<T, Edge<T>> CreateAlgorithmAndMaybeDoComputation<T>(
            ContractScenario<T> scenario)
            where T : notnull
        {
            var graph = new AdjacencyGraph<T, Edge<T>>();
            graph.AddVerticesAndEdgeRange(scenario.EdgesInGraph.Select(e => new Edge<T>(e.Source, e.Target)));
            graph.AddVertexRange(scenario.SingleVerticesInGraph);

            var algorithm = new BreadthFirstSearchAlgorithm<T, Edge<T>>(graph);

            if (scenario.DoComputation)
                algorithm.Compute(scenario.Root!);
            return algorithm;
        }
    }
}
