using System;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms.TopologicalSort;
using static QuikGraph.Tests.Algorithms.AlgorithmTestHelpers;
using static QuikGraph.Tests.QuikGraphUnitTestsHelpers;

namespace QuikGraph.Tests.Algorithms
{
    /// <summary>
    /// Tests for <see cref="SourceFirstTopologicalSortAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class SourceFirstTopologicalSortAlgorithmTests
    {
        #region Test helpers

        private static void RunSourceFirstTopologicalSortAndCheck<TVertex, TEdge>([NotNull] IVertexAndEdgeListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            var algorithm = new SourceFirstTopologicalSortAlgorithm<TVertex, TEdge>(graph);
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
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new SourceFirstTopologicalSortAlgorithm<int, Edge<int>>(graph);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new SourceFirstTopologicalSortAlgorithm<int, Edge<int>>(graph, -10);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new SourceFirstTopologicalSortAlgorithm<int, Edge<int>>(graph, 0);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new SourceFirstTopologicalSortAlgorithm<int, Edge<int>>(graph, 10);
            AssertAlgorithmProperties(algorithm, graph);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                SourceFirstTopologicalSortAlgorithm<TVertex, TEdge> algo,
                IVertexAndEdgeListGraph<TVertex, TEdge> g)
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
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => new SourceFirstTopologicalSortAlgorithm<int, Edge<int>>(null));
        }

        [Test]
        public void SimpleGraph()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
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

            var algorithm = new SourceFirstTopologicalSortAlgorithm<int, Edge<int>>(graph);
            algorithm.Compute();

            CollectionAssert.AreEqual(
                new[] { 1, 7, 4, 2, 5, 8, 3, 6 },
                algorithm.SortedVertices);
        }

        [Test]
        public void SimpleGraphOneToAnother()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(0, 1),
                new Edge<int>(1, 2),
                new Edge<int>(1, 3),
                new Edge<int>(2, 3),
                new Edge<int>(3, 4)
            });

            var algorithm = new SourceFirstTopologicalSortAlgorithm<int, Edge<int>>(graph);
            algorithm.Compute();

            CollectionAssert.AreEqual(
                new[] { 0, 1, 2, 3, 4 },
                algorithm.SortedVertices);
        }

        [Test]
        public void ForestGraph()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(0, 1),
                new Edge<int>(1, 2),
                new Edge<int>(1, 3),
                new Edge<int>(2, 3),
                new Edge<int>(3, 4),

                new Edge<int>(5, 6)
            });

            var algorithm = new SourceFirstTopologicalSortAlgorithm<int, Edge<int>>(graph);
            algorithm.Compute();

            CollectionAssert.AreEqual(
                new[] { 0, 5, 1, 6, 2, 3, 4 },
                algorithm.SortedVertices);
        }

        [Test]
        public void GraphWithSelfEdge_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(0, 1),
                new Edge<int>(1, 2),
                new Edge<int>(1, 3),
                new Edge<int>(2, 3),
                new Edge<int>(2, 2),
                new Edge<int>(3, 4)
            });

            var algorithm = new SourceFirstTopologicalSortAlgorithm<int, Edge<int>>(graph);
            Assert.Throws<NonAcyclicGraphException>(() => algorithm.Compute());
        }

        [Test]
        public void SourceFirstTopologicalSort()
        {
            foreach(AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs_All())
                RunSourceFirstTopologicalSortAndCheck(graph);
        }

        [Test]
        public void SourceFirstTopologicalSort_DCT8()
        {
            AdjacencyGraph<string, Edge<string>> graph = TestGraphFactory.LoadGraph(GetGraphFilePath("DCT8.graphml"));
            RunSourceFirstTopologicalSortAndCheck(graph);
        }

        [Test]
        public void SourceFirstTopologicalSort_Throws()
        {
            var cyclicGraph = new AdjacencyGraph<int, Edge<int>>();
            cyclicGraph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(2, 3),
                new Edge<int>(1, 4),
                new Edge<int>(3, 1)
            });

            var algorithm = new SourceFirstTopologicalSortAlgorithm<int, Edge<int>>(cyclicGraph);
            Assert.Throws<NonAcyclicGraphException>(() => algorithm.Compute());
        }
    }
}