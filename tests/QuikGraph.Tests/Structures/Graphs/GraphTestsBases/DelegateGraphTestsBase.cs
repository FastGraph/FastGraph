using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NUnit.Framework;
using static QuikGraph.Tests.AssertHelpers;
using static QuikGraph.Tests.GraphTestHelpers;

namespace QuikGraph.Tests.Structures
{
    /// <summary>
    /// Base class for tests over delegated graphs.
    /// </summary>
    [TestFixture]
    internal class DelegateGraphTestsBase : GraphTestsBase
    {
        [Pure]
        [NotNull]
        protected static TryFunc<TVertex, IEnumerable<TEdge>> GetEmptyGetter<TVertex, TEdge>()
            where TEdge : IEdge<TVertex>
        {
            return (TVertex vertex, out IEnumerable<TEdge> edges) =>
            {
                edges = null;
                return false;
            };
        }

        protected class GraphData<TVertex, TEdge>
            where TEdge : IEdge<TVertex>
        {
            public GraphData()
            {
                TryGetEdges = (TVertex vertex, out IEnumerable<TEdge> edges) =>
                {
                    ++_nbCalls;

                    if (ShouldReturnValue)
                        edges = ShouldReturnEdges ?? Enumerable.Empty<TEdge>();
                    else
                        edges = null;

                    return ShouldReturnValue;
                };
            }

            private int _nbCalls;

            [NotNull] 
            public TryFunc<TVertex, IEnumerable<TEdge>> TryGetEdges { get; }

            [CanBeNull, ItemNotNull]
            public IEnumerable<TEdge> ShouldReturnEdges { get; set; }

            public bool ShouldReturnValue { get; set; }

            public void CheckCalls(int expectedCalls)
            {
                Assert.AreEqual(expectedCalls, _nbCalls);
                _nbCalls = 0;
            }
        }

        #region Test helpers

        #region Contains Vertex

        protected static void ContainsVertex_Test(
            [NotNull] GraphData<int, Edge<int>> data,
            [NotNull] IImplicitVertexSet<int> graph)
        {
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Assert.IsFalse(graph.ContainsVertex(1));
            data.CheckCalls(1);

            data.ShouldReturnValue = true;
            Assert.IsTrue(graph.ContainsVertex(1));
            data.CheckCalls(1);
        }

        #endregion

        #region Contains Edge

        protected static void ContainsEdge_Test(
            [NotNull] GraphData<int, Edge<int>> data,
            [NotNull] IEdgeSet<int, Edge<int>> graph)
        {
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            var edge12 = new Edge<int>(1, 2);
            var edge21 = new Edge<int>(2, 1);
            Assert.IsFalse(graph.ContainsEdge(edge12));
            data.CheckCalls(1);
            Assert.IsFalse(graph.ContainsEdge(edge21));
            data.CheckCalls(1);

            data.ShouldReturnValue = true;
            Assert.IsFalse(graph.ContainsEdge(edge12));
            data.CheckCalls(1);
            Assert.IsFalse(graph.ContainsEdge(edge21));
            data.CheckCalls(1);

            var edge13 = new Edge<int>(1, 3);
            data.ShouldReturnEdges = new[] { edge12, edge13, edge21 };
            Assert.IsTrue(graph.ContainsEdge(edge12));
            data.CheckCalls(1);
            Assert.IsTrue(graph.ContainsEdge(edge21));
            data.CheckCalls(1);

            var edge15 = new Edge<int>(1, 5);
            var edge51 = new Edge<int>(5, 1);
            var edge56 = new Edge<int>(5, 6);
            Assert.IsFalse(graph.ContainsEdge(edge15));
            Assert.IsFalse(graph.ContainsEdge(edge51));
            Assert.IsFalse(graph.ContainsEdge(edge56));
        }

