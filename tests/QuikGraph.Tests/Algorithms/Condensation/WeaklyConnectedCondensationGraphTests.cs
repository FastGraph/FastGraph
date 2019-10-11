using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.Condensation;

namespace QuikGraph.Tests.Algorithms.Condensation
{
    /// <summary>
    /// Tests for <see cref="CondensationGraphAlgorithm{TVertex,TEdge,TGraph}"/> (weakly connected).
    /// </summary>
    [TestFixture]
    internal class WeaklyConnectedCondensationGraphAlgorithmTests
    {
        #region Helpers

        private static void RunWeaklyConnectedCondensateAndCheck<TVertex, TEdge>(
            [NotNull] IVertexAndEdgeListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            var algorithm = new CondensationGraphAlgorithm<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>(graph)
            {
                StronglyConnected = false
            };
            algorithm.Compute();
            CheckVertexCount(graph, algorithm);
            CheckEdgeCount(graph, algorithm);
            CheckComponentCount(graph, algorithm);
        }

        private static void CheckVertexCount<TVertex, TEdge>(
            [NotNull] IVertexAndEdgeListGraph<TVertex, TEdge> graph,
            [NotNull] CondensationGraphAlgorithm<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>> algorithm)
            where TEdge : IEdge<TVertex>
        {
            int count = 0;
            foreach (AdjacencyGraph<TVertex, TEdge> vertices in algorithm.CondensedGraph.Vertices)
                count += vertices.VertexCount;
            Assert.AreEqual(graph.VertexCount, count, "VertexCount does not match.");
        }

        private static void CheckEdgeCount<TVertex, TEdge>(
            [NotNull] IVertexAndEdgeListGraph<TVertex, TEdge> graph,
            [NotNull] CondensationGraphAlgorithm<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>> algorithm)
            where TEdge : IEdge<TVertex>
        {
            // Check edge count
            int count = 0;
            foreach (CondensedEdge<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>> edges in algorithm.CondensedGraph.Edges)
                count += edges.Edges.Count;
            foreach (AdjacencyGraph<TVertex, TEdge> vertices in algorithm.CondensedGraph.Vertices)
                count += vertices.EdgeCount;
            Assert.AreEqual(graph.EdgeCount, count, "EdgeCount does not match.");
        }

        private static void CheckComponentCount<TVertex, TEdge>(
            [NotNull] IVertexAndEdgeListGraph<TVertex, TEdge> graph,
            [NotNull] CondensationGraphAlgorithm<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>> algorithm)
            where TEdge : IEdge<TVertex>
        {
            // Check number of vertices = number of strongly connected components
            int components = graph.WeaklyConnectedComponents(new Dictionary<TVertex, int>());
            Assert.AreEqual(components, algorithm.CondensedGraph.VertexCount, "Component count does not match.");
        }

        #endregion

        [Test]
        public void WeaklyConnectedCondensateAll()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs_TMP())
                RunWeaklyConnectedCondensateAndCheck(graph);
        }
    }
}
