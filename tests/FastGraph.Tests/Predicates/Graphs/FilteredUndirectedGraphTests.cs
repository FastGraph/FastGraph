#nullable enable

using NUnit.Framework;
using FastGraph.Predicates;
using static FastGraph.Tests.GraphTestHelpers;

namespace FastGraph.Tests.Predicates
{
    /// <summary>
    /// Tests for <see cref="FilteredUndirectedGraph{TVertex,TEdge,TGraph}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class FilteredUndirectedGraphTests : FilteredGraphTestsBase
    {
        [Test]
        public void Construction()
        {
            VertexPredicate<int> vertexPredicate = _ => true;
            EdgePredicate<int, Edge<int>> edgePredicate = _ => true;

            var graph = new UndirectedGraph<int, Edge<int>>();
            var filteredGraph = new FilteredUndirectedGraph<int, Edge<int>, UndirectedGraph<int, Edge<int>>>(
                graph,
                vertexPredicate,
                edgePredicate);
            AssertGraphProperties(filteredGraph);

            graph = new UndirectedGraph<int, Edge<int>>(false);
            filteredGraph = new FilteredUndirectedGraph<int, Edge<int>, UndirectedGraph<int, Edge<int>>>(
                graph,
                vertexPredicate,
                edgePredicate);
            AssertGraphProperties(filteredGraph, false);

            #region Local function

            void AssertGraphProperties<TVertex, TEdge, TGraph>(
                FilteredUndirectedGraph<TVertex, TEdge, TGraph> g,
                bool parallelEdges = true)
                where TVertex : notnull
                where TEdge : IEdge<TVertex>
                where TGraph : IUndirectedGraph<TVertex, TEdge>
            {
                Assert.AreSame(graph, g.BaseGraph);
                Assert.IsFalse(g.IsDirected);
                Assert.AreEqual(parallelEdges, g.AllowParallelEdges);
                Assert.AreSame(vertexPredicate, g.VertexPredicate);
                Assert.AreSame(edgePredicate, g.EdgePredicate);
                Assert.IsNotNull(g.EdgeEqualityComparer);
                AssertEmptyGraph(g);
            }

            #endregion
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Assert.Throws<ArgumentNullException>(
                () => new FilteredUndirectedGraph<int, Edge<int>, UndirectedGraph<int, Edge<int>>>(
                    new UndirectedGraph<int, Edge<int>>(),
                    _ => true,
                    default));

            Assert.Throws<ArgumentNullException>(
                () => new FilteredUndirectedGraph<int, Edge<int>, UndirectedGraph<int, Edge<int>>>(
                    new UndirectedGraph<int, Edge<int>>(),
                    default,
                    _ => true));

            Assert.Throws<ArgumentNullException>(
                () => new FilteredUndirectedGraph<int, Edge<int>, UndirectedGraph<int, Edge<int>>>(
                    default,
                    _ => true,
                    _ => true));

            Assert.Throws<ArgumentNullException>(
                () => new FilteredUndirectedGraph<int, Edge<int>, UndirectedGraph<int, Edge<int>>>(
                    new UndirectedGraph<int, Edge<int>>(),
                    default,
                    default));

            Assert.Throws<ArgumentNullException>(
                () => new FilteredUndirectedGraph<int, Edge<int>, UndirectedGraph<int, Edge<int>>>(
                    default,
                    _ => true,
                    default));

            Assert.Throws<ArgumentNullException>(
                () => new FilteredUndirectedGraph<int, Edge<int>, UndirectedGraph<int, Edge<int>>>(
                    default,
                    default,
                    _ => true));

            Assert.Throws<ArgumentNullException>(
                () => new FilteredUndirectedGraph<int, Edge<int>, UndirectedGraph<int, Edge<int>>>(
                    default,
                    default,
                    default));
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        #region Vertices & Edges

        [Test]
        public void Vertices()
        {
            var wrappedGraph = new UndirectedGraph<int, Edge<int>>();
            Vertices_Test(
                wrappedGraph,
                (vertexPredicate, edgePredicate) =>
                    new FilteredUndirectedGraph<int, Edge<int>, UndirectedGraph<int, Edge<int>>>(
                        wrappedGraph,
                        vertexPredicate,
                        edgePredicate));
        }

        [Test]
        public void Edges()
        {
            var wrappedGraph = new UndirectedGraph<int, Edge<int>>();
            Edges_Test(
                wrappedGraph,
                (vertexPredicate, edgePredicate) =>
                    new FilteredUndirectedGraph<int, Edge<int>, UndirectedGraph<int, Edge<int>>>(
                        wrappedGraph,
                        vertexPredicate,
                        edgePredicate));
        }

        #endregion

        #region Contains Vertex

        [Test]
        public void ContainsVertex()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            ContainsVertex_Test(
                graph,
                (vertexPredicate, edgePredicate) =>
                    new FilteredUndirectedGraph<int, Edge<int>, UndirectedGraph<int, Edge<int>>>(
                        graph,
                        vertexPredicate,
                        edgePredicate));
        }

        [Test]
        public void ContainsVertex_Throws()
        {
            var filteredGraph = new FilteredUndirectedGraph<TestVertex, Edge<TestVertex>, UndirectedGraph<TestVertex, Edge<TestVertex>>>(
                new UndirectedGraph<TestVertex, Edge<TestVertex>>(),
                _ => true,
                _ => true);
            ContainsVertex_Throws_Test(filteredGraph);
        }

        #endregion

        #region Contains Edge

        [Test]
        public void ContainsEdge()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            ContainsEdge_Test(
                graph,
                (vertexPredicate, edgePredicate) =>
                    new FilteredUndirectedGraph<int, Edge<int>, UndirectedGraph<int, Edge<int>>>(
                        graph,
                        vertexPredicate,
                        edgePredicate));
        }

