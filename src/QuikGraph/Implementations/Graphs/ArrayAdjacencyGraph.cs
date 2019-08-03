using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// Implementation for an immutable directed graph data structure efficient for large sparse
    /// graph representation where out-edge need to be enumerated only.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    [DebuggerDisplay("VertexCount = {" + nameof(VertexCount) + "}, EdgeCount = {" + nameof(EdgeCount) + "}")]
    public sealed class ArrayAdjacencyGraph<TVertex, TEdge> : IVertexAndEdgeListGraph<TVertex, TEdge>
#if SUPPORTS_CLONEABLE
        , ICloneable
#endif
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayAdjacencyGraph{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        public ArrayAdjacencyGraph([NotNull] IVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph)
        {
            if (visitedGraph is null)
                throw new ArgumentNullException(nameof(visitedGraph));

            _vertexOutEdges = new Dictionary<TVertex, TEdge[]>(visitedGraph.VertexCount);
            EdgeCount = visitedGraph.EdgeCount;
            foreach (TVertex vertex in visitedGraph.Vertices)
            {
                _vertexOutEdges.Add(
                    vertex,
                    visitedGraph.OutEdges(vertex).ToArray());
            }
        }

        #region IGraph<TVertex,TEdge>

        /// <inheritdoc />
        public bool IsDirected => true;

        /// <inheritdoc />
        public bool AllowParallelEdges => true;

        #endregion

        #region IVertexSet<TVertex>

        /// <inheritdoc />
        public bool IsVerticesEmpty => _vertexOutEdges.Count == 0;

        /// <inheritdoc />
        public int VertexCount => _vertexOutEdges.Count;

        [NotNull]
        private readonly Dictionary<TVertex, TEdge[]> _vertexOutEdges;

        /// <inheritdoc />
        public IEnumerable<TVertex> Vertices => _vertexOutEdges.Keys;

        /// <inheritdoc />
        public bool ContainsVertex(TVertex vertex)
        {
            return _vertexOutEdges.ContainsKey(vertex);
        }

        #endregion

        #region IEdgeSet<TVertex,TEdge>

        /// <inheritdoc />
        public bool IsEdgesEmpty => EdgeCount == 0;

        /// <inheritdoc />
        public int EdgeCount { get; }

        /// <inheritdoc />
        public IEnumerable<TEdge> Edges => _vertexOutEdges.Values.SelectMany(edges => edges);

        /// <inheritdoc />
        public bool ContainsEdge(TEdge edge)
        {
            if (_vertexOutEdges.TryGetValue(edge.Source, out TEdge[] edges))
                return edges.Any(e => e.Equals(edge));
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
            if (_vertexOutEdges.TryGetValue(vertex, out TEdge[] edges))
                return edges.Length;
            return 0;
        }

        /// <inheritdoc />
        public IEnumerable<TEdge> OutEdges(TVertex vertex)
        {
            if (_vertexOutEdges.TryGetValue(vertex, out TEdge[] edges))
                return edges;
            return Enumerable.Empty<TEdge>();
        }

        /// <inheritdoc />
        public bool TryGetOutEdges(TVertex vertex, out IEnumerable<TEdge> edges)
        {
            if (_vertexOutEdges.TryGetValue(vertex, out TEdge[] outEdges))
            {
                edges = outEdges;
                return outEdges.Length > 0;
            }

            edges = null;
            return false;
        }

        /// <inheritdoc />
        public TEdge OutEdge(TVertex vertex, int index)
        {
            return _vertexOutEdges[vertex][index];
        }

        #endregion

        #region IIncidenceGraph<TVertex,TEdge>

        /// <inheritdoc />
        public bool ContainsEdge(TVertex source, TVertex target)
        {
            return TryGetEdge(source, target, out _);
        }

        /// <inheritdoc />
        public bool TryGetEdge(TVertex source, TVertex target, out TEdge edge)
        {
            if (_vertexOutEdges.TryGetValue(source, out TEdge[] outEdges))
            {
                foreach (TEdge outEdge in outEdges)
                {
                    if (outEdge.Target.Equals(target))
                    {
                        edge = outEdge;
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
            if (_vertexOutEdges.TryGetValue(source, out TEdge[] outEdges))
            {
                edges = outEdges.Where(edge => edge.Target.Equals(target));
                return edges.Any();
            }

            edges = null;
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
        public ArrayAdjacencyGraph<TVertex, TEdge> Clone()
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
