using System.Collections.Generic;
using JetBrains.Annotations;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
using QuikGraph.Contracts;
#endif

namespace QuikGraph
{
    /// <summary>
    /// Represents a set of edges.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_CONTRACTS
    [ContractClass(typeof(EdgeSetContract<,>))]
#endif
    public interface IEdgeSet<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Gets a value indicating whether there are no edges in this set.
        /// It is true if this edge set is empty, otherwise false.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        bool IsEdgesEmpty { get; }

        /// <summary>
        /// Gets the edge count.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        int EdgeCount { get; }

        /// <summary>
        /// Gets the edges.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull, ItemNotNull]
        IEnumerable<TEdge> Edges { get; }

        /// <summary>
        /// Determines whether this set contains the specified <paramref name="edge"/>.
        /// </summary>
        /// <param name="edge">Edge to check.</param>
        /// <returns>True if the specified <paramref name="edge"/> is contained in this set, false otherwise.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        bool ContainsEdge([NotNull] TEdge edge);
    }


}
