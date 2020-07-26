using System;
using NUnit.Framework;
using static QuikGraph.Tests.GraphTestHelpers;

namespace QuikGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="ArrayUndirectedGraph{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class ArrayUndirectedGraphTests : GraphTestsBase
    {
        [Test]
        public void Construction()
        {
            EdgeEqualityComparer<int> comparer = (edge, source, target) =>
                edge.Source.Equals(source) && edge.Target.Equals(target);

            var wrappedGraph = new UndirectedGraph<int, Edge<int>>();

            var graph = new ArrayUndirectedGraph<int, Edge<int>>(wrappedGraph);
            AssertGraphProperties(graph);
            AssertEmptyGraph(graph);

            wrappedGraph.AddVertexRange(new[] { 2, 3, 1 });
            graph = new ArrayUndirectedGraph<int, Edge<int>>(wrappedGraph);
            AssertGraphProperties(graph);
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertNoEdge(graph);

            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(2, 2);
            var edge3 = new Edge<int>(3, 4);
            var edge4 = new Edge<int>(1, 4);
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4 });
            graph = new ArrayUndirectedGraph<int, Edge<int>>(wrappedGraph);
            AssertGraphProperties(graph);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge1, edge2, edge3, edge4 });

            wrappedGraph = new UndirectedGraph<int, Edge<int>>(false);
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge1, edge2, edge3, edge4 });
            graph = new ArrayUndirectedGraph<int, Edge<int>>(wrappedGraph);
            AssertGraphProperties(graph, false);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge1, edge2, edge3, edge4 });

            wrappedGraph = new UndirectedGraph<int, Edge<int>>(true, comparer);
            graph = new ArrayUndirectedGraph<int, Edge<int>>(wrappedGraph);
            AssertGraphProperties(graph);
            AssertEmptyGraph(graph);

            #region Local function

            void AssertGraphProperties<TVertex, TEdge>(
                ArrayUndirectedGraph<TVertex, TEdge> g,
                bool allowParallelEdges = true)
                where TEdge : IEdge<TVertex>
            {
                Assert.IsFalse(g.IsDirected);
                Assert.AreEqual(allowParallelEdges, g.AllowParallelEdges);
                Assert.AreSame(wrappedGraph.EdgeEqualityComparer, g.EdgeEqualityComparer);
            }

            #endregion
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new ArrayUndirectedGraph<int, Edge<int>>(null));
        }

        #region Add Vertex => no effect

        [Test]
        public void AddVertex()
        {
            var wrappedGraph = new UndirectedGraph<int, Edge<int>>();
            AddVertex_ImmutableGraph_NoUpdate(
                wrappedGraph,
                () => new ArrayUndirectedGraph<int, Edge<int>>(wrappedGraph));
        }

        #endregion

        #region Add Edge => no effect

        [Test]
        public void AddEdge()
        {
            var wrappedGraph = new UndirectedGraph<int, Edge<int>>();
            AddEdge_ImmutableGraph_NoUpdate(
                wrappedGraph,
                () => new ArrayUndirectedGraph<int, Edge<int>>(wrappedGraph));
        }

        #endregion

        #region Contains Vertex

        [Test]
        public void ContainsVertex()
        {
            var wrappedGraph = new UndirectedGraph<TestVertex, Edge<TestVertex>>();
            ContainsVertex_ImmutableGraph_Test(
                wrappedGraph,
                () => new ArrayUndirectedGraph<TestVertex, Edge<TestVertex>>(wrappedGraph));
        }

        [Test]
        public void ContainsVertex_EquatableVertex()
        {
            var wrappedGraph = new UndirectedGraph<EquatableTestVertex, Edge<EquatableTestVertex>>();
            ContainsVertex_EquatableVertex_ImmutableGraph_Test(
                wrappedGraph,
                () => new ArrayUndirectedGraph<EquatableTestVertex, Edge<EquatableTestVertex>>(wrappedGraph));
        }

        [Test]
        public void ContainsVertex_Throws()
        {
            var wrappedGraph = new UndirectedGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ArrayUndirectedGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            ContainsVertex_Throws_Test(graph);
        }

        #endregion

        #region Contains Edge

        [Test]
        public void ContainsEdge()
        {
            var wrappedGraph = new UndirectedGraph<int, Edge<int>>();
            ContainsEdge_ImmutableGraph_Test(
                wrappedGraph, 
                () => new ArrayUndirectedGraph<int, Edge<int>>(wrappedGraph));
        }

        [Test]
        public void ContainsEdge_EquatableEdge()
        {
            var wrappedGraph = new UndirectedGraph<int, EquatableEdge<int>>();
            ContainsEdge_EquatableEdge_ImmutableGraph_Test(
                wrappedGraph,
                () => new ArrayUndirectedGraph<int, EquatableEdge<int>>(wrappedGraph));
        }

        [Test]
        public void ContainsEdge_SourceTarget()
        {
            var wrappedGraph = new UndirectedGraph<int, Edge<int>>();
            ContainsEdge_SourceTarget_ImmutableGraph_UndirectedGraph_Test(
                wrappedGraph,
                () => new ArrayUndirectedGraph<int, Edge<int>>(wrappedGraph));
        }

        [Test]
        public void ContainsEdge_Throws()
        {
            var wrappedGraph = new UndirectedGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ArrayUndirectedGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            ContainsEdge_NullThrows_Test(graph);
            ContainsEdge_SourceTarget_Throws_UndirectedGraph_Test(graph);
        }

        [Test]
        public void ContainsEdge_Undirected()
        {
            var wrappedGraph1 = new UndirectedGraph<int, EquatableEdge<int>>();
            var wrappedGraph2 = new UndirectedGraph<int, EquatableUndirectedEdge<int>>();
            ContainsEdge_UndirectedEdge_ImmutableGraph_UndirectedGraph_Test(
                wrappedGraph1,
                () => new ArrayUndirectedGraph<int, EquatableEdge<int>>(wrappedGraph1), 
                wrappedGraph2,
                () => new ArrayUndirectedGraph<int, EquatableUndirectedEdge<int>>(wrappedGraph2));
        }

        #endregion

        #region Adjacent Edges

        [Test]
        public void AdjacentEdge()
        {
            var wrappedGraph = new UndirectedGraph<int, Edge<int>>();
            AdjacentEdge_ImmutableGraph_Test(
                wrappedGraph,
                () => new ArrayUndirectedGraph<int, Edge<int>>(wrappedGraph));
        }

        [Test]
        public void AdjacentEdge_Throws()
        {
            var wrappedGraph1 = new UndirectedGraph<int, Edge<int>>();
            AdjacentEdge_Throws_ImmutableGraph_Test(
                wrappedGraph1, 
                () => new ArrayUndirectedGraph<int, Edge<int>>(wrappedGraph1));

            var wrappedGraph2 = new UndirectedGraph<TestVertex, Edge<TestVertex>>();
            var graph2 = new ArrayUndirectedGraph<TestVertex, Edge<TestVertex>>(wrappedGraph2);
            AdjacentEdge_NullThrows_Test(graph2);
        }

        [Test]
        public void AdjacentEdges()
        {
            var wrappedGraph = new UndirectedGraph<int, Edge<int>>();
            AdjacentEdges_ImmutableGraph_Test(
                wrappedGraph,
                () => new ArrayUndirectedGraph<int, Edge<int>>(wrappedGraph));
        }

        [Test]
        public void AdjacentEdges_Throws()
        {
            var wrappedGraph = new UndirectedGraph<EquatableTestVertex, Edge<EquatableTestVertex>>();
            var graph = new ArrayUndirectedGraph<EquatableTestVertex, Edge<EquatableTestVertex>>(wrappedGraph);
            AdjacentEdges_Throws_Test(graph);
        }

        #endregion

        #region Try Get Edges

        [Test]
        public void TryGetEdge()
        {
            var wrappedGraph = new UndirectedGraph<int, Edge<int>>();
            TryGetEdge_ImmutableGraph_UndirectedGraph_Test(
                wrappedGraph,
                () => new ArrayUndirectedGraph<int, Edge<int>>(wrappedGraph));
        }

        [Test]
        public void TryGetEdge_Throws()
        {
            var wrappedGraph = new UndirectedGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ArrayUndirectedGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            TryGetEdge_Throws_UndirectedGraph_Test(graph);
        }

        #endregion

        [Test]
        public void Clone()
        {
            var wrappedGraph = new UndirectedGraph<int, Edge<int>>();

            var graph = new ArrayUndirectedGraph<int, Edge<int>>(wrappedGraph);
            AssertEmptyGraph(graph);

            var clonedGraph = graph.Clone();
            Assert.IsNotNull(clonedGraph);
            AssertEmptyGraph(clonedGraph);

            clonedGraph = (ArrayUndirectedGraph<int, Edge<int>>)((ICloneable)graph).Clone();
            Assert.IsNotNull(clonedGraph);
            AssertEmptyGraph(clonedGraph);

            wrappedGraph.AddVertexRange(new[] { 1, 2, 3 });
            graph = new ArrayUndirectedGraph<int, Edge<int>>(wrappedGraph);
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertNoEdge(graph);

            clonedGraph = graph.Clone();
            Assert.IsNotNull(clonedGraph);
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3 });
            AssertNoEdge(clonedGraph);

            clonedGraph = (ArrayUndirectedGraph<int, Edge<int>>)((ICloneable)graph).Clone();
            Assert.IsNotNull(clonedGraph);
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3 });
            AssertNoEdge(clonedGraph);

            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 3);
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3 });

            graph = new ArrayUndirectedGraph<int, Edge<int>>(wrappedGraph);
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge1, edge2, edge3 });

            clonedGraph = graph.Clone();
            Assert.IsNotNull(clonedGraph);
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3 });
            AssertHasEdges(clonedGraph, new[] { edge1, edge2, edge3 });

            clonedGraph = (ArrayUndirectedGraph<int, Edge<int>>)((ICloneable)graph).Clone();
            Assert.IsNotNull(clonedGraph);
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3 });
            AssertHasEdges(clonedGraph, new[] { edge1, edge2, edge3 });

            wrappedGraph.AddVertex(4);
            graph = new ArrayUndirectedGraph<int, Edge<int>>(wrappedGraph);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge1, edge2, edge3 });

            clonedGraph = graph.Clone();
            Assert.IsNotNull(clonedGraph);
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(clonedGraph, new[] { edge1, edge2, edge3 });

            clonedGraph = (ArrayUndirectedGraph<int, Edge<int>>)((ICloneable)graph).Clone();
            Assert.IsNotNull(clonedGraph);
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(clonedGraph, new[] { edge1, edge2, edge3 });
        }
    }
}