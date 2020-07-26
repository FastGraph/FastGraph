using System;
using System.Collections.Generic;
using NUnit.Framework;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.ShortestPath;
using static QuikGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace QuikGraph.Tests.Algorithms.ShortestPath
{
    /// <summary>
    /// Tests for <see cref="FloydWarshallAllShortestPathAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class FloydWarshallAllShortestPathAlgorithmTests : FloydWarshallTestsBase
    {
        [Test]
        public void Constructor()
        {
            Func<Edge<int>, double> Weights = e => 1.0;

            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(graph, Weights);
            AssertAlgorithmState(algorithm, graph);

            algorithm = new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(graph, Weights, DistanceRelaxers.CriticalDistance);
            AssertAlgorithmState(algorithm, graph);

            algorithm = new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(null, graph, Weights, DistanceRelaxers.CriticalDistance);
            AssertAlgorithmState(algorithm, graph);
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            var graph = new AdjacencyGraph<int, Edge<int>>();

            Func<Edge<int>, double> Weights = e => 1.0;

            Assert.Throws<ArgumentNullException>(
                () => new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(null, Weights));
            Assert.Throws<ArgumentNullException>(
                () => new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(graph, null));
            Assert.Throws<ArgumentNullException>(
                () => new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(null, null));

            Assert.Throws<ArgumentNullException>(
                () => new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(null, Weights, DistanceRelaxers.CriticalDistance));
            Assert.Throws<ArgumentNullException>(
                () => new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(graph, null, DistanceRelaxers.CriticalDistance));
            Assert.Throws<ArgumentNullException>(
                () => new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(graph, Weights, null));
            Assert.Throws<ArgumentNullException>(
                () => new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(null, null, DistanceRelaxers.CriticalDistance));
            Assert.Throws<ArgumentNullException>(
                () => new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(null, Weights, null));
            Assert.Throws<ArgumentNullException>(
                () => new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(graph, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(null, null, null));

            Assert.Throws<ArgumentNullException>(
                () => new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(null, null, Weights, DistanceRelaxers.CriticalDistance));
            Assert.Throws<ArgumentNullException>(
                () => new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(null, graph, null, DistanceRelaxers.CriticalDistance));
            Assert.Throws<ArgumentNullException>(
                () => new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(null, graph, Weights, null));
            Assert.Throws<ArgumentNullException>(
                () => new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(null, null, null, DistanceRelaxers.CriticalDistance));
            Assert.Throws<ArgumentNullException>(
                () => new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(null, null, Weights, null));
            Assert.Throws<ArgumentNullException>(
                () => new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(null, graph, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(null, null, null, null));
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

            var algorithm = new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(graph, edge => 1.0);

            Assert.IsFalse(algorithm.TryGetDistance(vertex1, vertex2, out _));
            Assert.IsFalse(algorithm.TryGetDistance(vertex1, vertex3, out _));

            algorithm.Compute();

            Assert.IsTrue(algorithm.TryGetDistance(vertex1, vertex2, out double distance));
            Assert.AreEqual(1, distance);

            Assert.IsFalse(algorithm.TryGetDistance(vertex1, vertex3, out _));
        }

        [Test]
        public void TryGetDistance_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var algorithm = new FloydWarshallAllShortestPathAlgorithm<TestVertex, Edge<TestVertex>>(graph, edge => 1.0);

            var vertex = new TestVertex();
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => algorithm.TryGetDistance(vertex, null, out _));
            Assert.Throws<ArgumentNullException>(() => algorithm.TryGetDistance(null, vertex, out _));
            Assert.Throws<ArgumentNullException>(() => algorithm.TryGetDistance(null, null, out _));
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

            var algorithm = new FloydWarshallAllShortestPathAlgorithm<int, Edge<int>>(graph, edge => 1.0);

            Assert.IsFalse(algorithm.TryGetPath(vertex1, vertex1, out _));
            Assert.IsFalse(algorithm.TryGetPath(vertex1, vertex2, out _));
            Assert.IsFalse(algorithm.TryGetPath(vertex1, vertex4, out _));
            Assert.IsFalse(algorithm.TryGetPath(vertex1, vertex3, out _));

            algorithm.Compute();

            Assert.IsFalse(algorithm.TryGetPath(vertex1, vertex1, out _));

            Assert.IsTrue(algorithm.TryGetPath(vertex1, vertex2, out IEnumerable<Edge<int>> path));
            CollectionAssert.AreEqual(
                new[] { edge12 },
                path);

            Assert.IsTrue(algorithm.TryGetPath(vertex1, vertex4, out path));
            CollectionAssert.AreEqual(
                new[] { edge12, edge24 },
                path);

            Assert.IsFalse(algorithm.TryGetPath(vertex1, vertex3, out _));
        }

        [Test]
        public void TryGetPath_Throws()
        {
            var graph1 = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var algorithm1 = new FloydWarshallAllShortestPathAlgorithm<TestVertex, Edge<TestVertex>>(graph1, edge => 1.0);

            var vertex = new TestVertex();
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => algorithm1.TryGetPath(vertex, null, out _));
            Assert.Throws<ArgumentNullException>(() => algorithm1.TryGetPath(null, vertex, out _));
            Assert.Throws<ArgumentNullException>(() => algorithm1.TryGetPath(null, null, out _));
            // ReSharper restore AssignNullToNotNullAttribute
        }

        [Test]
        public void FloydWarshallSimpleGraph()
        {
            var distances = new Dictionary<Edge<char>, double>();
            AdjacencyGraph<char, Edge<char>> graph = CreateGraph(distances);
            var algorithm = new FloydWarshallAllShortestPathAlgorithm<char, Edge<char>>(graph, e => distances[e]);
            algorithm.Compute();

            Assert.IsTrue(algorithm.TryGetDistance('A', 'A', out double distance));
            Assert.AreEqual(0, distance);

            Assert.IsTrue(algorithm.TryGetDistance('A', 'B', out distance));
            Assert.AreEqual(6, distance);

            Assert.IsTrue(algorithm.TryGetDistance('A', 'C', out distance));
            Assert.AreEqual(1, distance);

            Assert.IsTrue(algorithm.TryGetDistance('A', 'D', out distance));
            Assert.AreEqual(4, distance);

            Assert.IsTrue(algorithm.TryGetDistance('A', 'E', out distance));
            Assert.AreEqual(5, distance);
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
            Assert.DoesNotThrow(() => algorithm.Compute());

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
            Assert.Throws<NegativeCycleGraphException>(() => algorithm.Compute());
        }
    }
}