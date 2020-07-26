using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using static QuikGraph.QuikGraphHelpers;

namespace QuikGraph
{
    /// <summary>
    /// Extensions for populating graph data structures.
    /// </summary>
    public static class GraphExtensions
    {
        #region Delegate graphs

        /// <summary>
        /// Creates an instance of <see cref="DelegateIncidenceGraph{TVertex,TEdge}"/> from this getter of out-edges.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="tryGetOutEdges">Getter of out-edges.</param>
        /// <returns>A corresponding <see cref="DelegateIncidenceGraph{TVertex,TEdge}"/>.</returns>
        [Pure]
        [NotNull]
        public static DelegateIncidenceGraph<TVertex, TEdge> ToDelegateIncidenceGraph<TVertex, TEdge>(
            [NotNull] this TryFunc<TVertex, IEnumerable<TEdge>> tryGetOutEdges)
            where TEdge : IEdge<TVertex>
        {
            return new DelegateIncidenceGraph<TVertex, TEdge>(tryGetOutEdges);
        }

        /// <summary>
        /// Creates an instance of <see cref="DelegateIncidenceGraph{TVertex,TEdge}"/> from this getter of out-edges.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="getOutEdges">Getter of out-edges.</param>
        /// <returns>A corresponding <see cref="DelegateIncidenceGraph{TVertex,TEdge}"/>.</returns>
        [Pure]
        [NotNull]
        public static DelegateIncidenceGraph<TVertex, TEdge> ToDelegateIncidenceGraph<TVertex, TEdge>(
            [NotNull] this Func<TVertex, IEnumerable<TEdge>> getOutEdges)
            where TEdge : IEdge<TVertex>
        {
            if (getOutEdges is null)
                throw new ArgumentNullException(nameof(getOutEdges));
            return ToDelegateIncidenceGraph(ToTryFunc(getOutEdges));
        }

        /// <summary>
        /// Wraps a dictionary into a vertex and edge list graph.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <typeparam name="TEdges">Type of the enumerable of out-edges.</typeparam>
        /// <param name="dictionary">Vertices and edges mapping.</param>
        /// <returns>A corresponding <see cref="DelegateVertexAndEdgeListGraph{TVertex,TEdge}"/>.</returns>
        [Pure]
        [NotNull]
        public static DelegateVertexAndEdgeListGraph<TVertex, TEdge> ToDelegateVertexAndEdgeListGraph<TVertex, TEdge, TEdges>(
            [NotNull] this IDictionary<TVertex, TEdges> dictionary)
            where TEdge : IEdge<TVertex>
            where TEdges : IEnumerable<TEdge>
        {
            return ToDelegateVertexAndEdgeListGraph(dictionary, kv => kv.Value);
        }

        /// <summary>
        /// Wraps a dictionary into a <see cref="DelegateVertexAndEdgeListGraph{TVertex,TEdge}"/> with the given edge conversion to get edges.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <typeparam name="TValue">Type of the enumerable of out-edges.</typeparam>
        /// <param name="dictionary">Vertices and edges mapping.</param>
        /// <param name="keyValueToOutEdges">Converter of vertex/edge mapping to enumerable of edges.</param>
        /// <returns>A corresponding <see cref="DelegateVertexAndEdgeListGraph{TVertex,TEdge}"/>.</returns>
        [Pure]
        [NotNull]
        public static DelegateVertexAndEdgeListGraph<TVertex, TEdge> ToDelegateVertexAndEdgeListGraph<TVertex, TEdge, TValue>(
            [NotNull] this IDictionary<TVertex, TValue> dictionary,
#if SUPPORTS_CONVERTER
            [NotNull] Converter<KeyValuePair<TVertex, TValue>, IEnumerable<TEdge>> keyValueToOutEdges)
#else
            [NotNull] Func<KeyValuePair<TVertex,TValue>, IEnumerable<TEdge>> keyValueToOutEdges)
#endif
            where TEdge : IEdge<TVertex>
        {
            if (dictionary is null)
                throw new ArgumentNullException(nameof(dictionary));
            if (keyValueToOutEdges is null)
                throw new ArgumentNullException(nameof(keyValueToOutEdges));

            return new DelegateVertexAndEdgeListGraph<TVertex, TEdge>(
                dictionary.Keys,
                (TVertex key, out IEnumerable<TEdge> edges) =>
                {
                    if (dictionary.TryGetValue(key, out TValue value))
                    {
                        edges = keyValueToOutEdges(new KeyValuePair<TVertex, TValue>(key, value));
                        return true;
                    }

                    edges = null;
                    return false;
                });
        }

