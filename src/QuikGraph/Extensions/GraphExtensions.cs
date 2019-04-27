using System;
using System.Collections.Generic;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using System.Linq;
using JetBrains.Annotations;
using static QuikGraph.QuikGraphHelpers;

namespace QuikGraph
{
    /// <summary>
    /// Extensions for populating graph data structures.
    /// </summary>
    public static class GraphExtensions
    {
        /// <summary>
        /// Wraps a dictionary into a vertex and edge list graph.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <typeparam name="TValue">Type of the enumerable of out-edges.</typeparam>
        /// <param name="dictionary">Vertices and edges mapping.</param>
        /// <returns>A corresponding <see cref="DelegateVertexAndEdgeListGraph{TVertex,TEdge}"/>.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        [NotNull]
        public static DelegateVertexAndEdgeListGraph<TVertex, TEdge> ToVertexAndEdgeListGraph<TVertex, TEdge, TValue>(
            [NotNull] this IDictionary<TVertex, TValue> dictionary)
            where TEdge : IEdge<TVertex>, IEquatable<TEdge>
            where TValue : IEnumerable<TEdge>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(dictionary != null);
            Contract.Requires(dictionary.Values.All(v => v != null));
#endif

            return ToVertexAndEdgeListGraph(dictionary, kv => kv.Value);
        }

        /// <summary>
        /// Wraps a dictionary into a vertex and edge list graph with the given edge conversion.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <typeparam name="TValue">Type of the enumerable of out-edges.</typeparam>
        /// <param name="dictionary">Vertices and edges mapping.</param>
        /// <param name="keyValueToOutEdges">Converter of vertex/edge mapping to enumerable of edges.</param>
        /// <returns>A corresponding <see cref="DelegateVertexAndEdgeListGraph{TVertex,TEdge}"/>.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        [NotNull]
        public static DelegateVertexAndEdgeListGraph<TVertex, TEdge> ToVertexAndEdgeListGraph<TVertex, TEdge, TValue>(
            [NotNull] this IDictionary<TVertex, TValue> dictionary,
#if SUPPORTS_CONVERTER
            [NotNull] Converter<KeyValuePair<TVertex, TValue>, IEnumerable<TEdge>> keyValueToOutEdges)
#else
            [NotNull] Func<KeyValuePair<TVertex,TValue>, IEnumerable<TEdge>> keyValueToOutEdges)
#endif
            where TEdge : IEdge<TVertex>, IEquatable<TEdge>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(dictionary != null);
            Contract.Requires(keyValueToOutEdges != null);
#endif

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
        /// Creates an instance of <see cref="DelegateIncidenceGraph{TVertex,TEdge}"/> from this getter of out-edges.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="tryGetOutEdges">Getter of out-edges.</param>
        /// <returns>A corresponding <see cref="DelegateIncidenceGraph{TVertex,TEdge}"/>.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        [NotNull]
        public static DelegateIncidenceGraph<TVertex, TEdge> ToDelegateIncidenceGraph<TVertex, TEdge>(
            [NotNull] this TryFunc<TVertex, IEnumerable<TEdge>> tryGetOutEdges)
            where TEdge : IEdge<TVertex>, IEquatable<TEdge>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(tryGetOutEdges != null);
#endif

            return new DelegateIncidenceGraph<TVertex, TEdge>(tryGetOutEdges);
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
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        [NotNull]
        public static DelegateBidirectionalIncidenceGraph<TVertex, TEdge> ToDelegateBidirectionalIncidenceGraph<TVertex, TEdge>(
            [NotNull] this TryFunc<TVertex, IEnumerable<TEdge>> tryGetOutEdges,
            [NotNull] TryFunc<TVertex, IEnumerable<TEdge>> tryGetInEdges)
            where TEdge : IEdge<TVertex>, IEquatable<TEdge>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(tryGetOutEdges != null);
            Contract.Requires(tryGetInEdges != null);
#endif

            return new DelegateBidirectionalIncidenceGraph<TVertex, TEdge>(tryGetOutEdges, tryGetInEdges);
        }

        /// <summary>
        /// Creates an instance of <see cref="DelegateIncidenceGraph{TVertex,TEdge}"/> from this getter of out-edges.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="getOutEdges">Getter of out-edges.</param>
        /// <returns>A corresponding <see cref="DelegateIncidenceGraph{TVertex,TEdge}"/>.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        [NotNull]
        public static DelegateIncidenceGraph<TVertex, TEdge> ToDelegateIncidenceGraph<TVertex, TEdge>(
            [NotNull] this Func<TVertex, IEnumerable<TEdge>> getOutEdges)
            where TEdge : IEdge<TVertex>, IEquatable<TEdge>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(getOutEdges != null);
