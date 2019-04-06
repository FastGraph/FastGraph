#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif

namespace QuickGraph.Predicates
{
    /// <summary>
    /// A vertex predicate that detects vertex with no in or out edges.
    /// </summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    /// <typeparam name="TEdge">type of the edges</typeparam>
    public sealed class IsolatedVertexPredicate<TVertex,TEdge>
        where TEdge : IEdge<TVertex>
    {
        private readonly IBidirectionalGraph<TVertex, TEdge> visitedGraph;

        public IsolatedVertexPredicate(IBidirectionalGraph<TVertex,TEdge> visitedGraph)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(visitedGraph!=null);
#endif

            this.visitedGraph = visitedGraph;
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public bool Test(TVertex v)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(v != null);
#endif

            return this.visitedGraph.IsInEdgesEmpty(v)
                && this.visitedGraph.IsOutEdgesEmpty(v);
        }
    }
}
