using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms.Observers;
using QuikGraph.Algorithms.RandomWalks;

namespace QuikGraph.Tests.Algorithms.RandomWalks
{
    /// <summary>
    /// Tests for <see cref="RandomWalkAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class RandomWalkAlgorithmTests
    {
        #region Helpers

        private static void NormalizedEdgeChainTest<TVertex, TEdge>([NotNull] IVertexListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            if (graph.VertexCount == 0)
                return;

            foreach (TVertex root in graph.Vertices)
            {
                var walker = new RandomWalkAlgorithm<TVertex, TEdge>(graph)
                {
                    EdgeChain = new NormalizedMarkovEdgeChain<TVertex, TEdge>()
                };

                walker.Generate(root);
            }
        }

        private static void NormalizedEdgeChainTestWithVisitor<TVertex, TEdge>([NotNull] IVertexListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            if (graph.VertexCount == 0)
                return;

            foreach (TVertex root in graph.Vertices)
            {
                var walker = new RandomWalkAlgorithm<TVertex, TEdge>(graph)
                {
                    EdgeChain = new NormalizedMarkovEdgeChain<TVertex, TEdge>()
                };

                var vis = new EdgeRecorderObserver<TVertex, TEdge>();
                using (vis.Attach(walker))
                    walker.Generate(root);
            }
        }

        #endregion

        [Test]
        public void RoundRobinAll()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs())
                NormalizedEdgeChainTest(graph);
        }
    }
}
