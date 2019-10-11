using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms.Search;

namespace QuikGraph.Tests.Algorithms.Search
{
    /// <summary>
    /// Tests for <see cref="BidirectionalDepthFirstSearchAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class BidirectionalDepthFirstSearchAlgorithmTests
    {
        #region Helpers

        private static void Compute<TVertex, TEdge>([NotNull] IBidirectionalGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            var dfs = new BidirectionalDepthFirstSearchAlgorithm<TVertex, TEdge>(graph);
            dfs.Compute();

            foreach (TVertex vertex in graph.Vertices)
            {
                Assert.IsTrue(dfs.VerticesColors.ContainsKey(vertex));
                Assert.AreEqual(dfs.VerticesColors[vertex], GraphColor.Black);
            }
        }

        #endregion

        [Test]
        public void ComputeAll()
        {
            foreach (BidirectionalGraph<string, Edge<string>> graph in TestGraphFactory.GetBidirectionalGraphs_TMP())
                Compute(graph);
        }
    }
}
