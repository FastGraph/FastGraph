#nullable enable

using JetBrains.Annotations;
using NUnit.Framework;
using FastGraph.Algorithms.Search;
using static FastGraph.Tests.Algorithms.AlgorithmTestHelpers;
using static FastGraph.Tests.GraphTestHelpers;

namespace FastGraph.Tests.Algorithms.Search
{
    /// <summary>
    /// Tests for <see cref="DepthFirstSearchAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class DepthFirstAlgorithmSearchTests : SearchAlgorithmTestsBase
    {
        #region Test helpers

        private static void RunDFSAndCheck<TVertex, TEdge>(
            IVertexListGraph<TVertex, TEdge> graph,
            int maxDepth = int.MaxValue)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            var parents = new Dictionary<TVertex, TVertex>();
            var discoverTimes = new Dictionary<TVertex, int>();
            var finishTimes = new Dictionary<TVertex, int>();
            int time = 0;
            var dfs = new DepthFirstSearchAlgorithm<TVertex, TEdge>(graph)
            {
                MaxDepth = maxDepth
            };

            dfs.InitializeVertex += vertex =>
            {
                Assert.AreEqual(GraphColor.White, dfs.VerticesColors[vertex]);
            };

            dfs.StartVertex += vertex =>
            {
                Assert.AreEqual(GraphColor.White, dfs.VerticesColors[vertex]);
                Assert.IsFalse(parents.ContainsKey(vertex));
                parents[vertex] = vertex;
            };

            dfs.DiscoverVertex += vertex =>
            {
                Assert.AreEqual(GraphColor.Gray, dfs.VerticesColors[vertex]);
                Assert.AreEqual(GraphColor.Gray, dfs.VerticesColors[parents[vertex]]);

                discoverTimes[vertex] = time++;
            };

            dfs.ExamineEdge += edge =>
            {
                Assert.AreEqual(GraphColor.Gray, dfs.VerticesColors[edge.Source]);
            };

            dfs.TreeEdge += edge =>
            {
                Assert.AreEqual(GraphColor.White, dfs.VerticesColors[edge.Target]);
                parents[edge.Target] = edge.Source;
            };

            dfs.BackEdge += edge =>
            {
                Assert.AreEqual(GraphColor.Gray, dfs.VerticesColors[edge.Target]);
            };

            dfs.ForwardOrCrossEdge += edge =>
            {
                Assert.AreEqual(GraphColor.Black, dfs.VerticesColors[edge.Target]);
            };

            dfs.FinishVertex += vertex =>
            {
                Assert.AreEqual(GraphColor.Black, dfs.VerticesColors[vertex]);
                finishTimes[vertex] = time++;
            };

            dfs.Compute();

            // Check
            // All vertices should be black
            foreach (TVertex vertex in graph.Vertices)
            {
                Assert.IsTrue(dfs.VerticesColors.ContainsKey(vertex));
                Assert.AreEqual(dfs.VerticesColors[vertex], GraphColor.Black);
            }

            foreach (TVertex u in graph.Vertices)
            {
                foreach (TVertex v in graph.Vertices)
                {
                    if (!u.Equals(v))
                    {
                        Assert.IsTrue(
                            finishTimes[u] < discoverTimes[v]
                            || finishTimes[v] < discoverTimes[u]
                            || (discoverTimes[v] < discoverTimes[u] && finishTimes[u] < finishTimes[v] && IsDescendant(parents, u, v))
                            || (discoverTimes[u] < discoverTimes[v] && finishTimes[v] < finishTimes[u] && IsDescendant(parents, v, u)));
                    }
                }
            }
        }

        #endregion

        [Test]
        public void Constructor()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new DepthFirstSearchAlgorithm<int, Edge<int>>(graph);
            AssertAlgorithmProperties(algorithm, graph);

            var verticesColors = new Dictionary<int, GraphColor>();
            algorithm = new DepthFirstSearchAlgorithm<int, Edge<int>>(graph, verticesColors);
            AssertAlgorithmProperties(algorithm, graph, verticesColors);

