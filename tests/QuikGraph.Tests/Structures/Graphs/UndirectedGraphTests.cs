using System;
using System.Linq;
using JetBrains.Annotations;
using NUnit.Framework;
using static QuikGraph.Tests.GraphTestHelpers;

namespace QuikGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="UndirectedGraph{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class UndirectedGraphTests : GraphTestsBase
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
            Assert.AreSame(comparer, graph.EdgeEqualityComparer);

            #region Local function

            void AssertGraphProperties<TVertex, TEdge>(
                UndirectedGraph<TVertex, TEdge> g,
                bool parallelEdges = true)
                where TEdge : IEdge<TVertex>
            {
                Assert.IsFalse(g.IsDirected);
                Assert.AreEqual(parallelEdges, g.AllowParallelEdges);
                AssertEmptyGraph(g);
                Assert.AreEqual(-1, g.EdgeCapacity);
            }

            #endregion
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new UndirectedGraph<int, Edge<int>>(true, null));
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

            CollectionAssert.AreEquivalent(
                new[] { 2, 3, 4, 5 },
                graph.AdjacentVertices(1));

            CollectionAssert.AreEquivalent(
                new[] { 1, 4 },
                graph.AdjacentVertices(2));

            CollectionAssert.AreEquivalent(
                new[] { 1 },
                graph.AdjacentVertices(3));

            CollectionAssert.AreEquivalent(
                new[] { 1, 2 },
                graph.AdjacentVertices(4));

            CollectionAssert.AreEquivalent(
                new[] { 1, 6 },
                graph.AdjacentVertices(5));

            CollectionAssert.AreEquivalent(
                new[] { 5 },
                graph.AdjacentVertices(6));

            CollectionAssert.IsEmpty(graph.AdjacentVertices(7));
        }

        [Test]
        public void AdjacentVertices_Throws()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            Assert.Throws<VertexNotFoundException>(() => graph.AdjacentVertices(10));
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

            graph.VertexRemoved += v =>
            {
                Assert.IsNotNull(v);
                ++verticesRemoved;
            };

            graph.EdgeRemoved += e =>
            {
                Assert.IsNotNull(e);
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

            Assert.AreEqual(0, graph.RemoveEdges(Enumerable.Empty<Edge<int>>()));
            CheckCounters(0);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge13, edge13Bis, edge14, edge24, edge31, edge33 });

            Assert.AreEqual(2, graph.RemoveEdges(new[] { edge12, edge13Bis }));
            CheckCounters(2);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge13, edge14, edge24, edge31, edge33 });

            Assert.AreEqual(2, graph.RemoveEdges(new[] { edge13, edge14, edgeNotInGraph }));
            CheckCounters(2);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge24, edge31, edge33 });

            Assert.AreEqual(3, graph.RemoveEdges(new[] { edge24, edge31, edge33 }));
            CheckCounters(3);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertNoEdge(graph);

            #region Local function

            void CheckCounters(int expectedRemovedEdges)
            {
                Assert.AreEqual(0, verticesRemoved);
                Assert.AreEqual(expectedRemovedEdges, edgesRemoved);
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
            graph.VertexRemoved += v =>
            {
                Assert.IsNotNull(v);
                ++verticesRemoved;
            };

            graph.EdgeRemoved += e =>
            {
                Assert.IsNotNull(e);
                ++edgesRemoved;
            };

            var edge = new Edge<int>(1, 2);
            graph.AddVerticesAndEdge(edge);

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.RemoveEdges(null));
            AssertHasEdges(graph, new[] { edge });
            CheckCounters();

            Assert.Throws<ArgumentNullException>(() => graph.RemoveEdges(new[] { edge, null }));
            Assert.AreEqual(0, edgesRemoved);
            AssertHasEdges(graph, new[] { edge });
            CheckCounters();

            #region Local function

            void CheckCounters()
            {
                Assert.AreEqual(0, verticesRemoved);
                Assert.AreEqual(0, edgesRemoved);
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
            graph.VertexRemoved += v =>
            {
                Assert.IsNotNull(v);
                // ReSharper disable once AccessToModifiedClosure
                ++verticesRemoved;
            };
            graph.EdgeRemoved += e =>
            {
                Assert.IsNotNull(e);
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
                Assert.AreEqual(expectedVerticesRemoved, verticesRemoved);
                Assert.AreEqual(expectedEdgesRemoved, edgesRemoved);
                verticesRemoved = 0;
                edgesRemoved = 0;
            }

            #endregion
        }

        private static void ClearEdgesCommon([NotNull, InstantHandle] Action<UndirectedGraph<int, Edge<int>>, int> clearEdges)
        {
            int edgesRemoved = 0;

            var graph = new UndirectedGraph<int, Edge<int>>();
            graph.EdgeRemoved += e =>
            {
                Assert.IsNotNull(e);
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

            #region Local function

            void CheckCounter(int expectedRemovedEdges)
            {
                Assert.AreEqual(expectedRemovedEdges, edgesRemoved);
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
            Assert.IsNotNull(clonedGraph);
            AssertEmptyGraph(clonedGraph);

            clonedGraph = (UndirectedGraph<int, Edge<int>>)((ICloneable)graph).Clone();
            Assert.IsNotNull(clonedGraph);
            AssertEmptyGraph(clonedGraph);

            graph.AddVertexRange(new[] { 1, 2, 3 });
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertNoEdge(graph);

            clonedGraph = graph.Clone();
            Assert.IsNotNull(clonedGraph);
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3 });
            AssertNoEdge(clonedGraph);

            clonedGraph = (UndirectedGraph<int, Edge<int>>)((ICloneable)graph).Clone();
            Assert.IsNotNull(clonedGraph);
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3 });
            AssertNoEdge(clonedGraph);

            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 3);
            graph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3 });
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge1, edge2, edge3 });

            clonedGraph = graph.Clone();
            Assert.IsNotNull(clonedGraph);
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3 });
            AssertHasEdges(clonedGraph, new[] { edge1, edge2, edge3 });

            clonedGraph = (UndirectedGraph<int, Edge<int>>)((ICloneable)graph).Clone();
            Assert.IsNotNull(clonedGraph);
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3 });
            AssertHasEdges(clonedGraph, new[] { edge1, edge2, edge3 });

            graph.AddVertex(4);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge1, edge2, edge3 });

            clonedGraph = graph.Clone();
            Assert.IsNotNull(clonedGraph);
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(clonedGraph, new[] { edge1, edge2, edge3 });

            clonedGraph = (UndirectedGraph<int, Edge<int>>)((ICloneable)graph).Clone();
            Assert.IsNotNull(clonedGraph);
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

            Assert.DoesNotThrow(() => graph.TrimEdgeExcess());
        }
    }
}