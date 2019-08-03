#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
using QuikGraph.Contracts;
#endif

namespace QuikGraph
{
    /// <summary>
    /// A graph with vertices of type <typeparamref name="TVertex"/>
    /// and edges of type <typeparamref name="TEdge"/> whose edges can be enumerated.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_CONTRACTS
    [ContractClass(typeof(EdgeListGraphContract<,>))]
#endif
    public interface IEdgeListGraph<TVertex, TEdge> : IGraph<TVertex, TEdge>, IEdgeSet<TVertex, TEdge>, IVertexSet<TVertex>
        where TEdge : IEdge<TVertex>
    {
    }
}
