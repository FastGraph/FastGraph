using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;

#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif

namespace QuikGraph
{
    /// <summary>
    /// Implementation for a bidirectional graph data structure based on a matrix.
    /// </summary>
    /// <typeparam name="TEdge">Edge type</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    [DebuggerDisplay("VertexCount = {" + nameof(VertexCount) + "}, EdgeCount = {" + nameof(EdgeCount) + "}")]
    public class BidirectionalMatrixGraph<TEdge> : IBidirectionalGraph<int, TEdge>, IMutableEdgeListGraph<int, TEdge>
#if SUPPORTS_CLONEABLE
        , ICloneable
#endif
        where TEdge : IEdge<int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BidirectionalMatrixGraph{TEdge}"/> class.
        /// </summary>
        /// <param name="vertexCount">Number of vertices.</param>
        public BidirectionalMatrixGraph(int vertexCount)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(vertexCount > 0);
#endif

            VertexCount = vertexCount;
            EdgeCount = 0;
            _edges = new TEdge[vertexCount, vertexCount];
        }

        #region IGraph<int,TEdge>


        /// <inheritdoc />
        public bool IsDirected => true;

        /// <inheritdoc />
        public bool AllowParallelEdges => false;

        #endregion

        #region IVertexSet<int>

        /// <inheritdoc />
        public bool IsVerticesEmpty => VertexCount == 0;

        /// <inheritdoc />
        public int VertexCount { get; }

        /// <inheritdoc />
        public IEnumerable<int> Vertices => Enumerable.Range(0, VertexCount);

        /// <inheritdoc />
        public bool ContainsVertex(int vertex)
        {
            return vertex >= 0 && vertex < VertexCount;
        }

        #endregion

        #region IEdgeSet<int,TEdge>

        /// <inheritdoc />
        public bool IsEdgesEmpty => EdgeCount == 0;

        /// <inheritdoc />
        public int EdgeCount { get; private set; }

        [NotNull]
        private readonly TEdge[,] _edges;

        /// <inheritdoc />
        public IEnumerable<TEdge> Edges
        {
            get
            {
                for (int i = 0; i < VertexCount; ++i)
                {
                    for (int j = 0; j < VertexCount; ++j)
                    {
                        TEdge edge = _edges[i, j];
                        if (edge != null)
                            yield return edge;
                    }
                }
            }
        }

        /// <inheritdoc />
        public bool ContainsEdge(TEdge edge)
        {
            TEdge e = _edges[edge.Source, edge.Target];
            return e != null && e.Equals(edge);
        }

        #endregion

        #region IIncidenceGraph<int,TEdge>

        /// <inheritdoc />
        public bool ContainsEdge(int source, int target)
        {
            return _edges[source, target] != null;
        }

        /// <inheritdoc />
        public bool TryGetEdge(int source, int target, out TEdge edge)
        {
            edge = _edges[source, target];
            return edge != null;
        }

        /// <inheritdoc />
        public bool TryGetEdges(int source, int target, out IEnumerable<TEdge> edges)
        {
            if (TryGetEdge(source, target, out TEdge edge))
            {
                edges = new[] { edge };
                return true;
            }

            edges = null;
            return false;
        }

        #endregion

        #region IImplicitGraph<int,TEdge>

        /// <inheritdoc />
        public bool IsOutEdgesEmpty(int vertex)
        {
            for (int j = 0; j < VertexCount; ++j)
            {
                if (_edges[vertex, j] != null)
                    return false;
            }

            return true;
        }

        /// <inheritdoc />
        public int OutDegree(int vertex)
        {
            int count = 0;
            for (int j = 0; j < VertexCount; ++j)
            {
                if (_edges[vertex, j] != null)
                    count++;
            }

            return count;
        }

        /// <inheritdoc />
        public IEnumerable<TEdge> OutEdges(int vertex)
        {
            for (int j = 0; j < VertexCount; ++j)
            {
                TEdge edge = _edges[vertex, j];
                if (edge != null)
                    yield return edge;
            }
        }

        /// <inheritdoc />
        public bool TryGetOutEdges(int vertex, out IEnumerable<TEdge> edges)
        {
            if (vertex > -1 && vertex < VertexCount)
            {
                TEdge[] outEdges = OutEdges(vertex).ToArray();
                edges = outEdges;
                return outEdges.Length > 0;
            }

            edges = null;
            return false;
        }

        /// <inheritdoc />
        public TEdge OutEdge(int vertex, int index)
        {
            int count = 0;
            for (int j = 0; j < VertexCount; ++j)
            {
                TEdge edge = _edges[vertex, j];
                if (edge != null)
                {
                    if (count == index)
                        return edge;
                    count++;
                }
            }

            throw new ArgumentOutOfRangeException(nameof(index));
        }

        #endregion

        #region IBidirectionalGraph<int,TEdge>

        /// <inheritdoc />
        public bool IsInEdgesEmpty(int vertex)
        {
            for (int i = 0; i < VertexCount; ++i)
            {
                if (_edges[i, vertex] != null)
                    return false;
            }

            return true;
        }

        /// <inheritdoc />
        public int InDegree(int vertex)
        {
            int count = 0;
            for (int i = 0; i < VertexCount; ++i)
            {
                if (_edges[i, vertex] != null)
                    count++;
            }

            return count;
        }

        /// <inheritdoc />
        public IEnumerable<TEdge> InEdges(int vertex)
        {
            for (int i = 0; i < VertexCount; ++i)
            {
                TEdge edge = _edges[i, vertex];
                if (edge != null)
                    yield return edge;
            }
        }

        /// <inheritdoc />
        public bool TryGetInEdges(int vertex, out IEnumerable<TEdge> edges)
        {
#if SUPPORTS_CONTRACTS
            Contract.Ensures(
                Contract.Result<bool>() == (0 <= vertex && vertex > VertexCount));
            Contract.Ensures(
                Contract.Result<bool>() == (Contract.ValueAtReturn(out edges) != null));
#endif

            if (vertex > -1 && vertex < VertexCount)
            {
                TEdge[] inEdges = InEdges(vertex).ToArray();
                edges = inEdges;
                return inEdges.Length > 0;
            }

            edges = null;
            return false;
        }

        /// <inheritdoc />
        public TEdge InEdge(int vertex, int index)
        {
            int count = 0;
            for (int i = 0; i < VertexCount; ++i)
            {
                TEdge edge = _edges[i, vertex];
                if (edge != null)
                {
                    if (count == index)
                        return edge;
                    count++;
                }
            }

            throw new ArgumentOutOfRangeException(nameof(index));
        }

        /// <inheritdoc />
        public int Degree(int vertex)
        {
            return InDegree(vertex) + OutDegree(vertex);
        }

        #endregion

        #region IMutableGraph<int,TEdge>

        /// <inheritdoc />
        public void Clear()
        {
            for (int i = 0; i < VertexCount; ++i)
                for (int j = 0; j < VertexCount; ++j)
                    _edges[i, j] = default(TEdge);
            EdgeCount = 0;
        }

        #endregion

        #region IMutableBidirectionalGraph<int,TEdge>

        /// <summary>
        /// Removes in-edges of the given <paramref name="vertex"/> that match
        /// predicate <paramref name="edgePredicate"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="edgePredicate">Edge predicate.</param>
        /// <returns>Number of edges removed.</returns>
        public int RemoveInEdgeIf(int vertex, [NotNull, InstantHandle] EdgePredicate<int, TEdge> edgePredicate)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(0 <= vertex && vertex < VertexCount);
