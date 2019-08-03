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
    /// graph representation where out-edge need to be enumerated only.
    /// </summary>
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
            : this(allowParallelEdges, capacity, -1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdjacencyGraph{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="allowParallelEdges">Indicates if parallel edges are allowed.</param>
        /// <param name="capacity">Vertex capacity.</param>
        /// <param name="edgeCapacity">Edge capacity.</param>
        public AdjacencyGraph(bool allowParallelEdges, int capacity, int edgeCapacity)
        {
            AllowParallelEdges = allowParallelEdges;
            _vertexEdges = capacity > -1
                ? new VertexEdgeDictionary<TVertex, TEdge>(capacity)
                : new VertexEdgeDictionary<TVertex, TEdge>();
            EdgeCapacity = edgeCapacity;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdjacencyGraph{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="allowParallelEdges">Indicates if parallel edges are allowed.</param>
        /// <param name="capacity">Vertex capacity.</param>
        /// <param name="edgeCapacity">Edge capacity.</param>
        /// <param name="vertexEdgesDictionaryFactory">Factory method to create vertices and their edges.</param>
        public AdjacencyGraph(
            bool allowParallelEdges,
            int capacity,
            int edgeCapacity,
            [NotNull, InstantHandle] Func<int, IVertexEdgeDictionary<TVertex, TEdge>> vertexEdgesDictionaryFactory)
        {
            if (vertexEdgesDictionaryFactory is null)
                throw new ArgumentNullException(nameof(vertexEdgesDictionaryFactory));

            AllowParallelEdges = allowParallelEdges;
            _vertexEdges = vertexEdgesDictionaryFactory(capacity);
            EdgeCapacity = edgeCapacity;
        }

        /// <summary>
        /// Gets or sets the edge capacity.
        /// </summary>
        public int EdgeCapacity { get; set; }

        /// <summary>
        /// Gives the type of edges.
        /// </summary>
        public static Type EdgeType => typeof(TEdge);

        #region IGraph<TVertex,TEdge>

        /// <inheritdoc />
        public bool IsDirected => true;

        /// <inheritdoc />
        public bool AllowParallelEdges { get; }

        #endregion

        #region IVertexSet<TVertex>

        /// <inheritdoc />
        public bool IsVerticesEmpty => _vertexEdges.Count == 0;

        /// <inheritdoc />
        public int VertexCount => _vertexEdges.Count;

        [NotNull]
        private readonly IVertexEdgeDictionary<TVertex, TEdge> _vertexEdges;

        /// <inheritdoc />
        public virtual IEnumerable<TVertex> Vertices => _vertexEdges.Keys;

        /// <inheritdoc />
        public bool ContainsVertex(TVertex vertex)
        {
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
            return _vertexEdges.TryGetValue(edge.Source, out IEdgeList<TVertex, TEdge> edges)
                   && edges.Contains(edge);
        }

        #endregion

        #region IIncidenceGraph<TVertex,TEdge>

        /// <inheritdoc />
        public bool ContainsEdge(TVertex source, TVertex target)
        {
            if (TryGetOutEdges(source, out IEnumerable<TEdge> outEdges))
                return outEdges.Any(edge => edge.Target.Equals(target));
            return false;
        }

        /// <inheritdoc />
        public bool TryGetEdge(TVertex source, TVertex target, out TEdge edge)
        {
            if (_vertexEdges.TryGetValue(source, out IEdgeList<TVertex, TEdge> edgeList)
                && edgeList.Count > 0)
            {
                foreach (TEdge e in edgeList)
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
        public virtual bool TryGetEdges(TVertex source, TVertex target, out IEnumerable<TEdge> edges)
        {
            if (_vertexEdges.TryGetValue(source, out IEdgeList<TVertex, TEdge> outEdges))
            {
                edges = outEdges.Where(edge => edge.Target.Equals(target));
                return edges.Any();
            }

            edges = null;
            return false;
        }

        #endregion

        #region IImplicitGraph<TVertex,TEdge>

        /// <inheritdoc />
        public bool IsOutEdgesEmpty(TVertex vertex)
        {
            return _vertexEdges[vertex].Count == 0;
        }

        /// <inheritdoc />
        public int OutDegree(TVertex vertex)
        {
            return _vertexEdges[vertex].Count;
        }

        /// <inheritdoc />
        public virtual IEnumerable<TEdge> OutEdges(TVertex vertex)
        {
            return _vertexEdges[vertex];
        }

        /// <inheritdoc />
        public virtual bool TryGetOutEdges(TVertex vertex, out IEnumerable<TEdge> edges)
        {
            if (_vertexEdges.TryGetValue(vertex, out IEdgeList<TVertex, TEdge> edgeList))
            {
                edges = edgeList;
                return edgeList.Count > 0;
            }

            edges = null;
            return false;
        }

        /// <inheritdoc />
        public TEdge OutEdge(TVertex vertex, int index)
        {
            return _vertexEdges[vertex][index];
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
            int count = 0;
            foreach (TVertex vertex in vertices)
            {
                if (AddVertex(vertex))
                    count++;
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
            foreach (var pair in _vertexEdges)
            {
                if (pair.Key.Equals(vertex))
                    continue; // We've already

                // Collect edges to remove
                foreach (var edge in pair.Value.Clone())
                {
                    if (edge.Target.Equals(vertex))
                    {
                        pair.Value.Remove(edge);
                        OnEdgeRemoved(edge);
                        EdgeCount--;
                    }
                }
            }

#if SUPPORTS_CONTRACTS
            Contract.Assert(EdgeCount >= 0);
#endif
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
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            VertexRemoved?.Invoke(vertex);
        }

        /// <inheritdoc />
        public int RemoveVertexIf(VertexPredicate<TVertex> predicate)
        {
            var vertices = new VertexList<TVertex>();
            vertices.AddRange(Vertices.Where(vertex => predicate(vertex)));

            foreach (TVertex vertex in vertices)
                RemoveVertex(vertex);

            return vertices.Count;
        }

        /// <inheritdoc />
        public virtual bool AddVerticesAndEdge(TEdge edge)
        {
            AddVertex(edge.Source);
            AddVertex(edge.Target);
            return AddEdge(edge);
        }

        /// <summary>
        /// Adds a range of edges to the graph
        /// </summary>
        /// <param name="edges"></param>
        /// <returns>the count edges that were added</returns>
        public int AddVerticesAndEdgeRange(IEnumerable<TEdge> edges)
        {
            int count = 0;
            foreach (TEdge edge in edges)
            {
                if (AddVerticesAndEdge(edge))
                    count++;
            }

            return count;
        }

        #endregion

        #region IMutableEdgeListGraph<TVertex,TEdge>

        /// <inheritdoc />
        public virtual bool AddEdge(TEdge edge)
        {
            if (!AllowParallelEdges && ContainsEdge(edge.Source, edge.Target))
                return false;

            _vertexEdges[edge.Source].Add(edge);
            EdgeCount++;

            OnEdgeAdded(edge);

            return true;
        }

        /// <inheritdoc />
        public int AddEdgeRange(IEnumerable<TEdge> edges)
        {
            int count = 0;
            foreach (TEdge edge in edges)
            {
                if (AddEdge(edge))
                    count++;
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
            if (_vertexEdges.TryGetValue(edge.Source, out IEdgeList<TVertex, TEdge> edges)
                && edges.Remove(edge))
            {
                EdgeCount--;

#if SUPPORTS_CONTRACTS
                Contract.Assert(EdgeCount >= 0);
#endif
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
            IEdgeList<TVertex, TEdge> edges = _vertexEdges[vertex];
            var edgesToRemove = new EdgeList<TVertex, TEdge>(edges.Count);
            edgesToRemove.AddRange(edges.Where(edge => predicate(edge)));

            foreach (TEdge edge in edgesToRemove)
            {
                edges.Remove(edge);
                OnEdgeRemoved(edge);
            }

            EdgeCount -= edgesToRemove.Count;

            return edgesToRemove.Count;
        }

        /// <inheritdoc />
        public void ClearOutEdges(TVertex vertex)
        {
            var edges = _vertexEdges[vertex];
            int count = edges.Count;
            if (EdgeRemoved != null) // Lazily notify
            {
                foreach (TEdge edge in edges)
                    OnEdgeRemoved(edge);
            }

            edges.Clear();
            EdgeCount -= count;
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
#if SUPPORTS_CONTRACTS
            Contract.Requires(vertexEdges != null);
            Contract.Requires(edgeCount >= 0);
#endif

            _vertexEdges = vertexEdges;
            EdgeCount = edgeCount;
            EdgeCapacity = edgeCapacity;
            AllowParallelEdges = allowParallelEdges;
        }

        /// <summary>
        /// Clones this graph.
        /// </summary>
        /// <returns>Cloned graph.</returns>
        [JetBrains.Annotations.Pure]
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
