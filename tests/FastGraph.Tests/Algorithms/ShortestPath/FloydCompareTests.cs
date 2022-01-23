#nullable enable

using JetBrains.Annotations;
using NUnit.Framework;
using FastGraph.Algorithms.Observers;
using FastGraph.Algorithms.ShortestPath;

namespace FastGraph.Tests.Algorithms.ShortestPath
{
    /// <summary>
    /// Tests for comparing <see cref="FloydWarshallAllShortestPathAlgorithm{TVertex,TEdge}"/> and other shortest path finder algorithms.
    /// </summary>
    [TestFixture]
    [Category(TestCategories.CISkip)]
    internal sealed class FloydCompareTests : FloydWarshallTestsBase
    {
        #region Test helpers

        private static void CheckPath<TVertex, TEdge>(TVertex source, TVertex target, TEdge[] edges)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            edges[0].Source.Should().Be(source);
            for (int i = 0; i < edges.Length - 1; ++i)
            {
                edges[i + 1].Source.Should().Be(edges[i].Target);
            }

            edges[edges.Length - 1].Target.Should().Be(target);
        }

        private static void CompareAlgorithms<TVertex, TEdge, TGraph>(
            AdjacencyGraph<TVertex, TEdge> graph,
            [InstantHandle] Func<TEdge, double> getDistances,
            [InstantHandle] Func<AdjacencyGraph<TVertex, TEdge>, Func<TEdge, double>, ShortestPathAlgorithmBase<TVertex, TEdge, TGraph>> shortestPathAlgorithmFactory)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
            where TGraph : IVertexSet<TVertex>
        {
            // Compute all paths
            var algorithm = new FloydWarshallAllShortestPathAlgorithm<TVertex, TEdge>(graph, getDistances);
            algorithm.Compute();

            TVertex[] vertices = graph.Vertices.ToArray();
            foreach (TVertex source in vertices)
            {
                ShortestPathAlgorithmBase<TVertex, TEdge, TGraph> otherAlgorithm = shortestPathAlgorithmFactory(graph, getDistances);
                var predecessors = new VertexPredecessorRecorderObserver<TVertex, TEdge>();
                using (predecessors.Attach(otherAlgorithm))
                    otherAlgorithm.Compute(source);

                TryFunc<TVertex, IEnumerable<TEdge>> otherPaths = predecessors.TryGetPath;
                foreach (TVertex target in vertices)
                {
                    if (source.Equals(target))
                        continue;

                    bool pathExists = algorithm.TryGetPath(source, target, out IEnumerable<TEdge>? floydPath);
                    otherPaths(target, out IEnumerable<TEdge>? otherPath).Should().Be(pathExists);

                    if (pathExists)
                    {
                        TEdge[] floydEdges = floydPath!.ToArray();
                        CheckPath(source, target, floydEdges);

                        TEdge[] otherEdges = otherPath!.ToArray();
                        CheckPath(source, target, otherEdges);

                        // All distances are usually 1 in this test, so it should at least
                        // be the same number
                        if (otherEdges.Length != floydEdges.Length)
                        {
                            Assert.Fail("Path do not have the same length.");
                        }

                        // Check path length are the same
                        double floydLength = floydEdges.Sum(getDistances);
                        double otherLength = otherEdges.Sum(getDistances);
                        if (Math.Abs(floydLength - otherLength) > double.Epsilon)
                        {
                            Assert.Fail("Path do not have the same length.");
                        }
                    }
                }
            }
        }

        #endregion

        [Test]
        [Category(TestCategories.LongRunning)]
        public void FloydVsBellmannGraphML()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs_SlowTests())
                CompareAlgorithms(graph, _ => 1.0, (g, d) => new BellmanFordShortestPathAlgorithm<string, Edge<string>>(g, d));
        }

        [Test]
        public void FloydVsDijkstra()
        {
            var distances = new Dictionary<Edge<char>, double>();
            AdjacencyGraph<char, Edge<char>> graph = CreateGraph(distances);
            CompareAlgorithms(graph, e => distances[e], (g, d) => new DijkstraShortestPathAlgorithm<char, Edge<char>>(g, d));
        }

        [Test]
        [Category(TestCategories.LongRunning)]
        public void FloydVsDijkstraGraphML()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs_SlowTests())
                CompareAlgorithms(graph, _ => 1, (g, d) => new DijkstraShortestPathAlgorithm<string, Edge<string>>(g, d));
        }
    }
}
