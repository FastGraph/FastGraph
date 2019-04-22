using System;
using System.Collections.Generic;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using System.Linq;
using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// A delegate-based incidence graph.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public class DelegateVertexAndEdgeListGraph<TVertex, TEdge> : DelegateIncidenceGraph<TVertex, TEdge>, IVertexAndEdgeListGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>, IEquatable<TEdge>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateVertexAndEdgeListGraph{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="vertices">Graph vertices.</param>
        /// <param name="tryGetOutEdges">Getter of out-edges.</param>
        public DelegateVertexAndEdgeListGraph(
            [NotNull, ItemNotNull] IEnumerable<TVertex> vertices,
            [NotNull] TryFunc<TVertex, IEnumerable<TEdge>> tryGetOutEdges)
            : base(tryGetOutEdges)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(vertices != null);
            Contract.Requires(vertices.All(vertex => tryGetOutEdges(vertex, out _)));
#endif

            _vertices = vertices;
        }

        #region IVertexSet<TVertex>

        /// <inheritdoc />
        public bool IsVerticesEmpty
        {
            get
            {
                // Shortcut if count is already computed
                if (_vertexCount > -1)
                    return _vertexCount == 0;
                return !_vertices.Any();
            }
        }

        private int _vertexCount = -1;

        /// <inheritdoc />
        public int VertexCount
        {
            get
            {
                if (_vertexCount < 0)
                    _vertexCount = _vertices.Count();
                return _vertexCount;
            }
        }

        [NotNull]
        private readonly IEnumerable<TVertex> _vertices;

        /// <inheritdoc />
        public virtual IEnumerable<TVertex> Vertices => _vertices;

        #endregion

        #region IEdgeSet<TVertex,TEdge>

        /// <inheritdoc />
        public bool IsEdgesEmpty
        {
            get
            {
                // Shortcut if count is already computed
                if (_edgeCount > -1)
                    return _edgeCount == 0;

                return _vertices.All(vertex => !OutEdges(vertex).Any());
            }
        }

        private int _edgeCount = -1;

        /// <inheritdoc />
        public int EdgeCount
        {
            get
            {
                if (_edgeCount < 0)
                    _edgeCount = Edges.Count();
                return _edgeCount;
            }
        }

        /// <inheritdoc />
        public virtual IEnumerable<TEdge> Edges => _vertices.SelectMany(OutEdges);

        /// <inheritdoc />
        public bool ContainsEdge(TEdge edge)
        {
            if (TryGetOutEdges(edge.Source, out IEnumerable<TEdge> edges))
                return edges.Any(e => e.Equals(edge));
            return false;
        }

        #endregion
    }
}
