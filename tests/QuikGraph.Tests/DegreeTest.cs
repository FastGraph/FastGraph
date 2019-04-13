using Microsoft.Pex.Framework;
using NUnit.Framework;
using QuickGraph.Serialization;
using QuikGraph.Tests;

namespace QuickGraph.Tests
{
    [TestFixture, PexClass]
    internal class DegreeTest : QuikGraphUnitTests
    {
        [Test]
        public void DegreeSumEqualsTwiceEdgeCountAll()
        {
            foreach (var g in TestGraphFactory.GetBidirectionalGraphs())
                this.DegreeSumEqualsTwiceEdgeCount(g);
        }

        [PexMethod]
        public void DegreeSumEqualsTwiceEdgeCount<TVertex, TEdge>(
            [PexAssumeNotNull]IBidirectionalGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            int edgeCount = graph.EdgeCount;
            int degCount = 0;
            foreach (var v in graph.Vertices)
                degCount += graph.Degree(v);

            Assert.AreEqual(edgeCount * 2, degCount);
        }

        [Test]
        public void InDegreeSumEqualsEdgeCountAll()
        {
            foreach (var g in TestGraphFactory.GetBidirectionalGraphs())
                this.InDegreeSumEqualsEdgeCount(g);
        }

        [PexMethod]
        public void InDegreeSumEqualsEdgeCount<TVertex,TEdge>(
            [PexAssumeNotNull] IBidirectionalGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            int edgeCount = graph.EdgeCount;
            int degCount = 0;
            foreach (var v in graph.Vertices)
                degCount += graph.InDegree(v);

            Assert.AreEqual(edgeCount, degCount);
        }

        [Test]
        public void OutDegreeSumEqualsEdgeCountAll()
        {
            foreach (var g in TestGraphFactory.GetBidirectionalGraphs())
                this.OutDegreeSumEqualsEdgeCount(g);
        }

        [PexMethod]
        public void OutDegreeSumEqualsEdgeCount<TVertex,TEdge>(
            [PexAssumeNotNull] IBidirectionalGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            int edgeCount = graph.EdgeCount;
            int degCount = 0;
            foreach (var v in graph.Vertices)
                degCount += graph.OutDegree(v);

            Assert.AreEqual(edgeCount, degCount);
        }

    }
}
