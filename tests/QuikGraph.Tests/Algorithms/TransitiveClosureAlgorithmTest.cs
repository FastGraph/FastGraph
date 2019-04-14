using NUnit.Framework;
using QuikGraph.Algorithms;
using QuikGraph.Tests;

namespace QuikGraph.Tests.Algorithms
{
    [TestFixture]
    internal class TransitiveClosureAlgorithmTest : QuikGraphUnitTests
    {

        [Test]
        public void SmallTest()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[] { new Edge<int>(1, 2), new Edge<int>(2, 3)});

            var result = graph.ComputeTransitiveClosure((u, v) => new Edge<int>(u, v));
            Assert.AreEqual(3, result.EdgeCount);
        }

        [Test]
        public void Test()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[] { new Edge<int>(1, 2) , new Edge<int>(2, 3), new Edge<int>(3, 4), new Edge<int>(3, 5) });

            var result = graph.ComputeTransitiveClosure((u, v) => new Edge<int>(u, v));
            Assert.AreEqual(9, result.EdgeCount);
        }
    }
}
