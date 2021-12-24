#nullable enable

using NUnit.Framework;
using FastGraph.Algorithms.MinimumSpanningTree;
using static FastGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace FastGraph.Tests.Algorithms.MinimumSpanningTree
{
    /// <summary>
    /// Tests for <see cref="KruskalMinimumSpanningTreeAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class KruskalMinimumSpanningTreeTests : MinimumSpanningTreeTestsBase
    {
        [Test]
        public void Constructor()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            var algorithm = new KruskalMinimumSpanningTreeAlgorithm<int, Edge<int>>(graph, _ => 1.0);
            AssertAlgorithmState(algorithm, graph);

            algorithm = new KruskalMinimumSpanningTreeAlgorithm<int, Edge<int>>(default, graph, _ => 1.0);
            AssertAlgorithmState(algorithm, graph);
        }

        [Test]
        public void Constructor_Throws()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();

            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new KruskalMinimumSpanningTreeAlgorithm<int, Edge<int>>(default, _ => 1.0)).Should().Throw<ArgumentNullException>();
            Invoking(() => new KruskalMinimumSpanningTreeAlgorithm<int, Edge<int>>(graph, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new KruskalMinimumSpanningTreeAlgorithm<int, Edge<int>>(default, default)).Should().Throw<ArgumentNullException>();

            Invoking(() => new KruskalMinimumSpanningTreeAlgorithm<int, Edge<int>>(default, default, _ => 1.0)).Should().Throw<ArgumentNullException>();
            Invoking(() => new KruskalMinimumSpanningTreeAlgorithm<int, Edge<int>>(default, graph, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new KruskalMinimumSpanningTreeAlgorithm<int, Edge<int>>(default, default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Kruskal()
        {
            // Kruskal 10, 50, 100, 200, 300, 400
            UndirectedGraph<string, TaggedEdge<string, double>> graph = GetUndirectedCompleteGraph(10);
            KruskalSpanningTree(graph, x => x.Tag);

            graph = GetUndirectedCompleteGraph(50);
            KruskalSpanningTree(graph, x => x.Tag);

            graph = GetUndirectedCompleteGraph(100);
            KruskalSpanningTree(graph, x => x.Tag);

            graph = GetUndirectedCompleteGraph(200);
            KruskalSpanningTree(graph, x => x.Tag);

            graph = GetUndirectedCompleteGraph(300);
            KruskalSpanningTree(graph, x => x.Tag);

            graph = GetUndirectedCompleteGraph(400);
            KruskalSpanningTree(graph, x => x.Tag);
        }

        [TestCaseSource(nameof(UndirectedGraphs_All))]
        public void KruskalMinimumSpanningTree(TestGraphInstance<UndirectedGraph<string, Edge<string>>, string> testGraph)
        {
            Kruskal(testGraph.Instance);
        }

        private static readonly IEnumerable<TestCaseData> UndirectedGraphs_All =
            TestGraphFactory
                .SampleUndirectedGraphs()
                .Select(t => new TestCaseData(t) { TestName = t.DescribeForTestCase() })
                .Memoize();
    }
}
