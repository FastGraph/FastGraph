#nullable enable

using NUnit.Framework;
using FastGraph.Algorithms.Search;
using static FastGraph.Tests.Algorithms.AlgorithmTestHelpers;
using static FastGraph.Tests.GraphTestHelpers;

namespace FastGraph.Tests.Algorithms.Search
{
    /// <summary>
    /// Tests for <see cref="EdgeDepthFirstSearchAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class EdgeDepthFirstAlgorithmSearchTests : SearchAlgorithmTestsBase
    {
        #region Test helpers

        private static void RunEdgeDFSAndCheck<TVertex, TEdge>(
            IEdgeListAndIncidenceGraph<TVertex, TEdge> graph,
            int maxDepth = int.MaxValue)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            var parents = new Dictionary<TEdge, TEdge>();
            var discoverTimes = new Dictionary<TEdge, int>();
            var finishTimes = new Dictionary<TEdge, int>();
            int time = 0;
            var dfs = new EdgeDepthFirstSearchAlgorithm<TVertex, TEdge>(graph)
            {
                MaxDepth = maxDepth
            };

            dfs.InitializeEdge += edge =>
            {
                dfs.EdgesColors[edge].Should().Be(GraphColor.White);
            };

            dfs.StartEdge += edge =>
            {
                dfs.EdgesColors[edge].Should().Be(GraphColor.White);
                parents.ContainsKey(edge).Should().BeFalse();
                parents[edge] = edge;
                discoverTimes[edge] = time++;
            };

            dfs.DiscoverTreeEdge += (edge, targetEdge) =>
            {
                parents[targetEdge] = edge;

                dfs.EdgesColors[targetEdge].Should().Be(GraphColor.White);
                dfs.EdgesColors[parents[targetEdge]].Should().Be(GraphColor.Gray);

                discoverTimes[targetEdge] = time++;
            };

            dfs.TreeEdge += edge =>
            {
                dfs.EdgesColors[edge].Should().Be(GraphColor.Gray);
            };

            dfs.BackEdge += edge =>
            {
                dfs.EdgesColors[edge].Should().Be(GraphColor.Gray);
            };

            dfs.ForwardOrCrossEdge += edge =>
            {
                dfs.EdgesColors[edge].Should().Be(GraphColor.Black);
            };

            dfs.FinishEdge += edge =>
            {
                dfs.EdgesColors[edge].Should().Be(GraphColor.Black);
                finishTimes[edge] = time++;
            };

            dfs.Compute();

            // Check
            // All vertices should be black
            foreach (TEdge edge in graph.Edges)
            {
                dfs.EdgesColors.ContainsKey(edge).Should().BeTrue();
                dfs.EdgesColors[edge].Should().Be(GraphColor.Black);
            }

            foreach (TEdge e1 in graph.Edges)
            {
                foreach (TEdge e2 in graph.Edges)
                {
                    if (!e1.Equals(e2))
                    {
                        (finishTimes[e1] < discoverTimes[e2]
                         || finishTimes[e2] < discoverTimes[e1]
                         || discoverTimes[e2] < discoverTimes[e1] && finishTimes[e1] < finishTimes[e2] && IsDescendant(parents, e1, e2)
                         || discoverTimes[e1] < discoverTimes[e2] && finishTimes[e2] < finishTimes[e1] && IsDescendant(parents, e2, e1)).Should().BeTrue();
                    }
                }
            }
        }

        #endregion

        [Test]
        public void Constructor()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new EdgeDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
            AssertAlgorithmProperties(algorithm, graph);

            var edgesColors = new Dictionary<Edge<int>, GraphColor>();
            algorithm = new EdgeDepthFirstSearchAlgorithm<int, Edge<int>>(graph, edgesColors);
            AssertAlgorithmProperties(algorithm, graph, edgesColors);

            algorithm = new EdgeDepthFirstSearchAlgorithm<int, Edge<int>>(default, graph, edgesColors);
            AssertAlgorithmProperties(algorithm, graph, edgesColors);

            algorithm.MaxDepth = 12;
            AssertAlgorithmProperties(algorithm, graph, edgesColors, 12);

