﻿using JetBrains.Annotations;

namespace FastGraph
{
    /// <summary>
    /// Represents an implicit set of vertices.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    public interface IImplicitVertexSet<in TVertex>
    {
        /// <summary>
        /// Determines whether this set contains the specified <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">Vertex to check.</param>
        /// <returns>True if the specified <paramref name="vertex"/> is contained in this set, false otherwise.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="vertex"/> is <see langword="null"/>.</exception>
        [Pure]
        bool ContainsVertex([NotNull] TVertex vertex);
    }
}
