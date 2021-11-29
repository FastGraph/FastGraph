#if SUPPORTS_SERIALIZATION
using System;
#endif
using System.Collections.Generic;
using JetBrains.Annotations;

namespace FastGraph.Algorithms.Condensation
{
    /// <summary>
    /// An edge connecting two graphs.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    /// <typeparam name="TGraph">Graph type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class CondensedEdge<TVertex, TEdge, TGraph> : Edge<TGraph>
        where TEdge : IEdge<TVertex>
        where TGraph : IMutableVertexAndEdgeSet<TVertex, TEdge>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CondensedEdge{TVertex,TEdge,TGraph}"/> class.
        /// </summary>
        /// <param name="source">The source graph.</param>
        /// <param name="target">The target graph.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="target"/> is <see langword="null"/>.</exception>
        public CondensedEdge([NotNull] TGraph source, [NotNull] TGraph target)
            : base(source, target)
        {
        }

        /// <summary>
        /// Edges between source and target graphs.
        /// </summary>
        [NotNull, ItemNotNull]
        public IList<TEdge> Edges { get; } = new List<TEdge>();
    }
}