#nullable enable

using NUnit.Framework;
using FastGraph.Predicates;
using static FastGraph.Tests.GraphTestHelpers;

namespace FastGraph.Tests.Predicates
{
    /// <summary>
    /// Tests for <see cref="FilteredEdgeListGraph{TVertex,TEdge,TGraph}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class FilteredEdgeListGraphTests : FilteredGraphTestsBase
    {
        [Test]
        public void Construction()
        {
            VertexPredicate<int> vertexPredicate = _ => true;
            EdgePredicate<int, Edge<int>> edgePredicate = _ => true;

            var graph1 = new AdjacencyGraph<int, Edge<int>>();
            var filteredGraph1 = new FilteredEdgeListGraph<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(
                graph1,
                vertexPredicate,
                edgePredicate);
            AssertGraphProperties(filteredGraph1, graph1);

            graph1 = new AdjacencyGraph<int, Edge<int>>(false);
            filteredGraph1 = new FilteredEdgeListGraph<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(
                graph1,
                vertexPredicate,
                edgePredicate);
            AssertGraphProperties(filteredGraph1, graph1, parallelEdges: false);

            var graph2 = new UndirectedGraph<int, Edge<int>>();
            var filteredGraph2 = new FilteredEdgeListGraph<int, Edge<int>, UndirectedGraph<int, Edge<int>>>(
                graph2,
                vertexPredicate,
                edgePredicate);
            AssertGraphProperties(filteredGraph2, graph2, false);

            graph2 = new UndirectedGraph<int, Edge<int>>(false);
            filteredGraph2 = new FilteredEdgeListGraph<int, Edge<int>, UndirectedGraph<int, Edge<int>>>(
                graph2,
                vertexPredicate,
                edgePredicate);
            AssertGraphProperties(filteredGraph2, graph2, false, false);

            #region Local function

            void AssertGraphProperties<TVertex, TEdge, TGraph>(
                FilteredEdgeListGraph<TVertex, TEdge, TGraph> g,
                TGraph expectedGraph,
                bool isDirected = true,
                bool parallelEdges = true)
                where TVertex : notnull
                where TEdge : IEdge<TVertex>
                where TGraph : IEdgeListGraph<TVertex, TEdge>
            {
                Assert.AreSame(expectedGraph, g.BaseGraph);
                Assert.AreEqual(isDirected, g.IsDirected);
                Assert.AreEqual(parallelEdges, g.AllowParallelEdges);
                Assert.AreSame(vertexPredicate, g.VertexPredicate);
                Assert.AreSame(edgePredicate, g.EdgePredicate);
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
                () => new FilteredEdgeListGraph<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(
                    new AdjacencyGraph<int, Edge<int>>(),
                    _ => true,
                    default));

            Assert.Throws<ArgumentNullException>(
                () => new FilteredEdgeListGraph<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(
                    new AdjacencyGraph<int, Edge<int>>(),
                    default,
                    _ => true));

            Assert.Throws<ArgumentNullException>(
                () => new FilteredEdgeListGraph<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(
                    default,
                    _ => true,
                    _ => true));

            Assert.Throws<ArgumentNullException>(
                () => new FilteredEdgeListGraph<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(
                    new AdjacencyGraph<int, Edge<int>>(),
                    default,
                    default));

            Assert.Throws<ArgumentNullException>(
                () => new FilteredEdgeListGraph<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(
                    default,
                    _ => true,
                    default));

            Assert.Throws<ArgumentNullException>(
                () => new FilteredEdgeListGraph<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(
                    default,
                    default,
                    _ => true));

            Assert.Throws<ArgumentNullException>(
                () => new FilteredEdgeListGraph<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(
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
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();
            Vertices_Test(
                wrappedGraph,
                (vertexPredicate, edgePredicate) =>
                    new FilteredEdgeListGraph<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(
                        wrappedGraph,
                        vertexPredicate,
                        edgePredicate));
        }

        [Test]
        public void Edges()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();
            Edges_Test(
                wrappedGraph,
                (vertexPredicate, edgePredicate) =>
                    new FilteredEdgeListGraph<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(
                        wrappedGraph,
                        vertexPredicate,
                        edgePredicate));
        }

        #endregion

        #region Contains Vertex

        [Test]
        public void ContainsVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            ContainsVertex_Test(
                graph,
                (vertexPredicate, edgePredicate) =>
                    new FilteredEdgeListGraph<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(
                        graph,
                        vertexPredicate,
                        edgePredicate));
        }

        [Test]
        public void ContainsVertex_Throws()
        {
            var filteredGraph = new FilteredEdgeListGraph<TestVertex, Edge<TestVertex>, AdjacencyGraph<TestVertex, Edge<TestVertex>>>(
                new AdjacencyGraph<TestVertex, Edge<TestVertex>>(),
                _ => true,
                _ => true);
            ContainsVertex_Throws_Test(filteredGraph);
        }

        #endregion

        #region Contains Edge

        [Test]
        public void ContainsEdge()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            ContainsEdge_Test(
                graph,
                (vertexPredicate, edgePredicate) =>
                    new FilteredEdgeListGraph<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(
                        graph,
                        vertexPredicate,
                        edgePredicate));
        }

        [Test]
        public void ContainsEdge_EquatableEdge()
        {
            var graph = new AdjacencyGraph<int, EquatableEdge<int>>();
            ContainsEdge_EquatableEdge_Test(
                graph,
                (vertexPredicate, edgePredicate) =>
                    new FilteredEdgeListGraph<int, EquatableEdge<int>, AdjacencyGraph<int, EquatableEdge<int>>>(
                        graph,
                        vertexPredicate,
                        edgePredicate));
        }

        #endregion
    }
}
