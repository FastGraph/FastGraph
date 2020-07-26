using System;
using System.Collections.Generic;
using NUnit.Framework;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.ConnectedComponents;
using static QuikGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace QuikGraph.Tests.Algorithms.ConnectedComponents
{
    /// <summary>
    /// Tests for <see cref="IncrementalConnectedComponentsAlgorithm{TVertex,TEdge}"/>
    /// </summary>
    [TestFixture]
    internal class IncrementalConnectedComponentsAlgorithmTests
    {
        [Test]
        public void Constructor()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new IncrementalConnectedComponentsAlgorithm<int, Edge<int>>(graph);
            AssertAlgorithmState(algorithm, graph);

            algorithm = new IncrementalConnectedComponentsAlgorithm<int, Edge<int>>(null, graph);
            AssertAlgorithmState(algorithm, graph);
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => new IncrementalConnectedComponentsAlgorithm<int, Edge<int>>(null));

            Assert.Throws<ArgumentNullException>(
                () => new IncrementalConnectedComponentsAlgorithm<int, Edge<int>>(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void InvalidUse()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new IncrementalConnectedComponentsAlgorithm<int, Edge<int>>(graph);

            Assert.Throws<InvalidOperationException>(() => { var _ = algorithm.ComponentCount; });
            Assert.Throws<InvalidOperationException>(() => algorithm.GetComponents());
        }

        [Test]
        public void IncrementalConnectedComponent()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVertexRange(new[] { 0, 1, 2, 3 });

            var algorithm = new IncrementalConnectedComponentsAlgorithm<int, Edge<int>>(graph);
            algorithm.Compute();

            Assert.AreEqual(4, algorithm.ComponentCount);
            Assert.AreEqual(4, algorithm.GetComponents().Key);
            CollectionAssert.AreEquivalent(
                new Dictionary<int, int>
                {
                    [0] = 0,
                    [1] = 1,
                    [2] = 2,
                    [3] = 3
                },
                algorithm.GetComponents().Value);

            graph.AddEdge(new Edge<int>(0, 1));
            Assert.AreEqual(3, algorithm.ComponentCount);
            Assert.AreEqual(3, algorithm.GetComponents().Key);
            CollectionAssert.AreEquivalent(
                new Dictionary<int, int>
                {
                    [0] = 0,
                    [1] = 0,
                    [2] = 1,
                    [3] = 2
                },
                algorithm.GetComponents().Value);

            graph.AddEdge(new Edge<int>(2, 3));
            Assert.AreEqual(2, algorithm.ComponentCount);
            Assert.AreEqual(2, algorithm.GetComponents().Key);
            CollectionAssert.AreEquivalent(
                new Dictionary<int, int>
                {
                    [0] = 0,
                    [1] = 0,
                    [2] = 1,
                    [3] = 1
                },
                algorithm.GetComponents().Value);

            graph.AddEdge(new Edge<int>(1, 3));
            Assert.AreEqual(1, algorithm.ComponentCount);
            Assert.AreEqual(1, algorithm.GetComponents().Key);
            CollectionAssert.AreEquivalent(
                new Dictionary<int, int>
                {
                    [0] = 0,
                    [1] = 0,
                    [2] = 0,
                    [3] = 0
                },
                algorithm.GetComponents().Value);

            graph.AddVerticesAndEdge(new Edge<int>(4, 5));
            Assert.AreEqual(2, algorithm.ComponentCount);
            Assert.AreEqual(2, algorithm.GetComponents().Key);
            CollectionAssert.AreEquivalent(
                new Dictionary<int, int>
                {
                    [0] = 0,
                    [1] = 0,
                    [2] = 0,
                    [3] = 0,
                    [4] = 1,
                    [5] = 1
                },
                algorithm.GetComponents().Value);

            graph.AddVertex(6);
            Assert.AreEqual(3, algorithm.ComponentCount);
            Assert.AreEqual(3, algorithm.GetComponents().Key);
            CollectionAssert.AreEquivalent(
                new Dictionary<int, int>
                {
                    [0] = 0,
                    [1] = 0,
                    [2] = 0,
                    [3] = 0,
                    [4] = 1,
                    [5] = 1,
                    [6] = 2
                },
                algorithm.GetComponents().Value);
        }

        [Test]
        public void IncrementalConnectedComponent_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var edge13 = new Edge<int>(1, 3);
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                edge13,
                new Edge<int>(4, 5)
            });
            graph.AddVertex(6);

            using (graph.IncrementalConnectedComponents(out Func<KeyValuePair<int, IDictionary<int, int>>> _))
            {
                Assert.Throws<InvalidOperationException>(() => graph.RemoveVertex(6));
                Assert.Throws<InvalidOperationException>(() => graph.RemoveEdge(edge13));
            }
        }

        [Test]
        public void IncrementalConnectedComponentMultiRun()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new IncrementalConnectedComponentsAlgorithm<int, Edge<int>>(graph);
            Assert.DoesNotThrow(() =>
            {
                algorithm.Compute();
                algorithm.Compute();
            });
        }

        [Test]
        public void Dispose()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new IncrementalConnectedComponentsAlgorithm<int, Edge<int>>(graph);
            Assert.DoesNotThrow(() => algorithm.Dispose());

            algorithm = new IncrementalConnectedComponentsAlgorithm<int, Edge<int>>(graph);
            algorithm.Compute();
            Assert.DoesNotThrow(() => algorithm.Dispose());
        }
    }
}