using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// Immutable undirected graph data structure.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    [DebuggerDisplay("VertexCount = {" + nameof(VertexCount) + "}, EdgeCount = {" + nameof(EdgeCount) + "}")]
    public sealed class ArrayUndirectedGraph<TVertex, TEdge> : IUndirectedGraph<TVertex, TEdge>
#if SUPPORTS_CLONEABLE
        , ICloneable
#endif
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayUndirectedGraph{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        public ArrayUndirectedGraph([NotNull] IUndirectedGraph<TVertex, TEdge> visitedGraph)
        {
            if (visitedGraph is null)
                throw new ArgumentNullException(nameof(visitedGraph));

            AllowParallelEdges = visitedGraph.AllowParallelEdges;
            EdgeEqualityComparer = visitedGraph.EdgeEqualityComparer;
            EdgeCount = visitedGraph.EdgeCount;
            _vertexEdges = new Dictionary<TVertex, TEdge[]>(visitedGraph.VertexCount);
            foreach (TVertex vertex in visitedGraph.Vertices)
            {
                _vertexEdges.Add(
                    vertex,
                    visitedGraph.AdjacentEdges(vertex).ToArray());
            }

            _edges = new List<TEdge>(visitedGraph.Edges);
        }

        /// <inheritdoc />
        public EdgeEqualityComparer<TVertex> EdgeEqualityComparer { get; }

        #region IGraph<TVertex,TEdge>

        /// <inheritdoc />
        public bool IsDirected => false;

        /// <inheritdoc />
        public bool AllowParallelEdges { get; }

        #endregion

        #region IVertexSet<TVertex>

        /// <inheritdoc />
        public bool IsVerticesEmpty => _vertexEdges.Count == 0;

        /// <inheritdoc />
        public int VertexCount => _vertexEdges.Count;

        [NotNull]
        private readonly Dictionary<TVertex, TEdge[]> _vertexEdges;

        /// <inheritdoc />
        public IEnumerable<TVertex> Vertices => _vertexEdges.Keys.AsEnumerable();

        #endregion

        #region IImplicitVertexSet<TVertex>

        /// <inheritdoc />
        public bool ContainsVertex(TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            return _vertexEdges.ContainsKey(vertex);
        }

        #endregion

        #region IEdgeSet<TVertex,TEdge>

        /// <inheritdoc />
        public bool IsEdgesEmpty => EdgeCount == 0;

        /// <inheritdoc />
        public int EdgeCount { get; }

        [NotNull, ItemNotNull]
        private readonly IList<TEdge> _edges;

        /// <inheritdoc />
        public IEnumerable<TEdge> Edges => _edges.AsEnumerable();

        /// <inheritdoc />
        public bool ContainsEdge(TEdge edge)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            if (_vertexEdges.TryGetValue(edge.Source, out TEdge[] edges))
                return edges.Any(e => EqualityComparer<TEdge>.Default.Equals(e, edge));
            return false;
        }

        /// <inheritdoc />
        public bool ContainsEdge(TVertex source, TVertex target)
        {
            return TryGetEdge(source, target, out _);
        }

        #endregion

        #region IImplicitUndirectedGraph<TVertex,TEdge>

        /// <inheritdoc />
        public IEnumerable<TEdge> AdjacentEdges(TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            if (_vertexEdges.TryGetValue(vertex, out TEdge[] edges))
                return edges.AsEnumerable();
            throw new VertexNotFoundException();
        }

        /// <inheritdoc />
        public int AdjacentDegree(TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            if (_vertexEdges.TryGetValue(vertex, out TEdge[] edges))
                return edges.Sum(edge => edge.IsSelfEdge() ? 2 : 1);    // Self edge count twice
            throw new VertexNotFoundException();
        }

        /// <inheritdoc />
        public bool IsAdjacentEdgesEmpty(TVertex vertex)
        {
            return AdjacentDegree(vertex) == 0;
        }

        /// <inheritdoc />
        public TEdge AdjacentEdge(TVertex vertex, int index)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            if (_vertexEdges.TryGetValue(vertex, out TEdge[] adjacentEdges))
                return adjacentEdges[index];
            throw new VertexNotFoundException();
        }

        /// <inheritdoc />
        public bool TryGetEdge(TVertex source, TVertex target, out TEdge edge)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            if (Comparer<TVertex>.Default.Compare(source, target) > 0)
            {
                TVertex temp = source;
                source = target;
                target = temp;
            }

            if (!_vertexEdges.TryGetValue(source, out TEdge[] adjacentEdges))
            {
                edge = default(TEdge);
                return false;
            }

            foreach (TEdge adjacentEdge in adjacentEdges)
            {
                if (EdgeEqualityComparer(adjacentEdge, source, target))
                {
                    edge = adjacentEdge;
                    return true;
                }
            }

            edge = default(TEdge);
            return false;
        }

        #endregion

        #region ICloneable

        /// <summary>
        /// Clones this graph, returns this instance because this class is immutable.
        /// </summary>
        /// <returns>This graph.</returns>
        [Pure]
        [NotNull]
        public ArrayUndirectedGraph<TVertex, TEdge> Clone()
        {
            return this;
        }

#if SUPPORTS_CLONEABLE
        /// <inheritdoc />
        object ICloneable.Clone()
        {
            return Clone();
        }
#endif
        #endregion
    }
}