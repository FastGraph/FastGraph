#if SUPPORTS_SERIALIZATION
using System;
#endif
using System.Collections.Generic;
using JetBrains.Annotations;

namespace QuikGraph.Algorithms.Condensation
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
        public MergedEdge(TVertex source, TVertex target)
            : base(source, target)
        {
        }

        [NotNull, ItemNotNull]
        private List<TEdge> _edges = new List<TEdge>();

        /// <summary>
        /// Edges merged.
        /// </summary>
        [NotNull, ItemNotNull]
        public IList<TEdge> Edges => _edges;

        /// <summary>
        /// Merges the given two edges.
        /// </summary>
        /// <param name="inEdge">First edge.</param>
        /// <param name="outEdge">Second edge.</param>
        /// <returns>The merged edge.</returns>
        public static MergedEdge<TVertex, TEdge> Merge(
            [NotNull] MergedEdge<TVertex, TEdge> inEdge,
            [NotNull] MergedEdge<TVertex, TEdge> outEdge)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(inEdge != null);
            Contract.Requires(outEdge != null);
#endif

            var newEdge = new MergedEdge<TVertex, TEdge>(inEdge.Source, outEdge.Target)
            {
                _edges = new List<TEdge>(inEdge.Edges.Count + outEdge.Edges.Count)
            };
            newEdge._edges.AddRange(inEdge.Edges);
            newEdge._edges.AddRange(outEdge._edges);

            return newEdge;
        }
    }
}
