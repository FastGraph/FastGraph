#nullable enable

namespace FastGraph.Algorithms.RandomWalks
{
    /// <summary>
    /// A Markov edges chain.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public interface IMarkovEdgeChain<TVertex, TEdge> : IEdgeChain<TVertex, TEdge>
        where TVertex : notnull
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Random number generator for a Markov process to do random walks.
        /// </summary>
        Random Rand { get; set; }
    }
}
