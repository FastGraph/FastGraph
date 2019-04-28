using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace QuikGraph.Predicates
{
    /// <summary>
    /// Represents an undirected graph that is filtered with a vertex and an edge predicate.
    /// This means only vertex and edge matching predicates are "accessible".
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    /// <typeparam name="TGraph">Graph type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class FilteredUndirectedGraph<TVertex, TEdge, TGraph>
        : FilteredGraph<TVertex, TEdge, TGraph>
        , IUndirectedGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
        where TGraph : IUndirectedGraph<TVertex, TEdge>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilteredUndirectedGraph{TVertex,TEdge,TGraph}"/> class.
        /// </summary>
        /// <param name="baseGraph">Graph in which applying predicates.</param>
        /// <param name="vertexPredicate">Predicate to match vertex that should be taken into account.</param>
        /// <param name="edgePredicate">Predicate to match edge that should be taken into account.</param>
        public FilteredUndirectedGraph(
            [NotNull] TGraph baseGraph,
            [NotNull] VertexPredicate<TVertex> vertexPredicate,
            [NotNull] EdgePredicate<TVertex, TEdge> edgePredicate)
            : base(baseGraph, vertexPredicate, edgePredicate)
        {
        }

        /// <inheritdoc />
        public EdgeEqualityComparer<TVertex, TEdge> EdgeEqualityComparer { get; } =
            EdgeExtensions.GetUndirectedVertexEquality<TVertex, TEdge>();

        #region IVertexSet<TVertex>

        /// <inheritdoc />
        public bool IsVerticesEmpty => VertexCount == 0;

        /// <inheritdoc />
        public int VertexCount => Vertices.Count();

        /// <inheritdoc />
        public IEnumerable<TVertex> Vertices => BaseGraph.Vertices.Where(vertex => VertexPredicate(vertex));

        /// <inheritdoc />
        public bool ContainsVertex(TVertex vertex)
        {
            return VertexPredicate(vertex)
                   && BaseGraph.ContainsVertex(vertex);
        }

        #endregion

        #region IEdgeSet<TVertex,TEdge>

        /// <inheritdoc />
        public bool IsEdgesEmpty => EdgeCount == 0;

        /// <inheritdoc />
        public int EdgeCount => Edges.Count();

        /// <inheritdoc />
        public IEnumerable<TEdge> Edges => BaseGraph.Edges.Where(FilterEdge);

        /// <inheritdoc />
        public bool ContainsEdge(TEdge edge)
        {
            return FilterEdge(edge) 
                   && BaseGraph.ContainsEdge(edge);
        }

        #endregion

        #region IImplicitUndirectedGraph<TVertex,TEdge>

        /// <inheritdoc />
        public IEnumerable<TEdge> AdjacentEdges(TVertex vertex)
        {
            if (VertexPredicate(vertex))
                return BaseGraph.AdjacentEdges(vertex).Where(FilterEdge);
            return Enumerable.Empty<TEdge>();
        }

        /// <inheritdoc />
        public int AdjacentDegree(TVertex vertex)
        {
            return AdjacentEdges(vertex).Count();
        }

        /// <inheritdoc />
        public bool IsAdjacentEdgesEmpty(TVertex vertex)
        {
            return AdjacentDegree(vertex) == 0;
        }

        /// <inheritdoc />
        public TEdge AdjacentEdge(TVertex vertex, int index)
        {
            if (VertexPredicate(vertex))
                return AdjacentEdges(vertex).ElementAt(index);

            throw new ArgumentOutOfRangeException(nameof(index));
        }

        /// <inheritdoc />
        public bool TryGetEdge(TVertex source, TVertex target, out TEdge edge)
        {
            if (VertexPredicate(source)
                && VertexPredicate(target))
            {
                // We need to find the edge
                foreach (TEdge e in Edges)
                {
                    if (EdgeEqualityComparer(e, source, target)
                        && EdgePredicate(e))
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
        public bool ContainsEdge(TVertex source, TVertex target)
        {
            return TryGetEdge(source, target, out _);
        }

        #endregion
    }
}