#endif

            return ToDelegateIncidenceGraph(ToTryFunc(getOutEdges));
        }

        /// <summary>
        /// Creates an instance of <see cref="DelegateVertexAndEdgeListGraph{TVertex,TEdge}"/>
        /// from given vertices and edge getter.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="vertices">Enumerable of vertices.</param>
        /// <param name="tryGetOutEdges">Getter of out-edges.</param>
        /// <returns>A corresponding <see cref="DelegateVertexAndEdgeListGraph{TVertex,TEdge}"/>.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        [NotNull]
        public static DelegateVertexAndEdgeListGraph<TVertex, TEdge> ToDelegateVertexAndEdgeListGraph<TVertex, TEdge>(
            [NotNull, ItemNotNull] this IEnumerable<TVertex> vertices,
            [NotNull] TryFunc<TVertex, IEnumerable<TEdge>> tryGetOutEdges)
            where TEdge : IEdge<TVertex>, IEquatable<TEdge>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(vertices != null);
            Contract.Requires(tryGetOutEdges != null);
            Contract.Requires(vertices.All(v => tryGetOutEdges(v, out _)));
#endif

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
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        [NotNull]
        public static DelegateVertexAndEdgeListGraph<TVertex, TEdge> ToDelegateVertexAndEdgeListGraph<TVertex, TEdge>(
            [NotNull, ItemNotNull] this IEnumerable<TVertex> vertices,
            [NotNull] Func<TVertex, IEnumerable<TEdge>> getOutEdges)
            where TEdge : IEdge<TVertex>, IEquatable<TEdge>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(vertices != null);
            Contract.Requires(getOutEdges != null);
#endif

            return ToDelegateVertexAndEdgeListGraph(vertices, ToTryFunc(getOutEdges));
        }

        /// <summary>
        /// Wraps a dictionary into an undirected list graph.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <typeparam name="TValue">Type of the enumerable of out-edges.</typeparam>
        /// <param name="dictionary">Vertices and edges mapping.</param>
        /// <returns>A corresponding <see cref="DelegateVertexAndEdgeListGraph{TVertex,TEdge}"/>.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        [NotNull]
        public static DelegateVertexAndEdgeListGraph<TVertex, TEdge> ToDelegateUndirectedGraph<TVertex, TEdge, TValue>(
            [NotNull] this IDictionary<TVertex, TValue> dictionary)
            where TEdge : IEdge<TVertex>, IEquatable<TEdge>
            where TValue : IEnumerable<TEdge>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(dictionary != null);
            Contract.Requires(dictionary.Values.All(v => v != null));
#endif

            return ToDelegateUndirectedGraph(dictionary, kv => kv.Value);
        }

        /// <summary>
        /// Wraps a dictionary into an undirected list graph with the given edge condition.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <typeparam name="TValue">Type of the enumerable of out-edges.</typeparam>
        /// <param name="dictionary">Vertices and edges mapping.</param>
        /// <param name="keyValueToOutEdges">Converter of vertex/edge mapping to enumerable of edges.</param>
        /// <returns>A corresponding <see cref="DelegateVertexAndEdgeListGraph{TVertex,TEdge}"/>.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        [NotNull]
        public static DelegateVertexAndEdgeListGraph<TVertex, TEdge> ToDelegateUndirectedGraph<TVertex, TEdge, TValue>(
            [NotNull] this IDictionary<TVertex, TValue> dictionary,
#if SUPPORTS_CONVERTER
            [NotNull] Converter<KeyValuePair<TVertex, TValue>, IEnumerable<TEdge>> keyValueToOutEdges)
#else
            [NotNull] Func<KeyValuePair<TVertex, TValue>, IEnumerable<TEdge>> keyValueToOutEdges)
#endif
            where TEdge : IEdge<TVertex>, IEquatable<TEdge>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(dictionary != null);
            Contract.Requires(keyValueToOutEdges != null);
