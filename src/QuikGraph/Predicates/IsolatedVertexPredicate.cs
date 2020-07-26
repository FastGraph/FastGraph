using System;
using JetBrains.Annotations;

namespace QuikGraph.Predicates
{
    /// <summary>
    /// Predicate that detects if a vertex is isolated (without any input or output edges).
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public sealed class IsolatedVertexPredicate<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        [NotNull]
        private readonly IBidirectionalGraph<TVertex, TEdge> _visitedGraph;

        /// <summary>
        /// Initializes a new instance of the <see cref="IsolatedVertexPredicate{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to consider.</param>
        public IsolatedVertexPredicate([NotNull] IBidirectionalGraph<TVertex, TEdge> visitedGraph)
        {
            _visitedGraph = visitedGraph ?? throw new ArgumentNullException(nameof(visitedGraph));
        }

        /// <summary>
        /// Checks if the given <paramref name="vertex"/> is isolated.
        /// </summary>
        /// <remarks>Check if the implemented predicate is matched.</remarks>
        /// <param name="vertex">Vertex to check.</param>
        /// <returns>True if the vertex is isolated, false otherwise.</returns>
        [Pure]
        public bool Test([NotNull] TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            return _visitedGraph.IsInEdgesEmpty(vertex)
                   && _visitedGraph.IsOutEdgesEmpty(vertex);
        }
    }
}