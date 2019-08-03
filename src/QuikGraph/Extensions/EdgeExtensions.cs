using System.Collections.Generic;
using System.Linq;
#if !SUPPORTS_TYPE_FULL_FEATURES
using System.Reflection;
#endif
using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// Extensions related to graph edges.
    /// </summary>
    public static class EdgeExtensions
    {
        /// <summary>
        /// Gets a value indicating if the edge is a self edge.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <param name="edge">Edge to check.</param>
        /// <returns>True if edge is a self one, false otherwise.</returns>
        [Pure]
        public static bool IsSelfEdge<TVertex>([NotNull] this IEdge<TVertex> edge)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(edge != null);
            Contract.Ensures(Contract.Result<bool>() == (edge.Source.Equals(edge.Target)));
#endif

            return edge.Source.Equals(edge.Target);
        }

        /// <summary>
        /// Given a source <paramref name="vertex"/>, returns the other vertex in the edge
        /// </summary>
        /// <remarks>The edge must not be a self-edge.</remarks>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <param name="edge">The edge.</param>
        /// <param name="vertex">The source or target vertex of the <paramref name="edge"/>.</param>
        /// <returns>The other edge vertex.</returns>
        [Pure]
        public static TVertex GetOtherVertex<TVertex>([NotNull] this IEdge<TVertex> edge, [NotNull] TVertex vertex)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(edge != null);
            Contract.Requires(vertex != null);
            Contract.Requires(!edge.Source.Equals(edge.Target));
            Contract.Requires(edge.Source.Equals(vertex) || edge.Target.Equals(vertex));
            Contract.Ensures(Contract.Result<TVertex>() != null);
            Contract.Ensures(
                Contract.Result<TVertex>().Equals(edge.Source.Equals(vertex) ? edge.Target : edge.Source));
#endif

            return edge.Source.Equals(vertex) ? edge.Target : edge.Source;
        }

        /// <summary>
        /// Gets a value indicating if the <paramref name="vertex"/> is adjacent to the
        /// <paramref name="edge"/> (is the source or target).
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <param name="edge">The edge.</param>
        /// <param name="vertex">Source or target <paramref name="edge"/> vertex.</param>
        /// <returns>True if the <paramref name="vertex"/> is adjacent to this <paramref name="edge"/>, false otherwise.</returns>
        [Pure]
        public static bool IsAdjacent<TVertex>([NotNull] this IEdge<TVertex> edge, [NotNull] TVertex vertex)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(edge != null);
            Contract.Requires(vertex != null);
            Contract.Ensures(
                Contract.Result<bool>() == (edge.Source.Equals(vertex) || edge.Target.Equals(vertex)));
#endif

            return edge.Source.Equals(vertex) || edge.Target.Equals(vertex);
        }

        /// <summary>
        /// Checks if this set of edges make a path.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="path">Set of edges.</param>
        /// <returns>True if the set makes a complete path, false otherwise.</returns>
        [Pure]
        public static bool IsPath<TVertex, TEdge>([NotNull, ItemNotNull] this IEnumerable<TEdge> path)
            where TEdge : IEdge<TVertex>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(path != null);
            Contract.Requires(
#if SUPPORTS_TYPE_FULL_FEATURES
                typeof(TEdge).IsValueType
#else
                typeof(TEdge).GetTypeInfo().IsValueType
#endif
                || path.All(e => e != null));
