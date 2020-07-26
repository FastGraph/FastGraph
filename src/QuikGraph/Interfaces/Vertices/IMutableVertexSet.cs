using System.Collections.Generic;
using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// Represents a mutable set of vertices.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    public interface IMutableVertexSet<TVertex> : IVertexSet<TVertex>
    {
        /// <summary>
        /// Fired when a vertex is added to this set.
        /// </summary>
        event VertexAction<TVertex> VertexAdded;

        /// <summary>
        /// Adds a vertex to this set.
        /// </summary>
        /// <param name="vertex">Vertex to add.</param>
        /// <returns>True if the vertex was added, false otherwise.</returns>
        bool AddVertex([NotNull] TVertex vertex);

        /// <summary>
        /// Adds given vertices to this set.
        /// </summary>
        /// <param name="vertices">Vertices to add.</param>
        /// <returns>The number of vertex added.</returns>
        int AddVertexRange([NotNull, ItemNotNull] IEnumerable<TVertex> vertices);

        /// <summary>
        /// Fired when a vertex is removed from this set.
        /// </summary>
        event VertexAction<TVertex> VertexRemoved;

        /// <summary>
        /// Removes the given vertex from this set.
        /// </summary>
        /// <param name="vertex">Vertex to remove.</param>
        /// <returns>True if the vertex was removed, false otherwise.</returns>
        bool RemoveVertex([NotNull] TVertex vertex);

        /// <summary>
        /// Removes all vertices matching the given <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">Predicate to check on each vertex.</param>
        /// <returns>The number of vertex removed.</returns>
        int RemoveVertexIf([NotNull, InstantHandle] VertexPredicate<TVertex> predicate);
    }
}