        /// <summary>
        /// Creates an instance of <see cref="DelegateVertexAndEdgeListGraph{TVertex,TEdge}"/>
        /// from given vertices and edge try getter.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="vertices">Enumerable of vertices.</param>
        /// <param name="tryGetOutEdges">Getter of out-edges.</param>
        /// <returns>A corresponding <see cref="DelegateVertexAndEdgeListGraph{TVertex,TEdge}"/>.</returns>
        [Pure]
        [NotNull]
        public static DelegateVertexAndEdgeListGraph<TVertex, TEdge> ToDelegateVertexAndEdgeListGraph<TVertex, TEdge>(
            [NotNull, ItemNotNull] this IEnumerable<TVertex> vertices,
            [NotNull] TryFunc<TVertex, IEnumerable<TEdge>> tryGetOutEdges)
            where TEdge : IEdge<TVertex>
        {
            return new DelegateVertexAndEdgeListGraph<TVertex, TEdge>(vertices, tryGetOutEdges);
        }

        /// <summary>
        /// Creates an instance of <see cref="DelegateVertexAndEdgeListGraph{TVertex,TEdge}"/>
        /// from given vertices and edge getter.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="vertices">Enumerable of vertices.</param>
        /// <param name="getOutEdges">Getter of out-edges.</param>
        /// <returns>A corresponding <see cref="DelegateVertexAndEdgeListGraph{TVertex,TEdge}"/>.</returns>
        [Pure]
        [NotNull]
        public static DelegateVertexAndEdgeListGraph<TVertex, TEdge> ToDelegateVertexAndEdgeListGraph<TVertex, TEdge>(
            [NotNull, ItemNotNull] this IEnumerable<TVertex> vertices,
            [NotNull] Func<TVertex, IEnumerable<TEdge>> getOutEdges)
            where TEdge : IEdge<TVertex>
        {
            if (getOutEdges is null)
                throw new ArgumentNullException(nameof(getOutEdges));
            return ToDelegateVertexAndEdgeListGraph(vertices, ToTryFunc(getOutEdges));
        }

        /// <summary>
        /// Creates an instance of <see cref="DelegateBidirectionalIncidenceGraph{TVertex,TEdge}"/>
        /// from these getters of edges.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="tryGetOutEdges">Getter of out-edges.</param>
        /// <param name="tryGetInEdges">Getter of in-edges.</param>
        /// <returns>A corresponding <see cref="DelegateBidirectionalIncidenceGraph{TVertex,TEdge}"/>.</returns>
        [Pure]
        [NotNull]
        public static DelegateBidirectionalIncidenceGraph<TVertex, TEdge> ToDelegateBidirectionalIncidenceGraph<TVertex, TEdge>(
            [NotNull] this TryFunc<TVertex, IEnumerable<TEdge>> tryGetOutEdges,
            [NotNull] TryFunc<TVertex, IEnumerable<TEdge>> tryGetInEdges)
            where TEdge : IEdge<TVertex>
        {
            return new DelegateBidirectionalIncidenceGraph<TVertex, TEdge>(tryGetOutEdges, tryGetInEdges);
        }

        /// <summary>
        /// Creates an instance of <see cref="DelegateUndirectedGraph{TVertex,TEdge}"/>
        /// from given vertices and edge getter.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="vertices">Enumerable of vertices.</param>
        /// <param name="tryGetAdjacentEdges">Getter of adjacent edges.</param>
        /// <returns>A corresponding <see cref="DelegateUndirectedGraph{TVertex,TEdge}"/>.</returns>
        [Pure]
        [NotNull]
        public static DelegateUndirectedGraph<TVertex, TEdge> ToDelegateUndirectedGraph<TVertex, TEdge>(
            [NotNull, ItemNotNull] this IEnumerable<TVertex> vertices,
            [NotNull] TryFunc<TVertex, IEnumerable<TEdge>> tryGetAdjacentEdges)
            where TEdge : IEdge<TVertex>
        {
            return new DelegateUndirectedGraph<TVertex, TEdge>(vertices, tryGetAdjacentEdges);
        }

        /// <summary>
        /// Creates an instance of <see cref="DelegateUndirectedGraph{TVertex,TEdge}"/>
        /// from given vertices and edge getter.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="vertices">Enumerable of vertices.</param>
        /// <param name="getAdjacentEdges">Getter of adjacent edges.</param>
        /// <returns>A corresponding <see cref="DelegateUndirectedGraph{TVertex,TEdge}"/>.</returns>
        [Pure]
        [NotNull]
        public static DelegateUndirectedGraph<TVertex, TEdge> ToDelegateUndirectedGraph<TVertex, TEdge>(
            [NotNull, ItemNotNull] this IEnumerable<TVertex> vertices,
            [NotNull] Func<TVertex, IEnumerable<TEdge>> getAdjacentEdges)
            where TEdge : IEdge<TVertex>
        {
            if (getAdjacentEdges is null)
                throw new ArgumentNullException(nameof(getAdjacentEdges));
            return ToDelegateUndirectedGraph(vertices, ToTryFunc(getAdjacentEdges));
        }

