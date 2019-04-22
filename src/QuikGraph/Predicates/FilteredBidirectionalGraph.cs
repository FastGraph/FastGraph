using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace QuikGraph.Predicates
{
    /// <summary>
    /// Represents a bidirectional graph that is filtered with a vertex and an edge predicate.
    /// This means only vertex and edge matching predicates are "accessible".
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    /// <typeparam name="TGraph">Graph type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public class FilteredBidirectionalGraph<TVertex, TEdge, TGraph>
        : FilteredVertexListGraph<TVertex, TEdge, TGraph>
        , IBidirectionalGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
        where TGraph : IBidirectionalGraph<TVertex, TEdge>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilteredBidirectionalGraph{TVertex,TEdge,TGraph}"/> class.
        /// </summary>
        /// <param name="baseGraph">Graph in which applying predicates.</param>
        /// <param name="vertexPredicate">Predicate to match vertex that should be taken into account.</param>
        /// <param name="edgePredicate">Predicate to match edge that should be taken into account.</param>
        public FilteredBidirectionalGraph(
            [NotNull] TGraph baseGraph,
            [NotNull] VertexPredicate<TVertex> vertexPredicate,
            [NotNull] EdgePredicate<TVertex, TEdge> edgePredicate)
            : base(baseGraph, vertexPredicate, edgePredicate)
        {
        }

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
            return FilterEdge(edge) && BaseGraph.ContainsEdge(edge);
        }

        #endregion

        #region IBidirectionalIncidenceGraph<TVertex,TEdge>

        /// <inheritdoc />
        public bool IsInEdgesEmpty(TVertex vertex)
        {
            return InDegree(vertex) == 0;
        }

        /// <inheritdoc />
        public int InDegree(TVertex vertex)
        {
            return InEdges(vertex).Count();
        }

        /// <inheritdoc />
        public IEnumerable<TEdge> InEdges(TVertex vertex)
        {
            return BaseGraph.InEdges(vertex).Where(FilterEdge);
        }

        /// <inheritdoc />
        public bool TryGetInEdges(TVertex vertex, out IEnumerable<TEdge> edges)
        {
            if (ContainsVertex(vertex))
            {
                TEdge[] inEdges = InEdges(vertex).ToArray();
                edges = inEdges;
                return inEdges.Length > 0;
            }

            edges = null;
            return false;
        }

        /// <summary>
        /// <see cref="InEdge"/> is not supported for this kind of graph.
        /// </summary>
        public TEdge InEdge(TVertex vertex, int index)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public int Degree(TVertex vertex)
        {
            return OutDegree(vertex) + InDegree(vertex);
        }

        #endregion
    }
}
