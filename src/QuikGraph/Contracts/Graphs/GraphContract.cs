#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;

namespace QuikGraph.Contracts
{
    /// <summary>
    /// Contract class for <see cref="IGraph{TVertex, TEdge}"/>.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    [ContractClassFor(typeof(IGraph<,>))]
    internal abstract class GraphContract<TVertex, TEdge> : IGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        bool IGraph<TVertex, TEdge>.IsDirected => default(bool);

        bool IGraph<TVertex, TEdge>.AllowParallelEdges => default(bool);
    }
}
#endif