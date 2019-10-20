using System;
using NUnit.Framework;
using QuikGraph.Algorithms;

namespace QuikGraph.Tests.Algorithms
{
    /// <summary>
    /// Tests for <see cref="TransitiveReductionAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class TransitiveReductionAlgorithmTests : AlgorithmTestsBase
    {
        [Test]
        public void Constructor()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            var algorithm = new TransitiveReductionAlgorithm<int, Edge<int>>(graph);
            AssertAlgorithmState(algorithm, graph);
            Assert.IsNotNull(algorithm.TransitiveReduction);
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => new TransitiveReductionAlgorithm<int, Edge<int>>(null));
        }

        [Test]
        public void TransitiveReduction_ValueType()
        {
            // Test 1
            var graph = new BidirectionalGraph<int, SEdge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new SEdge<int>(1, 2),
                new SEdge<int>(1, 3),
                new SEdge<int>(1, 4),
                new SEdge<int>(1, 5),
                new SEdge<int>(2, 4),
                new SEdge<int>(3, 4),
                new SEdge<int>(3, 5),
                new SEdge<int>(4, 5)
            });

            BidirectionalGraph<int, SEdge<int>> result = graph.ComputeTransitiveReduction();
            Assert.AreEqual(5, result.EdgeCount);

            // Test 2
            graph = new BidirectionalGraph<int, SEdge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new SEdge<int>(0, 1),
                new SEdge<int>(0, 2),
                new SEdge<int>(0, 3),
                new SEdge<int>(2, 3),
                new SEdge<int>(2, 4),
                new SEdge<int>(2, 5),
                new SEdge<int>(3, 5),
                new SEdge<int>(4, 5),
                new SEdge<int>(6, 5),
                new SEdge<int>(6, 7),
                new SEdge<int>(7, 4)
            });

            result = graph.ComputeTransitiveReduction();
            Assert.AreEqual(8, result.EdgeCount);
        }

        [Test]
        public void TransitiveReduction_ReferenceType()
        {
            // Test 1
            var graph = new BidirectionalGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(1, 3),
                new Edge<int>(1, 4),
                new Edge<int>(1, 5),
                new Edge<int>(2, 4),
                new Edge<int>(3, 4),
                new Edge<int>(3, 5),
                new Edge<int>(4, 5)
            });

            BidirectionalGraph<int, Edge<int>> result = graph.ComputeTransitiveReduction();
            Assert.AreEqual(5, result.EdgeCount);

            // Test 2
            graph = new BidirectionalGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(0, 1),
                new Edge<int>(0, 2),
                new Edge<int>(0, 3),
                new Edge<int>(2, 3),
                new Edge<int>(2, 4),
                new Edge<int>(2, 5),
                new Edge<int>(3, 5),
                new Edge<int>(4, 5),
                new Edge<int>(6, 5),
                new Edge<int>(6, 7),
                new Edge<int>(7, 4)
            });

            result = graph.ComputeTransitiveReduction();
            Assert.AreEqual(8, result.EdgeCount);
        }
    }
}
