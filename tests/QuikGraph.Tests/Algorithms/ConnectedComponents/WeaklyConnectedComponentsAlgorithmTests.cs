using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms.ConnectedComponents;
using static QuikGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace QuikGraph.Tests.Algorithms.ConnectedComponents
{
    /// <summary>
    /// Tests for <see cref="WeaklyConnectedComponentsAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class WeaklyConnectedComponentsAlgorithmTests
    {
        #region Test helpers

        public void RunWeaklyConnectedComponentsAndCheck<TVertex, TEdge>([NotNull] IVertexListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            var algorithm = new WeaklyConnectedComponentsAlgorithm<TVertex, TEdge>(graph);
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
                foreach (TEdge edge in graph.OutEdges(vertex))
                {
                    Assert.AreEqual(algorithm.Components[edge.Source], algorithm.Components[edge.Target]);
                }
            }
        }

        #endregion

        [Test]
        public void Constructor()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var components = new Dictionary<int, int>();
            var algorithm = new WeaklyConnectedComponentsAlgorithm<int, Edge<int>>(graph);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new WeaklyConnectedComponentsAlgorithm<int, Edge<int>>(graph, components);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new WeaklyConnectedComponentsAlgorithm<int, Edge<int>>(null, graph, components);
            AssertAlgorithmProperties(algorithm, graph);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                WeaklyConnectedComponentsAlgorithm<TVertex, TEdge> algo,
                IVertexListGraph<TVertex, TEdge> g)
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                Assert.AreEqual(0, algo.ComponentCount);
                CollectionAssert.IsEmpty(algo.Components);
                CollectionAssert.IsEmpty(algo.Graphs);
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
            Assert.Throws<ArgumentNullException>(
                () => new WeaklyConnectedComponentsAlgorithm<int, Edge<int>>(null));

            Assert.Throws<ArgumentNullException>(
                () => new WeaklyConnectedComponentsAlgorithm<int, Edge<int>>(graph, null));
            Assert.Throws<ArgumentNullException>(
                () => new WeaklyConnectedComponentsAlgorithm<int, Edge<int>>(null, components));
            Assert.Throws<ArgumentNullException>(
                () => new WeaklyConnectedComponentsAlgorithm<int, Edge<int>>(null, null));

            Assert.Throws<ArgumentNullException>(
                () => new WeaklyConnectedComponentsAlgorithm<int, Edge<int>>(null, graph, null));
            Assert.Throws<ArgumentNullException>(
                () => new WeaklyConnectedComponentsAlgorithm<int, Edge<int>>(null, null, components));
            Assert.Throws<ArgumentNullException>(
                () => new WeaklyConnectedComponentsAlgorithm<int, Edge<int>>(null, null, null));
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
                new Edge<int>(1, 3),
                new Edge<int>(2, 3),
                new Edge<int>(4, 2),
                new Edge<int>(4, 3)
            });

            var algorithm = new WeaklyConnectedComponentsAlgorithm<int, Edge<int>>(graph);
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
            Assert.AreEqual(1, algorithm.Graphs.Length);
            CollectionAssert.AreEquivalent(
                new[] { 1, 2, 3, 4 },
                algorithm.Graphs[0].Vertices);
        }

        [Test]
        public void TwoComponents()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
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

            var algorithm = new WeaklyConnectedComponentsAlgorithm<int, Edge<int>>(graph);
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
            Assert.AreEqual(2, algorithm.Graphs.Length);
            CollectionAssert.AreEquivalent(
                new[] { 1, 2, 3, 4  },
                algorithm.Graphs[0].Vertices);
            CollectionAssert.AreEquivalent(
                new[] { 5, 6, 7 },
                algorithm.Graphs[1].Vertices);
        }

        [Test]
        public void MultipleComponents()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
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

            var algorithm = new WeaklyConnectedComponentsAlgorithm<int, Edge<int>>(graph);
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
            Assert.AreEqual(4, algorithm.Graphs.Length);
            CollectionAssert.AreEquivalent(
                new[] { 1, 2, 3, 4 },
                algorithm.Graphs[0].Vertices);
            CollectionAssert.AreEquivalent(
                new[] { 5, 6, 7 },
                algorithm.Graphs[1].Vertices);
            CollectionAssert.AreEquivalent(
                new[] { 8, 9 },
                algorithm.Graphs[2].Vertices);
            CollectionAssert.AreEquivalent(
                new[] { 10 },
                algorithm.Graphs[3].Vertices);
        }

        [Test]
        [Category(TestCategories.LongRunning)]
        public void WeaklyConnectedComponents()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs_All())
                RunWeaklyConnectedComponentsAndCheck(graph);
        }
    }
}