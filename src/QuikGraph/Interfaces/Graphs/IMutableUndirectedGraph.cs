using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// A mutable indirect graph with vertices of type <typeparamref name="TVertex"/>
    /// and edges of type <typeparamref name="TEdge"/>.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public interface IMutableUndirectedGraph<TVertex, TEdge>
        : IUndirectedGraph<TVertex, TEdge>
        , IMutableVertexAndEdgeSet<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Removes adjacent edges of the given <paramref name="vertex"/> if edge matches the <paramref name="predicate"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="predicate">Predicate to match edges.</param>
        /// <returns>The number of removed edges.</returns>
        int RemoveAdjacentEdgeIf([NotNull] TVertex vertex, [NotNull, InstantHandle] EdgePredicate<TVertex, TEdge> predicate);

        /// <summary>
        /// Clears adjacent edges of the given <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        void ClearAdjacentEdges([NotNull] TVertex vertex);
    }
}