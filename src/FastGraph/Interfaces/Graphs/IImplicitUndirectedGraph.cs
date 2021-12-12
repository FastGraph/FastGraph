#nullable enable

using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace FastGraph
{
    /// <summary>
    /// An implicit undirected graph with vertices of type <typeparamref name="TVertex"/>
    /// and edges of type <typeparamref name="TEdge"/>.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public interface IImplicitUndirectedGraph<TVertex, TEdge> : IImplicitVertexSet<TVertex>, IGraph<TVertex, TEdge>
        where TVertex : notnull
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Comparer for edges.
        /// </summary>
        EdgeEqualityComparer<TVertex> EdgeEqualityComparer { get; }

        /// <summary>
        /// Gives the enumerable of edges adjacent to the given <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>Enumerable of adjacent edges.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="vertex"/> is <see langword="null"/>.</exception>
        /// <exception cref="VertexNotFoundException"><paramref name="vertex"/> is not part of the graph.</exception>
        [Pure]
        IEnumerable<TEdge> AdjacentEdges(TVertex vertex);

        /// <summary>
        /// Gives the adjacent degree of the given <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>Vertex adjacent degree.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="vertex"/> is <see langword="null"/>.</exception>
        /// <exception cref="VertexNotFoundException"><paramref name="vertex"/> is not part of the graph.</exception>
        [Pure]
        int AdjacentDegree(TVertex vertex);

        /// <summary>
        /// Indicates if the given <paramref name="vertex"/> has at least one adjacent edge.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>True if the vertex has at least one adjacent edge, false otherwise.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="vertex"/> is <see langword="null"/>.</exception>
        /// <exception cref="VertexNotFoundException"><paramref name="vertex"/> is not part of the graph.</exception>
        [Pure]
        bool IsAdjacentEdgesEmpty(TVertex vertex);

        /// <summary>
        /// Gets the <paramref name="index"/>th adjacent edge of the given <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="index">Index of the adjacent edge requested.</param>
        /// <returns>The adjacent edge.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="vertex"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">No vertex at <paramref name="index"/>.</exception>
        /// <exception cref="VertexNotFoundException"><paramref name="vertex"/> is not part of the graph.</exception>
        [Pure]
        TEdge AdjacentEdge(TVertex vertex, int index);

        /// <summary>
        /// Tries to get the edge that link
        /// <paramref name="source"/> and <paramref name="target"/> vertices.
        /// </summary>
        /// <param name="source">Source vertex.</param>
        /// <param name="target">Target vertex.</param>
        /// <param name="edge">Edge found, otherwise <see langword="null"/>.</param>
        /// <returns>True if an edge was found, false otherwise.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="target"/> is <see langword="null"/>.</exception>
        [Pure]
        [ContractAnnotation("=> true, edge:notnull;=> false, edge:null")]
        bool TryGetEdge(TVertex source, TVertex target, [NotNullWhen(true)] out TEdge? edge);

        /// <summary>
        /// Checks if this graph contains an edge that link
        /// <paramref name="source"/> and <paramref name="target"/> vertices.
        /// </summary>
        /// <param name="source">Source vertex.</param>
        /// <param name="target">Target vertex.</param>
        /// <returns>True if an edge exists, false otherwise.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="target"/> is <see langword="null"/>.</exception>
        [Pure]
        bool ContainsEdge(TVertex source, TVertex target);
    }
}