#endif

            return new DelegateVertexAndEdgeListGraph<TVertex, TEdge>(
                dictionary.Keys,
                delegate (TVertex key, out IEnumerable<TEdge> edges)
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
        /// Creates an instance of <see cref="DelegateUndirectedGraph{TVertex,TEdge}"/>
        /// from given vertices and edge getter.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="vertices">Enumerable of vertices.</param>
        /// <param name="tryGetAdjacentEdges">Getter of adjacent edges.</param>
        /// <returns>A corresponding <see cref="DelegateUndirectedGraph{TVertex,TEdge}"/>.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        [NotNull]
        public static DelegateUndirectedGraph<TVertex, TEdge> ToDelegateUndirectedGraph<TVertex, TEdge>(
            [NotNull, ItemNotNull] this IEnumerable<TVertex> vertices,
            [NotNull] TryFunc<TVertex, IEnumerable<TEdge>> tryGetAdjacentEdges)
            where TEdge : IEdge<TVertex>, IEquatable<TEdge>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(vertices != null);
            Contract.Requires(tryGetAdjacentEdges != null);
            Contract.Requires(vertices.All(v => tryGetAdjacentEdges(v, out _)));
#endif

            return new DelegateUndirectedGraph<TVertex, TEdge>(vertices, tryGetAdjacentEdges, true);
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
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        [NotNull]
        public static DelegateUndirectedGraph<TVertex, TEdge> ToDelegateUndirectedGraph<TVertex, TEdge>(
            [NotNull, ItemNotNull] this IEnumerable<TVertex> vertices,
            [NotNull] Func<TVertex, IEnumerable<TEdge>> getAdjacentEdges)
            where TEdge : IEdge<TVertex>, IEquatable<TEdge>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(vertices != null);
            Contract.Requires(getAdjacentEdges != null);
#endif

            return ToDelegateUndirectedGraph(vertices, ToTryFunc(getAdjacentEdges));
        }

        /// <summary>
        /// Converts a raw array of sources and targets (2 columns) vertices into a graph.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <param name="edges">
        /// Array of vertices defining edges.
        /// The first items of each column represents the number of vertices following.
        /// </param>
        /// <returns>A corresponding <see cref="AdjacencyGraph{TVertex,TEdge}"/>.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        [NotNull]
        public static AdjacencyGraph<TVertex, SEquatableEdge<TVertex>> ToAdjacencyGraph<TVertex>(
            [NotNull] this TVertex[][] edges)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(edges != null);
            Contract.Requires(edges.Length == 2);
            Contract.Requires(edges[0] != null);
            Contract.Requires(edges[1] != null);
            Contract.Requires(edges[0].Length == edges[1].Length);
            Contract.Ensures(Contract.Result<AdjacencyGraph<TVertex, SEquatableEdge<TVertex>>>() != null);
#endif

            var sources = edges[0];
            var targets = edges[1];
            int n = sources.Length;
            var edgePairs = new List<SEquatableEdge<TVertex>>(n);
            for (int i = 0; i < n; ++i)
                edgePairs.Add(new SEquatableEdge<TVertex>(sources[i], targets[i]));

            return ToAdjacencyGraph(edgePairs);
        }

        /// <summary>
        /// Creates an immutable <see cref="ArrayAdjacencyGraph{TVertex,TEdge}"/> from the input graph.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="graph">Graph to convert.</param>
        /// <returns>A corresponding <see cref="ArrayAdjacencyGraph{TVertex,TEdge}"/>.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        [NotNull]
        public static ArrayAdjacencyGraph<TVertex, TEdge> ToArrayAdjacencyGraph<TVertex, TEdge>(
            [NotNull] this IVertexAndEdgeListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(graph != null);
#endif

            return new ArrayAdjacencyGraph<TVertex, TEdge>(graph);
        }

        /// <summary>
        /// Creates an immutable <see cref="ArrayBidirectionalGraph{TVertex,TEdge}"/> from the input graph.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="graph">Graph to convert.</param>
        /// <returns>A corresponding <see cref="ArrayBidirectionalGraph{TVertex,TEdge}"/>.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        [NotNull]
        public static ArrayBidirectionalGraph<TVertex, TEdge> ToArrayBidirectionalGraph<TVertex, TEdge>(
            [NotNull] this IBidirectionalGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(graph != null);
#endif

            return new ArrayBidirectionalGraph<TVertex, TEdge>(graph);
        }

        /// <summary>
        /// Creates an immutable <see cref="ArrayUndirectedGraph{TVertex,TEdge}"/> from the input graph.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="graph">Graph to convert.</param>
        /// <returns>A corresponding <see cref="ArrayUndirectedGraph{TVertex,TEdge}"/>.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        [NotNull]
        public static ArrayUndirectedGraph<TVertex, TEdge> ToArrayUndirectedGraph<TVertex, TEdge>(
            [NotNull] this IUndirectedGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(graph != null);
#endif

            return new ArrayUndirectedGraph<TVertex, TEdge>(graph);
        }

        /// <summary>
        /// Wraps a adjacency graph (out-edges only) into a bidirectional graph.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="graph">Graph to convert.</param>
        /// <returns>A corresponding <see cref="IBidirectionalGraph{TVertex,TEdge}"/>.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        [NotNull]
        public static IBidirectionalGraph<TVertex, TEdge> ToBidirectionalGraph<TVertex, TEdge>(
            [NotNull] this IVertexAndEdgeListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(graph != null);
#endif

            if (graph is IBidirectionalGraph<TVertex, TEdge> self)
                return self;

            return new BidirectionalAdapterGraph<TVertex, TEdge>(graph);
        }

        /// <summary>
        /// Converts a sequence of edges into an <see cref="UndirectedGraph{TVertex,TEdge}"/>.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="edges">Set of edges to convert.</param>
        /// <param name="allowParallelEdges">Indicates if parallel edges are allowed.</param>
        /// <returns>A corresponding <see cref="UndirectedGraph{TVertex,TEdge}"/>.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        [NotNull]
        public static UndirectedGraph<TVertex, TEdge> ToUndirectedGraph<TVertex, TEdge>(
            [NotNull, ItemNotNull] this IEnumerable<TEdge> edges,
            bool allowParallelEdges = true)
            where TEdge : IEdge<TVertex>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(edges != null);
            Contract.Requires(edges.All(e => e != null));
