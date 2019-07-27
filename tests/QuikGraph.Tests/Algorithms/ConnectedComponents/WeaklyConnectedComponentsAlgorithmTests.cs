using NUnit.Framework;
using QuikGraph.Serialization;
using QuikGraph.Tests;

namespace QuikGraph.Algorithms.ConnectedComponents
{
    [TestFixture]
    internal class WeaklyConnectedComponentsAlgorithmTests : QuikGraphUnitTests
    {
        [Test]
        public void WeaklyConnectedComponentsAll()
        {
            foreach (var g in TestGraphFactory.GetAdjacencyGraphs())
                this.Compute(g);
        }

        public void Compute<TVertex,TEdge>(IVertexListGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            var dfs = 
                new WeaklyConnectedComponentsAlgorithm<TVertex,TEdge>(g);
            dfs.Compute();
            if (g.VertexCount == 0)
            {
                Assert.IsTrue(dfs.ComponentCount == 0);
                return;
            }

            Assert.IsTrue(0 < dfs.ComponentCount);
            Assert.IsTrue(dfs.ComponentCount <= g.VertexCount);
            foreach(var kv in dfs.Components)
            {
                Assert.IsTrue(0 <= kv.Value);
                Assert.IsTrue(kv.Value < dfs.ComponentCount, "{0} < {1}", kv.Value, dfs.ComponentCount);
            }

            foreach(var vertex in g.Vertices)
                foreach (var edge in g.OutEdges(vertex))
                {
                    Assert.AreEqual(dfs.Components[edge.Source], dfs.Components[edge.Target]);
                }
        }
    }
}
