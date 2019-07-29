using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Serialization;

namespace QuikGraph.Tests
{
    /// <summary>
    /// Tests relative the to the degree of graphs.
    /// </summary>
    [TestFixture]
    internal class DegreeTests
    {
        #region Helpers

        public void DegreeSumEqualsTwiceEdgeCount<TVertex, TEdge>(
            [NotNull] IBidirectionalGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            int edgeCount = graph.EdgeCount;
            int degCount = 0;
            foreach (TVertex vertex in graph.Vertices)
                degCount += graph.Degree(vertex);

            Assert.AreEqual(edgeCount * 2, degCount);
        }

        public void InDegreeSumEqualsEdgeCount<TVertex, TEdge>(
            [NotNull] IBidirectionalGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            int edgeCount = graph.EdgeCount;
            int degCount = 0;
            foreach (TVertex vertex in graph.Vertices)
                degCount += graph.InDegree(vertex);

            Assert.AreEqual(edgeCount, degCount);
        }

        public void OutDegreeSumEqualsEdgeCount<TVertex, TEdge>(
            [NotNull] IBidirectionalGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            int edgeCount = graph.EdgeCount;
            int degCount = 0;
            foreach (TVertex vertex in graph.Vertices)
                degCount += graph.OutDegree(vertex);

            Assert.AreEqual(edgeCount, degCount);
        }

        #endregion

        [Test]
        public void DegreeSumEqualsTwiceEdgeCountAll()
        {
            foreach (BidirectionalGraph<string, Edge<string>> graph in TestGraphFactory.GetBidirectionalGraphs())
                DegreeSumEqualsTwiceEdgeCount(graph);
        }

        [Test]
        public void InDegreeSumEqualsEdgeCountAll()
        {
            foreach (BidirectionalGraph<string, Edge<string>> graph in TestGraphFactory.GetBidirectionalGraphs())
                InDegreeSumEqualsEdgeCount(graph);
        }

        [Test]
        public void OutDegreeSumEqualsEdgeCountAll()
        {
            foreach (BidirectionalGraph<string, Edge<string>> graph in TestGraphFactory.GetBidirectionalGraphs())
                OutDegreeSumEqualsEdgeCount(graph);
        }
    }
}
