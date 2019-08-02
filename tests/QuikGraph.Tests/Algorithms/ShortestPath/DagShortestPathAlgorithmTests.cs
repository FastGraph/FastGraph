using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.Observers;
using QuikGraph.Algorithms.ShortestPath;

namespace QuikGraph.Tests.Algorithms.ShortestPath
{
    /// <summary>
    /// Tests for <see cref="DagShortestPathAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class DagShortestPathAlgorithmTests
    {
        #region Helpers

        private static void Compute<TVertex, TEdge>([NotNull] IVertexListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            // Is this a dag?
            bool isDag = graph.IsDirectedAcyclicGraph();
            IDistanceRelaxer relaxer = DistanceRelaxers.ShortestDistance;
            var vertices = new List<TVertex>(graph.Vertices);
            foreach (TVertex root in vertices)
            {
                if (isDag)
                {
                    Search(graph, root, relaxer);
                }
                else
                {
                    Assert.Throws<NonAcyclicGraphException>(() => Search(graph, root, relaxer));
                }
            }
        }

        private static void ComputeCriticalPath<TVertex, TEdge>(
            [NotNull] IVertexListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            // Is this a dag?
            bool isDag = graph.IsDirectedAcyclicGraph();

            IDistanceRelaxer relaxer = DistanceRelaxers.CriticalDistance;
            var vertices = new List<TVertex>(graph.Vertices);
            foreach (TVertex root in vertices)
            {
                if (isDag)
                {
                    Search(graph, root, relaxer);
                }
                else
                {
                    Assert.Throws<NonAcyclicGraphException>(() => Search(graph, root, relaxer));
                }
            }
        }

        private static void Search<TVertex, TEdge>(
            [NotNull] IVertexListGraph<TVertex, TEdge> graph,
            [NotNull] TVertex root,
            [NotNull] IDistanceRelaxer relaxer)
            where TEdge : IEdge<TVertex>
        {
            var algorithm = new DagShortestPathAlgorithm<TVertex, TEdge>(
                graph,
                e => 1,
                relaxer);
            var predecessors = new VertexPredecessorRecorderObserver<TVertex, TEdge>();
            using (predecessors.Attach(algorithm))
                algorithm.Compute(root);

            Verify(algorithm, predecessors);
        }

        private static void Verify<TVertex, TEdge>(
            [NotNull] DagShortestPathAlgorithm<TVertex, TEdge> algorithm,
            [NotNull] VertexPredecessorRecorderObserver<TVertex, TEdge> predecessors)
            where TEdge : IEdge<TVertex>
        {
            // Verify the result
            foreach (TVertex vertex in algorithm.VisitedGraph.Vertices)
            {
                if (!predecessors.VertexPredecessors.TryGetValue(vertex, out TEdge predecessor))
                    continue;
                if (predecessor.Source.Equals(vertex))
                    continue;
                Assert.AreEqual(algorithm.Distances[vertex], algorithm.Distances[predecessor.Source] + 1);
            }
        }

        #endregion

        [Test]
        public void DagShortestPathAll()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs())
            {
                Compute(graph);
                ComputeCriticalPath(graph);
            }
        }
    }
}
