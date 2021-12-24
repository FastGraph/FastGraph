#nullable enable

using NUnit.Framework;
using FastGraph.Algorithms;
using FastGraph.Algorithms.Condensation;
using FastGraph.Algorithms.ConnectedComponents;
using static FastGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace FastGraph.Tests.Algorithms.Condensation
{
    /// <summary>
    /// Tests for <see cref="CondensationGraphAlgorithm{TVertex,TEdge,TGraph}"/> (weakly connected).
    /// </summary>
    [TestFixture]
    internal sealed class WeaklyConnectedCondensationGraphAlgorithmTests : CondensationGraphAlgorithmTestsBase
    {
        #region Test helpers

        private static void RunWeaklyConnectedCondensationAndCheck<TVertex, TEdge>(
            IVertexAndEdgeListGraph<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            IMutableBidirectionalGraph<AdjacencyGraph<TVertex, TEdge>, CondensedEdge<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>> condensedGraph =
                graph.CondensateWeaklyConnected<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>();

            condensedGraph.Should().NotBeNull();
            CheckVertexCount(graph, condensedGraph);
            CheckEdgeCount(graph, condensedGraph);
            CheckComponentCount(graph, condensedGraph);
        }

        private static void CheckComponentCount<TVertex, TEdge>(
            IVertexListGraph<TVertex, TEdge> graph,
            IVertexSet<AdjacencyGraph<TVertex, TEdge>> condensedGraph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            // Check number of vertices = number of strongly connected components
            int components = graph.WeaklyConnectedComponents(new Dictionary<TVertex, int>());
            condensedGraph.VertexCount.Should().Be(components, because: "Component count does not match.");
        }

        #endregion

        [Test]
        public void Constructor()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm1 = new CondensationGraphAlgorithm<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(graph);
            AssertAlgorithmProperties(algorithm1, graph);

            algorithm1 = new CondensationGraphAlgorithm<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(graph)
            {
                StronglyConnected = false
            };
            AssertAlgorithmProperties(algorithm1, graph, false);

            var algorithm2 = new CondensationGraphAlgorithm<int, Edge<int>, BidirectionalGraph<int, Edge<int>>>(graph);
            AssertAlgorithmProperties(algorithm2, graph);

            algorithm2 = new CondensationGraphAlgorithm<int, Edge<int>, BidirectionalGraph<int, Edge<int>>>(graph)
            {
                StronglyConnected = false
            };
            AssertAlgorithmProperties(algorithm2, graph, false);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge, TGraph>(
                CondensationGraphAlgorithm<TVertex, TEdge, TGraph> algo,
                IVertexAndEdgeListGraph<TVertex, TEdge> g,
                bool stronglyConnected = true)
                where TVertex : notnull
                where TEdge : IEdge<TVertex>
                where TGraph : IMutableVertexAndEdgeSet<TVertex, TEdge>, new()
            {
                AssertAlgorithmState(algo, g);
                algo.StronglyConnected.Should().Be(stronglyConnected);
                algo.CondensedGraph.Should().BeNull();
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var components = new Dictionary<int, int>();

            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new WeaklyConnectedComponentsAlgorithm<int, Edge<int>>(default)).Should().Throw<ArgumentNullException>();

            Invoking(() => new WeaklyConnectedComponentsAlgorithm<int, Edge<int>>(graph, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new WeaklyConnectedComponentsAlgorithm<int, Edge<int>>(default, components)).Should().Throw<ArgumentNullException>();
            Invoking(() => new WeaklyConnectedComponentsAlgorithm<int, Edge<int>>(default, default)).Should().Throw<ArgumentNullException>();

            Invoking(() => new WeaklyConnectedComponentsAlgorithm<int, Edge<int>>(default, graph, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new WeaklyConnectedComponentsAlgorithm<int, Edge<int>>(default, default, components)).Should().Throw<ArgumentNullException>();
            Invoking(() => new WeaklyConnectedComponentsAlgorithm<int, Edge<int>>(default, default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void OneWeaklyConnectedComponent()
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge23 = new Edge<int>(2, 3);
            var edge42 = new Edge<int>(4, 2);
            var edge43 = new Edge<int>(4, 3);

            var edge45 = new Edge<int>(4, 5);

            var edge56 = new Edge<int>(5, 6);
            var edge57 = new Edge<int>(5, 7);
            var edge76 = new Edge<int>(7, 6);

            var edge71 = new Edge<int>(7, 1);

            var edge89 = new Edge<int>(8, 9);

            var edge82 = new Edge<int>(8, 2);

            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge13, edge23, edge42, edge43, edge45,
                edge56, edge57, edge76, edge71, edge89, edge82
            });

            IMutableBidirectionalGraph<AdjacencyGraph<int, Edge<int>>, CondensedEdge<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>> condensedGraph =
                graph.CondensateWeaklyConnected<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>();

            condensedGraph.Should().NotBeNull();
            condensedGraph.VertexCount.Should().Be(1);
            condensedGraph.EdgeCount.Should().Be(0);
            condensedGraph.Vertices.ElementAt(0).Vertices.Should().BeEquivalentTo(graph.Vertices);
            condensedGraph.Vertices.ElementAt(0).Edges.Should().BeEquivalentTo(graph.Edges);
        }

        [Test]
        public void MultipleWeaklyConnectedComponents()
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge23 = new Edge<int>(2, 3);
            var edge42 = new Edge<int>(4, 2);
            var edge43 = new Edge<int>(4, 3);

            var edge56 = new Edge<int>(5, 6);
            var edge57 = new Edge<int>(5, 7);
            var edge76 = new Edge<int>(7, 6);

            var edge89 = new Edge<int>(8, 9);

            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge13, edge23, edge42, edge43,
                edge56, edge57, edge76, edge89
            });

            IMutableBidirectionalGraph<AdjacencyGraph<int, Edge<int>>, CondensedEdge<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>> condensedGraph =
                graph.CondensateWeaklyConnected<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>();

            condensedGraph.Should().NotBeNull();
            condensedGraph.VertexCount.Should().Be(3);
            condensedGraph.EdgeCount.Should().Be(0);
            condensedGraph.Vertices.ElementAt(0).Vertices.Should().BeEquivalentTo(new[] { 1, 2, 3, 4 });
            condensedGraph.Vertices.ElementAt(0).Edges.Should().BeEquivalentTo(new[] { edge12, edge13, edge23, edge42, edge43 });

            condensedGraph.Vertices.ElementAt(1).Vertices.Should().BeEquivalentTo(new[] { 5, 6, 7 });
            condensedGraph.Vertices.ElementAt(1).Edges.Should().BeEquivalentTo(new[] { edge56, edge57, edge76 });

            condensedGraph.Vertices.ElementAt(2).Vertices.Should().BeEquivalentTo(new[] { 8, 9 });
            condensedGraph.Vertices.ElementAt(2).Edges.Should().BeEquivalentTo(new[] { edge89 });
        }

        [TestCaseSource(nameof(AdjacencyGraphs_All))]
        [Category(TestCategories.LongRunning)]
        public void WeaklyConnectedCondensation(TestGraphInstance<AdjacencyGraph<string, Edge<string>>, string> testGraph)
        {
            RunWeaklyConnectedCondensationAndCheck(testGraph.Instance);
        }

        private static readonly IEnumerable<TestCaseData> AdjacencyGraphs_All =
            TestGraphFactory
                .SampleAdjacencyGraphs()
                .Select(t => new TestCaseData(t) { TestName = t.DescribeForTestCase() })
                .Memoize();
    }
}
