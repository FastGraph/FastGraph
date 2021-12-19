#nullable enable

using NUnit.Framework;
using FastGraph.Algorithms.Search;
using static FastGraph.Tests.Algorithms.AlgorithmTestHelpers;
using static FastGraph.Tests.GraphTestHelpers;

namespace FastGraph.Tests.Algorithms.Search
{
    /// <summary>
    /// Tests for <see cref="ImplicitDepthFirstSearchAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class ImplicitDepthFirstAlgorithmSearchTests : SearchAlgorithmTestsBase
    {
        #region Test helpers

        private static void RunImplicitDFSAndCheck<TVertex, TEdge>(
            IIncidenceGraph<TVertex, TEdge> graph,
            TVertex sourceVertex,
            int maxDepth = int.MaxValue)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            var parents = new Dictionary<TVertex, TVertex>();
            var discoverTimes = new Dictionary<TVertex, int>();
            var finishTimes = new Dictionary<TVertex, int>();
            int time = 0;
            var dfs = new ImplicitDepthFirstSearchAlgorithm<TVertex, TEdge>(graph)
            {
                MaxDepth = maxDepth
            };

            dfs.StartVertex += vertex =>
            {
                parents.ContainsKey(vertex).Should().BeFalse();
                parents[vertex] = vertex;
            };

            dfs.DiscoverVertex += vertex =>
            {
                dfs.VerticesColors[vertex].Should().Be(GraphColor.Gray);
                dfs.VerticesColors[parents[vertex]].Should().Be(GraphColor.Gray);

                discoverTimes[vertex] = time++;
            };

            dfs.ExamineEdge += edge =>
            {
                dfs.VerticesColors[edge.Source].Should().Be(GraphColor.Gray);
            };

            dfs.TreeEdge += edge =>
            {
                parents[edge.Target] = edge.Source;
            };

            dfs.BackEdge += edge =>
            {
                dfs.VerticesColors[edge.Target].Should().Be(GraphColor.Gray);
            };

            dfs.ForwardOrCrossEdge += edge =>
            {
                dfs.VerticesColors[edge.Target].Should().Be(GraphColor.Black);
            };

            dfs.FinishVertex += vertex =>
            {
                dfs.VerticesColors[vertex].Should().Be(GraphColor.Black);
                finishTimes[vertex] = time++;
            };

            dfs.Compute(sourceVertex);

            // Check
            if (maxDepth == int.MaxValue)
                finishTimes.Count.Should().Be(discoverTimes.Count);
            else
                discoverTimes.Count.Should().BeGreaterThanOrEqualTo(finishTimes.Count);

            TVertex[] exploredVertices = finishTimes.Keys.ToArray();
            foreach (TVertex u in exploredVertices)
            {
                foreach (TVertex v in exploredVertices)
                {
                    if (!u.Equals(v))
                    {
                        (finishTimes[u] < discoverTimes[v]
                         || finishTimes[v] < discoverTimes[u]
                         || discoverTimes[v] < discoverTimes[u] && finishTimes[u] < finishTimes[v] && IsDescendant(parents, u, v)
                         || discoverTimes[u] < discoverTimes[v] && finishTimes[v] < finishTimes[u] && IsDescendant(parents, v, u)).Should().BeTrue();
                    }
                }
            }
        }

        #endregion

        [Test]
        public void Constructor()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new ImplicitDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new ImplicitDepthFirstSearchAlgorithm<int, Edge<int>>(default, graph);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm.MaxDepth = 12;
            AssertAlgorithmProperties(algorithm, graph, 12);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                ImplicitDepthFirstSearchAlgorithm<TVertex, TEdge> algo,
                IIncidenceGraph<TVertex, TEdge> g,
                int maxDepth = int.MaxValue)
                where TVertex : notnull
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                algo.VerticesColors.Should().BeEmpty();
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
            Invoking(() => new ImplicitDepthFirstSearchAlgorithm<int, Edge<int>>(default)).Should().Throw<ArgumentNullException>();

            Invoking(() => new ImplicitDepthFirstSearchAlgorithm<int, Edge<int>>(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement

            Invoking(() => new ImplicitDepthFirstSearchAlgorithm<int, Edge<int>>(graph).MaxDepth = -1).Should().Throw<ArgumentOutOfRangeException>();
        }

        #region Rooted algorithm

        [Test]
        public void TryGetRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new ImplicitDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
            TryGetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new ImplicitDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
            SetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var algorithm = new ImplicitDepthFirstSearchAlgorithm<TestVertex, Edge<TestVertex>>(graph);
            SetRootVertex_Throws_Test(algorithm);
        }

        [Test]
        public void ClearRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new ImplicitDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
            ClearRootVertex_Test(algorithm);
        }

        [Test]
        public void ComputeWithoutRoot_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            ComputeWithoutRoot_Throws_Test(
                () => new ImplicitDepthFirstSearchAlgorithm<int, Edge<int>>(graph));
        }

        [Test]
        public void ComputeWithRoot()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVertex(0);
            var algorithm = new ImplicitDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
            ComputeWithRoot_Test(algorithm);
        }

        [Test]
        public void ComputeWithRoot_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            ComputeWithRoot_Throws_Test(
                () => new ImplicitDepthFirstSearchAlgorithm<TestVertex, Edge<TestVertex>>(graph));
        }

        #endregion

        [Test]
        [Category(TestCategories.LongRunning)]
        public void ImplicitDepthFirstSearch()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs_SlowTests(10))
            {
                foreach (string vertex in graph.Vertices)
                {
                    RunImplicitDFSAndCheck(graph, vertex);
                    RunImplicitDFSAndCheck(graph, vertex, 12);
                }
            }
        }
    }
}
