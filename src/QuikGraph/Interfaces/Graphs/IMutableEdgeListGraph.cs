using System.Collections.Generic;
using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// A mutable edge list graph with vertices of type <typeparamref name="TVertex"/>
    /// and edges of type <typeparamref name="TEdge"/>.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public interface IMutableEdgeListGraph<TVertex, TEdge> : IMutableGraph<TVertex, TEdge>, IEdgeListGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Adds the <paramref name="edge"/> to this graph.
        /// </summary>
        /// <param name="edge">An edge.</param>
        /// <returns>True if the edge was added, false otherwise.</returns>
        bool AddEdge([NotNull] TEdge edge);

        /// <summary>
        /// Fired when an edge is added to this graph.
        /// </summary>
        event EdgeAction<TVertex, TEdge> EdgeAdded;

        /// <summary>
        /// Adds a set of edges to this graph.
        /// </summary>
        /// <param name="edges">Edges to add.</param>
        /// <returns>The number of edges successfully added to this graph.</returns>
        int AddEdgeRange([NotNull, ItemNotNull] IEnumerable<TEdge> edges);

        /// <summary>
        /// Removes the <paramref name="edge"/> from this graph.
        /// </summary>
        /// <param name="edge">Edge to remove.</param>
        /// <returns>True if the <paramref name="edge"/> was successfully removed, false otherwise.</returns>
        bool RemoveEdge([NotNull] TEdge edge);

        /// <summary>
        /// Fired when an edge has been removed from this graph.
        /// </summary>
        event EdgeAction<TVertex, TEdge> EdgeRemoved;

        /// <summary>
        /// Removes all edges that match the given <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">Predicate to check if an edge should be removed.</param>
        /// <returns>The number of edges removed.</returns>
        int RemoveEdgeIf([NotNull, InstantHandle] EdgePredicate<TVertex, TEdge> predicate);
    }
}