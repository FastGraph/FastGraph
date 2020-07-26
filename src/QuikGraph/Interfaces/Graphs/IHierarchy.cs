using System.Collections.Generic;
using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// Represents a hierarchy of graphs.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public interface IHierarchy<TVertex, TEdge> : IMutableVertexAndEdgeListGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Gets the root of the hierarchy.
        /// </summary>
        [CanBeNull]
        TVertex Root { get; }

        /// <summary>
        /// Gets the parent vertex of the given <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>The parent vertex if there is one, otherwise null.</returns>
        /// <exception cref="System.ArgumentException">The given <paramref name="vertex"/> is the root of the graph.</exception>
        [Pure]
        [CanBeNull]
        TVertex GetParent([NotNull] TVertex vertex);

        /// <summary>
        /// Gets the parent edge of the <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>The parent vertex edge.</returns>
        /// <exception cref="System.ArgumentException">The <paramref name="vertex"/> is the root of the graph.</exception>
        [Pure]
        [CanBeNull]
        TEdge GetParentEdge([NotNull] TVertex vertex);

        /// <summary>
        /// Gets a value indicating if <paramref name="edge"/> is  a cross edge.
        /// </summary>
        /// <param name="edge">The edge.</param>
        /// <returns>True if the edge is a cross edge, false otherwise.</returns>
        [Pure]
        bool IsCrossEdge([NotNull] TEdge edge);

        /// <summary>
        /// Gets a value indicating whether the <paramref name="edge"/> 
        /// exists really or is just an induced edge.
        /// </summary>
        /// <param name="edge">The edge.</param>
        /// <returns>True if it's a real edge, false otherwise.</returns>
        [Pure]
        bool IsRealEdge([NotNull] TEdge edge);

        /// <summary>
        /// Gets a value indicating if <paramref name="source"/>
        /// is a predecessor of <paramref name="target"/>.
        /// </summary>
        /// <param name="source">Source vertex.</param>
        /// <param name="target">Target vertex.</param>
        /// <returns>True if the <paramref name="source"/> is a predecessor of <paramref name="target"/>.</returns>
        [Pure]
        bool IsPredecessorOf([NotNull] TVertex source, [NotNull] TVertex target);

        /// <summary>
        /// Gets the number of edges between the <paramref name="source"/> and <paramref name="target"/> vertex. 
        /// </summary>
        /// <param name="source">Source vertex.</param>
        /// <param name="target">Target vertex.</param>
        /// <returns>The number of edge between <paramref name="source"/> and <paramref name="target"/>.</returns>
        /// <exception cref="System.ArgumentException">The <paramref name="source"/> is a predecessor of
        /// <paramref name="target"/> or the other-way round.</exception>
        [Pure]
        int InducedEdgeCount([NotNull] TVertex source, [NotNull] TVertex target);

        /// <summary>
        /// Gets a value indicating if the <paramref name="vertex"/> is an inner node or a leaf.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>True if the <paramref name="vertex"/> is not a leaf, false otherwise.</returns>
        [Pure]
        bool IsInnerNode([NotNull] TVertex vertex);

        /// <summary>
        /// Gets the collection of children edges from the <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>Children edges.</returns>
        [Pure]
        [NotNull, ItemNotNull]
        IEnumerable<TEdge> ChildrenEdges([NotNull] TVertex vertex);

        /// <summary>
        /// Gets the collection of children vertices from the <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>Children vertices.</returns>
        [Pure]
        [NotNull, ItemNotNull]
        IEnumerable<TVertex> ChildrenVertices([NotNull] TVertex vertex);
    }
}