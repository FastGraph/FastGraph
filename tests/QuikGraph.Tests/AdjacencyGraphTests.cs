using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace QuikGraph.Tests
{
    /// <summary>
    /// Tests for <see cref="AdjacencyGraph{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class AdjacencyGraphTests
    {
        [Test]
        public void AddEdge_Throws()
        {
            {
                KeyValuePair<int, int>[] keyValuePairs = new KeyValuePair<int, int>[0];
                AdjacencyGraph<int, Edge<int>> adjacencyGraph = AdjacencyGraphFactory.Create(false, keyValuePairs);
                Edge<int> edge = EdgeFactory.Create(0, 0);
                Assert.Throws<KeyNotFoundException>(() =>
                    adjacencyGraph.AddEdge(edge)); // Vertex is not added to the graph
            }

            {
                var adjacencyGraph = new AdjacencyGraph<int, SEdge<int>>(false, 0, 0);
                Assert.Throws<KeyNotFoundException>(() => adjacencyGraph.AddEdge(default)); // Vertex is not added to the graph
            }

            {
                var adjacencyGraph = new AdjacencyGraph<int, SEdge<int>>(false, 1, 0);
                Assert.Throws<KeyNotFoundException>(() => adjacencyGraph.AddEdge(default)); // Vertex is not added to the graph
            }

            {
                var adjacencyGraph = new AdjacencyGraph<int, SEdge<int>>(false, int.MinValue, 0);
                Assert.Throws<KeyNotFoundException>(() => adjacencyGraph.AddEdge(default)); // Vertex is not added to the graph
            }
        }

        [Test]
        public void AddEdge_ThrowsNullArgument()
        {
            // ReSharper disable AssignNullToNotNullAttribute
            KeyValuePair<int, int>[] keyValuePairs = new KeyValuePair<int, int>[0];
            AdjacencyGraph<int, Edge<int>> adjacencyGraph = AdjacencyGraphFactory.Create(false, keyValuePairs);
            Assert.Throws<ArgumentNullException>(() => adjacencyGraph.AddEdge(null));


            keyValuePairs = new KeyValuePair<int, int>[1];
            adjacencyGraph = AdjacencyGraphFactory.Create(false, keyValuePairs);
            Assert.Throws<ArgumentNullException>(() => adjacencyGraph.AddEdge(null));


            keyValuePairs = new KeyValuePair<int, int>[2];
            var s0 = new KeyValuePair<int, int>(1048578, 840056837);
            keyValuePairs[0] = s0;
            var s1 = new KeyValuePair<int, int>(273287168, 273287168);
            keyValuePairs[1] = s1;
            adjacencyGraph = AdjacencyGraphFactory.Create(false, keyValuePairs);
            Assert.Throws<ArgumentNullException>(() => adjacencyGraph.AddEdge(null));


            keyValuePairs = new KeyValuePair<int, int>[2];
            s0 = new KeyValuePair<int, int>(-2097099498, 50384150);
            keyValuePairs[0] = s0;
            s1 = new KeyValuePair<int, int>(-2097099498, -2097099498);
            keyValuePairs[1] = s1;
            adjacencyGraph = AdjacencyGraphFactory.Create(false, keyValuePairs);
            Assert.Throws<ArgumentNullException>(() => adjacencyGraph.AddEdge(null));


            keyValuePairs = new KeyValuePair<int, int>[3];
            s0 = new KeyValuePair<int, int>(1048578, 840056837);
            keyValuePairs[0] = s0;
            s1 = new KeyValuePair<int, int>(273287168, 273287168);
            keyValuePairs[1] = s1;
            var s2 = new KeyValuePair<int, int>(-1, -1);
            keyValuePairs[2] = s2;
            adjacencyGraph = AdjacencyGraphFactory.Create(false, keyValuePairs);
            Assert.Throws<ArgumentNullException>(() => adjacencyGraph.AddEdge(null));
            // ReSharper restore AssignNullToNotNullAttribute
        }
    }
}
