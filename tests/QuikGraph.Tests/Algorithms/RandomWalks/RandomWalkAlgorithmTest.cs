using Microsoft.Pex.Framework;
using NUnit.Framework;
using QuickGraph.Algorithms.Observers;
using QuickGraph.Serialization;
using QuikGraph.Tests;

namespace QuickGraph.Algorithms.RandomWalks
{
    [TestFixture]
    internal class RandomWalkAlgorithmTest : QuikGraphUnitTests
    {
        [Test]
        public void RoundRobinAll()
        {
            foreach (var g in TestGraphFactory.GetAdjacencyGraphs())
                this.RoundRobinTest(g);
        }

        [PexMethod]
        public void RoundRobinTest<TVertex, TEdge>(IVertexListGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            if (g.VertexCount == 0)
                return;

            foreach (var root in g.Vertices)
            {
                var walker =
                    new RandomWalkAlgorithm<TVertex, TEdge>(g);
                walker.EdgeChain = new NormalizedMarkovEdgeChain<TVertex, TEdge>();
                walker.Generate(root);
            }
        }

        [PexMethod]
        public void RoundRobinTestWithVisitor<TVertex, TEdge>(IVertexListGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            if (g.VertexCount == 0)
                return;

            foreach (var root in g.Vertices)
            {
                var walker =
                    new RandomWalkAlgorithm<TVertex, TEdge>(g);
                walker.EdgeChain = new NormalizedMarkovEdgeChain<TVertex, TEdge>();

                var vis = new EdgeRecorderObserver<TVertex, TEdge>();
                using(vis.Attach(walker))
                    walker.Generate(root);
            }
        }

    }
}
