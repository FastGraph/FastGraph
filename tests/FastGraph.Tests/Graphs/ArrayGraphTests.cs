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

        [TestCaseSource(nameof(AdjacencyGraphs_All))]
        public void ConversionToArrayGraph_Adjacency(TestGraphInstance<AdjacencyGraph<string, Edge<string>>, string> testGraph)
        {
            AssertSameProperties(testGraph.Instance);
        }

        [TestCaseSource(nameof(BidirectionalGraphs_All))]
        public void ConversionToArrayGraph_Bidirectional(TestGraphInstance<BidirectionalGraph<string, Edge<string>>, string> testGraph)
        {
            AssertSameProperties(testGraph.Instance);
        }

        [TestCaseSource(nameof(UndirectedGraphs_All))]
        public void ConversionToArrayGraph_Undirected(TestGraphInstance<UndirectedGraph<string, Edge<string>>, string> testGraph)
        {
            AssertSameProperties(testGraph.Instance);
        }

        private static readonly IEnumerable<TestCaseData> AdjacencyGraphs_All =
            TestGraphFactory
                .SampleAdjacencyGraphs()
                .Select(t => new TestCaseData(t) { TestName = t.DescribeForTestCase() })
                .Memoize();

        private static readonly IEnumerable<TestCaseData> BidirectionalGraphs_All =
            TestGraphFactory
                .SampleBidirectionalGraphs()
                .Select(t => new TestCaseData(t) { TestName = t.DescribeForTestCase() })
                .Memoize();

        private static readonly IEnumerable<TestCaseData> UndirectedGraphs_All =
            TestGraphFactory
                .SampleUndirectedGraphs()
                .Select(t => new TestCaseData(t) { TestName = t.DescribeForTestCase() })
                .Memoize();
    }
}
