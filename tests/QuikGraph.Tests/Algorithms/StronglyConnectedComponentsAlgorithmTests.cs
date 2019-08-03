using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms.ConnectedComponents;

namespace QuikGraph.Tests.Algorithms
{
    /// <summary>
    /// Tests for <see cref="StronglyConnectedComponentsAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class StronglyConnectedComponentAlgorithmTests
    {
        #region Helpers

        private static void CheckStrong<TVertex, TEdge>([NotNull] StronglyConnectedComponentsAlgorithm<TVertex, TEdge> algorithm)
            where TEdge : IEdge<TVertex>
        {
            Assert.AreEqual(algorithm.VisitedGraph.VertexCount, algorithm.Components.Count);
            Assert.AreEqual(algorithm.VisitedGraph.VertexCount, algorithm.DiscoverTimes.Count);
            Assert.AreEqual(algorithm.VisitedGraph.VertexCount, algorithm.Roots.Count);

            foreach (TVertex vertex in algorithm.VisitedGraph.Vertices)
            {
                Assert.IsTrue(algorithm.Components.ContainsKey(vertex));
                Assert.IsTrue(algorithm.DiscoverTimes.ContainsKey(vertex));
            }

            foreach (KeyValuePair<TVertex, int> component in algorithm.Components)
            {
                Assert.IsNotNull(component.Key);
                Assert.IsTrue(component.Value <= algorithm.ComponentCount);
            }

            foreach (KeyValuePair<TVertex, int> time in algorithm.DiscoverTimes)
            {
                Assert.IsNotNull(time.Key);
            }
        }

        private static void Compute<TVertex, TEdge>([NotNull] AdjacencyGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            var strong = new StronglyConnectedComponentsAlgorithm<TVertex, TEdge>(graph);

            strong.Compute();
            CheckStrong(strong);
        }

        #endregion

        [Test]
        public void EmptyGraph()
        {
            var graph = new AdjacencyGraph<string, Edge<string>>(true);

            var strong = new StronglyConnectedComponentsAlgorithm<string, Edge<string>>(graph);
            strong.Compute();

            Assert.AreEqual(0, strong.ComponentCount);
            CheckStrong(strong);
        }

        [Test]
        public void OneVertex()
        {
            var graph = new AdjacencyGraph<string, Edge<string>>(true);
            graph.AddVertex("test");

            var strong = new StronglyConnectedComponentsAlgorithm<string, Edge<string>>(graph);
            strong.Compute();

            Assert.AreEqual(1, strong.ComponentCount);
            CheckStrong(strong);
        }

        [Test]
        public void TwoVertex()
        {
            var graph = new AdjacencyGraph<string, Edge<string>>(true);
            graph.AddVertex("v1");
            graph.AddVertex("v2");

            var strong = new StronglyConnectedComponentsAlgorithm<string, Edge<string>>(graph);
            strong.Compute();

            Assert.AreEqual(2, strong.ComponentCount);
            CheckStrong(strong);
        }

        [Test]
        public void TwoVertexOnEdge()
        {
            var graph = new AdjacencyGraph<string, Edge<string>>(true);
            graph.AddVertex("v1");
            graph.AddVertex("v2");
            graph.AddEdge(new Edge<string>("v1", "v2"));

            var strong = new StronglyConnectedComponentsAlgorithm<string, Edge<string>>(graph);
            strong.Compute();

            Assert.AreEqual(2, strong.ComponentCount);
            CheckStrong(strong);
        }

        [Test]
        public void TwoVertexCycle()
        {
            var graph = new AdjacencyGraph<string, Edge<string>>(true);
            graph.AddVertex("v1");
            graph.AddVertex("v2");
            graph.AddEdge(new Edge<string>("v1", "v2"));
            graph.AddEdge(new Edge<string>("v2", "v1"));

            var strong = new StronglyConnectedComponentsAlgorithm<string, Edge<string>>(graph);
            strong.Compute();

            Assert.AreEqual(1, strong.ComponentCount);
            CheckStrong(strong);
        }

        [Test]
        public void StronglyConnectedComponentAll()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs())
                Compute(graph);
        }
    }
}
