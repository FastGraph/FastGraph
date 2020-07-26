using System.Collections.Generic;
using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// An incident graph with vertices of type <typeparamref name="TVertex"/>
    /// and edges of type <typeparamref name="TEdge"/>.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public interface IIncidenceGraph<TVertex, TEdge> : IImplicitGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Checks if this graph contains an edge that link
        /// <paramref name="source"/> and <paramref name="target"/> vertices.
        /// </summary>
        /// <param name="source">Source vertex.</param>
        /// <param name="target">Target vertex.</param>
        /// <returns>True if an edge exists, false otherwise.</returns>
        [Pure]
        bool ContainsEdge([NotNull] TVertex source, [NotNull] TVertex target);

        /// <summary>
        /// Tries to get the edge that link
        /// <paramref name="source"/> and <paramref name="target"/> vertices.
        /// </summary>
        /// <param name="source">Source vertex.</param>
        /// <param name="target">Target vertex.</param>
        /// <param name="edge">Edge found, otherwise null.</param>
        /// <returns>True if an edge was found, false otherwise.</returns>
        [Pure]
        [ContractAnnotation("=> true, edge:notnull;=> false, edge:null")]
        bool TryGetEdge([NotNull] TVertex source, [NotNull] TVertex target, out TEdge edge);

        /// <summary>
        /// Tries to get edges that link
        /// <paramref name="source"/> and <paramref name="target"/> vertices.
        /// </summary>
        /// <param name="source">Source vertex.</param>
        /// <param name="target">Target vertex.</param>
        /// <param name="edges">Edges found, otherwise null.</param>
        /// <returns>True if at least an edge was found, false otherwise.</returns>
        [Pure]
        [ContractAnnotation("=> true, edges:notnull;=> false, edges:null")]
        bool TryGetEdges([NotNull] TVertex source, [NotNull] TVertex target, [ItemNotNull] out IEnumerable<TEdge> edges);
    }
}