#nullable enable

#if SUPPORTS_SERIALIZATION
#endif
using FastGraph.Algorithms.Services;

namespace FastGraph.Algorithms.Cliques
{
    /// <summary>
    /// Base class for all maximum clique graph algorithm.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public abstract class MaximumCliqueAlgorithmBase<TVertex, TEdge> : AlgorithmBase<IUndirectedGraph<TVertex, TEdge>>
        where TVertex : notnull
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MaximumCliqueAlgorithmBase{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        protected MaximumCliqueAlgorithmBase(
            IAlgorithmComponent? host,
            IUndirectedGraph<TVertex, TEdge> visitedGraph)
            : base(host, visitedGraph)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaximumCliqueAlgorithmBase{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        protected MaximumCliqueAlgorithmBase(IUndirectedGraph<TVertex, TEdge> visitedGraph)
            : base(visitedGraph)
        {
        }
    }
}
