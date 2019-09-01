using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NUnit.Framework;
using static QuikGraph.Tests.AssertHelpers;

namespace QuikGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="DelegateImplicitGraph{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class DelegateImplicitGraphTests : GraphTestsBase
    {
        [Pure]
        [NotNull]
        private static TryFunc<TVertex, IEnumerable<TEdge>> GetEmptyGraph<TVertex, TEdge>()
            where TEdge : IEdge<TVertex>
        {
            return (TVertex vertex, out IEnumerable<TEdge> outEdges) =>
            {
                outEdges = Enumerable.Empty<TEdge>();
                return false;
            };
        }

        private class GraphData<TVertex, TEdge>
            where TEdge : IEdge<TVertex>
        {
            public GraphData()
            {
                TryGetEdges = (TVertex vertex, out IEnumerable<TEdge> outEdges) =>
                {
                    ++_nbCalls;

                    if (ShouldReturnValue && ShouldReturnEdges is null)
                        outEdges = Enumerable.Empty<TEdge>();
                    else
                        outEdges = ShouldReturnEdges;

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

        [Test]
        public void Construction()
        {
            var graph = new DelegateImplicitGraph<int, Edge<int>>(
                GetEmptyGraph<int, Edge<int>>());
            Assert.IsTrue(graph.IsDirected);
            Assert.IsTrue(graph.AllowParallelEdges);

            graph = new DelegateImplicitGraph<int, Edge<int>>(
                GetEmptyGraph<int, Edge<int>>(),
                false);
            Assert.IsTrue(graph.IsDirected);
            Assert.IsFalse(graph.AllowParallelEdges);
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new DelegateImplicitGraph<int, Edge<int>>(null));
        }

        #region Contains Vertex

        [Test]
        public void ContainsVertex()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph = new DelegateImplicitGraph<int, Edge<int>>(data.TryGetEdges);
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Assert.IsFalse(graph.ContainsVertex(1));
            data.CheckCalls(1);

            data.ShouldReturnValue = true;
            Assert.IsTrue(graph.ContainsVertex(1));
            data.CheckCalls(1);
        }

        [Test]
        public void ContainsVertex_Throws()
        {
            var graph = new DelegateImplicitGraph<TestVertex, Edge<TestVertex>>(
                GetEmptyGraph<TestVertex, Edge<TestVertex>>());
            ContainsVertex_Throws_Test(graph);
        }

        #endregion

        #region Out Edges

        [Test]
        public void OutEdge()
        {
            var edge11 = new Edge<int>(1, 1);
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);

            var data = new GraphData<int, Edge<int>>();
            var graph = new DelegateImplicitGraph<int, Edge<int>>(data.TryGetEdges);
            data.CheckCalls(0);

            data.ShouldReturnValue = true;
            data.ShouldReturnEdges = new[] { edge11, edge12, edge13 };
            Assert.AreSame(edge11, graph.OutEdge(1, 0));
            data.CheckCalls(1);

            Assert.AreSame(edge13, graph.OutEdge(1, 2));
            data.CheckCalls(1);
        }

        [Test]
        public void OutEdge_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            var data = new GraphData<int, Edge<int>>();
            var graph1 = new DelegateImplicitGraph<int, Edge<int>>(data.TryGetEdges);
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Assert.Throws<VertexNotFoundException>(() => graph1.OutEdge(1, 0));
            data.CheckCalls(1);

            data.ShouldReturnValue = true;
            AssertIndexOutOfRange(() => graph1.OutEdge(1, 0));
            data.CheckCalls(1);

            data.ShouldReturnEdges = new[] { new Edge<int>(1, 2) };
            AssertIndexOutOfRange(() => graph1.OutEdge(1, 1));
            data.CheckCalls(1);
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            var graph2 = new DelegateImplicitGraph<TestVertex, Edge<TestVertex>>(
                GetEmptyGraph<TestVertex, Edge<TestVertex>>());
            OutEdge_NullThrows_Test(graph2);
        }

        [Test]
        public void OutEdges()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph = new DelegateImplicitGraph<int, Edge<int>>(data.TryGetEdges);
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

        [Test]
        public void OutEdges_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            var data = new GraphData<int, Edge<int>>();
            var graph1 = new DelegateImplicitGraph<int, Edge<int>>(data.TryGetEdges);
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Assert.Throws<VertexNotFoundException>(() => graph1.IsOutEdgesEmpty(1));
            data.CheckCalls(1);

            Assert.Throws<VertexNotFoundException>(() => graph1.OutDegree(1));
            data.CheckCalls(1);

            Assert.Throws<VertexNotFoundException>(() => graph1.OutEdges(1));
            data.CheckCalls(1);
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            var graph2 = new DelegateImplicitGraph<EquatableTestVertex, Edge<EquatableTestVertex>>(
                GetEmptyGraph<EquatableTestVertex, Edge<EquatableTestVertex>>());
            OutEdges_NullThrows_Test(graph2);
        }

        #endregion

        #region Try Get Edges

        [Test]
        public void TryGetOutEdges()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph = new DelegateImplicitGraph<int, Edge<int>>(data.TryGetEdges);
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Assert.IsFalse(graph.TryGetOutEdges(1, out _));
            data.CheckCalls(1);

            data.ShouldReturnValue = true;
            Assert.IsTrue(graph.TryGetOutEdges(1, out _));
            data.CheckCalls(1);
        }

        [Test]
        public void TryGetOutEdges_Throws()
        {
            var graph = new DelegateImplicitGraph<TestVertex, Edge<TestVertex>>(
                GetEmptyGraph<TestVertex, Edge<TestVertex>>());
            TryGetOutEdges_Throws_Test(graph);
        }

        #endregion
    }
}