        private static void ContainsEdge_SourceTarget_GenericTest(
            [NotNull] GraphData<int, Edge<int>> data,
            [NotNull, InstantHandle] Func<int, int, bool> hasEdge,
            bool isDirected = true)
        {
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Assert.IsFalse(hasEdge(1, 2));
            data.CheckCalls(1);
            Assert.IsFalse(hasEdge(2, 1));
            data.CheckCalls(1);

            data.ShouldReturnValue = true;
            Assert.IsFalse(hasEdge(1, 2));
            data.CheckCalls(1);
            Assert.IsFalse(hasEdge(2, 1));
            data.CheckCalls(1);

            data.ShouldReturnEdges = new[] { new Edge<int>(1, 3), new Edge<int>(1, 2) };
            Assert.IsTrue(hasEdge(1, 2));
            data.CheckCalls(1);
            if (isDirected)
                Assert.IsFalse(hasEdge(2, 1));
            else
                Assert.IsTrue(hasEdge(2, 1));
            data.CheckCalls(1);

            Assert.IsFalse(hasEdge(1, 5));
            Assert.IsFalse(hasEdge(5, 1));
            Assert.IsFalse(hasEdge(5, 6));
        }

        protected static void ContainsEdge_SourceTarget_Test(
            [NotNull] GraphData<int, Edge<int>> data,
            [NotNull] IIncidenceGraph<int, Edge<int>> graph)
        {
            ContainsEdge_SourceTarget_GenericTest(
                data,
                graph.ContainsEdge);
        }

        protected static void ContainsEdge_SourceTarget_UndirectedGraph_Test(
            [NotNull] GraphData<int, Edge<int>> data,
            [NotNull] IImplicitUndirectedGraph<int, Edge<int>> graph)
        {
            ContainsEdge_SourceTarget_GenericTest(
                data,
                graph.ContainsEdge,
                false);
        }

        #endregion

        #region Out Edges

        protected static void OutEdge_Test(
            [NotNull] GraphData<int, Edge<int>> data,
            [NotNull] IImplicitGraph<int, Edge<int>> graph)
        {
            var edge11 = new Edge<int>(1, 1);
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);

            data.CheckCalls(0);

            data.ShouldReturnValue = true;
            data.ShouldReturnEdges = new[] { edge11, edge12, edge13 };
            Assert.AreSame(edge11, graph.OutEdge(1, 0));
            data.CheckCalls(1);

            Assert.AreSame(edge13, graph.OutEdge(1, 2));
            data.CheckCalls(1);
        }

        protected static void OutEdge_Throws_Test(
            [NotNull] GraphData<int, Edge<int>> data,
            [NotNull] IImplicitGraph<int, Edge<int>> graph)
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Assert.Throws<VertexNotFoundException>(() => graph.OutEdge(1, 0));
            data.CheckCalls(1);

            data.ShouldReturnValue = true;
            AssertIndexOutOfRange(() => graph.OutEdge(1, 0));
            data.CheckCalls(1);