#endif

            int count = 0;
            for (int i = 0; i < VertexCount; ++i)
            {
                TEdge edge = _edges[i, vertex];
                if (edge != null && edgePredicate(edge))
                {
                    RemoveEdge(edge);
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Clears in-edges of the given <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        public void ClearInEdges(int vertex)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(0 <= vertex && vertex < VertexCount);
#endif

            for (int i = 0; i < VertexCount; ++i)
            {
                TEdge edge = _edges[i, vertex];
                if (edge != null)
                    RemoveEdge(edge);
            }
        }

        /// <summary>
        /// Clears in-edges and out-edges of the given <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        public void ClearEdges(int vertex)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(0 <= vertex && vertex < VertexCount);
#endif

            ClearInEdges(vertex);
            ClearOutEdges(vertex);
        }

        #endregion

        #region IMutableIncidenceGraph<int,TEdge>

        /// <summary>
        /// Removes all out-edges of the <paramref name="vertex"/>
        /// where the <paramref name="predicate"/> is evaluated to true.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="predicate">Predicate to remove edges.</param>
        /// <returns>The number of removed edges.</returns>
        public int RemoveOutEdgeIf(int vertex, [NotNull, InstantHandle] EdgePredicate<int, TEdge> predicate)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(0 <= vertex && vertex < VertexCount);
