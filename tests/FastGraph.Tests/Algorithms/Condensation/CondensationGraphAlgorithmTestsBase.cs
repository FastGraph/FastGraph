#nullable enable

using NUnit.Framework;
using FastGraph.Algorithms;
using FastGraph.Algorithms.Condensation;

namespace FastGraph.Tests.Algorithms.Condensation
{
    /// <summary>
    /// Base class for condensation algorithms.
    /// </summary>
    internal abstract class CondensationGraphAlgorithmTestsBase
    {
        #region Test helpers

        protected static void CheckVertexCount<TVertex, TEdge>(
            IVertexSet<TVertex> graph,
            IVertexSet<AdjacencyGraph<TVertex, TEdge>> condensedGraph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            int count = 0;
            foreach (AdjacencyGraph<TVertex, TEdge> vertices in condensedGraph.Vertices)
                count += vertices.VertexCount;
            Assert.AreEqual(graph.VertexCount, count, $"{nameof(graph.VertexCount)} does not match.");
        }

        protected static void CheckEdgeCount<TVertex, TEdge>(
            IEdgeSet<TVertex, TEdge> graph,
            IEdgeListGraph<AdjacencyGraph<TVertex, TEdge>, CondensedEdge<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>> condensedGraph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            // Check edge count
            int count = 0;
            foreach (CondensedEdge<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>> edges in condensedGraph.Edges)
                count += edges.Edges.Count;
            foreach (AdjacencyGraph<TVertex, TEdge> vertices in condensedGraph.Vertices)
                count += vertices.EdgeCount;
            Assert.AreEqual(graph.EdgeCount, count, $"{nameof(graph.EdgeCount)} does not match.");
        }

        protected static void CheckDAG<TVertex, TEdge>(
            IVertexAndEdgeListGraph<AdjacencyGraph<TVertex, TEdge>, CondensedEdge<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>> condensedGraph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            Assert.IsTrue(condensedGraph.IsDirectedAcyclicGraph());
        }

        #endregion
    }
}
