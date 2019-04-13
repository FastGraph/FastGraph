using System;
using NUnit.Framework;
using QuikGraph.Tests;

namespace QuickGraph.Tests.Algorithms
{
    [TestFixture]
    internal class EulerianGraphTest : QuikGraphUnitTests
    {
        private UndirectedGraph<int, UndirectedEdge<int>> constructGraph(Tuple<int, int>[] vertices)
        {
            var g = new UndirectedGraph<int, UndirectedEdge<int>>();
            foreach (var pair in vertices)
            {
                g.AddVerticesAndEdge(new UndirectedEdge<int>(pair.Item1, pair.Item2));
            }
            return g;
        }

        [Test]
        public void IsEulerianOneComponentTrue()
        {
            var g = constructGraph(new Tuple<int, int>[] { new Tuple<int, int>(1, 2), new Tuple<int, int>(2, 3), new Tuple<int, int>(3, 1) });
            var gAlgo = new QuickGraph.Algorithms.IsEulerianGraphAlgorithm<int, UndirectedEdge<int>>(g);
            Assert.IsTrue(gAlgo.IsEulerian());
        }

        [Test]
        public void IsEulerianOneComponentFalse()
        {
            var g = constructGraph(new Tuple<int, int>[] { new Tuple<int, int>(1, 2),
                    new Tuple<int, int>(2, 3), new Tuple<int, int>(3, 4),
                    new Tuple<int, int>(4, 1), new Tuple<int, int>(1, 3)});
            var gAlgo = new QuickGraph.Algorithms.IsEulerianGraphAlgorithm<int, UndirectedEdge<int>>(g);
            Assert.IsFalse(gAlgo.IsEulerian());
        }

        [Test]
        public void IsEulerianManyComponentsTrue()
        {
            var g = constructGraph(new Tuple<int, int>[] { new Tuple<int, int>(1, 2), new Tuple<int, int>(2, 3), new Tuple<int, int>(3, 1) });
            g.AddVertex(4);
            g.AddVertex(5);

            var gAlgo = new QuickGraph.Algorithms.IsEulerianGraphAlgorithm<int, UndirectedEdge<int>>(g);
            Assert.IsTrue(gAlgo.IsEulerian());
        }

        [Test]
        public void IsEulerianManyComponentsFalse()
        {
            var g = constructGraph(new Tuple<int, int>[] { new Tuple<int, int>(1, 2),
                    new Tuple<int, int>(2, 3), new Tuple<int, int>(3, 1),
                    new Tuple<int, int>(4, 5), new Tuple<int, int>(5, 6),
                    new Tuple<int, int>(6, 4)});
            g.AddVertex(7);
            var gAlgo = new QuickGraph.Algorithms.IsEulerianGraphAlgorithm<int, UndirectedEdge<int>>(g);
            Assert.IsFalse(gAlgo.IsEulerian());
        }


        [Test]
        public void IsEulerianEmpty()
        {
            var g = constructGraph(new Tuple<int, int>[] { });
            var gAlgo = new QuickGraph.Algorithms.IsEulerianGraphAlgorithm<int, UndirectedEdge<int>>(g);
            Assert.IsFalse(gAlgo.IsEulerian());
        }

        [Test]
        public void IsEulerianOneVertex()
        {
            var g = constructGraph(new Tuple<int, int>[] { });
            g.AddVertex(420);
            var gAlgo = new QuickGraph.Algorithms.IsEulerianGraphAlgorithm<int, UndirectedEdge<int>>(g);
            Assert.IsTrue(gAlgo.IsEulerian());
        }

        [Test]
        public void IsEulerianOneVertexWithLoop()
        {
            var g = constructGraph(new Tuple<int, int>[] { new Tuple<int, int>(1, 1) });
            var gAlgo = new QuickGraph.Algorithms.IsEulerianGraphAlgorithm<int, UndirectedEdge<int>>(g);
            Assert.IsTrue(gAlgo.IsEulerian());
        }

        [Test]
        public void IsEulerianOneVertexWithTwoLoops()
        {
            var g = constructGraph(new Tuple<int, int>[] { new Tuple<int, int>(1, 1), new Tuple<int, int>(1, 1) });
            var gAlgo = new QuickGraph.Algorithms.IsEulerianGraphAlgorithm<int, UndirectedEdge<int>>(g);
            Assert.IsTrue(gAlgo.IsEulerian());
        }

        [Test]
        public void IsEulerianTwoVertices()
        {
            var g = constructGraph(new Tuple<int, int>[] { new Tuple<int, int>(1, 2), new Tuple<int, int>(2, 2) });
            var gAlgo = new QuickGraph.Algorithms.IsEulerianGraphAlgorithm<int, UndirectedEdge<int>>(g);
            Assert.IsFalse(gAlgo.IsEulerian());
        }

        [Test]
        public void IsEulerianTwoVerticesWithLoops()
        {
            var g = constructGraph(new Tuple<int, int>[] { new Tuple<int, int>(1, 1), new Tuple<int, int>(2, 2) });
            var gAlgo = new QuickGraph.Algorithms.IsEulerianGraphAlgorithm<int, UndirectedEdge<int>>(g);
            Assert.IsFalse(gAlgo.IsEulerian());
        }


        [Test]
        public void IsEulerianTwoVerticesTwoEdges()
        {
            var g = constructGraph(new Tuple<int, int>[] { new Tuple<int, int>(1, 2), new Tuple<int, int>(2, 1) });
            var gAlgo = new QuickGraph.Algorithms.IsEulerianGraphAlgorithm<int, UndirectedEdge<int>>(g);
            Assert.IsTrue(gAlgo.IsEulerian());
        }

        [Test]
        public void IsEulerianTwoVerticesOneEdge()
        {
            var g = constructGraph(new Tuple<int, int>[] { new Tuple<int, int>(1, 2) });
            var gAlgo = new QuickGraph.Algorithms.IsEulerianGraphAlgorithm<int, UndirectedEdge<int>>(g);
            Assert.IsFalse(gAlgo.IsEulerian());
        }
    }
}