#if SUPPORTS_SERIALIZATION
using System;
#endif
using JetBrains.Annotations;

namespace QuikGraph.Predicates
{
    /// <summary>
    /// Represents an implicit vertex set graph that is filtered with a vertex and an edge predicate.
    /// This means only vertex and edge matching predicates are "accessible".
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    /// <typeparam name="TGraph">Graph type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public class FilteredImplicitVertexSet<TVertex, TEdge, TGraph> 
        : FilteredGraph<TVertex, TEdge, TGraph>
        , IImplicitVertexSet<TVertex>
        where TEdge : IEdge<TVertex>
        where TGraph : IGraph<TVertex, TEdge>, IImplicitVertexSet<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilteredImplicitVertexSet{TVertex,TEdge,TGraph}"/> class.
        /// </summary>
        /// <param name="baseGraph">Graph in which applying predicates.</param>
        /// <param name="vertexPredicate">Predicate to match vertex that should be taken into account.</param>
        /// <param name="edgePredicate">Predicate to match edge that should be taken into account.</param>
        public FilteredImplicitVertexSet(
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
        public bool ContainsVertex(TVertex vertex)
        {
            return VertexPredicate(vertex)
                   && BaseGraph.ContainsVertex(vertex);
        }
    }
}
