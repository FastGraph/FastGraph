using System;
using NUnit.Framework;
using static QuikGraph.Tests.GraphTestHelpers;

namespace QuikGraph.Tests
{
    /// <summary>
    /// Test for <see cref="UndirectedGraph{TVertex,TEdge}"/>s.
    ///</summary>
    [TestFixture]
    internal class UndirectedGraphTests
    {
        [Test]
        public void ContainsEdge_DirectedEdge()
        {
            var graph = new UndirectedGraph<int, IEdge<int>>();
            var e12 = new EquatableEdge<int>(1, 2);
            var f12 = new EquatableEdge<int>(1, 2);
            var e21 = new EquatableEdge<int>(2, 1);
            var f21 = new EquatableEdge<int>(2, 1);

            graph.AddVerticesAndEdge(e12);

            ContainsEdgeAssertions(graph, e12, f12, e21, f21);
        }

        [Test]
        public void ContainsEdge_UndirectedEdge()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentException>(() => new SEquatableUndirectedEdge<int>(2, 1));

            var graph = new UndirectedGraph<int, IEdge<int>>();
            var e12 = new SEquatableUndirectedEdge<int>(1, 2);
            var f12 = new SEquatableUndirectedEdge<int>(1, 2);

            graph.AddVerticesAndEdge(e12);

            ContainsEdgeAssertions(graph, e12, f12, null, null);
        }
    }
}
