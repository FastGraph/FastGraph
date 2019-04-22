using System;
using System.Collections.Generic;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// Helpers to load graph from dot string.
    /// </summary>
    public static class DotGraphLoader
    {
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        [NotNull]
        private static TGraph LoadGraphFromDot<TVertex, TEdge, TGraph>(
            [NotNull] string dotSource,
            [NotNull, InstantHandle] Func<bool, TGraph> createGraph,
            [NotNull, InstantHandle] Func<string, IDictionary<string, string>, TVertex> vertexFactory,
            [NotNull, InstantHandle] Func<TVertex, TVertex, IDictionary<string, string>, TEdge> edgeFactory)
            where TEdge : IEdge<TVertex>
            where TGraph : IMutableVertexAndEdgeSet<TVertex, TEdge>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(dotSource != null);
            Contract.Requires(createGraph != null);
            Contract.Requires(vertexFactory != null);
            Contract.Requires(edgeFactory != null);
#endif

            return (TGraph)DotParserAdapter.LoadDot(
                dotSource,
                allowParallelEdges => createGraph(allowParallelEdges),
                vertexFactory,
                edgeFactory);
        }

        /// <summary>
        /// Loads an <see cref="AdjacencyGraph{TVertex,TEdge}"/> from a dot string.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="dotSource">Dot string representing a graph.</param>
        /// <param name="vertexFactory">Vertex factory method.</param>
        /// <param name="edgeFactory">Edge factory method.</param>
        /// <returns>A corresponding <see cref="AdjacencyGraph{TVertex,TEdge}"/>.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        [NotNull]
        public static AdjacencyGraph<TVertex, TEdge> LoadAdjacencyGraphFromDot<TVertex, TEdge>(
            [NotNull] string dotSource,
            [NotNull, InstantHandle] Func<string, IDictionary<string, string>, TVertex> vertexFactory,
            [NotNull, InstantHandle] Func<TVertex, TVertex, IDictionary<string, string>, TEdge> edgeFactory)
            where TEdge : IEdge<TVertex>
        {
            return LoadGraphFromDot(
                dotSource,
                allowParallelEdges => new AdjacencyGraph<TVertex, TEdge>(allowParallelEdges), 
                vertexFactory, 
                edgeFactory);
        }

        /// <summary>
        /// Loads an <see cref="UndirectedGraph{TVertex,TEdge}"/> from a dot string.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="dotSource">Dot string representing a graph.</param>
        /// <param name="vertexFactory">Vertex factory method.</param>
        /// <param name="edgeFactory">Edge factory method.</param>
        /// <returns>A corresponding <see cref="UndirectedGraph{TVertex,TEdge}"/>.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        [NotNull]
        public static UndirectedGraph<TVertex, TEdge> LoadUndirectedGraphFromDot<TVertex, TEdge>(
            [NotNull] string dotSource,
            [NotNull, InstantHandle] Func<string, IDictionary<string, string>, TVertex> vertexFactory,
            [NotNull, InstantHandle] Func<TVertex, TVertex, IDictionary<string, string>, TEdge> edgeFactory)
            where TEdge : IEdge<TVertex>
        {
            return LoadGraphFromDot(
                dotSource,
                allowParallelEdges => new UndirectedGraph<TVertex, TEdge>(allowParallelEdges),
                vertexFactory,
                edgeFactory);
        }

        /// <summary>
        /// Loads an <see cref="BidirectionalGraph{TVertex,TEdge}"/> from a dot string.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="dotSource">Dot string representing a graph.</param>
        /// <param name="vertexFactory">Vertex factory method.</param>
        /// <param name="edgeFactory">Edge factory method.</param>
        /// <returns>A corresponding <see cref="BidirectionalGraph{TVertex,TEdge}"/>.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        [NotNull]
        public static BidirectionalGraph<TVertex, TEdge> LoadBidirectionalGraphFromDot<TVertex, TEdge>(
            [NotNull] string dotSource,
            [NotNull, InstantHandle] Func<string, IDictionary<string, string>, TVertex> vertexFactory,
            [NotNull, InstantHandle] Func<TVertex, TVertex, IDictionary<string, string>, TEdge> edgeFactory)
            where TEdge : IEdge<TVertex>
        {
            return LoadGraphFromDot(
                dotSource,
                allowParallelEdges => new BidirectionalGraph<TVertex, TEdge>(allowParallelEdges),
                vertexFactory,
                edgeFactory);
        }
    }
}