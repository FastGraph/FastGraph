#if SUPPORTS_CLONEABLE
using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace QuikGraph.Algorithms.Exploration
{
    /// <summary>
    /// Represents a transition factory.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_ENUMERABLE_COVARIANT
    public interface ITransitionFactory<in TVertex, out TEdge>
#else
    public interface ITransitionFactory<in TVertex, TEdge>
#endif
        where TVertex : ICloneable
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Checks if the given <paramref name="vertex"/> is valid or not.
        /// </summary>
        /// <param name="vertex">Vertex to check.</param>
        /// <returns>True if the vertex is valid, false otherwise.</returns>
        bool IsValid([NotNull] TVertex vertex);

        /// <summary>
        /// Applies the transition from the given <paramref name="source"/>.
        /// </summary>
        /// <param name="source">Source vertex.</param>
        /// <returns>Edges resulting of the apply.</returns>
        [NotNull, ItemNotNull]
        IEnumerable<TEdge> Apply([NotNull] TVertex source);
    }
}
#endif