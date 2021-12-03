using System;
using NUnit.Framework;
using FastGraph.Predicates;

namespace FastGraph.Tests.Predicates
{
    /// <summary>
    /// Tests for <see cref="FilteredImplicitVertexSet{TVertex,TEdge,TGraph}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class FilteredImplicitVertexSetGraphTests : FilteredGraphTestsBase
    {
        [Test]
        public void Construction()
        {
            VertexPredicate<int> vertexPredicate = _ => true;
            EdgePredicate<int, Edge<int>> edgePredicate = _ => true;

            var graph1 = new AdjacencyGraph<int, Edge<int>>();
            var filteredGraph1 = new FilteredImplicitVertexSet<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(
                graph1,
                vertexPredicate,
                edgePredicate);
            AssertGraphProperties(filteredGraph1, graph1);

            graph1 = new AdjacencyGraph<int, Edge<int>>(false);
            filteredGraph1 = new FilteredImplicitVertexSet<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(
                graph1,
                vertexPredicate,
                edgePredicate);
            AssertGraphProperties(filteredGraph1, graph1, parallelEdges: false);

            var graph2 = new UndirectedGraph<int, Edge<int>>();
            var filteredGraph2 = new FilteredImplicitVertexSet<int, Edge<int>, UndirectedGraph<int, Edge<int>>>(
                graph2,
                vertexPredicate,
                edgePredicate);
            AssertGraphProperties(filteredGraph2, graph2, false);

            graph2 = new UndirectedGraph<int, Edge<int>>(false);
            filteredGraph2 = new FilteredImplicitVertexSet<int, Edge<int>, UndirectedGraph<int, Edge<int>>>(
                graph2,
                vertexPredicate,
                edgePredicate);
            AssertGraphProperties(filteredGraph2, graph2, false, false);

            #region Local function

            void AssertGraphProperties<TVertex, TEdge, TGraph>(
                FilteredImplicitVertexSet<TVertex, TEdge, TGraph> g,
                TGraph expectedGraph,
                bool isDirected = true,
                bool parallelEdges = true)
                where TEdge : IEdge<TVertex>
                where TGraph : IGraph<TVertex, TEdge>, IImplicitVertexSet<TVertex>
            {
                Assert.AreSame(expectedGraph, g.BaseGraph);
                Assert.AreEqual(isDirected, g.IsDirected);
                Assert.AreEqual(parallelEdges, g.AllowParallelEdges);
                Assert.AreSame(vertexPredicate, g.VertexPredicate);
                Assert.AreSame(edgePredicate, g.EdgePredicate);
            }

            #endregion
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => new FilteredImplicitVertexSet<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(
                    new AdjacencyGraph<int, Edge<int>>(),
                    _ => true,
                    null));

            Assert.Throws<ArgumentNullException>(
                () => new FilteredImplicitVertexSet<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(
                    new AdjacencyGraph<int, Edge<int>>(),
                    null,
                    _ => true));

            Assert.Throws<ArgumentNullException>(
                () => new FilteredImplicitVertexSet<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(
                    null,
                    _ => true,
                    _ => true));

            Assert.Throws<ArgumentNullException>(
                () => new FilteredImplicitVertexSet<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(
                    new AdjacencyGraph<int, Edge<int>>(),
                    null,
                    null));

            Assert.Throws<ArgumentNullException>(
                () => new FilteredImplicitVertexSet<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(
                    null,
                    _ => true,
                    null));

            Assert.Throws<ArgumentNullException>(
                () => new FilteredImplicitVertexSet<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(
                    null,
                    null,
                    _ => true));

            Assert.Throws<ArgumentNullException>(
                () => new FilteredImplicitVertexSet<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(
                    null,
                    null,
                    null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        #region Contains Vertex

        [Test]
        public void ContainsVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            ContainsVertex_Test(
                graph,
                (vertexPredicate, edgePredicate) =>
                    new FilteredImplicitVertexSet<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(
                        graph,
                        vertexPredicate,
                        edgePredicate));
        }

        [Test]
        public void ContainsVertex_Throws()
        {
            var filteredGraph = new FilteredImplicitVertexSet<TestVertex, Edge<TestVertex>, AdjacencyGraph<TestVertex, Edge<TestVertex>>>(
                new AdjacencyGraph<TestVertex, Edge<TestVertex>>(),
                _ => true,
                _ => true);
            ContainsVertex_Throws_Test(filteredGraph);
        }

        #endregion
    }
}
