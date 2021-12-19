#nullable enable

using NUnit.Framework;
using static FastGraph.Tests.AssertHelpers;
using static FastGraph.Tests.GraphTestHelpers;

namespace FastGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="DelegateVertexAndEdgeListGraph{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class DelegateVertexAndEdgeListGraphTests : DelegateGraphTestsBase
    {
        [Test]
        public void Construction()
        {
            var graph = new DelegateVertexAndEdgeListGraph<int, Edge<int>>(
                Enumerable.Empty<int>(),
                GetEmptyGetter<int, Edge<int>>());
            AssertGraphProperties(graph);

            graph = new DelegateVertexAndEdgeListGraph<int, Edge<int>>(
                Enumerable.Empty<int>(),
                GetEmptyGetter<int, Edge<int>>(),
                false);
            AssertGraphProperties(graph, false);

            #region Local function

            void AssertGraphProperties<TVertex, TEdge>(
                DelegateVertexAndEdgeListGraph<TVertex, TEdge> g,
                bool parallelEdges = true)
                where TVertex : notnull
                where TEdge : IEdge<TVertex>
            {
                g.IsDirected.Should().BeTrue();
                g.AllowParallelEdges.Should().Be(parallelEdges);
            }

            #endregion
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new DelegateVertexAndEdgeListGraph<int, Edge<int>>(default, GetEmptyGetter<int, Edge<int>>())).Should().Throw<ArgumentNullException>();
            Invoking(() => new DelegateVertexAndEdgeListGraph<int, Edge<int>>(Enumerable.Empty<int>(), default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new DelegateVertexAndEdgeListGraph<int, Edge<int>>(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        #region Vertices & Edges

        [Test]
        public void Vertices()
        {
            var graph = new DelegateVertexAndEdgeListGraph<int, Edge<int>>(
                Enumerable.Empty<int>(),
                GetEmptyGetter<int, Edge<int>>());
            AssertNoVertex(graph);
            AssertNoVertex(graph);

            graph = new DelegateVertexAndEdgeListGraph<int, Edge<int>>(
                new[] { 1, 2, 3 },
                GetEmptyGetter<int, Edge<int>>());
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasVertices(graph, new[] { 1, 2, 3 });
        }

        [Test]
        public void Edges()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph = new DelegateVertexAndEdgeListGraph<int, Edge<int>>(
                Enumerable.Empty<int>(),
                data.TryGetEdges);

            data.ShouldReturnValue = false;
            data.ShouldReturnEdges = default;
            AssertNoEdge(graph);

            data.ShouldReturnValue = true;
            AssertNoEdge(graph);

            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            data.ShouldReturnEdges = new[] { edge12, edge13 };
            AssertNoEdge(graph);    // No vertex so no possible edge!

            graph = new DelegateVertexAndEdgeListGraph<int, Edge<int>>(
                new[] { 1, 2, 3 },
                data.TryGetEdges);

            data.ShouldReturnValue = true;
            data.ShouldReturnEdges = default;
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
            var graph = new DelegateVertexAndEdgeListGraph<int, Edge<int>>(
                Enumerable.Empty<int>(),
                data.TryGetEdges);

            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            graph.ContainsVertex(1).Should().BeFalse();
            data.CheckCalls(0); // Implementation override

            data.ShouldReturnValue = true;
            graph.ContainsVertex(1).Should().BeFalse();
            data.CheckCalls(0); // Implementation override


            graph = new DelegateVertexAndEdgeListGraph<int, Edge<int>>(
                new[] { 1, 2 },
                data.TryGetEdges);
            data.ShouldReturnValue = false;
            graph.ContainsVertex(10).Should().BeFalse();
            data.CheckCalls(0); // Implementation override
            graph.ContainsVertex(2).Should().BeTrue();
            data.CheckCalls(0); // Implementation override

            data.ShouldReturnValue = true;
            graph.ContainsVertex(10).Should().BeFalse();
            data.CheckCalls(0); // Implementation override
            graph.ContainsVertex(2).Should().BeTrue();
            data.CheckCalls(0); // Implementation override
        }

        [Test]
        public void ContainsVertex_Throws()
        {
            var graph = new DelegateVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>(
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
            var graph = new DelegateVertexAndEdgeListGraph<int, Edge<int>>(
                new[] { 1, 2, 3 },
                data.TryGetEdges);
            ContainsEdge_Test(data, graph);
        }

        [Test]
        public void ContainsEdge_Throws()
        {
            var data = new GraphData<TestVertex, Edge<TestVertex>>();
            var graph = new DelegateVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>(
                Enumerable.Empty<TestVertex>(),
                data.TryGetEdges);
            ContainsEdge_NullThrows_Test(graph);
        }

        [Test]
        public void ContainsEdge_SourceTarget()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph = new DelegateVertexAndEdgeListGraph<int, Edge<int>>(
                Enumerable.Empty<int>(),
                data.TryGetEdges);

            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            graph.ContainsEdge(1, 2).Should().BeFalse();
            data.CheckCalls(0); // Vertex is not in graph so no need to call user code
            graph.ContainsEdge(2, 1).Should().BeFalse();
            data.CheckCalls(0); // Vertex is not in graph so no need to call user code

            data.ShouldReturnValue = true;
            graph.ContainsEdge(1, 2).Should().BeFalse();
            data.CheckCalls(0); // Vertex is not in graph so no need to call user code
            graph.ContainsEdge(2, 1).Should().BeFalse();
            data.CheckCalls(0); // Vertex is not in graph so no need to call user code

            data.ShouldReturnEdges = new[] { new Edge<int>(1, 3), new Edge<int>(1, 2) };
            graph.ContainsEdge(1, 2).Should().BeFalse();   // Vertices 1 and 2 are not part or the graph
            data.CheckCalls(0); // Vertex is not in graph so no need to call user code
            graph.ContainsEdge(2, 1).Should().BeFalse();
            data.CheckCalls(0); // Vertex is not in graph so no need to call user code


            graph = new DelegateVertexAndEdgeListGraph<int, Edge<int>>(
                new[] { 1, 3 },
                data.TryGetEdges);

            data.ShouldReturnValue = false;
            graph.ContainsEdge(1, 2).Should().BeFalse();
            data.CheckCalls(1);
            graph.ContainsEdge(2, 1).Should().BeFalse();
            data.CheckCalls(0); // Vertex is not in graph so no need to call user code

            data.ShouldReturnValue = true;
            graph.ContainsEdge(1, 2).Should().BeFalse();
            data.CheckCalls(1);
            graph.ContainsEdge(2, 1).Should().BeFalse();
            data.CheckCalls(0); // Vertex is not in graph so no need to call user code

            data.ShouldReturnEdges = new[] { new Edge<int>(1, 2), new Edge<int>(1, 3) };
            graph.ContainsEdge(1, 2).Should().BeFalse();   // Vertices 2 is not part or the graph
            data.CheckCalls(1);
            graph.ContainsEdge(2, 1).Should().BeFalse();
            data.CheckCalls(0); // Vertex is not in graph so no need to call user code

            graph.ContainsEdge(1, 3).Should().BeTrue();
            data.CheckCalls(1);
            graph.ContainsEdge(3, 1).Should().BeFalse();
            data.CheckCalls(1);
        }

        [Test]
        public void ContainsEdge_SourceTarget_Throws()
        {
            var data = new GraphData<TestVertex, Edge<TestVertex>>();
            var graph = new DelegateVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>(
                Enumerable.Empty<TestVertex>(),
                data.TryGetEdges);
            ContainsEdge_SourceTarget_Throws_Test(graph);
        }

        #endregion

        #region Out Edges

        [Test]
        public void OutEdge()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph = new DelegateVertexAndEdgeListGraph<int, Edge<int>>(
                new[] { 1, 2, 3 },
                data.TryGetEdges);
            OutEdge_Test(data, graph);

            // Additional tests
            var edge14 = new Edge<int>(1, 4);
            var edge12 = new Edge<int>(1, 2);
            data.ShouldReturnValue = true;
            data.ShouldReturnEdges = new[] { edge14, edge12 };
            graph.OutEdge(1, 0).Should().BeSameAs(edge12);
            data.CheckCalls(1);
        }

        [Test]
        public void OutEdge_Throws()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph1 = new DelegateVertexAndEdgeListGraph<int, Edge<int>>(
                new[] { 1, 2 },
                data.TryGetEdges);
            OutEdge_Throws_Test(data, graph1);

            // Additional tests
            data.ShouldReturnValue = true;
            var edge32 = new Edge<int>(3, 2);
            data.ShouldReturnEdges = new[] { edge32 };
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Invoking(() => graph1.OutEdge(3, 0)).Should().Throw<VertexNotFoundException>();
            data.CheckCalls(0); // Vertex is not in graph so no need to call user code

            var edge14 = new Edge<int>(1, 4);
            var edge12 = new Edge<int>(1, 2);
            data.ShouldReturnEdges = new[] { edge14, edge12 };
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            AssertIndexOutOfRange(() => graph1.OutEdge(1, 1));
            data.CheckCalls(1);

            var graph2 = new DelegateVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>(
                Enumerable.Empty<TestVertex>(),
                GetEmptyGetter<TestVertex, Edge<TestVertex>>());
            OutEdge_NullThrows_Test(graph2);
        }

        [Test]
        public void OutEdges()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph = new DelegateVertexAndEdgeListGraph<int, Edge<int>>(
                new[] { 1, 2, 3 },
                data.TryGetEdges);
            OutEdges_Test(data, graph);

            // Additional tests
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge21 = new Edge<int>(2, 1);
            data.ShouldReturnValue = true;
            data.ShouldReturnEdges = new[] { edge12, edge13, edge14, edge21 };
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            AssertHasOutEdges(graph, 1, new[] { edge12, edge13 });
            data.CheckCalls(3);
        }

        [Test]
        public void OutEdges_Throws()
        {
            var data1 = new GraphData<int, Edge<int>>();
            var graph1 = new DelegateVertexAndEdgeListGraph<int, Edge<int>>(
                new[] { 1 },
                data1.TryGetEdges);
            OutEdges_Throws_Test(data1, graph1);

            // Additional tests
            data1.ShouldReturnValue = true;
            var edge32 = new Edge<int>(3, 2);
            data1.ShouldReturnEdges = new[] { edge32 };
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Invoking(() => graph1.OutEdges(3)).Should().Throw<VertexNotFoundException>();
            data1.CheckCalls(0); // Vertex is not in graph so no need to call user code


            var graph2 = new DelegateVertexAndEdgeListGraph<EquatableTestVertex, Edge<EquatableTestVertex>>(
                Enumerable.Empty<EquatableTestVertex>(),
                GetEmptyGetter<EquatableTestVertex, Edge<EquatableTestVertex>>());
            OutEdges_NullThrows_Test(graph2);
        }

        #endregion

        #region Try Get Edges

        [Test]
        public void TryGetEdge()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph = new DelegateVertexAndEdgeListGraph<int, Edge<int>>(
                new[] { 1, 2, 3 },
                data.TryGetEdges);
            TryGetEdge_Test(data, graph);

            // Additional tests
            var edge13 = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge21 = new Edge<int>(2, 1);
            data.ShouldReturnValue = true;
            data.ShouldReturnEdges = new[] { edge13, edge14, edge21 };

            graph.TryGetEdge(1, 2, out Edge<int>? gotEdge).Should().BeFalse();

            var edge12 = new Edge<int>(1, 2);
            data.ShouldReturnEdges = new[] { edge12, edge13, edge14, edge21 };
            graph.TryGetEdge(1, 2, out gotEdge).Should().BeTrue();
            gotEdge.Should().BeSameAs(edge12);

            var edge51 = new Edge<int>(5, 1);
            var edge56 = new Edge<int>(5, 6);
            data.ShouldReturnEdges = new[] { edge12, edge13, edge51, edge56 };
            graph.TryGetEdge(1, 5, out _).Should().BeFalse();
            graph.TryGetEdge(5, 1, out _).Should().BeFalse();
            graph.TryGetEdge(5, 6, out _).Should().BeFalse();
        }

        [Test]
        public void TryGetEdge_Throws()
        {
            var data = new GraphData<TestVertex, Edge<TestVertex>>();
            var graph = new DelegateVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>(
                Enumerable.Empty<TestVertex>(),
                data.TryGetEdges);
            TryGetEdge_Throws_Test(graph);
        }

        [Test]
        public void TryGetEdges()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph = new DelegateVertexAndEdgeListGraph<int, Edge<int>>(
                new[] { 1, 2, 3 },
                data.TryGetEdges);
            TryGetEdges_Test(data, graph);
        }

        [Test]
        public void TryGetEdges_Throws()
        {
            var data = new GraphData<TestVertex, Edge<TestVertex>>();
            var graph = new DelegateVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>(
                Enumerable.Empty<TestVertex>(),
                data.TryGetEdges);
            TryGetEdges_Throws_Test(graph);
        }

        [Test]
        public void TryGetOutEdges()
        {
            var data = new GraphData<int, Edge<int>>();
            var graph = new DelegateVertexAndEdgeListGraph<int, Edge<int>>(
                new[] { 1, 2, 3, 4 },
                data.TryGetEdges);
            TryGetOutEdges_Test(data, graph);
        }

        [Test]
        public void TryGetOutEdges_Throws()
        {
            var graph = new DelegateVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>(
                Enumerable.Empty<TestVertex>(),
                GetEmptyGetter<TestVertex, Edge<TestVertex>>());
            TryGetOutEdges_Throws_Test(graph);
        }

        #endregion
    }
}
