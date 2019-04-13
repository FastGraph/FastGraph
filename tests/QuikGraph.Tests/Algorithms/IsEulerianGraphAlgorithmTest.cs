using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using QuikGraph.Tests;
using QuikGraph.Tests.Algorithms;

namespace QuickGraph.Tests.Algorithms
{
    [TestFixture]
    internal class EulerianGraphTest : QuikGraphUnitTests
    {
        private UndirectedGraph<int, UndirectedEdge<int>> constructGraph(IEnumerable<Vertices> vertices)
        {
            var g = new UndirectedGraph<int, UndirectedEdge<int>>();
            foreach (var pair in vertices)
            {
                g.AddVerticesAndEdge(new UndirectedEdge<int>(pair.Source, pair.Target));
            }
            return g;
        }

        [Test]
        public void IsEulerianOneComponentTrue()
        {
            var g = constructGraph(new[] { new Vertices(1, 2), new Vertices(2, 3), new Vertices(3, 1) });
            var gAlgo = new QuickGraph.Algorithms.IsEulerianGraphAlgorithm<int, UndirectedEdge<int>>(g);
            Assert.IsTrue(gAlgo.IsEulerian());
        }

        [Test]
        public void IsEulerianOneComponentFalse()
        {
            var g = constructGraph(new[] { new Vertices(1, 2),
                    new Vertices(2, 3), new Vertices(3, 4),
                    new Vertices(4, 1), new Vertices(1, 3) });
            var gAlgo = new QuickGraph.Algorithms.IsEulerianGraphAlgorithm<int, UndirectedEdge<int>>(g);
            Assert.IsFalse(gAlgo.IsEulerian());
        }

        [Test]
        public void IsEulerianManyComponentsTrue()
        {
            var g = constructGraph(new[] { new Vertices(1, 2), new Vertices(2, 3), new Vertices(3, 1) });
            g.AddVertex(4);
            g.AddVertex(5);

            var gAlgo = new QuickGraph.Algorithms.IsEulerianGraphAlgorithm<int, UndirectedEdge<int>>(g);
            Assert.IsTrue(gAlgo.IsEulerian());
        }

        [Test]
        public void IsEulerianManyComponentsFalse()
        {
            var g = constructGraph(new[] { new Vertices(1, 2),
                    new Vertices(2, 3), new Vertices(3, 1),
                    new Vertices(4, 5), new Vertices(5, 6),
                    new Vertices(6, 4) });
            g.AddVertex(7);
            var gAlgo = new QuickGraph.Algorithms.IsEulerianGraphAlgorithm<int, UndirectedEdge<int>>(g);
            Assert.IsFalse(gAlgo.IsEulerian());
        }


        [Test]
        public void IsEulerianEmpty()
        {
            var g = constructGraph(Enumerable.Empty<Vertices>());
            var gAlgo = new QuickGraph.Algorithms.IsEulerianGraphAlgorithm<int, UndirectedEdge<int>>(g);
            Assert.IsFalse(gAlgo.IsEulerian());
        }

        [Test]
        public void IsEulerianOneVertex()
        {
            var g = constructGraph(Enumerable.Empty<Vertices>());
            g.AddVertex(420);
            var gAlgo = new QuickGraph.Algorithms.IsEulerianGraphAlgorithm<int, UndirectedEdge<int>>(g);
            Assert.IsTrue(gAlgo.IsEulerian());
        }

        [Test]
        public void IsEulerianOneVertexWithLoop()
        {
            var g = constructGraph(new[] { new Vertices(1, 1) });
            var gAlgo = new QuickGraph.Algorithms.IsEulerianGraphAlgorithm<int, UndirectedEdge<int>>(g);
            Assert.IsTrue(gAlgo.IsEulerian());
        }

        [Test]
        public void IsEulerianOneVertexWithTwoLoops()
        {
            var g = constructGraph(new[] { new Vertices(1, 1), new Vertices(1, 1) });
            var gAlgo = new QuickGraph.Algorithms.IsEulerianGraphAlgorithm<int, UndirectedEdge<int>>(g);
            Assert.IsTrue(gAlgo.IsEulerian());
        }

        [Test]
        public void IsEulerianTwoVertices()
        {
            var g = constructGraph(new[] { new Vertices(1, 2), new Vertices(2, 2) });
            var gAlgo = new QuickGraph.Algorithms.IsEulerianGraphAlgorithm<int, UndirectedEdge<int>>(g);
            Assert.IsFalse(gAlgo.IsEulerian());
        }

        [Test]
        public void IsEulerianTwoVerticesWithLoops()
        {
            var g = constructGraph(new[] { new Vertices(1, 1), new Vertices(2, 2) });
            var gAlgo = new QuickGraph.Algorithms.IsEulerianGraphAlgorithm<int, UndirectedEdge<int>>(g);
            Assert.IsFalse(gAlgo.IsEulerian());
        }


        [Test]
        public void IsEulerianTwoVerticesTwoEdges()
        {
            var g = constructGraph(new[] { new Vertices(1, 2), new Vertices(2, 1) });
            var gAlgo = new QuickGraph.Algorithms.IsEulerianGraphAlgorithm<int, UndirectedEdge<int>>(g);
            Assert.IsTrue(gAlgo.IsEulerian());
        }

        [Test]
        public void IsEulerianTwoVerticesOneEdge()
        {
            var g = constructGraph(new[] { new Vertices(1, 2) });
            var gAlgo = new QuickGraph.Algorithms.IsEulerianGraphAlgorithm<int, UndirectedEdge<int>>(g);
            Assert.IsFalse(gAlgo.IsEulerian());
        }
    }
}