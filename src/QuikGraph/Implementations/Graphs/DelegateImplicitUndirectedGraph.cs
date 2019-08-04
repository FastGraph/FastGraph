using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// A delegate-based implicit undirected graph.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public class DelegateImplicitUndirectedGraph<TVertex, TEdge> : IImplicitUndirectedGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateImplicitUndirectedGraph{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="tryGetAdjacentEdges">Getter of adjacent edges.</param>
        /// <param name="allowParallelEdges">Indicates if parallel edges are allowed.</param>
        public DelegateImplicitUndirectedGraph(
            [NotNull] TryFunc<TVertex, IEnumerable<TEdge>> tryGetAdjacentEdges,
            bool allowParallelEdges)
        {
            _tryGetAdjacencyEdges = tryGetAdjacentEdges ?? throw new ArgumentNullException(nameof(tryGetAdjacentEdges));
            AllowParallelEdges = allowParallelEdges;
        }

        /// <inheritdoc />
        public EdgeEqualityComparer<TVertex> EdgeEqualityComparer { get; } =
            EdgeExtensions.GetUndirectedVertexEquality<TVertex, TEdge>();

        /// <summary>
        /// Getter of adjacent edges.
        /// </summary>
        [NotNull]
        private readonly TryFunc<TVertex, IEnumerable<TEdge>> _tryGetAdjacencyEdges;

        #region IGraph<TVertex,TEdge>

        /// <inheritdoc />
        public bool IsDirected => false;

        /// <inheritdoc />
        public bool AllowParallelEdges { get; }

        #endregion

        #region IImplicitUndirectedGraph<TVertex,TEdge>

        /// <inheritdoc />
        public IEnumerable<TEdge> AdjacentEdges(TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            if (_tryGetAdjacencyEdges(vertex, out IEnumerable<TEdge> result))
                return result;
            return Enumerable.Empty<TEdge>();
        }

        /// <inheritdoc />
        public int AdjacentDegree(TVertex vertex)
        {
            return AdjacentEdges(vertex).Count();
        }

        /// <inheritdoc />
        public bool IsAdjacentEdgesEmpty(TVertex vertex)
        {
            return !AdjacentEdges(vertex).Any();
        }

        /// <inheritdoc />
        public TEdge AdjacentEdge(TVertex vertex, int index)
        {
            return AdjacentEdges(vertex).ElementAt(index);
        }

        /// <inheritdoc />
        public bool TryGetEdge(TVertex source, TVertex target, out TEdge edge)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            if (TryGetAdjacentEdges(source, out IEnumerable<TEdge> adjacentEdges))
            {
                foreach (TEdge adjacentEdge in adjacentEdges)
                {
                    if (EdgeEqualityComparer(adjacentEdge, source, target))
                    {
                        edge = adjacentEdge;
                        return true;
                    }
                }
            }

            edge = default(TEdge);
            return false;
        }

        /// <summary>
        /// Tries to get adjacent edges of the given <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="edges">Edges found, otherwise null.</param>
        /// <returns>True if at least one edge was found, false otherwise.</returns>
        public bool TryGetAdjacentEdges(TVertex vertex, out IEnumerable<TEdge> edges)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            return _tryGetAdjacencyEdges(vertex, out edges);
        }

        /// <inheritdoc />
        public bool ContainsEdge(TVertex source, TVertex target)
        {
            return TryGetEdge(source, target, out _);
        }

        /// <inheritdoc />
        public bool ContainsVertex(TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            return _tryGetAdjacencyEdges(vertex, out _);
        }

        #endregion
    }
}