        #endregion

        #region Graphs

        /// <summary>
        /// Converts a raw array of sources and targets (2 columns) vertices into a graph.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <param name="edges">
        /// Array of vertices defining edges.
        /// The first items of each column represents the number of vertices following.
        /// </param>
        /// <returns>A corresponding <see cref="AdjacencyGraph{TVertex,TEdge}"/>.</returns>
        [Pure]
        [NotNull]
        public static AdjacencyGraph<TVertex, SEquatableEdge<TVertex>> ToAdjacencyGraph<TVertex>(
            [NotNull] this TVertex[][] edges)
        {
            if (edges is null)
                throw new ArgumentNullException(nameof(edges));
            if (edges.Length != 2)
                throw new ArgumentException("Must have a length of 2.", nameof(edges));
            if (edges[0] is null)
                throw new ArgumentNullException(nameof(edges));
            if (edges[1] is null)
                throw new ArgumentNullException(nameof(edges));
            if (edges[0].Length != edges[1].Length)
                throw new ArgumentException("Edges columns must have same size.");

            TVertex[] sources = edges[0];
            TVertex[] targets = edges[1];
            int n = sources.Length;
            var edgePairs = new List<SEquatableEdge<TVertex>>(n);
            for (int i = 0; i < n; ++i)
                edgePairs.Add(new SEquatableEdge<TVertex>(sources[i], targets[i]));

            return ToAdjacencyGraph(edgePairs);
        }

        /// <summary>
        /// Converts a set of edges into an <see cref="AdjacencyGraph{TVertex,TEdge}"/>.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="edges">Set of edges to convert.</param>
        /// <param name="allowParallelEdges">Indicates if parallel edges are allowed.</param>
        /// <returns>A corresponding <see cref="AdjacencyGraph{TVertex,TEdge}"/>.</returns>
        [Pure]
        [NotNull]
        public static AdjacencyGraph<TVertex, TEdge> ToAdjacencyGraph<TVertex, TEdge>(
            [NotNull, ItemNotNull] this IEnumerable<TEdge> edges,
            bool allowParallelEdges = true)
            where TEdge : IEdge<TVertex>
        {
            var graph = new AdjacencyGraph<TVertex, TEdge>(allowParallelEdges);
            graph.AddVerticesAndEdgeRange(edges);
            return graph;
        }

        /// <summary>
        /// Converts a set of vertex pairs into an <see cref="AdjacencyGraph{TVertex,TEdge}"/>.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <param name="vertexPairs">Set of vertex pairs to convert.</param>
        /// <returns>A corresponding <see cref="AdjacencyGraph{TVertex,TEdge}"/>.</returns>
        [Pure]
        [NotNull]
        public static AdjacencyGraph<TVertex, SEquatableEdge<TVertex>> ToAdjacencyGraph<TVertex>(
            [NotNull] this IEnumerable<SEquatableEdge<TVertex>> vertexPairs)
        {
            if (vertexPairs is null)
                throw new ArgumentNullException(nameof(vertexPairs));

            var graph = new AdjacencyGraph<TVertex, SEquatableEdge<TVertex>>();
            graph.AddVerticesAndEdgeRange(vertexPairs);
            return graph;
        }

        /// <summary>
        /// Converts a set of vertices into an <see cref="AdjacencyGraph{TVertex,TEdge}"/>
        /// using an edge factory.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="vertices">Set of vertices to convert.</param>
        /// <param name="outEdgesFactory">The out edges factory.</param>
        /// <param name="allowParallelEdges">Indicates if parallel edges are allowed.</param>
        /// <returns>A corresponding <see cref="AdjacencyGraph{TVertex,TEdge}"/>.</returns>
        [Pure]
        [NotNull]
        public static AdjacencyGraph<TVertex, TEdge> ToAdjacencyGraph<TVertex, TEdge>(
            [NotNull, ItemNotNull] this IEnumerable<TVertex> vertices,
            [NotNull, InstantHandle] Func<TVertex, IEnumerable<TEdge>> outEdgesFactory,
            bool allowParallelEdges = true)
            where TEdge : IEdge<TVertex>
        {
            if (outEdgesFactory is null)
                throw new ArgumentNullException(nameof(outEdgesFactory));

            var graph = new AdjacencyGraph<TVertex, TEdge>(allowParallelEdges);
            graph.AddVertexRange(vertices);

            foreach (TVertex vertex in graph.Vertices)
                graph.AddEdgeRange(outEdgesFactory(vertex));

            return graph;
        }

