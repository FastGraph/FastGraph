using System;
using System.Linq;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms;
using static QuikGraph.Tests.TestHelpers;

namespace QuikGraph.Tests.Algorithms
{
    /// <summary>
    /// Tests for <see cref="IsEulerianGraphAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class EulerianGraphAlgorithmTests
    {
        #region Test helpers

        private static void AssertIsEulerian(
            bool expectedEulerian,
            [NotNull] IUndirectedGraph<int, UndirectedEdge<int>> graph)
        {
            var algorithm = new IsEulerianGraphAlgorithm<int, UndirectedEdge<int>>(graph);
            Assert.AreEqual(expectedEulerian, algorithm.IsEulerian());
            Assert.AreEqual(expectedEulerian, IsEulerianGraphAlgorithm.IsEulerian(graph));
        }

        #endregion

        [Test]
        public void IsEulerianEmpty()
        {
            UndirectedGraph<int, UndirectedEdge<int>> graph = CreateUndirectedGraph(Enumerable.Empty<Vertices>());
            AssertIsEulerian(false, graph);
        }

        [Test]
        public void IsEulerianOneVertex()
        {
            UndirectedGraph<int, UndirectedEdge<int>> graph = CreateUndirectedGraph(Enumerable.Empty<Vertices>());
            graph.AddVertex(42);

            AssertIsEulerian(true, graph);
        }

        [Test]
        public void IsEulerianOneComponent()
        {
            // Eulerian
            UndirectedGraph<int, UndirectedEdge<int>> graph = CreateUndirectedGraph(new[]
            {
                new Vertices(1, 2),
                new Vertices(2, 3),
                new Vertices(1, 3)
            });

            AssertIsEulerian(true, graph);

            // Not Eulerian
            graph = CreateUndirectedGraph(new[]
            {
                new Vertices(1, 2),
                new Vertices(2, 3),
                new Vertices(3, 4),
                new Vertices(1, 4),
                new Vertices(1, 3)
            });

            AssertIsEulerian(false, graph);
        }

        [Test]
        public void IsEulerianManyComponents()
        {
            // Eulerian
            UndirectedGraph<int, UndirectedEdge<int>> graph = CreateUndirectedGraph(new[]
            {
                new Vertices(1, 2),
                new Vertices(2, 3),
                new Vertices(1, 3)
            });

            graph.AddVertex(4);
            graph.AddVertex(5);

            AssertIsEulerian(true, graph);

            // Not Eulerian
            graph = CreateUndirectedGraph(new[]
            {
                new Vertices(1, 2),
                new Vertices(2, 3),
                new Vertices(1, 3),
                new Vertices(4, 5),
                new Vertices(5, 6),
                new Vertices(4, 6)
            });

            graph.AddVertex(7);

            AssertIsEulerian(false, graph);
        }

        [Test]
        public void IsEulerianOneVertexWithLoop()
        {
            UndirectedGraph<int, UndirectedEdge<int>> graph = CreateUndirectedGraph(new[]
            {
                new Vertices(1, 1)
            });

            AssertIsEulerian(true, graph);
        }

        [Test]
        public void IsEulerianOneVertexWithTwoLoops()
        {
            UndirectedGraph<int, UndirectedEdge<int>> graph = CreateUndirectedGraph(new[]
            {
                new Vertices(1, 1),
                new Vertices(1, 1)
            });

            AssertIsEulerian(true, graph);
        }

        [Test]
        public void IsEulerianTwoVertices()
        {
            UndirectedGraph<int, UndirectedEdge<int>> graph = CreateUndirectedGraph(new[]
            {
                new Vertices(1, 2),
                new Vertices(2, 2)
            });

            AssertIsEulerian(false, graph);
        }

        [Test]
        public void IsEulerianTwoVerticesWithLoops()
        {
            UndirectedGraph<int, UndirectedEdge<int>> graph = CreateUndirectedGraph(new[]
            {
                new Vertices(1, 1),
                new Vertices(2, 2)
            });

            AssertIsEulerian(false, graph);
        }

        [Test]
        public void IsEulerianTwoVerticesOneEdge()
        {
            UndirectedGraph<int, UndirectedEdge<int>> graph = CreateUndirectedGraph(new[]
            {
                new Vertices(1, 2)
            });

            AssertIsEulerian(false, graph);
        }

        [Test]
        public void IsEulerian_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => new IsEulerianGraphAlgorithm<int, UndirectedEdge<int>>(null));

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(
                () => IsEulerianGraphAlgorithm.IsEulerian<int, UndirectedEdge<int>>(null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }
    }
}