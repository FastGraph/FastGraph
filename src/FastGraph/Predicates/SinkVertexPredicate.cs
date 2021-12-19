#nullable enable

using JetBrains.Annotations;

namespace FastGraph.Predicates
{
    /// <summary>
    /// Predicate that tests if a vertex is a sink vertex (no output edge).
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public sealed class SinkVertexPredicate<TVertex, TEdge>
        where TVertex : notnull
        where TEdge : IEdge<TVertex>
    {
        private readonly IIncidenceGraph<TVertex, TEdge> _visitedGraph;

        /// <summary>
        /// Initializes a new instance of the <see cref="SinkVertexPredicate{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to consider.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        public SinkVertexPredicate(IIncidenceGraph<TVertex, TEdge> visitedGraph)
        {
            _visitedGraph = visitedGraph ?? throw new ArgumentNullException(nameof(visitedGraph));
        }

        /// <summary>
        /// Checks if the given <paramref name="vertex"/> is a sink vertex.
        /// </summary>
        /// <remarks>Check if the implemented predicate is matched.</remarks>
        /// <param name="vertex">Vertex to use in predicate.</param>
        /// <returns>True if the vertex is a sink, false otherwise.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="vertex"/> is <see langword="null"/>.</exception>
        [Pure]
        public bool Test(TVertex vertex)
        {
            return _visitedGraph.IsOutEdgesEmpty(vertex);
        }
    }
}
