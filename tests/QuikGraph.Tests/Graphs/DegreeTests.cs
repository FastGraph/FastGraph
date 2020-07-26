using JetBrains.Annotations;
using NUnit.Framework;

namespace QuikGraph.Tests.Graphs
{
    /// <summary>
    /// Tests relative the to the degree of graphs.
    /// </summary>
    [TestFixture]
    internal class DegreeTests
    {
        #region Test helpers

        private static void AssertDegreeSumEqualsTwiceEdgeCount<TVertex, TEdge>(
            [NotNull] IBidirectionalGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            int totalDegree = 0;
            foreach (TVertex vertex in graph.Vertices)
                totalDegree += graph.Degree(vertex);

            Assert.AreEqual(graph.EdgeCount * 2, totalDegree);
        }

        private static void AssertInDegreeSumEqualsEdgeCount<TVertex, TEdge>(
            [NotNull] IBidirectionalGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            int totalInDegree = 0;
            foreach (TVertex vertex in graph.Vertices)
                totalInDegree += graph.InDegree(vertex);

            Assert.AreEqual(graph.EdgeCount, totalInDegree);
        }

        private static void OutDegreeSumEqualsEdgeCount<TVertex, TEdge>(
            [NotNull] IVertexAndEdgeListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            int totalOutDegree = 0;
            foreach (TVertex vertex in graph.Vertices)
                totalOutDegree += graph.OutDegree(vertex);

            Assert.AreEqual(graph.EdgeCount, totalOutDegree);
        }

        private static void AssertAdjacentDegreeSumEqualsTwiceEdgeCount<TVertex, TEdge>(
            [NotNull] IUndirectedGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            int totalAdjacentDegree = 0;
            foreach (TVertex vertex in graph.Vertices)
                totalAdjacentDegree += graph.AdjacentDegree(vertex);

            Assert.AreEqual(graph.EdgeCount * 2, totalAdjacentDegree);
        }

        #endregion

        [Test]
        public void DegreeSumEqualsTwiceEdgeCount()
        {
            foreach (BidirectionalGraph<string, Edge<string>> graph in TestGraphFactory.GetBidirectionalGraphs_All())
                AssertDegreeSumEqualsTwiceEdgeCount(graph);
        }

        [Test]
        public void InDegreeSumEqualsEdgeCount()
        {
            foreach (BidirectionalGraph<string, Edge<string>> graph in TestGraphFactory.GetBidirectionalGraphs_All())
                AssertInDegreeSumEqualsEdgeCount(graph);
        }

        [Test]
        public void OutDegreeSumEqualsEdgeCount()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs_All())
                OutDegreeSumEqualsEdgeCount(graph);
            foreach (BidirectionalGraph<string, Edge<string>> graph in TestGraphFactory.GetBidirectionalGraphs_All())
                OutDegreeSumEqualsEdgeCount(graph);
        }

        [Test]
        public void AdjacentDegreeSumEqualsTwiceEdgeCount()
        {
            foreach (UndirectedGraph<string, Edge<string>> graph in TestGraphFactory.GetUndirectedGraphs_All())
                AssertAdjacentDegreeSumEqualsTwiceEdgeCount(graph);
        }
    }
}