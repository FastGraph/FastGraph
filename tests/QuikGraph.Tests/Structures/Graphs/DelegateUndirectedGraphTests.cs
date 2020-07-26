using System;
using System.Linq;
using NUnit.Framework;
using static QuikGraph.Tests.AssertHelpers;
using static QuikGraph.Tests.GraphTestHelpers;

namespace QuikGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="DelegateUndirectedGraph{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class DelegateUndirectedGraphTests : DelegateGraphTestsBase
    {
        [Test]
        public void Construction()
        {
            var graph = new DelegateUndirectedGraph<int, Edge<int>>(
                Enumerable.Empty<int>(),
                GetEmptyGetter<int, Edge<int>>());
            AssertGraphProperties(graph);

            graph = new DelegateUndirectedGraph<int, Edge<int>>(
                Enumerable.Empty<int>(),
                GetEmptyGetter<int, Edge<int>>(),
                false);
            AssertGraphProperties(graph, false);

            #region Local function

            void AssertGraphProperties<TVertex, TEdge>(
                DelegateUndirectedGraph<TVertex, TEdge> g,
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
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => new DelegateUndirectedGraph<int, Edge<int>>(null, GetEmptyGetter<int, Edge<int>>()));
            Assert.Throws<ArgumentNullException>(
                () => new DelegateUndirectedGraph<int, Edge<int>>(Enumerable.Empty<int>(), null));
            Assert.Throws<ArgumentNullException>(
                () => new DelegateUndirectedGraph<int, Edge<int>>(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        #region Vertices & Edges

        [Test]
        public void Vertices()
        {
            var graph = new DelegateUndirectedGraph<int, Edge<int>>(
                Enumerable.Empty<int>(),
                GetEmptyGetter<int, Edge<int>>());
            AssertNoVertex(graph);
            AssertNoVertex(graph);

            graph = new DelegateUndirectedGraph<int, Edge<int>>(
                new[] { 1, 2, 3 },
                GetEmptyGetter<int, Edge<int>>());
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasVertices(graph, new[] { 1, 2, 3 });
        }

        [Test]
        public void Edges()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph = new DelegateUndirectedGraph<int, Edge<int>>(
                Enumerable.Empty<int>(),
                data.TryGetEdges);

            data.ShouldReturnValue = false;
            data.ShouldReturnEdges = null;
            AssertNoEdge(graph);

            data.ShouldReturnValue = true;
            AssertNoEdge(graph);

            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            data.ShouldReturnEdges = new[] { edge12, edge13 };
            AssertNoEdge(graph);    // No vertex so no possible edge!

            graph = new DelegateUndirectedGraph<int, Edge<int>>(
                new[] { 1, 2, 3 },
                data.TryGetEdges);

            data.ShouldReturnValue = true;
            data.ShouldReturnEdges = null;
            AssertNoEdge(graph);

            var edge22 = new Edge<int>(2, 2);
            var edge31 = new Edge<int>(3, 1);
            data.ShouldReturnEdges = new[] { edge12, edge13, edge22, edge31 };
            AssertHasEdges(graph, new[] { edge12, edge13, edge22, edge31 });

            var edge15 = new Edge<int>(1, 5);
            var edge51 = new Edge<int>(5, 1);
            var edge56 = new Edge<int>(5, 6);
            data.ShouldReturnEdges = new[] { edge12, edge13, edge22, edge31, edge15, edge51, edge56 };
            // Some edges skipped because they have vertices not in the graph
            AssertHasEdges(graph, new[] { edge12, edge13, edge22, edge31 });
        }

        #endregion

        #region Contains Vertex

        [Test]
        public void ContainsVertex()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph = new DelegateUndirectedGraph<int, Edge<int>>(
                Enumerable.Empty<int>(),
                data.TryGetEdges);

            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Assert.IsFalse(graph.ContainsVertex(1));
            data.CheckCalls(0); // Implementation override

            data.ShouldReturnValue = true;
            Assert.IsFalse(graph.ContainsVertex(1));
            data.CheckCalls(0); // Implementation override


            graph = new DelegateUndirectedGraph<int, Edge<int>>(
                new[] { 1, 2 },
                data.TryGetEdges);
            data.ShouldReturnValue = false;
            Assert.IsFalse(graph.ContainsVertex(10));
            data.CheckCalls(0); // Implementation override
            Assert.IsTrue(graph.ContainsVertex(2));
            data.CheckCalls(0); // Implementation override

            data.ShouldReturnValue = true;
            Assert.IsFalse(graph.ContainsVertex(10));
            data.CheckCalls(0); // Implementation override
            Assert.IsTrue(graph.ContainsVertex(2));
            data.CheckCalls(0); // Implementation override
        }

        [Test]
        public void ContainsVertex_Throws()
        {
            var graph = new DelegateUndirectedGraph<TestVertex, Edge<TestVertex>>(
                Enumerable.Empty<TestVertex>(),
                GetEmptyGetter<TestVertex, Edge<TestVertex>>());
            ContainsVertex_Throws_Test(graph);
        }

        #endregion

        #region Contains Edge

        [Test]
        public void ContainsEdge()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph = new DelegateUndirectedGraph<int, Edge<int>>(
                new[] { 1, 2, 3 },
                data.TryGetEdges);
            ContainsEdge_Test(data, graph);
        }

        [Test]
        public void ContainsEdge_Throws()
        {
            var data = new GraphData<TestVertex, Edge<TestVertex>>();
            var graph = new DelegateUndirectedGraph<TestVertex, Edge<TestVertex>>(
                Enumerable.Empty<TestVertex>(),
                data.TryGetEdges);
            ContainsEdge_NullThrows_Test(graph);
        }

        [Test]
        public void ContainsEdge_SourceTarget()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph = new DelegateUndirectedGraph<int, Edge<int>>(
                Enumerable.Empty<int>(),
                data.TryGetEdges);

            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Assert.IsFalse(graph.ContainsEdge(1, 2));
            data.CheckCalls(0); // Vertex is not in graph so no need to call user code
            Assert.IsFalse(graph.ContainsEdge(2, 1));
            data.CheckCalls(0); // Vertex is not in graph so no need to call user code

            data.ShouldReturnValue = true;
            Assert.IsFalse(graph.ContainsEdge(1, 2));
            data.CheckCalls(0); // Vertex is not in graph so no need to call user code
            Assert.IsFalse(graph.ContainsEdge(2, 1));
            data.CheckCalls(0); // Vertex is not in graph so no need to call user code

            data.ShouldReturnEdges = new[] { new Edge<int>(1, 3), new Edge<int>(1, 2) };
            Assert.IsFalse(graph.ContainsEdge(1, 2));   // Vertices 1 and 2 are not part or the graph
            data.CheckCalls(0); // Vertex is not in graph so no need to call user code
            Assert.IsFalse(graph.ContainsEdge(2, 1));
            data.CheckCalls(0); // Vertex is not in graph so no need to call user code


            graph = new DelegateUndirectedGraph<int, Edge<int>>(
                new[] { 1, 3 },
                data.TryGetEdges);

            data.ShouldReturnValue = false;
            Assert.IsFalse(graph.ContainsEdge(1, 2));
            data.CheckCalls(1);
            Assert.IsFalse(graph.ContainsEdge(2, 1));
            data.CheckCalls(0); // Vertex is not in graph so no need to call user code

            data.ShouldReturnValue = true;
            Assert.IsFalse(graph.ContainsEdge(1, 2));
            data.CheckCalls(1);
            Assert.IsFalse(graph.ContainsEdge(2, 1));
            data.CheckCalls(0); // Vertex is not in graph so no need to call user code

            data.ShouldReturnEdges = new[] { new Edge<int>(1, 2), new Edge<int>(1, 3) };
            Assert.IsFalse(graph.ContainsEdge(1, 2));   // Vertices 2 is not part or the graph
            data.CheckCalls(1);
            Assert.IsFalse(graph.ContainsEdge(2, 1));
            data.CheckCalls(0); // Vertex is not in graph so no need to call user code

            Assert.IsTrue(graph.ContainsEdge(1, 3));
            data.CheckCalls(1);
            Assert.IsTrue(graph.ContainsEdge(3, 1));
            data.CheckCalls(1);
        }

        [Test]
        public void ContainsEdge_SourceTarget_Throws()
        {
            var data = new GraphData<TestVertex, Edge<TestVertex>>();
            var graph = new DelegateUndirectedGraph<TestVertex, Edge<TestVertex>>(
                Enumerable.Empty<TestVertex>(),
                data.TryGetEdges);
            ContainsEdge_SourceTarget_Throws_UndirectedGraph_Test(graph);
        }

        #endregion

        #region Adjacent Edges

        [Test]
        public void AdjacentEdge()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph = new DelegateUndirectedGraph<int, Edge<int>>(
                new[] { 1, 2, 3 },
                data.TryGetEdges);
            AdjacentEdge_Test(data, graph);

            // Additional tests
            var edge14 = new Edge<int>(1, 4);
            var edge12 = new Edge<int>(1, 2);
            data.ShouldReturnValue = true;
            data.ShouldReturnEdges = new[] { edge14, edge12 };
            Assert.AreSame(edge12, graph.AdjacentEdge(1, 0));
            data.CheckCalls(1);
        }

        [Test]
        public void AdjacentEdge_Throws()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph1 = new DelegateUndirectedGraph<int, Edge<int>>(
                new[] { 1, 2 },
                data.TryGetEdges);
            AdjacentEdge_Throws_Test(data, graph1);

            // Additional tests
            data.ShouldReturnValue = true;
            var edge32 = new Edge<int>(3, 2);
            data.ShouldReturnEdges = new[] { edge32 };
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<VertexNotFoundException>(() => graph1.AdjacentEdge(3, 0));
            data.CheckCalls(0); // Vertex is not in graph so no need to call user code

            var edge14 = new Edge<int>(1, 4);
            var edge12 = new Edge<int>(1, 2);
            data.ShouldReturnEdges = new[] { edge14, edge12 };
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            AssertIndexOutOfRange(() => graph1.AdjacentEdge(1, 1));
            data.CheckCalls(1);

            var graph2 = new DelegateUndirectedGraph<TestVertex, Edge<TestVertex>>(
                Enumerable.Empty<TestVertex>(),
                GetEmptyGetter<TestVertex, Edge<TestVertex>>());
            AdjacentEdge_NullThrows_Test(graph2);
        }

        [Test]
        public void AdjacentEdges()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph = new DelegateUndirectedGraph<int, Edge<int>>(
                new [] { 1, 2, 3 },
                data.TryGetEdges);
            AdjacentEdges_Test(data, graph);

            // Additional tests
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge21 = new Edge<int>(2, 1);
            data.ShouldReturnValue = true;
            data.ShouldReturnEdges = new[] { edge12, edge13, edge14, edge21 };
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            AssertHasAdjacentEdges(graph, 1, new[] { edge12, edge13, edge21 });
            data.CheckCalls(3);
        }

        [Test]
        public void AdjacentEdges_Throws()
        {
            var data1 = new GraphData<int, Edge<int>>();
            var graph1 = new DelegateUndirectedGraph<int, Edge<int>>(
                new[] { 1 },
                data1.TryGetEdges);
            AdjacentEdges_Throws_Test(data1, graph1);

            // Additional tests
            data1.ShouldReturnValue = true;
            var edge32 = new Edge<int>(3, 2);
            data1.ShouldReturnEdges = new[] { edge32 };
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<VertexNotFoundException>(() => graph1.AdjacentEdges(3));
            data1.CheckCalls(0); // Vertex is not in graph so no need to call user code


            var data2 = new GraphData<TestVertex, Edge<TestVertex>>();
            var graph2 = new DelegateUndirectedGraph<TestVertex, Edge<TestVertex>>(
                Enumerable.Empty<TestVertex>(),
                data2.TryGetEdges);
            AdjacentEdges_NullThrows_Test(graph2);
        }

        #endregion

        #region Try Get Edges

        [Test]
        public void TryGetEdge()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph = new DelegateUndirectedGraph<int, Edge<int>>(
                new[] { 1, 2, 3 },
                data.TryGetEdges);
            TryGetEdge_UndirectedGraph_Test(data, graph);

            // Additional tests
            var edge13 = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge21 = new Edge<int>(2, 1);
            data.ShouldReturnValue = true;
            data.ShouldReturnEdges = new[] { edge13, edge14, edge21 };

            var edge12 = new Edge<int>(1, 2);
            Assert.IsTrue(graph.TryGetEdge(1, 2, out Edge<int> gotEdge));
            Assert.AreSame(edge21, gotEdge);

            data.ShouldReturnEdges = new[] { edge12, edge13, edge14, edge21 };
            Assert.IsTrue(graph.TryGetEdge(1, 2, out gotEdge));
            Assert.AreSame(edge12, gotEdge);

            var edge51 = new Edge<int>(5, 1);
            var edge56 = new Edge<int>(5, 6);
            data.ShouldReturnEdges = new[] { edge12, edge13, edge51, edge56 };
            Assert.IsFalse(graph.TryGetEdge(1, 5, out _));
            Assert.IsFalse(graph.TryGetEdge(5, 1, out _));
            Assert.IsFalse(graph.TryGetEdge(5, 6, out _));
        }

        [Test]
        public void TryGetEdge_Throws()
        {
            var data = new GraphData<TestVertex, Edge<TestVertex>>();
            var graph = new DelegateUndirectedGraph<TestVertex, Edge<TestVertex>>(
                Enumerable.Empty<TestVertex>(),
                data.TryGetEdges);
            TryGetEdge_Throws_UndirectedGraph_Test(graph);
        }

        [Test]
        public void TryGetAdjacentEdges()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph = new DelegateUndirectedGraph<int, Edge<int>>(
                new[] { 1, 2, 3, 4 },
                data.TryGetEdges);
            TryGetAdjacentEdges_Test(data, graph);
        }

        [Test]
        public void TryGetAdjacentEdges_Throws()
        {
            var graph = new DelegateUndirectedGraph<TestVertex, Edge<TestVertex>>(
                Enumerable.Empty<TestVertex>(),
                GetEmptyGetter<TestVertex, Edge<TestVertex>>());
            TryGetAdjacentEdges_Throws_Test(graph);
        }

        #endregion
    }
}