            algorithm = new DepthFirstSearchAlgorithm<int, Edge<int>>(default, graph);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new DepthFirstSearchAlgorithm<int, Edge<int>>(default, graph, verticesColors);
            AssertAlgorithmProperties(algorithm, graph, verticesColors);

            algorithm = new DepthFirstSearchAlgorithm<int, Edge<int>>(default, graph, verticesColors, edges => edges.Where(e => e != default));
            AssertAlgorithmProperties(algorithm, graph, verticesColors);

            algorithm.MaxDepth = 12;
            AssertAlgorithmProperties(algorithm, graph, verticesColors, 12);

            algorithm.ProcessAllComponents = true;
            AssertAlgorithmProperties(algorithm, graph, verticesColors, 12, true);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                DepthFirstSearchAlgorithm<TVertex, TEdge> algo,
                IVertexListGraph<TVertex, TEdge> g,
                IDictionary<TVertex, GraphColor>? vColors = default,
                int maxDepth = int.MaxValue,
                bool processAllComponents = false)
                where TVertex : notnull
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                if (vColors is null)
                    CollectionAssert.IsEmpty(algo.VerticesColors);
                else
                    Assert.AreSame(vColors, algo.VerticesColors);
                Assert.AreEqual(maxDepth, algo.MaxDepth);
                Assert.AreEqual(processAllComponents, algo.ProcessAllComponents);
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
            IEnumerable<Edge<int>> Filter(IEnumerable<Edge<int>> edges) => edges.Where(e => e != default);

            Assert.Throws<ArgumentNullException>(
                () => new DepthFirstSearchAlgorithm<int, Edge<int>>(default));

            Assert.Throws<ArgumentNullException>(
                () => new DepthFirstSearchAlgorithm<int, Edge<int>>(graph, default));
            Assert.Throws<ArgumentNullException>(
                () => new DepthFirstSearchAlgorithm<int, Edge<int>>(default, verticesColors));
            Assert.Throws<ArgumentNullException>(
                () => new DepthFirstSearchAlgorithm<int, Edge<int>>((IVertexListGraph<int, Edge<int>>?)default, default));

            Assert.Throws<ArgumentNullException>(
                () => new DepthFirstSearchAlgorithm<int, Edge<int>>(default, (IVertexListGraph<int, Edge<int>>?)default));

            Assert.Throws<ArgumentNullException>(
                () => new DepthFirstSearchAlgorithm<int, Edge<int>>(default, default, verticesColors));
            Assert.Throws<ArgumentNullException>(
                () => new DepthFirstSearchAlgorithm<int, Edge<int>>(default, graph, default));
            Assert.Throws<ArgumentNullException>(
                () => new DepthFirstSearchAlgorithm<int, Edge<int>>(default, default, default));

            Assert.Throws<ArgumentNullException>(
                () => new DepthFirstSearchAlgorithm<int, Edge<int>>(default, default, verticesColors, Filter));
            Assert.Throws<ArgumentNullException>(
                () => new DepthFirstSearchAlgorithm<int, Edge<int>>(default, graph, default, Filter));
            Assert.Throws<ArgumentNullException>(
                () => new DepthFirstSearchAlgorithm<int, Edge<int>>(default, graph, verticesColors, default));
            Assert.Throws<ArgumentNullException>(
                () => new DepthFirstSearchAlgorithm<int, Edge<int>>(default, graph, default, default));
            Assert.Throws<ArgumentNullException>(
                () => new DepthFirstSearchAlgorithm<int, Edge<int>>(default, default, default, Filter));
            Assert.Throws<ArgumentNullException>(
                () => new DepthFirstSearchAlgorithm<int, Edge<int>>(default, default, verticesColors, default));
            Assert.Throws<ArgumentNullException>(
                () => new DepthFirstSearchAlgorithm<int, Edge<int>>(default, default, default, default));
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement

            Assert.Throws<ArgumentOutOfRangeException>(() => new DepthFirstSearchAlgorithm<int, Edge<int>>(graph).MaxDepth = -1);
        }

        #region Rooted algorithm

        [Test]
        public void TryGetRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new DepthFirstSearchAlgorithm<int, Edge<int>>(graph);
            TryGetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new DepthFirstSearchAlgorithm<int, Edge<int>>(graph);
            SetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var algorithm = new DepthFirstSearchAlgorithm<TestVertex, Edge<TestVertex>>(graph);
            SetRootVertex_Throws_Test(algorithm);
        }

        [Test]
        public void ClearRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new DepthFirstSearchAlgorithm<int, Edge<int>>(graph);
            ClearRootVertex_Test(algorithm);
        }

        [Test]
        public void ComputeWithoutRoot_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            ComputeWithoutRoot_NoThrows_Test(
                graph,
                () => new DepthFirstSearchAlgorithm<int, Edge<int>>(graph));
        }

        [Test]
        public void ComputeWithRoot()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVertex(0);
            var algorithm = new DepthFirstSearchAlgorithm<int, Edge<int>>(graph);
            ComputeWithRoot_Test(algorithm);
        }

        [Test]
        public void ComputeWithRoot_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            ComputeWithRoot_Throws_Test(
                () => new DepthFirstSearchAlgorithm<TestVertex, Edge<TestVertex>>(graph));
        }

        #endregion

        [Test]
        public void GetVertexColor()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdge(new Edge<int>(1, 2));

            var algorithm = new DepthFirstSearchAlgorithm<int, Edge<int>>(graph);
            // Algorithm not run
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<VertexNotFoundException>(() => algorithm.GetVertexColor(1));

            algorithm.Compute();

            Assert.AreEqual(GraphColor.Black, algorithm.GetVertexColor(1));
            Assert.AreEqual(GraphColor.Black, algorithm.GetVertexColor(2));
        }

        [Test]
        public void DepthFirstSearch()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs_SlowTests())
            {
                RunDFSAndCheck(graph);
                RunDFSAndCheck(graph, 12);
            }
        }

        [TestCase(false)]
        [TestCase(true)]
        public void ProcessAllComponents(bool processAll)
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(1, 3),
                new Edge<int>(2, 1),
                new Edge<int>(2, 4),
                new Edge<int>(2, 5),

                new Edge<int>(6, 7),
                new Edge<int>(6, 8),
                new Edge<int>(8, 6)
            });

            var algorithm = new DepthFirstSearchAlgorithm<int, Edge<int>>(graph)
            {
                ProcessAllComponents = processAll
            };
            algorithm.Compute(1);

            if (processAll)
            {
                FastGraphAssert.TrueForAll(algorithm.VerticesColors, pair => pair.Value == GraphColor.Black);
            }
            else
            {
                FastGraphAssert.TrueForAll(
                    new[] { 1, 2, 3, 4, 5 },
                    vertex => algorithm.VerticesColors[vertex] == GraphColor.Black);
                FastGraphAssert.TrueForAll(
                    new[] { 6, 7, 8 },
                    vertex => algorithm.VerticesColors[vertex] == GraphColor.White);
            }
        }

        [Pure]
        public static DepthFirstSearchAlgorithm<T, Edge<T>> CreateAlgorithmAndMaybeDoComputation<T>(
            ContractScenario<T> scenario)
            where T : notnull
        {
            var graph = new AdjacencyGraph<T, Edge<T>>();
            graph.AddVerticesAndEdgeRange(scenario.EdgesInGraph.Select(e => new Edge<T>(e.Source, e.Target)));
            graph.AddVertexRange(scenario.SingleVerticesInGraph);

            var algorithm = new DepthFirstSearchAlgorithm<T, Edge<T>>(graph);

            if (scenario.DoComputation)
                algorithm.Compute(scenario.Root!);
            return algorithm;
        }
    }
}
