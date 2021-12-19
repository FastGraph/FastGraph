#nullable enable

using JetBrains.Annotations;
using NUnit.Framework;
using static FastGraph.Tests.GraphTestHelpers;

namespace FastGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="UndirectedGraph{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class UndirectedGraphTests : GraphTestsBase
    {
        [Test]
        public void Construction()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            AssertGraphProperties(graph);

            graph = new UndirectedGraph<int, Edge<int>>(true);
            AssertGraphProperties(graph);

            graph = new UndirectedGraph<int, Edge<int>>(false);
            AssertGraphProperties(graph, false);

            EdgeEqualityComparer<int> comparer = (edge, source, target) =>
                edge.Source.Equals(source) && edge.Target.Equals(target);
            graph = new UndirectedGraph<int, Edge<int>>(true, comparer);
            AssertGraphProperties(graph);
            graph.EdgeEqualityComparer.Should().BeSameAs(comparer);

            #region Local function

            void AssertGraphProperties<TVertex, TEdge>(
                UndirectedGraph<TVertex, TEdge> g,
                bool parallelEdges = true)
                where TVertex : notnull
                where TEdge : IEdge<TVertex>
            {
                g.IsDirected.Should().BeFalse();
                g.AllowParallelEdges.Should().Be(parallelEdges);
                AssertEmptyGraph(g);
                g.EdgeCapacity.Should().Be(-1);
            }

            #endregion
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new UndirectedGraph<int, Edge<int>>(true, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        #region Add Vertices

        [Test]
        public void AddVertex()
        {
            var graph = new UndirectedGraph<TestVertex, Edge<TestVertex>>();
            AddVertex_Test(graph);
        }

        [Test]
        public void AddVertex_Throws()
        {
            var graph = new UndirectedGraph<TestVertex, Edge<TestVertex>>();
            AddVertex_Throws_Test(graph);
        }

        [Test]
        public void AddVertex_EquatableVertex()
        {
            var graph = new UndirectedGraph<EquatableTestVertex, Edge<EquatableTestVertex>>();
            AddVertex_EquatableVertex_Test(graph);
        }

        [Test]
        public void AddVertexRange()
        {
            var graph = new UndirectedGraph<TestVertex, Edge<TestVertex>>();
            AddVertexRange_Test(graph);
        }

        [Test]
        public void AddVertexRange_Throws()
        {
            var graph = new UndirectedGraph<TestVertex, Edge<TestVertex>>();
            AddVertexRange_Throws_Test(graph);
        }

        #endregion

        #region Add Edges

        [Test]
        public void AddEdge_ParallelEdges()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            AddEdge_ParallelEdges_Test(graph);
        }

        [Test]
        public void AddEdge_ParallelEdges_EquatableEdge()
        {
            var graph = new UndirectedGraph<int, EquatableEdge<int>>();
            AddEdge_ParallelEdges_EquatableEdge_Test(graph);
        }

        [Test]
        public void AddEdge_NoParallelEdges()
        {
            var graph = new UndirectedGraph<int, Edge<int>>(false);
            AddEdge_NoParallelEdges_UndirectedGraph_Test(graph);
        }

        [Test]
        public void AddEdge_NoParallelEdges_EquatableEdge()
        {
            var graph = new UndirectedGraph<int, EquatableEdge<int>>(false);
            AddEdge_NoParallelEdges_EquatableEdge_UndirectedGraph_Test(graph);
        }

        [Test]
        public void AddEdge_Throws()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            AddEdge_Throws_Test(graph);
        }

        [Test]
        public void AddEdgeRange()
        {
            var graph = new UndirectedGraph<int, Edge<int>>(false);
            AddEdgeRange_Test(graph);
        }

        [Test]
        public void AddEdgeRange_Throws()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            AddEdgeRange_Throws_Test(graph);
        }

        #endregion

        #region Add Vertices & Edges

        [Test]
        public void AddVerticesAndEdge()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            AddVerticesAndEdge_Test(graph);
        }

        [Test]
        public void AddVerticesAndEdge_Throws()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            AddVerticesAndEdge_Throws_Test(graph);
        }

        [Test]
        public void AddVerticesAndEdgeRange()
        {
            var graph = new UndirectedGraph<int, Edge<int>>(false);
            AddVerticesAndEdgeRange_Test(graph);
        }

        [Test]
        public void AddVerticesAndEdgeRange_Throws()
        {
            var graph = new UndirectedGraph<int, Edge<int>>(false);
            AddVerticesAndEdgeRange_Throws_Test(graph);
        }

        #endregion

        #region Contains Vertex

        [Test]
        public void ContainsVertex()
        {
            var graph = new UndirectedGraph<TestVertex, Edge<TestVertex>>();
            ContainsVertex_Test(graph);
        }

        [Test]
        public void ContainsVertex_EquatableVertex()
        {
            var graph = new UndirectedGraph<EquatableTestVertex, Edge<EquatableTestVertex>>();
            ContainsVertex_EquatableVertex_Test(graph);
        }

        [Test]
        public void ContainsVertex_Throws()
        {
            var graph = new UndirectedGraph<TestVertex, Edge<TestVertex>>();
            ContainsVertex_Throws_Test(graph);
        }

        #endregion

        #region Contains Edge

        [Test]
        public void ContainsEdge()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            ContainsEdge_Test(graph);
        }

        [Test]
        public void ContainsEdge_EquatableEdge()
        {
            var graph = new UndirectedGraph<int, EquatableEdge<int>>();
            ContainsEdge_EquatableEdge_Test(graph);
        }

        [Test]
        public void ContainsEdge_SourceTarget()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            ContainsEdge_SourceTarget_UndirectedGraph_Test(graph);
        }

        [Test]
        public void ContainsEdge_Throws()
        {
            var graph = new UndirectedGraph<TestVertex, Edge<TestVertex>>();
            ContainsEdge_NullThrows_Test(graph);
            ContainsEdge_SourceTarget_Throws_UndirectedGraph_Test(graph);
        }

        [Test]
        public void ContainsEdge_Undirected()
        {
            var graph1 = new UndirectedGraph<int, EquatableEdge<int>>();
            var graph2 = new UndirectedGraph<int, EquatableUndirectedEdge<int>>();
            ContainsEdge_UndirectedEdge_UndirectedGraph_Test(
                graph1,
                graph2);
        }

        #endregion

        #region Adjacent Edges

        [Test]
        public void AdjacentEdge()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            AdjacentEdge_Test(graph);
        }

        [Test]
        public void AdjacentEdge_Throws()
        {
            var graph1 = new UndirectedGraph<int, Edge<int>>();
            AdjacentEdge_Throws_Test(graph1);

            var graph2 = new UndirectedGraph<TestVertex, Edge<TestVertex>>();
            AdjacentEdge_NullThrows_Test(graph2);
        }

        [Test]
        public void AdjacentEdges()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            AdjacentEdges_Test(graph);
        }

        [Test]
        public void AdjacentEdges_Throws()
        {
            var graph = new UndirectedGraph<EquatableTestVertex, Edge<EquatableTestVertex>>();
            AdjacentEdges_Throws_Test(graph);
        }

        #endregion

        [Test]
        public void AdjacentVertices()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();

            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge13Bis = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            var edge51 = new Edge<int>(5, 1);
            var edge65 = new Edge<int>(6, 5);
            var edge66 = new Edge<int>(6, 6);

            graph.AddVertex(7);
            graph.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge13, edge13Bis,
                edge14, edge24, edge31, edge33,
                edge51, edge65, edge66
            });

            graph.AdjacentVertices(1).Should().BeEquivalentTo(new[] { 2, 3, 4, 5 });

            graph.AdjacentVertices(2).Should().BeEquivalentTo(new[] { 1, 4 });

            graph.AdjacentVertices(3).Should().BeEquivalentTo(new[] { 1 });

            graph.AdjacentVertices(4).Should().BeEquivalentTo(new[] { 1, 2 });

            graph.AdjacentVertices(5).Should().BeEquivalentTo(new[] { 1, 6 });

            graph.AdjacentVertices(6).Should().BeEquivalentTo(new[] { 5 });

            graph.AdjacentVertices(7).Should().BeEmpty();
        }

        [Test]
        public void AdjacentVertices_Throws()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            Invoking(() => { IEnumerable<int> _ = graph.AdjacentVertices(10); }).Should().Throw<VertexNotFoundException>();
        }

        #region Try Get Edges

        [Test]
        public void TryGetEdge()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            TryGetEdge_UndirectedGraph_Test(graph);
        }

        [Test]
        public void TryGetEdge_Throws()
        {
            var graph = new UndirectedGraph<TestVertex, Edge<TestVertex>>();
            TryGetEdge_Throws_UndirectedGraph_Test(graph);
        }

        #endregion

        #region Remove Vertices

        [Test]
        public void RemoveVertex()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            RemoveVertex_Test(graph);
        }

        [Test]
        public void RemoveVertex_Throws()
        {
            var graph = new UndirectedGraph<TestVertex, Edge<TestVertex>>();
            RemoveVertex_Throws_Test(graph);
        }

        [Test]
        public void RemoveVertexIf()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            RemoveVertexIf_Test(graph);

            graph = new UndirectedGraph<int, Edge<int>>();
            RemoveVertexIf_Test2(graph);
        }

        [Test]
        public void RemoveVertexIf_Throws()
        {
            var graph = new UndirectedGraph<TestVertex, Edge<TestVertex>>();
            RemoveVertexIf_Throws_Test(graph);
        }

        #endregion

        #region Remove Edges

        [Test]
        public void RemoveEdge()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            RemoveEdge_Test(graph);
        }

        [Test]
        public void RemoveEdge_EquatableEdge()
        {
            var graph = new UndirectedGraph<int, EquatableEdge<int>>();
            RemoveEdge_EquatableEdge_Test(graph);
        }

        [Test]
        public void RemoveEdge_Throws()
        {
            var graph = new UndirectedGraph<TestVertex, Edge<TestVertex>>();
            RemoveEdge_Throws_Test(graph);
        }

        [Test]
        public void RemoveEdgeIf()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            RemoveEdgeIf_Test(graph);
        }

        [Test]
        public void RemoveEdgeIf_Throws()
        {
            var graph = new UndirectedGraph<TestVertex, Edge<TestVertex>>();
            RemoveEdgeIf_Throws_Test(graph);
        }

        [Test]
        public void RemoveEdges()
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            var graph = new UndirectedGraph<int, Edge<int>>();

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.VertexRemoved += v =>
            {
                ++verticesRemoved;
            };

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeRemoved += e =>
            {
                e.Should().NotBeNull();
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge13Bis = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            var edgeNotInGraph = new Edge<int>(3, 2);
            graph.AddVertexRange(new[] { 1, 2, 3, 4 });
            graph.AddEdgeRange(new[] { edge12, edge13, edge13Bis, edge14, edge24, edge31, edge33 });

            graph.RemoveEdges(Enumerable.Empty<Edge<int>>()).Should().Be(0);
            CheckCounters(0);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge13, edge13Bis, edge14, edge24, edge31, edge33 });

            graph.RemoveEdges(new[] { edge12, edge13Bis }).Should().Be(2);
            CheckCounters(2);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge13, edge14, edge24, edge31, edge33 });

            graph.RemoveEdges(new[] { edge13, edge14, edgeNotInGraph }).Should().Be(2);
            CheckCounters(2);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge24, edge31, edge33 });

            graph.RemoveEdges(new[] { edge24, edge31, edge33 }).Should().Be(3);
            CheckCounters(3);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertNoEdge(graph);

            #region Local function

            void CheckCounters(int expectedRemovedEdges)
            {
                verticesRemoved.Should().Be(0);
                edgesRemoved.Should().Be(expectedRemovedEdges);
                edgesRemoved = 0;
            }

            #endregion
        }

        [Test]
        public void RemoveEdges_Throws()
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            var graph = new UndirectedGraph<int, Edge<int>>();
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.VertexRemoved += v =>
            {
                ++verticesRemoved;
            };

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeRemoved += e =>
            {
                e.Should().NotBeNull();
                ++edgesRemoved;
            };

            var edge = new Edge<int>(1, 2);
            graph.AddVerticesAndEdge(edge);

            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => graph.RemoveEdges(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            AssertHasEdges(graph, new[] { edge });
            CheckCounters();

#pragma warning disable CS8620
            Invoking(() => graph.RemoveEdges(new[] { edge, default })).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8620
            edgesRemoved.Should().Be(0);
            AssertHasEdges(graph, new[] { edge });
            CheckCounters();

            #region Local function

            void CheckCounters()
            {
                verticesRemoved.Should().Be(0);
                edgesRemoved.Should().Be(0);
            }

            #endregion
        }

        [Test]
        public void RemoveAdjacentEdgeIf()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            RemoveAdjacentEdgeIf_Test(graph);
        }

        [Test]
        public void RemoveAdjacentEdgeIf_Throws()
        {
            var graph = new UndirectedGraph<TestVertex, Edge<TestVertex>>();
            RemoveAdjacentEdgeIf_Throws_Test(graph);
        }

        #endregion

        #region Clear

        [Test]
        public void Clear()
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            var graph = new UndirectedGraph<int, Edge<int>>();
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

        private static void ClearEdgesCommon([InstantHandle] Action<UndirectedGraph<int, Edge<int>>, int> clearEdges)
        {
            int edgesRemoved = 0;

            var graph = new UndirectedGraph<int, Edge<int>>();
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

            AssertHasEdges(graph, new[] { edge12 });
            CheckCounter(4);

            // Clear 2 = Clear
            clearEdges(graph, 2);

            AssertNoEdge(graph);
            CheckCounter(1);

            var edge11 = new Edge<int>(1, 1);
            graph.AddVerticesAndEdgeRange(new[] { edge11, edge12, edge13, edge23, edge31, edge32 });

            // Clear self edge
            clearEdges(graph, 1);

            AssertHasEdges(graph, new[] { edge23, edge32 });
            CheckCounter(4);

            #region Local function

            void CheckCounter(int expectedRemovedEdges)
            {
                edgesRemoved.Should().Be(expectedRemovedEdges);
                edgesRemoved = 0;
            }

            #endregion
        }

        [Test]
        public void ClearAdjacentEdges()
        {
            ClearEdgesCommon((graph, vertex) => graph.ClearAdjacentEdges(vertex));
        }

        [Test]
        public void ClearEdges()
        {
            ClearEdgesCommon((graph, vertex) => graph.ClearEdges(vertex));
        }

        #endregion

        [Test]
        public void Clone()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            AssertEmptyGraph(graph);

            var clonedGraph = graph.Clone();
            clonedGraph.Should().NotBeNull();
            AssertEmptyGraph(clonedGraph);

            clonedGraph = (UndirectedGraph<int, Edge<int>>)((ICloneable)graph).Clone();
            clonedGraph.Should().NotBeNull();
            AssertEmptyGraph(clonedGraph);

            graph.AddVertexRange(new[] { 1, 2, 3 });
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertNoEdge(graph);

            clonedGraph = graph.Clone();
            clonedGraph.Should().NotBeNull();
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3 });
            AssertNoEdge(clonedGraph);

            clonedGraph = (UndirectedGraph<int, Edge<int>>)((ICloneable)graph).Clone();
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

            clonedGraph = (UndirectedGraph<int, Edge<int>>)((ICloneable)graph).Clone();
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

            clonedGraph = (UndirectedGraph<int, Edge<int>>)((ICloneable)graph).Clone();
            clonedGraph.Should().NotBeNull();
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(clonedGraph, new[] { edge1, edge2, edge3 });
        }

        [Test]
        public void TrimEdgeExcess()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
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
