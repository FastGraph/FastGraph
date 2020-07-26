using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms.Search;
using static QuikGraph.Tests.Algorithms.AlgorithmTestHelpers;
using static QuikGraph.Tests.GraphTestHelpers;

namespace QuikGraph.Tests.Algorithms.Search
{
    /// <summary>
    /// Tests for <see cref="ImplicitDepthFirstSearchAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class ImplicitDepthFirstAlgorithmSearchTests : SearchAlgorithmTestsBase
    {
        #region Test helpers

        private static void RunImplicitDFSAndCheck<TVertex, TEdge>(
            [NotNull] IIncidenceGraph<TVertex, TEdge> graph,
            [NotNull] TVertex sourceVertex,
            int maxDepth = int.MaxValue)
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

            dfs.Compute(sourceVertex);

            // Check
            if (maxDepth == int.MaxValue)
                Assert.AreEqual(discoverTimes.Count, finishTimes.Count);
            else
                Assert.GreaterOrEqual(discoverTimes.Count, finishTimes.Count);

            TVertex[] exploredVertices = finishTimes.Keys.ToArray();
            foreach (TVertex u in exploredVertices)
            {
                foreach (TVertex v in exploredVertices)
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
            var algorithm = new ImplicitDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new ImplicitDepthFirstSearchAlgorithm<int, Edge<int>>(null, graph);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm.MaxDepth = 12;
            AssertAlgorithmProperties(algorithm, graph, 12);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                ImplicitDepthFirstSearchAlgorithm<TVertex, TEdge> algo,
                IIncidenceGraph<TVertex, TEdge> g,
                int maxDepth = int.MaxValue)
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                CollectionAssert.IsEmpty(algo.VerticesColors);
                Assert.AreEqual(maxDepth, algo.MaxDepth);
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            var graph = new AdjacencyGraph<int, Edge<int>>();

            Assert.Throws<ArgumentNullException>(
                () => new ImplicitDepthFirstSearchAlgorithm<int, Edge<int>>(null));

            Assert.Throws<ArgumentNullException>(
                () => new ImplicitDepthFirstSearchAlgorithm<int, Edge<int>>(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement

            Assert.Throws<ArgumentOutOfRangeException>(() => new ImplicitDepthFirstSearchAlgorithm<int, Edge<int>>(graph).MaxDepth = -1);
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