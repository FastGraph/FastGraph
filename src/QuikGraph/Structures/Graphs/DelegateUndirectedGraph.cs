using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// A delegate-based implicit undirected graph.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public class DelegateUndirectedGraph<TVertex, TEdge> : DelegateImplicitUndirectedGraph<TVertex, TEdge>, IUndirectedGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateUndirectedGraph{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="vertices">Graph vertices.</param>
        /// <param name="tryGetAdjacentEdges">Getter of adjacent edges.</param>
        /// <param name="allowParallelEdges">Indicates if parallel edges are allowed.</param>
        public DelegateUndirectedGraph(
            [NotNull, ItemNotNull] IEnumerable<TVertex> vertices,
            [NotNull] TryFunc<TVertex, IEnumerable<TEdge>> tryGetAdjacentEdges,
            bool allowParallelEdges)
            : base(tryGetAdjacentEdges, allowParallelEdges)
        {
            _vertices = vertices ?? throw new ArgumentNullException(nameof(vertices));
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

        private int _edgeCount = -1;

        /// <inheritdoc />
        public bool IsEdgesEmpty
        {
            get
            {
                if (_vertexCount == 0 || _edgeCount == 0)
                    return true; // No vertices or no edges

                return _vertices.All(vertex => !AdjacentEdges(vertex).Any());
            }
        }

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
        public virtual IEnumerable<TEdge> Edges =>
            _vertices.SelectMany(
                vertex => AdjacentEdges(vertex).Where(edge => edge.Source.Equals(vertex)));

        /// <inheritdoc />
        public bool ContainsEdge(TEdge edge)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            if (TryGetAdjacentEdges(edge.Source, out IEnumerable<TEdge> edges))
                return edges.Any(e => e.Equals(edge));
            return false;
        }

        #endregion
    }
}
