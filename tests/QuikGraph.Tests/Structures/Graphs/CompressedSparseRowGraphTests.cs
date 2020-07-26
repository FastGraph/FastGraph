using System;
using NUnit.Framework;
using static QuikGraph.Tests.GraphTestHelpers;

namespace QuikGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="CompressedSparseRowGraph{TVertex}"/>.
    /// </summary>
    [TestFixture]
    internal class CompressedSparseRowGraphTests : GraphTestsBase
    {
        [Test]
        public void Construction()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();
            var graph = CompressedSparseRowGraph<int>.FromGraph(wrappedGraph);
            AssertGraphProperties(graph);
            AssertEmptyGraph(graph);

            wrappedGraph.AddVertexRange(new[] { 1, 2, 3 });
            graph = CompressedSparseRowGraph<int>.FromGraph(wrappedGraph);
            AssertGraphProperties(graph);
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertNoEdge(graph);

            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 1);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 3);
            wrappedGraph.AddEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5 });
            graph = CompressedSparseRowGraph<int>.FromGraph(wrappedGraph);
            AssertGraphProperties(graph);
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(
                graph,
                new[]
                {
                    new SEquatableEdge<int>(1, 2),
                    new SEquatableEdge<int>(1, 3),
                    new SEquatableEdge<int>(2, 1),
                    new SEquatableEdge<int>(2, 2),
                    new SEquatableEdge<int>(2, 3)
                });

            var edge6 = new Edge<int>(3, 1);
            var edge7 = new Edge<int>(3, 2);
            wrappedGraph.Clear();
            wrappedGraph.AddVertexRange(new[] { 1, 2, 3, 4 });
            wrappedGraph.AddEdgeRange(new[] { edge1, edge2, edge6, edge7 });
            graph = CompressedSparseRowGraph<int>.FromGraph(wrappedGraph);
            AssertGraphProperties(graph);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(
                graph,
                new[]
                {
                    new SEquatableEdge<int>(1, 2),
                    new SEquatableEdge<int>(1, 3),
                    new SEquatableEdge<int>(3, 1),
                    new SEquatableEdge<int>(3, 2)
                });

            #region Local function

            void AssertGraphProperties<TVertex>(
                // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
                CompressedSparseRowGraph<TVertex> g)
            {
                Assert.IsTrue(g.IsDirected);
                Assert.IsFalse(g.AllowParallelEdges);
            }

            #endregion
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => CompressedSparseRowGraph<int>.FromGraph<Edge<int>>(null));
        }

        #region Contains Vertex

        [Test]
        public void ContainsVertex()
        {
            var wrappedGraph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            ContainsVertex_ImmutableGraph_Test(
                wrappedGraph,
                () => CompressedSparseRowGraph<TestVertex>.FromGraph(wrappedGraph));
        }

        [Test]
        public void ContainsVertex_EquatableVertex()
        {
            var wrappedGraph = new AdjacencyGraph<EquatableTestVertex, Edge<EquatableTestVertex>>();
            ContainsVertex_EquatableVertex_ImmutableGraph_Test(
                wrappedGraph,
                () => CompressedSparseRowGraph<EquatableTestVertex>.FromGraph(wrappedGraph));
        }

        [Test]
        public void ContainsVertex_Throws()
        {
            var wrappedGraph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph = CompressedSparseRowGraph<TestVertex>.FromGraph(wrappedGraph);
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
                () => CompressedSparseRowGraph<int>.FromGraph(wrappedGraph));
        }

        [Test]
        public void ContainsEdge_SourceTarget()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();
            ContainsEdge_SourceTarget_ImmutableGraph_Test(
                wrappedGraph,
                () => CompressedSparseRowGraph<int>.FromGraph(wrappedGraph));
        }

        [Test]
        public void ContainsEdge_Throws()
        {
            var wrappedGraph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph = CompressedSparseRowGraph<TestVertex>.FromGraph(wrappedGraph);
            ContainsEdge_DefaultNullThrows_Test(graph);
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
                () => CompressedSparseRowGraph<int>.FromGraph(wrappedGraph));
        }

        [Test]
        public void OutEdge_Throws()
        {
            var wrappedGraph1 = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph1 = CompressedSparseRowGraph<TestVertex>.FromGraph(wrappedGraph1);
            OutEdge_NullThrows_Test(graph1);

            var wrappedGraph2 = new AdjacencyGraph<int, Edge<int>>();
            OutEdge_Throws_ImmutableGraph_Test(
                wrappedGraph2,
                () => CompressedSparseRowGraph<int>.FromGraph(wrappedGraph2));
        }

        [Test]
        public void OutEdges()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();
            OutEdges_ImmutableGraph_Test(
                wrappedGraph,
                () => CompressedSparseRowGraph<int>.FromGraph(wrappedGraph));
        }

        [Test]
        public void OutEdges_Throws()
        {
            var wrappedGraph1 = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph1 = CompressedSparseRowGraph<TestVertex>.FromGraph(wrappedGraph1);
            OutEdges_NullThrows_Test(graph1);

            var wrappedGraph2 = new AdjacencyGraph<EquatableTestVertex, Edge<EquatableTestVertex>>();
            var graph2 = CompressedSparseRowGraph<EquatableTestVertex>.FromGraph(wrappedGraph2);
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
                () => CompressedSparseRowGraph<int>.FromGraph(wrappedGraph));
        }

        [Test]
        public void TryGetEdge_Throws()
        {
            var wrappedGraph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph = CompressedSparseRowGraph<TestVertex>.FromGraph(wrappedGraph);
            TryGetEdge_Throws_Test(graph);
        }

        [Test]
        public void TryGetEdges()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();
            TryGetEdges_ImmutableGraph_Test(
                wrappedGraph,
                () => CompressedSparseRowGraph<int>.FromGraph(wrappedGraph));
        }

        [Test]
        public void TryGetEdges_Throws()
        {
            var wrappedGraph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph = CompressedSparseRowGraph<TestVertex>.FromGraph(wrappedGraph);
            TryGetEdges_Throws_Test(graph);
        }

        [Test]
        public void TryGetOutEdges()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();
            TryGetOutEdges_ImmutableGraph_Test(
                wrappedGraph,
                () => CompressedSparseRowGraph<int>.FromGraph(wrappedGraph));
        }

        [Test]
        public void TryGetOutEdges_Throws()
        {
            var wrappedGraph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph = CompressedSparseRowGraph<TestVertex>.FromGraph(wrappedGraph);
            TryGetOutEdges_Throws_Test(graph);
        }

        #endregion

        [Test]
        public void Clone()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();
            var graph = CompressedSparseRowGraph<int>.FromGraph(wrappedGraph);
            AssertEmptyGraph(graph);

            var clonedGraph = graph.Clone();
            Assert.IsNotNull(clonedGraph);
            AssertEmptyGraph(clonedGraph);

            clonedGraph = (CompressedSparseRowGraph<int>)((ICloneable)graph).Clone();
            Assert.IsNotNull(clonedGraph);
            AssertEmptyGraph(clonedGraph);

            wrappedGraph.AddVertexRange(new[] { 1, 2, 3 });
            graph = CompressedSparseRowGraph<int>.FromGraph(wrappedGraph);
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertNoEdge(graph);

            clonedGraph = graph.Clone();
            Assert.IsNotNull(clonedGraph);
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3 });
            AssertNoEdge(clonedGraph);

            clonedGraph = (CompressedSparseRowGraph<int>)((ICloneable)graph).Clone();
            Assert.IsNotNull(clonedGraph);
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3 });
            AssertNoEdge(clonedGraph);

            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 3);
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3 });
            graph = CompressedSparseRowGraph<int>.FromGraph(wrappedGraph);
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(
                graph,
                new[]
                {
                    new SEquatableEdge<int>(1, 2),
                    new SEquatableEdge<int>(1, 3),
                    new SEquatableEdge<int>(2, 3)
                });

            clonedGraph = graph.Clone();
            Assert.IsNotNull(clonedGraph);
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3 });
            AssertHasEdges(
                clonedGraph,
                new[]
                {
                    new SEquatableEdge<int>(1, 2),
                    new SEquatableEdge<int>(1, 3),
                    new SEquatableEdge<int>(2, 3)
                });

            clonedGraph = (CompressedSparseRowGraph<int>)((ICloneable)graph).Clone();
            Assert.IsNotNull(clonedGraph);
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3 });
            AssertHasEdges(
                clonedGraph,
                new[]
                {
                    new SEquatableEdge<int>(1, 2),
                    new SEquatableEdge<int>(1, 3),
                    new SEquatableEdge<int>(2, 3)
                });

            wrappedGraph.AddVertex(4);
            graph = CompressedSparseRowGraph<int>.FromGraph(wrappedGraph);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 }); AssertHasEdges(
                graph,
                new[]
                {
                    new SEquatableEdge<int>(1, 2),
                    new SEquatableEdge<int>(1, 3),
                    new SEquatableEdge<int>(2, 3)
                });

            clonedGraph = graph.Clone();
            Assert.IsNotNull(clonedGraph);
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3, 4 }); AssertHasEdges(
                clonedGraph,
                new[]
                {
                    new SEquatableEdge<int>(1, 2),
                    new SEquatableEdge<int>(1, 3),
                    new SEquatableEdge<int>(2, 3)
                });

            clonedGraph = (CompressedSparseRowGraph<int>)((ICloneable)graph).Clone();
            Assert.IsNotNull(clonedGraph);
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3, 4 }); AssertHasEdges(
                clonedGraph,
                new[]
                {
                    new SEquatableEdge<int>(1, 2),
                    new SEquatableEdge<int>(1, 3),
                    new SEquatableEdge<int>(2, 3)
                });
        }
    }
}