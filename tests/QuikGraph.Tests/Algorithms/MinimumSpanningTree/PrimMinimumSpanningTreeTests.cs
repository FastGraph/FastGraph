using System;
using NUnit.Framework;
using QuikGraph.Algorithms.MinimumSpanningTree;
using static QuikGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace QuikGraph.Tests.Algorithms.MinimumSpanningTree
{
    /// <summary>
    /// Tests for <see cref="PrimMinimumSpanningTreeAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class PrimMinimumSpanningTreeTests : MinimumSpanningTreeTestsBase
    {
        [Test]
        public void Constructor()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            var algorithm = new PrimMinimumSpanningTreeAlgorithm<int, Edge<int>>(graph, edge => 1.0);
            AssertAlgorithmState(algorithm, graph);

            algorithm = new PrimMinimumSpanningTreeAlgorithm<int, Edge<int>>(null, graph, edge => 1.0);
            AssertAlgorithmState(algorithm, graph);
        }

        [Test]
        public void Constructor_Throws()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();

            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => new PrimMinimumSpanningTreeAlgorithm<int, Edge<int>>(null, edge => 1.0));
            Assert.Throws<ArgumentNullException>(
                () => new PrimMinimumSpanningTreeAlgorithm<int, Edge<int>>(graph, null));
            Assert.Throws<ArgumentNullException>(
                () => new PrimMinimumSpanningTreeAlgorithm<int, Edge<int>>(null, null));

            Assert.Throws<ArgumentNullException>(
                () => new PrimMinimumSpanningTreeAlgorithm<int, Edge<int>>(null, null, edge => 1.0));
            Assert.Throws<ArgumentNullException>(
                () => new PrimMinimumSpanningTreeAlgorithm<int, Edge<int>>(null, graph, null));
            Assert.Throws<ArgumentNullException>(
                () => new PrimMinimumSpanningTreeAlgorithm<int, Edge<int>>(null, null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Prim()
        {
            // Prim 10, 50, 100, 200, 300, 400
            UndirectedGraph<string, TaggedEdge<string, double>> graph = GetUndirectedCompleteGraph(10);
            PrimSpanningTree(graph, x => x.Tag);

            graph = GetUndirectedCompleteGraph(50);
            PrimSpanningTree(graph, x => x.Tag);

            graph = GetUndirectedCompleteGraph(100);
            PrimSpanningTree(graph, x => x.Tag);

            graph = GetUndirectedCompleteGraph(200);
            PrimSpanningTree(graph, x => x.Tag);

            graph = GetUndirectedCompleteGraph(300);
            PrimSpanningTree(graph, x => x.Tag);

            graph = GetUndirectedCompleteGraph(400);
            PrimSpanningTree(graph, x => x.Tag);
        }

        [Test]
        public void PrimMinimumSpanningTree()
        {
            foreach (UndirectedGraph<string, Edge<string>> graph in TestGraphFactory.GetUndirectedGraphs_All())
                Prim(graph);
        }
    }
}