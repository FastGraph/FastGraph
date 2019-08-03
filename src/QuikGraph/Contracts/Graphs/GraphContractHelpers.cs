
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
#if SUPPORTS_CONTRACTS
            Contract.Requires(left != null);
            Contract.Requires(right != null);
#endif

            return left.VertexCount == right.VertexCount;
        }

        public static bool EdgeCountEqual<TVertex, TEdge>(
            this IEdgeListGraph<TVertex, TEdge> left, 
            IEdgeListGraph<TVertex, TEdge> right)
            where TEdge : IEdge<TVertex>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(left != null);
            Contract.Requires(right != null);
#endif

            return left.EdgeCount == right.EdgeCount;
        }

        public static bool InVertexSet<TVertex>(
            IVertexSet<TVertex> graph, 
            TVertex vertex)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(graph != null);
            Contract.Requires(vertex != null);
#endif

            // todo make requires
            return graph.ContainsVertex(vertex);
        }

        public static bool InVertexSet<TVertex, TEdge>(
            IEdgeListGraph<TVertex, TEdge> graph, 
            TEdge edge)
            where TEdge : IEdge<TVertex>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(graph != null);
            Contract.Requires(edge != null);
#endif

            return InVertexSet(graph, edge.Source)
                   && InVertexSet(graph, edge.Target);
        }

        public static bool InEdgeSet<TVertex, TEdge>(
            IEdgeListGraph<TVertex, TEdge> graph,
            TEdge edge)
            where TEdge : IEdge<TVertex>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(graph != null);
            Contract.Requires(edge != null);
#endif

            return InVertexSet(graph, edge) && graph.ContainsEdge(edge);
        }
    }
}