#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
using QuikGraph.Contracts;

#endif

namespace QuikGraph
{
    /// <summary>
    /// An undirected graph
    /// </summary>
    /// <typeparam name="TVertex"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
#if SUPPORTS_CONTRACTS
    [ContractClass(typeof(IUndirectedGraphContract<,>))]
#endif
    public interface IUndirectedGraph<TVertex,TEdge> 
        : IImplicitUndirectedGraph<TVertex, TEdge>
        , IEdgeListGraph<TVertex,TEdge>
        , IGraph<TVertex,TEdge>
        where TEdge : IEdge<TVertex>
    {
    }
}
