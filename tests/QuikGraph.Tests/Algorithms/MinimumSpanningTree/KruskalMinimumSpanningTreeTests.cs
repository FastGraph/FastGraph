using System;
using NUnit.Framework;
using QuikGraph.Algorithms.MinimumSpanningTree;
using static QuikGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace QuikGraph.Tests.Algorithms.MinimumSpanningTree
{
    /// <summary>
    /// Tests for <see cref="KruskalMinimumSpanningTreeAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class KruskalMinimumSpanningTreeTests : MinimumSpanningTreeTestsBase
    {
        [Test]
        public void Constructor()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            var algorithm = new KruskalMinimumSpanningTreeAlgorithm<int, Edge<int>>(graph, edge => 1.0);
            AssertAlgorithmState(algorithm, graph);

            algorithm = new KruskalMinimumSpanningTreeAlgorithm<int, Edge<int>>(null, graph, edge => 1.0);
            AssertAlgorithmState(algorithm, graph);
        }

        [Test]
        public void Constructor_Throws()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();

            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => new KruskalMinimumSpanningTreeAlgorithm<int, Edge<int>>(null, edge => 1.0));
            Assert.Throws<ArgumentNullException>(
                () => new KruskalMinimumSpanningTreeAlgorithm<int, Edge<int>>(graph, null));
            Assert.Throws<ArgumentNullException>(
                () => new KruskalMinimumSpanningTreeAlgorithm<int, Edge<int>>(null, null));

            Assert.Throws<ArgumentNullException>(
                () => new KruskalMinimumSpanningTreeAlgorithm<int, Edge<int>>(null, null, edge => 1.0));
            Assert.Throws<ArgumentNullException>(
                () => new KruskalMinimumSpanningTreeAlgorithm<int, Edge<int>>(null, graph, null));
            Assert.Throws<ArgumentNullException>(
                () => new KruskalMinimumSpanningTreeAlgorithm<int, Edge<int>>(null, null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Kruskal()
        {
            // Kruskal 10, 50, 100, 200, 300, 400
            UndirectedGraph<string, TaggedEdge<string, double>> graph = GetUndirectedCompleteGraph(10);
            KruskalSpanningTree(graph, x => x.Tag);

            graph = GetUndirectedCompleteGraph(50);
            KruskalSpanningTree(graph, x => x.Tag);

            graph = GetUndirectedCompleteGraph(100);
            KruskalSpanningTree(graph, x => x.Tag);

            graph = GetUndirectedCompleteGraph(200);
            KruskalSpanningTree(graph, x => x.Tag);

            graph = GetUndirectedCompleteGraph(300);
            KruskalSpanningTree(graph, x => x.Tag);

            graph = GetUndirectedCompleteGraph(400);
            KruskalSpanningTree(graph, x => x.Tag);
        }

        [Test]
        public void KruskalMinimumSpanningTree()
        {
            foreach (UndirectedGraph<string, Edge<string>> graph in TestGraphFactory.GetUndirectedGraphs_All())
                Kruskal(graph);
        }
    }
}