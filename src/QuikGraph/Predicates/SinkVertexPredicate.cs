#if SUPPORTS_SERIALIZATION
using System;
#endif
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif

namespace QuikGraph.Predicates
{
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class SinkVertexPredicate<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private readonly IIncidenceGraph<TVertex, TEdge> visitedGraph;

        public SinkVertexPredicate(IIncidenceGraph<TVertex, TEdge> visitedGraph)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(visitedGraph != null);
#endif

            this.visitedGraph = visitedGraph;
        }

        public bool Test(TVertex v)
        {
            return this.visitedGraph.IsOutEdgesEmpty(v);
        }
    }
}
