﻿using System.Collections.Generic;
using JetBrains.Annotations;

namespace FastGraph.Algorithms.RandomWalks
{
    /// <summary>
    /// Set of edges forming chain of edges.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public interface IEdgeChain<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Tries to get the successor of the given <paramref name="vertex"/> in the given <paramref name="graph"/>.
        /// </summary>
        /// <param name="graph">The graph to search in.</param>
        /// <param name="vertex">The vertex.</param>
        /// <param name="successor">Found successor, otherwise <see langword="null"/>.</param>
        /// <returns>True if a successor was found, false otherwise.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="graph"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="vertex"/> is <see langword="null"/>.</exception>
        [Pure]
        [ContractAnnotation("=> true, successor:notnull;=> false, successor:null")]
        bool TryGetSuccessor([NotNull] IImplicitGraph<TVertex, TEdge> graph, [NotNull] TVertex vertex, out TEdge successor);

        /// <summary>
        /// Tries to get the successor of the given <paramref name="vertex"/> in the given set of <paramref name="edges"/>.
        /// </summary>
        /// <param name="edges">Edge set in which searching.</param>
        /// <param name="vertex">The vertex.</param>
        /// <param name="successor">Found successor, otherwise <see langword="null"/>.</param>
        /// <returns>True if a successor was found, false otherwise.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="edges"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="vertex"/> is <see langword="null"/>.</exception>
        [Pure]
        [ContractAnnotation("=> true, successor:notnull;=> false, successor:null")]
        bool TryGetSuccessor([NotNull, ItemNotNull] IEnumerable<TEdge> edges, [NotNull] TVertex vertex, out TEdge successor);
    }
}