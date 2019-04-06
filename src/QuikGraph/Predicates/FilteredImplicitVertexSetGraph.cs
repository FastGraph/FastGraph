#if SUPPORTS_SERIALIZATION
using System;
#endif
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif

namespace QuickGraph.Predicates
{
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public class FilteredImplicitVertexSet<TVertex, TEdge, TGraph> 
        : FilteredGraph<TVertex,TEdge,TGraph>
        , IImplicitVertexSet<TVertex>
        where TEdge : IEdge<TVertex>
        where TGraph : IGraph<TVertex, TEdge>, IImplicitVertexSet<TVertex>
    {
        public FilteredImplicitVertexSet(
            TGraph baseGraph,
            VertexPredicate<TVertex> vertexPredicate,
            EdgePredicate<TVertex, TEdge> edgePredicate
            )
            :base(baseGraph,vertexPredicate,edgePredicate)
        { }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public bool ContainsVertex(TVertex vertex)
        {
            return
                this.VertexPredicate(vertex) &&
                this.BaseGraph.ContainsVertex(vertex);
        }
    }
}
