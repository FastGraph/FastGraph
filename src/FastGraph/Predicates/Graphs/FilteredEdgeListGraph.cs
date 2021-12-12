#nullable enable

namespace FastGraph.Predicates
{
    /// <summary>
    /// Edge list graph data structure that is filtered with a vertex and an edge
    /// predicate. This means only vertex and edge matching predicates are "accessible".
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    /// <typeparam name="TGraph">Graph type.</typeparam>
    public sealed class FilteredEdgeListGraph<TVertex, TEdge, TGraph>
        : FilteredImplicitVertexSet<TVertex, TEdge, TGraph>
        , IEdgeListGraph<TVertex, TEdge>
        where TVertex : notnull
        where TGraph : IEdgeListGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilteredEdgeListGraph{TVertex,TEdge,TGraph}"/> class.
        /// </summary>
        /// <param name="baseGraph">Graph in which applying predicates.</param>
        /// <param name="vertexPredicate">Predicate to match vertex that should be taken into account.</param>
        /// <param name="edgePredicate">Predicate to match edge that should be taken into account.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="baseGraph"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="vertexPredicate"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="edgePredicate"/> is <see langword="null"/>.</exception>
        public FilteredEdgeListGraph(
            TGraph baseGraph,
            VertexPredicate<TVertex> vertexPredicate,
            EdgePredicate<TVertex, TEdge> edgePredicate)
            : base(baseGraph, vertexPredicate, edgePredicate)
        {
        }

        #region IVertexSet<TVertex>

        /// <inheritdoc />
        public bool IsVerticesEmpty => !Vertices.Any();

        /// <inheritdoc />
        public int VertexCount => Vertices.Count();

        /// <inheritdoc />
        public IEnumerable<TVertex> Vertices => BaseGraph.Vertices.Where(vertex => VertexPredicate(vertex));

        #endregion

        #region IEdgeSet<TVertex,TEdge>

        /// <inheritdoc />
        public bool IsEdgesEmpty => !Edges.Any();

        /// <inheritdoc />
        public int EdgeCount => Edges.Count();

        /// <inheritdoc />
        public IEnumerable<TEdge> Edges => BaseGraph.Edges.Where(FilterEdge);

        /// <inheritdoc />
        public bool ContainsEdge(TEdge edge)
        {
            return FilterEdge(edge) && BaseGraph.ContainsEdge(edge);
        }

        #endregion
    }
}
