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

        /// <inheritdoc />
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [Pure]
        public bool IsInEdgesEmpty(TVertex vertex)
        {
            return InDegree(vertex) == 0;
        }

        /// <inheritdoc />
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [Pure]
        public int InDegree(TVertex vertex)
        {
            return InEdges(vertex).Count();
        }

        /// <inheritdoc />
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [Pure]
        public IEnumerable<TEdge> InEdges(TVertex vertex)
        {
            return BaseGraph.InEdges(vertex).Where(FilterEdge);
        }

        /// <inheritdoc />
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [Pure]
        public bool TryGetInEdges(TVertex vertex, out IEnumerable<TEdge> edges)
        {
            if (ContainsVertex(vertex))
            {
                edges = InEdges(vertex);
                return true;
            }

            edges = null;
            return false;
        }

        /// <inheritdoc />
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [Pure]
        public int Degree(TVertex vertex)
        {
            return OutDegree(vertex) + InDegree(vertex);
        }

        /// <inheritdoc />
        public bool IsEdgesEmpty => EdgeCount == 0;

        /// <inheritdoc />
        public int EdgeCount => Edges.Count();

        /// <inheritdoc />
        public IEnumerable<TEdge> Edges => BaseGraph.Edges.Where(FilterEdge);

        /// <inheritdoc />
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [Pure]
        public bool ContainsEdge(TEdge edge)
        {
            return FilterEdge(edge)
                && BaseGraph.ContainsEdge(edge);
        }

        /// <summary>
        /// <see cref="InEdge"/> is not supported for this kind of graph.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [Pure]
        public TEdge InEdge(TVertex vertex, int index)
        {
            throw new NotSupportedException();
        }
    }
}
