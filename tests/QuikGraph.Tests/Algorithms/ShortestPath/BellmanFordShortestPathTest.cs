using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using QuickGraph.Algorithms;
using QuikGraph.Tests;

namespace QuickGraph.Tests.Algorithms.ShortestPath
{
    [TestFixture]
    internal class BellmanFordShortestPathTest : QuikGraphUnitTests
    {
        [Test]
        public void Sample()
        {
            var testGraph = new AdjacencyGraph<int, Edge<int>>();
            testGraph.AddVerticesAndEdge(new Edge<int>(1, 2));
            testGraph.AddVerticesAndEdge(new Edge<int>(1, 3));
            testGraph.AddVerticesAndEdge(new Edge<int>(3, 4));
            testGraph.AddVerticesAndEdge(new Edge<int>(1, 4));
            var testPath = testGraph.ShortestPathsBellmanFord<int, Edge<int>>(e => 1.0, 1);
            foreach(var i in testGraph.Vertices)
            {
                IEnumerable<Edge<int>> es;
                if (testPath(i, out es))
                    Console.WriteLine("{0}: {1}", i, es.Count());
            }
        }
    }
}
