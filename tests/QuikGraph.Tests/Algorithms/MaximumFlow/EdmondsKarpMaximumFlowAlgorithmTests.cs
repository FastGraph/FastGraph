using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.MaximumFlow;

namespace QuikGraph.Tests.Algorithms.MaximumFlow
{
    /// <summary>
    /// Tests for <see cref="EdmondsKarpMaximumFlowAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class EdmondsKarpMaximumFlowAlgorithmTests
    {
        #region Helpers

        private static void EdmondsKarpMaxFlow<TVertex, TEdge>(
            [NotNull] IMutableVertexAndEdgeListGraph<TVertex, TEdge> graph,
            [NotNull] EdgeFactory<TVertex, TEdge> edgeFactory)
            where TEdge : IEdge<TVertex>
        {
            Assert.IsTrue(graph.VertexCount > 0);

            foreach (TVertex source in graph.Vertices)
            {
                foreach (TVertex sink in graph.Vertices)
                {
                    if (source.Equals(sink))
                        continue;

                    RunMaxFlowAlgorithmAndCheck(graph, edgeFactory, source, sink);
                    // TODO: Add Asserts
                }
            }
        }

        private static double RunMaxFlowAlgorithmAndCheck<TVertex, TEdge>(
            [NotNull] IMutableVertexAndEdgeListGraph<TVertex, TEdge> graph,
            [NotNull] EdgeFactory<TVertex, TEdge> edgeFactory,
            [NotNull] TVertex source,
            [NotNull] TVertex sink)
            where TEdge : IEdge<TVertex>
        {
            var reversedEdgeAugmentorAlgorithm = new ReversedEdgeAugmentorAlgorithm<TVertex, TEdge>(graph, edgeFactory);
            reversedEdgeAugmentorAlgorithm.AddReversedEdges();

            var flow = graph.MaximumFlow(
                edge => 1,
                source, sink,
                out _,
                edgeFactory,
                reversedEdgeAugmentorAlgorithm);

            reversedEdgeAugmentorAlgorithm.RemoveReversedEdges();

            return flow;
        }

        #endregion

        [Test]
        public void EdmondsKarpMaxFlowAll()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs())
            {
                if (graph.VertexCount > 0)
                    EdmondsKarpMaxFlow(graph, (source, target) => new Edge<string>(source, target));
            }
        }
    }
}