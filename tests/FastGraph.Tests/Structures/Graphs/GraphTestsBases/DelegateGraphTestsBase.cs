#nullable enable

using JetBrains.Annotations;
using static FastGraph.Tests.AssertHelpers;
using static FastGraph.Tests.GraphTestHelpers;

namespace FastGraph.Tests.Structures
{
    /// <summary>
    /// Base class for tests over delegated graphs.
    /// </summary>
    internal class DelegateGraphTestsBase : GraphTestsBase
    {
        [Pure]
        protected static TryFunc<TVertex, IEnumerable<TEdge>> GetEmptyGetter<TVertex, TEdge>()
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            return (TVertex _, out IEnumerable<TEdge>? edges) =>
            {
                edges = default;
                return false;
            };
        }

        protected class GraphData<TVertex, TEdge>
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            public GraphData()
            {
                TryGetEdges = (TVertex _, out IEnumerable<TEdge>? edges) =>
                {
                    ++_nbCalls;

                    if (ShouldReturnValue)
                        edges = ShouldReturnEdges ?? Enumerable.Empty<TEdge>();
                    else
                        edges = default;

                    return ShouldReturnValue;
                };
            }

            private int _nbCalls;

            public TryFunc<TVertex, IEnumerable<TEdge>> TryGetEdges { get; }

            public IEnumerable<TEdge>? ShouldReturnEdges { get; set; }

            public bool ShouldReturnValue { get; set; }

