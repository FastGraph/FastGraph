using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace QuikGraph.Predicates
{
    /// <summary>
    /// Represents an implicit graph that is filtered with a vertex and an edge predicate.
    /// This means only vertex and edge matching predicates are "accessible".
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    /// <typeparam name="TGraph">Graph type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public class FilteredImplicitGraph<TVertex, TEdge, TGraph> 
        : FilteredImplicitVertexSet<TVertex, TEdge, TGraph>
        , IImplicitGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
        where TGraph : IImplicitGraph<TVertex, TEdge>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilteredImplicitGraph{TVertex,TEdge,TGraph}"/> class.
        /// </summary>
        /// <param name="baseGraph">Graph in which applying predicates.</param>
        /// <param name="vertexPredicate">Predicate to match vertex that should be taken into account.</param>
        /// <param name="edgePredicate">Predicate to match edge that should be taken into account.</param>
        public FilteredImplicitGraph(
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
        public bool IsOutEdgesEmpty(TVertex vertex)
        {
            return OutDegree(vertex) == 0;
        }

        /// <inheritdoc />
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [Pure]
        public int OutDegree(TVertex vertex)
        {
            return OutEdges(vertex).Count();
        }

        /// <inheritdoc />
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [Pure]
        public IEnumerable<TEdge> OutEdges(TVertex vertex)
        {
            return BaseGraph.OutEdges(vertex).Where(FilterEdge);
        }

        /// <inheritdoc />
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [Pure]
        public bool TryGetOutEdges(TVertex vertex, out IEnumerable<TEdge> edges)
        {
            if (!BaseGraph.TryGetOutEdges(vertex, out _))
            {
                edges = null;
                return false;
            }

            edges = OutEdges(vertex);
            return true;
        }

        /// <summary>
        /// <see cref="OutEdge"/> is not supported for this kind of graph.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [Pure]
        public TEdge OutEdge(TVertex vertex, int index)
        {
            throw new NotSupportedException();
        }
    }
}
