using NUnit.Framework;
using QuikGraph.Algorithms;

namespace QuikGraph.Tests.Algorithms
{
    /// <summary>
    /// Tests for <see cref="TransitiveClosureAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class TransitiveClosureAlgorithmTests
    {
        [Test]
        public void TransitiveClosure1()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(2, 3)
            });

            BidirectionalGraph<int, Edge<int>> result = graph.ComputeTransitiveClosure((u, v) => new Edge<int>(u, v));
            Assert.AreEqual(3, result.EdgeCount);
        }

        [Test]
        public void TransitiveClosure2()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(2, 3),
                new Edge<int>(3, 4),
                new Edge<int>(3, 5)
            });

            BidirectionalGraph<int, Edge<int>> result = graph.ComputeTransitiveClosure((u, v) => new Edge<int>(u, v));
            Assert.AreEqual(9, result.EdgeCount);
        }
    }
}
