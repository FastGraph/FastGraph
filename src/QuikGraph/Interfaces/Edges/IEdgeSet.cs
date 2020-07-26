using System.Collections.Generic;
using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// Represents a set of edges.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public interface IEdgeSet<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Gets a value indicating whether there are no edges in this set.
        /// It is true if this edge set is empty, otherwise false.
        /// </summary>
        bool IsEdgesEmpty { get; }

        /// <summary>
        /// Gets the edge count.
        /// </summary>
        int EdgeCount { get; }

        /// <summary>
        /// Gets the edges.
        /// </summary>
        [NotNull, ItemNotNull]
        IEnumerable<TEdge> Edges { get; }

        /// <summary>
        /// Determines whether this set contains the specified <paramref name="edge"/>.
        /// </summary>
        /// <param name="edge">Edge to check.</param>
        /// <returns>True if the specified <paramref name="edge"/> is contained in this set, false otherwise.</returns>
        [Pure]
        bool ContainsEdge([NotNull] TEdge edge);
    }
}