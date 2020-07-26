using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NUnit.Framework;
using static QuikGraph.Tests.GraphTestHelpers;

namespace QuikGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="ClusteredAdjacencyGraph{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class ClusteredAdjacencyGraphTests : GraphTestsBase
    {
        [Test]
        public void Construction()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>(true);
            var graph = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph);
            AssertGraphProperties(graph);

            wrappedGraph = new AdjacencyGraph<int, Edge<int>>(false);
            graph = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph);
            AssertGraphProperties(graph, false);

            wrappedGraph = new AdjacencyGraph<int, Edge<int>>(true, 12, 25);
            graph = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph);
            AssertGraphProperties(graph, edgeCapacity: 25);

            wrappedGraph = new AdjacencyGraph<int, Edge<int>>(false, 12, 52);
            graph = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph);
            AssertGraphProperties(graph, false, 52);

            wrappedGraph = new AdjacencyGraph<int, Edge<int>>(false, 12);
            graph = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph) { EdgeCapacity = 25 };
            AssertGraphProperties(graph, false, 25);

            var subGraph = new ClusteredAdjacencyGraph<int, Edge<int>>(graph);
            AssertGraphProperties(subGraph, false, parent: graph);

            #region Local function

            void AssertGraphProperties<TVertex, TEdge>(
                ClusteredAdjacencyGraph<TVertex, TEdge> g,
                bool parallelEdges = true,
                int edgeCapacity = 0,
                ClusteredAdjacencyGraph<int, Edge<int>> parent = null)
                where TEdge : IEdge<TVertex>
            {
                Assert.IsTrue(g.IsDirected);
                Assert.AreEqual(parallelEdges, g.AllowParallelEdges);
                AssertEmptyGraph(g);
                Assert.AreEqual(edgeCapacity, g.EdgeCapacity);
                Assert.AreSame(typeof(int), g.VertexType);
                Assert.AreSame(typeof(Edge<int>), g.EdgeType);
                if (parent is null)
                    Assert.IsNull(g.Parent);
                else
                    Assert.AreSame(parent, g.Parent);
            }

            #endregion
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() =>
                new ClusteredAdjacencyGraph<int, Edge<int>>((AdjacencyGraph<int, Edge<int>>)null));

            Assert.Throws<ArgumentNullException>(() =>
                new ClusteredAdjacencyGraph<int, Edge<int>>((ClusteredAdjacencyGraph<int, Edge<int>>)null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        #region Add Vertices

        [Test]
        public void AddVertex()
        {
            var wrappedGraph1 = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph1 = new ClusteredAdjacencyGraph<TestVertex, Edge<TestVertex>>(wrappedGraph1);

            var wrappedGraph2 = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph2 = new ClusteredAdjacencyGraph<TestVertex, Edge<TestVertex>>(wrappedGraph2);
            var subGraph2 = new ClusteredAdjacencyGraph<TestVertex, Edge<TestVertex>>(graph2);

            AddVertex_Clusters_Test(
                graph1,
                graph2,
                subGraph2);
        }

        [Test]
        public void AddVertex_Throws()
        {
            var wrappedGraph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ClusteredAdjacencyGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            AddVertex_Throws_Clusters_Test(graph);

            var subGraph = new ClusteredAdjacencyGraph<TestVertex, Edge<TestVertex>>(graph);
            AddVertex_Throws_Clusters_Test(subGraph);
        }

        [Test]
        public void AddVertex_EquatableVertex()
        {
            var wrappedGraph1 = new AdjacencyGraph<EquatableTestVertex, Edge<EquatableTestVertex>>();
            var graph1 = new ClusteredAdjacencyGraph<EquatableTestVertex, Edge<EquatableTestVertex>>(wrappedGraph1);

            var wrappedGraph2 = new AdjacencyGraph<EquatableTestVertex, Edge<EquatableTestVertex>>();
            var graph2 = new ClusteredAdjacencyGraph<EquatableTestVertex, Edge<EquatableTestVertex>>(wrappedGraph2);
            var subGraph2 = new ClusteredAdjacencyGraph<EquatableTestVertex, Edge<EquatableTestVertex>>(graph2);

            AddVertex_EquatableVertex_Clusters_Test(
                graph1,
                graph2,
                subGraph2);
        }

        [Test]
        public void AddVertexRange()
        {
            var wrappedGraph1 = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph1 = new ClusteredAdjacencyGraph<TestVertex, Edge<TestVertex>>(wrappedGraph1);

            var wrappedGraph2 = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph2 = new ClusteredAdjacencyGraph<TestVertex, Edge<TestVertex>>(wrappedGraph2);
            var subGraph2 = new ClusteredAdjacencyGraph<TestVertex, Edge<TestVertex>>(graph2);

            AddVertexRange_Clusters_Test(
                graph1,
                graph2,
                subGraph2);
        }

        [Test]
        public void AddVertexRange_Throws()
        {
            var wrappedGraph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ClusteredAdjacencyGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            AddVertexRange_Throws_Clusters_Test(graph);

            var subGraph = new ClusteredAdjacencyGraph<TestVertex, Edge<TestVertex>>(graph);
            AddVertexRange_Throws_Clusters_Test(subGraph);
        }

        #endregion

        #region Add Edges

        [Test]
        public void AddEdge_ParallelEdges()
        {
            var wrappedGraph1 = new AdjacencyGraph<int, Edge<int>>();
            var graph1 = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph1);

            var wrappedGraph2 = new AdjacencyGraph<int, Edge<int>>();
            var graph2 = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph2);
            var subGraph2 = new ClusteredAdjacencyGraph<int, Edge<int>>(graph2);

            AddEdge_ParallelEdges_Clusters_Test(
                graph1,
                graph2,
                subGraph2);
        }

        [Test]
        public void AddEdge_ParallelEdges_EquatableEdge()
        {
            var wrappedGraph1 = new AdjacencyGraph<int, EquatableEdge<int>>();
            var graph1 = new ClusteredAdjacencyGraph<int, EquatableEdge<int>>(wrappedGraph1);

            var wrappedGraph2 = new AdjacencyGraph<int, EquatableEdge<int>>();
            var graph2 = new ClusteredAdjacencyGraph<int, EquatableEdge<int>>(wrappedGraph2);
            var subGraph2 = new ClusteredAdjacencyGraph<int, EquatableEdge<int>>(graph2);

            AddEdge_ParallelEdges_EquatableEdge_Clusters_Test(
                graph1,
                graph2,
                subGraph2);
        }

        [Test]
        public void AddEdge_NoParallelEdges()
        {
            var wrappedGraph1 = new AdjacencyGraph<int, Edge<int>>(false);
            var graph1 = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph1);

            var wrappedGraph2 = new AdjacencyGraph<int, Edge<int>>(false);
            var graph2 = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph2);
            var subGraph2 = new ClusteredAdjacencyGraph<int, Edge<int>>(graph2);

            AddEdge_NoParallelEdges_Clusters_Test(
                graph1,
                graph2,
                subGraph2);
        }

        [Test]
        public void AddEdge_NoParallelEdges_EquatableEdge()
        {
            var wrappedGraph1 = new AdjacencyGraph<int, EquatableEdge<int>>(false);
            var graph1 = new ClusteredAdjacencyGraph<int, EquatableEdge<int>>(wrappedGraph1);

            var wrappedGraph2 = new AdjacencyGraph<int, EquatableEdge<int>>(false);
            var graph2 = new ClusteredAdjacencyGraph<int, EquatableEdge<int>>(wrappedGraph2);
            var subGraph2 = new ClusteredAdjacencyGraph<int, EquatableEdge<int>>(graph2);

            AddEdge_NoParallelEdges_EquatableEdge_Clusters_Test(
                graph1,
                graph2,
                subGraph2);
        }

        [Test]
        public void AddEdge_Throws()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();
            var graph = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph);
            AddEdge_Throws_Clusters_Test(graph);

            var subGraph = new ClusteredAdjacencyGraph<int, Edge<int>>(graph);
            AddEdge_Throws_Clusters_Test(subGraph);
        }

        [Test]
        public void AddEdgeRange()
        {
            var wrappedGraph1 = new AdjacencyGraph<int, Edge<int>>(false);
            var graph1 = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph1);

            var wrappedGraph2 = new AdjacencyGraph<int, Edge<int>>(false);
            var graph2 = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph2);
            var subGraph2 = new ClusteredAdjacencyGraph<int, Edge<int>>(graph2);

            AddEdgeRange_Clusters_Test(
                graph1,
                graph2,
                subGraph2);
        }

        [Test]
        public void AddEdgeRange_Throws()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();
            var graph = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph);
            AddEdgeRange_Throws_Clusters_Test(graph);

            var subGraph = new ClusteredAdjacencyGraph<int, Edge<int>>(graph);
            AddEdgeRange_Throws_Clusters_Test(subGraph);
        }

        #endregion

        #region Add Vertices & Edges

        [Test]
        public void AddVerticesAndEdge()
        {
            var wrappedGraph1 = new AdjacencyGraph<int, Edge<int>>();
            var graph1 = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph1);

            var wrappedGraph2 = new AdjacencyGraph<int, Edge<int>>();
            var graph2 = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph2);
            var subGraph2 = new ClusteredAdjacencyGraph<int, Edge<int>>(graph2);

            AddVerticesAndEdge_Clusters_Test(
                graph1,
                graph2,
                subGraph2);
        }

        [Test]
        public void AddVerticesAndEdge_Throws()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();
            var graph = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph);
            AddVerticesAndEdge_Throws_Clusters_Test(graph);

            var subGraph = new ClusteredAdjacencyGraph<int, Edge<int>>(graph);
            AddVerticesAndEdge_Throws_Clusters_Test(subGraph);
        }

        [Test]
        public void AddVerticesAndEdgeRange()
        {
            var wrappedGraph1 = new AdjacencyGraph<int, Edge<int>>(false);
            var graph1 = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph1);

            var wrappedGraph2 = new AdjacencyGraph<int, Edge<int>>(false);
            var graph2 = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph2);
            var subGraph2 = new ClusteredAdjacencyGraph<int, Edge<int>>(graph2);

            AddVerticesAndEdgeRange_Clusters_Test(
                graph1,
                graph2,
                subGraph2);
        }

        [Test]
        public void AddVerticesAndEdgeRange_Throws()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();
            var graph = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph);
            AddVerticesAndEdgeRange_Throws_Clusters_Test(graph);

            var subGraph = new ClusteredAdjacencyGraph<int, Edge<int>>(graph);
            AddVerticesAndEdgeRange_Throws_Clusters_Test(subGraph);
        }

        #endregion

        #region Contains Vertex

        [Test]
        public void ContainsVertex()
        {
            var wrappedGraph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            ContainsVertex_ImmutableGraph_Test(
                wrappedGraph,
                () => new ClusteredAdjacencyGraph<TestVertex, Edge<TestVertex>>(wrappedGraph));
        }

        [Test]
        public void ContainsVertex_EquatableVertex()
        {
            var wrappedGraph = new AdjacencyGraph<EquatableTestVertex, Edge<EquatableTestVertex>>();
            ContainsVertex_EquatableVertex_ImmutableGraph_Test(
                wrappedGraph,
                () => new ClusteredAdjacencyGraph<EquatableTestVertex, Edge<EquatableTestVertex>>(wrappedGraph));
        }

        [Test]
        public void ContainsVertex_Throws()
        {
            var wrappedGraph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ClusteredAdjacencyGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            ContainsVertex_Throws_Test(graph);
        }

        #endregion

        #region Contains Edge

        [Test]
        public void ContainsEdge()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();
            var graph = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph);
            ContainsEdge_Test(
                graph,
                edge => graph.AddVerticesAndEdge(edge));
        }

        [Test]
        public void ContainsEdge_EquatableEdge()
        {
            var wrappedGraph = new AdjacencyGraph<int, EquatableEdge<int>>();
            var graph = new ClusteredAdjacencyGraph<int, EquatableEdge<int>>(wrappedGraph);
            ContainsEdge_EquatableEdge_Test(
                graph,
                edge => graph.AddVerticesAndEdge(edge));
        }

        [Test]
        public void ContainsEdge_SourceTarget()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            ContainsEdge_SourceTarget_Test(
                graph,
                edge => graph.AddVerticesAndEdge(edge));
        }

        [Test]
        public void ContainsEdge_Throws()
        {
            var wrappedGraph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ClusteredAdjacencyGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            ContainsEdge_NullThrows_Test(graph);
            ContainsEdge_SourceTarget_Throws_Test(graph);
        }

        #endregion

        #region Out Edges

        [Test]
        public void OutEdge()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            OutEdge_Test(
                graph,
                edges => graph.AddVerticesAndEdgeRange(edges));
        }

        [Test]
        public void OutEdge_Throws()
        {
            var wrappedGraph1 = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph1 = new ClusteredAdjacencyGraph<TestVertex, Edge<TestVertex>>(wrappedGraph1);
            OutEdge_NullThrows_Test(graph1);

            var wrappedGraph2 = new AdjacencyGraph<int, Edge<int>>();
            var graph2 = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph2);
            OutEdge_Throws_Test(
                graph2,
                vertex => graph2.AddVertex(vertex),
                edge => graph2.AddEdge(edge));
        }

        [Test]
        public void OutEdges()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();
            var graph = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph);
            OutEdges_Test(
                graph,
                vertex => graph.AddVertex(vertex),
                edges => graph.AddVerticesAndEdgeRange(edges));
        }

        [Test]
        public void OutEdges_Throws()
        {
            var wrappedGraph1 = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph1 = new ClusteredAdjacencyGraph<TestVertex, Edge<TestVertex>>(wrappedGraph1);
            OutEdges_NullThrows_Test(graph1);

            var wrappedGraph2 = new AdjacencyGraph<EquatableTestVertex, Edge<EquatableTestVertex>>();
            var graph2 = new ClusteredAdjacencyGraph<EquatableTestVertex, Edge<EquatableTestVertex>>(wrappedGraph2);
            OutEdges_Throws_Test(graph2);
        }

        #endregion

        #region Try Get Edges

        [Test]
        public void TryGetEdge()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();
            var graph = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph);
            TryGetEdge_Test(
                graph,
                edges => graph.AddVerticesAndEdgeRange(edges));
        }

        [Test]
        public void TryGetEdge_Throws()
        {
            var wrappedGraph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ClusteredAdjacencyGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            TryGetEdge_Throws_Test(graph);
        }

        [Test]
        public void TryGetEdges()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();
            var graph = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph);
            TryGetEdges_Test(
                graph,
                edges => graph.AddVerticesAndEdgeRange(edges));
        }

        [Test]
        public void TryGetEdges_Throws()
        {
            var wrappedGraph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ClusteredAdjacencyGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            TryGetEdges_Throws_Test(graph);
        }

        [Test]
        public void TryGetOutEdges()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();
            var graph = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph);
            TryGetOutEdges_Test(
                graph,
                edges => graph.AddVerticesAndEdgeRange(edges));
        }

        [Test]
        public void TryGetOutEdges_Throws()
        {
            var wrappedGraph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ClusteredAdjacencyGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            TryGetOutEdges_Throws_Test(graph);
        }

        #endregion

        #region Remove Vertices

        [Test]
        public void RemoveVertex()
        {
            var wrappedGraph1 = new AdjacencyGraph<int, Edge<int>>();
            var graph1 = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph1);
            RemoveVertex_Clusters_Test(graph1);

            var wrappedGraph2 = new AdjacencyGraph<int, Edge<int>>();
            var graph2 = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph2);
            var subGraph2 = new ClusteredAdjacencyGraph<int, Edge<int>>(graph2);
            RemoveVertex_Clusters_Test(subGraph2);
        }

        [Test]
        public void RemoveVertex_Throws()
        {
            var wrappedGraph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ClusteredAdjacencyGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            RemoveVertex_Throws_Clusters_Test(graph);
        }

        [Test]
        public void RemoveVertexIf()
        {
            var wrappedGraph1 = new AdjacencyGraph<int, Edge<int>>();
            var graph1 = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph1);
            RemoveVertexIf_Clusters_Test(graph1);

            var wrappedGraph2 = new AdjacencyGraph<int, Edge<int>>();
            var graph2 = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph2);
            var subGraph2 = new ClusteredAdjacencyGraph<int, Edge<int>>(graph2);
            RemoveVertexIf_Clusters_Test(subGraph2);
        }

        [Test]
        public void RemoveVertexIf_Throws()
        {
            var wrappedGraph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ClusteredAdjacencyGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            RemoveVertexIf_Throws_Clusters_Test(graph);
        }

        #endregion

        #region Remove Edges

        [Test]
        public void RemoveEdge()
        {
            var wrappedGraph1 = new AdjacencyGraph<int, Edge<int>>();
            var graph1 = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph1);
            RemoveEdge_Clusters_Test(graph1);

            var wrappedGraph2 = new AdjacencyGraph<int, Edge<int>>();
            var graph2 = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph2);
            var subGraph2 = new ClusteredAdjacencyGraph<int, Edge<int>>(graph2);
            RemoveEdge_Clusters_Test(subGraph2);
        }

        [Test]
        public void RemoveEdge_EquatableEdge()
        {
            var wrappedGraph1 = new AdjacencyGraph<int, EquatableEdge<int>>();
            var graph1 = new ClusteredAdjacencyGraph<int, EquatableEdge<int>>(wrappedGraph1);
            RemoveEdge_EquatableEdge_Clusters_Test(graph1);

            var wrappedGraph2 = new AdjacencyGraph<int, EquatableEdge<int>>();
            var graph2 = new ClusteredAdjacencyGraph<int, EquatableEdge<int>>(wrappedGraph2);
            var subGraph2 = new ClusteredAdjacencyGraph<int, EquatableEdge<int>>(graph2);
            RemoveEdge_EquatableEdge_Clusters_Test(subGraph2);
        }

        [Test]
        public void RemoveEdge_Throws()
        {
            var wrappedGraph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ClusteredAdjacencyGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            RemoveEdge_Throws_Clusters_Test(graph);
        }

        [Test]
        public void RemoveEdgeIf()
        {
            var wrappedGraph1 = new AdjacencyGraph<int, Edge<int>>();
            var graph1 = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph1);
            RemoveEdgeIf_Clusters_Test(graph1);

            var wrappedGraph2 = new AdjacencyGraph<int, Edge<int>>();
            var graph2 = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph2);
            var subGraph2 = new ClusteredAdjacencyGraph<int, Edge<int>>(graph2);
            RemoveEdgeIf_Clusters_Test(subGraph2);
        }

        [Test]
        public void RemoveEdgeIf_Throws()
        {
            var wrappedGraph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ClusteredAdjacencyGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            RemoveEdgeIf_Throws_Clusters_Test(graph);
        }

        [Test]
        public void RemoveOutEdgeIf()
        {
            var wrappedGraph1 = new AdjacencyGraph<int, Edge<int>>();
            var graph1 = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph1);
            RemoveOutEdgeIf_Clusters_Test(graph1);

            var wrappedGraph2 = new AdjacencyGraph<int, Edge<int>>();
            var graph2 = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph2);
            var subGraph2 = new ClusteredAdjacencyGraph<int, Edge<int>>(graph2);
            RemoveOutEdgeIf_Clusters_Test(subGraph2);
        }

        [Test]
        public void RemoveOutEdgeIf_Throws()
        {
            var wrappedGraph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ClusteredAdjacencyGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            RemoveOutEdgeIf_Throws_Test(graph);
        }

        #endregion

        #region Clear

        [Test]
        public void Clear()
        {
            var wrappedGraph1 = new AdjacencyGraph<int, Edge<int>>();
            var graph1 = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph1);
            ClearGraphTest(graph1);

            var wrappedGraph2 = new AdjacencyGraph<int, Edge<int>>();
            var graph2 = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph2);
            var subGraph2 = new ClusteredAdjacencyGraph<int, Edge<int>>(graph2);
            ClearGraphTest(subGraph2);

            #region Local function

            void ClearGraphTest(ClusteredAdjacencyGraph<int, Edge<int>> g)
            {
                AssertEmptyGraph(g);

                g.Clear();
                AssertEmptyGraph(g);

                g.AddVerticesAndEdge(new Edge<int>(1, 2));
                g.AddVerticesAndEdge(new Edge<int>(2, 3));
                g.AddVerticesAndEdge(new Edge<int>(3, 1));

                g.Clear();
                AssertEmptyGraph(g);
            }

            #endregion
        }

        [Test]
        public void ClearOutEdges()
        {
            var wrappedGraph1 = new AdjacencyGraph<int, Edge<int>>();
            var graph1 = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph1);
            ClearOutEdgesTest(graph1);

            var wrappedGraph2 = new AdjacencyGraph<int, Edge<int>>();
            var graph2 = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph2);
            var subGraph2 = new ClusteredAdjacencyGraph<int, Edge<int>>(graph2);
            ClearOutEdgesTest(subGraph2);

            #region Local function

            void ClearOutEdgesTest(ClusteredAdjacencyGraph<int, Edge<int>> g)
            {
                AssertEmptyGraph(g);

                // Clear 1 => not in graph
                g.ClearOutEdges(1);
                AssertEmptyGraph(g);

                // Clear 1 => In graph but no out edges
                g.AddVertex(1);
                g.ClearOutEdges(1);
                AssertHasVertices(g, new[] { 1 });
                AssertNoEdge(g);

                var edge12 = new Edge<int>(1, 2);
                var edge23 = new Edge<int>(2, 3);
                g.AddVerticesAndEdgeRange(new[] { edge12, edge23 });

                // Clear 1
                g.ClearOutEdges(1);
                AssertHasEdges(g, new[] { edge23 });

                var edge13 = new Edge<int>(1, 3);
                var edge31 = new Edge<int>(3, 1);
                var edge32 = new Edge<int>(3, 2);
                g.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge31, edge32 });

                // Clear 3
                g.ClearOutEdges(3);
                AssertHasEdges(g, new[] { edge12, edge13, edge23 });

                // Clear 1
                g.ClearOutEdges(1);
                AssertHasEdges(g, new[] { edge23 });

                // Clear 2 = Clear
                g.ClearOutEdges(2);

                AssertNoEdge(g);
            }

            #endregion
        }

        [Test]
        public void ClearOutEdges_Throws()
        {
            var wrappedGraph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ClusteredAdjacencyGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.ClearOutEdges(null));
        }

        #endregion

        #region Test helpers

        private static void AssertNoCluster(
            [NotNull] IClusteredGraph graph)
        {
            Assert.AreEqual(0, graph.ClustersCount);
            CollectionAssert.IsEmpty(graph.Clusters);
        }

        private static void AssertHasClusters(
            [NotNull] IClusteredGraph graph,
            [NotNull, ItemNotNull] IEnumerable<IClusteredGraph> clusters)
        {
            IClusteredGraph[] clusterArray = clusters.ToArray();
            CollectionAssert.IsNotEmpty(clusterArray);

            Assert.AreEqual(clusterArray.Length, graph.ClustersCount);
            CollectionAssert.AreEquivalent(clusterArray, graph.Clusters);
        }

        #endregion

        #region Cluster

        [Test]
        public void Collapsed()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();
            var graph = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph);

            Assert.IsFalse(graph.Collapsed);

            graph.Collapsed = true;
            Assert.IsTrue(graph.Collapsed);

            graph.Collapsed = true;
            Assert.IsTrue(graph.Collapsed);

            graph.Collapsed = false;
            Assert.IsFalse(graph.Collapsed);
        }

        [Test]
        public void AddCluster()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();
            var graph = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph);

            AssertNoCluster(graph);

            IClusteredGraph cluster = graph.AddCluster();
            Assert.IsNotNull(cluster);
            AssertHasClusters(graph, new[] { cluster });

            IClusteredGraph cluster2 = ((IClusteredGraph)graph).AddCluster();
            Assert.IsNotNull(cluster2);
            AssertHasClusters(graph, new[] { cluster, cluster2 });
        }

        [Test]
        public void RemoveCluster()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();
            var graph = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph);

            AssertNoCluster(graph);

            IClusteredGraph cluster = graph.AddCluster();
            IClusteredGraph cluster2 = graph.AddCluster();
            IClusteredGraph cluster3 = graph.AddCluster();
            var wrappedGraph2 = new AdjacencyGraph<int, Edge<int>>();
            var graphNotInClusters = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph2);

            graph.RemoveCluster(graphNotInClusters);
            AssertHasClusters(graph, new[] { cluster, cluster2, cluster3 });

            graph.RemoveCluster(cluster2);
            AssertHasClusters(graph, new[] { cluster, cluster3 });

            graph.RemoveCluster(cluster);
            AssertHasClusters(graph, new[] { cluster3 });

            graph.RemoveCluster(cluster3);
            AssertNoCluster(graph);
        }

        [Test]
        public void RemoveCluster_Throws()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();
            var graph = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph);

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.RemoveCluster(null));
        }

        #endregion
    }
}