        [Test]
        public void ContainsEdge_EquatableEdge()
        {
            var graph = new UndirectedGraph<int, EquatableEdge<int>>();
            ContainsEdge_EquatableEdge_Test(
                graph,
                (vertexPredicate, edgePredicate) =>
                    new FilteredUndirectedGraph<int, EquatableEdge<int>, UndirectedGraph<int, EquatableEdge<int>>>(
                        graph,
                        vertexPredicate,
                        edgePredicate));
        }

        [Test]
        public void ContainsEdge_SourceTarget()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            ContainsEdge_SourceTarget_UndirectedGraph_Test(
                graph,
                (vertexPredicate, edgePredicate) =>
                    new FilteredUndirectedGraph<int, Edge<int>, UndirectedGraph<int, Edge<int>>>(
                        graph,
                        vertexPredicate,
                        edgePredicate));
        }

        [Test]
        public void ContainsEdge_Throws()
        {
            var filteredGraph = new FilteredUndirectedGraph<TestVertex, Edge<TestVertex>, UndirectedGraph<TestVertex, Edge<TestVertex>>>(
                new UndirectedGraph<TestVertex, Edge<TestVertex>>(),
                _ => true,
                _ => true);
            ContainsEdge_NullThrows_Test(filteredGraph);
            ContainsEdge_SourceTarget_Throws_UndirectedGraph_Test(filteredGraph);
        }

        #endregion

        #region Adjacent Edges

        [Test]
        public void AdjacentEdge()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            AdjacentEdge_Test(
                graph,
                (vertexPredicate, edgePredicate) =>
                    new FilteredUndirectedGraph<int, Edge<int>, UndirectedGraph<int, Edge<int>>>(
                        graph,
                        vertexPredicate,
                        edgePredicate));
        }

        [Test]
        public void AdjacentEdge_Throws()
        {
            var graph1 = new UndirectedGraph<int, Edge<int>>();
            AdjacentEdge_Throws_Test(
                graph1,
                (vertexPredicate, edgePredicate) =>
                    new FilteredUndirectedGraph<int, Edge<int>, UndirectedGraph<int, Edge<int>>>(
                        graph1,
                        vertexPredicate,
                        edgePredicate));

            var graph2 = new UndirectedGraph<TestVertex, Edge<TestVertex>>();
            var filteredGraph2 = new FilteredUndirectedGraph<TestVertex, Edge<TestVertex>, UndirectedGraph<TestVertex, Edge<TestVertex>>>(
                graph2,
                _ => true,
                _ => true);
            AdjacentEdge_NullThrows_Test(filteredGraph2);
        }

        [Test]
        public void AdjacentEdges()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            AdjacentEdges_Test(
                graph,
                (vertexPredicate, edgePredicate) =>
                    new FilteredUndirectedGraph<int, Edge<int>, UndirectedGraph<int, Edge<int>>>(
                        graph,
                        vertexPredicate,
                        edgePredicate));
        }

        [Test]
        public void AdjacentEdges_Throws()
        {
            var graph1 = new UndirectedGraph<EquatableTestVertex, Edge<EquatableTestVertex>>();
            var filteredGraph1 = new FilteredUndirectedGraph
                <
                    EquatableTestVertex,
                    Edge<EquatableTestVertex>,
                    UndirectedGraph<EquatableTestVertex, Edge<EquatableTestVertex>>
                >(
                graph1,
                _ => true,
                _ => true);
            AdjacentEdges_NullThrows_Test(filteredGraph1);
            AdjacentEdges_Throws_Test(filteredGraph1);

            var graph2 = new UndirectedGraph<int, Edge<int>>();
            var filteredGraph2 = new FilteredUndirectedGraph<int, Edge<int>, UndirectedGraph<int, Edge<int>>>(
                graph2,
                vertex => vertex < 4,
                _ => true);

            graph2.AddVertexRange(new[] { 1, 2, 3, 4, 5 });
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<VertexNotFoundException>(() => filteredGraph2.AdjacentEdges(4));
            Assert.Throws<VertexNotFoundException>(() => filteredGraph2.AdjacentEdges(5));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        #endregion

        #region Try Get Edges

        [Test]
        public void TryGetEdge()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            TryGetEdge_UndirectedGraph_Test(
                graph,
                (vertexPredicate, edgePredicate) =>
                    new FilteredUndirectedGraph<int, Edge<int>, UndirectedGraph<int, Edge<int>>>(
                        graph,
                        vertexPredicate,
                        edgePredicate));
        }

        [Test]
        public void TryGetEdge_Throws()
        {
            var filteredGraph = new FilteredUndirectedGraph<TestVertex, Edge<TestVertex>, UndirectedGraph<TestVertex, Edge<TestVertex>>>(
                new UndirectedGraph<TestVertex, Edge<TestVertex>>(),
                _ => true,
                _ => true);
            TryGetEdge_Throws_UndirectedGraph_Test(filteredGraph);
        }

        #endregion
    }
}
