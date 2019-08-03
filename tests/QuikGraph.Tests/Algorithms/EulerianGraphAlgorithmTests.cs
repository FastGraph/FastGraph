using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms;

namespace QuikGraph.Tests.Algorithms
{
    /// <summary>
    /// Tests for <see cref="IsEulerianGraphAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class EulerianGraphAlgorithmTests
    {
        #region Helpers

        [Pure]
        [NotNull]
        private static UndirectedGraph<int, UndirectedEdge<int>> ConstructGraph([NotNull] IEnumerable<Vertices> vertices)
        {
            var graph = new UndirectedGraph<int, UndirectedEdge<int>>();
            foreach (Vertices pair in vertices)
            {
                graph.AddVerticesAndEdge(new UndirectedEdge<int>(pair.Source, pair.Target));
            }

            return graph;
        }

        #endregion

        [Test]
        public void IsEulerianOneComponentTrue()
        {
            var graph = ConstructGraph(new[]
            {
                new Vertices(1, 2),
                new Vertices(2, 3),
                new Vertices(3, 1)
            });

            var algorithm = new IsEulerianGraphAlgorithm<int, UndirectedEdge<int>>(graph);
            Assert.IsTrue(algorithm.IsEulerian());
        }

        [Test]
        public void IsEulerianOneComponentFalse()
        {
            UndirectedGraph<int, UndirectedEdge<int>> graph = ConstructGraph(new[] 
            {
                new Vertices(1, 2),
                new Vertices(2, 3),
                new Vertices(3, 4),
                new Vertices(4, 1),
                new Vertices(1, 3)
            });

            var algorithm = new IsEulerianGraphAlgorithm<int, UndirectedEdge<int>>(graph);
            Assert.IsFalse(algorithm.IsEulerian());
        }

        [Test]
        public void IsEulerianManyComponentsTrue()
        {
            UndirectedGraph<int, UndirectedEdge<int>> graph = ConstructGraph(new[]
            {
                new Vertices(1, 2),
                new Vertices(2, 3),
                new Vertices(3, 1)
            });

            graph.AddVertex(4);
            graph.AddVertex(5);

            var algorithm = new IsEulerianGraphAlgorithm<int, UndirectedEdge<int>>(graph);
            Assert.IsTrue(algorithm.IsEulerian());
        }

        [Test]
        public void IsEulerianManyComponentsFalse()
        {
            UndirectedGraph<int, UndirectedEdge<int>> graph = ConstructGraph(new[] 
            {
                new Vertices(1, 2),
                new Vertices(2, 3),
                new Vertices(3, 1),
                new Vertices(4, 5),
                new Vertices(5, 6),
                new Vertices(6, 4)
            });

            graph.AddVertex(7);

            var algorithm = new IsEulerianGraphAlgorithm<int, UndirectedEdge<int>>(graph);
            Assert.IsFalse(algorithm.IsEulerian());
        }


        [Test]
        public void IsEulerianEmpty()
        {
            UndirectedGraph<int, UndirectedEdge<int>> graph = ConstructGraph(Enumerable.Empty<Vertices>());

            var algorithm = new IsEulerianGraphAlgorithm<int, UndirectedEdge<int>>(graph);
            Assert.IsFalse(algorithm.IsEulerian());
        }

        [Test]
        public void IsEulerianOneVertex()
        {
            UndirectedGraph<int, UndirectedEdge<int>> graph = ConstructGraph(Enumerable.Empty<Vertices>());
            graph.AddVertex(420);

            var algorithm = new IsEulerianGraphAlgorithm<int, UndirectedEdge<int>>(graph);
            Assert.IsTrue(algorithm.IsEulerian());
        }

        [Test]
        public void IsEulerianOneVertexWithLoop()
        {
            UndirectedGraph<int, UndirectedEdge<int>> graph = ConstructGraph(new[]
            {
                new Vertices(1, 1)
            });

            var algorithm = new IsEulerianGraphAlgorithm<int, UndirectedEdge<int>>(graph);
            Assert.IsTrue(algorithm.IsEulerian());
        }

        [Test]
        public void IsEulerianOneVertexWithTwoLoops()
        {
            UndirectedGraph<int, UndirectedEdge<int>> graph = ConstructGraph(new[]
            {
                new Vertices(1, 1),
                new Vertices(1, 1)
            });

            var algorithm = new IsEulerianGraphAlgorithm<int, UndirectedEdge<int>>(graph);
            Assert.IsTrue(algorithm.IsEulerian());
        }

        [Test]
        public void IsEulerianTwoVertices()
        {
            UndirectedGraph<int, UndirectedEdge<int>> graph = ConstructGraph(new[]
            {
                new Vertices(1, 2),
                new Vertices(2, 2)
            });

            var algorithm = new IsEulerianGraphAlgorithm<int, UndirectedEdge<int>>(graph);
            Assert.IsFalse(algorithm.IsEulerian());
        }

        [Test]
        public void IsEulerianTwoVerticesWithLoops()
        {
            UndirectedGraph<int, UndirectedEdge<int>> graph = ConstructGraph(new[]
            {
                new Vertices(1, 1),
                new Vertices(2, 2)
            });

            var algorithm = new IsEulerianGraphAlgorithm<int, UndirectedEdge<int>>(graph);
            Assert.IsFalse(algorithm.IsEulerian());
        }

        [Test]
        public void IsEulerianTwoVerticesTwoEdges()
        {
            UndirectedGraph<int, UndirectedEdge<int>> graph = ConstructGraph(new[]
            {
                new Vertices(1, 2),
                new Vertices(2, 1)
            });

            var algorithm = new IsEulerianGraphAlgorithm<int, UndirectedEdge<int>>(graph);
            Assert.IsTrue(algorithm.IsEulerian());
        }

        [Test]
        public void IsEulerianTwoVerticesOneEdge()
        {
            UndirectedGraph<int, UndirectedEdge<int>> graph = ConstructGraph(new[]
            {
                new Vertices(1, 2)
            });

            var algorithm = new IsEulerianGraphAlgorithm<int, UndirectedEdge<int>>(graph);
            Assert.IsFalse(algorithm.IsEulerian());
        }
    }
}