            public void CheckCalls(int expectedCalls)
            {
                _nbCalls.Should().Be(expectedCalls);
                _nbCalls = 0;
            }
        }

        #region Test helpers

        #region Contains Vertex

        protected static void ContainsVertex_Test(
            GraphData<int, Edge<int>> data,
            IImplicitVertexSet<int> graph)
        {
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            graph.ContainsVertex(1).Should().BeFalse();
            data.CheckCalls(1);

            data.ShouldReturnValue = true;
            graph.ContainsVertex(1).Should().BeTrue();
            data.CheckCalls(1);
        }

        #endregion

        #region Contains Edge

        protected static void ContainsEdge_Test(
            GraphData<int, Edge<int>> data,
            IEdgeSet<int, Edge<int>> graph)
        {
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            var edge12 = new Edge<int>(1, 2);
            var edge21 = new Edge<int>(2, 1);
            graph.ContainsEdge(edge12).Should().BeFalse();
            data.CheckCalls(1);
            graph.ContainsEdge(edge21).Should().BeFalse();
            data.CheckCalls(1);

            data.ShouldReturnValue = true;
            graph.ContainsEdge(edge12).Should().BeFalse();
            data.CheckCalls(1);
            graph.ContainsEdge(edge21).Should().BeFalse();
            data.CheckCalls(1);

            var edge13 = new Edge<int>(1, 3);
            data.ShouldReturnEdges = new[] { edge12, edge13, edge21 };
            graph.ContainsEdge(edge12).Should().BeTrue();
            data.CheckCalls(1);
            graph.ContainsEdge(edge21).Should().BeTrue();
            data.CheckCalls(1);

            var edge15 = new Edge<int>(1, 5);
            var edge51 = new Edge<int>(5, 1);
            var edge56 = new Edge<int>(5, 6);
            graph.ContainsEdge(edge15).Should().BeFalse();
            graph.ContainsEdge(edge51).Should().BeFalse();
            graph.ContainsEdge(edge56).Should().BeFalse();
        }

        private static void ContainsEdge_SourceTarget_GenericTest(
            GraphData<int, Edge<int>> data,
            [InstantHandle] Func<int, int, bool> hasEdge,
            bool isDirected = true)
        {
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            hasEdge(1, 2).Should().BeFalse();
            data.CheckCalls(1);
            hasEdge(2, 1).Should().BeFalse();
            data.CheckCalls(1);

            data.ShouldReturnValue = true;
            hasEdge(1, 2).Should().BeFalse();
            data.CheckCalls(1);
            hasEdge(2, 1).Should().BeFalse();
            data.CheckCalls(1);

            data.ShouldReturnEdges = new[] { new Edge<int>(1, 3), new Edge<int>(1, 2) };
            hasEdge(1, 2).Should().BeTrue();
            data.CheckCalls(1);
            if (isDirected)
                hasEdge(2, 1).Should().BeFalse();
            else
                hasEdge(2, 1).Should().BeTrue();
            data.CheckCalls(1);

            hasEdge(1, 5).Should().BeFalse();
            hasEdge(5, 1).Should().BeFalse();
            hasEdge(5, 6).Should().BeFalse();
        }

        protected static void ContainsEdge_SourceTarget_Test(
            GraphData<int, Edge<int>> data,
            IIncidenceGraph<int, Edge<int>> graph)
        {
            ContainsEdge_SourceTarget_GenericTest(
                data,
                graph.ContainsEdge);
        }

        protected static void ContainsEdge_SourceTarget_UndirectedGraph_Test(
            GraphData<int, Edge<int>> data,
            IImplicitUndirectedGraph<int, Edge<int>> graph)
        {
            ContainsEdge_SourceTarget_GenericTest(
                data,
                graph.ContainsEdge,
                false);
        }

        #endregion

        #region Out Edges

        protected static void OutEdge_Test(
            GraphData<int, Edge<int>> data,
            IImplicitGraph<int, Edge<int>> graph)
        {
            var edge11 = new Edge<int>(1, 1);
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);

            data.CheckCalls(0);

            data.ShouldReturnValue = true;
            data.ShouldReturnEdges = new[] { edge11, edge12, edge13 };
            graph.OutEdge(1, 0).Should().BeSameAs(edge11);
            data.CheckCalls(1);

            graph.OutEdge(1, 2).Should().BeSameAs(edge13);
            data.CheckCalls(1);
        }

        protected static void OutEdge_Throws_Test(
            GraphData<int, Edge<int>> data,
            IImplicitGraph<int, Edge<int>> graph)
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Invoking(() => graph.OutEdge(1, 0)).Should().Throw<VertexNotFoundException>();
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
            GraphData<int, Edge<int>> data,
            IImplicitGraph<int, Edge<int>> graph)
        {
            data.CheckCalls(0);

            data.ShouldReturnValue = true;
            AssertNoOutEdge(graph, 1);
            data.CheckCalls(3);

            Edge<int>[] edges =
            {
                new Edge<int>(1, 2),
                new Edge<int>(1, 3)
            };
            data.ShouldReturnEdges = edges;
            AssertHasOutEdges(graph, 1, edges);
            data.CheckCalls(3);
        }

        protected static void OutEdges_Throws_Test(
            GraphData<int, Edge<int>> data,
            IImplicitGraph<int, Edge<int>> graph)
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Invoking(() => graph.IsOutEdgesEmpty(1)).Should().Throw<VertexNotFoundException>();
            data.CheckCalls(1);

            Invoking(() => graph.OutDegree(1)).Should().Throw<VertexNotFoundException>();
            data.CheckCalls(1);

            Invoking(() => graph.OutEdges(1)).Should().Throw<VertexNotFoundException>();
            data.CheckCalls(1);
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        #endregion

        #region Adjacent Edges

        protected static void AdjacentEdge_Test(
            GraphData<int, Edge<int>> data,
            IImplicitUndirectedGraph<int, Edge<int>> graph)
        {
            var edge11 = new Edge<int>(1, 1);
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);

            data.CheckCalls(0);

            data.ShouldReturnValue = true;
            data.ShouldReturnEdges = new[] { edge11, edge12, edge13 };
            graph.AdjacentEdge(1, 0).Should().BeSameAs(edge11);
            data.CheckCalls(1);

            graph.AdjacentEdge(1, 2).Should().BeSameAs(edge13);
            data.CheckCalls(1);
        }

        protected static void AdjacentEdge_Throws_Test(
            GraphData<int, Edge<int>> data,
            IImplicitUndirectedGraph<int, Edge<int>> graph)
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Invoking(() => graph.AdjacentEdge(1, 0)).Should().Throw<VertexNotFoundException>();
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
            GraphData<int, Edge<int>> data,
            IImplicitUndirectedGraph<int, Edge<int>> graph)
        {
            data.CheckCalls(0);

            data.ShouldReturnValue = true;
            AssertNoAdjacentEdge(graph, 1);
            data.CheckCalls(3);

            Edge<int>[] edges =
            {
                new Edge<int>(1, 2),
                new Edge<int>(1, 3)
            };
            data.ShouldReturnEdges = edges;
            AssertHasAdjacentEdges(graph, 1, edges);
            data.CheckCalls(3);
        }

        protected static void AdjacentEdges_Throws_Test(
            GraphData<int, Edge<int>> data,
            IImplicitUndirectedGraph<int, Edge<int>> graph)
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Invoking(() => graph.IsAdjacentEdgesEmpty(1)).Should().Throw<VertexNotFoundException>();
            data.CheckCalls(1);

            Invoking(() => graph.AdjacentDegree(1)).Should().Throw<VertexNotFoundException>();
            data.CheckCalls(1);

            Invoking(() => graph.AdjacentEdges(1)).Should().Throw<VertexNotFoundException>();
            data.CheckCalls(1);
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        #endregion

        #region Out Edges

        protected static void InEdge_Test(
            GraphData<int, Edge<int>> data,
            IBidirectionalIncidenceGraph<int, Edge<int>> graph)
        {
            var edge11 = new Edge<int>(1, 1);
            var edge21 = new Edge<int>(2, 1);
            var edge31 = new Edge<int>(3, 1);

            data.CheckCalls(0);

            data.ShouldReturnValue = true;
            data.ShouldReturnEdges = new[] { edge11, edge21, edge31 };
            graph.InEdge(1, 0).Should().BeSameAs(edge11);
            data.CheckCalls(1);

            graph.InEdge(1, 2).Should().BeSameAs(edge31);
            data.CheckCalls(1);
        }

        protected static void InEdge_Throws_Test(
            GraphData<int, Edge<int>> data,
            IBidirectionalIncidenceGraph<int, Edge<int>> graph)
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Invoking(() => graph.InEdge(1, 0)).Should().Throw<VertexNotFoundException>();
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
            GraphData<int, Edge<int>> data,
            IBidirectionalIncidenceGraph<int, Edge<int>> graph)
        {
            data.CheckCalls(0);

            data.ShouldReturnValue = true;
            AssertNoInEdge(graph, 1);
            data.CheckCalls(3);

            Edge<int>[] edges =
            {
                new Edge<int>(1, 2),
                new Edge<int>(1, 3)
            };
            data.ShouldReturnEdges = edges;
            AssertHasInEdges(graph, 1, edges);
            data.CheckCalls(3);
        }

        protected static void InEdges_Throws_Test(
            GraphData<int, Edge<int>> data,
            IBidirectionalIncidenceGraph<int, Edge<int>> graph)
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Invoking(() => graph.IsInEdgesEmpty(1)).Should().Throw<VertexNotFoundException>();
            data.CheckCalls(1);

            Invoking(() => graph.InDegree(1)).Should().Throw<VertexNotFoundException>();
            data.CheckCalls(1);

            Invoking(() => graph.InEdges(1)).Should().Throw<VertexNotFoundException>();
            data.CheckCalls(1);
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        #endregion

        #region Degree

        protected static void Degree_Test(
            GraphData<int, Edge<int>> data1,
            GraphData<int, Edge<int>> data2,
            IBidirectionalIncidenceGraph<int, Edge<int>> graph)
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            data1.CheckCalls(0);
            data2.CheckCalls(0);

            data1.ShouldReturnValue = false;
            data2.ShouldReturnValue = false;
            Invoking(() => graph.Degree(1)).Should().Throw<VertexNotFoundException>();
            data1.CheckCalls(0);
            data2.CheckCalls(1);

            data1.ShouldReturnValue = true;
            data2.ShouldReturnValue = false;
            Invoking(() => graph.Degree(1)).Should().Throw<VertexNotFoundException>();
            data1.CheckCalls(0);
            data2.CheckCalls(1);

            data1.ShouldReturnValue = false;
            data2.ShouldReturnValue = true;
            Invoking(() => graph.Degree(1)).Should().Throw<VertexNotFoundException>();
            data1.CheckCalls(1);
            data2.CheckCalls(1);
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            data1.ShouldReturnValue = true;
            data2.ShouldReturnValue = true;
            graph.Degree(1).Should().Be(0);

            data1.ShouldReturnEdges = new[] { new Edge<int>(1, 2) };
            data2.ShouldReturnEdges = default;
            graph.Degree(1).Should().Be(1);

            data1.ShouldReturnEdges = default;
            data2.ShouldReturnEdges = new[] { new Edge<int>(3, 1) };
            graph.Degree(1).Should().Be(1);

            data1.ShouldReturnEdges = new[] { new Edge<int>(1, 2), new Edge<int>(1, 3) };
            data2.ShouldReturnEdges = new[] { new Edge<int>(4, 1) };
            graph.Degree(1).Should().Be(3);

            // Self edge
            data1.ShouldReturnEdges = new[] { new Edge<int>(1, 2), new Edge<int>(1, 3), new Edge<int>(1, 1) };
            data2.ShouldReturnEdges = new[] { new Edge<int>(4, 1), new Edge<int>(1, 1) };
            graph.Degree(1).Should().Be(5);
        }

        #endregion

        #region Try Get Edges

        protected static void TryGetEdge_Test(
            GraphData<int, Edge<int>> data,
            IIncidenceGraph<int, Edge<int>> graph)
        {
            ContainsEdge_SourceTarget_GenericTest(
                data,
                (source, target) => graph.TryGetEdge(source, target, out _));
        }

        protected static void TryGetEdge_UndirectedGraph_Test(
            GraphData<int, Edge<int>> data,
            IImplicitUndirectedGraph<int, Edge<int>> graph)
        {
            ContainsEdge_SourceTarget_GenericTest(
                data,
                (source, target) => graph.TryGetEdge(source, target, out _),
                false);
        }

        protected static void TryGetEdges_Test(
            GraphData<int, Edge<int>> data,
            IIncidenceGraph<int, Edge<int>> graph)
        {
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            graph.TryGetEdges(0, 1, out _).Should().BeFalse();
            data.CheckCalls(1);

            data.ShouldReturnValue = true;
            graph.TryGetEdges(1, 2, out IEnumerable<Edge<int>>? edges).Should().BeTrue();
            edges.Should().BeEmpty();
            data.CheckCalls(1);

            data.ShouldReturnEdges = new[] { new Edge<int>(1, 2), new Edge<int>(1, 2) };
            graph.TryGetEdges(1, 2, out edges).Should().BeTrue();
            data.ShouldReturnEdges.Should().BeEquivalentTo(edges);
            data.CheckCalls(1);
        }

        protected static void TryGetEdges_Test(
            GraphData<int, Edge<int>> data,
            DelegateVertexAndEdgeListGraph<int, Edge<int>> graph)
        {
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            graph.TryGetEdges(0, 1, out _).Should().BeFalse();
            data.CheckCalls(0); // Vertex is not in graph so no need to call user code

            data.ShouldReturnValue = true;
            graph.TryGetEdges(1, 2, out IEnumerable<Edge<int>>? edges).Should().BeTrue();
            edges.Should().BeEmpty();
            data.CheckCalls(1);

            data.ShouldReturnEdges = new[] { new Edge<int>(1, 2), new Edge<int>(1, 2) };
            graph.TryGetEdges(1, 2, out edges).Should().BeTrue();
            data.ShouldReturnEdges.Should().BeEquivalentTo(edges);
            data.CheckCalls(1);

            var edge14 = new Edge<int>(1, 4);
            var edge12 = new Edge<int>(1, 2);
            var edge12Bis = new Edge<int>(1, 2);
            data.ShouldReturnValue = true;
            data.ShouldReturnEdges = new[] { edge14, edge12 };
            graph.TryGetEdges(1, 2, out edges).Should().BeTrue();
            new[] { edge12 }.Should().BeEquivalentTo(edges);
            data.CheckCalls(1);

            data.ShouldReturnEdges = new[] { edge14, edge12, edge12Bis };
            graph.TryGetEdges(1, 2, out edges).Should().BeTrue();
            new[] { edge12, edge12Bis }.Should().BeEquivalentTo(edges);
            data.CheckCalls(1);

            data.ShouldReturnEdges = new[] { edge14, edge12 };
            graph.TryGetEdges(2, 1, out edges).Should().BeTrue();
            edges.Should().BeEmpty();
            data.CheckCalls(1);

            var edge41 = new Edge<int>(4, 1);
            data.ShouldReturnEdges = new[] { edge14, edge41 };
            graph.TryGetEdges(1, 4, out edges).Should().BeTrue();
            edges.Should().BeEmpty();
            data.CheckCalls(1);

            graph.TryGetEdges(4, 1, out _).Should().BeFalse();
            data.CheckCalls(0);

            var edge45 = new Edge<int>(4, 5);
            data.ShouldReturnEdges = new[] { edge14, edge41, edge45 };
            graph.TryGetEdges(4, 5, out _).Should().BeFalse();
            data.CheckCalls(0);
        }

        protected static void TryGetOutEdges_Test(
            GraphData<int, Edge<int>> data,
            IImplicitGraph<int, Edge<int>> graph)
        {
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            graph.TryGetOutEdges(1, out _).Should().BeFalse();
            data.CheckCalls(1);

            data.ShouldReturnValue = true;
            graph.TryGetOutEdges(1, out IEnumerable<Edge<int>>? edges).Should().BeTrue();
            edges.Should().BeEmpty();
            data.CheckCalls(1);

            data.ShouldReturnEdges = new[] { new Edge<int>(1, 4), new Edge<int>(1, 2) };
            graph.TryGetOutEdges(1, out edges).Should().BeTrue();
            data.ShouldReturnEdges.Should().BeEquivalentTo(edges);
            data.CheckCalls(1);
        }

        protected static void TryGetOutEdges_Test(
            GraphData<int, Edge<int>> data,
            DelegateVertexAndEdgeListGraph<int, Edge<int>> graph)
        {
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            graph.TryGetOutEdges(5, out _).Should().BeFalse();
            data.CheckCalls(0); // Vertex is not in graph so no need to call user code

            data.ShouldReturnValue = true;
            graph.TryGetOutEdges(1, out IEnumerable<Edge<int>>? edges).Should().BeTrue();
            edges.Should().BeEmpty();
            data.CheckCalls(1);

            data.ShouldReturnEdges = new[] { new Edge<int>(1, 4), new Edge<int>(1, 2) };
            graph.TryGetOutEdges(1, out edges).Should().BeTrue();
            data.ShouldReturnEdges.Should().BeEquivalentTo(edges);
            data.CheckCalls(1);

            data.ShouldReturnEdges = default;
            graph.TryGetOutEdges(1, out IEnumerable<Edge<int>>? outEdges).Should().BeTrue();
            outEdges.Should().BeEmpty();
            data.CheckCalls(1);

            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge15 = new Edge<int>(1, 5);
            var edge21 = new Edge<int>(2, 1);
            var edge23 = new Edge<int>(2, 3);
            data.ShouldReturnEdges = new[] { edge12, edge13, edge15, edge21, edge23 };
            graph.TryGetOutEdges(1, out outEdges).Should().BeTrue();
            new[] { edge12, edge13 }.Should().BeEquivalentTo(outEdges);
            data.CheckCalls(1);

            var edge52 = new Edge<int>(5, 2);
            data.ShouldReturnEdges = new[] { edge15, edge52 };
            graph.TryGetOutEdges(5, out _).Should().BeFalse();
            data.CheckCalls(0); // Vertex is not in graph so no need to call user code
        }

        protected static void TryGetAdjacentEdges_Test(
            GraphData<int, Edge<int>> data,
            DelegateImplicitUndirectedGraph<int, Edge<int>> graph)
        {
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            graph.TryGetAdjacentEdges(1, out _).Should().BeFalse();
            data.CheckCalls(1);

            data.ShouldReturnValue = true;
            graph.TryGetAdjacentEdges(1, out IEnumerable<Edge<int>>? edges).Should().BeTrue();
            edges.Should().BeEmpty();
            data.CheckCalls(1);

            data.ShouldReturnEdges = new[] { new Edge<int>(1, 4), new Edge<int>(1, 2) };
            graph.TryGetAdjacentEdges(1, out edges).Should().BeTrue();
            data.ShouldReturnEdges.Should().BeEquivalentTo(edges);
            data.CheckCalls(1);
        }

        protected static void TryGetAdjacentEdges_Test(
            GraphData<int, Edge<int>> data,
            DelegateUndirectedGraph<int, Edge<int>> graph)
        {
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            graph.TryGetAdjacentEdges(5, out _).Should().BeFalse();
            data.CheckCalls(0); // Vertex is not in graph so no need to call user code

            data.ShouldReturnValue = true;
            graph.TryGetAdjacentEdges(1, out IEnumerable<Edge<int>>? edges).Should().BeTrue();
            edges!.Should().BeEmpty();
            data.CheckCalls(1);

            data.ShouldReturnEdges = new[] { new Edge<int>(1, 4), new Edge<int>(1, 2) };
            graph.TryGetAdjacentEdges(1, out edges).Should().BeTrue();
            data.ShouldReturnEdges.Should().BeEquivalentTo(edges);
            data.CheckCalls(1);

            data.ShouldReturnEdges = default;
            graph.TryGetAdjacentEdges(1, out IEnumerable<Edge<int>>? adjacentEdges).Should().BeTrue();
            adjacentEdges!.Should().BeEmpty();
            data.CheckCalls(1);

            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge15 = new Edge<int>(1, 5);
            var edge21 = new Edge<int>(2, 1);
            var edge23 = new Edge<int>(2, 3);
            data.ShouldReturnEdges = new[] { edge12, edge13, edge15, edge21, edge23 };
            graph.TryGetAdjacentEdges(1, out adjacentEdges).Should().BeTrue();
            new[] { edge12, edge13, edge21 }.Should().BeEquivalentTo(adjacentEdges);
            data.CheckCalls(1);

            var edge52 = new Edge<int>(5, 2);
            data.ShouldReturnEdges = new[] { edge15, edge52 };
            graph.TryGetAdjacentEdges(5, out _).Should().BeFalse();
            data.CheckCalls(0); // Vertex is not in graph so no need to call user code
        }

        protected static void TryGetAdjacentEdges_Throws_Test<TVertex, TEdge>(
            DelegateImplicitUndirectedGraph<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8604
            Invoking(() => graph.TryGetAdjacentEdges(default, out _)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8604
        }

        protected static void TryGetInEdges_Test(
            GraphData<int, Edge<int>> data,
            IBidirectionalIncidenceGraph<int, Edge<int>> graph)
        {
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            graph.TryGetInEdges(1, out _).Should().BeFalse();
            data.CheckCalls(1);

            data.ShouldReturnValue = true;
            graph.TryGetInEdges(1, out IEnumerable<Edge<int>>? edges).Should().BeTrue();
            edges.Should().BeEmpty();
            data.CheckCalls(1);

            data.ShouldReturnEdges = new[] { new Edge<int>(4, 1), new Edge<int>(2, 1) };
            graph.TryGetInEdges(1, out edges).Should().BeTrue();
            data.ShouldReturnEdges.Should().BeEquivalentTo(edges);
            data.CheckCalls(1);
        }

        #endregion

        #endregion
    }
}
