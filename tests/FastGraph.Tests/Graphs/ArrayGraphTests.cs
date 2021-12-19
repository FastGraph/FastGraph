#nullable enable

using FluentAssertions.Execution;
using NUnit.Framework;

namespace FastGraph.Tests.Structures
{
    /// <summary>
    /// Tests related to array graphs.
    /// </summary>
    [TestFixture]
    internal sealed class ArrayGraphTests
    {
        #region Test helpers

        private static void AssertSameProperties<TVertex, TEdge>(IVertexAndEdgeListGraph<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            var adjacencyGraph = graph.ToArrayAdjacencyGraph();

            using (_ = new AssertionScope())
            {
                adjacencyGraph.VertexCount.Should().Be(graph.VertexCount);
                graph.Vertices.Should().BeEquivalentTo(adjacencyGraph.Vertices);

                adjacencyGraph.EdgeCount.Should().Be(graph.EdgeCount);
                graph.Edges.Should().BeEquivalentTo(adjacencyGraph.Edges);
            }

            using (_ = new AssertionScope())
            {
                foreach (TVertex vertex in graph.Vertices)
                {
                    graph.OutEdges(vertex).Should().BeEquivalentTo(adjacencyGraph.OutEdges(vertex));
                }
            }
        }

        private static void AssertSameProperties<TVertex, TEdge>(IBidirectionalGraph<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            var bidirectionalGraph = graph.ToArrayBidirectionalGraph();

            using (_ = new AssertionScope())
            {
                bidirectionalGraph.VertexCount.Should().Be(graph.VertexCount);
                graph.Vertices.Should().BeEquivalentTo(bidirectionalGraph.Vertices);

                bidirectionalGraph.EdgeCount.Should().Be(graph.EdgeCount);
                graph.Edges.Should().BeEquivalentTo(bidirectionalGraph.Edges);
            }

            using (_ = new AssertionScope())
            {
                foreach (TVertex vertex in graph.Vertices)
                {
                    graph.OutEdges(vertex).Should().BeEquivalentTo(bidirectionalGraph.OutEdges(vertex));
                }
            }
        }

        private static void AssertSameProperties<TVertex, TEdge>(IUndirectedGraph<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            var undirectedGraph = graph.ToArrayUndirectedGraph();

            using (_ = new AssertionScope())
            {
                undirectedGraph.VertexCount.Should().Be(graph.VertexCount);
                graph.Vertices.Should().BeEquivalentTo(undirectedGraph.Vertices);

                undirectedGraph.EdgeCount.Should().Be(graph.EdgeCount);
                graph.Edges.Should().BeEquivalentTo(undirectedGraph.Edges);
            }

            using (_ = new AssertionScope())
            {
                foreach (TVertex vertex in graph.Vertices)
                {
                    graph.AdjacentEdges(vertex).Should().BeEquivalentTo(undirectedGraph.AdjacentEdges(vertex));
                }
            }
        }

        #endregion

        [Test]
        public void ConversionToArrayGraph()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs_All())
                AssertSameProperties(graph);
            foreach (BidirectionalGraph<string, Edge<string>> graph in TestGraphFactory.GetBidirectionalGraphs_All())
                AssertSameProperties(graph);
            foreach (UndirectedGraph<string, Edge<string>> graph in TestGraphFactory.GetUndirectedGraphs_All())
                AssertSameProperties(graph);
        }
    }
}
