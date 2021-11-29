using System;
using NUnit.Framework;

namespace FastGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="DelegateIncidenceGraph{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class DelegateIncidenceGraphTests : DelegateGraphTestsBase
    {
        [Test]
        public void Construction()
        {
            var graph = new DelegateIncidenceGraph<int, Edge<int>>(
                GetEmptyGetter<int, Edge<int>>());
            AssertGraphProperties(graph);

            graph = new DelegateIncidenceGraph<int, Edge<int>>(
                GetEmptyGetter<int, Edge<int>>(),
                false);
            AssertGraphProperties(graph, false);

            #region Local function

            void AssertGraphProperties<TVertex, TEdge>(
                DelegateIncidenceGraph<TVertex, TEdge> g,
                bool parallelEdges = true)
                where TEdge : IEdge<TVertex>
            {
                Assert.IsTrue(g.IsDirected);
                Assert.AreEqual(parallelEdges, g.AllowParallelEdges);
            }

            #endregion
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new DelegateIncidenceGraph<int, Edge<int>>(null));
        }

        #region Contains Vertex

        [Test]
        public void ContainsVertex()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph = new DelegateIncidenceGraph<int, Edge<int>>(data.TryGetEdges);
            ContainsVertex_Test(data, graph);
        }

        [Test]
        public void ContainsVertex_Throws()
        {
            var graph = new DelegateIncidenceGraph<TestVertex, Edge<TestVertex>>(
                GetEmptyGetter<TestVertex, Edge<TestVertex>>());
            ContainsVertex_Throws_Test(graph);
        }

        #endregion

        #region Contains Edge

        [Test]
        public void ContainsEdge()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph = new DelegateIncidenceGraph<int, Edge<int>>(data.TryGetEdges);
            ContainsEdge_SourceTarget_Test(data, graph);
        }

        [Test]
        public void ContainsEdge_Throws()
        {
            var data = new GraphData<TestVertex, Edge<TestVertex>>();
            var graph = new DelegateIncidenceGraph<TestVertex, Edge<TestVertex>>(data.TryGetEdges);
            ContainsEdge_SourceTarget_Throws_Test(graph);
        }

        #endregion

        #region Out Edges

        [Test]
        public void OutEdge()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph = new DelegateIncidenceGraph<int, Edge<int>>(data.TryGetEdges);
            OutEdge_Test(data, graph);
        }

        [Test]
        public void OutEdge_Throws()
        {
            var graph1 = new DelegateIncidenceGraph<TestVertex, Edge<TestVertex>>(
                GetEmptyGetter<TestVertex, Edge<TestVertex>>());
            OutEdge_NullThrows_Test(graph1);

            var data = new GraphData<int, Edge<int>>();
            var graph2 = new DelegateIncidenceGraph<int, Edge<int>>(data.TryGetEdges);
            OutEdge_Throws_Test(data, graph2);
        }

        [Test]
        public void OutEdges()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph = new DelegateIncidenceGraph<int, Edge<int>>(data.TryGetEdges);
            OutEdges_Test(data, graph);
        }

        [Test]
        public void OutEdges_Throws()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph1 = new DelegateIncidenceGraph<int, Edge<int>>(data.TryGetEdges);
            OutEdges_Throws_Test(data, graph1);

            var graph2 = new DelegateIncidenceGraph<EquatableTestVertex, Edge<EquatableTestVertex>>(
                GetEmptyGetter<EquatableTestVertex, Edge<EquatableTestVertex>>());
            OutEdges_NullThrows_Test(graph2);
        }

        #endregion

        #region Try Get Edges

        [Test]
        public void TryGetEdge()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph = new DelegateIncidenceGraph<int, Edge<int>>(data.TryGetEdges);
            TryGetEdge_Test(data, graph);
        }

        [Test]
        public void TryGetEdge_Throws()
        {
            var data = new GraphData<TestVertex, Edge<TestVertex>>();
            var graph = new DelegateIncidenceGraph<TestVertex, Edge<TestVertex>>(data.TryGetEdges);
            TryGetEdge_Throws_Test(graph);
        }

        [Test]
        public void TryGetEdges()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph = new DelegateIncidenceGraph<int, Edge<int>>(data.TryGetEdges);
            TryGetEdges_Test(data, graph);
        }

        [Test]
        public void TryGetEdges_Throws()
        {
            var data = new GraphData<TestVertex, Edge<TestVertex>>();
            var graph = new DelegateIncidenceGraph<TestVertex, Edge<TestVertex>>(data.TryGetEdges);
            TryGetEdges_Throws_Test(graph);
        }

        [Test]
        public void TryGetOutEdges()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph = new DelegateIncidenceGraph<int, Edge<int>>(data.TryGetEdges);
            TryGetOutEdges_Test(data, graph);
        }

        [Test]
        public void TryGetOutEdges_Throws()
        {
            var graph = new DelegateIncidenceGraph<TestVertex, Edge<TestVertex>>(
                GetEmptyGetter<TestVertex, Edge<TestVertex>>());
            TryGetOutEdges_Throws_Test(graph);
        }

        #endregion
    }
}