using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NUnit.Framework;
using static QuikGraph.Tests.AssertHelpers;

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
        protected static TryFunc<TVertex, IEnumerable<TEdge>> GetEmptyGraph<TVertex, TEdge>()
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

        private static void ContainsEdge_GenericTest(
            [NotNull] GraphData<int, Edge<int>> data,
            [NotNull, InstantHandle] Func<int, int, bool> hasEdge)
        {
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Assert.IsFalse(hasEdge(1, 2));
            data.CheckCalls(1);

            data.ShouldReturnValue = true;
            Assert.IsFalse(hasEdge(1, 2));
            data.CheckCalls(1);

            data.ShouldReturnEdges = new[] { new Edge<int>(1, 3), new Edge<int>(1, 2) };
            Assert.IsTrue(hasEdge(1, 2));
            data.CheckCalls(1);
        }

        protected static void ContainsEdge_Test(
            [NotNull] GraphData<int, Edge<int>> data,
            [NotNull] IIncidenceGraph<int, Edge<int>> graph)
        {
            ContainsEdge_GenericTest(
                data,
                graph.ContainsEdge);
        }

        protected static void ContainsEdge_Test(
            [NotNull] GraphData<int, Edge<int>> data,
            [NotNull] IImplicitUndirectedGraph<int, Edge<int>> graph)
        {
            ContainsEdge_GenericTest(
                data,
                graph.ContainsEdge);
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

        #region Try Get Edges

        protected static void TryGetEdge_Test(
            [NotNull] GraphData<int, Edge<int>> data,
            [NotNull] IIncidenceGraph<int, Edge<int>> graph)
        {
            ContainsEdge_GenericTest(
                data,
                (source, target) => graph.TryGetEdge(source, target, out _));
        }

        protected static void TryGetEdge_Test(
            [NotNull] GraphData<int, Edge<int>> data,
            [NotNull] IImplicitUndirectedGraph<int, Edge<int>> graph)
        {
            ContainsEdge_GenericTest(
                data,
                (source, target) => graph.TryGetEdge(source, target, out _));
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
            Assert.IsTrue(graph.TryGetEdges(1, 2, out _));
            data.CheckCalls(1);

            data.ShouldReturnEdges = new[] { new Edge<int>(1, 2), new Edge<int>(1, 2) };
            Assert.IsTrue(graph.TryGetEdges(1, 2, out _));
            data.CheckCalls(1);
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
            Assert.IsTrue(graph.TryGetOutEdges(1, out _));
            data.CheckCalls(1);

            data.ShouldReturnEdges = new[] { new Edge<int>(1, 4), new Edge<int>(1, 2) };
            Assert.IsTrue(graph.TryGetOutEdges(1, out _));
            data.CheckCalls(1);
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
            Assert.IsTrue(graph.TryGetAdjacentEdges(1, out _));
            data.CheckCalls(1);

            data.ShouldReturnEdges = new[] { new Edge<int>(1, 4), new Edge<int>(1, 2) };
            Assert.IsTrue(graph.TryGetAdjacentEdges(1, out _));
            data.CheckCalls(1);
        }

        protected static void TryGetAdjacentEdges_Throws_Test<TVertex, TEdge>(
            [NotNull] DelegateImplicitUndirectedGraph<TVertex, TEdge> graph)
            where TVertex : class
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.TryGetAdjacentEdges(null, out _));
        }

        #endregion

        #endregion
    }
}
