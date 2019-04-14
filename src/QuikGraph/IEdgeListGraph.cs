#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
using QuikGraph.Contracts;

#endif

namespace QuikGraph
{
    /// <summary>
    /// A graph whose edges can be enumerated.
    /// </summary>
    /// <typeparam name="TVertex">type of the vertices.</typeparam>
    /// <typeparam name="TEdge">type of the edges.</typeparam>
#if SUPPORTS_CONTRACTS
    [ContractClass(typeof(IEdgeListGraphContract<,>))]
#endif
    public interface IEdgeListGraph<TVertex, TEdge> : IGraph<TVertex, TEdge>, IEdgeSet<TVertex, TEdge>, IVertexSet<TVertex>
        where TEdge : IEdge<TVertex>
    {
    }
}
