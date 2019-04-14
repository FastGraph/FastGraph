using System.Collections.Generic;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using QuikGraph.Contracts;

namespace QuikGraph
{
#if SUPPORTS_CONTRACTS
    [ContractClass(typeof(IImplicitUndirectedGraphContract<,>))]
#endif
    public interface IImplicitUndirectedGraph<TVertex, TEdge>
        : IImplicitVertexSet<TVertex>
        , IGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        EdgeEqualityComparer<TVertex, TEdge> EdgeEqualityComparer { get; }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        IEnumerable<TEdge> AdjacentEdges(TVertex v);

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        int AdjacentDegree(TVertex v);

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        bool IsAdjacentEdgesEmpty(TVertex v);

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        TEdge AdjacentEdge(TVertex v, int index);

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        bool TryGetEdge(TVertex source, TVertex target, out TEdge edge);

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        bool ContainsEdge(TVertex source, TVertex target);
    }
}
