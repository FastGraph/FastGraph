using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms.ConnectedComponents;

namespace QuikGraph.Tests.Algorithms.ConnectedComponents
{
    /// <summary>
    /// Tests for <see cref="ConnectedComponentsAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class ConnectedComponentsAlgorithmTests
    {
        #region Helpers

        private static void Compute<TVertex, TEdge>([NotNull] IUndirectedGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            var dfs = new ConnectedComponentsAlgorithm<TVertex, TEdge>(graph);
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
                foreach (TEdge edge in graph.AdjacentEdges(vertex))
                {
                    Assert.AreEqual(dfs.Components[edge.Source], dfs.Components[edge.Target]);
                }
            }
        }

        #endregion

        [Test]
        [Category(TestCategories.LongRunning)]
        public void ConnectedComponentsAll()
        {
            foreach (UndirectedGraph<string, Edge<string>> graph in TestGraphFactory.GetUndirectedGraphs())
            {
                while (graph.EdgeCount > 0)
                {
                    Compute(graph);
                    graph.RemoveEdge(graph.Edges.First());
                }
            }
        }
    }
}