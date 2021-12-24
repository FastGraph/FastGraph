#nullable enable

using NUnit.Framework;
using FastGraph.Algorithms.Search;
using static FastGraph.Tests.Algorithms.AlgorithmTestHelpers;
using static FastGraph.Tests.GraphTestHelpers;

namespace FastGraph.Tests.Algorithms.Search
{
    /// <summary>
    /// Tests for <see cref="ImplicitEdgeDepthFirstSearchAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class ImplicitEdgeDepthFirstAlgorithmSearchTests : SearchAlgorithmTestsBase
    {
        #region Test helpers

        private static void RunImplicitEdgeDFSAndCheck<TVertex, TEdge>(
            IEdgeListAndIncidenceGraph<TVertex, TEdge> graph,
            TVertex sourceVertex,
            int maxDepth = int.MaxValue)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            var parents = new Dictionary<TEdge, TEdge>();
            var discoverTimes = new Dictionary<TEdge, int>();
            var finishTimes = new Dictionary<TEdge, int>();
            int time = 0;
            var dfs = new ImplicitEdgeDepthFirstSearchAlgorithm<TVertex, TEdge>(graph)
            {
                MaxDepth = maxDepth
            };

            dfs.StartEdge += edge =>
            {
                parents.ContainsKey(edge).Should().BeFalse();
                parents[edge] = edge;
                discoverTimes[edge] = time++;
            };

            dfs.DiscoverTreeEdge += (edge, targetEdge) =>
            {
                parents[targetEdge] = edge;

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

            dfs.Compute(sourceVertex);

            // Check
            if (maxDepth == int.MaxValue)
                finishTimes.Count.Should().Be(discoverTimes.Count);
            else
                discoverTimes.Count.Should().BeGreaterThanOrEqualTo(finishTimes.Count);

            TEdge[] exploredEdges = finishTimes.Keys.ToArray();
            foreach (TEdge e1 in exploredEdges)
            {
                foreach (TEdge e2 in exploredEdges)
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
            var algorithm = new ImplicitEdgeDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new ImplicitEdgeDepthFirstSearchAlgorithm<int, Edge<int>>(default, graph);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm.MaxDepth = 12;
            AssertAlgorithmProperties(algorithm, graph, 12);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                ImplicitEdgeDepthFirstSearchAlgorithm<TVertex, TEdge> algo,
                IIncidenceGraph<TVertex, TEdge> g,
                int maxDepth = int.MaxValue)
                where TVertex : notnull
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                algo.EdgesColors.Should().BeEmpty();
                algo.MaxDepth.Should().Be(maxDepth);
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            var graph = new AdjacencyGraph<int, Edge<int>>();

#pragma warning disable CS8625
            Invoking(() => new ImplicitEdgeDepthFirstSearchAlgorithm<int, Edge<int>>(default)).Should().Throw<ArgumentNullException>();

            Invoking(() => new ImplicitEdgeDepthFirstSearchAlgorithm<int, Edge<int>>(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement

            Invoking(() => new ImplicitEdgeDepthFirstSearchAlgorithm<int, Edge<int>>(graph).MaxDepth = -1).Should().Throw<ArgumentOutOfRangeException>();
        }

        #region Rooted algorithm

        [Test]
        public void TryGetRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new ImplicitEdgeDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
            TryGetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new ImplicitEdgeDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
            SetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var algorithm = new ImplicitEdgeDepthFirstSearchAlgorithm<TestVertex, Edge<TestVertex>>(graph);
            SetRootVertex_Throws_Test(algorithm);
        }

        [Test]
        public void ClearRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new ImplicitEdgeDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
            ClearRootVertex_Test(algorithm);
        }

        [Test]
        public void ComputeWithoutRoot_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            ComputeWithoutRoot_Throws_Test(
                () => new ImplicitEdgeDepthFirstSearchAlgorithm<int, Edge<int>>(graph));
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
                () => new ImplicitEdgeDepthFirstSearchAlgorithm<TestVertex, Edge<TestVertex>>(graph));
        }

        #endregion

        [TestCaseSource(nameof(AdjacencyGraphs_SlowTests_AllVertices))]
        [Category(TestCategories.LongRunning)]
        public void EdgeDepthFirstSearch(TestGraphInstanceWithSelectedVertex<AdjacencyGraph<string, Edge<string>>, string> testGraph)
        {
            var graph = testGraph.Instance;
            var vertex = testGraph.SelectedVertex;
            RunImplicitEdgeDFSAndCheck(graph, vertex);
            RunImplicitEdgeDFSAndCheck(graph, vertex, 12);
        }

        private static readonly IEnumerable<TestCaseData> AdjacencyGraphs_SlowTests_AllVertices =
            TestGraphFactory
                .SampleAdjacencyGraphs()
                .SelectMany(t => t.Select())
                .Select(t => new TestCaseData(t) { TestName = t.DescribeForTestCase() })
                .Memoize();
    }
}
