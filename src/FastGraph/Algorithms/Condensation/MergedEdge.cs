using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace FastGraph.Algorithms.Condensation
{
    /// <summary>
    /// An edge that merge several other edges.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class MergedEdge<TVertex, TEdge> : Edge<TVertex>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MergedEdge{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <param name="target">The target vertex.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="target"/> is <see langword="null"/>.</exception>
        public MergedEdge([NotNull] TVertex source, [NotNull] TVertex target)
            : base(source, target)
        {
        }

        [NotNull, ItemNotNull]
        private List<TEdge> _edges = new List<TEdge>();

        /// <summary>
        /// Merged edges.
        /// </summary>
        [NotNull, ItemNotNull]
        public IList<TEdge> Edges => _edges;

        /// <summary>
        /// Merges the given two edges.
        /// </summary>
        /// <param name="inEdge">First edge.</param>
        /// <param name="outEdge">Second edge.</param>
        /// <returns>The merged edge.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="inEdge"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="outEdge"/> is <see langword="null"/>.</exception>
        [Pure]
        [NotNull]
        public static MergedEdge<TVertex, TEdge> Merge(
            [NotNull] MergedEdge<TVertex, TEdge> inEdge,
            [NotNull] MergedEdge<TVertex, TEdge> outEdge)
        {
            if (inEdge is null)
                throw new ArgumentNullException(nameof(inEdge));
            if (outEdge is null)
                throw new ArgumentNullException(nameof(outEdge));

            var newEdge = new MergedEdge<TVertex, TEdge>(inEdge.Source, outEdge.Target)
            {
                _edges = new List<TEdge>(inEdge.Edges.Count + outEdge.Edges.Count)
            };
            newEdge._edges.AddRange(inEdge._edges);
            newEdge._edges.AddRange(outEdge._edges);

            return newEdge;
        }
    }

    /// <summary>
    /// Helpers for <see cref="MergedEdge{TVertex,TEdge}"/>.
    /// </summary>
    public static class MergedEdge
    {
        /// <inheritdoc cref="MergedEdge{TVertex,TEdge}.Merge"/>
        [Pure]
        [NotNull]
        public static MergedEdge<TVertex, TEdge> Merge<TVertex, TEdge>(
            [NotNull] MergedEdge<TVertex, TEdge> inEdge,
            [NotNull] MergedEdge<TVertex, TEdge> outEdge)
            where TEdge : IEdge<TVertex>
        {
            return MergedEdge<TVertex, TEdge>.Merge(inEdge, outEdge);
        }
    }
}