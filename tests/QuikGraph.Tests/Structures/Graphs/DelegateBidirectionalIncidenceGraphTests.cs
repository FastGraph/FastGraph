using System;
using NUnit.Framework;

namespace QuikGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="DelegateBidirectionalIncidenceGraph{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class DelegateBidirectionalIncidenceGraphTests : DelegateGraphTestsBase
    {
        [Test]
        public void Construction()
        {
            var graph = new DelegateBidirectionalIncidenceGraph<int, Edge<int>>(
                GetEmptyGetter<int, Edge<int>>(),
                GetEmptyGetter<int, Edge<int>>());
            AssertGraphProperties(graph);

            graph = new DelegateBidirectionalIncidenceGraph<int, Edge<int>>(
                GetEmptyGetter<int, Edge<int>>(),
                GetEmptyGetter<int, Edge<int>>(),
                false);
            AssertGraphProperties(graph, false);

            #region Local function

            void AssertGraphProperties<TVertex, TEdge>(
                DelegateBidirectionalIncidenceGraph<TVertex, TEdge> g,
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
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => new DelegateBidirectionalIncidenceGraph<int, Edge<int>>(
                    GetEmptyGetter<int, Edge<int>>(),
                    null));
            Assert.Throws<ArgumentNullException>(
                () => new DelegateBidirectionalIncidenceGraph<int, Edge<int>>(
                    null,
                    GetEmptyGetter<int, Edge<int>>()));
            Assert.Throws<ArgumentNullException>(
                () => new DelegateBidirectionalIncidenceGraph<int, Edge<int>>(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        #region Contains Vertex

        [Test]
        public void ContainsVertex()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph = new DelegateBidirectionalIncidenceGraph<int, Edge<int>>(
                data.TryGetEdges,
                GetEmptyGetter<int, Edge<int>>());
            ContainsVertex_Test(data, graph);
        }

        [Test]
        public void ContainsVertex_Throws()
        {
            var graph = new DelegateBidirectionalIncidenceGraph<TestVertex, Edge<TestVertex>>(
                GetEmptyGetter<TestVertex, Edge<TestVertex>>(),
                GetEmptyGetter<TestVertex, Edge<TestVertex>>());
            ContainsVertex_Throws_Test(graph);
        }

        #endregion

        #region Out Edges

        [Test]
        public void OutEdge()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph = new DelegateBidirectionalIncidenceGraph<int, Edge<int>>(
                data.TryGetEdges,
                GetEmptyGetter<int, Edge<int>>());
            OutEdge_Test(data, graph);
        }

        [Test]
        public void OutEdge_Throws()
        {
            var graph1 = new DelegateBidirectionalIncidenceGraph<TestVertex, Edge<TestVertex>>(
                GetEmptyGetter<TestVertex, Edge<TestVertex>>(),
                GetEmptyGetter<TestVertex, Edge<TestVertex>>());
            OutEdge_NullThrows_Test(graph1);

            var data = new GraphData<int, Edge<int>>();
            var graph2 = new DelegateBidirectionalIncidenceGraph<int, Edge<int>>(
                data.TryGetEdges,
                GetEmptyGetter<int, Edge<int>>());
            OutEdge_Throws_Test(data, graph2);
        }

        [Test]
        public void OutEdges()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph = new DelegateBidirectionalIncidenceGraph<int, Edge<int>>(
                data.TryGetEdges,
                GetEmptyGetter<int, Edge<int>>());
            OutEdges_Test(data, graph);
        }

        [Test]
        public void OutEdges_Throws()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph1 = new DelegateBidirectionalIncidenceGraph<int, Edge<int>>(
                data.TryGetEdges,
                GetEmptyGetter<int, Edge<int>>());
            OutEdges_Throws_Test(data, graph1);

            var graph2 = new DelegateBidirectionalIncidenceGraph<EquatableTestVertex, Edge<EquatableTestVertex>>(
                GetEmptyGetter<EquatableTestVertex, Edge<EquatableTestVertex>>(),
                GetEmptyGetter<EquatableTestVertex, Edge<EquatableTestVertex>>());
            OutEdges_NullThrows_Test(graph2);
        }

        #endregion

        #region In Edges

        [Test]
        public void InEdge()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph = new DelegateBidirectionalIncidenceGraph<int, Edge<int>>(
                GetEmptyGetter<int, Edge<int>>(),
                data.TryGetEdges);
            InEdge_Test(data, graph);
        }

        [Test]
        public void InEdge_Throws()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph1 = new DelegateBidirectionalIncidenceGraph<int, Edge<int>>(
                GetEmptyGetter<int, Edge<int>>(),
                data.TryGetEdges);
            InEdge_Throws_Test(data, graph1);

            var graph2 = new DelegateBidirectionalIncidenceGraph<TestVertex, Edge<TestVertex>>(
                GetEmptyGetter<TestVertex, Edge<TestVertex>>(),
                GetEmptyGetter<TestVertex, Edge<TestVertex>>());
            InEdge_NullThrows_Test(graph2);
        }

        [Test]
        public void InEdges()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph = new DelegateBidirectionalIncidenceGraph<int, Edge<int>>(
                GetEmptyGetter<int, Edge<int>>(),
                data.TryGetEdges);
            InEdges_Test(data, graph);
        }

        [Test]
        public void InEdges_Throws()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph1 = new DelegateBidirectionalIncidenceGraph<int, Edge<int>>(
                GetEmptyGetter<int, Edge<int>>(),
                data.TryGetEdges);
            InEdges_Throws_Test(data, graph1);

            var graph2 = new DelegateBidirectionalIncidenceGraph<EquatableTestVertex, Edge<EquatableTestVertex>>(
                GetEmptyGetter<EquatableTestVertex, Edge<EquatableTestVertex>>(),
                GetEmptyGetter<EquatableTestVertex, Edge<EquatableTestVertex>>());
            InEdges_NullThrows_Test(graph2);
        }

        #endregion

        [Test]
        public void Degree()
        {
            var data1 = new GraphData<int, Edge<int>>();
            var data2 = new GraphData<int, Edge<int>>();
            var graph = new DelegateBidirectionalIncidenceGraph<int, Edge<int>>(
                data1.TryGetEdges,
                data2.TryGetEdges);
            Degree_Test(data1, data2, graph);
        }

        [Test]
        public void Degree_Throws()
        {
            var graph = new DelegateBidirectionalIncidenceGraph<EquatableTestVertex, Edge<EquatableTestVertex>>(
                GetEmptyGetter<EquatableTestVertex, Edge<EquatableTestVertex>>(),
                GetEmptyGetter<EquatableTestVertex, Edge<EquatableTestVertex>>());
            Degree_Throws_Test(graph);
        }

        #region Try Get Edges

        [Test]
        public void TryGetOutEdges()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph = new DelegateBidirectionalIncidenceGraph<int, Edge<int>>(
                data.TryGetEdges,
                GetEmptyGetter<int, Edge<int>>());
            TryGetOutEdges_Test(data, graph);
        }

        [Test]
        public void TryGetOutEdges_Throws()
        {
            var graph = new DelegateBidirectionalIncidenceGraph<TestVertex, Edge<TestVertex>>(
                GetEmptyGetter<TestVertex, Edge<TestVertex>>(),
                GetEmptyGetter<TestVertex, Edge<TestVertex>>());
            TryGetOutEdges_Throws_Test(graph);
        }

        [Test]
        public void TryGetInEdges()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph = new DelegateBidirectionalIncidenceGraph<int, Edge<int>>(
                GetEmptyGetter<int, Edge<int>>(),
                data.TryGetEdges);
            TryGetInEdges_Test(data, graph);
        }

        [Test]
        public void TryGetInEdges_Throws()
        {
            var graph = new DelegateBidirectionalIncidenceGraph<TestVertex, Edge<TestVertex>>(
                GetEmptyGetter<TestVertex, Edge<TestVertex>>(),
                GetEmptyGetter<TestVertex, Edge<TestVertex>>());
            TryGetInEdges_Throws_Test(graph);
        }

        #endregion
    }
}