            algorithm.ProcessAllComponents = true;
            AssertAlgorithmProperties(algorithm, graph, edgesColors, 12, true);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                EdgeDepthFirstSearchAlgorithm<TVertex, TEdge> algo,
                IEdgeListAndIncidenceGraph<TVertex, TEdge> g,
                IDictionary<TEdge, GraphColor>? vColors = default,
                int maxDepth = int.MaxValue,
                bool processAllComponents = false)
                where TVertex : notnull
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                if (vColors is null)
                    algo.EdgesColors.Should().BeEmpty();
                else
                    algo.EdgesColors.Should().BeSameAs(vColors);
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
#pragma warning disable CS8625
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var edgesColors = new Dictionary<Edge<int>, GraphColor>();

            Invoking(() => new EdgeDepthFirstSearchAlgorithm<int, Edge<int>>(default)).Should().Throw<ArgumentNullException>();

            Invoking(() => new EdgeDepthFirstSearchAlgorithm<int, Edge<int>>(graph, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EdgeDepthFirstSearchAlgorithm<int, Edge<int>>(default, edgesColors)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EdgeDepthFirstSearchAlgorithm<int, Edge<int>>(default, default)).Should().Throw<ArgumentNullException>();

            Invoking(() => new EdgeDepthFirstSearchAlgorithm<int, Edge<int>>(default, default, edgesColors)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EdgeDepthFirstSearchAlgorithm<int, Edge<int>>(default, graph, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EdgeDepthFirstSearchAlgorithm<int, Edge<int>>(default, default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement

            Invoking(() => new EdgeDepthFirstSearchAlgorithm<int, Edge<int>>(graph).MaxDepth = -1).Should().Throw<ArgumentOutOfRangeException>();
        }

        #region Rooted algorithm

        [Test]
        public void TryGetRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new EdgeDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
            TryGetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new EdgeDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
            SetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var algorithm = new EdgeDepthFirstSearchAlgorithm<TestVertex, Edge<TestVertex>>(graph);
            SetRootVertex_Throws_Test(algorithm);
        }

        [Test]
        public void ClearRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new EdgeDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
            ClearRootVertex_Test(algorithm);
        }

        [Test]
        public void ComputeWithoutRoot_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            ComputeWithoutRoot_NoThrows_Test(
                graph,
                () => new EdgeDepthFirstSearchAlgorithm<int, Edge<int>>(graph));
        }

        [Test]
        public void ComputeWithRoot()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVertex(0);
            var algorithm = new EdgeDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
            ComputeWithRoot_Test(algorithm);
        }

        [Test]
        public void ComputeWithRoot_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            ComputeWithRoot_Throws_Test(
                () => new EdgeDepthFirstSearchAlgorithm<TestVertex, Edge<TestVertex>>(graph));
        }

        #endregion

        [TestCaseSource(nameof(AdjacencyGraphs_SlowTests))]
        [Category(TestCategories.LongRunning)]
        public void EdgeDepthFirstSearch(TestGraphInstance<AdjacencyGraph<string, Edge<string>>, string> testGraph)
        {
            RunEdgeDFSAndCheck(testGraph.Instance);
            RunEdgeDFSAndCheck(testGraph.Instance, 12);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void ProcessAllComponents(bool processAll)
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge21 = new Edge<int>(2, 1);
            var edge24 = new Edge<int>(2, 4);
            var edge25 = new Edge<int>(2, 5);

            var edge67 = new Edge<int>(6, 7);
            var edge68 = new Edge<int>(6, 8);
            var edge86 = new Edge<int>(8, 6);

            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge13, edge21, edge24, edge25,

                edge67, edge68, edge86
            });

            var algorithm = new EdgeDepthFirstSearchAlgorithm<int, Edge<int>>(graph)
            {
                ProcessAllComponents = processAll
            };
            algorithm.Compute(1);

            if (processAll)
            {
                algorithm.EdgesColors.Should().OnlyContain(pair => pair.Value == GraphColor.Black);
            }
            else
            {
                new[] { edge12, edge13, edge24, edge25 }.Should().OnlyContain(edge => algorithm.EdgesColors[edge] == GraphColor.Black);
                new[] { edge67, edge68, edge86 }.Should().OnlyContain(edge => algorithm.EdgesColors[edge] == GraphColor.White);
            }
        }

        private static readonly IEnumerable<TestCaseData> AdjacencyGraphs_SlowTests =
            TestGraphFactory
                .SampleAdjacencyGraphs()
                .Select(t => new TestCaseData(t) { TestName = t.DescribeForTestCase() })
                .Memoize();
    }
}
