using System;
using System.Collections.Generic;
using NUnit.Framework;
using QuikGraph.Algorithms;
using QuikGraph.Tests;

namespace QuikGraph.Tests.Regression
{
    [TestFixture]
    internal class ShortestPathBellmanFordTest : QuikGraphUnitTests
    {
        [Test]
        public void Repro12901()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var graph = new BidirectionalGraph<int, Edge<int>>();
                int vertex = 1;
                graph.AddVerticesAndEdge(new Edge<int>(vertex, vertex));
                var pathFinder = AlgorithmExtensions.ShortestPathsBellmanFord<int, Edge<int>>(graph, edge => -1.0, vertex);
                pathFinder(vertex, out IEnumerable<Edge<int>> path);
            });
        }
    }
}
