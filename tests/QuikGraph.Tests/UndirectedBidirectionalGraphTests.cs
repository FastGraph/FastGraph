using NUnit.Framework;
using static QuikGraph.Tests.GraphTestHelpers;

namespace QuikGraph.Tests
{
    /// <summary>
    /// Test for <see cref="UndirectedBidirectionalGraph{TVertex,TEdge}"/>s.
    ///</summary>
    [TestFixture]
    internal class UndirectedBidirectionalGraphTests
    {
        [Test]
        public void ContainsEdge_DirectedEdge()
        {
            var graph = new BidirectionalGraph<int, IEdge<int>>();

            var e12 = new EquatableEdge<int>(1, 2);
            var f12 = new EquatableEdge<int>(1, 2);
            var e21 = new EquatableEdge<int>(2, 1);
            var f21 = new EquatableEdge<int>(2, 1);

            graph.AddVerticesAndEdge(e12);

            BidirectionalContainsEdgeAssertions(graph, e12, f12, e21, f21);

            var undirectedGraph = new UndirectedBidirectionalGraph<int, IEdge<int>>(graph);

            ContainsEdgeAssertions(undirectedGraph, e12, f12, e21, f21);
        }

        [Test]
        public void ContainsEdge_UndirectedEdge()
        {
            var graph = new BidirectionalGraph<int, IEdge<int>>();

            var e12 = new EquatableUndirectedEdge<int>(1, 2);
            var f12 = new EquatableUndirectedEdge<int>(1, 2);

            graph.AddVerticesAndEdge(e12);

            BidirectionalContainsEdgeAssertions(graph, e12, f12, null, null);

            var undirectedGraph = new UndirectedBidirectionalGraph<int, IEdge<int>>(graph);

            ContainsEdgeAssertions(undirectedGraph, e12, f12, null, null);
        }
    }
}
