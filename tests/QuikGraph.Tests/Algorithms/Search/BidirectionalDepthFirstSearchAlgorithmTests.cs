using NUnit.Framework;
using QuikGraph.Serialization;
using QuikGraph.Tests;

namespace QuikGraph.Algorithms.Search
{
    [TestFixture]
    internal class BidirectionalDepthFirstSearchAlgorithmTests : QuikGraphUnitTests
    {
        [Test]
        public void ComputeAll()
        {
            foreach (var g in TestGraphFactory.GetBidirectionalGraphs())
                this.Compute(g);
        }

        public void Compute<TVertex,TEdge>(IBidirectionalGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            var dfs = new BidirectionalDepthFirstSearchAlgorithm<TVertex, TEdge>(g);
            dfs.Compute();

            // let's make sure
            foreach (var v in g.Vertices)
            {
                Assert.IsTrue(dfs.VerticesColors.ContainsKey(v));
                Assert.AreEqual(dfs.VerticesColors[v], GraphColor.Black);
            }
        }
    }
}
