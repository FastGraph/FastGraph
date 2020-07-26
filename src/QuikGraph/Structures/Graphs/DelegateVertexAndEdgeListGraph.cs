using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// A delegate-based directed graph data structure.
    /// This graph is vertex immutable.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public class DelegateVertexAndEdgeListGraph<TVertex, TEdge> : DelegateIncidenceGraph<TVertex, TEdge>, IVertexAndEdgeListGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateVertexAndEdgeListGraph{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="vertices">Graph vertices.</param>
        /// <param name="tryGetOutEdges">Getter of out-edges.</param>
        /// <param name="allowParallelEdges">
        /// Indicates if parallel edges are allowed.
        /// Note that get of edges is delegated so you may have bugs related
        /// to parallel edges due to the delegated implementation.
        /// </param>
        public DelegateVertexAndEdgeListGraph(
            [NotNull, ItemNotNull] IEnumerable<TVertex> vertices,
            [NotNull] TryFunc<TVertex, IEnumerable<TEdge>> tryGetOutEdges,
            bool allowParallelEdges = true)
            : base(tryGetOutEdges, allowParallelEdges)
        {
            _vertices = vertices ?? throw new ArgumentNullException(nameof(vertices));
        }

        #region IVertexSet<TVertex>

        /// <inheritdoc />
        public bool IsVerticesEmpty => !_vertices.Any();

        /// <inheritdoc />
        public int VertexCount => _vertices.Count();

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
                if (VertexCount == 0)
                    return true; // No vertex => must be empty 
                return _vertices.All(vertex => !OutEdges(vertex).Any());
            }
        }

        /// <inheritdoc />
        public int EdgeCount => Edges.Count();  // Cannot be cache since it depends on user delegate

        /// <inheritdoc />
        public virtual IEnumerable<TEdge> Edges =>
            _vertices.SelectMany(
                vertex => OutEdges(vertex).Where(edge => EqualityComparer<TVertex>.Default.Equals(edge.Source, vertex)));

        /// <inheritdoc />
        public bool ContainsEdge(TEdge edge)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            if (TryGetOutEdges(edge.Source, out IEnumerable<TEdge> edges))
                return edges.Any(e => EqualityComparer<TEdge>.Default.Equals(e, edge));
            return false;
        }

        #endregion

        #region IImplicitVertexSet<TVertex>

        // Should override parent implementation since the provided delegate
        // may not be accurate to check a vertex is present or not.
        // In case a vertex is part of an edge returned by user delegate 
        // but not part of the graph.
        /// <inheritdoc />
        internal override bool ContainsVertexInternal(TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            return _vertices.Any(v => EqualityComparer<TVertex>.Default.Equals(vertex, v));
        }

        #endregion

        #region IIncidenceGraph<TVertex,TEdge>

        private bool FilterEdges([NotNull] TEdge edge, [NotNull] TVertex vertex)
        {
            return IsInGraph(edge, vertex) && EqualityComparer<TVertex>.Default.Equals(edge.Source, vertex);
        }

        /// <summary>
        /// Checks if the given <paramref name="edge"/> is part of the graph
        /// by checking if the other vertex of the edge is also in the graph.
        /// It requires to have been checked if <paramref name="vertex"/> is
        /// in the graph before.
        /// </summary>
        [Pure]
        private bool IsInGraph([NotNull] TEdge edge, [NotNull] TVertex vertex)
        {
            Debug.Assert(edge != null);
            Debug.Assert(vertex != null);

            return ContainsVertexInternal(edge.GetOtherVertex(vertex));
        }

        // Should override parent implementation since the provided delegate
        // may not be accurate to check an edge is present or not.
        // In case source or target is part of an edge returned by user delegate 
        // but not part of the graph.
        /// <inheritdoc />
        internal override bool ContainsEdgeInternal(TVertex source, TVertex target)
        {
            if (base.ContainsEdgeInternal(source, target))
                return ContainsVertex(source) && ContainsVertex(target);
            return false;
        }

        // Should override parent implementation since the provided delegate
        // may not be accurate and returning too many edges that cannot be part
        // of the graph (edge between vertices not in the graph).
        /// <inheritdoc />
        internal override IEnumerable<TEdge> OutEdgesInternal(TVertex vertex)
        {
            if (!ContainsVertexInternal(vertex))
                throw new VertexNotFoundException();
            return base.OutEdgesInternal(vertex).Where(edge => FilterEdges(edge, vertex));
        }

        /// <inheritdoc />
        internal override bool TryGetOutEdgesInternal(TVertex vertex, out IEnumerable<TEdge> edges)
        {
            if (!ContainsVertexInternal(vertex))
            {
                edges = null;
                return false;
            }

            // Ignore return because "vertex" exists in the graph
            // so it should always return true.
            base.TryGetOutEdgesInternal(vertex, out IEnumerable<TEdge> unfilteredEdges);

            edges = unfilteredEdges is null
                ? Enumerable.Empty<TEdge>()
                : unfilteredEdges.Where(edge => FilterEdges(edge, vertex));

            return true;
        }

        #endregion
    }
}