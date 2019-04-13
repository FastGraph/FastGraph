using System.Linq;
using Microsoft.Pex.Framework;
using NUnit.Framework;
using QuickGraph.Serialization;
using QuikGraph.Tests;

namespace QuickGraph.Algorithms.ConnectedComponents
{
    [TestFixture, PexClass]
    internal class ConnectedComponentsAlgorithmTest : QuikGraphUnitTests
    {
        [Test]
        [Category(TestCategories.LongRunning)]
        public void ConnectedComponentsAll()
        {
            foreach (var g in TestGraphFactory.GetUndirectedGraphs())
            {
                while (g.EdgeCount > 0)
                {
                    this.Compute(g);
                    g.RemoveEdge(Enumerable.First(g.Edges));
                }
            }
        }

        [PexMethod]
        public void Compute<TVertex, TEdge>([PexAssumeNotNull]IUndirectedGraph<TVertex, TEdge> g)
             where TEdge : IEdge<TVertex>
        {
            var dfs = new ConnectedComponentsAlgorithm<TVertex, TEdge>(g);
            dfs.Compute();
            if (g.VertexCount == 0)
            {
                Assert.IsTrue(dfs.ComponentCount == 0);
                return;
            }

            Assert.IsTrue(0 < dfs.ComponentCount);
            Assert.IsTrue(dfs.ComponentCount <= g.VertexCount);
            foreach (var kv in dfs.Components)
            {
                Assert.IsTrue(0 <= kv.Value);
                Assert.IsTrue(kv.Value < dfs.ComponentCount, "{0} < {1}", kv.Value, dfs.ComponentCount);
            }

            foreach (var vertex in g.Vertices)
                foreach (var edge in g.AdjacentEdges(vertex))
                {
                    Assert.AreEqual(dfs.Components[edge.Source], dfs.Components[edge.Target]);
                }
        }
    }
}
