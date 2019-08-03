using System.Linq;
using NUnit.Framework;

namespace QuikGraph.Tests
{
    /// <summary>
    /// Tests for <see cref="BidirectionalGraph{TVertex,TEdge}"/>s.
    ///</summary>
    [TestFixture]
    internal class BidirectionalGraphTests
    {
        [Test]
        public void Clone()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            graph.AddVertexRange(new[] { 1, 2, 3 });
            graph.AddEdge(new Edge<int>(1, 2));
            graph.AddEdge(new Edge<int>(2, 3));
            graph.AddEdge(new Edge<int>(3, 1));

            Assert.AreEqual(3, graph.VertexCount);
            Assert.AreEqual(3, graph.EdgeCount);

            var clonedGraph = graph.Clone();

            Assert.AreEqual(3, clonedGraph.VertexCount);
            Assert.AreEqual(3, clonedGraph.EdgeCount);

            clonedGraph.AddVertexRange(new[] { 10, 11, 12, 13 });
            clonedGraph.AddEdge(new Edge<int>(10, 11));

            Assert.AreEqual(7, clonedGraph.VertexCount);
            Assert.AreEqual(4, clonedGraph.EdgeCount);

            int edgeCount = clonedGraph.Edges.Count();

            Assert.AreEqual(4, edgeCount);

            Assert.AreEqual(3, graph.VertexCount);
            Assert.AreEqual(3, graph.EdgeCount);
        }

        //[Test]
        //public void LoadGraphFromDot()
        //{
        //    const string dotSource = "digraph { a -> b }";
        //    var vertexFunc = DotParserAdapter.VertexFactory.Name;
        //    var edgeFunc = DotParserAdapter.EdgeFactory<string>.VerticesOnly;
        //    var graph = BidirectionalGraph<string, SEdge<string>>.LoadDot(dotSource, vertexFunc, edgeFunc);
        //    Assert.IsNotNull(graph);
        //}
    }
}
