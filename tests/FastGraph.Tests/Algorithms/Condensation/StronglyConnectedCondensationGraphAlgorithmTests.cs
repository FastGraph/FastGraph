#nullable enable

using NUnit.Framework;
using FastGraph.Algorithms;
using FastGraph.Algorithms.Condensation;

namespace FastGraph.Tests.Algorithms.Condensation
{
    /// <summary>
    /// Tests for <see cref="CondensationGraphAlgorithm{TVertex,TEdge,TGraph}"/> (strongly connected).
    /// </summary>
    [TestFixture]
    internal sealed class StronglyConnectedCondensationGraphAlgorithmTests : CondensationGraphAlgorithmTestsBase
    {
        #region Test helpers

        private static void RunStronglyConnectedCondensationAndCheck<TVertex, TEdge>(
            IVertexAndEdgeListGraph<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            IMutableBidirectionalGraph<AdjacencyGraph<TVertex, TEdge>, CondensedEdge<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>> condensedGraph =
                graph.CondensateStronglyConnected<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>();

            condensedGraph.Should().NotBeNull();
            CheckVertexCount(graph, condensedGraph);
            CheckEdgeCount(graph, condensedGraph);
            CheckComponentCount(graph, condensedGraph);
            CheckDAG(condensedGraph);
        }

        private static void CheckComponentCount<TVertex, TEdge>(
            IVertexListGraph<TVertex, TEdge> graph,
            IVertexSet<AdjacencyGraph<TVertex, TEdge>> condensedGraph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            // Check number of vertices = number of strongly connected components
            var components = new Dictionary<TVertex, int>();
            int componentCount = graph.StronglyConnectedComponents(components);
            condensedGraph.VertexCount.Should().Be(componentCount, because: "Component count does not match.");
        }

        #endregion

        [Test]
        public void OneStronglyConnectedComponent()
        {
            var edge12 = new Edge<int>(1, 2);
            var edge23 = new Edge<int>(2, 3);
            var edge31 = new Edge<int>(3, 1);

            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge23, edge31
            });

            IMutableBidirectionalGraph<AdjacencyGraph<int, Edge<int>>, CondensedEdge<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>> condensedGraph =
                graph.CondensateStronglyConnected<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>();

            condensedGraph.Should().NotBeNull();
            condensedGraph.VertexCount.Should().Be(1);
            condensedGraph.EdgeCount.Should().Be(0);
            condensedGraph.Vertices.ElementAt(0).Vertices.Should().BeEquivalentTo(graph.Vertices);
            condensedGraph.Vertices.ElementAt(0).Edges.Should().BeEquivalentTo(graph.Edges);
        }

        [Test]
        public void MultipleStronglyConnectedComponents()
        {
            var edge12 = new Edge<int>(1, 2);
            var edge23 = new Edge<int>(2, 3);
            var edge24 = new Edge<int>(2, 4);
            var edge25 = new Edge<int>(2, 5);
            var edge31 = new Edge<int>(3, 1);
            var edge34 = new Edge<int>(3, 4);
            var edge46 = new Edge<int>(4, 6);
            var edge56 = new Edge<int>(5, 6);
            var edge57 = new Edge<int>(5, 7);
            var edge64 = new Edge<int>(6, 4);
            var edge75 = new Edge<int>(7, 5);
            var edge78 = new Edge<int>(7, 8);
            var edge86 = new Edge<int>(8, 6);
            var edge87 = new Edge<int>(8, 7);

            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge23, edge24, edge25, edge31, edge34, edge46,
                edge56, edge57, edge64, edge75, edge78, edge86, edge87
            });
            graph.AddVertex(10);

            IMutableBidirectionalGraph<AdjacencyGraph<int, Edge<int>>, CondensedEdge<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>> condensedGraph =
                graph.CondensateStronglyConnected<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>();

            condensedGraph.Should().NotBeNull();
            condensedGraph.VertexCount.Should().Be(4);
            condensedGraph.EdgeCount.Should().Be(3);

            // Condensed edge
            condensedGraph.Edges.ElementAt(0).Edges.Should().BeEquivalentTo(new[] { edge56, edge86 });
            condensedGraph.Edges.ElementAt(1).Edges.Should().BeEquivalentTo(new[] { edge24, edge34 });
            condensedGraph.Edges.ElementAt(2).Edges.Should().BeEquivalentTo(new[] { edge25 });

            // Components
            condensedGraph.Vertices.ElementAt(0).Vertices.Should().BeEquivalentTo(new[] { 4, 6 });
            condensedGraph.Vertices.ElementAt(0).Edges.Should().BeEquivalentTo(new[] { edge46, edge64 });

            condensedGraph.Vertices.ElementAt(1).Vertices.Should().BeEquivalentTo(new[] { 5, 7, 8 });
            condensedGraph.Vertices.ElementAt(1).Edges.Should().BeEquivalentTo(new[] { edge57, edge75, edge78, edge87 });

            condensedGraph.Vertices.ElementAt(2).Vertices.Should().BeEquivalentTo(new[] { 1, 2, 3 });
            condensedGraph.Vertices.ElementAt(2).Edges.Should().BeEquivalentTo(new[] { edge12, edge23, edge31 });

            condensedGraph.Vertices.ElementAt(3).Vertices.Should().BeEquivalentTo(new[] { 10 });
            condensedGraph.Vertices.ElementAt(3).Edges.Should().BeEmpty();
        }

        [TestCaseSource(nameof(AdjacencyGraphs_All))]
        [Category(TestCategories.LongRunning)]
        public void StronglyConnectedCondensation(TestGraphInstance<AdjacencyGraph<string, Edge<string>>, string> testGraph)
        {
            RunStronglyConnectedCondensationAndCheck(testGraph.Instance);
        }

        private static readonly IEnumerable<TestCaseData> AdjacencyGraphs_All =
            TestGraphFactory
                .SampleAdjacencyGraphs()
                .Select(t => new TestCaseData(t) { TestName = t.DescribeForTestCase() })
                .Memoize();
    }
}
