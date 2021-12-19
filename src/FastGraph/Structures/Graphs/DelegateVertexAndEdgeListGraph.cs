#nullable enable

using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace FastGraph
{
    /// <summary>
    /// A delegate-based directed graph data structure.
    /// This graph is vertex immutable.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public class DelegateVertexAndEdgeListGraph<TVertex, TEdge> : DelegateIncidenceGraph<TVertex, TEdge>, IVertexAndEdgeListGraph<TVertex, TEdge>
        where TVertex : notnull
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
        /// <exception cref="T:System.ArgumentNullException"><paramref name="vertices"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="tryGetOutEdges"/> is <see langword="null"/>.</exception>
        public DelegateVertexAndEdgeListGraph(
            IEnumerable<TVertex> vertices,
            TryFunc<TVertex, IEnumerable<TEdge>> tryGetOutEdges,
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
                vertex => OutEdges(vertex).Where(outEdge => EqualityComparer<TVertex>.Default.Equals(outEdge.Source, vertex)));

        /// <inheritdoc />
        public bool ContainsEdge(TEdge edge)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            if (TryGetOutEdges(edge.Source, out IEnumerable<TEdge>? outEdges))
                return outEdges.Any(outEdge => EqualityComparer<TEdge>.Default.Equals(outEdge, edge));
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

        private bool FilterEdges(TEdge edge, TVertex vertex)
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
        private bool IsInGraph(TEdge edge, TVertex vertex)
        {
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
            return base.OutEdgesInternal(vertex).Where(outEdge => FilterEdges(outEdge, vertex));
        }

        /// <inheritdoc />
        internal override bool TryGetOutEdgesInternal(TVertex vertex, [NotNullWhen(true)] out IEnumerable<TEdge>? edges)
        {
            if (!ContainsVertexInternal(vertex))
            {
                edges = default;
                return false;
            }

            // Ignore return because "vertex" exists in the graph
            // so it should always return true.
            base.TryGetOutEdgesInternal(vertex, out IEnumerable<TEdge>? unfilteredOutEdges);

            edges = unfilteredOutEdges is null
                ? Enumerable.Empty<TEdge>()
                : unfilteredOutEdges.Where(outEdge => FilterEdges(outEdge, vertex));

            return true;
        }

        #endregion
    }
}
