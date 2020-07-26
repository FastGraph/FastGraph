using System.Collections.Generic;
using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// A mutable vertex and edge set.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public interface IMutableVertexAndEdgeSet<TVertex,TEdge>
        : IMutableVertexSet<TVertex>
        , IMutableEdgeListGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Adds <paramref name="edge"/> and its vertices to this graph.
        /// </summary>
        /// <param name="edge">The edge to add.</param>
        /// <returns>True if the edge was added, false otherwise.</returns>
        bool AddVerticesAndEdge([NotNull] TEdge edge);

        /// <summary>
        /// Adds a set of edges (and it's vertices if necessary).
        /// </summary>
        /// <param name="edges">Edges to add.</param>
        /// <returns>The number of edges added.</returns>
        int AddVerticesAndEdgeRange([NotNull, ItemNotNull] IEnumerable<TEdge> edges);
    }
}