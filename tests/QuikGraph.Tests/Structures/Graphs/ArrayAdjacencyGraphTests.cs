using System;
using NUnit.Framework;
using static QuikGraph.Tests.GraphTestHelpers;

namespace QuikGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="ArrayAdjacencyGraph{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class ArrayAdjacencyGraphTests : GraphTestsBase
    {
        [Test]
        public void Construction()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();

            var graph = new ArrayAdjacencyGraph<int, Edge<int>>(wrappedGraph);
            AssertGraphProperties(graph);
            AssertEmptyGraph(graph);

            wrappedGraph.AddVertexRange(new[] { 2, 3, 1 });
            graph = new ArrayAdjacencyGraph<int, Edge<int>>(wrappedGraph);
            AssertGraphProperties(graph);
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertNoEdge(graph);

            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(2, 2);
            var edge3 = new Edge<int>(3, 4);
            var edge4 = new Edge<int>(1, 4);
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4 });
            graph = new ArrayAdjacencyGraph<int, Edge<int>>(wrappedGraph);
            AssertGraphProperties(graph);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge1, edge2, edge3, edge4 });

            wrappedGraph = new AdjacencyGraph<int, Edge<int>>(false);
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge1, edge2, edge3, edge4 });
            graph = new ArrayAdjacencyGraph<int, Edge<int>>(wrappedGraph);
            AssertGraphProperties(graph, false);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge1, edge2, edge3, edge4 });

            #region Local function

            void AssertGraphProperties<TVertex, TEdge>(
                ArrayAdjacencyGraph<TVertex, TEdge> g,
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
            Assert.Throws<ArgumentNullException>(() => new ArrayAdjacencyGraph<int, Edge<int>>(null));
        }

        #region Add Vertex => no effect

        [Test]
        public void AddVertex()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();
            AddVertex_ImmutableGraph_NoUpdate(
                wrappedGraph,
                () => new ArrayAdjacencyGraph<int, Edge<int>>(wrappedGraph));
        }

        #endregion

        #region Add Edge => no effect

        [Test]
        public void AddEdge()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();
            AddEdge_ImmutableGraph_NoUpdate(
                wrappedGraph,
                () => new ArrayAdjacencyGraph<int, Edge<int>>(wrappedGraph));
        }

        #endregion

        #region Contains Vertex

        [Test]
        public void ContainsVertex()
        {
            var wrappedGraph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            ContainsVertex_ImmutableGraph_Test(
                wrappedGraph,
                () => new ArrayAdjacencyGraph<TestVertex, Edge<TestVertex>>(wrappedGraph));
        }

        [Test]
        public void ContainsVertex_EquatableVertex()
        {
            var wrappedGraph = new AdjacencyGraph<EquatableTestVertex, Edge<EquatableTestVertex>>();
            ContainsVertex_EquatableVertex_ImmutableGraph_Test(
                wrappedGraph,
                () => new ArrayAdjacencyGraph<EquatableTestVertex, Edge<EquatableTestVertex>>(wrappedGraph));
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
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();
            ContainsEdge_ImmutableGraph_Test(
                wrappedGraph,
                () => new ArrayAdjacencyGraph<int, Edge<int>>(wrappedGraph));
        }

        [Test]
        public void ContainsEdge_EquatableEdge()
        {
            var wrappedGraph = new AdjacencyGraph<int, EquatableEdge<int>>();
            ContainsEdge_EquatableEdge_ImmutableGraph_Test(
                wrappedGraph,
                () => new ArrayAdjacencyGraph<int, EquatableEdge<int>>(wrappedGraph));
        }

        [Test]
        public void ContainsEdge_SourceTarget()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();
            ContainsEdge_SourceTarget_ImmutableGraph_Test(
                wrappedGraph,
                () => new ArrayAdjacencyGraph<int, Edge<int>>(wrappedGraph));
        }

        [Test]
        public void ContainsEdge_Throws()
        {
            var wrappedGraph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ArrayAdjacencyGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            ContainsEdge_NullThrows_Test(graph);
            ContainsEdge_SourceTarget_Throws_Test(graph);
        }

        #endregion

        #region Out Edges

        [Test]
        public void OutEdge()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();
            OutEdge_ImmutableGraph_Test(
                wrappedGraph,
                () => new ArrayAdjacencyGraph<int, Edge<int>>(wrappedGraph));
        }

        [Test]
        public void OutEdge_Throws()
        {
            var wrappedGraph1 = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph1 = new ArrayAdjacencyGraph<TestVertex, Edge<TestVertex>>(wrappedGraph1);
            OutEdge_NullThrows_Test(graph1);

            var wrappedGraph2 = new AdjacencyGraph<int, Edge<int>>();
            OutEdge_Throws_ImmutableGraph_Test(
                wrappedGraph2,
                () => new ArrayAdjacencyGraph<int, Edge<int>>(wrappedGraph2));
        }

        [Test]
        public void OutEdges()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();
            OutEdges_ImmutableGraph_Test(
                wrappedGraph,
                () => new ArrayAdjacencyGraph<int, Edge<int>>(wrappedGraph));
        }

        [Test]
        public void OutEdges_Throws()
        {
            var wrappedGraph1 = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph1 = new ArrayAdjacencyGraph<TestVertex, Edge<TestVertex>>(wrappedGraph1);
            OutEdges_NullThrows_Test(graph1);

            var wrappedGraph2 = new AdjacencyGraph<EquatableTestVertex, Edge<EquatableTestVertex>>();
            var graph2 = new ArrayAdjacencyGraph<EquatableTestVertex, Edge<EquatableTestVertex>>(wrappedGraph2);
            OutEdges_Throws_Test(graph2);
        }

        #endregion

        #region Try Get Edges

        [Test]
        public void TryGetEdge()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();
            TryGetEdge_ImmutableGraph_Test(
                wrappedGraph,
                () => new ArrayAdjacencyGraph<int, Edge<int>>(wrappedGraph));
        }

        [Test]
        public void TryGetEdge_Throws()
        {
            var wrappedGraph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ArrayAdjacencyGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            TryGetEdge_Throws_Test(graph);
        }

        [Test]
        public void TryGetEdges()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();
            TryGetEdges_ImmutableGraph_Test(
                wrappedGraph,
                () => new ArrayAdjacencyGraph<int, Edge<int>>(wrappedGraph));
        }

        [Test]
        public void TryGetEdges_Throws()
        {
            var wrappedGraph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ArrayAdjacencyGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            TryGetEdges_Throws_Test(graph);
        }

        [Test]
        public void TryGetOutEdges()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();
            TryGetOutEdges_ImmutableGraph_Test(
                wrappedGraph,
                () => new ArrayAdjacencyGraph<int, Edge<int>>(wrappedGraph));
        }

        [Test]
        public void TryGetOutEdges_Throws()
        {
            var wrappedGraph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ArrayAdjacencyGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            TryGetOutEdges_Throws_Test(graph);
        }

        #endregion

        [Test]
        public void Clone()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();

            var graph = new ArrayAdjacencyGraph<int, Edge<int>>(wrappedGraph);
            AssertEmptyGraph(graph);

            var clonedGraph = graph.Clone();
            Assert.IsNotNull(clonedGraph);
            AssertEmptyGraph(clonedGraph);

            clonedGraph = (ArrayAdjacencyGraph<int, Edge<int>>)((ICloneable)graph).Clone();
            Assert.IsNotNull(clonedGraph);
            AssertEmptyGraph(clonedGraph);

            wrappedGraph.AddVertexRange(new[] { 1, 2, 3 });
            graph = new ArrayAdjacencyGraph<int, Edge<int>>(wrappedGraph);
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertNoEdge(graph);

            clonedGraph = graph.Clone();
            Assert.IsNotNull(clonedGraph);
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3 });
            AssertNoEdge(clonedGraph);

            clonedGraph = (ArrayAdjacencyGraph<int, Edge<int>>)((ICloneable)graph).Clone();
            Assert.IsNotNull(clonedGraph);
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3 });
            AssertNoEdge(clonedGraph);

            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 3);
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3 });

            graph = new ArrayAdjacencyGraph<int, Edge<int>>(wrappedGraph);
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge1, edge2, edge3 });

            clonedGraph = graph.Clone();
            Assert.IsNotNull(clonedGraph);
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3 });
            AssertHasEdges(clonedGraph, new[] { edge1, edge2, edge3 });

            clonedGraph = (ArrayAdjacencyGraph<int, Edge<int>>)((ICloneable)graph).Clone();
            Assert.IsNotNull(clonedGraph);
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3 });
            AssertHasEdges(clonedGraph, new[] { edge1, edge2, edge3 });

            wrappedGraph.AddVertex(4);
            graph = new ArrayAdjacencyGraph<int, Edge<int>>(wrappedGraph);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge1, edge2, edge3 });

            clonedGraph = graph.Clone();
            Assert.IsNotNull(clonedGraph);
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(clonedGraph, new[] { edge1, edge2, edge3 });

            clonedGraph = (ArrayAdjacencyGraph<int, Edge<int>>)((ICloneable)graph).Clone();
            Assert.IsNotNull(clonedGraph);
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(clonedGraph, new[] { edge1, edge2, edge3 });
        }
    }
}