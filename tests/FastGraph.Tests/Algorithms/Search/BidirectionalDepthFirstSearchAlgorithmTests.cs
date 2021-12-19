#nullable enable

using JetBrains.Annotations;
using NUnit.Framework;
using FastGraph.Algorithms.Search;
using static FastGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace FastGraph.Tests.Algorithms.Search
{
    /// <summary>
    /// Tests for <see cref="BidirectionalDepthFirstSearchAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class BidirectionalDepthFirstSearchAlgorithmTests : RootedAlgorithmTestsBase
    {
        #region Test helpers

        private static void RunDFSAndCheck<TVertex, TEdge>(
            IBidirectionalGraph<TVertex, TEdge> graph,
            int maxDepth = int.MaxValue)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            var discoverTimes = new Dictionary<TVertex, int>();
            var finishTimes = new Dictionary<TVertex, int>();
            int time = 0;
            var dfs = new BidirectionalDepthFirstSearchAlgorithm<TVertex, TEdge>(graph)
            {
                MaxDepth = maxDepth
            };

            dfs.InitializeVertex += vertex =>
            {
                dfs.VerticesColors[vertex].Should().Be(GraphColor.White);
            };

            dfs.StartVertex += vertex =>
            {
                dfs.VerticesColors[vertex].Should().Be(GraphColor.White);
            };

            dfs.DiscoverVertex += vertex =>
            {
                dfs.VerticesColors[vertex].Should().Be(GraphColor.Gray);
                discoverTimes[vertex] = time++;
            };

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            dfs.ExamineEdge += edge =>
            {
                // Depending if the edge was taken from in or out edges
                // Here we cannot determine in which case we are
                (dfs.VerticesColors[edge.Source] == GraphColor.Gray
                 ||
                 dfs.VerticesColors[edge.Target] == GraphColor.Gray).Should().BeTrue();
            };

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            dfs.TreeEdge += edge =>
            {
                // Depending if the edge was taken from in or out edges
                // Here we cannot determine in which case we are
                (dfs.VerticesColors[edge.Source] == GraphColor.White
                 ||
                 dfs.VerticesColors[edge.Target] == GraphColor.White).Should().BeTrue();
            };

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            dfs.BackEdge += edge =>
            {
                // Depending if the edge was taken from in or out edges
                // Here we cannot determine in which case we are
                (dfs.VerticesColors[edge.Source] == GraphColor.Gray
                 ||
                 dfs.VerticesColors[edge.Target] == GraphColor.Gray).Should().BeTrue();
            };

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            dfs.ForwardOrCrossEdge += edge =>
            {
                // Depending if the edge was taken from in or out edges
                // Here we cannot determine in which case we are
                (dfs.VerticesColors[edge.Source] == GraphColor.Black
                 ||
                 dfs.VerticesColors[edge.Target] == GraphColor.Black).Should().BeTrue();
            };

            dfs.FinishVertex += vertex =>
            {
                dfs.VerticesColors[vertex].Should().Be(GraphColor.Black);
                finishTimes[vertex] = time++;
            };

            dfs.Compute();

            // Check
            // All vertices should be black
            foreach (TVertex vertex in graph.Vertices)
            {
                dfs.VerticesColors.ContainsKey(vertex).Should().BeTrue();
                dfs.VerticesColors[vertex].Should().Be(GraphColor.Black);
            }

            foreach (TVertex u in graph.Vertices)
            {
                foreach (TVertex v in graph.Vertices)
                {
                    if (!u.Equals(v))
                    {
                        (finishTimes[u] < discoverTimes[v]
                         || finishTimes[v] < discoverTimes[u]
                         || discoverTimes[v] < discoverTimes[u] && finishTimes[u] < finishTimes[v]
                         || discoverTimes[u] < discoverTimes[v] && finishTimes[v] < finishTimes[u]).Should().BeTrue();
                    }
                }
            }
        }

        #endregion

        [Test]
        public void Constructor()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            var algorithm = new BidirectionalDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
            AssertAlgorithmProperties(algorithm, graph);

            var verticesColors = new Dictionary<int, GraphColor>();
            algorithm = new BidirectionalDepthFirstSearchAlgorithm<int, Edge<int>>(graph, verticesColors);
            AssertAlgorithmProperties(algorithm, graph, verticesColors);

            algorithm = new BidirectionalDepthFirstSearchAlgorithm<int, Edge<int>>(default, graph, verticesColors);
            AssertAlgorithmProperties(algorithm, graph, verticesColors);

            algorithm.MaxDepth = 12;
            AssertAlgorithmProperties(algorithm, graph, verticesColors, 12);

            algorithm.ProcessAllComponents = true;
            AssertAlgorithmProperties(algorithm, graph, verticesColors, 12, true);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                BidirectionalDepthFirstSearchAlgorithm<TVertex, TEdge> algo,
                IBidirectionalGraph<TVertex, TEdge> g,
                IDictionary<TVertex, GraphColor>? vColors = default,
                int maxDepth = int.MaxValue,
                bool processAllComponents = false)
                where TVertex : notnull
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                if (vColors is null)
                    algo.VerticesColors.Should().BeEmpty();
                else
                    algo.VerticesColors.Should().BeSameAs(vColors);
                algo.MaxDepth.Should().Be(maxDepth);
                algo.ProcessAllComponents.Should().Be(processAllComponents);
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            var graph = new BidirectionalGraph<int, Edge<int>>();
            var verticesColors = new Dictionary<int, GraphColor>();

