using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms.Observers;
using QuikGraph.Algorithms.ShortestPath;

namespace QuikGraph.Tests.Algorithms.ShortestPath
{
    /// <summary>
    /// Tests for <see cref="UndirectedDijkstraShortestPathAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class UndirectedDijkstraShortestPathAlgorithmTests
    {
        #region Helpers

        private static void RunUndirectedDijkstraAndCheck<TVertex, TEdge>([NotNull] IUndirectedGraph<TVertex, TEdge> graph, [NotNull] TVertex root)
            where TEdge : IEdge<TVertex>
        {
            var distances = new Dictionary<TEdge, double>();
            foreach (TEdge edge in graph.Edges)
                distances[edge] = graph.AdjacentDegree(edge.Source) + 1;

            var algorithm = new UndirectedDijkstraShortestPathAlgorithm<TVertex, TEdge>(graph, e => distances[e]);
            var predecessors = new UndirectedVertexPredecessorRecorderObserver<TVertex, TEdge>();
            using (predecessors.Attach(algorithm))
                algorithm.Compute(root);

            Verify(algorithm, predecessors);
        }

        private static void Verify<TVertex, TEdge>(
            [NotNull] UndirectedDijkstraShortestPathAlgorithm<TVertex, TEdge> algorithm,
            [NotNull] UndirectedVertexPredecessorRecorderObserver<TVertex, TEdge> predecessors)
            where TEdge : IEdge<TVertex>
        {
            // Verify the result
            foreach (TVertex vertex in algorithm.VisitedGraph.Vertices)
            {
                if (!predecessors.VerticesPredecessors.TryGetValue(vertex, out TEdge predecessor))
                    continue;
                if (predecessor.Source.Equals(vertex))
                    continue;
                bool found = algorithm.TryGetDistance(vertex, out double vd);
                Assert.AreEqual(found, algorithm.TryGetDistance(predecessor.Source, out double vp));
                if (found)
                    Assert.AreEqual(vd, vp + 1);
            }
        }

        #endregion

        [Test]
        public void UndirectedDijkstraAll()
        {
            foreach (UndirectedGraph<string, Edge<string>> graph in TestGraphFactory.GetUndirectedGraphs_TMP())
            {
                int cut = 0;
                foreach (var root in graph.Vertices)
                {
                    if (cut++ > 10)
                        break;
                    RunUndirectedDijkstraAndCheck(graph, root);
                }
            }
        }

        [Test]
        public void Repro42450()
        {
            var undirectedGraph = new UndirectedGraph<object, Edge<object>>(true);
            object v1 = "vertex1";
            object v2 = "vertex2";
            object v3 = "vertex3";
            var e1 = new Edge<object>(v1, v2);
            var e2 = new Edge<object>(v2, v3);
            var e3 = new Edge<object>(v3, v1);
            undirectedGraph.AddVertex(v1);
            undirectedGraph.AddVertex(v2);
            undirectedGraph.AddVertex(v3);
            undirectedGraph.AddEdge(e1);
            undirectedGraph.AddEdge(e2);
            undirectedGraph.AddEdge(e3);

            var algorithm = new UndirectedDijkstraShortestPathAlgorithm<object, Edge<object>>(
                undirectedGraph,
                edge => 1.0);
            var observer = new UndirectedVertexPredecessorRecorderObserver<object, Edge<object>>();
            using (observer.Attach(algorithm))
                algorithm.Compute(v1);

            Assert.IsTrue(observer.TryGetPath(v3, out _));
        }
    }
}
