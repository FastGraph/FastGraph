using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace QuikGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="BidirectionalGraph{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class BidirectionalGraphTests : GraphTestsBase
    {
        [Test]
        public void Construction()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            AssertEmptyGraph(graph);

            graph = new BidirectionalGraph<int, Edge<int>>(true);
            AssertEmptyGraph(graph);

            graph = new BidirectionalGraph<int, Edge<int>>(false);
            AssertEmptyGraph(graph, false);

            graph = new BidirectionalGraph<int, Edge<int>>(true, 12);
            AssertEmptyGraph(graph);

            graph = new BidirectionalGraph<int, Edge<int>>(false, 12);
            AssertEmptyGraph(graph, false);

            graph = new BidirectionalGraph<int, Edge<int>>(true, 42, 12);
            AssertEmptyGraph(graph, edgeCapacity: 12);

            graph = new BidirectionalGraph<int, Edge<int>>(false, 42, 12);
            AssertEmptyGraph(graph, false, 12);

            #region Local function

            void AssertEmptyGraph<TVertex, TEdge>(BidirectionalGraph<TVertex, TEdge> g, bool parallelEdges = true, int edgeCapacity = 0)
                where TEdge : IEdge<TVertex>
            {
                Assert.IsTrue(g.IsDirected);
                Assert.AreEqual(parallelEdges, g.AllowParallelEdges);
                AssertNoVertex(g);
                AssertNoEdge(g);
                Assert.AreEqual(edgeCapacity, g.EdgeCapacity);
                Assert.AreSame(typeof(int), g.VertexType);
                Assert.AreSame(typeof(Edge<int>), g.EdgeType);
            }

            #endregion
        }

        #region Add Vertices

        [Test]
        public void AddVertex()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            AddVertex_Test(graph);
        }

        [Test]
        public void AddVertex_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            AddVertex_Throws_Test(graph);
        }

        [Test]
        public void AddVertex_EquatableVertex()
        {
            var graph = new BidirectionalGraph<EquatableTestVertex, Edge<EquatableTestVertex>>();
            AddVertex_EquatableVertex_Test(graph);
        }

        [Test]
        public void AddVertexRange()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            AddVertexRange_Test(graph);
        }

        [Test]
        public void AddVertexRange_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            AddVertexRange_Throws_Test(graph);
        }

        #endregion

        #region Add Edges

        [Test]
        public void AddEdge_ParallelEdges()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            AddEdge_ParallelEdges_Test(graph);
        }

        [Test]
        public void AddEdge_ParallelEdges_EquatableEdge()
        {
            var graph = new BidirectionalGraph<int, EquatableEdge<int>>();
            AddEdge_ParallelEdges_EquatableEdge_Test(graph);
        }

        [Test]
        public void AddEdge_NoParallelEdges()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>(false);
            AddEdge_NoParallelEdges_Test(graph);
        }

        [Test]
        public void AddEdge_NoParallelEdges_EquatableEdge()
        {
            var graph = new BidirectionalGraph<int, EquatableEdge<int>>(false);
            AddEdge_NoParallelEdges_EquatableEdge_Test(graph);
        }

        [Test]
        public void AddEdge_Throws()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            AddEdge_Throws_Test(graph);
        }

        [Test]
        public void AddEdgeRange()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>(false);
            AddEdgeRange_Test(graph);
        }

        [Test]
        public void AddEdgeRange_Throws()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            AddEdgeRange_Throws_Test(graph);
        }

        #endregion

        #region Add Vertices & Edges

        [Test]
        public void AddVerticesAndEdge()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            AddVerticesAndEdge_Test(graph);
        }

        [Test]
        public void AddVerticesAndEdge_Throws()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            AddVerticesAndEdge_Throws_Test(graph);
        }

        [Test]
        public void AddVerticesAndEdgeRange()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>(false);
            AddVerticesAndEdgeRange_Test(graph);
        }

        [Test]
        public void AddVerticesAndEdgeRange_Throws()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>(false);
            AddVerticesAndEdgeRange_Throws_Test(graph);
        }

        #endregion

        #region Contains Vertex

        [Test]
        public void ContainsVertex()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            ContainsVertex_Test(graph);
        }

        [Test]
        public void ContainsVertex_EquatableVertex()
        {
            var graph = new BidirectionalGraph<EquatableTestVertex, Edge<EquatableTestVertex>>();
            ContainsVertex_EquatableVertex_Test(graph);
        }

        [Test]
        public void ContainsVertex_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            ContainsVertex_Throws_Test(graph);
        }

        #endregion

        #region Contains Edge

        [Test]
        public void ContainsEdge()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            ContainsEdge_Test(graph);
        }

        [Test]
        public void ContainsEdge_EquatableEdge()
        {
            var graph = new BidirectionalGraph<int, EquatableEdge<int>>();
            ContainsEdge_EquatableEdge_Test(graph);
        }

        [Test]
        public void ContainsEdge_SourceTarget()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            ContainsEdge_SourceTarget_Test(graph);
        }

        [Test]
        public void ContainsEdge_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            ContainsEdge_Throws_Test(graph);
            ContainsEdge_SourceTarget_Throws_Test(graph);
        }

        #endregion

        #region Out Edges

        [Test]
        public void OutEdge()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            OutEdge_Test(graph);
        }

        [Test]
        public void OutEdge_Throws()
        {
            var graph1 = new BidirectionalGraph<int, Edge<int>>();
            var graph2 = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            OutEdge_Throws_Test(graph1, graph2);
        }

        [Test]
        public void OutEdges()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            OutEdges_Test(graph);
        }

        [Test]
        public void OutEdges_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
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

            var graph = new BidirectionalGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[] { edge11, edge13, edge21, edge41 });

            Assert.AreSame(edge11, graph.InEdge(1, 0));
            Assert.AreSame(edge41, graph.InEdge(1, 2));
            Assert.AreSame(edge13, graph.InEdge(3, 0));
        }

        [Test]
        public void InEdge_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            var graph1 = new BidirectionalGraph<int, Edge<int>>();
            const int vertex1 = 1;
            const int vertex2 = 2;
            Assert.Throws<KeyNotFoundException>(() => graph1.InEdge(vertex1, 0));

            graph1.AddVertex(vertex1);
            graph1.AddVertex(vertex2);
            Assert.Throws<ArgumentOutOfRangeException>(() => graph1.InEdge(vertex1, 0));

            graph1.AddEdge(new Edge<int>(1, 2));
            Assert.Throws<ArgumentOutOfRangeException>(() => graph1.InEdge(vertex1, 5));

            var graph2 = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
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

            var graph = new BidirectionalGraph<int, Edge<int>>();
            AssertNoInEdge(graph, 1);
            AssertNoOutEdge(graph, 1);

            graph.AddVertex(1);
            AssertNoInEdge(graph, 1);
            AssertNoOutEdge(graph, 1);

            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge14, edge24, edge32, edge33 });

            AssertHasOutEdges(graph, 1, new[] { edge12, edge13, edge14 });
            AssertHasOutEdges(graph, 2, new[] { edge24 });
            AssertHasOutEdges(graph, 3, new[] { edge32, edge33 });
            AssertNoOutEdge(graph, 4);

            AssertNoInEdge(graph, 1);
            AssertHasInEdges(graph, 2, new[] { edge12, edge32 });
            AssertHasInEdges(graph, 3, new[] { edge13, edge33 });
            AssertHasInEdges(graph, 4, new [] { edge14, edge24 });
        }

        [Test]
        public void InEdges_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.IsInEdgesEmpty(null));
            Assert.Throws<ArgumentNullException>(() => graph.InDegree(null));
            Assert.Throws<ArgumentNullException>(() => graph.InEdges(null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
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

            var graph = new BidirectionalGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });

            graph.AddVertex(5);
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
            var graph = new BidirectionalGraph<int, Edge<int>>();
            TryGetEdge_Test(graph);
        }

        [Test]
        public void TryGetEdge_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            TryGetEdge_Throws_Test(graph);
        }

        [Test]
        public void TryGetEdges()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            TryGetEdges_Test(graph);
        }

        [Test]
        public void TryGetEdges_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            TryGetEdges_Throws_Test(graph);
        }

        [Test]
        public void TryGetOutEdges()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            TryGetOutEdges_Test(graph);
        }

        [Test]
        public void TryGetOutEdges_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
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

            var graph = new BidirectionalGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });

            Assert.IsFalse(graph.TryGetInEdges(0, out IEnumerable<Edge<int>> _));

            Assert.IsTrue(graph.TryGetInEdges(4, out IEnumerable<Edge<int>> gotEdges));
            CollectionAssert.AreEqual(new[] { edge5 }, gotEdges);

            Assert.IsTrue(graph.TryGetInEdges(2, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge1, edge2, edge4 }, gotEdges);
        }

        [Test]
        public void TryGetInEdges_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.TryGetInEdges(null, out _));
        }

        #endregion

        #region Remove Vertices

        [Test]
        public void RemoveVertex()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            RemoveVertex_Test(graph);
        }

        [Test]
        public void RemoveVertex_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            RemoveVertex_Throws_Test(graph);
        }

        [Test]
        public void RemoveVertexIf()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            RemoveVertexIf_Test(graph);
        }

        [Test]
        public void RemoveVertexIf_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            RemoveVertexIf_Throws_Test(graph);
        }

        #endregion

        #region Remove Edges

        [Test]
        public void RemoveEdge()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            RemoveEdge_Test(graph);
        }

        [Test]
        public void RemoveEdge_EquatableEdge()
        {
            var graph = new BidirectionalGraph<int, EquatableEdge<int>>();
            RemoveEdge_EquatableEdge_Test(graph);
        }

        [Test]
        public void RemoveEdge_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            RemoveEdge_Throws_Test(graph);
        }

        [Test]
        public void RemoveEdgeIf()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            RemoveEdgeIf_Test(graph);
        }

        [Test]
        public void RemoveEdgeIf_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            RemoveEdgeIf_Throws_Test(graph);
        }

        [Test]
        public void RemoveOutEdgeIf()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            RemoveOutEdgeIf_Test(graph);
        }

        [Test]
        public void RemoveOutEdgeIf_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            RemoveOutEdgeIf_Throws_Test(graph);
        }

        [Test]
        public void RemoveInEdgeIf()
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            var graph = new BidirectionalGraph<int, Edge<int>>();
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

            Assert.AreEqual(0, graph.RemoveInEdgeIf(1, edge => true));
            CheckCounters(0, 0);
            AssertEmptyGraph(graph);

            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge13Bis = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge13Bis, edge14, edge24, edge31, edge33 });

            Assert.AreEqual(2, graph.RemoveInEdgeIf(3, edge => edge.Source == 1));
            CheckCounters(0, 2);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge14, edge24, edge31, edge33 });

            Assert.AreEqual(0, graph.RemoveInEdgeIf(3, edge => edge.Target > 5));
            CheckCounters(0, 0);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge14, edge24, edge31, edge33 });

            Assert.AreEqual(1, graph.RemoveInEdgeIf(2, edge => true));
            CheckCounters(0, 1);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge14, edge24, edge31, edge33 });

            #region Local function

            void CheckCounters(int expectedRemovedVertices, int expectedRemovedEdges)
            {
                Assert.AreEqual(expectedRemovedVertices, verticesRemoved);
                Assert.AreEqual(expectedRemovedEdges, edgesRemoved);
                verticesRemoved = 0;
                edgesRemoved = 0;
            }

            #endregion
        }

        [Test]
        public void RemoveInEdgeIf_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();

            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.RemoveInEdgeIf(null, edge => true));
            Assert.Throws<ArgumentNullException>(() => graph.RemoveInEdgeIf(new TestVertex("v1"), null));
            Assert.Throws<ArgumentNullException>(() => graph.RemoveInEdgeIf(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
        }

        #endregion

        #region Clear

        [Test]
        public void Clear()
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            var graph = new BidirectionalGraph<int, Edge<int>>();
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

            graph.AddVerticesAndEdge(new Edge<int>(1, 2));
            graph.AddVerticesAndEdge(new Edge<int>(3, 2));
            graph.AddVerticesAndEdge(new Edge<int>(3, 1));
            graph.AddVerticesAndEdge(new Edge<int>(3, 3));

            graph.Clear();

            AssertEmptyGraph(graph);
            CheckCounters(3, 4);

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

        [Test]
        public void ClearOutEdges()
        {
            int edgesRemoved = 0;

            var graph = new BidirectionalGraph<int, Edge<int>>();
            graph.EdgeRemoved += e =>
            {
                Assert.IsNotNull(e);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            AssertEmptyGraph(graph);

            var edge12 = new Edge<int>(1, 2);
            var edge23 = new Edge<int>(2, 3);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge23 });

            // Clear out 1
            graph.ClearOutEdges(1);

            AssertHasEdges(graph, new[] { edge23 });
            CheckCounter(1);

            var edge13 = new Edge<int>(1, 3);
            var edge31 = new Edge<int>(3, 1);
            var edge32 = new Edge<int>(3, 2);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge31, edge32 });

            // Clear out 3
            graph.ClearOutEdges(3);

            AssertHasEdges(graph, new[] { edge12, edge13, edge23 });
            CheckCounter(2);

            // Clear out 1
            graph.ClearOutEdges(1);

            AssertHasEdges(graph, new[] { edge23 });
            CheckCounter(2);

            // Clear out 2 = Clear
            graph.ClearOutEdges(2);

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
        public void ClearOutEdges_Throws()
        {
            Assert.Throws<KeyNotFoundException>(() => new BidirectionalGraph<int, Edge<int>>().ClearOutEdges(1));
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new BidirectionalGraph<TestVertex, Edge<TestVertex>>().ClearOutEdges(null));
        }

        [Test]
        public void ClearInEdges()
        {
            int edgesRemoved = 0;

            var graph = new BidirectionalGraph<int, Edge<int>>();
            graph.EdgeRemoved += e =>
            {
                Assert.IsNotNull(e);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            AssertEmptyGraph(graph);

            var edge12 = new Edge<int>(1, 2);
            var edge23 = new Edge<int>(2, 3);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge23 });

            // Clear in 2
            graph.ClearInEdges(2);

            AssertHasEdges(graph, new[] { edge23 });
            CheckCounter(1);

            var edge13 = new Edge<int>(1, 3);
            var edge31 = new Edge<int>(3, 1);
            var edge32 = new Edge<int>(3, 2);
            graph.AddVerticesAndEdgeRange(new [] { edge12, edge13, edge31, edge32 });

            // Clear in 3
            graph.ClearInEdges(3);

            AssertHasEdges(graph, new[] { edge12, edge31, edge32 });
            CheckCounter(2);

            // Clear in 1
            graph.ClearInEdges(1);

            AssertHasEdges(graph, new[] { edge12, edge32 });
            CheckCounter(1);

            // Clear 2 = Clear
            graph.ClearInEdges(2);

            AssertNoEdge(graph);
            CheckCounter(2);

            #region Local function

            void CheckCounter(int expectedRemovedEdges)
            {
                Assert.AreEqual(expectedRemovedEdges, edgesRemoved);
                edgesRemoved = 0;
            }

            #endregion
        }

        [Test]
        public void ClearInEdges_Throws()
        {
            Assert.Throws<KeyNotFoundException>(() => new BidirectionalGraph<int, Edge<int>>().ClearInEdges(1));
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new BidirectionalGraph<TestVertex, Edge<TestVertex>>().ClearInEdges(null));
        }

        [Test]
        public void ClearEdges()
        {
            int edgesRemoved = 0;

            var graph = new BidirectionalGraph<int, Edge<int>>();
            graph.EdgeRemoved += e =>
            {
                Assert.IsNotNull(e);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            AssertEmptyGraph(graph);

            var edge12 = new Edge<int>(1, 2);
            var edge23 = new Edge<int>(2, 3);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge23 });

            // Clear 2
            graph.ClearEdges(2);

            AssertNoEdge(graph);
            CheckCounter(2);

            var edge13 = new Edge<int>(1, 3);
            var edge31 = new Edge<int>(3, 1);
            var edge32 = new Edge<int>(3, 2);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge31, edge32 });

            // Clear 3
            graph.ClearEdges(3);

            AssertHasEdges(graph, new[] { edge12 });
            CheckCounter(3);

            // Clear 1 = clear
            graph.ClearEdges(1);

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
        public void ClearEdges_Throws()
        {
            Assert.Throws<KeyNotFoundException>(() => new BidirectionalGraph<int, Edge<int>>().ClearEdges(1));
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new BidirectionalGraph<TestVertex, Edge<TestVertex>>().ClearEdges(null));
        }

        #endregion

        [Test]
        public void Clone()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            AssertEmptyGraph(graph);

            var clonedGraph = graph.Clone();
            Assert.IsNotNull(clonedGraph);
            AssertEmptyGraph(clonedGraph);

            clonedGraph = new BidirectionalGraph<int, Edge<int>>(graph);
            Assert.IsNotNull(clonedGraph);
            AssertEmptyGraph(clonedGraph);

            clonedGraph = (BidirectionalGraph<int, Edge<int>>)((ICloneable)graph).Clone();
            Assert.IsNotNull(clonedGraph);
            AssertEmptyGraph(clonedGraph);

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

            clonedGraph = new BidirectionalGraph<int, Edge<int>>(graph);
            Assert.IsNotNull(clonedGraph);
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3 });
            AssertHasEdges(clonedGraph, new[] { edge1, edge2, edge3 });

            clonedGraph = (BidirectionalGraph<int, Edge<int>>)((ICloneable)graph).Clone();
            Assert.IsNotNull(clonedGraph);
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3 });
            AssertHasEdges(clonedGraph, new[] { edge1, edge2, edge3 });
        }

        [Test]
        public void Clone_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new BidirectionalGraph<int, Edge<int>>(null));
        }

        [Test]
        public void TrimEdgeExcess()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>(true, 12);
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
