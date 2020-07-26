using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using QuikGraph.Collections;

namespace QuikGraph
{
    /// <summary>
    /// Mutable directed graph data structure.
    /// </summary>
    /// <remarks>
    /// It is efficient for sparse graph representation
    /// where out-edge need to be enumerated only.
    /// </remarks>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    [DebuggerDisplay("VertexCount = {" + nameof(VertexCount) + ("}, EdgeCount = {" + nameof(EdgeCount) + "}"))]
    public class AdjacencyGraph<TVertex, TEdge> : IEdgeListAndIncidenceGraph<TVertex, TEdge>, IMutableVertexAndEdgeListGraph<TVertex, TEdge>
#if SUPPORTS_CLONEABLE
        , ICloneable
#endif
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdjacencyGraph{TVertex,TEdge}"/> class.
        /// </summary>
        public AdjacencyGraph()
            : this(true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdjacencyGraph{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="allowParallelEdges">Indicates if parallel edges are allowed.</param>
        public AdjacencyGraph(bool allowParallelEdges)
            : this(allowParallelEdges, -1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdjacencyGraph{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="allowParallelEdges">Indicates if parallel edges are allowed.</param>
        /// <param name="capacity">Vertex capacity.</param>
        public AdjacencyGraph(bool allowParallelEdges, int capacity)
            : this(allowParallelEdges, capacity, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdjacencyGraph{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="allowParallelEdges">Indicates if parallel edges are allowed.</param>
        /// <param name="vertexCapacity">Vertex capacity.</param>
        /// <param name="edgeCapacity">Edge capacity.</param>
        public AdjacencyGraph(bool allowParallelEdges, int vertexCapacity, int edgeCapacity)
        {
            AllowParallelEdges = allowParallelEdges;
            _vertexEdges = vertexCapacity > -1
                ? new VertexEdgeDictionary<TVertex, TEdge>(vertexCapacity)
                : new VertexEdgeDictionary<TVertex, TEdge>();
            EdgeCapacity = edgeCapacity;
        }

        /// <summary>
        /// Gets or sets the edge capacity.
        /// </summary>
        public int EdgeCapacity { get; set; }

        /// <summary>
        /// Gets the type of vertices.
        /// </summary>
        [NotNull]
        public Type VertexType => typeof(TVertex);

        /// <summary>
        /// Gets the type of edges.
        /// </summary>
        [NotNull]
        public Type EdgeType => typeof(TEdge);

        #region IGraph<TVertex,TEdge>

        /// <inheritdoc />
        public bool IsDirected => true;

        /// <inheritdoc />
        public bool AllowParallelEdges { get; }

        #endregion

        #region IVertexSet<TVertex>

        /// <inheritdoc />
        public bool IsVerticesEmpty => VertexCount == 0;

        /// <inheritdoc />
        public int VertexCount => _vertexEdges.Count;

        [NotNull]
        private readonly IVertexEdgeDictionary<TVertex, TEdge> _vertexEdges;

        /// <inheritdoc />
        public virtual IEnumerable<TVertex> Vertices => _vertexEdges.Keys.AsEnumerable();

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
        public int EdgeCount { get; private set; }

        /// <inheritdoc />
        public virtual IEnumerable<TEdge> Edges => _vertexEdges.Values.SelectMany(edges => edges);

        /// <inheritdoc />
        public bool ContainsEdge(TEdge edge)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            return _vertexEdges.TryGetValue(edge.Source, out IEdgeList<TVertex, TEdge> edges)
                   && edges.Contains(edge);
        }

        #endregion

        #region IIncidenceGraph<TVertex,TEdge>

        /// <inheritdoc />
        public bool ContainsEdge(TVertex source, TVertex target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            if (TryGetOutEdges(source, out IEnumerable<TEdge> outEdges))
                return outEdges.Any(edge => EqualityComparer<TVertex>.Default.Equals(edge.Target, target));
            return false;
        }

        /// <inheritdoc />
        public bool TryGetEdge(TVertex source, TVertex target, out TEdge edge)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            if (_vertexEdges.TryGetValue(source, out IEdgeList<TVertex, TEdge> edgeList)
                && edgeList.Count > 0)
            {
                foreach (TEdge e in edgeList)
                {
                    if (EqualityComparer<TVertex>.Default.Equals(e.Target, target))
                    {
                        edge = e;
                        return true;
                    }
                }
            }

            edge = default(TEdge);
            return false;
        }

        /// <inheritdoc />
        public virtual bool TryGetEdges(TVertex source, TVertex target, out IEnumerable<TEdge> edges)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            if (_vertexEdges.TryGetValue(source, out IEdgeList<TVertex, TEdge> outEdges))
            {
                edges = outEdges.Where(edge => EqualityComparer<TVertex>.Default.Equals(edge.Target, target));
                return true;
            }

            edges = null;
            return false;
        }

        #endregion

        #region IImplicitGraph<TVertex,TEdge>

        /// <inheritdoc />
        public bool IsOutEdgesEmpty(TVertex vertex)
        {
            return OutDegree(vertex) == 0;
        }

        /// <inheritdoc />
        public int OutDegree(TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            if (_vertexEdges.TryGetValue(vertex, out IEdgeList<TVertex, TEdge> edges))
                return edges.Count;
            throw new VertexNotFoundException();
        }

        /// <inheritdoc />
        public virtual IEnumerable<TEdge> OutEdges(TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            if (_vertexEdges.TryGetValue(vertex, out IEdgeList<TVertex, TEdge> outEdges))
                return outEdges.AsEnumerable();
            throw new VertexNotFoundException();
        }

        /// <inheritdoc />
        public virtual bool TryGetOutEdges(TVertex vertex, out IEnumerable<TEdge> edges)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            if (_vertexEdges.TryGetValue(vertex, out IEdgeList<TVertex, TEdge> edgeList))
            {
                edges = edgeList.AsEnumerable();
                return true;
            }

            edges = null;
            return false;
        }

        /// <inheritdoc />
        public TEdge OutEdge(TVertex vertex, int index)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            if (_vertexEdges.TryGetValue(vertex, out IEdgeList<TVertex, TEdge> outEdges))
                return outEdges[index];
            throw new VertexNotFoundException();
        }

        #endregion

        #region IMutableVertexAndEdgeSet<TVertex,TEdge>

        /// <inheritdoc />
        public virtual bool AddVertex(TVertex vertex)
        {
            if (ContainsVertex(vertex))
                return false;

            if (EdgeCapacity > 0)
                _vertexEdges.Add(vertex, new EdgeList<TVertex, TEdge>(EdgeCapacity));
            else
                _vertexEdges.Add(vertex, new EdgeList<TVertex, TEdge>());

            OnVertexAdded(vertex);

            return true;
        }

        /// <inheritdoc />
        public virtual int AddVertexRange(IEnumerable<TVertex> vertices)
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
        public virtual bool RemoveVertex(TVertex vertex)
        {
            if (!ContainsVertex(vertex))
                return false;

            // Remove out edges
            IEdgeList<TVertex, TEdge> edges = _vertexEdges[vertex];
            if (EdgeRemoved != null) // Lazily notify
            {
                foreach (TEdge edge in edges)
                    OnEdgeRemoved(edge);
            }

            EdgeCount -= edges.Count;
            edges.Clear();

            // Run over edges and remove each edge touching the vertex
            foreach (KeyValuePair<TVertex, IEdgeList<TVertex, TEdge>> pair in _vertexEdges)
            {
                if (EqualityComparer<TVertex>.Default.Equals(pair.Key, vertex))
                    continue; // We've already

                // Collect edges to remove
                foreach (TEdge edge in pair.Value.Clone())
                {
                    if (EqualityComparer<TVertex>.Default.Equals(edge.Target, vertex))
                    {
                        pair.Value.Remove(edge);
                        OnEdgeRemoved(edge);
                        --EdgeCount;
                    }
                }
            }

            Debug.Assert(EdgeCount >= 0);

            _vertexEdges.Remove(vertex);
            OnVertexRemoved(vertex);

            return true;
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
        public int RemoveVertexIf(VertexPredicate<TVertex> predicate)
        {
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            var vertices = new VertexList<TVertex>();
            vertices.AddRange(Vertices.Where(vertex => predicate(vertex)));

            foreach (TVertex vertex in vertices)
                RemoveVertex(vertex);

            return vertices.Count;
        }

        /// <inheritdoc />
        public virtual bool AddVerticesAndEdge(TEdge edge)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            AddVertex(edge.Source);
            AddVertex(edge.Target);
            return AddEdgeInternal(edge);
        }

        /// <summary>
        /// Adds a range of edges to the graph.
        /// </summary>
        /// <param name="edges">Edges to add.</param>
        /// <returns>The number of edges that were added.</returns>
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

        #endregion

        #region IMutableEdgeListGraph<TVertex,TEdge>

        private bool AddEdgeInternal([NotNull] TEdge edge)
        {
            Debug.Assert(edge != null);

            if (!AllowParallelEdges && ContainsEdge(edge.Source, edge.Target))
                return false;

            _vertexEdges[edge.Source].Add(edge);
            ++EdgeCount;

            OnEdgeAdded(edge);

            return true;
        }

        /// <inheritdoc />
        public virtual bool AddEdge(TEdge edge)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));
            if (!ContainsVertex(edge.Source) || !ContainsVertex(edge.Target))
                throw new VertexNotFoundException();

            return AddEdgeInternal(edge);
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
        public virtual bool RemoveEdge(TEdge edge)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            if (_vertexEdges.TryGetValue(edge.Source, out IEdgeList<TVertex, TEdge> edges)
                && edges.Remove(edge))
            {
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

            var edgesToRemove = new EdgeList<TVertex, TEdge>();
            edgesToRemove.AddRange(Edges.Where(edge => predicate(edge)));

            foreach (TEdge edge in edgesToRemove)
            {
                OnEdgeRemoved(edge);
                _vertexEdges[edge.Source].Remove(edge);
            }

            EdgeCount -= edgesToRemove.Count;

            return edgesToRemove.Count;
        }

        #endregion

        #region IMutableIncidenceGraph<TVertex,TEdge>

        /// <inheritdoc />
        public int RemoveOutEdgeIf(TVertex vertex, EdgePredicate<TVertex, TEdge> predicate)
        {
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));
            if (!_vertexEdges.TryGetValue(vertex, out IEdgeList<TVertex, TEdge> outEdges))
                return 0;

            var edgesToRemove = new EdgeList<TVertex, TEdge>();
            edgesToRemove.AddRange(outEdges.Where(edge => predicate(edge)));

            foreach (TEdge edge in edgesToRemove)
                RemoveEdge(edge);

            return edgesToRemove.Count;
        }

        /// <inheritdoc />
        public void ClearOutEdges(TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            if (!_vertexEdges.TryGetValue(vertex, out IEdgeList<TVertex, TEdge> outEdges))
                return;

            int count = outEdges.Count;
            if (EdgeRemoved != null) // Lazily notify
            {
                foreach (TEdge edge in outEdges)
                    OnEdgeRemoved(edge);
            }

            outEdges.Clear();
            EdgeCount -= count;
        }

        /// <summary>
        /// Clears edges of the given <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        public void ClearEdges([NotNull] TVertex vertex)
        {
            ClearOutEdges(vertex);
        }

        /// <inheritdoc />
        public void TrimEdgeExcess()
        {
            foreach (IEdgeList<TVertex, TEdge> edges in _vertexEdges.Values)
                edges.TrimExcess();
        }

        #endregion

        #region IMutableGraph<TVertex,TEdge>

        /// <inheritdoc />
        public void Clear()
        {
            if (EdgeRemoved != null) // Lazily notify
            {
                foreach (TEdge edge in _vertexEdges.SelectMany(edges => edges.Value).Distinct())
                    OnEdgeRemoved(edge);
                foreach (TVertex vertex in _vertexEdges.Keys)
                    OnVertexRemoved(vertex);
            }

            _vertexEdges.Clear();
            EdgeCount = 0;
        }

        #endregion

        #region ICloneable

        private AdjacencyGraph(
            [NotNull] IVertexEdgeDictionary<TVertex, TEdge> vertexEdges,
            int edgeCount,
            int edgeCapacity,
            bool allowParallelEdges)
        {
            Debug.Assert(vertexEdges != null);
            Debug.Assert(edgeCount >= 0);

            _vertexEdges = vertexEdges;
            EdgeCount = edgeCount;
            EdgeCapacity = edgeCapacity;
            AllowParallelEdges = allowParallelEdges;
        }

        /// <summary>
        /// Clones this graph.
        /// </summary>
        /// <returns>Cloned graph.</returns>
        [Pure]
        [NotNull]
        public AdjacencyGraph<TVertex, TEdge> Clone()
        {
            return new AdjacencyGraph<TVertex, TEdge>(
                _vertexEdges.Clone(),
                EdgeCount,
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