        /// <summary>
        /// Creates an immutable <see cref="ArrayAdjacencyGraph{TVertex,TEdge}"/> from the input graph.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="graph">Graph to convert.</param>
        /// <returns>A corresponding <see cref="ArrayAdjacencyGraph{TVertex,TEdge}"/>.</returns>
        [Pure]
        [NotNull]
        public static ArrayAdjacencyGraph<TVertex, TEdge> ToArrayAdjacencyGraph<TVertex, TEdge>(
            [NotNull] this IVertexAndEdgeListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            return new ArrayAdjacencyGraph<TVertex, TEdge>(graph);
        }

        /// <summary>
        /// Wraps a graph (out-edges only) into a bidirectional graph.
        /// </summary>
        /// <remarks>For already bidirectional graph it returns itself.</remarks>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="graph">Graph to convert.</param>
        /// <returns>A corresponding <see cref="IBidirectionalGraph{TVertex,TEdge}"/>.</returns>
        [Pure]
        [NotNull]
        public static IBidirectionalGraph<TVertex, TEdge> ToBidirectionalGraph<TVertex, TEdge>(
            [NotNull] this IVertexAndEdgeListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            if (graph is null)
                throw new ArgumentNullException(nameof(graph));

            if (graph is IBidirectionalGraph<TVertex, TEdge> self)
                return self;

            return new BidirectionalAdapterGraph<TVertex, TEdge>(graph);
        }

        /// <summary>
        /// Converts a set of edges into a <see cref="BidirectionalGraph{TVertex,TEdge}"/>.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="edges">Set of edges to convert.</param>
        /// <param name="allowParallelEdges">Indicates if parallel edges are allowed.</param>
        /// <returns>A corresponding <see cref="BidirectionalGraph{TVertex,TEdge}"/>.</returns>
        [Pure]
        [NotNull]
        public static BidirectionalGraph<TVertex, TEdge> ToBidirectionalGraph<TVertex, TEdge>(
            [NotNull, ItemNotNull] this IEnumerable<TEdge> edges,
            bool allowParallelEdges = true)
            where TEdge : IEdge<TVertex>
        {
            var graph = new BidirectionalGraph<TVertex, TEdge>(allowParallelEdges);
            graph.AddVerticesAndEdgeRange(edges);
            return graph;
        }

        /// <summary>
        /// Converts a set of vertex pairs into a <see cref="BidirectionalGraph{TVertex,TEdge}"/>.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <param name="vertexPairs">Set of vertex pairs to convert.</param>
        /// <returns>A corresponding <see cref="BidirectionalGraph{TVertex,TEdge}"/>.</returns>
        [Pure]
        [NotNull]
        public static BidirectionalGraph<TVertex, SEquatableEdge<TVertex>> ToBidirectionalGraph<TVertex>(
            [NotNull] this IEnumerable<SEquatableEdge<TVertex>> vertexPairs)
        {
            if (vertexPairs is null)
                throw new ArgumentNullException(nameof(vertexPairs));

            var graph = new BidirectionalGraph<TVertex, SEquatableEdge<TVertex>>();
            graph.AddVerticesAndEdgeRange(vertexPairs);
            return graph;
        }

        /// <summary>
        /// Converts a set of vertices into a <see cref="BidirectionalGraph{TVertex,TEdge}"/>
        /// using an edge factory.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="vertices">Set of vertices to convert.</param>
        /// <param name="outEdgesFactory">The out edges factory.</param>
        /// <param name="allowParallelEdges">Indicates if parallel edges are allowed.</param>
        /// <returns>A corresponding <see cref="BidirectionalGraph{TVertex,TEdge}"/>.</returns>
        [Pure]
        [NotNull]
        public static BidirectionalGraph<TVertex, TEdge> ToBidirectionalGraph<TVertex, TEdge>(
            [NotNull, ItemNotNull] this IEnumerable<TVertex> vertices,
            [NotNull, InstantHandle] Func<TVertex, IEnumerable<TEdge>> outEdgesFactory,
            bool allowParallelEdges = true)
            where TEdge : IEdge<TVertex>
        {
            if (outEdgesFactory is null)
                throw new ArgumentNullException(nameof(outEdgesFactory));

            var graph = new BidirectionalGraph<TVertex, TEdge>(allowParallelEdges);
            graph.AddVertexRange(vertices);

            foreach (TVertex vertex in graph.Vertices)
                graph.AddEdgeRange(outEdgesFactory(vertex));

            return graph;
        }

