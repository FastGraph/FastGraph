#nullable enable

using NUnit.Framework;
using FastGraph.Algorithms.ConnectedComponents;
using static FastGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace FastGraph.Tests.Algorithms.ConnectedComponents
{
    /// <summary>
    /// Tests for <see cref="ConnectedComponentsAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class ConnectedComponentsAlgorithmTests
    {
        #region Test helpers

        private static void RunConnectedComponentsAndCheck<TVertex, TEdge>(
            IUndirectedGraph<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            var algorithm = new ConnectedComponentsAlgorithm<TVertex, TEdge>(graph);
            algorithm.Compute();

            Assert.AreEqual(graph.VertexCount, algorithm.Components.Count);
            if (graph.VertexCount == 0)
            {
                Assert.IsTrue(algorithm.ComponentCount == 0);
                return;
            }

            Assert.Positive(algorithm.ComponentCount);
            Assert.LessOrEqual(algorithm.ComponentCount, graph.VertexCount);
            foreach (KeyValuePair<TVertex, int> pair in algorithm.Components)
            {
                Assert.GreaterOrEqual(pair.Value, 0);
                Assert.IsTrue(pair.Value < algorithm.ComponentCount, $"{pair.Value} < {algorithm.ComponentCount}");
            }

            foreach (TVertex vertex in graph.Vertices)
            {
                foreach (TEdge edge in graph.AdjacentEdges(vertex))
                {
                    Assert.AreEqual(algorithm.Components[edge.Source], algorithm.Components[edge.Target]);
                }
            }
        }

        #endregion

        [Test]
        public void Constructor()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            var components = new Dictionary<int, int>();
            var algorithm = new ConnectedComponentsAlgorithm<int, Edge<int>>(graph);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new ConnectedComponentsAlgorithm<int, Edge<int>>(graph, components);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new ConnectedComponentsAlgorithm<int, Edge<int>>(default, graph, components);
            AssertAlgorithmProperties(algorithm, graph);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                ConnectedComponentsAlgorithm<TVertex, TEdge> algo,
                IUndirectedGraph<TVertex, TEdge> g)
                where TVertex : notnull
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                Assert.AreEqual(0, algo.ComponentCount);
                CollectionAssert.IsEmpty(algo.Components);
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            var components = new Dictionary<int, int>();

            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Assert.Throws<ArgumentNullException>(
                () => new ConnectedComponentsAlgorithm<int, Edge<int>>(default));

            Assert.Throws<ArgumentNullException>(
                () => new ConnectedComponentsAlgorithm<int, Edge<int>>(graph, default));
            Assert.Throws<ArgumentNullException>(
                () => new ConnectedComponentsAlgorithm<int, Edge<int>>(default, components));
            Assert.Throws<ArgumentNullException>(
                () => new ConnectedComponentsAlgorithm<int, Edge<int>>(default, default));

            Assert.Throws<ArgumentNullException>(
                () => new ConnectedComponentsAlgorithm<int, Edge<int>>(default, graph, default));
            Assert.Throws<ArgumentNullException>(
                () => new ConnectedComponentsAlgorithm<int, Edge<int>>(default, default, components));
            Assert.Throws<ArgumentNullException>(
                () => new ConnectedComponentsAlgorithm<int, Edge<int>>(default, default, default));
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void OneComponent()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(1, 3),
                new Edge<int>(2, 3),
                new Edge<int>(4, 2),
                new Edge<int>(4, 3)
            });

            var algorithm = new ConnectedComponentsAlgorithm<int, Edge<int>>(graph);
            algorithm.Compute();

            Assert.AreEqual(1, algorithm.ComponentCount);
            CollectionAssert.AreEquivalent(
                new Dictionary<int, int>
                {
                    [1] = 0,
                    [2] = 0,
                    [3] = 0,
                    [4] = 0
                },
                algorithm.Components);
        }

        [Test]
        public void TwoComponents()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(1, 3),
                new Edge<int>(2, 3),
                new Edge<int>(4, 2),
                new Edge<int>(4, 3),

                new Edge<int>(5, 6),
                new Edge<int>(5, 7),
                new Edge<int>(7, 6)
            });

            var algorithm = new ConnectedComponentsAlgorithm<int, Edge<int>>(graph);
            algorithm.Compute();

            Assert.AreEqual(2, algorithm.ComponentCount);
            CollectionAssert.AreEquivalent(
                new Dictionary<int, int>
                {
                    [1] = 0,
                    [2] = 0,
                    [3] = 0,
                    [4] = 0,
                    [5] = 1,
                    [6] = 1,
                    [7] = 1
                },
                algorithm.Components);
        }

        [Test]
        public void MultipleComponents()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(1, 3),
                new Edge<int>(2, 3),
                new Edge<int>(4, 2),
                new Edge<int>(4, 3),

                new Edge<int>(5, 6),
                new Edge<int>(5, 7),
                new Edge<int>(7, 6),

                new Edge<int>(8, 9)
            });
            graph.AddVertex(10);

            var algorithm = new ConnectedComponentsAlgorithm<int, Edge<int>>(graph);
            algorithm.Compute();

            Assert.AreEqual(4, algorithm.ComponentCount);
            CollectionAssert.AreEquivalent(
                new Dictionary<int, int>
                {
                    [1] = 0,
                    [2] = 0,
                    [3] = 0,
                    [4] = 0,
                    [5] = 1,
                    [6] = 1,
                    [7] = 1,
                    [8] = 2,
                    [9] = 2,
                    [10] = 3
                },
                algorithm.Components);
        }

        [Test]
        [Category(TestCategories.LongRunning)]
        public void ConnectedComponents()
        {
            foreach (UndirectedGraph<string, Edge<string>> graph in TestGraphFactory.GetUndirectedGraphs_SlowTests(10))
            {
                while (graph.EdgeCount > 0)
                {
                    RunConnectedComponentsAndCheck(graph);
                    graph.RemoveEdge(graph.Edges.First());
                }
            }
        }
    }
}
