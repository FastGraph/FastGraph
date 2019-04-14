using System.Collections.Generic;
using QuikGraph.Contracts;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;

#endif

namespace QuikGraph
{
#if SUPPORTS_CONTRACTS
    [ContractClass(typeof(IIncidenceGraphContract<,>))]
#endif
    public interface IIncidenceGraph<TVertex, TEdge> 
        : IImplicitGraph<TVertex,TEdge>
        where TEdge : IEdge<TVertex>
    {
        bool ContainsEdge(TVertex source, TVertex target);

        bool TryGetEdges(
            TVertex source,
            TVertex target,
            out IEnumerable<TEdge> edges);

        bool TryGetEdge(
            TVertex source,
            TVertex target,
            out TEdge edge);
    }
}
