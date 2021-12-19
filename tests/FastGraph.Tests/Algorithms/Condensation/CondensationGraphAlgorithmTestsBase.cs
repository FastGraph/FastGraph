#nullable enable

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
            count.Should().Be(graph.VertexCount, because: $"{nameof(graph.VertexCount)} does not match.");
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
            count.Should().Be(graph.EdgeCount, because: $"{nameof(graph.EdgeCount)} does not match.");
        }

        protected static void CheckDAG<TVertex, TEdge>(
            IVertexAndEdgeListGraph<AdjacencyGraph<TVertex, TEdge>, CondensedEdge<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>> condensedGraph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            condensedGraph.IsDirectedAcyclicGraph().Should().BeTrue();
        }

        #endregion
    }
}
