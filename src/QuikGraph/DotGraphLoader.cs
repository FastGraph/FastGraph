using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// Helpers to load graph from dot string.
    /// </summary>
    public static class DotGraphLoader
    {
        /// <summary>
        /// Loads an <see cref="UndirectedGraph{TVertex,TEdge}"/> from a dot string.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="dotSource">Dot string representing a graph.</param>
        /// <param name="vertexFactory">Vertex factory method.</param>
        /// <param name="edgeFactory">Edge factory method.</param>
        /// <returns>A corresponding <see cref="UndirectedGraph{TVertex,TEdge}"/>.</returns>
        public static UndirectedGraph<TVertex, TEdge> LoadUndirectedGraphFromDot<TVertex, TEdge>(
            string dotSource,
            [NotNull, InstantHandle] Func<string, IDictionary<string, string>, TVertex> vertexFactory,
            [NotNull, InstantHandle] Func<TVertex, TVertex, IDictionary<string, string>, TEdge> edgeFactory)
            where TEdge : IEdge<TVertex>
        {
            return (UndirectedGraph<TVertex, TEdge>)DotParserAdapter.LoadDot(
                dotSource, 
                CreateGraph, 
                vertexFactory, 
                edgeFactory);

            #region Local function

            IMutableVertexAndEdgeSet<TVertex, TEdge> CreateGraph(bool allowParallelEdges)
            {
                return new UndirectedGraph<TVertex, TEdge>(allowParallelEdges);
            }

            #endregion
        }
    }
}