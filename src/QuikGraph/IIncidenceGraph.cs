using System.Collections.Generic;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
using QuickGraph.Contracts;
#endif

namespace QuickGraph
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
