#nullable enable

using JetBrains.Annotations;
using NUnit.Framework;
using static FastGraph.Tests.GraphTestHelpers;

namespace FastGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="AdjacencyGraph{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class AdjacencyGraphTests : GraphTestsBase
    {
        [Test]
        public void Construction()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            AssertGraphProperties(graph);

            graph = new AdjacencyGraph<int, Edge<int>>(true);
            AssertGraphProperties(graph);

            graph = new AdjacencyGraph<int, Edge<int>>(false);
            AssertGraphProperties(graph, false);

            graph = new AdjacencyGraph<int, Edge<int>>(true, 12);
            AssertGraphProperties(graph);

            graph = new AdjacencyGraph<int, Edge<int>>(false, 12);
            AssertGraphProperties(graph, false);

            graph = new AdjacencyGraph<int, Edge<int>>(true, 42, 12);
            AssertGraphProperties(graph, edgeCapacity: 12);

            graph = new AdjacencyGraph<int, Edge<int>>(false, 42, 12);
            AssertGraphProperties(graph, false, 12);

            #region Local function

            void AssertGraphProperties<TVertex, TEdge>(
                AdjacencyGraph<TVertex, TEdge> g,
                bool parallelEdges = true,
                int edgeCapacity = 0)
                where TVertex : notnull
                where TEdge : IEdge<TVertex>
            {
                g.IsDirected.Should().BeTrue();
                g.AllowParallelEdges.Should().Be(parallelEdges);
                AssertEmptyGraph(g);
                g.EdgeCapacity.Should().Be(edgeCapacity);
                g.VertexType.Should().BeSameAs(typeof(int));
                g.EdgeType.Should().BeSameAs(typeof(Edge<int>));
            }

            #endregion
        }

        #region Add Vertices

        [Test]
        public void AddVertex()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            AddVertex_Test(graph);
        }

        [Test]
        public void AddVertex_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            AddVertex_Throws_Test(graph);
        }

        [Test]
        public void AddVertex_EquatableVertex()
        {
            var graph = new AdjacencyGraph<EquatableTestVertex, Edge<EquatableTestVertex>>();
            AddVertex_EquatableVertex_Test(graph);
        }

        [Test]
        public void AddVertexRange()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            AddVertexRange_Test(graph);
        }

        [Test]
        public void AddVertexRange_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            AddVertexRange_Throws_Test(graph);
        }

        #endregion

        #region Add Edges

        [Test]
        public void AddEdge_ParallelEdges()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            AddEdge_ParallelEdges_Test(graph);
        }

        [Test]
        public void AddEdge_ParallelEdges_EquatableEdge()
        {
            var graph = new AdjacencyGraph<int, EquatableEdge<int>>();
            AddEdge_ParallelEdges_EquatableEdge_Test(graph);
        }

        [Test]
        public void AddEdge_NoParallelEdges()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>(false);
            AddEdge_NoParallelEdges_Test(graph);
        }

        [Test]
        public void AddEdge_NoParallelEdges_EquatableEdge()
        {
            var graph = new AdjacencyGraph<int, EquatableEdge<int>>(false);
            AddEdge_NoParallelEdges_EquatableEdge_Test(graph);
        }

        [Test]
        public void AddEdge_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            AddEdge_Throws_Test(graph);
        }

        [Test]
        public void AddEdgeRange()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>(false);
            AddEdgeRange_Test(graph);
        }

        [Test]
        public void AddEdgeRange_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            AddEdgeRange_Throws_Test(graph);
        }

        #endregion

        #region Add Vertices & Edges

        [Test]
        public void AddVerticesAndEdge()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            AddVerticesAndEdge_Test(graph);
        }

        [Test]
        public void AddVerticesAndEdge_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            AddVerticesAndEdge_Throws_Test(graph);
        }

        [Test]
        public void AddVerticesAndEdgeRange()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>(false);
            AddVerticesAndEdgeRange_Test(graph);
        }

        [Test]
        public void AddVerticesAndEdgeRange_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>(false);
            AddVerticesAndEdgeRange_Throws_Test(graph);
        }

        #endregion

        #region Contains Vertex

        [Test]
        public void ContainsVertex()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            ContainsVertex_Test(graph);
        }

        [Test]
        public void ContainsVertex_EquatableVertex()
        {
            var graph = new AdjacencyGraph<EquatableTestVertex, Edge<EquatableTestVertex>>();
            ContainsVertex_EquatableVertex_Test(graph);
        }

        [Test]
        public void ContainsVertex_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            ContainsVertex_Throws_Test(graph);
        }

        #endregion

        #region Contains Edge

        [Test]
        public void ContainsEdge()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            ContainsEdge_Test(graph);
        }

        [Test]
        public void ContainsEdge_EquatableEdge()
        {
            var graph = new AdjacencyGraph<int, EquatableEdge<int>>();
            ContainsEdge_EquatableEdge_Test(graph);
        }

        [Test]
        public void ContainsEdge_SourceTarget()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            ContainsEdge_SourceTarget_Test(graph);
        }

        [Test]
        public void ContainsEdge_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            ContainsEdge_NullThrows_Test(graph);
            ContainsEdge_SourceTarget_Throws_Test(graph);
        }

        #endregion

        #region Out Edges

        [Test]
        public void OutEdge()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            OutEdge_Test(graph);
        }

        [Test]
        public void OutEdge_Throws()
        {
            var graph1 = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            OutEdge_NullThrows_Test(graph1);

            var graph2 = new AdjacencyGraph<int, Edge<int>>();
            OutEdge_Throws_Test(graph2);
        }

        [Test]
        public void OutEdges()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            OutEdges_Test(graph);
        }

        [Test]
        public void OutEdges_Throws()
        {
            var graph1 = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            OutEdges_NullThrows_Test(graph1);

            var graph2 = new AdjacencyGraph<EquatableTestVertex, Edge<EquatableTestVertex>>();
            OutEdges_Throws_Test(graph2);
        }

        #endregion

        #region Try Get Edges

        [Test]
        public void TryGetEdge()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            TryGetEdge_Test(graph);
        }

        [Test]
        public void TryGetEdge_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            TryGetEdge_Throws_Test(graph);
        }

        [Test]
        public void TryGetEdges()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            TryGetEdges_Test(graph);
        }

        [Test]
        public void TryGetEdges_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            TryGetEdges_Throws_Test(graph);
        }

        [Test]
        public void TryGetOutEdges()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            TryGetOutEdges_Test(graph);
        }

        [Test]
        public void TryGetOutEdges_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            TryGetOutEdges_Throws_Test(graph);
        }

        #endregion

        #region Remove Vertices

        [Test]
        public void RemoveVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            RemoveVertex_Test(graph);
        }

        [Test]
        public void RemoveVertex_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            RemoveVertex_Throws_Test(graph);
        }

        [Test]
        public void RemoveVertexIf()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            RemoveVertexIf_Test(graph);

            graph = new AdjacencyGraph<int, Edge<int>>();
            RemoveVertexIf_Test2(graph);
        }

        [Test]
        public void RemoveVertexIf_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            RemoveVertexIf_Throws_Test(graph);
        }

        #endregion

        #region Remove Edges

        [Test]
        public void RemoveEdge()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            RemoveEdge_Test(graph);
        }

        [Test]
        public void RemoveEdge_EquatableEdge()
        {
            var graph = new AdjacencyGraph<int, EquatableEdge<int>>();
            RemoveEdge_EquatableEdge_Test(graph);
        }

        [Test]
        public void RemoveEdge_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            RemoveEdge_Throws_Test(graph);
        }

        [Test]
        public void RemoveEdgeIf()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            RemoveEdgeIf_Test(graph);
        }

        [Test]
        public void RemoveEdgeIf_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            RemoveEdgeIf_Throws_Test(graph);
        }

        [Test]
        public void RemoveOutEdgeIf()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            RemoveOutEdgeIf_Test(graph);
        }

        [Test]
        public void RemoveOutEdgeIf_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            RemoveOutEdgeIf_Throws_Test(graph);
        }

        #endregion

        #region Clear

        [Test]
        public void Clear()
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            var graph = new AdjacencyGraph<int, Edge<int>>();
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.VertexRemoved += v =>
            {
                // ReSharper disable once AccessToModifiedClosure
                ++verticesRemoved;
            };
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeRemoved += e =>
            {
                e.Should().NotBeNull();
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            AssertEmptyGraph(graph);

            graph.Clear();
            AssertEmptyGraph(graph);
            CheckCounters(0, 0);

            graph.AddVerticesAndEdge(new Edge<int>(1, 2));
            graph.AddVerticesAndEdge(new Edge<int>(2, 3));
            graph.AddVerticesAndEdge(new Edge<int>(3, 1));

            graph.Clear();
            AssertEmptyGraph(graph);
            CheckCounters(3, 3);

            #region Local function

            void CheckCounters(int expectedVerticesRemoved, int expectedEdgesRemoved)
            {
                verticesRemoved.Should().Be(expectedVerticesRemoved);
                edgesRemoved.Should().Be(expectedEdgesRemoved);
                verticesRemoved = 0;
                edgesRemoved = 0;
            }

            #endregion
        }

        private static void ClearEdgesCommon([InstantHandle] Action<AdjacencyGraph<int, Edge<int>>, int> clearEdges)
        {
            int edgesRemoved = 0;

            var graph = new AdjacencyGraph<int, Edge<int>>();
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeRemoved += e =>
            {
                e.Should().NotBeNull();
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            AssertEmptyGraph(graph);

            // Clear 1 => not in graph
            clearEdges(graph, 1);
            AssertEmptyGraph(graph);
            CheckCounter(0);

            // Clear 1 => In graph but no out edges
            graph.AddVertex(1);
            clearEdges(graph, 1);
            AssertHasVertices(graph, new[] { 1 });
            AssertNoEdge(graph);
            CheckCounter(0);

            var edge12 = new Edge<int>(1, 2);
            var edge23 = new Edge<int>(2, 3);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge23 });

            // Clear 1
            clearEdges(graph, 1);

            AssertHasEdges(graph, new[] { edge23 });
            CheckCounter(1);

            var edge13 = new Edge<int>(1, 3);
            var edge31 = new Edge<int>(3, 1);
            var edge32 = new Edge<int>(3, 2);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge31, edge32 });

            // Clear 3
            clearEdges(graph, 3);

            AssertHasEdges(graph, new[] { edge12, edge13, edge23 });
            CheckCounter(2);

            // Clear 1
            clearEdges(graph, 1);

            AssertHasEdges(graph, new[] { edge23 });
            CheckCounter(2);

            // Clear 2 = Clear
            clearEdges(graph, 2);

            AssertNoEdge(graph);
            CheckCounter(1);

            #region Local function

            void CheckCounter(int expectedRemovedEdges)
            {
                edgesRemoved.Should().Be(expectedRemovedEdges);
                edgesRemoved = 0;
            }

            #endregion
        }

        [Test]
        public void ClearOutEdges()
        {
            ClearEdgesCommon((graph, vertex) => graph.ClearOutEdges(vertex));
        }

        [Test]
        public void ClearEdges()
        {
            ClearEdgesCommon((graph, vertex) => graph.ClearEdges(vertex));
        }

        [Test]
        public void ClearEdges_Throws()
        {
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new AdjacencyGraph<TestVertex, Edge<TestVertex>>().ClearOutEdges(default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new AdjacencyGraph<TestVertex, Edge<TestVertex>>().ClearEdges(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
        }

        #endregion

        [Test]
        public void Clone()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            AssertEmptyGraph(graph);

            AdjacencyGraph<int, Edge<int>> clonedGraph = graph.Clone();
            clonedGraph.Should().NotBeNull();
            AssertEmptyGraph(clonedGraph);

            clonedGraph = (AdjacencyGraph<int, Edge<int>>)((ICloneable)graph).Clone();
            clonedGraph.Should().NotBeNull();
            AssertEmptyGraph(clonedGraph);

            graph.AddVertexRange(new[] { 1, 2, 3 });
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertNoEdge(graph);

            clonedGraph = graph.Clone();
            clonedGraph.Should().NotBeNull();
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3 });
            AssertNoEdge(clonedGraph);

            clonedGraph = (AdjacencyGraph<int, Edge<int>>)((ICloneable)graph).Clone();
            clonedGraph.Should().NotBeNull();
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3 });
            AssertNoEdge(clonedGraph);

            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 3);
            graph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3 });
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge1, edge2, edge3 });

            clonedGraph = graph.Clone();
            clonedGraph.Should().NotBeNull();
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3 });
            AssertHasEdges(clonedGraph, new[] { edge1, edge2, edge3 });

            clonedGraph = (AdjacencyGraph<int, Edge<int>>)((ICloneable)graph).Clone();
            clonedGraph.Should().NotBeNull();
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3 });
            AssertHasEdges(clonedGraph, new[] { edge1, edge2, edge3 });

            graph.AddVertex(4);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge1, edge2, edge3 });

            clonedGraph = graph.Clone();
            clonedGraph.Should().NotBeNull();
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(clonedGraph, new[] { edge1, edge2, edge3 });

            clonedGraph = (AdjacencyGraph<int, Edge<int>>)((ICloneable)graph).Clone();
            clonedGraph.Should().NotBeNull();
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(clonedGraph, new[] { edge1, edge2, edge3 });
        }

        [Test]
        public void TrimEdgeExcess()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>(true, 12, 50);
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(1, 3),
                new Edge<int>(1, 4)
            });

            Invoking(() => graph.TrimEdgeExcess()).Should().NotThrow();
        }
    }
}
