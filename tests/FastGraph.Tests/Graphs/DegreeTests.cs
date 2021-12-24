#nullable enable

using NUnit.Framework;

namespace FastGraph.Tests.Graphs
{
    /// <summary>
    /// Tests relative the to the degree of graphs.
    /// </summary>
    [TestFixture]
    internal sealed class DegreeTests
    {
        #region Test helpers

        private static void AssertDegreeSumEqualsTwiceEdgeCount<TVertex, TEdge>(
            IBidirectionalGraph<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            int totalDegree = 0;
            foreach (TVertex vertex in graph.Vertices)
                totalDegree += graph.Degree(vertex);

            totalDegree.Should().Be(graph.EdgeCount * 2);
        }

        private static void AssertInDegreeSumEqualsEdgeCount<TVertex, TEdge>(
            IBidirectionalGraph<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            int totalInDegree = 0;
            foreach (TVertex vertex in graph.Vertices)
                totalInDegree += graph.InDegree(vertex);

            totalInDegree.Should().Be(graph.EdgeCount);
        }

        private static void OutDegreeSumEqualsEdgeCount<TVertex, TEdge>(
            IVertexAndEdgeListGraph<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            int totalOutDegree = 0;
            foreach (TVertex vertex in graph.Vertices)
                totalOutDegree += graph.OutDegree(vertex);

            totalOutDegree.Should().Be(graph.EdgeCount);
        }

        private static void AssertAdjacentDegreeSumEqualsTwiceEdgeCount<TVertex, TEdge>(
            IUndirectedGraph<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            int totalAdjacentDegree = 0;
            foreach (TVertex vertex in graph.Vertices)
                totalAdjacentDegree += graph.AdjacentDegree(vertex);

            totalAdjacentDegree.Should().Be(graph.EdgeCount * 2);
        }

        #endregion

        [TestCaseSource(nameof(BidirectionalGraphs_All))]
        public void DegreeSumEqualsTwiceEdgeCount(TestGraphInstance<BidirectionalGraph<string, Edge<string>>, string> testGraph)
        {
            AssertDegreeSumEqualsTwiceEdgeCount(testGraph.Instance);
        }

        [TestCaseSource(nameof(BidirectionalGraphs_All))]
        public void InDegreeSumEqualsEdgeCount(TestGraphInstance<BidirectionalGraph<string, Edge<string>>, string> testGraph)
        {
            AssertInDegreeSumEqualsEdgeCount(testGraph.Instance);
        }

        [TestCaseSource(nameof(AdjacencyGraphs_All))]
        public void OutDegreeSumEqualsEdgeCount_Adjacency(TestGraphInstance<AdjacencyGraph<string, Edge<string>>, string> testGraph)
        {
            OutDegreeSumEqualsEdgeCount(testGraph.Instance);
        }

        [TestCaseSource(nameof(BidirectionalGraphs_All))]
        public void OutDegreeSumEqualsEdgeCount_Bidirectional(TestGraphInstance<BidirectionalGraph<string, Edge<string>>, string> testGraph)
        {
            OutDegreeSumEqualsEdgeCount(testGraph.Instance);
        }

        [TestCaseSource(nameof(UndirectedGraphs_All))]
        public void AdjacentDegreeSumEqualsTwiceEdgeCount(TestGraphInstance<UndirectedGraph<string, Edge<string>>, string> testGraph)
        {
            AssertAdjacentDegreeSumEqualsTwiceEdgeCount(testGraph.Instance);
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
