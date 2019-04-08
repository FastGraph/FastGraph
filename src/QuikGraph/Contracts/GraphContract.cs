#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif

namespace QuickGraph.Contracts
{
    /// <summary>
    /// Debug only assertions and assumptions
    /// </summary>
    public static class GraphContract
    {
#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public static bool VertexCountEqual<TVertex>(this IVertexSet<TVertex> left, IVertexSet<TVertex> right)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(left != null);
            Contract.Requires(right != null);
#endif

            return left.VertexCount == right.VertexCount;
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public static bool EdgeCountEqual<TVertex, TEdge>(this IEdgeListGraph<TVertex, TEdge> left, IEdgeListGraph<TVertex, TEdge> right)
            where TEdge : IEdge<TVertex>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(left != null);
            Contract.Requires(right != null);
#endif

            return left.EdgeCount == right.EdgeCount;
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public static bool InVertexSet<TVertex>(IVertexSet<TVertex> g, TVertex v)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(g != null);
            Contract.Requires(v != null);
#endif

            // todo make requires
            return g.ContainsVertex(v);
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public static bool InVertexSet<TVertex, TEdge>(IEdgeListGraph<TVertex, TEdge> g, TEdge e)
            where TEdge : IEdge<TVertex>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(g != null);
            Contract.Requires(e != null);
#endif

            return InVertexSet<TVertex>(g, e.Source)
                && InVertexSet<TVertex>(g, e.Target);
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public static bool InEdgeSet<TVertex, TEdge>(
            IEdgeListGraph<TVertex, TEdge> g,
            TEdge e)
            where TEdge : IEdge<TVertex>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(g != null);
            Contract.Requires(e != null);
#endif

            return InVertexSet(g, e)
                && g.ContainsEdge(e);
        }
    }
}
