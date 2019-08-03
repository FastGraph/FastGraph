using System;
using JetBrains.Annotations;

namespace QuikGraph.Predicates
{
    /// <summary>
    /// Predicate that tests if a vertex is a root vertex (no input edge).
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class SinkVertexPredicate<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        [NotNull]
        private readonly IIncidenceGraph<TVertex, TEdge> _visitedGraph;

        /// <summary>
        /// Initializes a new instance of the <see cref="SinkVertexPredicate{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to consider.</param>
        public SinkVertexPredicate([NotNull] IIncidenceGraph<TVertex, TEdge> visitedGraph)
        {
            _visitedGraph = visitedGraph ?? throw new ArgumentNullException(nameof(visitedGraph));
        }

        /// <summary>
        /// Checks if the given <paramref name="vertex"/> is a root vertex.
        /// </summary>
        /// <remarks>Check if the implemented predicate is matched.</remarks>
        /// <param name="vertex">Vertex to check.</param>
        /// <returns>True if the vertex is a root one, false otherwise.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        public bool Test([NotNull] TVertex vertex)
        {
            return _visitedGraph.IsOutEdgesEmpty(vertex);
        }
    }
}