#endif

            var graph = new UndirectedGraph<TVertex, TEdge>(allowParallelEdges);
            graph.AddVerticesAndEdgeRange(edges);
            return graph;
        }

        /// <summary>
        /// Converts a set of edges into a <see cref="BidirectionalGraph{TVertex,TEdge}"/>.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="edges">Set of edges to convert.</param>
        /// <param name="allowParallelEdges">Indicates if parallel edges are allowed.</param>
        /// <returns>A corresponding <see cref="BidirectionalGraph{TVertex,TEdge}"/>.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        [NotNull]
        public static BidirectionalGraph<TVertex, TEdge> ToBidirectionalGraph<TVertex, TEdge>(
            [NotNull, ItemNotNull] this IEnumerable<TEdge> edges,
            bool allowParallelEdges = true)
            where TEdge : IEdge<TVertex>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(edges != null);
            Contract.Requires(EnumerableContract.ElementsNotNull(edges));
#endif

            var graph = new BidirectionalGraph<TVertex, TEdge>(allowParallelEdges);
            graph.AddVerticesAndEdgeRange(edges);
            return graph;
        }

        /// <summary>
        /// Converts a set of edges into an <see cref="AdjacencyGraph{TVertex,TEdge}"/>.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="edges">Set of edges to convert.</param>
        /// <param name="allowParallelEdges">Indicates if parallel edges are allowed.</param>
        /// <returns>A corresponding <see cref="AdjacencyGraph{TVertex,TEdge}"/>.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        [NotNull]
        public static AdjacencyGraph<TVertex, TEdge> ToAdjacencyGraph<TVertex, TEdge>(
            [NotNull, ItemNotNull] this IEnumerable<TEdge> edges,
            bool allowParallelEdges = true)
            where TEdge : IEdge<TVertex>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(edges != null);
            Contract.Requires(EnumerableContract.ElementsNotNull(edges));
#endif

            var graph = new AdjacencyGraph<TVertex, TEdge>(allowParallelEdges);
            graph.AddVerticesAndEdgeRange(edges);
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
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        [NotNull]
        public static AdjacencyGraph<TVertex, TEdge> ToAdjacencyGraph<TVertex, TEdge>(
            [NotNull, ItemNotNull] this IEnumerable<TVertex> vertices,
            [NotNull, InstantHandle] Func<TVertex, IEnumerable<TEdge>> outEdgesFactory,
            bool allowParallelEdges = true)
            where TEdge : IEdge<TVertex>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(vertices != null);
            Contract.Requires(outEdgesFactory != null);
            Contract.Requires(EnumerableContract.ElementsNotNull(vertices));