#endif

            int count = 0;
            for (int j = 0; j < VertexCount; ++j)
            {
                TEdge edge = _edges[vertex, j];
                if (edge != null && predicate(edge))
                {
                    RemoveEdge(edge);
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Trims the out-edges of the given <paramref name="vertex"/>
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        public void ClearOutEdges(int vertex)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(0 <= vertex && vertex < VertexCount);
#endif

            for (int j = 0; j < VertexCount; ++j)
            {
                TEdge edge = _edges[vertex, j];
                if (edge != null)
                    RemoveEdge(edge);
            }
        }

        #endregion

        #region IMutableEdgeListGraph<int,TEdge>

        /// <inheritdoc />
        public bool AddEdge(TEdge edge)
        {
            if (_edges[edge.Source, edge.Target] != null)
                throw new ParallelEdgeNotAllowedException();

            _edges[edge.Source, edge.Target] = edge;
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
        public event EdgeAction<int, TEdge> EdgeAdded;

        /// <summary>
        /// Called on each added edge.
        /// </summary>
        /// <param name="edge">Added edge.</param>
        protected virtual void OnEdgeAdded([NotNull] TEdge edge)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(edge != null);
#endif

            EdgeAdded?.Invoke(edge);
        }

        /// <inheritdoc />
        public bool RemoveEdge(TEdge edge)
        {
            TEdge e = _edges[edge.Source, edge.Target];
            _edges[edge.Source, edge.Target] = default(TEdge);
            if (!e.Equals(default(TEdge)))
            {
                EdgeCount--;
                OnEdgeRemoved(edge);
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public event EdgeAction<int, TEdge> EdgeRemoved;

        /// <summary>
        /// Called on each removed edge.
        /// </summary>
        /// <param name="edge">Removed edge.</param>
        protected virtual void OnEdgeRemoved([NotNull] TEdge edge)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(edge != null);
#endif

            EdgeRemoved?.Invoke(edge);
        }

        /// <summary>
        /// <see cref="RemoveEdgeIf"/> is not implemented for this kind of graph.
        /// </summary>
        public int RemoveEdgeIf(EdgePredicate<int, TEdge> predicate)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ICloneable

        private BidirectionalMatrixGraph(
            int vertexCount,
            int edgeCount,
            [NotNull] TEdge[,] edges)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(vertexCount > 0);
            Contract.Requires(edgeCount >= 0);
            Contract.Requires(edges != null);
            Contract.Requires(vertexCount == edges.GetLength(0));
            Contract.Requires(vertexCount == edges.GetLength(1));
#endif

            VertexCount = vertexCount;
            EdgeCount = edgeCount;
            _edges = edges;
        }

        /// <summary>
        /// Clones this graph.
        /// </summary>
        /// <returns>Cloned graph.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        public BidirectionalMatrixGraph<TEdge> Clone()
        {
            return new BidirectionalMatrixGraph<TEdge>(
                VertexCount,
                EdgeCount,
                (TEdge[,])_edges.Clone());
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
