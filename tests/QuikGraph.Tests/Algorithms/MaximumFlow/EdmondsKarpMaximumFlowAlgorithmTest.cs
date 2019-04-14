using NUnit.Framework;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.MaximumFlow;
using QuikGraph.Serialization;
using QuikGraph.Tests;

namespace QuikGraph.Tests.Algorithms.MaximumFlow
{
    [TestFixture]
    internal class EdmondsKarpMaximumFlowAlgorithmTest : QuikGraphUnitTests
    {
        [Test]
        public void EdmondsKarpMaxFlowAll()
        {
            foreach (var g in TestGraphFactory.GetAdjacencyGraphs())
            {
                if (g.VertexCount > 0)
                    this.EdmondsKarpMaxFlow(g, (source, target) => new Edge<string>(source, target));
            }
        }


        public void EdmondsKarpMaxFlow<TVertex, TEdge>(IMutableVertexAndEdgeListGraph<TVertex, TEdge> g, 
            EdgeFactory<TVertex, TEdge> edgeFactory)
            where TEdge : IEdge<TVertex>
        {
            Assert.IsTrue(g.VertexCount > 0);

            foreach (var source in g.Vertices)
                foreach (var sink in g.Vertices)
                {
                    if (source.Equals(sink)) continue;

                    RunMaxFlowAlgorithm<TVertex, TEdge>(g, edgeFactory, source, sink);
                }
        }

        private static double RunMaxFlowAlgorithm<TVertex, TEdge>(IMutableVertexAndEdgeListGraph<TVertex, TEdge> g, EdgeFactory<TVertex, TEdge> edgeFactory, TVertex source, TVertex sink) where TEdge : IEdge<TVertex>
        {
            var reversedEdgeAugmentorAlgorithm = new ReversedEdgeAugmentorAlgorithm<TVertex, TEdge>(g, edgeFactory);
            reversedEdgeAugmentorAlgorithm.AddReversedEdges();

            TryFunc<TVertex, TEdge> flowPredecessors;
            var flow = AlgorithmExtensions.MaximumFlowEdmondsKarp<TVertex, TEdge>(
                g,
                e => 1,
                source, sink,
                out flowPredecessors,
                edgeFactory,
                reversedEdgeAugmentorAlgorithm
                );

            reversedEdgeAugmentorAlgorithm.RemoveReversedEdges();

            return flow;
        }

    }
}
