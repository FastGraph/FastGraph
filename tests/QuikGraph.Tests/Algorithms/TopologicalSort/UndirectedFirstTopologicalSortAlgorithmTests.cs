using System;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms.TopologicalSort;
using static QuikGraph.Tests.Algorithms.AlgorithmTestHelpers;
using static QuikGraph.Tests.QuikGraphUnitTestsHelpers;


namespace QuikGraph.Tests.Algorithms
{
    /// <summary>
    /// Tests for <see cref="UndirectedFirstTopologicalSortAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class UndirectedFirstTopologicalSortAlgorithmTests
    {
        #region Test helpers

        private static void RunUndirectedFirstTopologicalSortAndCheck<TVertex, TEdge>(
            [NotNull] IUndirectedGraph<TVertex, TEdge> graph,
            bool allowCycles)
            where TEdge : IEdge<TVertex>
        {
            var algorithm = new UndirectedFirstTopologicalSortAlgorithm<TVertex, TEdge>(graph)
            {
                AllowCyclicGraph = allowCycles
            };

            algorithm.Compute();

            Assert.IsNotNull(algorithm.SortedVertices);
            Assert.AreEqual(graph.VertexCount, algorithm.SortedVertices.Length);
            Assert.IsNotNull(algorithm.Degrees);
            Assert.AreEqual(graph.VertexCount, algorithm.Degrees.Count);
        }

        #endregion

        [Test]
        public void Constructor()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            var algorithm = new UndirectedFirstTopologicalSortAlgorithm<int, Edge<int>>(graph);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new UndirectedFirstTopologicalSortAlgorithm<int, Edge<int>>(graph)
            {
                AllowCyclicGraph = true
            };
            AssertAlgorithmProperties(algorithm, graph, true);

            algorithm = new UndirectedFirstTopologicalSortAlgorithm<int, Edge<int>>(graph, 0);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new UndirectedFirstTopologicalSortAlgorithm<int, Edge<int>>(graph, 10);
            AssertAlgorithmProperties(algorithm, graph);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                UndirectedFirstTopologicalSortAlgorithm<TVertex, TEdge> algo,
                IUndirectedGraph<TVertex, TEdge> g,
                bool allowCycles = false)
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                Assert.IsNull(algo.SortedVertices);
                CollectionAssert.IsEmpty(algo.Degrees);
                Assert.AreEqual(allowCycles, algo.AllowCyclicGraph);
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => new UndirectedFirstTopologicalSortAlgorithm<int, Edge<int>>(null));
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

            var algorithm = new UndirectedFirstTopologicalSortAlgorithm<int, Edge<int>>(graph);
            algorithm.Compute();

            // Order in undirected graph is some strange thing, here the order
            // is more vertices ordered from lower to higher adjacent vertices
            CollectionAssert.AreEqual(
                new[] { 1, 8, 3, 7, 2, 6, 4, 5 },
                algorithm.SortedVertices);
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

            var algorithm = new UndirectedFirstTopologicalSortAlgorithm<int, Edge<int>>(graph);
            algorithm.Compute();

            // Order in undirected graph is some strange thing, here the order
            // is more vertices ordered from lower to higher adjacent vertices
            CollectionAssert.AreEqual(
                new[] { 0, 4, 2, 3, 1 },
                algorithm.SortedVertices);
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

            var algorithm = new UndirectedFirstTopologicalSortAlgorithm<int, Edge<int>>(graph);
            algorithm.Compute();

            // Order in undirected graph is some strange thing, here the order
            // is more vertices ordered from lower to higher adjacent vertices
            CollectionAssert.AreEqual(
                new[] { 0, 6, 5, 4, 2, 3, 1 },
                algorithm.SortedVertices);
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

            var algorithm = new UndirectedFirstTopologicalSortAlgorithm<int, Edge<int>>(graph);
            Assert.Throws<NonAcyclicGraphException>(() => algorithm.Compute());

            algorithm = new UndirectedFirstTopologicalSortAlgorithm<int, Edge<int>>(graph)
            {
                AllowCyclicGraph = true
            };
            algorithm.Compute();

            // Order in undirected graph is some strange thing, here the order
            // is more vertices ordered from lower to higher adjacent vertices
            CollectionAssert.AreEqual(
                new[] { 0, 4, 1, 3, 2 },
                algorithm.SortedVertices);
        }

        [Test]
        public void UndirectedFirstTopologicalSort()
        {
            foreach (UndirectedGraph<string, Edge<string>> graph in TestGraphFactory.GetUndirectedGraphs_All())
            {
                RunUndirectedFirstTopologicalSortAndCheck(graph, true);
            }
        }

        [Test]
        public void UndirectedFirstTopologicalSort_DCT8()
        {
            UndirectedGraph<string, Edge<string>> graph = TestGraphFactory.LoadUndirectedGraph(GetGraphFilePath("DCT8.graphml"));
            RunUndirectedFirstTopologicalSortAndCheck(graph, true);
        }

        [Test]
        public void UndirectedFirstTopologicalSort_Throws()
        {
            var cyclicGraph = new UndirectedGraph<int, Edge<int>>();
            cyclicGraph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(2, 3),
                new Edge<int>(1, 4),
                new Edge<int>(3, 1)
            });

            var algorithm = new UndirectedFirstTopologicalSortAlgorithm<int, Edge<int>>(cyclicGraph);
            Assert.Throws<NonAcyclicGraphException>(() => algorithm.Compute());

            algorithm = new UndirectedFirstTopologicalSortAlgorithm<int, Edge<int>>(cyclicGraph)
            {
                AllowCyclicGraph = true
            };
            Assert.DoesNotThrow(() => algorithm.Compute());
        }
    }
}