using System;
using System.Collections.Generic;
using NUnit.Framework;
using static QuikGraph.Tests.AssertHelpers;

namespace QuikGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="ArrayBidirectionalGraph{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class ArrayBidirectionalGraphTests : GraphTestsBase
    {
        [Test]
        public void Construction()
        {
            var wrappedGraph = new BidirectionalGraph<int, Edge<int>>();

            var graph = new ArrayBidirectionalGraph<int, Edge<int>>(wrappedGraph);
            AssertGraphProperties(graph);
            AssertEmptyGraph(graph);

            wrappedGraph.AddVertexRange(new[] { 2, 3, 1 });
            graph = new ArrayBidirectionalGraph<int, Edge<int>>(wrappedGraph);
            AssertGraphProperties(graph);
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertNoEdge(graph);

            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(2, 2);
            var edge3 = new Edge<int>(3, 4);
            var edge4 = new Edge<int>(1, 4);
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4 });
            graph = new ArrayBidirectionalGraph<int, Edge<int>>(wrappedGraph);
            AssertGraphProperties(graph);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge1, edge2, edge3, edge4 });

            wrappedGraph = new BidirectionalGraph<int, Edge<int>>(false);
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge1, edge2, edge3, edge4 });
            graph = new ArrayBidirectionalGraph<int, Edge<int>>(wrappedGraph);
            AssertGraphProperties(graph, false);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge1, edge2, edge3, edge4 });

            #region Local function

            void AssertGraphProperties<TVertex, TEdge>(
                ArrayBidirectionalGraph<TVertex, TEdge> g,
                bool allowParallelEdges = true)
                where TEdge : IEdge<TVertex>
            {
                Assert.IsTrue(g.IsDirected);
                Assert.AreEqual(allowParallelEdges, g.AllowParallelEdges);
            }

            #endregion
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new ArrayBidirectionalGraph<int, Edge<int>>(null));
        }

        #region Contains Vertex

        [Test]
        public void ContainsVertex()
        {
            var wrappedGraph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            ContainsVertex_ImmutableGraph_Test(
                wrappedGraph,
                () => new ArrayBidirectionalGraph<TestVertex, Edge<TestVertex>>(wrappedGraph));
        }

        [Test]
        public void ContainsVertex_EquatableVertex()
        {
            var wrappedGraph = new BidirectionalGraph<EquatableTestVertex, Edge<EquatableTestVertex>>();
            ContainsVertex_EquatableVertex_ImmutableGraph_Test(
                wrappedGraph,
                () => new ArrayBidirectionalGraph<EquatableTestVertex, Edge<EquatableTestVertex>>(wrappedGraph));
        }

        [Test]
        public void ContainsVertex_Throws()
        {
            var wrappedGraph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ArrayAdjacencyGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            ContainsVertex_Throws_Test(graph);
        }

        #endregion

        #region Contains Edge

        [Test]
        public void ContainsEdge()
        {
            var wrappedGraph = new BidirectionalGraph<int, Edge<int>>();
            ContainsEdge_ImmutableGraph_Test(
                wrappedGraph,
                () => new ArrayBidirectionalGraph<int, Edge<int>>(wrappedGraph));
        }

        [Test]
        public void ContainsEdge_EquatableEdge()
        {
            var wrappedGraph = new BidirectionalGraph<int, EquatableEdge<int>>();
            ContainsEdge_EquatableEdge_ImmutableGraph_Test(
                wrappedGraph,
                () => new ArrayBidirectionalGraph<int, EquatableEdge<int>>(wrappedGraph));
        }

        [Test]
        public void ContainsEdge_SourceTarget()
        {
            var wrappedGraph = new BidirectionalGraph<int, Edge<int>>();
            ContainsEdge_SourceTarget_ImmutableGraph_Test(
                wrappedGraph,
                () => new ArrayBidirectionalGraph<int, Edge<int>>(wrappedGraph));
        }

        [Test]
        public void ContainsEdge_Throws()
        {
            var wrappedGraph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ArrayBidirectionalGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            ContainsEdge_Throws_Test(graph);
            ContainsEdge_SourceTarget_Throws_Test(graph);
        }

        #endregion

        #region Out Edges

        [Test]
        public void OutEdge()
        {
            var wrappedGraph = new BidirectionalGraph<int, Edge<int>>();
            OutEdge_ImmutableGraph_Test(
                wrappedGraph,
                () => new ArrayBidirectionalGraph<int, Edge<int>>(wrappedGraph));
        }

        [Test]
        public void OutEdge_Throws()
        {
            var wrappedGraph1 = new BidirectionalGraph<int, Edge<int>>();
            var wrappedGraph2 = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            var graph2 = new ArrayBidirectionalGraph<TestVertex, Edge<TestVertex>>(wrappedGraph2);

            OutEdge_Throws_ImmutableGraph_Test(
                wrappedGraph1,
                () => new ArrayBidirectionalGraph<int, Edge<int>>(wrappedGraph1),
                graph2);
        }

        [Test]
        public void OutEdges()
        {
            var wrappedGraph = new BidirectionalGraph<int, Edge<int>>();
            OutEdges_ImmutableGraph_Test(
                wrappedGraph,
                () => new ArrayBidirectionalGraph<int, Edge<int>>(wrappedGraph));
        }

        [Test]
        public void OutEdges_Throws()
        {
            var wrappedGraph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ArrayBidirectionalGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            OutEdges_Throws_Test(graph);
        }

        #endregion

        #region In Edges

        [Test]
        public void InEdge()
        {
            var edge11 = new Edge<int>(1, 1);
            var edge13 = new Edge<int>(1, 3);
            var edge21 = new Edge<int>(2, 1);
            var edge41 = new Edge<int>(4, 1);

            var wrappedGraph = new BidirectionalGraph<int, Edge<int>>();
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge11, edge13, edge21, edge41 });
            var graph = new ArrayBidirectionalGraph<int, Edge<int>>(wrappedGraph);

            Assert.AreSame(edge11, graph.InEdge(1, 0));
            Assert.AreSame(edge41, graph.InEdge(1, 2));
            Assert.AreSame(edge13, graph.InEdge(3, 0));
        }

        [Test]
        public void InEdge_Throws()
        {
            const int vertex1 = 1;
            const int vertex2 = 2;

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            var wrappedGraph1 = new BidirectionalGraph<int, Edge<int>>();
            var graph1 = new ArrayBidirectionalGraph<int, Edge<int>>(wrappedGraph1);
            Assert.Throws<KeyNotFoundException>(() => graph1.InEdge(vertex1, 0));

            wrappedGraph1.AddVertex(vertex1);
            wrappedGraph1.AddVertex(vertex2);
            graph1 = new ArrayBidirectionalGraph<int, Edge<int>>(wrappedGraph1);
            AssertIndexOutOfRange(() => graph1.InEdge(vertex1, 0));

            wrappedGraph1.AddEdge(new Edge<int>(1, 2));
            graph1 = new ArrayBidirectionalGraph<int, Edge<int>>(wrappedGraph1);
            AssertIndexOutOfRange(() => graph1.InEdge(vertex1, 5));

            var wrappedGraph2 = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            var graph2 = new ArrayBidirectionalGraph<TestVertex, Edge<TestVertex>>(wrappedGraph2);
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph2.InEdge(null, 0));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void InEdges()
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge32 = new Edge<int>(3, 2);
            var edge33 = new Edge<int>(3, 3);

            var wrappedGraph = new BidirectionalGraph<int, Edge<int>>();
            var graph = new ArrayBidirectionalGraph<int, Edge<int>>(wrappedGraph);
            AssertNoInEdge(graph, 1);
            AssertNoOutEdge(graph, 1);

            wrappedGraph.AddVertex(1);
            graph = new ArrayBidirectionalGraph<int, Edge<int>>(wrappedGraph);
            AssertNoInEdge(graph, 1);
            AssertNoOutEdge(graph, 1);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge14, edge24, edge32, edge33 });
            graph = new ArrayBidirectionalGraph<int, Edge<int>>(wrappedGraph);

            AssertHasOutEdges(graph, 1, new[] { edge12, edge13, edge14 });
            AssertHasOutEdges(graph, 2, new[] { edge24 });
            AssertHasOutEdges(graph, 3, new[] { edge32, edge33 });
            AssertNoOutEdge(graph, 4);

            AssertNoInEdge(graph, 1);
            AssertHasInEdges(graph, 2, new[] { edge12, edge32 });
            AssertHasInEdges(graph, 3, new[] { edge13, edge33 });
            AssertHasInEdges(graph, 4, new[] { edge14, edge24 });
        }

        [Test]
        public void InEdges_Throws()
        {
            var wrappedGraph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ArrayBidirectionalGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            InEdges_Throws_Test(graph);
        }

        #endregion

        [Test]
        public void Degree()
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(1, 4);
            var edge4 = new Edge<int>(2, 4);
            var edge5 = new Edge<int>(3, 2);
            var edge6 = new Edge<int>(3, 3);

            var wrappedGraph = new BidirectionalGraph<int, Edge<int>>();
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });
            wrappedGraph.AddVertex(5);
            var graph = new ArrayBidirectionalGraph<int, Edge<int>>(wrappedGraph);

            Assert.AreEqual(3, graph.Degree(1));
            Assert.AreEqual(3, graph.Degree(2));
            Assert.AreEqual(4, graph.Degree(3)); // Self edge
            Assert.AreEqual(2, graph.Degree(4));
            Assert.AreEqual(0, graph.Degree(5));
        }

        #region Try Get Edges

        [Test]
        public void TryGetEdge()
        {
            var wrappedGraph = new BidirectionalGraph<int, Edge<int>>();
            TryGetEdge_ImmutableGraph_Test(
                wrappedGraph,
                () => new ArrayBidirectionalGraph<int, Edge<int>>(wrappedGraph));
        }

        [Test]
        public void TryGetEdge_Throws()
        {
            var wrappedGraph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ArrayBidirectionalGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            TryGetEdge_Throws_Test(graph);
        }

        [Test]
        public void TryGetEdges()
        {
            var wrappedGraph = new BidirectionalGraph<int, Edge<int>>();
            TryGetEdges_ImmutableGraph_Test(
                wrappedGraph,
                () => new ArrayBidirectionalGraph<int, Edge<int>>(wrappedGraph));
        }

        [Test]
        public void TryGetEdges_Throws()
        {
            var wrappedGraph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ArrayBidirectionalGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            TryGetEdges_Throws_Test(graph);
        }

        [Test]
        public void TryGetOutEdges()
        {
            var wrappedGraph = new BidirectionalGraph<int, Edge<int>>();
            TryGetOutEdges_ImmutableGraph_Test(
                wrappedGraph,
                () => new ArrayBidirectionalGraph<int, Edge<int>>(wrappedGraph));
        }

        [Test]
        public void TryGetOutEdges_Throws()
        {
            var wrappedGraph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ArrayBidirectionalGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            TryGetOutEdges_Throws_Test(graph);
        }

        [Test]
        public void TryGetInEdges()
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);

            var wrappedGraph = new BidirectionalGraph<int, Edge<int>>();
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });
            var graph = new ArrayBidirectionalGraph<int, Edge<int>>(wrappedGraph);

            Assert.IsFalse(graph.TryGetInEdges(0, out IEnumerable<Edge<int>> _));

            Assert.IsTrue(graph.TryGetInEdges(4, out IEnumerable<Edge<int>> gotEdges));
            CollectionAssert.AreEqual(new[] { edge5 }, gotEdges);

            Assert.IsTrue(graph.TryGetInEdges(2, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge1, edge2, edge4 }, gotEdges);
        }

        [Test]
        public void TryGetInEdges_Throws()
        {
            var wrappedGraph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ArrayBidirectionalGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            TryGetInEdges_Throws_Test(graph);
        }

        #endregion

        [Test]
        public void Clone()
        {
            var wrappedGraph = new BidirectionalGraph<int, Edge<int>>();

            var graph = new ArrayBidirectionalGraph<int, Edge<int>>(wrappedGraph);
            AssertEmptyGraph(graph);

            var clonedGraph = graph.Clone();
            Assert.IsNotNull(clonedGraph);
            AssertEmptyGraph(clonedGraph);

            clonedGraph = (ArrayBidirectionalGraph<int, Edge<int>>)((ICloneable)graph).Clone();
            Assert.IsNotNull(clonedGraph);
            AssertEmptyGraph(clonedGraph);

            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 3);
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3 });

            graph = new ArrayBidirectionalGraph<int, Edge<int>>(wrappedGraph);
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge1, edge2, edge3 });

            clonedGraph = graph.Clone();
            Assert.IsNotNull(clonedGraph);
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3 });
            AssertHasEdges(clonedGraph, new[] { edge1, edge2, edge3 });

            clonedGraph = (ArrayBidirectionalGraph<int, Edge<int>>)((ICloneable)graph).Clone();
            Assert.IsNotNull(clonedGraph);
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3 });
            AssertHasEdges(clonedGraph, new[] { edge1, edge2, edge3 });
        }
    }
}
