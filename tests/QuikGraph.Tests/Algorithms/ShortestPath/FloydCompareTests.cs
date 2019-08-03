using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms.Observers;
using QuikGraph.Algorithms.ShortestPath;

namespace QuikGraph.Tests.Algorithms.ShortestPath
{
    /// <summary>
    /// Tests for comparing <see cref="FloydWarshallAllShortestPathAlgorithm{TVertex,TEdge}"/> and other shortest path finder algorithms.
    /// </summary>
    [TestFixture]
    internal class FloydCompareTests : FloydWarshallTestsBase
    {
#if !SUPPORTS_SYSTEM_DELEGATES
        // System.Core and NUnit both define this delegate that is conflicting
        // Defining it here allows to use it without conflict.
        public delegate TResult Func<in T1, in T2, out TResult>(T1 arg1, T2 arg2);
#endif

        #region Helpers

        private static void Compare<TVertex, TEdge, TGraph>(
            [NotNull] AdjacencyGraph<TVertex, TEdge> graph,
            [NotNull] Func<TEdge, double> distances,
            [NotNull, InstantHandle] Func<AdjacencyGraph<TVertex, TEdge>, Func<TEdge, double>, ShortestPathAlgorithmBase<TVertex, TEdge, TGraph>> shortestPathAlgorithmFactory)
            where TEdge : IEdge<TVertex>
            where TGraph : IVertexSet<TVertex>
        {
            // Compute all paths
            var algorithm = new FloydWarshallAllShortestPathAlgorithm<TVertex, TEdge>(graph, distances);
            algorithm.Compute();
            var vertices = graph.Vertices.ToArray();
            foreach (TVertex source in vertices)
            {
                var otherAlgorithm = shortestPathAlgorithmFactory(graph, distances);
                var predecessors = new VertexPredecessorRecorderObserver<TVertex, TEdge>();
                using (predecessors.Attach(otherAlgorithm))
                    otherAlgorithm.Compute(source);

                TryFunc<TVertex, IEnumerable<TEdge>> otherPaths = predecessors.TryGetPath;
                foreach (TVertex target in vertices)
                {
                    if (source.Equals(target))
                        continue;

                    bool pathExists = algorithm.TryGetPath(source, target, out IEnumerable<TEdge> floydPath);
                    Assert.AreEqual(pathExists, otherPaths(target, out IEnumerable<TEdge> otherPath));

                    if (pathExists)
                    {
                        TEdge[] floydEdges = floydPath.ToArray();
                        CheckPath(source, target, floydEdges);

                        TEdge[] otherEdges = otherPath.ToArray();
                        CheckPath(source, target, otherEdges);

                        // All distances are usually 1 in this test, so it should at least
                        // be the same number
                        if (otherEdges.Length != floydEdges.Length)
                        {
                            Assert.Fail("Path do not have the same length.");
                        }

                        // Check path length are the same
                        double floydLength = floydEdges.Sum(distances);
                        double otherLength = otherEdges.Sum(distances);
                        if (Math.Abs(floydLength - otherLength) > double.Epsilon)
                        {
                            Assert.Fail("Path do not have the same length.");
                        }
                    }
                }
            }
        }

        private static void CheckPath<TVertex, TEdge>([NotNull] TVertex source, [NotNull] TVertex target, [NotNull, ItemNotNull] TEdge[] edges) 
            where TEdge : IEdge<TVertex>
        {
            Assert.AreEqual(source, edges[0].Source);
            for (int i = 0; i < edges.Length - 1; ++i)
                Assert.AreEqual(edges[i].Target, edges[i + 1].Source);
            Assert.AreEqual(target, edges[edges.Length - 1].Target);
        }

        #endregion

        [Test]
        public void FloydVsBellmannGraphML()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs())
                Compare(graph, e => 1, (g, d) => new BellmanFordShortestPathAlgorithm<string, Edge<string>>(g, d));
        }

        [Test]
        public void FloydVsDijkstra()
        {
            var distances = new Dictionary<Edge<char>, double>();
            AdjacencyGraph<char, Edge<char>> graph = CreateGraph(distances);
            Compare(graph, e => distances[e], (g, d) => new DijkstraShortestPathAlgorithm<char, Edge<char>>(g, d));
        }

        [Test]
        public void FloydVsDijkstraGraphML()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs())
                Compare(graph, e => 1, (g, d) => new DijkstraShortestPathAlgorithm<string, Edge<string>>(g, d));
        }
    }
}