#pragma warning disable CS8625
            Invoking(() => new BidirectionalDepthFirstSearchAlgorithm<int, Edge<int>>(default)).Should().Throw<ArgumentNullException>();

            Invoking(() => new BidirectionalDepthFirstSearchAlgorithm<int, Edge<int>>(graph, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BidirectionalDepthFirstSearchAlgorithm<int, Edge<int>>(default, verticesColors)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BidirectionalDepthFirstSearchAlgorithm<int, Edge<int>>(default, default)).Should().Throw<ArgumentNullException>();

            Invoking(() => new BidirectionalDepthFirstSearchAlgorithm<int, Edge<int>>(default, default, verticesColors)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BidirectionalDepthFirstSearchAlgorithm<int, Edge<int>>(default, graph, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BidirectionalDepthFirstSearchAlgorithm<int, Edge<int>>(default, default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement

            Invoking(() => new BidirectionalDepthFirstSearchAlgorithm<int, Edge<int>>(graph).MaxDepth = -1).Should().Throw<ArgumentOutOfRangeException>();
        }

        #region Rooted algorithm

        [Test]
        public void TryGetRootVertex()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            var algorithm = new BidirectionalDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
            TryGetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            var algorithm = new BidirectionalDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
            SetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            var algorithm = new BidirectionalDepthFirstSearchAlgorithm<TestVertex, Edge<TestVertex>>(graph);
            SetRootVertex_Throws_Test(algorithm);
        }

        [Test]
        public void ClearRootVertex()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            var algorithm = new BidirectionalDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
            ClearRootVertex_Test(algorithm);
        }

        [Test]
        public void ComputeWithoutRoot_Throws()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            ComputeWithoutRoot_NoThrows_Test(
                graph,
                () => new BidirectionalDepthFirstSearchAlgorithm<int, Edge<int>>(graph));
        }

        [Test]
        public void ComputeWithRoot()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            graph.AddVertex(0);
            var algorithm = new BidirectionalDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
            ComputeWithRoot_Test(algorithm);
        }

        [Test]
        public void ComputeWithRoot_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            ComputeWithRoot_Throws_Test(
                () => new BidirectionalDepthFirstSearchAlgorithm<TestVertex, Edge<TestVertex>>(graph));
        }

        #endregion

        [Test]
        public void GetVertexColor()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            graph.AddVerticesAndEdge(new Edge<int>(1, 2));

            var algorithm = new BidirectionalDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
            // Algorithm not run
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Invoking(() => algorithm.GetVertexColor(1)).Should().Throw<VertexNotFoundException>();

            algorithm.Compute();

            algorithm.GetVertexColor(1).Should().Be(GraphColor.Black);
            algorithm.GetVertexColor(2).Should().Be(GraphColor.Black);
        }

        [Test]
        [Category(TestCategories.LongRunning)]
        public void DepthFirstSearch()
        {
            foreach (BidirectionalGraph<string, Edge<string>> graph in TestGraphFactory.GetBidirectionalGraphs_SlowTests())
            {
                RunDFSAndCheck(graph);
                RunDFSAndCheck(graph, 12);
            }
        }

        [TestCase(false)]
        [TestCase(true)]
        public void ProcessAllComponents(bool processAll)
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
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

            var algorithm = new BidirectionalDepthFirstSearchAlgorithm<int, Edge<int>>(graph)
            {
                ProcessAllComponents = processAll
            };
            algorithm.Compute(1);

            if (processAll)
            {
                algorithm.VerticesColors.Should().OnlyContain(pair => pair.Value == GraphColor.Black);
            }
            else
            {
                algorithm.VerticesColors.Should().OnlyContain(kvp =>
                    kvp.Key >= 1 && kvp.Key <= 5 && kvp.Value == GraphColor.Black
                    || kvp.Key >= 6 && kvp.Key <= 8 && kvp.Value == GraphColor.White);
            }
        }

        [Pure]
        public static BidirectionalDepthFirstSearchAlgorithm<T, Edge<T>> CreateAlgorithmAndMaybeDoComputation<T>(
            ContractScenario<T> scenario)
            where T : notnull
        {
            var graph = new BidirectionalGraph<T, Edge<T>>();
            graph.AddVerticesAndEdgeRange(scenario.EdgesInGraph.Select(e => new Edge<T>(e.Source, e.Target)));
            graph.AddVertexRange(scenario.SingleVerticesInGraph);

            var algorithm = new BidirectionalDepthFirstSearchAlgorithm<T, Edge<T>>(graph);

            if (scenario.DoComputation)
                algorithm.Compute(scenario.Root!);
            return algorithm;
        }
    }
}
