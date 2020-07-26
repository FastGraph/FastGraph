using System;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms.TopologicalSort;
using static QuikGraph.Tests.Algorithms.AlgorithmTestHelpers;
using static QuikGraph.Tests.QuikGraphUnitTestsHelpers;

namespace QuikGraph.Tests.Algorithms
{
    /// <summary>
    /// Tests for <see cref="SourceFirstBidirectionalTopologicalSortAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class SourceFirstBidirectionalTopologicalSortAlgorithmTests
    {
        #region Test helpers

        private static void RunSourceFirstTopologicalSortAndCheck<TVertex, TEdge>(
            [NotNull] IBidirectionalGraph<TVertex, TEdge> graph,
            TopologicalSortDirection direction)
            where TEdge : IEdge<TVertex>
        {
            var algorithm = new SourceFirstBidirectionalTopologicalSortAlgorithm<TVertex, TEdge>(graph, direction);
            algorithm.Compute();

            Assert.IsNotNull(algorithm.SortedVertices);
            Assert.AreEqual(graph.VertexCount, algorithm.SortedVertices.Length);
            Assert.IsNotNull(algorithm.InDegrees);
            Assert.AreEqual(graph.VertexCount, algorithm.InDegrees.Count);
        }

        #endregion

        [Test]
        public void Constructor()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            var algorithm = new SourceFirstBidirectionalTopologicalSortAlgorithm<int, Edge<int>>(graph);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new SourceFirstBidirectionalTopologicalSortAlgorithm<int, Edge<int>>(graph, -10);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new SourceFirstBidirectionalTopologicalSortAlgorithm<int, Edge<int>>(graph, 0);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new SourceFirstBidirectionalTopologicalSortAlgorithm<int, Edge<int>>(graph, 10);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new SourceFirstBidirectionalTopologicalSortAlgorithm<int, Edge<int>>(graph, TopologicalSortDirection.Forward);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new SourceFirstBidirectionalTopologicalSortAlgorithm<int, Edge<int>>(graph, TopologicalSortDirection.Forward, -10);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new SourceFirstBidirectionalTopologicalSortAlgorithm<int, Edge<int>>(graph, TopologicalSortDirection.Forward, 0);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new SourceFirstBidirectionalTopologicalSortAlgorithm<int, Edge<int>>(graph, TopologicalSortDirection.Forward, 10);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new SourceFirstBidirectionalTopologicalSortAlgorithm<int, Edge<int>>(graph, TopologicalSortDirection.Backward);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new SourceFirstBidirectionalTopologicalSortAlgorithm<int, Edge<int>>(graph, TopologicalSortDirection.Backward, -10);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new SourceFirstBidirectionalTopologicalSortAlgorithm<int, Edge<int>>(graph, TopologicalSortDirection.Backward, 0);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new SourceFirstBidirectionalTopologicalSortAlgorithm<int, Edge<int>>(graph, TopologicalSortDirection.Backward, 10);
            AssertAlgorithmProperties(algorithm, graph);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                SourceFirstBidirectionalTopologicalSortAlgorithm<TVertex, TEdge> algo,
                IBidirectionalGraph<TVertex, TEdge> g)
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                Assert.IsNull(algo.SortedVertices);
                CollectionAssert.IsEmpty(algo.InDegrees);
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => new SourceFirstBidirectionalTopologicalSortAlgorithm<int, Edge<int>>(null));
            Assert.Throws<ArgumentNullException>(
                () => new SourceFirstBidirectionalTopologicalSortAlgorithm<int, Edge<int>>(null, TopologicalSortDirection.Forward));
            Assert.Throws<ArgumentNullException>(
                () => new SourceFirstBidirectionalTopologicalSortAlgorithm<int, Edge<int>>(null, TopologicalSortDirection.Backward));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void SimpleGraph()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(2, 3),
                new Edge<int>(2, 6),
                new Edge<int>(2, 8),
                new Edge<int>(4, 2),
                new Edge<int>(4, 5),
                new Edge<int>(5, 6),
                new Edge<int>(7, 5),
                new Edge<int>(7, 8)
            });

            var algorithm = new SourceFirstBidirectionalTopologicalSortAlgorithm<int, Edge<int>>(graph);
            algorithm.Compute();

            CollectionAssert.AreEqual(
                new[] { 1, 7, 4, 2, 5, 8, 3, 6 },
                algorithm.SortedVertices);

            algorithm = new SourceFirstBidirectionalTopologicalSortAlgorithm<int, Edge<int>>(graph, TopologicalSortDirection.Backward);
            algorithm.Compute();

            CollectionAssert.AreEqual(
                new[] { 3, 6, 8, 5, 2, 7, 1, 4 },
                algorithm.SortedVertices);
        }

        [Test]
        public void SimpleGraphOneToAnother()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(0, 1),
                new Edge<int>(1, 2),
                new Edge<int>(1, 3),
                new Edge<int>(2, 3),
                new Edge<int>(3, 4)
            });

            var algorithm = new SourceFirstBidirectionalTopologicalSortAlgorithm<int, Edge<int>>(graph);
            algorithm.Compute();

            CollectionAssert.AreEqual(
                new[] { 0, 1, 2, 3, 4 },
                algorithm.SortedVertices);

            algorithm = new SourceFirstBidirectionalTopologicalSortAlgorithm<int, Edge<int>>(graph, TopologicalSortDirection.Backward);
            algorithm.Compute();

            CollectionAssert.AreEqual(
                new[] { 4, 3, 2, 1, 0 },
                algorithm.SortedVertices);
        }

        [Test]
        public void ForestGraph()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(0, 1),
                new Edge<int>(1, 2),
                new Edge<int>(1, 3),
                new Edge<int>(2, 3),
                new Edge<int>(3, 4),

                new Edge<int>(5, 6)
            });

            var algorithm = new SourceFirstBidirectionalTopologicalSortAlgorithm<int, Edge<int>>(graph);
            algorithm.Compute();

            CollectionAssert.AreEqual(
                new[] { 0, 5, 1, 6, 2, 3, 4 },
                algorithm.SortedVertices);

            algorithm = new SourceFirstBidirectionalTopologicalSortAlgorithm<int, Edge<int>>(graph, TopologicalSortDirection.Backward);
            algorithm.Compute();

            CollectionAssert.AreEqual(
                new[] { 4, 6, 3, 5, 2, 1, 0 },
                algorithm.SortedVertices);
        }

        [Test]
        public void GraphWithSelfEdge_Throws()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(0, 1),
                new Edge<int>(1, 2),
                new Edge<int>(1, 3),
                new Edge<int>(2, 3),
                new Edge<int>(2, 2),
                new Edge<int>(3, 4)
            });

            var algorithm = new SourceFirstBidirectionalTopologicalSortAlgorithm<int, Edge<int>>(graph);
            Assert.Throws<NonAcyclicGraphException>(() => algorithm.Compute());

            algorithm = new SourceFirstBidirectionalTopologicalSortAlgorithm<int, Edge<int>>(graph, TopologicalSortDirection.Backward);
            Assert.Throws<NonAcyclicGraphException>(() => algorithm.Compute());
        }

        [Test]
        public void SourceFirstBidirectionalTopologicalSort()
        {
            foreach (BidirectionalGraph<string, Edge<string>> graph in TestGraphFactory.GetBidirectionalGraphs_All())
            {
                RunSourceFirstTopologicalSortAndCheck(graph, TopologicalSortDirection.Forward);
                RunSourceFirstTopologicalSortAndCheck(graph, TopologicalSortDirection.Backward);
            }
        }

        [Test]
        public void SourceFirstBidirectionalTopologicalSort_DCT8()
        {
            BidirectionalGraph<string, Edge<string>> graph = TestGraphFactory.LoadBidirectionalGraph(GetGraphFilePath("DCT8.graphml"));
            RunSourceFirstTopologicalSortAndCheck(graph, TopologicalSortDirection.Forward);
            RunSourceFirstTopologicalSortAndCheck(graph, TopologicalSortDirection.Backward);
        }

        [Test]
        public void SourceFirstBidirectionalTopologicalSort_Throws()
        {
            var cyclicGraph = new BidirectionalGraph<int, Edge<int>>();
            cyclicGraph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(2, 3),
                new Edge<int>(1, 4),
                new Edge<int>(3, 1)
            });

            var algorithm = new SourceFirstBidirectionalTopologicalSortAlgorithm<int, Edge<int>>(cyclicGraph);
            Assert.Throws<NonAcyclicGraphException>(() => algorithm.Compute());
        }
    }
}