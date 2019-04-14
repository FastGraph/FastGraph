#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
using QuikGraph.Contracts;

#endif

namespace QuikGraph
{
    /// <summary>
    /// A mutable vertex list graph
    /// </summary>
    /// <typeparam name="TVertex"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
#if SUPPORTS_CONTRACTS
    [ContractClass(typeof(IMutableVertexListGraphContract<,>))]
#endif
    public interface IMutableVertexListGraph<TVertex, TEdge> :
         IMutableIncidenceGraph<TVertex, TEdge>,
         IMutableVertexSet<TVertex>
         where TEdge : IEdge<TVertex>
    {
    }
}
