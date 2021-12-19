#nullable enable

using NUnit.Framework;
using FastGraph.Algorithms;
using FastGraph.Algorithms.ShortestPath;
using static FastGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace FastGraph.Tests.Algorithms.ShortestPath
{
    /// <summary>
    /// Tests for <see cref="FloydWarshallAllShortestPathAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class FloydWarshallAllShortestPathAlgorithmTests : FloydWarshallTestsBase
    {
        [Test]
        public void Constructor()
        {
            Func<Edge<int>, double> Weights = _ => 1.0;

            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(graph, Weights);
            AssertAlgorithmState(algorithm, graph);

            algorithm = new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(graph, Weights, DistanceRelaxers.CriticalDistance);
            AssertAlgorithmState(algorithm, graph);

            algorithm = new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(default, graph, Weights, DistanceRelaxers.CriticalDistance);
            AssertAlgorithmState(algorithm, graph);
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            var graph = new AdjacencyGraph<int, Edge<int>>();

            Func<Edge<int>, double> Weights = _ => 1.0;

#pragma warning disable CS8625
            Invoking(() => new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(default, Weights)).Should().Throw<ArgumentNullException>();
            Invoking(() => new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(graph, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(default, default)).Should().Throw<ArgumentNullException>();

            Invoking(() => new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(default, Weights, DistanceRelaxers.CriticalDistance)).Should().Throw<ArgumentNullException>();
            Invoking(() => new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(graph, default, DistanceRelaxers.CriticalDistance)).Should().Throw<ArgumentNullException>();
            Invoking(() => new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(graph, Weights, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(default, default, DistanceRelaxers.CriticalDistance)).Should().Throw<ArgumentNullException>();
            Invoking(() => new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(default, Weights, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(graph, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(default, default, default)).Should().Throw<ArgumentNullException>();

            Invoking(() => new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(default, default, Weights, DistanceRelaxers.CriticalDistance)).Should().Throw<ArgumentNullException>();
            Invoking(() => new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(default, graph, default, DistanceRelaxers.CriticalDistance)).Should().Throw<ArgumentNullException>();
            Invoking(() => new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(default, graph, Weights, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(default, default, default, DistanceRelaxers.CriticalDistance)).Should().Throw<ArgumentNullException>();
            Invoking(() => new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(default, default, Weights, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(default, graph, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(default, default, default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void TryGetDistance()
        {
            const int vertex1 = 1;
            const int vertex2 = 2;
            const int vertex3 = 3;

            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdge(new Edge<int>(vertex1, vertex2));
            graph.AddVertex(vertex3);

            var algorithm = new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0);

            algorithm.TryGetDistance(vertex1, vertex2, out _).Should().BeFalse();
            algorithm.TryGetDistance(vertex1, vertex3, out _).Should().BeFalse();

            algorithm.Compute();

            algorithm.TryGetDistance(vertex1, vertex2, out double distance).Should().BeTrue();
            distance.Should().Be(1);

            algorithm.TryGetDistance(vertex1, vertex3, out _).Should().BeFalse();
        }

        [Test]
        public void TryGetDistance_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var algorithm = new FloydWarshallAllShortestPathAlgorithm<TestVertex, Edge<TestVertex>>(graph, _ => 1.0);

            var vertex = new TestVertex();
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => algorithm.TryGetDistance(vertex, default, out _)).Should().Throw<ArgumentNullException>();
            Invoking(() => algorithm.TryGetDistance(default, vertex, out _)).Should().Throw<ArgumentNullException>();
            Invoking(() => algorithm.TryGetDistance(default, default, out _)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
        }

        [Test]
        public void TryGetPath()
        {
            const int vertex1 = 1;
            const int vertex2 = 2;
            const int vertex3 = 3;
            const int vertex4 = 4;

            var edge12 = new Edge<int>(vertex1, vertex2);
            var edge24 = new Edge<int>(vertex2, vertex4);

            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdge(edge12);
            graph.AddVerticesAndEdge(edge24);
            graph.AddVertex(vertex3);

            var algorithm = new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0);

            algorithm.TryGetPath(vertex1, vertex1, out _).Should().BeFalse();
            algorithm.TryGetPath(vertex1, vertex2, out _).Should().BeFalse();
            algorithm.TryGetPath(vertex1, vertex4, out _).Should().BeFalse();
            algorithm.TryGetPath(vertex1, vertex3, out _).Should().BeFalse();

            algorithm.Compute();

            algorithm.TryGetPath(vertex1, vertex1, out _).Should().BeFalse();

            algorithm.TryGetPath(vertex1, vertex2, out IEnumerable<Edge<int>>? path).Should().BeTrue();
            new[] { edge12 }.Should().BeEquivalentTo(path);

            algorithm.TryGetPath(vertex1, vertex4, out path).Should().BeTrue();
            new[] { edge12, edge24 }.Should().BeEquivalentTo(path);

            algorithm.TryGetPath(vertex1, vertex3, out _).Should().BeFalse();
        }

        [Test]
        public void TryGetPath_Throws()
        {
            var graph1 = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var algorithm1 = new FloydWarshallAllShortestPathAlgorithm<TestVertex, Edge<TestVertex>>(graph1, _ => 1.0);

            var vertex = new TestVertex();
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => algorithm1.TryGetPath(vertex, default, out _)).Should().Throw<ArgumentNullException>();
            Invoking(() => algorithm1.TryGetPath(default, vertex, out _)).Should().Throw<ArgumentNullException>();
            Invoking(() => algorithm1.TryGetPath(default, default, out _)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
        }

        [Test]
        public void FloydWarshallSimpleGraph()
        {
            var distances = new Dictionary<Edge<char>, double>();
            AdjacencyGraph<char, Edge<char>> graph = CreateGraph(distances);
            var algorithm = new FloydWarshallAllShortestPathAlgorithm<char, Edge<char>>(graph, e => distances[e]);
            algorithm.Compute();

            algorithm.TryGetDistance('A', 'A', out double distance).Should().BeTrue();
            distance.Should().Be(0);

            algorithm.TryGetDistance('A', 'B', out distance).Should().BeTrue();
            distance.Should().Be(6);

            algorithm.TryGetDistance('A', 'C', out distance).Should().BeTrue();
            distance.Should().Be(1);

            algorithm.TryGetDistance('A', 'D', out distance).Should().BeTrue();
            distance.Should().Be(4);

            algorithm.TryGetDistance('A', 'E', out distance).Should().BeTrue();
            distance.Should().Be(5);
        }

        [Test]
        public void FloydWarshall_Throws()
        {
            // Without negative cycle
            var edge12 = new Edge<int>(1, 2);
            var edge23 = new Edge<int>(2, 3);
            var edge34 = new Edge<int>(3, 4);

            var negativeWeightGraph = new AdjacencyGraph<int, Edge<int>>();
            negativeWeightGraph.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge23, edge34
            });

            var algorithm = new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(
                negativeWeightGraph,
                e =>
                {
                    if (e == edge12)
                        return 12.0;
                    if (e == edge23)
                        return -23.0;
                    if (e == edge34)
                        return -34.0;
                    return 1.0;
                });
            Invoking(() => algorithm.Compute()).Should().NotThrow();

            // With negative cycle
            var edge41 = new Edge<int>(4, 1);

            var negativeCycleGraph = new AdjacencyGraph<int, Edge<int>>();
            negativeCycleGraph.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge23, edge34, edge41
            });

            algorithm = new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(
                negativeCycleGraph,
                e =>
                {
                    if (e == edge12)
                        return 12.0;
                    if (e == edge23)
                        return -23.0;
                    if (e == edge34)
                        return -34.0;
                    if (e == edge41)
                        return 41.0;
                    return 1.0;
                });
            Invoking(() => algorithm.Compute()).Should().Throw<NegativeCycleGraphException>();
        }
    }
}
