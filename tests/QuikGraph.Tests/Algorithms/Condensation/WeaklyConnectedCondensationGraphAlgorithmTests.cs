using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.Condensation;
using QuikGraph.Algorithms.ConnectedComponents;
using static QuikGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace QuikGraph.Tests.Algorithms.Condensation
{
    /// <summary>
    /// Tests for <see cref="CondensationGraphAlgorithm{TVertex,TEdge,TGraph}"/> (weakly connected).
    /// </summary>
    [TestFixture]
    internal class WeaklyConnectedCondensationGraphAlgorithmTests : CondensationGraphAlgorithmTestsBase
    {
        #region Test helpers

        private static void RunWeaklyConnectedCondensationAndCheck<TVertex, TEdge>(
            [NotNull] IVertexAndEdgeListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            IMutableBidirectionalGraph<AdjacencyGraph<TVertex, TEdge>, CondensedEdge<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>> condensedGraph =
                graph.CondensateWeaklyConnected<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>();

            Assert.IsNotNull(condensedGraph);
            CheckVertexCount(graph, condensedGraph);
            CheckEdgeCount(graph, condensedGraph);
            CheckComponentCount(graph, condensedGraph);
        }

        private static void CheckComponentCount<TVertex, TEdge>(
            [NotNull] IVertexListGraph<TVertex, TEdge> graph,
            [NotNull] IVertexSet<AdjacencyGraph<TVertex, TEdge>> condensedGraph)
            where TEdge : IEdge<TVertex>
        {
            // Check number of vertices = number of strongly connected components
            int components = graph.WeaklyConnectedComponents(new Dictionary<TVertex, int>());
            Assert.AreEqual(components, condensedGraph.VertexCount, "Component count does not match.");
        }

        #endregion

        [Test]
        public void Constructor()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm1 = new CondensationGraphAlgorithm<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(graph);
            AssertAlgorithmProperties(algorithm1, graph);

            algorithm1 = new CondensationGraphAlgorithm<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(graph)
            {
                StronglyConnected = false
            };
            AssertAlgorithmProperties(algorithm1, graph, false);

            var algorithm2 = new CondensationGraphAlgorithm<int, Edge<int>, BidirectionalGraph<int, Edge<int>>>(graph);
            AssertAlgorithmProperties(algorithm2, graph);

            algorithm2 = new CondensationGraphAlgorithm<int, Edge<int>, BidirectionalGraph<int, Edge<int>>>(graph)
            {
                StronglyConnected = false
            };
            AssertAlgorithmProperties(algorithm2, graph, false);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge, TGraph>(
                CondensationGraphAlgorithm<TVertex, TEdge, TGraph> algo,
                IVertexAndEdgeListGraph<TVertex, TEdge> g,
                bool stronglyConnected = true)
                where TEdge : IEdge<TVertex>
                where TGraph : IMutableVertexAndEdgeSet<TVertex, TEdge>, new()
            {
                AssertAlgorithmState(algo, g);
                Assert.AreEqual(stronglyConnected, algo.StronglyConnected);
                Assert.IsNull(algo.CondensedGraph);
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
        public void OneWeaklyConnectedComponent()
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge23 = new Edge<int>(2, 3);
            var edge42 = new Edge<int>(4, 2);
            var edge43 = new Edge<int>(4, 3);

            var edge45 = new Edge<int>(4, 5);

            var edge56 = new Edge<int>(5, 6);
            var edge57 = new Edge<int>(5, 7);
            var edge76 = new Edge<int>(7, 6);

            var edge71 = new Edge<int>(7, 1);

            var edge89 = new Edge<int>(8, 9);

            var edge82 = new Edge<int>(8, 2);

            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge13, edge23, edge42, edge43, edge45,
                edge56, edge57, edge76, edge71, edge89, edge82
            });

            IMutableBidirectionalGraph<AdjacencyGraph<int, Edge<int>>, CondensedEdge<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>> condensedGraph =
                graph.CondensateWeaklyConnected<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>();

            Assert.IsNotNull(condensedGraph);
            Assert.AreEqual(1, condensedGraph.VertexCount);
            Assert.AreEqual(0, condensedGraph.EdgeCount);
            CollectionAssert.AreEquivalent(graph.Vertices, condensedGraph.Vertices.ElementAt(0).Vertices);
            CollectionAssert.AreEquivalent(graph.Edges, condensedGraph.Vertices.ElementAt(0).Edges);
        }

        [Test]
        public void MultipleWeaklyConnectedComponents()
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge23 = new Edge<int>(2, 3);
            var edge42 = new Edge<int>(4, 2);
            var edge43 = new Edge<int>(4, 3);

            var edge56 = new Edge<int>(5, 6);
            var edge57 = new Edge<int>(5, 7);
            var edge76 = new Edge<int>(7, 6);

            var edge89 = new Edge<int>(8, 9);

            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge13, edge23, edge42, edge43,
                edge56, edge57, edge76, edge89
            });

            IMutableBidirectionalGraph<AdjacencyGraph<int, Edge<int>>, CondensedEdge<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>> condensedGraph =
                graph.CondensateWeaklyConnected<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>();

            Assert.IsNotNull(condensedGraph);
            Assert.AreEqual(3, condensedGraph.VertexCount);
            Assert.AreEqual(0, condensedGraph.EdgeCount);
            CollectionAssert.AreEquivalent(
                new[] { 1, 2, 3, 4 },
                condensedGraph.Vertices.ElementAt(0).Vertices);
            CollectionAssert.AreEquivalent(
                new[] { edge12, edge13, edge23, edge42, edge43 },
                condensedGraph.Vertices.ElementAt(0).Edges);

            CollectionAssert.AreEquivalent(
                new[] { 5, 6, 7 },
                condensedGraph.Vertices.ElementAt(1).Vertices);
            CollectionAssert.AreEquivalent(
                new[] { edge56, edge57, edge76 },
                condensedGraph.Vertices.ElementAt(1).Edges);

            CollectionAssert.AreEquivalent(
                new[] { 8, 9 },
                condensedGraph.Vertices.ElementAt(2).Vertices);
            CollectionAssert.AreEquivalent(
                new[] { edge89 },
                condensedGraph.Vertices.ElementAt(2).Edges);
        }

        [Test]
        [Category(TestCategories.LongRunning)]
        public void WeaklyConnectedCondensation()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs_All())
                RunWeaklyConnectedCondensationAndCheck(graph);
        }
    }
}