            data.ShouldReturnEdges = new[] { new Edge<int>(1, 2) };
            AssertIndexOutOfRange(() => graph.OutEdge(1, 1));
            data.CheckCalls(1);
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        protected static void OutEdges_Test(
            [NotNull] GraphData<int, Edge<int>> data,
            [NotNull] IImplicitGraph<int, Edge<int>> graph)
        {
            data.CheckCalls(0);

            data.ShouldReturnValue = true;
            AssertNoOutEdge(graph, 1);
            data.CheckCalls(3);

            var edges = new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(1, 3)
            };
            data.ShouldReturnEdges = edges;
            AssertHasOutEdges(graph, 1, edges);
            data.CheckCalls(3);
        }

        protected static void OutEdges_Throws_Test(
            [NotNull] GraphData<int, Edge<int>> data,
            [NotNull] IImplicitGraph<int, Edge<int>> graph)
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Assert.Throws<VertexNotFoundException>(() => graph.IsOutEdgesEmpty(1));
            data.CheckCalls(1);

            Assert.Throws<VertexNotFoundException>(() => graph.OutDegree(1));
            data.CheckCalls(1);

            Assert.Throws<VertexNotFoundException>(() => graph.OutEdges(1));
            data.CheckCalls(1);
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        #endregion

        #region Adjacent Edges

        protected static void AdjacentEdge_Test(
            [NotNull] GraphData<int, Edge<int>> data,
            [NotNull] IImplicitUndirectedGraph<int, Edge<int>> graph)
        {
            var edge11 = new Edge<int>(1, 1);
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);

            data.CheckCalls(0);

            data.ShouldReturnValue = true;
            data.ShouldReturnEdges = new[] { edge11, edge12, edge13 };
            Assert.AreSame(edge11, graph.AdjacentEdge(1, 0));
            data.CheckCalls(1);

            Assert.AreSame(edge13, graph.AdjacentEdge(1, 2));
            data.CheckCalls(1);
        }

        protected static void AdjacentEdge_Throws_Test(
            [NotNull] GraphData<int, Edge<int>> data,
            [NotNull] IImplicitUndirectedGraph<int, Edge<int>> graph)
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Assert.Throws<VertexNotFoundException>(() => graph.AdjacentEdge(1, 0));
            data.CheckCalls(1);

            data.ShouldReturnValue = true;
            AssertIndexOutOfRange(() => graph.AdjacentEdge(1, 0));
            data.CheckCalls(1);

            data.ShouldReturnEdges = new[] { new Edge<int>(1, 2) };
            AssertIndexOutOfRange(() => graph.AdjacentEdge(1, 1));
            data.CheckCalls(1);
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        protected static void AdjacentEdges_Test(
            [NotNull] GraphData<int, Edge<int>> data,
            [NotNull] IImplicitUndirectedGraph<int, Edge<int>> graph)
        {
            data.CheckCalls(0);

            data.ShouldReturnValue = true;
            AssertNoAdjacentEdge(graph, 1);
            data.CheckCalls(3);

            var edges = new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(1, 3)
            };
            data.ShouldReturnEdges = edges;
            AssertHasAdjacentEdges(graph, 1, edges);
            data.CheckCalls(3);
        }

        protected static void AdjacentEdges_Throws_Test(
            [NotNull] GraphData<int, Edge<int>> data,
            [NotNull] IImplicitUndirectedGraph<int, Edge<int>> graph)
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Assert.Throws<VertexNotFoundException>(() => graph.IsAdjacentEdgesEmpty(1));
            data.CheckCalls(1);

            Assert.Throws<VertexNotFoundException>(() => graph.AdjacentDegree(1));
            data.CheckCalls(1);

            Assert.Throws<VertexNotFoundException>(() => graph.AdjacentEdges(1));
            data.CheckCalls(1);
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        #endregion

        #region Out Edges

        protected static void InEdge_Test(
            [NotNull] GraphData<int, Edge<int>> data,
            [NotNull] IBidirectionalIncidenceGraph<int, Edge<int>> graph)
        {
            var edge11 = new Edge<int>(1, 1);
            var edge21 = new Edge<int>(2, 1);
            var edge31 = new Edge<int>(3, 1);

            data.CheckCalls(0);

            data.ShouldReturnValue = true;
            data.ShouldReturnEdges = new[] { edge11, edge21, edge31 };
            Assert.AreSame(edge11, graph.InEdge(1, 0));
            data.CheckCalls(1);

            Assert.AreSame(edge31, graph.InEdge(1, 2));
            data.CheckCalls(1);
        }

        protected static void InEdge_Throws_Test(
            [NotNull] GraphData<int, Edge<int>> data,
            [NotNull] IBidirectionalIncidenceGraph<int, Edge<int>> graph)
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Assert.Throws<VertexNotFoundException>(() => graph.InEdge(1, 0));
            data.CheckCalls(1);

            data.ShouldReturnValue = true;
            AssertIndexOutOfRange(() => graph.InEdge(1, 0));
            data.CheckCalls(1);

            data.ShouldReturnEdges = new[] { new Edge<int>(1, 2) };
            AssertIndexOutOfRange(() => graph.InEdge(1, 1));
            data.CheckCalls(1);
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        protected static void InEdges_Test(
            [NotNull] GraphData<int, Edge<int>> data,
            [NotNull] IBidirectionalIncidenceGraph<int, Edge<int>> graph)
        {
            data.CheckCalls(0);

            data.ShouldReturnValue = true;
            AssertNoInEdge(graph, 1);
            data.CheckCalls(3);

            var edges = new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(1, 3)
            };
            data.ShouldReturnEdges = edges;
            AssertHasInEdges(graph, 1, edges);
            data.CheckCalls(3);
        }

        protected static void InEdges_Throws_Test(
            [NotNull] GraphData<int, Edge<int>> data,
            [NotNull] IBidirectionalIncidenceGraph<int, Edge<int>> graph)
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Assert.Throws<VertexNotFoundException>(() => graph.IsInEdgesEmpty(1));
            data.CheckCalls(1);

            Assert.Throws<VertexNotFoundException>(() => graph.InDegree(1));
            data.CheckCalls(1);

            Assert.Throws<VertexNotFoundException>(() => graph.InEdges(1));
            data.CheckCalls(1);
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        #endregion

        #region Degree

        protected static void Degree_Test(
            [NotNull] GraphData<int, Edge<int>> data1,
            [NotNull] GraphData<int, Edge<int>> data2,
            [NotNull] IBidirectionalIncidenceGraph<int, Edge<int>> graph)
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            data1.CheckCalls(0);
            data2.CheckCalls(0);

            data1.ShouldReturnValue = false;
            data2.ShouldReturnValue = false;
            Assert.Throws<VertexNotFoundException>(() => graph.Degree(1));
            data1.CheckCalls(0);
            data2.CheckCalls(1);

            data1.ShouldReturnValue = true;
            data2.ShouldReturnValue = false;
            Assert.Throws<VertexNotFoundException>(() => graph.Degree(1));
            data1.CheckCalls(0);
            data2.CheckCalls(1);

            data1.ShouldReturnValue = false;
            data2.ShouldReturnValue = true;
            Assert.Throws<VertexNotFoundException>(() => graph.Degree(1));
            data1.CheckCalls(1);
            data2.CheckCalls(1);
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            data1.ShouldReturnValue = true;
            data2.ShouldReturnValue = true;
            Assert.AreEqual(0, graph.Degree(1));

            data1.ShouldReturnEdges = new[] { new Edge<int>(1, 2) };
            data2.ShouldReturnEdges = null;
            Assert.AreEqual(1, graph.Degree(1));

            data1.ShouldReturnEdges = null;
            data2.ShouldReturnEdges = new[] { new Edge<int>(3, 1) };
            Assert.AreEqual(1, graph.Degree(1));

            data1.ShouldReturnEdges = new[] { new Edge<int>(1, 2), new Edge<int>(1, 3) };
            data2.ShouldReturnEdges = new[] { new Edge<int>(4, 1) };
            Assert.AreEqual(3, graph.Degree(1));

            // Self edge
            data1.ShouldReturnEdges = new[] { new Edge<int>(1, 2), new Edge<int>(1, 3), new Edge<int>(1, 1) };
            data2.ShouldReturnEdges = new[] { new Edge<int>(4, 1), new Edge<int>(1, 1) };
            Assert.AreEqual(5, graph.Degree(1));
        }

        #endregion

        #region Try Get Edges

        protected static void TryGetEdge_Test(
            [NotNull] GraphData<int, Edge<int>> data,
            [NotNull] IIncidenceGraph<int, Edge<int>> graph)
        {
            ContainsEdge_SourceTarget_GenericTest(
                data,
                (source, target) => graph.TryGetEdge(source, target, out _));
        }

        protected static void TryGetEdge_UndirectedGraph_Test(
            [NotNull] GraphData<int, Edge<int>> data,
            [NotNull] IImplicitUndirectedGraph<int, Edge<int>> graph)
        {
            ContainsEdge_SourceTarget_GenericTest(
                data,
                (source, target) => graph.TryGetEdge(source, target, out _),
                false);
        }

        protected static void TryGetEdges_Test(
            [NotNull] GraphData<int, Edge<int>> data,
            [NotNull] IIncidenceGraph<int, Edge<int>> graph)
        {
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Assert.IsFalse(graph.TryGetEdges(0, 1, out _));
            data.CheckCalls(1);

            data.ShouldReturnValue = true;
            Assert.IsTrue(graph.TryGetEdges(1, 2, out IEnumerable<Edge<int>> edges));
            CollectionAssert.IsEmpty(edges);
            data.CheckCalls(1);

            data.ShouldReturnEdges = new[] { new Edge<int>(1, 2), new Edge<int>(1, 2) };
            Assert.IsTrue(graph.TryGetEdges(1, 2, out edges));
            CollectionAssert.AreEqual(data.ShouldReturnEdges, edges);
            data.CheckCalls(1);
        }

        protected static void TryGetEdges_Test(
            [NotNull] GraphData<int, Edge<int>> data,
            [NotNull] DelegateVertexAndEdgeListGraph<int, Edge<int>> graph)
        {
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Assert.IsFalse(graph.TryGetEdges(0, 1, out _));
            data.CheckCalls(0); // Vertex is not in graph so no need to call user code

            data.ShouldReturnValue = true;
            Assert.IsTrue(graph.TryGetEdges(1, 2, out IEnumerable<Edge<int>> edges));
            CollectionAssert.IsEmpty(edges);
            data.CheckCalls(1);

            data.ShouldReturnEdges = new[] { new Edge<int>(1, 2), new Edge<int>(1, 2) };
            Assert.IsTrue(graph.TryGetEdges(1, 2, out edges));
            CollectionAssert.AreEqual(data.ShouldReturnEdges, edges);
            data.CheckCalls(1);

            var edge14 = new Edge<int>(1, 4);
            var edge12 = new Edge<int>(1, 2);
            var edge12Bis = new Edge<int>(1, 2);
            data.ShouldReturnValue = true;
            data.ShouldReturnEdges = new[] { edge14, edge12 };
            Assert.IsTrue(graph.TryGetEdges(1, 2, out edges));
            CollectionAssert.AreEqual(new[] { edge12 }, edges);
            data.CheckCalls(1);

            data.ShouldReturnEdges = new[] { edge14, edge12, edge12Bis };
            Assert.IsTrue(graph.TryGetEdges(1, 2, out edges));
            CollectionAssert.AreEqual(new[] { edge12, edge12Bis }, edges);
            data.CheckCalls(1);

            data.ShouldReturnEdges = new[] { edge14, edge12 };
            Assert.IsTrue(graph.TryGetEdges(2, 1, out edges));
            CollectionAssert.IsEmpty(edges);
            data.CheckCalls(1);

            var edge41 = new Edge<int>(4, 1);
            data.ShouldReturnEdges = new[] { edge14, edge41 };
            Assert.IsTrue(graph.TryGetEdges(1, 4, out edges));
            CollectionAssert.IsEmpty(edges);
            data.CheckCalls(1);

            Assert.IsFalse(graph.TryGetEdges(4, 1, out _));
            data.CheckCalls(0);

            var edge45 = new Edge<int>(4, 5);
            data.ShouldReturnEdges = new[] { edge14, edge41, edge45 };
            Assert.IsFalse(graph.TryGetEdges(4, 5, out _));
            data.CheckCalls(0);
        }

        protected static void TryGetOutEdges_Test(
            [NotNull] GraphData<int, Edge<int>> data,
            [NotNull] IImplicitGraph<int, Edge<int>> graph)
        {
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Assert.IsFalse(graph.TryGetOutEdges(1, out _));
            data.CheckCalls(1);

            data.ShouldReturnValue = true;
            Assert.IsTrue(graph.TryGetOutEdges(1, out IEnumerable<Edge<int>> edges));
            CollectionAssert.IsEmpty(edges);
            data.CheckCalls(1);

            data.ShouldReturnEdges = new[] { new Edge<int>(1, 4), new Edge<int>(1, 2) };
            Assert.IsTrue(graph.TryGetOutEdges(1, out edges));
            CollectionAssert.AreEqual(data.ShouldReturnEdges, edges);
            data.CheckCalls(1);
        }

        protected static void TryGetOutEdges_Test(
            [NotNull] GraphData<int, Edge<int>> data,
            [NotNull] DelegateVertexAndEdgeListGraph<int, Edge<int>> graph)
        {
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Assert.IsFalse(graph.TryGetOutEdges(5, out _));
            data.CheckCalls(0); // Vertex is not in graph so no need to call user code

            data.ShouldReturnValue = true;
            Assert.IsTrue(graph.TryGetOutEdges(1, out IEnumerable<Edge<int>> edges));
            CollectionAssert.IsEmpty(edges);
            data.CheckCalls(1);

            data.ShouldReturnEdges = new[] { new Edge<int>(1, 4), new Edge<int>(1, 2) };
            Assert.IsTrue(graph.TryGetOutEdges(1, out edges));
            CollectionAssert.AreEqual(data.ShouldReturnEdges, edges);
            data.CheckCalls(1);

            data.ShouldReturnEdges = null;
            Assert.IsTrue(graph.TryGetOutEdges(1, out IEnumerable<Edge<int>> outEdges));
            CollectionAssert.IsEmpty(outEdges);
            data.CheckCalls(1);

            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge15 = new Edge<int>(1, 5);
            var edge21 = new Edge<int>(2, 1);
            var edge23 = new Edge<int>(2, 3);
            data.ShouldReturnEdges = new[] { edge12, edge13, edge15, edge21, edge23 };
            Assert.IsTrue(graph.TryGetOutEdges(1, out outEdges));
            CollectionAssert.AreEqual(
                new[] { edge12, edge13 },
                outEdges);
            data.CheckCalls(1);

            var edge52 = new Edge<int>(5, 2);
            data.ShouldReturnEdges = new[] { edge15, edge52 };
            Assert.IsFalse(graph.TryGetOutEdges(5, out _));
            data.CheckCalls(0); // Vertex is not in graph so no need to call user code
        }

        protected static void TryGetAdjacentEdges_Test(
            [NotNull] GraphData<int, Edge<int>> data,
            [NotNull] DelegateImplicitUndirectedGraph<int, Edge<int>> graph)
        {
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Assert.IsFalse(graph.TryGetAdjacentEdges(1, out _));
            data.CheckCalls(1);

            data.ShouldReturnValue = true;
            Assert.IsTrue(graph.TryGetAdjacentEdges(1, out IEnumerable<Edge<int>> edges));
            CollectionAssert.IsEmpty(edges);
            data.CheckCalls(1);

            data.ShouldReturnEdges = new[] { new Edge<int>(1, 4), new Edge<int>(1, 2) };
            Assert.IsTrue(graph.TryGetAdjacentEdges(1, out edges));
            CollectionAssert.AreEqual(data.ShouldReturnEdges, edges);
            data.CheckCalls(1);
        }

        protected static void TryGetAdjacentEdges_Test(
            [NotNull] GraphData<int, Edge<int>> data,
            [NotNull] DelegateUndirectedGraph<int, Edge<int>> graph)
        {
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Assert.IsFalse(graph.TryGetAdjacentEdges(5, out _));
            data.CheckCalls(0); // Vertex is not in graph so no need to call user code

            data.ShouldReturnValue = true;
            Assert.IsTrue(graph.TryGetAdjacentEdges(1, out IEnumerable<Edge<int>> edges));
            CollectionAssert.IsEmpty(edges);
            data.CheckCalls(1);

            data.ShouldReturnEdges = new[] { new Edge<int>(1, 4), new Edge<int>(1, 2) };
            Assert.IsTrue(graph.TryGetAdjacentEdges(1, out edges));
            CollectionAssert.AreEqual(data.ShouldReturnEdges, edges);
            data.CheckCalls(1);

            data.ShouldReturnEdges = null;
            Assert.IsTrue(graph.TryGetAdjacentEdges(1, out IEnumerable<Edge<int>> adjacentEdges));
            CollectionAssert.IsEmpty(adjacentEdges);
            data.CheckCalls(1);

            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge15 = new Edge<int>(1, 5);
            var edge21 = new Edge<int>(2, 1);
            var edge23 = new Edge<int>(2, 3);
            data.ShouldReturnEdges = new[] { edge12, edge13, edge15, edge21, edge23 };
            Assert.IsTrue(graph.TryGetAdjacentEdges(1, out adjacentEdges));
            CollectionAssert.AreEqual(
                new[] { edge12, edge13, edge21 },
                adjacentEdges);
            data.CheckCalls(1);

            var edge52 = new Edge<int>(5, 2);
            data.ShouldReturnEdges = new[] { edge15, edge52 };
            Assert.IsFalse(graph.TryGetAdjacentEdges(5, out _));
            data.CheckCalls(0); // Vertex is not in graph so no need to call user code
        }

        protected static void TryGetAdjacentEdges_Throws_Test<TVertex, TEdge>(
            [NotNull] DelegateImplicitUndirectedGraph<TVertex, TEdge> graph)
            where TVertex : class
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.TryGetAdjacentEdges(null, out _));
        }

        protected static void TryGetInEdges_Test(
            [NotNull] GraphData<int, Edge<int>> data,
            [NotNull] IBidirectionalIncidenceGraph<int, Edge<int>> graph)
        {
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Assert.IsFalse(graph.TryGetInEdges(1, out _));
            data.CheckCalls(1);

            data.ShouldReturnValue = true;
            Assert.IsTrue(graph.TryGetInEdges(1, out IEnumerable<Edge<int>> edges));
            CollectionAssert.IsEmpty(edges);
            data.CheckCalls(1);

            data.ShouldReturnEdges = new[] { new Edge<int>(4, 1), new Edge<int>(2, 1) };
            Assert.IsTrue(graph.TryGetInEdges(1, out edges));
            CollectionAssert.AreEqual(data.ShouldReturnEdges, edges);
            data.CheckCalls(1);
        }

        #endregion

        #endregion
    }
}