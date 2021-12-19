#nullable enable

using NUnit.Framework;
using FastGraph.Algorithms.ConnectedComponents;
using static FastGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace FastGraph.Tests.Algorithms.ConnectedComponents
{
    /// <summary>
    /// Tests for <see cref="StronglyConnectedComponentsAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class StronglyConnectedComponentsAlgorithmTests
    {
        #region Test helpers

        public void RunStronglyConnectedComponentsAndCheck<TVertex, TEdge>(IVertexListGraph<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            var algorithm = new StronglyConnectedComponentsAlgorithm<TVertex, TEdge>(graph);
            algorithm.Compute();

            algorithm.Components.Count.Should().Be(graph.VertexCount);
            algorithm.Roots.Count.Should().Be(graph.VertexCount);
            algorithm.DiscoverTimes.Count.Should().Be(graph.VertexCount);
            if (graph.VertexCount == 0)
            {
                algorithm.Steps.Should().Be(0);
                (algorithm.ComponentCount == 0).Should().BeTrue();
                return;
            }

            algorithm.ComponentCount.Should().BePositive();
            algorithm.ComponentCount.Should().BeLessThanOrEqualTo(graph.VertexCount);
            foreach (TVertex vertex in algorithm.VisitedGraph.Vertices)
            {
                algorithm.Components.ContainsKey(vertex).Should().BeTrue();
                algorithm.DiscoverTimes.ContainsKey(vertex).Should().BeTrue();
            }

            algorithm.Steps.Should().BePositive();
            AssertStepsProperties();
            foreach (KeyValuePair<TVertex, int> pair in algorithm.Components)
            {
                pair.Value.Should().BeGreaterThanOrEqualTo(0);
                (pair.Value < algorithm.ComponentCount).Should().BeTrue();
            }

            foreach (KeyValuePair<TVertex, int> time in algorithm.DiscoverTimes)
            {
                time.Key.Should().NotBeNull();
            }

            foreach (TVertex vertex in graph.Vertices)
            {
                algorithm.Components[vertex].Should().BeGreaterThanOrEqualTo(0);
            }

            #region Local function

            void AssertStepsProperties()
            {
                algorithm.Steps.Should().BeGreaterThanOrEqualTo(0);
                algorithm.VerticesPerStep!.Count.Should().Be(algorithm.Steps);
                algorithm.ComponentsPerStep!.Count.Should().Be(algorithm.Steps);
            }

            #endregion
        }

        #endregion

        [Test]
        public void Constructor()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var components = new Dictionary<int, int>();
            var algorithm = new StronglyConnectedComponentsAlgorithm<int, Edge<int>>(graph);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new StronglyConnectedComponentsAlgorithm<int, Edge<int>>(graph, components);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new StronglyConnectedComponentsAlgorithm<int, Edge<int>>(default, graph, components);
            AssertAlgorithmProperties(algorithm, graph);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                StronglyConnectedComponentsAlgorithm<TVertex, TEdge> algo,
                IVertexListGraph<TVertex, TEdge> g)
                where TVertex : notnull
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                algo.ComponentCount.Should().Be(0);
                algo.Components.Should().BeEmpty();
                algo.Graphs.Should().BeEmpty();
                algo.Roots.Should().BeEmpty();

                algo.Steps.Should().Be(0);
                algo.VerticesPerStep.Should().BeNull();
                algo.ComponentsPerStep.Should().BeNull();
                algo.DiscoverTimes.Should().BeEmpty();
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
            Invoking(() => new StronglyConnectedComponentsAlgorithm<int, Edge<int>>(default)).Should().Throw<ArgumentNullException>();

            Invoking(() => new StronglyConnectedComponentsAlgorithm<int, Edge<int>>(graph, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new StronglyConnectedComponentsAlgorithm<int, Edge<int>>(default, components)).Should().Throw<ArgumentNullException>();
            Invoking(() => new StronglyConnectedComponentsAlgorithm<int, Edge<int>>(default, default)).Should().Throw<ArgumentNullException>();

            Invoking(() => new StronglyConnectedComponentsAlgorithm<int, Edge<int>>(default, graph, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new StronglyConnectedComponentsAlgorithm<int, Edge<int>>(default, default, components)).Should().Throw<ArgumentNullException>();
            Invoking(() => new StronglyConnectedComponentsAlgorithm<int, Edge<int>>(default, default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void OneComponent()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(2, 3),
                new Edge<int>(3, 1)
            });

            var algorithm = new StronglyConnectedComponentsAlgorithm<int, Edge<int>>(graph);
            algorithm.Compute();

            algorithm.ComponentCount.Should().Be(1);
            algorithm.Components.Should().BeEquivalentTo(new Dictionary<int, int>
            {
                [1] = 0,
                [2] = 0,
                [3] = 0
            });
            algorithm.Graphs.Length.Should().Be(1);
            algorithm.Graphs[0].Vertices.Should().BeEquivalentTo(new[] { 1, 2, 3 });
        }

        [Test]
        public void ThreeComponents()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(2, 3),
                new Edge<int>(2, 4),
                new Edge<int>(3, 1),
                new Edge<int>(4, 5)
            });

            var algorithm = new StronglyConnectedComponentsAlgorithm<int, Edge<int>>(graph);
            algorithm.Compute();

            algorithm.ComponentCount.Should().Be(3);
            algorithm.Components.Should().BeEquivalentTo(new Dictionary<int, int>
            {
                [1] = 2,
                [2] = 2,
                [3] = 2,
                [4] = 1,
                [5] = 0
            });
            algorithm.Graphs.Length.Should().Be(3);
            algorithm.Graphs[0].Vertices.Should().BeEquivalentTo(new[] { 5 });
            algorithm.Graphs[1].Vertices.Should().BeEquivalentTo(new[] { 4 });
            algorithm.Graphs[2].Vertices.Should().BeEquivalentTo(new[] { 1, 2, 3 });
        }

        [Test]
        public void MultipleComponents()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(2, 3),
                new Edge<int>(2, 4),
                new Edge<int>(2, 5),
                new Edge<int>(3, 1),
                new Edge<int>(3, 4),
                new Edge<int>(4, 6),
                new Edge<int>(5, 6),
                new Edge<int>(5, 7),
                new Edge<int>(6, 4),
                new Edge<int>(7, 5),
                new Edge<int>(7, 8),
                new Edge<int>(8, 6),
                new Edge<int>(8, 7)
            });
            graph.AddVertex(10);

            var algorithm = new StronglyConnectedComponentsAlgorithm<int, Edge<int>>(graph);
            algorithm.Compute();

            algorithm.ComponentCount.Should().Be(4);
            algorithm.Components.Should().BeEquivalentTo(new Dictionary<int, int>
            {
                [1] = 2,
                [2] = 2,
                [3] = 2,
                [4] = 0,
                [5] = 1,
                [6] = 0,
                [7] = 1,
                [8] = 1,
                [10] = 3
            });
            algorithm.Graphs.Length.Should().Be(4);
            algorithm.Graphs[0].Vertices.Should().BeEquivalentTo(new[] { 4, 6 });
            algorithm.Graphs[1].Vertices.Should().BeEquivalentTo(new[] { 5, 7, 8 });
            algorithm.Graphs[2].Vertices.Should().BeEquivalentTo(new[] { 1, 2, 3 });
            algorithm.Graphs[3].Vertices.Should().BeEquivalentTo(new[] { 10 });
        }

        [Test]
        [Category(TestCategories.LongRunning)]
        public void StronglyConnectedComponents()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs_All())
                RunStronglyConnectedComponentsAndCheck(graph);
        }
    }
}