#endif

            bool first = true;
            TVertex lastTarget = default(TVertex);
            foreach (var edge in path)
            {
                if (first)
                {
                    lastTarget = edge.Target;
                    first = false;
                }
                else
                {
                    if (!lastTarget.Equals(edge.Source))
                        return false;
                    lastTarget = edge.Target;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks if this set of edges make a cycle.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="path">Set of edges.</param>
        /// <returns>True if the set makes a cycle, false otherwise.</returns>
        [Pure]
        public static bool HasCycles<TVertex, TEdge>([NotNull, ItemNotNull] this IEnumerable<TEdge> path)
            where TEdge : IEdge<TVertex>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(path != null);
            Contract.Requires(
#if SUPPORTS_TYPE_FULL_FEATURES
                typeof(TEdge).IsValueType
#else
                typeof(TEdge).GetTypeInfo().IsValueType
#endif
                || path.All(e => e != null));
#endif

            var vertices = new Dictionary<TVertex, int>();
            bool first = true;
            foreach (TEdge edge in path)
            {
                if (first)
                {
                    if (edge.IsSelfEdge())
                        return true;
                    vertices.Add(edge.Source, 0);
                    vertices.Add(edge.Target, 0);
                    first = false;
                }
                else
                {
                    if (vertices.ContainsKey(edge.Target))
                        return true;
                    vertices.Add(edge.Target, 0);
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if this path of edges does not make a cycle.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="path">Path of edges.</param>
        /// <returns>True if the path makes a cycle, false otherwise.</returns>
        [Pure]
        public static bool IsPathWithoutCycles<TVertex, TEdge>([NotNull, ItemNotNull] this IEnumerable<TEdge> path)
            where TEdge : IEdge<TVertex>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(path != null);
            Contract.Requires(
#if SUPPORTS_TYPE_FULL_FEATURES
                typeof(TEdge).IsValueType
#else
                typeof(TEdge).GetTypeInfo().IsValueType
#endif
                || path.All(e => e != null));
            Contract.Requires(IsPath<TVertex, TEdge>(path));
#endif

            var vertices = new Dictionary<TVertex, int>();
            bool first = true;
            TVertex lastTarget = default(TVertex);
            foreach (TEdge edge in path)
            {
                if (first)
                {
                    lastTarget = edge.Target;
                    if (edge.IsSelfEdge())
                        return false;
                    vertices.Add(edge.Source, 0);
                    vertices.Add(lastTarget, 0);
                    first = false;
                }
                else
                {
                    if (!lastTarget.Equals(edge.Source))
                        return false;
                    if (vertices.ContainsKey(edge.Target))
                        return false;

                    lastTarget = edge.Target;
                    vertices.Add(edge.Target, 0);
                }
            }

            return true;
        }

        /// <summary>
        /// Creates a vertex pair (source, target) from this edge.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <param name="edge">The edge.</param>
        /// <returns>A <see cref="SEquatableEdge{TVertex}"/>.</returns>
        [Pure]
        public static SEquatableEdge<TVertex> ToVertexPair<TVertex>([NotNull] this IEdge<TVertex> edge)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(edge != null);
            Contract.Ensures(Contract.Result<SEquatableEdge<TVertex>>().Source.Equals(edge.Source));
            Contract.Ensures(Contract.Result<SEquatableEdge<TVertex>>().Target.Equals(edge.Target));
#endif

            return new SEquatableEdge<TVertex>(edge.Source, edge.Target);
        }

        /// <summary>
        /// Checks that the <paramref name="root"/> is a predecessor of the given <paramref name="vertex"/>.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="predecessors">Predecessors map.</param>
        /// <param name="root">Root vertex.</param>
        /// <param name="vertex">Ending vertex.</param>
        /// <returns>True if the <paramref name="root"/> is a predecessor of the <paramref name="vertex"/>.</returns>
        [Pure]
        public static bool IsPredecessor<TVertex, TEdge>(
            [NotNull] this IDictionary<TVertex, TEdge> predecessors,
            [NotNull] TVertex root,
            [NotNull] TVertex vertex)
            where TEdge : IEdge<TVertex>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(predecessors != null);
            Contract.Requires(root != null);
            Contract.Requires(vertex != null);
            Contract.Requires(
#if SUPPORTS_TYPE_FULL_FEATURES
                typeof(TEdge).IsValueType
#else
                typeof(TEdge).GetTypeInfo().IsValueType
#endif
                || predecessors.Values.All(e => e != null));
#endif

            TVertex currentVertex = vertex;
            if (root.Equals(currentVertex))
                return true;

            while (predecessors.TryGetValue(currentVertex, out TEdge predecessor))
            {
                var source = GetOtherVertex(predecessor, currentVertex);
                if (currentVertex.Equals(source))
                    return false;
                if (source.Equals(root))
                    return true;
                currentVertex = source;
            }

            return false;
        }

        /// <summary>
        /// Tries to get the predecessor path, if reachable.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="predecessors">Predecessors map.</param>
        /// <param name="vertex">Path ending vertex.</param>
        /// <param name="result">Path to the ending vertex.</param>
        /// <returns>True if a path was found, false otherwise.</returns>
        [Pure]
        public static bool TryGetPath<TVertex, TEdge>(
            [NotNull] this IDictionary<TVertex, TEdge> predecessors,
            [NotNull] TVertex vertex,
            [ItemNotNull] out IEnumerable<TEdge> result)
            where TEdge : IEdge<TVertex>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(predecessors != null);
            Contract.Requires(vertex != null);
            Contract.Requires(
#if SUPPORTS_TYPE_FULL_FEATURES
                typeof(TEdge).IsValueType
#else
                typeof(TEdge).GetTypeInfo().IsValueType
#endif
                || predecessors.Values.All(e => e != null));
            Contract.Ensures(
                !Contract.Result<bool>()
                || (Contract.ValueAtReturn(out result) != null
#if SUPPORTS_TYPE_FULL_FEATURES
                    && (typeof(TEdge).IsValueType
#else
                    && (typeof(TEdge).GetTypeInfo().IsValueType
#endif
                        || Contract.ValueAtReturn(out result).All(e => e != null))));
#endif

            var path = new List<TEdge>();

            TVertex currentVertex = vertex;
            while (predecessors.TryGetValue(currentVertex, out TEdge edge))
            {
                path.Add(edge);
                currentVertex = GetOtherVertex(edge, currentVertex);
            }

            if (path.Count > 0)
            {
                path.Reverse();
                result = path.ToArray();
                return true;
            }

            result = null;
            return false;
        }

        /// <summary>
        /// Returns the most efficient comparer for the particular type of <typeparamref name="TEdge"/>.
        /// If <typeparamref name="TEdge"/> implements <see cref="IUndirectedEdge{TVertex}"/>, then only
        /// the (source, target) pair has to be compared; if not, (source, target) and (target, source)
        /// have to be compared.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <returns>The best edge equality comparer.</returns>
        [Pure]
        public static EdgeEqualityComparer<TVertex> GetUndirectedVertexEquality<TVertex, TEdge>()
        {
            if (typeof(IUndirectedEdge<TVertex>).IsAssignableFrom(typeof(TEdge)))
                return SortedVertexEquality;
            return UndirectedVertexEquality;
        }

        /// <summary>
        /// Gets a value indicating if the vertices of this edge match
        /// <paramref name="source"/> and <paramref name="target"/>
        /// or <paramref name="target"/> and <paramref name="source"/> vertices.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <param name="edge">The edge.</param>
        /// <param name="source">Source vertex.</param>
        /// <param name="target">Target vertex.</param>
        /// <returns>True if both <paramref name="source"/> and
        /// <paramref name="target"/> match edge vertices, false otherwise.</returns>
        [Pure]
        public static bool UndirectedVertexEquality<TVertex>(
            [NotNull] this IEdge<TVertex> edge,
            [NotNull] TVertex source,
            [NotNull] TVertex target)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(edge != null);
            Contract.Requires(source != null);
            Contract.Requires(target != null);
#endif

            return (edge.Source.Equals(source) && edge.Target.Equals(target)) 
                   || (edge.Target.Equals(source) && edge.Source.Equals(target));
        }

        /// <summary>
        /// Gets a value indicating if the vertices of this edge match
        /// <paramref name="source"/> and <paramref name="target"/> vertices.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <param name="edge">The edge.</param>
        /// <param name="source">Source vertex.</param>
        /// <param name="target">Target vertex.</param>
        /// <returns>True if both <paramref name="source"/> and
        /// <paramref name="target"/> match edge vertices, false otherwise.</returns>
        [Pure]
        public static bool SortedVertexEquality<TVertex>(
            [NotNull] this IEdge<TVertex> edge, 
            [NotNull] TVertex source,
            [NotNull] TVertex target)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(edge != null);
            Contract.Requires(source != null);
            Contract.Requires(target != null);
            Contract.Requires(Comparer<TVertex>.Default.Compare(source, target) <= 0);
#endif

            return edge.Source.Equals(source) && edge.Target.Equals(target);
        }

        /// <summary>
        /// Returns an enumeration of reversed edges.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="edges">Edges to reversed.</param>
        /// <returns>Reversed edges.</returns>
        [Pure]
        public static IEnumerable<SReversedEdge<TVertex, TEdge>> ReverseEdges<TVertex, TEdge>(
            [NotNull, ItemNotNull] IEnumerable<TEdge> edges)
            where TEdge : IEdge<TVertex>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(edges != null);
            Contract.Requires(edges.All(e => e != null));
            Contract.Ensures(Contract.Result<IEnumerable<SReversedEdge<TVertex, TEdge>>>() != null);
#endif

            return edges.Select(edge => new SReversedEdge<TVertex, TEdge>(edge));
        }
    }
}
