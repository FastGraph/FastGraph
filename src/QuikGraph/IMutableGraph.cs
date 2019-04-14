#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using QuikGraph.Contracts;

namespace QuikGraph
{
    /// <summary>
    /// A mutable graph instance
    /// </summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    /// <typeparam name="TEdge">type of the edges</typeparam>
#if SUPPORTS_CONTRACTS
    [ContractClass(typeof(IMutableGraphContract<,>))]
#endif
    public interface IMutableGraph<TVertex,TEdge> 
        : IGraph<TVertex,TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Clears the vertex and edges
        /// </summary>
        void Clear();
    }
}
