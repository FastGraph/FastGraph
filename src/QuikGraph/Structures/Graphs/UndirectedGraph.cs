using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
#if !SUPPORTS_TYPE_FULL_FEATURES
using System.Reflection;
#endif
using JetBrains.Annotations;
using QuikGraph.Collections;

namespace QuikGraph
{
    /// <summary>
    /// Mutable undirected graph data structure.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    [DebuggerDisplay("VertexCount = {" + nameof(VertexCount) + "}, EdgeCount = {" + nameof(EdgeCount) + "}")]
    public class UndirectedGraph<TVertex, TEdge> : IMutableUndirectedGraph<TVertex, TEdge>
#if SUPPORTS_CLONEABLE
        , ICloneable
#endif
        where TEdge : IEdge<TVertex>
    {
        [NotNull]
        private readonly IVertexEdgeDictionary<TVertex, TEdge> _adjacentEdges =
            new VertexEdgeDictionary<TVertex, TEdge>();

        /// <summary>
        /// Initializes a new instance of the <see cref="UndirectedGraph{TVertex,TEdge}"/> class.
        /// </summary>
        /// <remarks>Allow parallel edges and gets the best edge equality comparer.</remarks>
        public UndirectedGraph()
            : this(true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UndirectedGraph{TVertex,TEdge}"/> class.
        /// </summary>
        /// <remarks>Gets the best edge equality comparer.</remarks>
        /// <param name="allowParallelEdges">Indicates if parallel edges are allowed.</param>
        public UndirectedGraph(bool allowParallelEdges)
            : this(allowParallelEdges, EdgeExtensions.GetUndirectedVertexEquality<TVertex, TEdge>())
        {
            AllowParallelEdges = allowParallelEdges;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UndirectedGraph{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="allowParallelEdges">Indicates if parallel edges are allowed.</param>
        /// <param name="edgeEqualityComparer">Equality comparer to use to compare edges.</param>
        public UndirectedGraph(bool allowParallelEdges, [NotNull] EdgeEqualityComparer<TVertex> edgeEqualityComparer)
        {
            AllowParallelEdges = allowParallelEdges;
            EdgeEqualityComparer = edgeEqualityComparer ?? throw new ArgumentNullException(nameof(edgeEqualityComparer));

            _reorder = typeof(IUndirectedEdge<TVertex>).IsAssignableFrom(typeof(TEdge))
                ? (ReorderVertices)((TVertex source, TVertex target, out TVertex orderedSource, out TVertex orderedTarget) =>
                {
                    if (Comparer<TVertex>.Default.Compare(source, target) > 0)
                    {
                        orderedSource = target;
                        orderedTarget = source;
                    }
                    else
                    {
                        orderedSource = source;
                        orderedTarget = target;
                    }
                })
                : (TVertex source, TVertex target, out TVertex orderedSource, out TVertex orderedTarget) =>
                {
                    orderedSource = source;
                    orderedTarget = target;
                };
        }

        private delegate void ReorderVertices(
            [NotNull] TVertex source,
            [NotNull] TVertex target,
            [NotNull] out TVertex orderedSource,
            [NotNull] out TVertex orderedTarget);

        [NotNull]
        private readonly ReorderVertices _reorder;

        /// <inheritdoc />
        public EdgeEqualityComparer<TVertex> EdgeEqualityComparer { get; }

        /// <summary>
        /// Gets or sets the edge capacity.
        /// </summary>
        public int EdgeCapacity { get; set; } = -1;

        /// <summary>
        /// Gets the set of vertices adjacent to the given <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">Vertex to get adjacent ones.</param>
        /// <returns>Set of adjacent vertices.</returns>
        [NotNull, ItemNotNull]
        public IEnumerable<TVertex> AdjacentVertices([NotNull] TVertex vertex)
        {
            IEnumerable<TEdge> adjacentEdges = AdjacentEdges(vertex);
            var adjacentVertices = new HashSet<TVertex>();
            foreach (TEdge edge in adjacentEdges)
            {
                adjacentVertices.Add(edge.Source);
                adjacentVertices.Add(edge.Target);
            }

            adjacentVertices.Remove(vertex);

            return adjacentVertices;
        }

        #region IGraph<TVertex,TEdge>

        /// <inheritdoc />
        public bool IsDirected => false;

        /// <inheritdoc />
        public bool AllowParallelEdges { get; }

        #endregion

        #region IVertexSet<TVertex>

        /// <inheritdoc />
        public bool IsVerticesEmpty => _adjacentEdges.Count == 0;

        /// <inheritdoc />
        public int VertexCount => _adjacentEdges.Count;

        /// <inheritdoc />
        public IEnumerable<TVertex> Vertices => _adjacentEdges.Keys.AsEnumerable();

        /// <inheritdoc />
        public bool ContainsVertex(TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            return _adjacentEdges.ContainsKey(vertex);
        }

        #endregion

        #region IEdgeSet<TVertex,TEdge>

        /// <inheritdoc />
        public bool IsEdgesEmpty => EdgeCount == 0;

        /// <inheritdoc />
        public int EdgeCount { get; private set; }

        [NotNull, ItemNotNull]
        private readonly IList<TEdge> _edges = new List<TEdge>();

        /// <inheritdoc />
        public IEnumerable<TEdge> Edges => _edges.AsEnumerable();

        /// <inheritdoc />
        public bool ContainsEdge(TEdge edge)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            if (!_adjacentEdges.TryGetValue(edge.Source, out IEdgeList<TVertex, TEdge> adjacentEdges))
                return false;

            foreach (TEdge adjacentEdge in adjacentEdges)
            {
                if (EqualityComparer<TEdge>.Default.Equals(adjacentEdge, edge))
                    return true;
            }

            return false;
        }

        /// <inheritdoc />
        public bool ContainsEdge(TVertex source, TVertex target)
        {
            return TryGetEdge(source, target, out _);
        }

        private bool ContainsEdgeBetweenVertices([NotNull, ItemNotNull] IEnumerable<TEdge> edges, [NotNull] TEdge edge)
        {
            Debug.Assert(edges != null);
            Debug.Assert(edge != null);

            TVertex source = edge.Source;
            TVertex target = edge.Target;
            foreach (TEdge e in edges)
            {
                if (EdgeEqualityComparer(e, source, target))
                    return true;
            }

            return false;
        }

        #endregion

        #region IUndirectedGraph<TVertex,TEdge>

        /// <inheritdoc />
        public IEnumerable<TEdge> AdjacentEdges(TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            if (_adjacentEdges.TryGetValue(vertex, out IEdgeList<TVertex, TEdge> edges))
                return edges.AsEnumerable();
            throw new VertexNotFoundException();
        }

        /// <inheritdoc />
        public int AdjacentDegree(TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            if (_adjacentEdges.TryGetValue(vertex, out IEdgeList<TVertex, TEdge> edges))
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

            if (_adjacentEdges.TryGetValue(vertex, out IEdgeList<TVertex, TEdge> adjacentEdges))
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

            _reorder(source, target, out source, out target);

            if (!_adjacentEdges.TryGetValue(source, out IEdgeList<TVertex, TEdge> adjacentEdges))
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

        #region IMutableGraph<TVertex,TEdge>

        /// <summary>
        /// Trims excess storage allocated for edges.
        /// </summary>
        public void TrimEdgeExcess()
        {
            foreach (IEdgeList<TVertex, TEdge> edges in _adjacentEdges.Values)
                edges.TrimExcess();
        }

        /// <inheritdoc />
        public void Clear()
        {
            if (EdgeRemoved != null) // Lazily notify
            {
                foreach (TEdge edge in _edges)
                    OnEdgeRemoved(edge);
                foreach (TVertex vertex in _adjacentEdges.Keys)
                    OnVertexRemoved(vertex);
            }

            _edges.Clear();
            _adjacentEdges.Clear();
            EdgeCount = 0;
        }

        #endregion

        #region IMutableUndirected<TVertex,TEdge>

        /// <inheritdoc />
        public event VertexAction<TVertex> VertexAdded;

        /// <summary>
        /// Called on each added vertex.
        /// </summary>
        /// <param name="vertex">Added vertex.</param>
        protected virtual void OnVertexAdded([NotNull] TVertex vertex)
        {
            Debug.Assert(vertex != null);

            VertexAdded?.Invoke(vertex);
        }

        /// <inheritdoc />
        public int AddVertexRange(IEnumerable<TVertex> vertices)
        {
            if (vertices is null)
                throw new ArgumentNullException(nameof(vertices));
            TVertex[] verticesArray = vertices.ToArray();
            if (verticesArray.Any(v => v == null))
                throw new ArgumentNullException(nameof(vertices), "At least one vertex is null.");

            int count = 0;
            foreach (TVertex vertex in verticesArray)
            {
                if (AddVertex(vertex))
                    ++count;
            }

            return count;
        }

        /// <inheritdoc />
        public bool AddVertex(TVertex vertex)
        {
            if (ContainsVertex(vertex))
                return false;

            var edges = EdgeCapacity < 0
                ? new EdgeList<TVertex, TEdge>()
                : new EdgeList<TVertex, TEdge>(EdgeCapacity);

            _adjacentEdges.Add(vertex, edges);
            OnVertexAdded(vertex);

            return true;
        }

        [NotNull]
        private IEdgeList<TVertex, TEdge> AddAndReturnEdges([NotNull] TVertex vertex)
        {
            Debug.Assert(vertex != null);

            if (!_adjacentEdges.TryGetValue(vertex, out IEdgeList<TVertex, TEdge> edges))
            {
                _adjacentEdges[vertex] = edges = EdgeCapacity < 0
                    ? new EdgeList<TVertex, TEdge>()
                    : new EdgeList<TVertex, TEdge>(EdgeCapacity);

                OnVertexAdded(vertex);
            }

            return edges;
        }

        /// <inheritdoc />
        public event VertexAction<TVertex> VertexRemoved;

        /// <summary>
        /// Called for each removed vertex.
        /// </summary>
        /// <param name="vertex">Removed vertex.</param>
        protected virtual void OnVertexRemoved([NotNull] TVertex vertex)
        {
            Debug.Assert(vertex != null);

            VertexRemoved?.Invoke(vertex);
        }

        /// <inheritdoc />
        public bool RemoveVertex(TVertex vertex)
        {
            ClearAdjacentEdges(vertex);

            bool removed = _adjacentEdges.Remove(vertex);
            if (removed)
                OnVertexRemoved(vertex);

            return removed;
        }

        /// <inheritdoc />
        public int RemoveVertexIf(VertexPredicate<TVertex> predicate)
        {
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            TVertex[] verticesToRemove = Vertices
                .Where(vertex => predicate(vertex))
                .ToArray();

            foreach (TVertex vertex in verticesToRemove)
                RemoveVertex(vertex);

            return verticesToRemove.Length;
        }

        #endregion

        #region IMutableIncidenceGraph<TVertex,TEdge>

        /// <inheritdoc />
        public int RemoveAdjacentEdgeIf(TVertex vertex, EdgePredicate<TVertex, TEdge> predicate)
        {
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));
            if (!_adjacentEdges.TryGetValue(vertex, out IEdgeList<TVertex, TEdge> adjacentEdges))
                return 0;

            var edges = new List<TEdge>();
            edges.AddRange(adjacentEdges.Where(edge => predicate(edge)));

            RemoveEdges(edges);
            return edges.Count;
        }

        /// <inheritdoc />
        public void ClearAdjacentEdges(TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            if (!_adjacentEdges.TryGetValue(vertex, out IEdgeList<TVertex, TEdge> adjacentEdges))
                return;

            IEdgeList<TVertex, TEdge> edgesToRemove = adjacentEdges.Clone();
            EdgeCount -= edgesToRemove.Count;

            foreach (TEdge edge in edgesToRemove)
            {
                if (_adjacentEdges.TryGetValue(edge.Target, out adjacentEdges))
                    adjacentEdges.Remove(edge);

                if (_adjacentEdges.TryGetValue(edge.Source, out adjacentEdges))
                    adjacentEdges.Remove(edge);

                _edges.Remove(edge);
                OnEdgeRemoved(edge);
            }
        }

        /// <summary>
        /// Clears edges of the given <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        public void ClearEdges([NotNull] TVertex vertex)
        {
            ClearAdjacentEdges(vertex);
        }

        #endregion

        #region IMutableEdgeListGraph<TVertex,TEdge>

        /// <inheritdoc />
        public bool AddVerticesAndEdge(TEdge edge)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            IEdgeList<TVertex, TEdge> sourceEdges = AddAndReturnEdges(edge.Source);
            IEdgeList<TVertex, TEdge> targetEdges = AddAndReturnEdges(edge.Target);

            if (!AllowParallelEdges && ContainsEdgeBetweenVertices(sourceEdges, edge))
                return false;

            _edges.Add(edge);
            sourceEdges.Add(edge);
            if (!edge.IsSelfEdge())
                targetEdges.Add(edge);

            ++EdgeCount;
            OnEdgeAdded(edge);

            return true;
        }

        /// <inheritdoc />
        public int AddVerticesAndEdgeRange(IEnumerable<TEdge> edges)
        {
            if (edges is null)
                throw new ArgumentNullException(nameof(edges));
            TEdge[] edgesArray = edges.ToArray();
            if (edgesArray.Any(e => e == null))
                throw new ArgumentNullException(nameof(edges), "At least one edge is null.");

            int count = 0;
            foreach (TEdge edge in edgesArray)
            {
                if (AddVerticesAndEdge(edge))
                    ++count;
            }

            return count;
        }

        /// <inheritdoc />
        public bool AddEdge(TEdge edge)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            if (!_adjacentEdges.TryGetValue(edge.Source, out IEdgeList<TVertex, TEdge> sourceEdges))
                throw new VertexNotFoundException();
            if (!_adjacentEdges.TryGetValue(edge.Target, out IEdgeList<TVertex, TEdge> targetEdges))
                throw new VertexNotFoundException();

            if (!AllowParallelEdges && ContainsEdgeBetweenVertices(sourceEdges, edge))
                return false;

            _edges.Add(edge);
            sourceEdges.Add(edge);
            if (!edge.IsSelfEdge())
            {
                targetEdges.Add(edge);
            }

            ++EdgeCount;
            OnEdgeAdded(edge);

            return true;
        }

