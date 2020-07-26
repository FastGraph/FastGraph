using System;
using NUnit.Framework;
using static QuikGraph.Tests.GraphTestHelpers;

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

        #region Add Vertex => no effect

        [Test]
        public void AddVertex()
        {
            var wrappedGraph = new BidirectionalGraph<int, Edge<int>>();
            AddVertex_ImmutableGraph_NoUpdate(
                wrappedGraph,
                () => new ArrayBidirectionalGraph<int, Edge<int>>(wrappedGraph));
        }

        #endregion

        #region Add Edge => no effect

        [Test]
        public void AddEdge()
        {
            var wrappedGraph = new BidirectionalGraph<int, Edge<int>>();
            AddEdge_ImmutableGraph_NoUpdate(
                wrappedGraph,
                () => new ArrayBidirectionalGraph<int, Edge<int>>(wrappedGraph));
        }

        #endregion

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
            var wrappedGraph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ArrayBidirectionalGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
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
            ContainsEdge_NullThrows_Test(graph);
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
            var wrappedGraph1 = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            var graph1 = new ArrayBidirectionalGraph<TestVertex, Edge<TestVertex>>(wrappedGraph1);
            OutEdge_NullThrows_Test(graph1);

            var wrappedGraph2 = new BidirectionalGraph<int, Edge<int>>();
            OutEdge_Throws_ImmutableGraph_Test(
                wrappedGraph2,
                () => new ArrayBidirectionalGraph<int, Edge<int>>(wrappedGraph2));
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
            var wrappedGraph1 = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            var graph1 = new ArrayBidirectionalGraph<TestVertex, Edge<TestVertex>>(wrappedGraph1);
            OutEdges_NullThrows_Test(graph1);

            var wrappedGraph2 = new BidirectionalGraph<EquatableTestVertex, Edge<EquatableTestVertex>>();
            var graph2 = new ArrayBidirectionalGraph<EquatableTestVertex, Edge<EquatableTestVertex>>(wrappedGraph2);
            OutEdges_Throws_Test(graph2);
        }

        #endregion

        #region In Edges

        [Test]
        public void InEdge()
        {
            var wrappedGraph = new BidirectionalGraph<int, Edge<int>>();
            InEdge_ImmutableGraph_Test(
                wrappedGraph,
                () => new ArrayBidirectionalGraph<int, Edge<int>>(wrappedGraph));
        }

        [Test]
        public void InEdge_Throws()
        {
            var wrappedGraph1 = new BidirectionalGraph<int, Edge<int>>();
            InEdge_Throws_ImmutableGraph_Test(
                wrappedGraph1,
                () => new ArrayBidirectionalGraph<int, Edge<int>>(wrappedGraph1));

            var wrappedGraph2 = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            var graph2 = new ArrayBidirectionalGraph<TestVertex, Edge<TestVertex>>(wrappedGraph2);
            InEdge_NullThrows_Test(graph2);
        }

        [Test]
        public void InEdges()
        {
            var wrappedGraph = new BidirectionalGraph<int, Edge<int>>();
            InEdges_ImmutableGraph_Test(
                wrappedGraph,
                () => new ArrayBidirectionalGraph<int, Edge<int>>(wrappedGraph));
        }

        [Test]
        public void InEdges_Throws()
        {
            var wrappedGraph1 = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            var graph1 = new ArrayBidirectionalGraph<TestVertex, Edge<TestVertex>>(wrappedGraph1);
            InEdges_NullThrows_Test(graph1);

            var wrappedGraph2 = new BidirectionalGraph<EquatableTestVertex, Edge<EquatableTestVertex>>();
            var graph2 = new ArrayBidirectionalGraph<EquatableTestVertex, Edge<EquatableTestVertex>>(wrappedGraph2);
            InEdges_Throws_Test(graph2);
        }

        #endregion

        [Test]
        public void Degree()
        {
            var wrappedGraph = new BidirectionalGraph<int, Edge<int>>();
            Degree_ImmutableGraph_Test(
                wrappedGraph,
                () => new ArrayBidirectionalGraph<int, Edge<int>>(wrappedGraph));
        }

        [Test]
        public void Degree_Throws()
        {
            var wrappedGraph = new BidirectionalGraph<EquatableTestVertex, Edge<EquatableTestVertex>>();
            var graph = new ArrayBidirectionalGraph<EquatableTestVertex, Edge<EquatableTestVertex>>(wrappedGraph);
            Degree_Throws_Test(graph);
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
            var wrappedGraph = new BidirectionalGraph<int, Edge<int>>();
            TryGetInEdges_ImmutableGraph_Test(
                wrappedGraph,
                () => new ArrayBidirectionalGraph<int, Edge<int>>(wrappedGraph));
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

            wrappedGraph.AddVertexRange(new[] { 1, 2, 3 });
            graph = new ArrayBidirectionalGraph<int, Edge<int>>(wrappedGraph);
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertNoEdge(graph);

            clonedGraph = graph.Clone();
            Assert.IsNotNull(clonedGraph);
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3 });
            AssertNoEdge(clonedGraph);

            clonedGraph = (ArrayBidirectionalGraph<int, Edge<int>>)((ICloneable)graph).Clone();
            Assert.IsNotNull(clonedGraph);
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3 });
            AssertNoEdge(clonedGraph);

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

            wrappedGraph.AddVertex(4);
            graph = new ArrayBidirectionalGraph<int, Edge<int>>(wrappedGraph);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge1, edge2, edge3 });

            clonedGraph = graph.Clone();
            Assert.IsNotNull(clonedGraph);
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(clonedGraph, new[] { edge1, edge2, edge3 });

            clonedGraph = (ArrayBidirectionalGraph<int, Edge<int>>)((ICloneable)graph).Clone();
            Assert.IsNotNull(clonedGraph);
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(clonedGraph, new[] { edge1, edge2, edge3 });
        }
    }
}