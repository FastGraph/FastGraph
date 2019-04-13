using NUnit.Framework;
using Microsoft.Pex.Framework;
using QuickGraph.Algorithms.Observers;
using QuickGraph.Serialization;
using QuikGraph.Tests;

namespace QuickGraph.Algorithms.RandomWalks
{
    [TestFixture]
    internal class EdgeChainTest : QuikGraphUnitTests
    {
        [Test]
        public void GenerateAll()
        {
            foreach (var g in TestGraphFactory.GetAdjacencyGraphs())
                this.Generate(g);
        }

        [PexMethod]
        public void Generate<TVertex, TEdge>(IVertexListGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {

            foreach (var v in g.Vertices)
            {
                var walker = new RandomWalkAlgorithm<TVertex, TEdge>(g);
                var vis = new EdgeRecorderObserver<TVertex, TEdge>();
                using(vis.Attach(walker))
                    walker.Generate(v);
            }
        }
    }
}
