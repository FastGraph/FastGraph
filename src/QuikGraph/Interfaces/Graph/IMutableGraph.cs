#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
using QuikGraph.Contracts;
#endif

namespace QuikGraph
{
    /// <summary>
    /// A mutable graph with vertices of type <typeparamref name="TVertex"/>
    /// and edges of type <typeparamref name="TEdge"/>.
    /// </summary>
    /// <typeparam name="TVertex">type of the vertices.</typeparam>
    /// <typeparam name="TEdge">type of the edges.</typeparam>
#if SUPPORTS_CONTRACTS
    [ContractClass(typeof(MutableGraphContract<,>))]
#endif
    public interface IMutableGraph<TVertex, TEdge> : IGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Clears the vertex and edges.
        /// </summary>
        void Clear();
    }
}
