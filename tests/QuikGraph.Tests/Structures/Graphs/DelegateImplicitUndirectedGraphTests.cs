using System;
using NUnit.Framework;

namespace QuikGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="DelegateImplicitUndirectedGraph{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class DelegateImplicitUndirectedGraphTests : DelegateGraphTestsBase
    {
        [Test]
        public void Construction()
        {
            var graph = new DelegateImplicitUndirectedGraph<int, Edge<int>>(
                GetEmptyGetter<int, Edge<int>>());
            AssertGraphProperties(graph);

            graph = new DelegateImplicitUndirectedGraph<int, Edge<int>>(
                GetEmptyGetter<int, Edge<int>>(),
                false);
            AssertGraphProperties(graph, false);

            #region Local function

            void AssertGraphProperties<TVertex, TEdge>(
                DelegateImplicitUndirectedGraph<TVertex, TEdge> g,
                bool parallelEdges = true)
                where TEdge : IEdge<TVertex>
            {
                Assert.IsFalse(g.IsDirected);
                Assert.AreEqual(parallelEdges, g.AllowParallelEdges);
                Assert.IsNotNull(g.EdgeEqualityComparer);
            }

            #endregion
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new DelegateImplicitUndirectedGraph<int, Edge<int>>(null));
        }

        #region Contains Vertex

        [Test]
        public void ContainsVertex()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph = new DelegateImplicitUndirectedGraph<int, Edge<int>>(data.TryGetEdges);
            ContainsVertex_Test(data, graph);
        }

        [Test]
        public void ContainsVertex_Throws()
        {
            var graph = new DelegateImplicitUndirectedGraph<TestVertex, Edge<TestVertex>>(
                GetEmptyGetter<TestVertex, Edge<TestVertex>>());
            ContainsVertex_Throws_Test(graph);
        }

        #endregion

        #region Contains Edge

        [Test]
        public void ContainsEdge()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph = new DelegateImplicitUndirectedGraph<int, Edge<int>>(data.TryGetEdges);
            ContainsEdge_SourceTarget_UndirectedGraph_Test(data, graph);
        }

        [Test]
        public void ContainsEdge_Throws()
        {
            var data = new GraphData<TestVertex, Edge<TestVertex>>();
            var graph = new DelegateImplicitUndirectedGraph<TestVertex, Edge<TestVertex>>(data.TryGetEdges);
            ContainsEdge_SourceTarget_Throws_UndirectedGraph_Test(graph);
        }

        #endregion

        #region Adjacent Edges

        [Test]
        public void AdjacentEdge()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph = new DelegateImplicitUndirectedGraph<int, Edge<int>>(data.TryGetEdges);
            AdjacentEdge_Test(data, graph);
        }

        [Test]
        public void AdjacentEdge_Throws()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph1 = new DelegateImplicitUndirectedGraph<int, Edge<int>>(data.TryGetEdges);
            AdjacentEdge_Throws_Test(data, graph1);

            var graph2 = new DelegateImplicitUndirectedGraph<TestVertex, Edge<TestVertex>>(
                GetEmptyGetter<TestVertex, Edge<TestVertex>>());
            AdjacentEdge_NullThrows_Test(graph2);
        }

        [Test]
        public void AdjacentEdges()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph = new DelegateImplicitUndirectedGraph<int, Edge<int>>(data.TryGetEdges);
            AdjacentEdges_Test(data, graph);
        }

        [Test]
        public void AdjacentEdges_Throws()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph1 = new DelegateImplicitUndirectedGraph<int, Edge<int>>(data.TryGetEdges);
            AdjacentEdges_Throws_Test(data, graph1);

            var graph2 = new DelegateImplicitUndirectedGraph<TestVertex, Edge<TestVertex>>(
                GetEmptyGetter<TestVertex, Edge<TestVertex>>());
            AdjacentEdges_NullThrows_Test(graph2);
        }

        #endregion

        #region Try Get Edges

        [Test]
        public void TryGetEdge()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph = new DelegateImplicitUndirectedGraph<int, Edge<int>>(data.TryGetEdges);
            TryGetEdge_UndirectedGraph_Test(data, graph);
        }

        [Test]
        public void TryGetEdge_Throws()
        {
            var data = new GraphData<TestVertex, Edge<TestVertex>>();
            var graph = new DelegateImplicitUndirectedGraph<TestVertex, Edge<TestVertex>>(data.TryGetEdges);
            TryGetEdge_Throws_UndirectedGraph_Test(graph);
        }

        [Test]
        public void TryGetAdjacentEdges()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph = new DelegateImplicitUndirectedGraph<int, Edge<int>>(data.TryGetEdges);
            TryGetAdjacentEdges_Test(data, graph);
        }

        [Test]
        public void TryGetAdjacentEdges_Throws()
        {
            var graph = new DelegateImplicitUndirectedGraph<TestVertex, Edge<TestVertex>>(
                GetEmptyGetter<TestVertex, Edge<TestVertex>>());
            TryGetAdjacentEdges_Throws_Test(graph);
        }

        #endregion
    }
}