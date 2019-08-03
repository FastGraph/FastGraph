using NUnit.Framework;
using QuikGraph.Algorithms;

namespace QuikGraph.Tests.Algorithms
{
    /// <summary>
    /// Tests for <see cref="TransitiveReductionAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class TransitiveReductionAlgorithmTests
    {
        [Test]
        public void TransitiveReduction1()
        {
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
        }

        [Test]
        public void TransitiveReduction2()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();

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

            BidirectionalGraph<int, Edge<int>> result = graph.ComputeTransitiveReduction();
            Assert.AreEqual(8, result.EdgeCount);
        }
    }
}