#endif

            var graph = new AdjacencyGraph<TVertex, TEdge>(allowParallelEdges);
            graph.AddVertexRange(vertices);

            foreach (var vertex in graph.Vertices)
                graph.AddEdgeRange(outEdgesFactory(vertex));

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
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        [NotNull]
        public static BidirectionalGraph<TVertex, TEdge> ToBidirectionalGraph<TVertex, TEdge>(
            [NotNull, ItemNotNull] this IEnumerable<TVertex> vertices,
            [NotNull, InstantHandle] Func<TVertex, IEnumerable<TEdge>> outEdgesFactory,
            bool allowParallelEdges = true)
            where TEdge : IEdge<TVertex>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(vertices != null);
            Contract.Requires(outEdgesFactory != null);
            Contract.Requires(EnumerableContract.ElementsNotNull(vertices));
#endif

            var graph = new BidirectionalGraph<TVertex, TEdge>(allowParallelEdges);
            graph.AddVertexRange(vertices);

            foreach (var vertex in graph.Vertices)
                graph.AddEdgeRange(outEdgesFactory(vertex));

            return graph;
        }

        /// <summary>
        /// Converts a set of vertex pairs into an <see cref="AdjacencyGraph{TVertex,TEdge}"/>.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <param name="vertexPairs">Set of vertex pairs to convert.</param>
        /// <returns>A corresponding <see cref="AdjacencyGraph{TVertex,TEdge}"/>.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        [NotNull]
        public static AdjacencyGraph<TVertex, SEquatableEdge<TVertex>> ToAdjacencyGraph<TVertex>(
            [NotNull] this IEnumerable<SEquatableEdge<TVertex>> vertexPairs)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(vertexPairs != null);
#endif

            var graph = new AdjacencyGraph<TVertex, SEquatableEdge<TVertex>>();
            graph.AddVerticesAndEdgeRange(vertexPairs);
            return graph;
        }

        /// <summary>
        /// Converts a set of vertex pairs into a <see cref="BidirectionalGraph{TVertex,TEdge}"/>.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <param name="vertexPairs">Set of vertex pairs to convert.</param>
        /// <returns>A corresponding <see cref="BidirectionalGraph{TVertex,TEdge}"/>.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        [NotNull]
        public static BidirectionalGraph<TVertex, SEquatableEdge<TVertex>> ToBidirectionalGraph<TVertex>(
            [NotNull] this IEnumerable<SEquatableEdge<TVertex>> vertexPairs)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(vertexPairs != null);
#endif

            var graph = new BidirectionalGraph<TVertex, SEquatableEdge<TVertex>>();
            graph.AddVerticesAndEdgeRange(vertexPairs);
            return graph;
        }

        /// <summary>
        /// Creates a <see cref="BidirectionalGraph{TVertex,TEdge}"/> from this graph.
        /// </summary>
        /// <param name="graph">Graph to convert.</param>
        /// <returns>A corresponding <see cref="BidirectionalGraph{TVertex,TEdge}"/>.</returns>
        [NotNull]
        public static BidirectionalGraph<TVertex, TEdge> ToBidirectionalGraph<TVertex, TEdge>(
            [NotNull] this UndirectedGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            var newGraph = new BidirectionalGraph<TVertex, TEdge>();

            newGraph.AddVertexRange(graph.Vertices);
            newGraph.AddEdgeRange(graph.Edges);

            return newGraph;
        }

        /// <summary>
        /// Converts a set of vertex pairs into an <see cref="UndirectedGraph{TVertex,TEdge}"/>.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <param name="vertexPairs">Set of vertex pairs to convert.</param>
        /// <returns>A corresponding <see cref="UndirectedGraph{TVertex,TEdge}"/>.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        [NotNull]
        public static UndirectedGraph<TVertex, SEquatableEdge<TVertex>> ToUndirectedGraph<TVertex>(
            [NotNull] this IEnumerable<SEquatableEdge<TVertex>> vertexPairs)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(vertexPairs != null);
#endif

            var graph = new UndirectedGraph<TVertex, SEquatableEdge<TVertex>>();
            graph.AddVerticesAndEdgeRange(vertexPairs);
            return graph;
        }

        /// <summary>
        /// Creates an immutable compressed row graph representation of the <paramref name="visitedGraph"/>.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <returns>A corresponding <see cref="CompressedSparseRowGraph{TVertex}"/>.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        [NotNull]
        public static CompressedSparseRowGraph<TVertex> ToCompressedRowGraph<TVertex, TEdge>(
            [NotNull] this IVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph)
            where TEdge : IEdge<TVertex>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(visitedGraph != null);
#endif

            return CompressedSparseRowGraph<TVertex>.FromGraph(visitedGraph);
        }
    }
}