        /// <inheritdoc />
        public int AddEdgeRange(IEnumerable<TEdge> edges)
        {
            if (edges is null)
                throw new ArgumentNullException(nameof(edges));
            TEdge[] edgesArray = edges.ToArray();
            if (edgesArray.Any(e => e == null))
                throw new ArgumentNullException(nameof(edges), "At least one edge is null.");

            int count = 0;
            foreach (TEdge edge in edgesArray)
            {
                if (AddEdge(edge))
                    ++count;
            }

            return count;
        }

        /// <inheritdoc />
        public event EdgeAction<TVertex, TEdge> EdgeAdded;

        /// <summary>
        /// Called on each added edge.
        /// </summary>
        /// <param name="edge">Added edge.</param>
        protected virtual void OnEdgeAdded([NotNull] TEdge edge)
        {
            Debug.Assert(edge != null);

            EdgeAdded?.Invoke(edge);
        }

        /// <inheritdoc />
        public bool RemoveEdge(TEdge edge)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            if (_adjacentEdges.TryGetValue(edge.Source, out IEdgeList<TVertex, TEdge> adjacentEdges)
                && adjacentEdges.Remove(edge))
            {
                _edges.Remove(edge);

                if (!edge.IsSelfEdge())
                    _adjacentEdges[edge.Target].Remove(edge);
                --EdgeCount;

                Debug.Assert(EdgeCount >= 0);

                OnEdgeRemoved(edge);
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public event EdgeAction<TVertex, TEdge> EdgeRemoved;

        /// <summary>
        /// Called on each removed edge.
        /// </summary>
        /// <param name="edge">Removed edge.</param>
        protected virtual void OnEdgeRemoved([NotNull] TEdge edge)
        {
            Debug.Assert(edge != null);

            EdgeRemoved?.Invoke(edge);
        }

        /// <inheritdoc />
        public int RemoveEdgeIf(EdgePredicate<TVertex, TEdge> predicate)
        {
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return RemoveEdges(
                Edges.Where(edge => predicate(edge)).ToArray());
        }

        /// <summary>
        /// Removes the given set of edges.
        /// </summary>
        /// <param name="edges">Edges to remove.</param>
        /// <returns>The number of removed edges.</returns>
        public int RemoveEdges([NotNull, ItemNotNull] IEnumerable<TEdge> edges)
        {
            if (edges is null)
                throw new ArgumentNullException(nameof(edges));
            TEdge[] edgesArray = edges.ToArray();
            if (edgesArray.Any(e => e == null))
                throw new ArgumentNullException(nameof(edges), "At least one edge is null.");

            int count = 0;
            foreach (TEdge edge in edgesArray)
            {
                if (RemoveEdge(edge))
                    ++count;
            }

            return count;
        }

        #endregion

        #region ICloneable

        private UndirectedGraph(
            [NotNull, ItemNotNull] IList<TEdge> edges,
            [NotNull] IVertexEdgeDictionary<TVertex, TEdge> adjacentEdges,
            [NotNull] EdgeEqualityComparer<TVertex> edgeEqualityComparer,
            int edgeCapacity,
            bool allowParallelEdges)
            : this(allowParallelEdges, edgeEqualityComparer)
        {
            Debug.Assert(edges != null);
            Debug.Assert(adjacentEdges != null);

            _edges = edges;
            _adjacentEdges = adjacentEdges;
            EdgeCount = edges.Count;
            EdgeCapacity = edgeCapacity;
        }

        /// <summary>
        /// Clones this graph.
        /// </summary>
        /// <returns>Cloned graph.</returns>
        [Pure]
        [NotNull]
        public UndirectedGraph<TVertex, TEdge> Clone()
        {
            return new UndirectedGraph<TVertex, TEdge>(
                _edges.ToList(),
                _adjacentEdges.Clone(),
                EdgeEqualityComparer,
                EdgeCapacity,
                AllowParallelEdges);
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