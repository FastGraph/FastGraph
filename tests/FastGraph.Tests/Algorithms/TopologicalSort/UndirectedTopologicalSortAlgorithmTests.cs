#nullable enable

using NUnit.Framework;
using FastGraph.Algorithms.TopologicalSort;
using static FastGraph.Tests.Algorithms.AlgorithmTestHelpers;


namespace FastGraph.Tests.Algorithms
{
    /// <summary>
    /// Tests for <see cref="UndirectedTopologicalSortAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class UndirectedTopologicalSortAlgorithmTests
    {
        #region Test helpers

        private static void RunUndirectedTopologicalSortAndCheck<TVertex, TEdge>(
            IUndirectedGraph<TVertex, TEdge> graph,
            bool allowCycles)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            var algorithm = new UndirectedTopologicalSortAlgorithm<TVertex, TEdge>(graph)
            {
                AllowCyclicGraph = allowCycles
            };

            algorithm.Compute();

            algorithm.SortedVertices.Should().NotBeNull();
            algorithm.SortedVertices!.Length.Should().Be(graph.VertexCount);
        }

        #endregion

        [Test]
        public void Constructor()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            var algorithm = new UndirectedTopologicalSortAlgorithm<int, Edge<int>>(graph);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new UndirectedTopologicalSortAlgorithm<int, Edge<int>>(graph)
            {
                AllowCyclicGraph = true
            };
            AssertAlgorithmProperties(algorithm, graph, true);

            algorithm = new UndirectedTopologicalSortAlgorithm<int, Edge<int>>(graph, 0);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new UndirectedTopologicalSortAlgorithm<int, Edge<int>>(graph, 10);
            AssertAlgorithmProperties(algorithm, graph);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                UndirectedTopologicalSortAlgorithm<TVertex, TEdge> algo,
                IUndirectedGraph<TVertex, TEdge> g,
                bool allowCycles = false)
                where TVertex : notnull
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                algo.SortedVertices.Should().BeNull();
                algo.AllowCyclicGraph.Should().Be(allowCycles);
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new UndirectedTopologicalSortAlgorithm<int, Edge<int>>(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        [Test]
        public void SimpleGraph()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(2, 3),
                new Edge<int>(4, 2),
                new Edge<int>(4, 5),
                new Edge<int>(5, 6),
                new Edge<int>(7, 5),
                new Edge<int>(7, 8)
            });

            var algorithm = new UndirectedTopologicalSortAlgorithm<int, Edge<int>>(graph);
            algorithm.Compute();

            // Order in undirected graph is some strange thing,
            // here the order is more vertices ordered by depth
            new[] { 1, 2, 4, 5, 7, 8, 6, 3 }.Should().BeEquivalentTo(algorithm.SortedVertices);
        }

        [Test]
        public void SimpleGraphOneToAnother()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(0, 1),
                new Edge<int>(1, 2),
                new Edge<int>(1, 3),
                new Edge<int>(3, 4)
            });

            var algorithm = new UndirectedTopologicalSortAlgorithm<int, Edge<int>>(graph);
            algorithm.Compute();

            // Order in undirected graph is some strange thing,
            // here the order is more vertices ordered by depth
            new[] { 0, 1, 3, 4, 2 }.Should().BeEquivalentTo(algorithm.SortedVertices);
        }

        [Test]
        public void ForestGraph()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(0, 1),
                new Edge<int>(1, 2),
                new Edge<int>(1, 3),
                new Edge<int>(3, 4),

                new Edge<int>(5, 6)
            });

            var algorithm = new UndirectedTopologicalSortAlgorithm<int, Edge<int>>(graph);
            algorithm.Compute();

            // Order in undirected graph is some strange thing,
            // here the order is more vertices ordered by depth
            new[] { 5, 6, 0, 1, 3, 4, 2 }.Should().BeEquivalentTo(algorithm.SortedVertices);
        }

        [Test]
        public void GraphWithSelfEdge()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(0, 1),
                new Edge<int>(1, 2),
                new Edge<int>(1, 3),
                new Edge<int>(2, 3),
                new Edge<int>(2, 2),
                new Edge<int>(3, 4)
            });

            var algorithm = new UndirectedTopologicalSortAlgorithm<int, Edge<int>>(graph);
            Invoking(() => algorithm.Compute()).Should().Throw<NonAcyclicGraphException>();

            algorithm = new UndirectedTopologicalSortAlgorithm<int, Edge<int>>(graph)
            {
                AllowCyclicGraph = true
            };
            algorithm.Compute();

            // Order in undirected graph is some strange thing,
            // here the order is more vertices ordered by depth
            new[] { 0, 1, 2, 3, 4 }.Should().BeEquivalentTo(algorithm.SortedVertices);
        }

        [TestCaseSource(nameof(UndirectedGraphs_All))]
        public void UndirectedTopologicalSort(TestGraphInstance<UndirectedGraph<string, Edge<string>>, string> testGraph)
        {
            RunUndirectedTopologicalSortAndCheck(testGraph.Instance, true);
        }

        [TestCaseSource(nameof(LoadUndirectedGraph_DCT8))]
        public void UndirectedTopologicalSort_DCT8(TestGraphInstance<UndirectedGraph<string, Edge<string>>, string> testGraph)
        {
            RunUndirectedTopologicalSortAndCheck(testGraph.Instance, true);
        }

        [Test]
        public void UndirectedTopologicalSort_Throws()
        {
            var cyclicGraph = new UndirectedGraph<int, Edge<int>>();
            cyclicGraph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(2, 3),
                new Edge<int>(1, 4),
                new Edge<int>(3, 1)
            });

            var algorithm = new UndirectedTopologicalSortAlgorithm<int, Edge<int>>(cyclicGraph);
            Invoking(() => algorithm.Compute()).Should().Throw<NonAcyclicGraphException>();

            algorithm = new UndirectedTopologicalSortAlgorithm<int, Edge<int>>(cyclicGraph)
            {
                AllowCyclicGraph = true
            };
            Invoking(() => algorithm.Compute()).Should().NotThrow();
        }

        private static readonly IEnumerable<TestCaseData> UndirectedGraphs_All =
            TestGraphFactory
                .SampleUndirectedGraphs()
                .Select(t => new TestCaseData(t) { TestName = t.DescribeForTestCase() })
                .Memoize();

        private static readonly IEnumerable<TestCaseData> LoadUndirectedGraph_DCT8 =
            new[] { new TestCaseData(TestGraphSourceProvider.Instance.DCT8.DeferDeserializeAsUndirectedGraph().CreateInstanceHandle()) };
    }
}
