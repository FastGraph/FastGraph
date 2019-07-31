using System;
using NUnit.Framework;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.ShortestPath;

namespace QuikGraph.Tests.Regression
{
    /// <summary>
    /// Tests for <see cref="BellmanFordShortestPathAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class ShortestPathBellmanFordTests
    {
        [Test]
        public void Repro12901()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var graph = new BidirectionalGraph<int, Edge<int>>();
                const int vertex = 1;
                graph.AddVerticesAndEdge(new Edge<int>(vertex, vertex));
                var pathFinder = graph.ShortestPathsBellmanFord(edge => -1.0, vertex);
                pathFinder(vertex, out _);
            });
        }
    }
}
