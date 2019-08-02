using System;
using System.Collections.Generic;
using NUnit.Framework;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.ConnectedComponents;

namespace QuikGraph.Tests.Algorithms.ConnectedComponents
{
    /// <summary>
    /// Tests for <see cref="IncrementalConnectedComponentsAlgorithm{TVertex,TEdge}"/>
    /// </summary>
    [TestFixture]
    internal class IncrementalConnectedComponentsAlgorithmTests
    {
        [Test]
        public void IncrementalConnectedComponent()
        {
            var graph = new AdjacencyGraph<int, SEquatableEdge<int>>();
            graph.AddVertexRange(new[] { 0, 1, 2, 3 });
            Func<KeyValuePair<int, IDictionary<int, int>>> components = graph.IncrementalConnectedComponents();

            KeyValuePair<int, IDictionary<int, int>> current = components();
            Assert.AreEqual(4, current.Key);

            graph.AddEdge(new SEquatableEdge<int>(0, 1));
            current = components();
            Assert.AreEqual(3, current.Key);

            graph.AddEdge(new SEquatableEdge<int>(2, 3));
            current = components();
            Assert.AreEqual(2, current.Key);

            graph.AddEdge(new SEquatableEdge<int>(1, 3));
            current = components();
            Assert.AreEqual(1, current.Key);
        }
    }
}