        /// <summary>
        /// Creates a <see cref="BidirectionalGraph{TVertex,TEdge}"/> from this graph.
        /// </summary>
        /// <param name="graph">Graph to convert.</param>
        /// <returns>A corresponding <see cref="BidirectionalGraph{TVertex,TEdge}"/>.</returns>
        [NotNull]
        public static BidirectionalGraph<TVertex, TEdge> ToBidirectionalGraph<TVertex, TEdge>(
            [NotNull] this IUndirectedGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            if (graph is null)
                throw new ArgumentNullException(nameof(graph));

            var newGraph = new BidirectionalGraph<TVertex, TEdge>();

            newGraph.AddVertexRange(graph.Vertices);
            newGraph.AddEdgeRange(graph.Edges);

            return newGraph;
        }

        /// <summary>
        /// Creates an immutable <see cref="ArrayBidirectionalGraph{TVertex,TEdge}"/> from the input graph.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="graph">Graph to convert.</param>
        /// <returns>A corresponding <see cref="ArrayBidirectionalGraph{TVertex,TEdge}"/>.</returns>
        [Pure]
        [NotNull]
        public static ArrayBidirectionalGraph<TVertex, TEdge> ToArrayBidirectionalGraph<TVertex, TEdge>(
            [NotNull] this IBidirectionalGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            return new ArrayBidirectionalGraph<TVertex, TEdge>(graph);
        }

        /// <summary>
        /// Converts a sequence of edges into an <see cref="UndirectedGraph{TVertex,TEdge}"/>.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="edges">Set of edges to convert.</param>
        /// <param name="allowParallelEdges">Indicates if parallel edges are allowed.</param>
        /// <returns>A corresponding <see cref="UndirectedGraph{TVertex,TEdge}"/>.</returns>
        [Pure]
        [NotNull]
        public static UndirectedGraph<TVertex, TEdge> ToUndirectedGraph<TVertex, TEdge>(
            [NotNull, ItemNotNull] this IEnumerable<TEdge> edges,
            bool allowParallelEdges = true)
            where TEdge : IEdge<TVertex>
        {
            var graph = new UndirectedGraph<TVertex, TEdge>(allowParallelEdges);
            graph.AddVerticesAndEdgeRange(edges);
            return graph;
        }

        /// <summary>
        /// Converts a set of vertex pairs into an <see cref="UndirectedGraph{TVertex,TEdge}"/>.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <param name="vertexPairs">Set of vertex pairs to convert.</param>
        /// <returns>A corresponding <see cref="UndirectedGraph{TVertex,TEdge}"/>.</returns>
        [Pure]
        [NotNull]
        public static UndirectedGraph<TVertex, SEquatableEdge<TVertex>> ToUndirectedGraph<TVertex>(
            [NotNull] this IEnumerable<SEquatableEdge<TVertex>> vertexPairs)
        {
            if (vertexPairs is null)
                throw new ArgumentNullException(nameof(vertexPairs));

            var graph = new UndirectedGraph<TVertex, SEquatableEdge<TVertex>>();
            graph.AddVerticesAndEdgeRange(vertexPairs);
            return graph;
        }

        /// <summary>
        /// Creates an immutable <see cref="ArrayUndirectedGraph{TVertex,TEdge}"/> from the input graph.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="graph">Graph to convert.</param>
        /// <returns>A corresponding <see cref="ArrayUndirectedGraph{TVertex,TEdge}"/>.</returns>
        [Pure]
        [NotNull]
        public static ArrayUndirectedGraph<TVertex, TEdge> ToArrayUndirectedGraph<TVertex, TEdge>(
            [NotNull] this IUndirectedGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            return new ArrayUndirectedGraph<TVertex, TEdge>(graph);
        }

        /// <summary>
        /// Creates an immutable compressed row graph representation of the <paramref name="visitedGraph"/>.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <returns>A corresponding <see cref="CompressedSparseRowGraph{TVertex}"/>.</returns>
        [Pure]
        [NotNull]
        public static CompressedSparseRowGraph<TVertex> ToCompressedRowGraph<TVertex, TEdge>(
            [NotNull] this IVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph)
            where TEdge : IEdge<TVertex>
        {
            return CompressedSparseRowGraph<TVertex>.FromGraph(visitedGraph);
        }

        #endregion
    }
}