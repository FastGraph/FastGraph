using System;
using NUnit.Framework;
using QuikGraph.Algorithms.VertexCover;
using static QuikGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace QuikGraph.Tests.Algorithms.VertexCover
{
    /// <summary>
    /// Tests for <see cref="MinimumVertexCoverApproximationAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class MinimumVertexCoverApproximationAlgorithmTests
    {
        [Test]
        public void Constructor()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            var algorithm = new MinimumVertexCoverApproximationAlgorithm<int, Edge<int>>(graph);
            AssertAlgorithmState(algorithm, graph);
            Assert.IsNull(algorithm.CoverSet);

            algorithm = new MinimumVertexCoverApproximationAlgorithm<int, Edge<int>>(graph, new Random(123));
            AssertAlgorithmState(algorithm, graph);
            Assert.IsNull(algorithm.CoverSet);
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => new MinimumVertexCoverApproximationAlgorithm<int, Edge<int>>(null));
            Assert.Throws<ArgumentNullException>(
                () => new MinimumVertexCoverApproximationAlgorithm<int, Edge<int>>(null, new Random(123)));
            Assert.Throws<ArgumentNullException>(
                () => new MinimumVertexCoverApproximationAlgorithm<int, Edge<int>>(new UndirectedGraph<int, Edge<int>>(), null));
            Assert.Throws<ArgumentNullException>(
                () => new MinimumVertexCoverApproximationAlgorithm<int, Edge<int>>(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Cover()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            var algorithm = new MinimumVertexCoverApproximationAlgorithm<int, Edge<int>>(graph);
            algorithm.Compute();
            CollectionAssert.IsEmpty(algorithm.CoverSet);

            graph.AddVertexRange(new[] { 1, 2, 3 });
            algorithm = new MinimumVertexCoverApproximationAlgorithm<int, Edge<int>>(graph);
            algorithm.Compute();
            CollectionAssert.IsEmpty(algorithm.CoverSet);

            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(2, 2),
                new Edge<int>(3, 1)
            });
            algorithm = new MinimumVertexCoverApproximationAlgorithm<int, Edge<int>>(graph, new Random(123456));
            algorithm.Compute();
            CollectionAssert.AreEquivalent(
                new[] { 1, 2 },
                algorithm.CoverSet);

            graph.AddVertex(4);
            algorithm = new MinimumVertexCoverApproximationAlgorithm<int, Edge<int>>(graph, new Random(123456));
            algorithm.Compute();
            CollectionAssert.AreEquivalent(
                new[] { 1, 2 },
                algorithm.CoverSet);

            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(5, 2)
            });
            algorithm = new MinimumVertexCoverApproximationAlgorithm<int, Edge<int>>(graph, new Random(123456));
            algorithm.Compute();
            CollectionAssert.AreEquivalent(
                new[] { 1, 2 },
                algorithm.CoverSet);

            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(6, 7),
                new Edge<int>(7, 8),
                new Edge<int>(9, 8)
            });
            algorithm = new MinimumVertexCoverApproximationAlgorithm<int, Edge<int>>(graph, new Random(123456));
            algorithm.Compute();
            CollectionAssert.AreEquivalent(
                new[] { 2, 3, 7, 9 },
                algorithm.CoverSet);

            // Other seed give other results
            algorithm = new MinimumVertexCoverApproximationAlgorithm<int, Edge<int>>(graph, new Random(456789));
            algorithm.Compute();
            CollectionAssert.AreEquivalent(
                new[] { 1, 2, 7, 8 },
                algorithm.CoverSet);
        }
    }
}