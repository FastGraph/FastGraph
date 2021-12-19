#nullable enable

using NUnit.Framework;
using FastGraph.Algorithms.VertexCover;
using static FastGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace FastGraph.Tests.Algorithms.VertexCover
{
    /// <summary>
    /// Tests for <see cref="MinimumVertexCoverApproximationAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class MinimumVertexCoverApproximationAlgorithmTests
    {
        [Test]
        public void Constructor()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            var algorithm = new MinimumVertexCoverApproximationAlgorithm<int, Edge<int>>(graph);
            AssertAlgorithmState(algorithm, graph);
            algorithm.CoverSet.Should().BeNull();

            algorithm = new MinimumVertexCoverApproximationAlgorithm<int, Edge<int>>(graph, new Random(123));
            AssertAlgorithmState(algorithm, graph);
            algorithm.CoverSet.Should().BeNull();
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new MinimumVertexCoverApproximationAlgorithm<int, Edge<int>>(default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MinimumVertexCoverApproximationAlgorithm<int, Edge<int>>(default, new Random(123))).Should().Throw<ArgumentNullException>();
            Invoking(() => new MinimumVertexCoverApproximationAlgorithm<int, Edge<int>>(new UndirectedGraph<int, Edge<int>>(), default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MinimumVertexCoverApproximationAlgorithm<int, Edge<int>>(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Cover()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            var algorithm = new MinimumVertexCoverApproximationAlgorithm<int, Edge<int>>(graph);
            algorithm.Compute();
            algorithm.CoverSet.Should().BeEmpty();

            graph.AddVertexRange(new[] { 1, 2, 3 });
            algorithm = new MinimumVertexCoverApproximationAlgorithm<int, Edge<int>>(graph);
            algorithm.Compute();
            algorithm.CoverSet.Should().BeEmpty();

            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(2, 2),
                new Edge<int>(3, 1)
            });
            algorithm = new MinimumVertexCoverApproximationAlgorithm<int, Edge<int>>(graph, new Random(123456));
            algorithm.Compute();
            algorithm.CoverSet.Should().BeEquivalentTo(new[] { 1, 2 });

            graph.AddVertex(4);
            algorithm = new MinimumVertexCoverApproximationAlgorithm<int, Edge<int>>(graph, new Random(123456));
            algorithm.Compute();
            algorithm.CoverSet.Should().BeEquivalentTo(new[] { 1, 2 });

            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(5, 2)
            });
            algorithm = new MinimumVertexCoverApproximationAlgorithm<int, Edge<int>>(graph, new Random(123456));
            algorithm.Compute();
            algorithm.CoverSet.Should().BeEquivalentTo(new[] { 1, 2 });

            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(6, 7),
                new Edge<int>(7, 8),
                new Edge<int>(9, 8)
            });
            algorithm = new MinimumVertexCoverApproximationAlgorithm<int, Edge<int>>(graph, new Random(123456));
            algorithm.Compute();
            algorithm.CoverSet.Should().BeEquivalentTo(new[] { 2, 3, 7, 9 });

            // Other seed give other results
            algorithm = new MinimumVertexCoverApproximationAlgorithm<int, Edge<int>>(graph, new Random(456789));
            algorithm.Compute();
            algorithm.CoverSet.Should().BeEquivalentTo(new[] { 1, 2, 7, 8 });
        }
    }
}
