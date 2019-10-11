using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms.ConnectedComponents;

namespace QuikGraph.Tests.Algorithms.ConnectedComponents
{
    /// <summary>
    /// Tests for <see cref="WeaklyConnectedComponentsAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class WeaklyConnectedComponentsAlgorithmTests
    {
        #region Helpers

        public void Compute<TVertex, TEdge>([NotNull] IVertexListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            var dfs = new WeaklyConnectedComponentsAlgorithm<TVertex, TEdge>(graph);
            dfs.Compute();
            if (graph.VertexCount == 0)
            {
                Assert.IsTrue(dfs.ComponentCount == 0);
                return;
            }

            Assert.IsTrue(0 < dfs.ComponentCount);
            Assert.IsTrue(dfs.ComponentCount <= graph.VertexCount);
            foreach (KeyValuePair<TVertex, int> pair in dfs.Components)
            {
                Assert.IsTrue(0 <= pair.Value);
                Assert.IsTrue(pair.Value < dfs.ComponentCount, $"{pair.Value} < {dfs.ComponentCount}");
            }

            foreach (TVertex vertex in graph.Vertices)
            {
                foreach (TEdge edge in graph.OutEdges(vertex))
                {
                    Assert.AreEqual(dfs.Components[edge.Source], dfs.Components[edge.Target]);
                }
            }
        }

        #endregion

        [Test]
        public void WeaklyConnectedComponentsAll()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs_TMP())
                Compute(graph);
        }
    }
}