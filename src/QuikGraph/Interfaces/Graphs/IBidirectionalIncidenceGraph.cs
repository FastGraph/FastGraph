using System.Collections.Generic;
using JetBrains.Annotations;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
using QuikGraph.Contracts;
#endif

namespace QuikGraph
{
    /// <summary>
    /// A incident directed graph with vertices of type <typeparamref name="TVertex"/>
    /// and edges of type <typeparamref name="TEdge"/>, that is efficient
    /// to traverse both in and out edges.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_CONTRACTS
    [ContractClass(typeof(BidirectionalIncidenceGraphContract<,>))]
#endif
    public interface IBidirectionalIncidenceGraph<TVertex, TEdge> : IIncidenceGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Determines whether <paramref name="vertex"/> has no in-edges.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>True if <paramref name="vertex"/> has no in-edges, false otherwise.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        bool IsInEdgesEmpty([NotNull] TVertex vertex);

        /// <summary>
        /// Gets the number of in-edges of <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>The number of in-edges pointing towards <paramref name="vertex"/>.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        int InDegree([NotNull] TVertex vertex);

        /// <summary>
        /// Gets the collection of in-edges of <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>The collection of in-edges of <paramref name="vertex"/>.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        [NotNull, ItemNotNull]
        IEnumerable<TEdge> InEdges([NotNull] TVertex vertex);

        /// <summary>
        /// Tries to get the in-edges of <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="edges">In edges.</param>
        /// <returns>True if in-edges were found, false otherwise.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        bool TryGetInEdges([NotNull] TVertex vertex, [ItemNotNull] out IEnumerable<TEdge> edges);

        /// <summary>
        /// Gets the in-edge at location <paramref name="index"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="index">The index.</param>
        /// <returns>The in-edge at position <paramref name="index"/>.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        [NotNull]
        TEdge InEdge([NotNull] TVertex vertex, int index);

        /// <summary>
        /// Gets the degree of <paramref name="vertex"/>, i.e.
        /// the sum of the out-degree and in-degree of <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>The sum of OutDegree and InDegree of <paramref name="vertex"/>.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        int Degree([NotNull] TVertex vertex);
    }
}
