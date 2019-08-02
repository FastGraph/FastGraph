using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.Condensation;

namespace QuikGraph.Tests.Algorithms.Condensation
{
    /// <summary>
    /// Tests for <see cref="CondensationGraphAlgorithm{TVertex,TEdge,TGraph}"/> (strongly connected).
    /// </summary>
    [TestFixture]
    internal class StronglyConnectedCondensationGraphAlgorithmTests
    {
        #region Helpers

        private static void RunStronglyConnectedCondensateAndCheck<TVertex, TEdge>(
            [NotNull] IVertexAndEdgeListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            var condensedGraph = graph.CondensateStronglyConnected<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>();

            CheckVertexCount(graph, condensedGraph);
            CheckEdgeCount(graph, condensedGraph);
            CheckComponentCount(graph, condensedGraph);
            CheckDAG(condensedGraph);
        }

        private static void CheckVertexCount<TVertex, TEdge>(
            [NotNull] IVertexAndEdgeListGraph<TVertex, TEdge> graph,
            [NotNull] IMutableBidirectionalGraph<AdjacencyGraph<TVertex, TEdge>, CondensedEdge<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>> condensedGraph)
            where TEdge : IEdge<TVertex>
        {
            int count = 0;
            foreach (AdjacencyGraph<TVertex, TEdge> vertices in condensedGraph.Vertices)
                count += vertices.VertexCount;
            Assert.AreEqual(graph.VertexCount, count, "VertexCount does not match.");
        }

        private static void CheckEdgeCount<TVertex, TEdge>(
            [NotNull] IVertexAndEdgeListGraph<TVertex, TEdge> graph,
            [NotNull] IMutableBidirectionalGraph<AdjacencyGraph<TVertex, TEdge>, CondensedEdge<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>> condensedGraph)
            where TEdge : IEdge<TVertex>
        {
            // check edge count
            int count = 0;
            foreach (CondensedEdge<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>> edges in condensedGraph.Edges)
                count += edges.Edges.Count;
            foreach (AdjacencyGraph<TVertex, TEdge> vertices in condensedGraph.Vertices)
                count += vertices.EdgeCount;
            Assert.AreEqual(graph.EdgeCount, count, "EdgeCount does not match.");
        }

        private static void CheckComponentCount<TVertex, TEdge>(
            [NotNull] IVertexAndEdgeListGraph<TVertex, TEdge> graph,
            [NotNull] IMutableBidirectionalGraph<AdjacencyGraph<TVertex, TEdge>, CondensedEdge<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>> condensedGraph)
            where TEdge : IEdge<TVertex>
        {
            // Check number of vertices = number of strongly connected components
            var components = new Dictionary<TVertex, int>();
            int componentCount = graph.StronglyConnectedComponents(components);
            Assert.AreEqual(componentCount, condensedGraph.VertexCount, "Component count does not match.");
        }

        private static void CheckDAG<TVertex, TEdge>(
            [NotNull] IMutableBidirectionalGraph<AdjacencyGraph<TVertex, TEdge>, CondensedEdge<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>> condensedGraph)
            where TEdge : IEdge<TVertex>
        {
            // Check it's a dag
            try
            {
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                condensedGraph.TopologicalSort();
            }
            catch (NonAcyclicGraphException)
            {
                Assert.Fail("Graph is not a DAG.");
            }
        }

        #endregion

        [Test]
        public void StronglyConnectedCondensateAll()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs())
                RunStronglyConnectedCondensateAndCheck(graph);
        }
    }
}
