using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using QuikGraph.Collections;

namespace QuikGraph
{
    /// <summary>
    /// Implementation for a mutable directed graph data structure efficient for sparse
    /// graph representation where out-edge and in-edges need to be enumerated. Requires
    /// twice as much memory as the adjacency graph.
    /// </summary>
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
        /// <param name="vertexCapacity">Vertex capacity.</param>
        public BidirectionalGraph(bool allowParallelEdges, int vertexCapacity)
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
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BidirectionalGraph{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="allowParallelEdges">Indicates if parallel edges are allowed.</param>
        /// <param name="capacity">Vertex capacity.</param>
        /// <param name="vertexEdgesDictionaryFactory">Factory method to create vertices and their edges.</param>
        public BidirectionalGraph(
            bool allowParallelEdges,
            int capacity,
            [NotNull, InstantHandle] Func<int, IVertexEdgeDictionary<TVertex, TEdge>> vertexEdgesDictionaryFactory)
        {
            if (vertexEdgesDictionaryFactory is null)
                throw new ArgumentNullException(nameof(vertexEdgesDictionaryFactory));

            AllowParallelEdges = allowParallelEdges;
            _vertexInEdges = vertexEdgesDictionaryFactory(capacity);
            _vertexOutEdges = vertexEdgesDictionaryFactory(capacity);
        }

        /// <summary>
        /// Gives the type of edges.
        /// </summary>
        public static Type EdgeType => typeof(TEdge);

        /// <summary>
        /// Gets or sets the edge capacity.
        /// </summary>
        public int EdgeCapacity { get; set; }

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
        public virtual IEnumerable<TVertex> Vertices => _vertexOutEdges.Keys;

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

            return _vertexOutEdges[vertex].Count;
        }

        [NotNull]
        private readonly IVertexEdgeDictionary<TVertex, TEdge> _vertexOutEdges;

        /// <inheritdoc />
        public IEnumerable<TEdge> OutEdges(TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            return _vertexOutEdges[vertex];
        }

        /// <inheritdoc />
        public bool TryGetOutEdges(TVertex vertex, out IEnumerable<TEdge> edges)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            if (_vertexOutEdges.TryGetValue(vertex, out IEdgeList<TVertex, TEdge> outEdges))
            {
                edges = outEdges;
                return outEdges.Count > 0;
            }

