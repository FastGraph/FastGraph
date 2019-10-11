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
    internal class EdgeChainTests
    {
        #region Helpers

        private static void Generate<TVertex, TEdge>([NotNull] IVertexListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            foreach (TVertex vertex in graph.Vertices)
            {
                var walker = new RandomWalkAlgorithm<TVertex, TEdge>(graph);
                var vis = new EdgeRecorderObserver<TVertex, TEdge>();
                using (vis.Attach(walker))
                    walker.Generate(vertex);
            }
        }

        #endregion

        [Test]
        public void GenerateAll()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs_TMP())
                Generate(graph);
        }
    }
}
