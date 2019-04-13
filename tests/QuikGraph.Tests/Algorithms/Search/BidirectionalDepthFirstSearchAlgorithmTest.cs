using Microsoft.Pex.Framework;
using NUnit.Framework;
using QuickGraph.Serialization;
using QuikGraph.Tests;

namespace QuickGraph.Algorithms.Search
{
    [TestFixture]
    internal class BidirectionalDepthFirstSearchAlgorithmTest : QuikGraphUnitTests
    {
        [Test]
        public void ComputeAll()
        {
            foreach (var g in TestGraphFactory.GetBidirectionalGraphs())
                this.Compute(g);
        }

        [PexMethod]
        public void Compute<TVertex,TEdge>(IBidirectionalGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            var dfs = new BidirectionalDepthFirstSearchAlgorithm<TVertex, TEdge>(g);
            dfs.Compute();

            // let's make sure
            foreach (var v in g.Vertices)
            {
                Assert.IsTrue(dfs.VertexColors.ContainsKey(v));
                Assert.AreEqual(dfs.VertexColors[v], GraphColor.Black);
            }
        }
    }
}
