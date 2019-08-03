using System;

namespace QuikGraph.Contracts
{
    /// <summary>
    /// Assertions and assumptions for graphs.
    /// </summary>
    internal static class GraphContractHelpers
    {
        public static bool VertexCountEqual<TVertex>(
            this IVertexSet<TVertex> left, 
            IVertexSet<TVertex> right)
        {
            if (left is null)
                throw new ArgumentNullException(nameof(left));
            if (right is null)
                throw new ArgumentNullException(nameof(right));

            return left.VertexCount == right.VertexCount;
        }

        public static bool EdgeCountEqual<TVertex, TEdge>(
            this IEdgeListGraph<TVertex, TEdge> left, 
            IEdgeListGraph<TVertex, TEdge> right)
            where TEdge : IEdge<TVertex>
        {
            if (left is null)
                throw new ArgumentNullException(nameof(left));
            if (right is null)
                throw new ArgumentNullException(nameof(right));

            return left.EdgeCount == right.EdgeCount;
        }

        public static bool InVertexSet<TVertex>(
            IVertexSet<TVertex> graph, 
            TVertex vertex)
        {
            if (graph is null)
                throw new ArgumentNullException(nameof(graph));
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            // todo make requires
            return graph.ContainsVertex(vertex);
        }

        public static bool InVertexSet<TVertex, TEdge>(
            IEdgeListGraph<TVertex, TEdge> graph, 
            TEdge edge)
            where TEdge : IEdge<TVertex>
        {
            if (graph is null)
                throw new ArgumentNullException(nameof(graph));
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            return InVertexSet(graph, edge.Source)
                   && InVertexSet(graph, edge.Target);
        }

        public static bool InEdgeSet<TVertex, TEdge>(
            IEdgeListGraph<TVertex, TEdge> graph,
            TEdge edge)
            where TEdge : IEdge<TVertex>
        {
            if (graph is null)
                throw new ArgumentNullException(nameof(graph));
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            return InVertexSet(graph, edge) && graph.ContainsEdge(edge);
        }
    }
}