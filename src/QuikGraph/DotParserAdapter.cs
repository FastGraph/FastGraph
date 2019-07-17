//using System;
//using System.Collections.Generic;
//#if SUPPORTS_CONTRACTS
//using System.Diagnostics.Contracts;
//#endif
//using JetBrains.Annotations;

//namespace QuikGraph
//{
//    using Attributes = IDictionary<string, string>;

//    /// <summary>
//    /// Dot parser adapter, offers features to load graph from dot.
//    /// </summary>
//    public class DotParserAdapter
//    {
//        /// <summary>
//        /// Loads the given <paramref name="dotSource"/> and creates the corresponding graph.
//        /// </summary>
//        /// <param name="dotSource">Dot source string.</param>
//        /// <param name="createGraph">Graph constructor function.</param>
//        /// <param name="vertexFunction">Packing function (See <see cref="VertexFactory"/> class).</param>
//        /// <param name="edgeFunction">Packing function (See <see cref="EdgeFactory{TVertex}"/> class).</param>
//        /// <exception cref="NotImplementedException">This method is not implemented yet.</exception>
//#if SUPPORTS_CONTRACTS
//        [System.Diagnostics.Contracts.Pure]
//#endif
//        [JetBrains.Annotations.Pure]
//        [NotNull]
//        internal static IMutableVertexAndEdgeSet<TVertex, TEdge> LoadDot<TVertex, TEdge>(
//            [NotNull] string dotSource,
//            [NotNull, InstantHandle] Func<bool, IMutableVertexAndEdgeSet<TVertex, TEdge>> createGraph,
//            [NotNull, InstantHandle] Func<string, Attributes, TVertex> vertexFunction,
//            [NotNull, InstantHandle] Func<TVertex, TVertex, Attributes, TEdge> edgeFunction) 
//            where TEdge : IEdge<TVertex>
//        {
//            //var graphData = DotParser.parse(dotSource);
//            //var graph = createGraph(!graphData.IsStrict);

//            //var vertices = graphData.Nodes.ToDictionary(v => v.Key, v => vertexFunction(v.Key, v.Value));
//            //graph.AddVertexRange(vertices.Values);

//            //foreach (var parallelEdges in graphData.Edges)
//            //{
//            //    var edgeVertices = parallelEdges.Key;
//            //    foreach (var attr in parallelEdges.Value)
//            //    {
//            //        graph.AddEdge(edgeFunction(vertices[edgeVertices.Item1], vertices[edgeVertices.Item2], attr));
//            //        if (graph.IsDirected && !graphData.IsDirected)
//            //        {
//            //            graph.AddEdge(edgeFunction(vertices[edgeVertices.Item2], vertices[edgeVertices.Item1], attr));
//            //        }
//            //    }
//            //}
//            //return graph;
//            throw new NotImplementedException();
//        }

//        /// <summary>
//        /// Helpers to get weight from attributes.
//        /// </summary>
//        public class WeightHelpers
//        {
//            [NotNull]
//            private const string Weight = "weight";

//            /// <summary>
//            /// Gets the <see cref="Weight"/> attribute if available.
//            /// </summary>
//            /// <param name="attributes">Attributes.</param>
//            /// <returns>Found weight, null otherwise.</returns>
//            [CanBeNull]
//            public static int? GetWeight([NotNull] Attributes attributes)
//            {
//                return int.TryParse(attributes["weight"], out int weight) ? (int?) weight : null;
//            }

//            /// <summary>
//            /// Gets the <see cref="Weight"/> attribute if available,
//            /// and fallback on <paramref name="defaultValue"/> if not found.
//            /// </summary>
//            /// <param name="attributes">Attributes.</param>
//            /// <param name="defaultValue">Default weight value.</param>
//            /// <returns>Found weight, otherwise returns the <paramref name="defaultValue"/>.</returns>
//            public static int GetWeight([NotNull] Attributes attributes, int defaultValue)
//            {
//                if (!attributes.TryGetValue("weight", out string weightAttribute))
//                    return defaultValue;

//                return int.TryParse(weightAttribute, out int weight) 
//                    ? weight 
//                    : defaultValue;
//            }
//        }

//        /// <summary>
//        /// Vertex factory.
//        /// </summary>
//        public class VertexFactory
//        {
//            /// <summary>
//            /// Gets the vertex name.
//            /// </summary>
//            [NotNull]
//            public static Func<string, Attributes, string> Name = (vertex, attributes) => vertex;

//            /// <summary>
//            /// Gets the vertex name and its attributes.
//            /// </summary>
//            [NotNull]
//            public static Func<string, Attributes, KeyValuePair<string, Attributes>> NameAndAttributes = 
//                (vertex, attributes) => new KeyValuePair<string, Attributes>(vertex, new Dictionary<string, string>(attributes));

//            /// <summary>
//            /// Gets the vertex weight (if available).
//            /// </summary>
//            [NotNull]
//            public static Func<string, Attributes, KeyValuePair<string, int?>> WeightedNullable =
//                (vertex, attributes) => new KeyValuePair<string, int?>(vertex, WeightHelpers.GetWeight(attributes));

//            /// <summary>
//            /// Gets the vertex weight (with fallback value).
//            /// </summary>
//            [NotNull]
//            public static Func<string, Attributes, KeyValuePair<string, int>> Weighted(int defaultValue) => 
//                (vertex, attributes) => new KeyValuePair<string, int>(vertex, WeightHelpers.GetWeight(attributes, defaultValue));
//        }

//        /// <summary>
//        /// Edge factory.
//        /// </summary>
//        /// <typeparam name="TVertex">Vertex type.</typeparam>
//        public class EdgeFactory<TVertex>
//        {
//            /// <summary>
//            /// Gets the edge vertices.
//            /// </summary>
//            [NotNull]
//            public static Func<TVertex, TVertex, Attributes, SEdge<TVertex>> VerticesOnly = 
//                (vertex1, vertex2, attributes) => new SEdge<TVertex>(vertex1, vertex2);

//            /// <summary>
//            /// Gets the edge vertices and its attributes.
//            /// </summary>
//            [NotNull]
//            public static Func<TVertex, TVertex, Attributes, STaggedEdge<TVertex, Attributes>> VerticesAndEdgeAttributes = 
//                (vertex1, vertex2, attributes) => new STaggedEdge<TVertex, Attributes>(vertex1, vertex2, new Dictionary<string, string>(attributes));

//            /// <summary>
//            /// Gets the edge vertices and weight (if available).
//            /// </summary>
//            [NotNull]
//            public static Func<TVertex, TVertex, Attributes, STaggedEdge<TVertex, int?>> WeightedNullable = 
//                (vertex1, vertex2, attributes) => new STaggedEdge<TVertex, int?>(vertex1, vertex2, WeightHelpers.GetWeight(attributes));

//            /// <summary>
//            /// Gets the edge vertices and weight (with fallback value).
//            /// </summary>
//            [NotNull]
//            public static Func<TVertex, TVertex, Attributes, STaggedEdge<TVertex, int>> Weighted(int defaultValue) => 
//                (vertex1, vertex2, attributes) => new STaggedEdge<TVertex, int>(vertex1, vertex2, WeightHelpers.GetWeight(attributes, defaultValue));
//        }
//    }
//}