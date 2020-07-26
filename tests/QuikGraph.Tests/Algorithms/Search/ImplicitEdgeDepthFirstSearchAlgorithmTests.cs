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
    /// Tests for <see cref="ImplicitEdgeDepthFirstSearchAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class ImplicitEdgeDepthFirstAlgorithmSearchTests : SearchAlgorithmTestsBase
    {
        #region Test helpers

        private static void RunImplicitEdgeDFSAndCheck<TVertex, TEdge>(
            [NotNull] IEdgeListAndIncidenceGraph<TVertex, TEdge> graph,
            [NotNull] TVertex sourceVertex,
            int maxDepth = int.MaxValue)
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
                Assert.IsFalse(parents.ContainsKey(edge));
                parents[edge] = edge;
                discoverTimes[edge] = time++;
            };

            dfs.DiscoverTreeEdge += (edge, targetEdge) =>
            {
                parents[targetEdge] = edge;

                Assert.AreEqual(GraphColor.Gray, dfs.EdgesColors[parents[targetEdge]]);

                discoverTimes[targetEdge] = time++;
            };

            dfs.TreeEdge += edge =>
            {
                Assert.AreEqual(GraphColor.Gray, dfs.EdgesColors[edge]);
            };

            dfs.BackEdge += edge =>
            {
                Assert.AreEqual(GraphColor.Gray, dfs.EdgesColors[edge]);
            };

            dfs.ForwardOrCrossEdge += edge =>
            {
                Assert.AreEqual(GraphColor.Black, dfs.EdgesColors[edge]);
            };

            dfs.FinishEdge += edge =>
            {
                Assert.AreEqual(GraphColor.Black, dfs.EdgesColors[edge]);
                finishTimes[edge] = time++;
            };

            dfs.Compute(sourceVertex);

            // Check
            if (maxDepth == int.MaxValue)
                Assert.AreEqual(discoverTimes.Count, finishTimes.Count);
            else
                Assert.GreaterOrEqual(discoverTimes.Count, finishTimes.Count);

            TEdge[] exploredEdges = finishTimes.Keys.ToArray();
            foreach (TEdge e1 in exploredEdges)
            {
                foreach (TEdge e2 in exploredEdges)
                {
                    if (!e1.Equals(e2))
                    {
                        Assert.IsTrue(
                            finishTimes[e1] < discoverTimes[e2]
                            || finishTimes[e2] < discoverTimes[e1]
                            || (discoverTimes[e2] < discoverTimes[e1] && finishTimes[e1] < finishTimes[e2] && IsDescendant(parents, e1, e2))
                            || (discoverTimes[e1] < discoverTimes[e2] && finishTimes[e2] < finishTimes[e1] && IsDescendant(parents, e2, e1)));
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

            algorithm = new ImplicitEdgeDepthFirstSearchAlgorithm<int, Edge<int>>(null, graph);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm.MaxDepth = 12;
            AssertAlgorithmProperties(algorithm, graph, 12);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                ImplicitEdgeDepthFirstSearchAlgorithm<TVertex, TEdge> algo,
                IIncidenceGraph<TVertex, TEdge> g,
                int maxDepth = int.MaxValue)
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                CollectionAssert.IsEmpty(algo.EdgesColors);
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
                () => new ImplicitEdgeDepthFirstSearchAlgorithm<int, Edge<int>>(null));

            Assert.Throws<ArgumentNullException>(
                () => new ImplicitEdgeDepthFirstSearchAlgorithm<int, Edge<int>>(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement

            Assert.Throws<ArgumentOutOfRangeException>(() => new ImplicitEdgeDepthFirstSearchAlgorithm<int, Edge<int>>(graph).MaxDepth = -1);
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

        [Test]
        [Category(TestCategories.LongRunning)]
        public void EdgeDepthFirstSearch()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs_SlowTests())
            {
                foreach (string vertex in graph.Vertices)
                {
                    RunImplicitEdgeDFSAndCheck(graph, vertex);
                    RunImplicitEdgeDFSAndCheck(graph, vertex, 12);
                }
            }
        }
    }
}