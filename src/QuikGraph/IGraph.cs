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
    /// <typeparam name="TVertex">type of the vertices.</typeparam>
    /// <typeparam name="TEdge">type of the edges.</typeparam>
#if SUPPORTS_CONTRACTS
    [ContractClass(typeof(GraphContract<,>))]
#endif
    public interface IGraph<TVertex,TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Gets a value indicating if the graph is directed
        /// </summary>
        bool IsDirected { get;}

        /// <summary>
        /// Gets a value indicating if the graph allows parallel edges
        /// </summary>
        bool AllowParallelEdges { get;}
    }
}
