using System.Collections.Generic;
using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// An implicit undirected graph with vertices of type <typeparamref name="TVertex"/>
    /// and edges of type <typeparamref name="TEdge"/>.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public interface IImplicitUndirectedGraph<TVertex, TEdge> : IImplicitVertexSet<TVertex>, IGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Comparer for edges.
        /// </summary>
        [NotNull]
        EdgeEqualityComparer<TVertex> EdgeEqualityComparer { get; }

        /// <summary>
        /// Gives the enumerable of edges adjacent to the given <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>Enumerable of adjacent edges.</returns>
        [Pure]
        [NotNull, ItemNotNull]
        IEnumerable<TEdge> AdjacentEdges([NotNull] TVertex vertex);

        /// <summary>
        /// Gives the adjacent degree of the given <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>Vertex adjacent degree.</returns>
        [Pure]
        int AdjacentDegree([NotNull] TVertex vertex);

        /// <summary>
        /// Indicates if the given <paramref name="vertex"/> has at least one adjacent edge.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>True if the vertex has at least one adjacent edge, false otherwise.</returns>
        [Pure]
        bool IsAdjacentEdgesEmpty([NotNull] TVertex vertex);

        /// <summary>
        /// Gets the <paramref name="index"/>th adjacent edge of the given <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="index">Index of the adjacent edge requested.</param>
        /// <returns>The adjacent edge.</returns>
        [Pure]
        [NotNull]
        TEdge AdjacentEdge([NotNull] TVertex vertex, int index);

        /// <summary>
        /// Tries to get the edge that link
        /// <paramref name="source"/> and <paramref name="target"/> vertices.
        /// </summary>
        /// <param name="source">Source vertex.</param>
        /// <param name="target">Target vertex.</param>
        /// <param name="edge">Edge found, otherwise null.</param>
        /// <returns>True if an edge was found, false otherwise.</returns>
        [Pure]
        [ContractAnnotation("=> true, edge:notnull;=> false, edge:null")]
        bool TryGetEdge([NotNull] TVertex source, [NotNull] TVertex target, out TEdge edge);

        /// <summary>
        /// Checks if this graph contains an edge that link
        /// <paramref name="source"/> and <paramref name="target"/> vertices.
        /// </summary>
        /// <param name="source">Source vertex.</param>
        /// <param name="target">Target vertex.</param>
        /// <returns>True if an edge exists, false otherwise.</returns>
        [Pure]
        bool ContainsEdge([NotNull] TVertex source, [NotNull] TVertex target);
    }
}