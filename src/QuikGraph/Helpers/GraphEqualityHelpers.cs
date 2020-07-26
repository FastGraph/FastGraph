using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// Equality helpers for graphs.
    /// </summary>
    public static class EquateGraphs
    {
        /// <summary>
        /// Checks if both graphs <paramref name="g"/> and <paramref name="h"/> content are equal.
        /// Uses the provided <paramref name="vertexEquality"/> and <paramref name="edgeEquality"/>
        /// comparer to respectively compare vertices and edges.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="g">First graph to compare.</param>
        /// <param name="h">Second graph to compare.</param>
        /// <param name="vertexEquality">Vertex equality comparer.</param>
        /// <param name="edgeEquality">Edge equality comparer.</param>
        /// <returns>True if both graphs are equal, false otherwise.</returns>
        [Pure]
        public static bool Equate<TVertex, TEdge>(
            [CanBeNull] IEdgeListGraph<TVertex, TEdge> g,
            [CanBeNull] IEdgeListGraph<TVertex, TEdge> h,
            [NotNull] IEqualityComparer<TVertex> vertexEquality,
            [NotNull] IEqualityComparer<TEdge> edgeEquality)
            where TEdge : IEdge<TVertex>
        {
            if (vertexEquality is null)
                throw new ArgumentNullException(nameof(vertexEquality));
            if (edgeEquality is null)
                throw new ArgumentNullException(nameof(edgeEquality));

            if (g is null)
                return h is null;
            if (h is null)
                return false;

            if (ReferenceEquals(g, h))
                return true;

            if (g.VertexCount != h.VertexCount)
                return false;

            if (g.IsDirected != h.IsDirected)
                return false;

            if (g.EdgeCount != h.EdgeCount)
                return false;

            foreach (TVertex vertex in g.Vertices)
            {
                if (!h.Vertices.Any(v => vertexEquality.Equals(v, vertex)))
                    return false;
            }

            foreach (TEdge edge in g.Edges)
            {
                if (!h.Edges.Any(e => edgeEquality.Equals(e, edge)))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if both graphs <paramref name="g"/> and <paramref name="h"/> content are equal.
        /// Uses the default comparer for vertices and edges.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="g">First graph to compare.</param>
        /// <param name="h">Second graph to compare.</param>
        /// <returns>True if both graphs are equal, false otherwise.</returns>
        [Pure]
        public static bool Equate<TVertex, TEdge>(
            [CanBeNull] IEdgeListGraph<TVertex, TEdge> g,
            [CanBeNull] IEdgeListGraph<TVertex, TEdge> h)
            where TEdge : IEdge<TVertex>
        {
            return Equate(g, h, EqualityComparer<TVertex>.Default, EqualityComparer<TEdge>.Default);
        }
    }
}