using System.Collections.Generic;
using NUnit.Framework;
using QuikGraph.Algorithms.ShortestPath;

namespace QuikGraph.Tests.Algorithms.ShortestPath
{
    /// <summary>
    /// Tests for <see cref="FloydWarshallAllShortestPathAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class FloydWarshallAllShortestPathAlgorithmTests : FloydWarshallTestsBase
    {
        [Test]
        public void Compute()
        {
            var distances = new Dictionary<Edge<char>, double>();
            var graph = CreateGraph(distances);
            var algorithm = new FloydWarshallAllShortestPathAlgorithm<char, Edge<char>>(graph, e => distances[e]);
            algorithm.Compute();

            Assert.IsTrue(algorithm.TryGetDistance('A', 'A', out double distance));
            Assert.AreEqual(0, distance);

            Assert.IsTrue(algorithm.TryGetDistance('A', 'B', out distance));
            Assert.AreEqual(6, distance);

            Assert.IsTrue(algorithm.TryGetDistance('A', 'C', out distance));
            Assert.AreEqual(1, distance);

            Assert.IsTrue(algorithm.TryGetDistance('A', 'D', out distance));
            Assert.AreEqual(4, distance);

            Assert.IsTrue(algorithm.TryGetDistance('A', 'E', out distance));
            Assert.AreEqual(5, distance);
        }
    }
}
