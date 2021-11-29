﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
#if SUPPORTS_AGGRESSIVE_INLINING
using System.Runtime.CompilerServices;
#endif
using JetBrains.Annotations;
using FastGraph.Collections;

namespace FastGraph
{
    /// <summary>
    /// Mutable directed graph data structure.
    /// </summary>
    /// <remarks>
    /// It is efficient for sparse graph representation
    /// where out-edge and in-edges need to be enumerated.
    /// Requires twice as much memory as the <see cref="AdjacencyGraph{TVertex,TEdge}"/>.
    /// </remarks>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    [DebuggerDisplay("VertexCount = {" + nameof(VertexCount) + "}, EdgeCount = {" + nameof(EdgeCount) + "}")]
    public class BidirectionalGraph<TVertex, TEdge>
        : IEdgeListAndIncidenceGraph<TVertex, TEdge>
        , IMutableBidirectionalGraph<TVertex, TEdge>
#if SUPPORTS_CLONEABLE
        , ICloneable
#endif
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BidirectionalGraph{TVertex,TEdge}"/> class.
        /// </summary>
        public BidirectionalGraph()
            : this(true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BidirectionalGraph{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="allowParallelEdges">Indicates if parallel edges are allowed.</param>
        public BidirectionalGraph(bool allowParallelEdges)
            : this(allowParallelEdges, -1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BidirectionalGraph{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="allowParallelEdges">Indicates if parallel edges are allowed.</param>
        /// <param name="capacity">Vertex capacity.</param>
        public BidirectionalGraph(bool allowParallelEdges, int capacity)
            : this(allowParallelEdges, capacity, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BidirectionalGraph{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="allowParallelEdges">Indicates if parallel edges are allowed.</param>
        /// <param name="vertexCapacity">Vertex capacity.</param>
        /// <param name="edgeCapacity">Edge capacity.</param>
        public BidirectionalGraph(bool allowParallelEdges, int vertexCapacity, int edgeCapacity)
        {
            AllowParallelEdges = allowParallelEdges;
            if (vertexCapacity > -1)
            {
                _vertexInEdges = new VertexEdgeDictionary<TVertex, TEdge>(vertexCapacity);
                _vertexOutEdges = new VertexEdgeDictionary<TVertex, TEdge>(vertexCapacity);
            }
            else
            {
                _vertexInEdges = new VertexEdgeDictionary<TVertex, TEdge>();
                _vertexOutEdges = new VertexEdgeDictionary<TVertex, TEdge>();
            }
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
        /// Gives the type of edges.
        /// </summary>
        public Type EdgeType => typeof(TEdge);

        #region IGraph<TVertex,TEdge>

        /// <inheritdoc />
        public bool IsDirected => true;

        /// <inheritdoc />
        public bool AllowParallelEdges { get; }

        #endregion

        #region IVertexSet<TVertex>

        /// <inheritdoc />
        public bool IsVerticesEmpty => _vertexOutEdges.Count == 0;

        /// <inheritdoc />
        public int VertexCount => _vertexOutEdges.Count;

        /// <inheritdoc />
        public virtual IEnumerable<TVertex> Vertices => _vertexOutEdges.Keys.AsEnumerable();

        /// <inheritdoc />
        public bool ContainsVertex(TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            return _vertexOutEdges.ContainsKey(vertex);
        }

        #endregion

        #region IEdgeSet<TVertex,TEdge>

        /// <inheritdoc />
        public bool IsEdgesEmpty => EdgeCount == 0;

        /// <inheritdoc />
        public int EdgeCount { get; private set; }

        /// <inheritdoc />
        public virtual IEnumerable<TEdge> Edges => _vertexOutEdges.Values.SelectMany(edges => edges);

        /// <inheritdoc />
        public bool ContainsEdge(TEdge edge)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            return _vertexOutEdges.TryGetValue(edge.Source, out IEdgeList<TVertex, TEdge> outEdges)
                   && outEdges.Contains(edge);
        }

        /// <inheritdoc />
        public bool ContainsEdge(TVertex source, TVertex target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            if (TryGetOutEdges(source, out IEnumerable<TEdge> outEdges))
                return outEdges.Any(edge => EqualityComparer<TVertex>.Default.Equals(edge.Target, target));
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

            if (_vertexOutEdges.TryGetValue(vertex, out IEdgeList<TVertex, TEdge> outEdges))
                return outEdges.Count;
            throw new VertexNotFoundException();
        }

        [NotNull]
        private IVertexEdgeDictionary<TVertex, TEdge> _vertexOutEdges;

        /// <inheritdoc />
        public IEnumerable<TEdge> OutEdges(TVertex vertex)
        {
            if (_vertexOutEdges.TryGetValue(vertex, out IEdgeList<TVertex, TEdge> outEdges))
                return outEdges.AsEnumerable();
            throw new VertexNotFoundException();
        }

        /// <inheritdoc />
        public bool TryGetOutEdges(TVertex vertex, out IEnumerable<TEdge> edges)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            if (_vertexOutEdges.TryGetValue(vertex, out IEdgeList<TVertex, TEdge> outEdges))
            {
                edges = outEdges.AsEnumerable();
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

            if (_vertexOutEdges.TryGetValue(vertex, out IEdgeList<TVertex, TEdge> outEdges))
                return outEdges[index];
            throw new VertexNotFoundException();
        }

        #endregion

        #region IIncidenceGraph<TVertex,TEdge>

        /// <inheritdoc />
        public bool TryGetEdge(TVertex source, TVertex target, out TEdge edge)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            if (_vertexOutEdges.TryGetValue(source, out IEdgeList<TVertex, TEdge> outEdges)
                && outEdges.Count > 0)
            {
                foreach (TEdge outEdge in outEdges.Where(outEdge => EqualityComparer<TVertex>.Default.Equals(outEdge.Target, target)))
                {
                    edge = outEdge;
                    return true;
                }
            }

            edge = default(TEdge);
            return false;
        }

        /// <inheritdoc />
        public bool TryGetEdges(TVertex source, TVertex target, out IEnumerable<TEdge> edges)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            if (_vertexOutEdges.TryGetValue(source, out IEdgeList<TVertex, TEdge> outEdges))
            {
                edges = outEdges.Where(edge => EqualityComparer<TVertex>.Default.Equals(edge.Target, target));
                return true;
            }

            edges = null;
            return false;
        }

        #endregion

        #region IBidirectionalIncidenceGraph<TVertex,TEdge>

        /// <inheritdoc />
        public bool IsInEdgesEmpty(TVertex vertex)
        {
            return InDegree(vertex) == 0;
        }

        /// <inheritdoc />
        public int InDegree(TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            if (_vertexInEdges.TryGetValue(vertex, out IEdgeList<TVertex, TEdge> inEdges))
                return inEdges.Count;
            throw new VertexNotFoundException();
        }

        [NotNull]
        private IVertexEdgeDictionary<TVertex, TEdge> _vertexInEdges;

        /// <inheritdoc />
        public IEnumerable<TEdge> InEdges(TVertex vertex)
        {
            if (_vertexInEdges.TryGetValue(vertex, out IEdgeList<TVertex, TEdge> inEdges))
                return inEdges.AsEnumerable();
            throw new VertexNotFoundException();
        }

        /// <inheritdoc />
        public bool TryGetInEdges(TVertex vertex, out IEnumerable<TEdge> edges)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            if (_vertexInEdges.TryGetValue(vertex, out IEdgeList<TVertex, TEdge> inEdges))
            {
                edges = inEdges.AsEnumerable();
                return true;
            }

            edges = null;
            return false;
        }

        /// <inheritdoc />
        public TEdge InEdge(TVertex vertex, int index)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            if (_vertexInEdges.TryGetValue(vertex, out IEdgeList<TVertex, TEdge> inEdges))
                return inEdges[index];
            throw new VertexNotFoundException();
        }

        /// <inheritdoc />
        public int Degree(TVertex vertex)
        {
            return OutDegree(vertex) + InDegree(vertex);
        }

        #endregion

        #region IMutableGraph<TVertex,TEdge>

        /// <inheritdoc />
        public void Clear()
        {
            IVertexEdgeDictionary<TVertex, TEdge> vertexOutEdges = _vertexOutEdges;
            _vertexOutEdges = new VertexEdgeDictionary<TVertex, TEdge>();
            IVertexEdgeDictionary<TVertex, TEdge> vertexInEdges = _vertexInEdges;
            _vertexInEdges = new VertexEdgeDictionary<TVertex, TEdge>();
            EdgeCount = 0;

            NotifyEdgesRemoved(vertexOutEdges.SelectMany(edges => edges.Value).Distinct());
            NotifyVerticesRemoved(vertexOutEdges.Keys);
            vertexOutEdges.Clear();
            vertexInEdges.Clear();
        }

        #endregion

        #region IMutableVertexSet<TVertex>

        /// <inheritdoc />
        public virtual bool AddVertex(TVertex vertex)
        {
            if (ContainsVertex(vertex))
                return false;

            if (EdgeCapacity > 0)
            {
                _vertexOutEdges.Add(vertex, new EdgeList<TVertex, TEdge>(EdgeCapacity));
                _vertexInEdges.Add(vertex, new EdgeList<TVertex, TEdge>(EdgeCapacity));
            }
            else
            {
                _vertexOutEdges.Add(vertex, new EdgeList<TVertex, TEdge>());
                _vertexInEdges.Add(vertex, new EdgeList<TVertex, TEdge>());
            }

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

            return verticesArray.Count(AddVertex);
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

#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private void NotifyVerticesRemoved([NotNull, ItemNotNull] ICollection<TVertex> vertices)
        {
            if (VertexRemoved != null) // Lazily notify
            {
                foreach (TVertex vertex in vertices)
                {
                    OnVertexRemoved(vertex);
                }
            }
        }

        [NotNull, ItemNotNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private IEnumerable<TEdge> RemoveInOutEdges([NotNull] TVertex vertex)
        {
            IEdgeList<TVertex, TEdge> outEdges = _vertexOutEdges[vertex];
            _vertexOutEdges.Remove(vertex);
            foreach (TEdge outEdge in outEdges)
            {
                _vertexInEdges[outEdge.Target].Remove(outEdge);
            }

            IEdgeList<TVertex, TEdge> inEdges = _vertexInEdges[vertex];
            _vertexInEdges.Remove(vertex);
            foreach (TEdge inEdge in inEdges)
            {
                _vertexOutEdges[inEdge.Source].Remove(inEdge);
            }

            EdgeCount -= outEdges.Count + inEdges.Count;
            Debug.Assert(EdgeCount >= 0);

            return outEdges.Concat(inEdges);
        }

        /// <inheritdoc />
        public virtual bool RemoveVertex(TVertex vertex)
        {
            if (!ContainsVertex(vertex))
                return false;

            IEnumerable<TEdge> inOutEdges = RemoveInOutEdges(vertex);

            NotifyEdgesRemoved(inOutEdges);
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

            var verticesToRemove = new VertexList<TVertex>();
            verticesToRemove.AddRange(Vertices.Where(vertex => predicate(vertex)));

            IEnumerable<TEdge> edgesRemoved = verticesToRemove.Aggregate(
                Enumerable.Empty<TEdge>(),
                (current, vertex) => current.Concat(RemoveInOutEdges(vertex)));

            NotifyEdgesRemoved(edgesRemoved);
            NotifyVerticesRemoved(verticesToRemove);

            return verticesToRemove.Count;
        }

        #endregion

        #region IMutableEdgeListGraph<TVertex,TEdge>

#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private bool AddEdgeInternal([NotNull] TEdge edge)
        {
            Debug.Assert(edge != null);

            if (!AllowParallelEdges && ContainsEdge(edge.Source, edge.Target))
                return false;

            _vertexOutEdges[edge.Source].Add(edge);
            _vertexInEdges[edge.Target].Add(edge);
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

            return edgesArray.Count(AddEdge);
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

#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private void NotifyEdgesRemoved([NotNull, ItemNotNull] IEnumerable<TEdge> edges)
        {
            Debug.Assert(edges != null);

            if (EdgeRemoved != null) // Lazily notify
            {
                // Enumeration is only made on safe enumerable
                foreach (TEdge edge in edges)
                {
                    OnEdgeRemoved(edge);
                }
            }
        }

        /// <inheritdoc />
        public virtual bool RemoveEdge(TEdge edge)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            if (_vertexOutEdges.TryGetValue(edge.Source, out IEdgeList<TVertex, TEdge> outEdges)
                && outEdges.Remove(edge))
            {
                _vertexInEdges[edge.Target].Remove(edge);
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

#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private int RemoveEdgesInternal([NotNull] ICollection<TEdge> edgesToRemove)
        {
            foreach (TEdge edge in edgesToRemove)
            {
                _vertexOutEdges[edge.Source].Remove(edge);
                _vertexInEdges[edge.Target].Remove(edge);
            }

            EdgeCount -= edgesToRemove.Count;
            Debug.Assert(EdgeCount >= 0);
            NotifyEdgesRemoved(edgesToRemove);

            return edgesToRemove.Count;
        }

        /// <inheritdoc />
        public int RemoveEdgeIf(EdgePredicate<TVertex, TEdge> predicate)
        {
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            var edgesToRemove = new EdgeList<TVertex, TEdge>();
            edgesToRemove.AddRange(Edges.Where(edge => predicate(edge)));

            return RemoveEdgesInternal(edgesToRemove);
        }

        #endregion

        #region IMutableVertexAndEdgeSet<TVertex,TEdge>

        /// <inheritdoc />
        public virtual bool AddVerticesAndEdge(TEdge edge)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            AddVertex(edge.Source);
            AddVertex(edge.Target);
            return AddEdgeInternal(edge);
        }

        /// <inheritdoc />
        public int AddVerticesAndEdgeRange(IEnumerable<TEdge> edges)
        {
            if (edges is null)
                throw new ArgumentNullException(nameof(edges));
            TEdge[] edgesArray = edges.ToArray();
            if (edgesArray.Any(e => e == null))
                throw new ArgumentNullException(nameof(edges), "At least one edge is null.");

            return edgesArray.Count(AddVerticesAndEdge);
        }

        #endregion

        #region IMutableIncidenceGraph<TVertex,TEdge> 

        /// <inheritdoc />
        public int RemoveOutEdgeIf(TVertex vertex, EdgePredicate<TVertex, TEdge> predicate)
        {
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            if (_vertexOutEdges.TryGetValue(vertex, out IEdgeList<TVertex, TEdge> outEdges))
            {
                var edgesToRemove = new EdgeList<TVertex, TEdge>();
                edgesToRemove.AddRange(outEdges.Where(edge => predicate(edge)));
                return RemoveEdgesInternal(edgesToRemove);
            }

            return 0;
        }

        /// <inheritdoc />
        public void ClearOutEdges(TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            if (_vertexOutEdges.TryGetValue(vertex, out IEdgeList<TVertex, TEdge> outEdges))
            {
                _vertexOutEdges[vertex] = new EdgeList<TVertex, TEdge>();
                foreach (TEdge outEdge in outEdges)
                {
                    _vertexInEdges[outEdge.Target].Remove(outEdge);
                }

                EdgeCount -= outEdges.Count;
                Debug.Assert(EdgeCount >= 0);
                NotifyEdgesRemoved(outEdges);
                outEdges.Clear();
            }
        }

        /// <inheritdoc />
        public void TrimEdgeExcess()
        {
            foreach (IEdgeList<TVertex, TEdge> inEdges in _vertexInEdges.Values)
            {
                inEdges.TrimExcess();
            }

            foreach (IEdgeList<TVertex, TEdge> outEdges in _vertexOutEdges.Values)
            {
                outEdges.TrimExcess();
            }
        }

        #endregion

        #region IMutableBidirectionalGraph<TVertex,TEdge>

        /// <inheritdoc />
        public int RemoveInEdgeIf(TVertex vertex, EdgePredicate<TVertex, TEdge> predicate)
        {
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            if (_vertexInEdges.TryGetValue(vertex, out IEdgeList<TVertex, TEdge> inEdges))
            {
                var edgesToRemove = new EdgeList<TVertex, TEdge>();
                edgesToRemove.AddRange(inEdges.Where(edge => predicate(edge)));
                return RemoveEdgesInternal(edgesToRemove);
            }

            return 0;
        }

        /// <inheritdoc />
        public void ClearInEdges(TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            if (_vertexInEdges.TryGetValue(vertex, out IEdgeList<TVertex, TEdge> inEdges))
            {
                _vertexInEdges[vertex] = new EdgeList<TVertex, TEdge>();
                foreach (TEdge inEdge in inEdges)
                {
                    _vertexOutEdges[inEdge.Source].Remove(inEdge);
                }

                EdgeCount -= inEdges.Count;
                Debug.Assert(EdgeCount >= 0);
                NotifyEdgesRemoved(inEdges);
                inEdges.Clear();
            }
        }

        /// <inheritdoc />
        public void ClearEdges(TVertex vertex)
        {
            ClearOutEdges(vertex);
            ClearInEdges(vertex);
        }

        #endregion

        /// <summary>
        /// Removes the given <paramref name="vertex"/> and merges all its connection to other vertices.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="edgeFactory">Factory method to create an edge.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="vertex"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="edgeFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="VertexNotFoundException"><paramref name="vertex"/> is not part of the graph.</exception>
        public void MergeVertex(
            [NotNull] TVertex vertex,
            [NotNull, InstantHandle] EdgeFactory<TVertex, TEdge> edgeFactory)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));
            if (edgeFactory is null)
                throw new ArgumentNullException(nameof(edgeFactory));

            // Storing edges (not a copy)
            // Remove vertex will delete some of these edges
            // but it will remain needed edges to perform the merge
            if (!_vertexInEdges.TryGetValue(vertex, out IEdgeList<TVertex, TEdge> inEdges))
                throw new VertexNotFoundException();
            IEdgeList<TVertex, TEdge> outEdges = _vertexOutEdges[vertex];

            // Remove vertex
            RemoveVertex(vertex);

            // Add edges from each source to each target
            foreach (TVertex source in inEdges.Select(source => source.Source))
            {
                IEnumerable<TVertex> targets = outEdges
                    .Where(target => !EqualityComparer<TVertex>.Default.Equals(vertex, target.Target))
                    .Select(target => target.Target);
                foreach (TVertex target in targets)
                {
                    // We add an new edge
                    AddEdgeInternal(edgeFactory(source, target));
                }
            }
        }

        /// <summary>
        /// Removes vertices matching the given <paramref name="vertexPredicate"/> and merges all their
        /// connections to other vertices.
        /// </summary>
        /// <param name="vertexPredicate">Predicate to match vertices.</param>
        /// <param name="edgeFactory">Factory method to create an edge.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="vertexPredicate"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="edgeFactory"/> is <see langword="null"/>.</exception>
        public void MergeVerticesIf(
            [NotNull, InstantHandle] VertexPredicate<TVertex> vertexPredicate,
            [NotNull, InstantHandle] EdgeFactory<TVertex, TEdge> edgeFactory)
        {
            if (vertexPredicate is null)
                throw new ArgumentNullException(nameof(vertexPredicate));
            if (edgeFactory is null)
                throw new ArgumentNullException(nameof(edgeFactory));

            // Storing vertices to merge
            var mergeVertices = new VertexList<TVertex>(VertexCount / 4);
            mergeVertices.AddRange(Vertices.Where(vertex => vertexPredicate(vertex)));

            // Applying merge recursively
            foreach (TVertex vertex in mergeVertices)
            {
                MergeVertex(vertex, edgeFactory);
            }
        }

        #region ICloneable

        /// <summary>
        /// Copy constructor that creates sufficiently deep copy of the graph.
        /// </summary>
        /// <param name="other">Graph to copy.</param>
        public BidirectionalGraph([NotNull] BidirectionalGraph<TVertex, TEdge> other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));

            _vertexInEdges = other._vertexInEdges.Clone();
            _vertexOutEdges = other._vertexOutEdges.Clone();
            EdgeCount = other.EdgeCount;
            EdgeCapacity = other.EdgeCapacity;
            AllowParallelEdges = other.AllowParallelEdges;
        }

        /// <summary>
        /// Clones this graph.
        /// </summary>
        /// <returns>Cloned graph.</returns>
        [Pure]
        [NotNull]
        public BidirectionalGraph<TVertex, TEdge> Clone()
        {
            return new BidirectionalGraph<TVertex, TEdge>(this);
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