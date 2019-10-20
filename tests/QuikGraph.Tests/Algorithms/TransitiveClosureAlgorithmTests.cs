using System;
using NUnit.Framework;
using QuikGraph.Algorithms;

namespace QuikGraph.Tests.Algorithms
{
    /// <summary>
    /// Tests for <see cref="TransitiveClosureAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class TransitiveClosureAlgorithmTests : AlgorithmTestsBase
    {
        [Test]
        public void Constructor()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            var algorithm = new TransitiveClosureAlgorithm<int, Edge<int>>(graph, (v1, v2) => new Edge<int>(v1, v2));
            AssertAlgorithmState(algorithm, graph);
            Assert.IsNotNull(algorithm.TransitiveClosure);
        }

        [Test]
        public void Constructor_Throws()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => new TransitiveClosureAlgorithm<int, Edge<int>>(null, (v1, v2) => new Edge<int>(v1, v2)));
            Assert.Throws<ArgumentNullException>(
                () => new TransitiveClosureAlgorithm<int, Edge<int>>(graph, null));
            Assert.Throws<ArgumentNullException>(
                () => new TransitiveClosureAlgorithm<int, Edge<int>>(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void TransitiveClosure_ValueType()
        {
            // Test 1
            var graph = new BidirectionalGraph<int, SEdge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new SEdge<int>(1, 2),
                new SEdge<int>(2, 3)
            });

            BidirectionalGraph<int, SEdge<int>> result = graph.ComputeTransitiveClosure((u, v) => new SEdge<int>(u, v));
            Assert.AreEqual(3, result.EdgeCount);

            // Test 2
            graph = new BidirectionalGraph<int, SEdge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new SEdge<int>(1, 2),
                new SEdge<int>(2, 3),
                new SEdge<int>(3, 4),
                new SEdge<int>(3, 5)
            });

            result = graph.ComputeTransitiveClosure((u, v) => new SEdge<int>(u, v));
            Assert.AreEqual(9, result.EdgeCount);
        }

        [Test]
        public void TransitiveClosure_ReferenceType()
        {
            // Test 1
            var graph = new BidirectionalGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(2, 3)
            });

            BidirectionalGraph<int, Edge<int>> result = graph.ComputeTransitiveClosure((u, v) => new Edge<int>(u, v));
            Assert.AreEqual(3, result.EdgeCount);

            // Test 2
            graph = new BidirectionalGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(2, 3),
                new Edge<int>(3, 4),
                new Edge<int>(3, 5)
            });

            result = graph.ComputeTransitiveClosure((u, v) => new Edge<int>(u, v));
            Assert.AreEqual(9, result.EdgeCount);
        }
    }
}
