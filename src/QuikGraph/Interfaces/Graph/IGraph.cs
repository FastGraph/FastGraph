#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
using QuikGraph.Contracts;
#endif

namespace QuikGraph
{
    /// <summary>
    /// A graph with vertices of type <typeparamref name="TVertex"/>
    /// and edges of type <typeparamref name="TEdge"/>.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_CONTRACTS
    [ContractClass(typeof(GraphContract<,>))]
#endif
    // ReSharper disable once UnusedTypeParameter
    public interface IGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Gets a value indicating if the graph is directed
        /// </summary>
        bool IsDirected { get; }

        /// <summary>
        /// Gets a value indicating if the graph allows parallel edges
        /// </summary>
        bool AllowParallelEdges { get; }
    }
}
