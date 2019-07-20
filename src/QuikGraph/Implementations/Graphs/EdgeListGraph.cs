#if SUPPORTS_SERIALIZATION || SUPPORTS_CLONEABLE
using System;
#endif
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using QuikGraph.Collections;

namespace QuikGraph
{
    /// <summary>
    /// Implementation for a edge list graph.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    [DebuggerDisplay("EdgeCount = {" + nameof(EdgeCount) + "}")]
    public class EdgeListGraph<TVertex, TEdge> : IMutableEdgeListGraph<TVertex, TEdge>
#if SUPPORTS_CLONEABLE
        , ICloneable
#endif
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EdgeListGraph{TVertex,TEdge}"/> class.
        /// </summary>
        public EdgeListGraph()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EdgeListGraph{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="isDirected">Indicates if the graph is directed.</param>
        /// <param name="allowParallelEdges">Indicates if parallel edges are allowed.</param>
        public EdgeListGraph(bool isDirected, bool allowParallelEdges)
        {
            IsDirected = isDirected;
            AllowParallelEdges = allowParallelEdges;
        }

        #region IGraph<TVertex,TEdge>

        /// <inheritdoc />
        public bool IsDirected { get; } = true;

        /// <inheritdoc />
        public bool AllowParallelEdges { get; } = true;

        #endregion

        #region IVertexSet<TVertex>

        /// <inheritdoc />
        public bool IsVerticesEmpty => _edges.Count == 0;

        /// <inheritdoc />
        public int VertexCount => GetVertexCounts().Count;

        /// <inheritdoc />
        public IEnumerable<TVertex> Vertices => GetVertexCounts().Keys;

        private Dictionary<TVertex, int> GetVertexCounts()
        {
            var vertices = new Dictionary<TVertex, int>(EdgeCount * 2);
            foreach (TEdge edge in Edges)
            {
                ++vertices[edge.Source];
                ++vertices[edge.Target];
            }

            return vertices;
        }

        /// <inheritdoc />
        public bool ContainsVertex(TVertex vertex)
        {
            return Edges.Any(
                edge => edge.Source.Equals(vertex) || edge.Target.Equals(vertex));
        }

        #endregion

        #region IEdgeSet<TVertex,TEdge>

        [NotNull]
        private readonly EdgeEdgeDictionary<TVertex, TEdge> _edges = new EdgeEdgeDictionary<TVertex, TEdge>();

        /// <inheritdoc />
        public bool IsEdgesEmpty => _edges.Count == 0;

        /// <inheritdoc />
        public int EdgeCount => _edges.Count;

        /// <inheritdoc />
        public IEnumerable<TEdge> Edges => _edges.Keys;

        /// <inheritdoc />
        public bool ContainsEdge(TEdge edge)
        {
            return _edges.ContainsKey(edge);
        }

        #endregion

        #region IMutableEdgeListGraph<TVertex,TEdge>

        /// <summary>
        /// Adds <paramref name="edge"/> and its vertices to this graph.
        /// </summary>
        /// <param name="edge">The edge to add.</param>
        /// <returns>True if the edge was added, false otherwise.</returns>
        public bool AddVerticesAndEdge([NotNull] TEdge edge)
        {
            return AddEdge(edge);
        }

        /// <summary>
        /// Adds a set of edges (and it's vertices if necessary).
        /// </summary>
        /// <param name="edges">Edges to add.</param>
        /// <returns>The number of edges added.</returns>
        public int AddVerticesAndEdgeRange([NotNull, ItemNotNull] IEnumerable<TEdge> edges)
        {
            int count = 0;
            foreach (TEdge edge in edges)
            {
                if (AddVerticesAndEdge(edge))
                    count++;
            }

            return count;
        }

        /// <inheritdoc />
        public bool AddEdge(TEdge edge)
        {
            if (ContainsEdge(edge))
                return false;

            _edges.Add(edge, edge);
            OnEdgeAdded(edge);

            return true;
        }

        /// <inheritdoc />
        public int AddEdgeRange(IEnumerable<TEdge> edges)
        {
            int count = 0;
            foreach (var edge in edges)
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
#if SUPPORTS_CONTRACTS
            Contract.Requires(edge != null);
#endif

            EdgeAdded?.Invoke(edge);
        }

        /// <inheritdoc />
        public bool RemoveEdge(TEdge edge)
        {
            if (_edges.Remove(edge))
            {
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
#if SUPPORTS_CONTRACTS
            Contract.Requires(edge != null);
#endif

            EdgeRemoved?.Invoke(edge);
        }

        /// <inheritdoc />
        public int RemoveEdgeIf(EdgePredicate<TVertex, TEdge> predicate)
        {
            var edgesToRemove = Edges.Where(edge => predicate(edge)).ToArray();

            foreach (TEdge edge in edgesToRemove)
                _edges.Remove(edge);
            return edgesToRemove.Length;
        }

        #endregion

        /// <inheritdoc />
        public void Clear()
        {
            var edges = _edges.Clone();
            _edges.Clear();

            foreach (var edge in edges.Keys)
                OnEdgeRemoved(edge);
        }

        #region ICloneable

        private EdgeListGraph(
            bool isDirected,
            bool allowParallelEdges,
            [NotNull] EdgeEdgeDictionary<TVertex, TEdge> edges)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(edges != null);
#endif

            IsDirected = isDirected;
            AllowParallelEdges = allowParallelEdges;
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
        [NotNull]
        public EdgeListGraph<TVertex, TEdge> Clone()
        {
            return new EdgeListGraph<TVertex, TEdge>(
                IsDirected,
                AllowParallelEdges,
                _edges.Clone());
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
