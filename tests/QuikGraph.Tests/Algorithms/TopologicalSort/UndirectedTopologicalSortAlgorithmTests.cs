using System;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms.TopologicalSort;
using static QuikGraph.Tests.Algorithms.AlgorithmTestHelpers;
using static QuikGraph.Tests.QuikGraphUnitTestsHelpers;


namespace QuikGraph.Tests.Algorithms
{
    /// <summary>
    /// Tests for <see cref="UndirectedTopologicalSortAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class UndirectedTopologicalSortAlgorithmTests
    {
        #region Test helpers

        private static void RunUndirectedTopologicalSortAndCheck<TVertex, TEdge>(
            [NotNull] IUndirectedGraph<TVertex, TEdge> graph,
            bool allowCycles)
            where TEdge : IEdge<TVertex>
        {
            var algorithm = new UndirectedTopologicalSortAlgorithm<TVertex, TEdge>(graph)
            {
                AllowCyclicGraph = allowCycles
            };

            algorithm.Compute();

            Assert.IsNotNull(algorithm.SortedVertices);
            Assert.AreEqual(graph.VertexCount, algorithm.SortedVertices.Length);
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
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                Assert.IsNull(algo.SortedVertices);
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
                () => new UndirectedTopologicalSortAlgorithm<int, Edge<int>>(null));
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
            CollectionAssert.AreEqual(
                new[] { 1, 2, 4, 5, 7, 8, 6, 3 },
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

            var algorithm = new UndirectedTopologicalSortAlgorithm<int, Edge<int>>(graph);
            algorithm.Compute();

            // Order in undirected graph is some strange thing,
            // here the order is more vertices ordered by depth
            CollectionAssert.AreEqual(
                new[] { 0, 1, 3, 4, 2 },
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

            var algorithm = new UndirectedTopologicalSortAlgorithm<int, Edge<int>>(graph);
            algorithm.Compute();

            // Order in undirected graph is some strange thing,
            // here the order is more vertices ordered by depth
            CollectionAssert.AreEqual(
                new[] { 5, 6, 0, 1, 3, 4, 2 },
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

            var algorithm = new UndirectedTopologicalSortAlgorithm<int, Edge<int>>(graph);
            Assert.Throws<NonAcyclicGraphException>(() => algorithm.Compute());

            algorithm = new UndirectedTopologicalSortAlgorithm<int, Edge<int>>(graph)
            {
                AllowCyclicGraph = true
            };
            algorithm.Compute();

            // Order in undirected graph is some strange thing,
            // here the order is more vertices ordered by depth
            CollectionAssert.AreEqual(
                new[] { 0, 1, 2, 3, 4 },
                algorithm.SortedVertices);
        }

        [Test]
        public void UndirectedTopologicalSort()
        {
            foreach (UndirectedGraph<string, Edge<string>> graph in TestGraphFactory.GetUndirectedGraphs_All())
            {
                RunUndirectedTopologicalSortAndCheck(graph, true);
            }
        }

        [Test]
        public void UndirectedTopologicalSort_DCT8()
        {
            UndirectedGraph<string, Edge<string>> graph = TestGraphFactory.LoadUndirectedGraph(GetGraphFilePath("DCT8.graphml"));
            RunUndirectedTopologicalSortAndCheck(graph, true);
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
            Assert.Throws<NonAcyclicGraphException>(() => algorithm.Compute());

            algorithm = new UndirectedTopologicalSortAlgorithm<int, Edge<int>>(cyclicGraph)
            {
                AllowCyclicGraph = true
            };
            Assert.DoesNotThrow(() => algorithm.Compute());
        }
    }
}