            edges = null;
            return false;
        }

        /// <inheritdoc />
        public TEdge OutEdge(TVertex vertex, int index)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            return _vertexOutEdges[vertex][index];
        }

        #endregion

        #region IIncidenceGraph<TVertex,TEdge>

        /// <inheritdoc />
        public bool ContainsEdge(TVertex source, TVertex target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            if (TryGetOutEdges(source, out IEnumerable<TEdge> outEdges))
                return outEdges.Any(edge => edge.Target.Equals(target));
            return false;
        }

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
                foreach (TEdge e in outEdges)
                {
                    if (e.Target.Equals(target))
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
        public bool TryGetEdges(TVertex source, TVertex target, out IEnumerable<TEdge> edges)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            if (_vertexOutEdges.TryGetValue(source, out IEdgeList<TVertex, TEdge> outEdges))
            {
                edges = outEdges.Where(edge => edge.Target.Equals(target));
                return edges.Any();
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

            return _vertexInEdges[vertex].Count;
        }

        [NotNull]
        private readonly IVertexEdgeDictionary<TVertex, TEdge> _vertexInEdges;

        /// <inheritdoc />
        public IEnumerable<TEdge> InEdges(TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            return _vertexInEdges[vertex];
        }

        /// <inheritdoc />
        public bool TryGetInEdges(TVertex vertex, out IEnumerable<TEdge> edges)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            if (_vertexInEdges.TryGetValue(vertex, out IEdgeList<TVertex, TEdge> inEdges))
            {
                edges = inEdges;
                return inEdges.Count > 0;
            }

            edges = null;
            return false;
        }

        /// <inheritdoc />
        public TEdge InEdge(TVertex vertex, int index)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            return _vertexInEdges[vertex][index];
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
            _vertexOutEdges.Clear();
            _vertexInEdges.Clear();
            EdgeCount = 0;
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

            int count = 0;
            foreach (TVertex vertex in vertices)
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
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            VertexAdded?.Invoke(vertex);
        }

        /// <inheritdoc />
        public virtual bool RemoveVertex(TVertex vertex)
        {
            if (!ContainsVertex(vertex))
                return false;

            // Collect edges to remove
            var edgesToRemove = new EdgeList<TVertex, TEdge>();
            foreach (TEdge outEdge in OutEdges(vertex))
            {
                _vertexInEdges[outEdge.Target].Remove(outEdge);
                edgesToRemove.Add(outEdge);
            }

            foreach (TEdge inEdge in InEdges(vertex))
            {
                // Might already have been removed
                if (_vertexOutEdges[inEdge.Source].Remove(inEdge))
                    edgesToRemove.Add(inEdge);
            }

            // Notify users
            if (EdgeRemoved != null)
            {
                foreach (TEdge edge in edgesToRemove)
                    OnEdgeRemoved(edge);
            }

            _vertexOutEdges.Remove(vertex);
            _vertexInEdges.Remove(vertex);
            EdgeCount -= edgesToRemove.Count;
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
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            VertexRemoved?.Invoke(vertex);
        }

        /// <inheritdoc />
        public int RemoveVertexIf(VertexPredicate<TVertex> predicate)
        {
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            var verticesToRemove = new VertexList<TVertex>();
            verticesToRemove.AddRange(Vertices.Where(vertex => predicate(vertex)));

            foreach (TVertex vertex in verticesToRemove)
                RemoveVertex(vertex);

            return verticesToRemove.Count;
        }

        #endregion

        #region IMutableEdgeListGraph<TVertex,TEdge>

        /// <inheritdoc />
        public virtual bool AddEdge(TEdge edge)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            if (!AllowParallelEdges && ContainsEdge(edge.Source, edge.Target))
                return false;

            _vertexOutEdges[edge.Source].Add(edge);
            _vertexInEdges[edge.Target].Add(edge);
            ++EdgeCount;

            OnEdgeAdded(edge);

            return true;
        }

        /// <inheritdoc />
        public int AddEdgeRange(IEnumerable<TEdge> edges)
        {
            if (edges is null)
                throw new ArgumentNullException(nameof(edges));

            int count = 0;
            foreach (TEdge edge in edges)
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
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            EdgeAdded?.Invoke(edge);
        }

        /// <inheritdoc />
        public virtual bool RemoveEdge(TEdge edge)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            if (_vertexOutEdges[edge.Source].Remove(edge))
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
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

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
                RemoveEdge(edge);

            return edgesToRemove.Count;
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
            return AddEdge(edge);
        }

        /// <inheritdoc />
        public int AddVerticesAndEdgeRange(IEnumerable<TEdge> edges)
        {
            if (edges is null)
                throw new ArgumentNullException(nameof(edges));

            int count = 0;
            foreach (TEdge edge in edges)
            {
                if (AddVerticesAndEdge(edge))
                    ++count;
            }

            return count;
        }

        #endregion

        #region IMutableIncidenceGraph<TVertex,TEdge> 

        /// <inheritdoc />
        public int RemoveOutEdgeIf(TVertex vertex, EdgePredicate<TVertex, TEdge> predicate)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            var edgesToRemove = new EdgeList<TVertex, TEdge>();
            edgesToRemove.AddRange(OutEdges(vertex).Where(edge => predicate(edge)));

            foreach (TEdge edge in edgesToRemove)
                RemoveEdge(edge);

            return edgesToRemove.Count;
        }

        /// <inheritdoc />
        public void ClearOutEdges(TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            IEdgeList<TVertex, TEdge> outEdges = _vertexOutEdges[vertex];
            foreach (TEdge edge in outEdges)
            {
                _vertexInEdges[edge.Target].Remove(edge);
                OnEdgeRemoved(edge);
            }

            EdgeCount -= outEdges.Count;
            outEdges.Clear();
        }

        /// <inheritdoc />
        public void TrimEdgeExcess()
        {
            foreach (IEdgeList<TVertex, TEdge> edges in _vertexInEdges.Values)
                edges.TrimExcess();
            foreach (IEdgeList<TVertex, TEdge> edges in _vertexOutEdges.Values)
                edges.TrimExcess();
        }

        #endregion

        #region IMutableBidirectionalGraph<TVertex,TEdge>

        /// <inheritdoc />
        public int RemoveInEdgeIf(TVertex vertex, EdgePredicate<TVertex, TEdge> predicate)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            var edgesToRemove = new EdgeList<TVertex, TEdge>();
            edgesToRemove.AddRange(InEdges(vertex).Where(edge => predicate(edge)));

            foreach (TEdge edge in edgesToRemove)
                RemoveEdge(edge);

            return edgesToRemove.Count;
        }

        /// <inheritdoc />
        public void ClearInEdges(TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            IEdgeList<TVertex, TEdge> inEdges = _vertexInEdges[vertex];
            foreach (TEdge edge in inEdges)
            {
                _vertexOutEdges[edge.Source].Remove(edge);
                OnEdgeRemoved(edge);
            }

            EdgeCount -= inEdges.Count;
            inEdges.Clear();
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
        public void MergeVertex(
            [NotNull] TVertex vertex, 
            [NotNull, InstantHandle] EdgeFactory<TVertex, TEdge> edgeFactory)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));
            if (edgeFactory is null)
                throw new ArgumentNullException(nameof(edgeFactory));

            // Storing edges in local array
            IEdgeList<TVertex, TEdge> inEdges = _vertexInEdges[vertex];
            IEdgeList<TVertex, TEdge> outEdges = _vertexOutEdges[vertex];

            // Remove vertex
            RemoveVertex(vertex);

            // Add edges from each source to each target
            foreach (TEdge source in inEdges)
            {
                // Is it a self edge?
                if (source.Source.Equals(vertex))
                    continue;

                foreach (TEdge target in outEdges)
                {
                    if (vertex.Equals(target.Target))
                        continue;

                    // We add an new edge
                    AddEdge(edgeFactory(source.Source, target.Target));
                }
            }
        }

        /// <summary>
        /// Removes vertices matching the given <paramref name="vertexPredicate"/> and merges all their
        /// connections to other vertices.
        /// </summary>
        /// <param name="vertexPredicate">Predicate to match vertices.</param>
        /// <param name="edgeFactory">Factory method to create an edge.</param>
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
                MergeVertex(vertex, edgeFactory);
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
