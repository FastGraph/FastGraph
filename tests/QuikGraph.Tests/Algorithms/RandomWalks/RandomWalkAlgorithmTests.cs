using NUnit.Framework;
using QuikGraph.Algorithms.Observers;
using QuikGraph.Serialization;
using QuikGraph.Tests;

namespace QuikGraph.Algorithms.RandomWalks
{
    [TestFixture]
    internal class RandomWalkAlgorithmTests : QuikGraphUnitTests
    {
        [Test]
        public void RoundRobinAll()
        {
            foreach (var g in TestGraphFactory.GetAdjacencyGraphs())
                this.RoundRobinTest(g);
        }

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
