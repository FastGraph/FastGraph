#nullable enable

using NUnit.Framework;
using FastGraph.Predicates;

namespace FastGraph.Tests.Predicates
{
    /// <summary>
    /// Tests for <see cref="FilteredGraph{TVertex,TEdge,TGraph}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class FilteredGraphTests
    {
        [Test]
        public void Construction()
        {
            VertexPredicate<int> vertexPredicate = _ => true;
            EdgePredicate<int, Edge<int>> edgePredicate = _ => true;

            var graph1 = new AdjacencyGraph<int, Edge<int>>();
            var filteredGraph1 = new FilteredGraph<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(
                graph1,
                vertexPredicate,
                edgePredicate);
            AssertGraphProperties(filteredGraph1, graph1);

            graph1 = new AdjacencyGraph<int, Edge<int>>(false);
            filteredGraph1 = new FilteredGraph<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(
                graph1,
                vertexPredicate,
                edgePredicate);
            AssertGraphProperties(filteredGraph1, graph1, parallelEdges: false);

            var graph2 = new UndirectedGraph<int, Edge<int>>();
            var filteredGraph2 = new FilteredGraph<int, Edge<int>, UndirectedGraph<int, Edge<int>>>(
                graph2,
                vertexPredicate,
                edgePredicate);
            AssertGraphProperties(filteredGraph2, graph2, false);

            graph2 = new UndirectedGraph<int, Edge<int>>(false);
            filteredGraph2 = new FilteredGraph<int, Edge<int>, UndirectedGraph<int, Edge<int>>>(
                graph2,
                vertexPredicate,
                edgePredicate);
            AssertGraphProperties(filteredGraph2, graph2, false, false);

            #region Local function

            void AssertGraphProperties<TVertex, TEdge, TGraph>(
                FilteredGraph<TVertex, TEdge, TGraph> g,
                TGraph expectedGraph,
                bool isDirected = true,
                bool parallelEdges = true)
                where TVertex : notnull
                where TEdge : IEdge<TVertex>
                where TGraph : IGraph<TVertex, TEdge>
            {
                g.BaseGraph.Should().BeSameAs(expectedGraph);
                g.IsDirected.Should().Be(isDirected);
                g.AllowParallelEdges.Should().Be(parallelEdges);
                g.VertexPredicate.Should().BeSameAs(vertexPredicate);
                g.EdgePredicate.Should().BeSameAs(edgePredicate);
            }

            #endregion
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new FilteredGraph<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(
                new AdjacencyGraph<int, Edge<int>>(),
                _ => true,
                default)).Should().Throw<ArgumentNullException>();

            Invoking(() => new FilteredGraph<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(
                new AdjacencyGraph<int, Edge<int>>(),
                default,
                _ => true)).Should().Throw<ArgumentNullException>();

            Invoking(() => new FilteredGraph<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(
                default,
                _ => true,
                _ => true)).Should().Throw<ArgumentNullException>();

            Invoking(() => new FilteredGraph<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(
                new AdjacencyGraph<int, Edge<int>>(),
                default,
                default)).Should().Throw<ArgumentNullException>();

            Invoking(() => new FilteredGraph<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(
                default,
                _ => true,
                default)).Should().Throw<ArgumentNullException>();

            Invoking(() => new FilteredGraph<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(
                default,
                default,
                _ => true)).Should().Throw<ArgumentNullException>();

            Invoking(() => new FilteredGraph<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(
                default,
                default,
                default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }
    }
}
