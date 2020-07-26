using System;
using NUnit.Framework;
using QuikGraph.Algorithms;
using static QuikGraph.Tests.Algorithms.AlgorithmTestHelpers;
using static QuikGraph.Tests.GraphTestHelpers;

namespace QuikGraph.Tests.Algorithms
{
    /// <summary>
    /// Tests for <see cref="TransitiveReductionAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class TransitiveReductionAlgorithmTests
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
            var edge12 = new SEdge<int>(1, 2);
            var edge13 = new SEdge<int>(1, 3);
            var edge14 = new SEdge<int>(1, 4);
            var edge15 = new SEdge<int>(1, 5);
            var edge24 = new SEdge<int>(2, 4);
            var edge34 = new SEdge<int>(3, 4);
            var edge35 = new SEdge<int>(3, 5);
            var edge45 = new SEdge<int>(4, 5);
            var graph = new BidirectionalGraph<int, SEdge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge13, edge14, edge15,
                edge24, edge34, edge35, edge45
            });

            BidirectionalGraph<int, SEdge<int>> result = graph.ComputeTransitiveReduction();
            AssertHasVertices(result, new[] { 1, 2, 3, 4, 5 });
            AssertHasEdges(
                result,
                new[] { edge12, edge13, edge24, edge34, edge45 });
            Assert.AreEqual(5, result.EdgeCount);

            // Test 2
            var edge01 = new SEdge<int>(0, 1);
            var edge02 = new SEdge<int>(0, 2);
            var edge03 = new SEdge<int>(0, 3);
            var edge23 = new SEdge<int>(2, 3);
            var edge25 = new SEdge<int>(2, 5);
            var edge65 = new SEdge<int>(6, 5);
            var edge67 = new SEdge<int>(6, 7);
            var edge74 = new SEdge<int>(7, 4);
            graph = new BidirectionalGraph<int, SEdge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                edge01, edge02, edge03, edge23,
                edge24, edge25, edge35, edge45,
                edge65, edge67, edge74
            });

            result = graph.ComputeTransitiveReduction();
            AssertHasVertices(result, new[] { 0, 1, 2, 3, 4, 5, 6, 7 });
            AssertHasEdges(
                result,
                new[] { edge01, edge02, edge23, edge24, edge35, edge45, edge67, edge74 });
        }

        [Test]
        public void TransitiveReduction_ReferenceType()
        {
            // Test 1
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge15 = new Edge<int>(1, 5);
            var edge24 = new Edge<int>(2, 4);
            var edge34 = new Edge<int>(3, 4);
            var edge35 = new Edge<int>(3, 5);
            var edge45 = new Edge<int>(4, 5);
            var graph = new BidirectionalGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge13, edge14, edge15,
                edge24, edge34, edge35, edge45
            });

            BidirectionalGraph<int, Edge<int>> result = graph.ComputeTransitiveReduction();
            AssertHasVertices(result, new[] { 1, 2, 3, 4, 5 });
            AssertHasEdges(
                result,
                new[] { edge12, edge13, edge24, edge34, edge45 });

            // Test 2
            var edge01 = new Edge<int>(0, 1);
            var edge02 = new Edge<int>(0, 2);
            var edge03 = new Edge<int>(0, 3);
            var edge23 = new Edge<int>(2, 3);
            var edge25 = new Edge<int>(2, 5);
            var edge65 = new Edge<int>(6, 5);
            var edge67 = new Edge<int>(6, 7);
            var edge74 = new Edge<int>(7, 4);
            graph = new BidirectionalGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                edge01, edge02, edge03, edge23,
                edge24, edge25, edge35, edge45,
                edge65, edge67, edge74
            });

            result = graph.ComputeTransitiveReduction();
            AssertHasVertices(result, new[] { 0, 1, 2, 3, 4, 5, 6, 7 });
            AssertHasEdges(
                result,
                new[] { edge01, edge02, edge23, edge24, edge35, edge45, edge67, edge74 });
        }

        [Test]
        public void TransitiveReduction_IsolatedVertices()
        {
            const string vertex1 = "/test";
            const string vertex2 = "/test/123";
            const string vertex3 = "/test/notlinked";
            var edge12 = new Edge<string>(vertex1, vertex2);

            var graph = new BidirectionalGraph<string, Edge<string>>();
            graph.AddVertexRange(new[] { vertex1, vertex2, vertex3 });
            graph.AddEdge(edge12);

            BidirectionalGraph<string, Edge<string>> result = graph.ComputeTransitiveReduction();
            AssertHasVertices(result, new[] { vertex1, vertex2, vertex3 });
            AssertHasEdges(result, new[] { edge12 });
        }
    }
}