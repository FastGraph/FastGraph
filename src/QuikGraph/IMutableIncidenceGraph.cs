#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
using QuikGraph.Contracts;

#endif

namespace QuikGraph
{
    /// <summary>
    /// A mutable incidence graph
    /// </summary>
    /// <typeparam name="TVertex"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
#if SUPPORTS_CONTRACTS
    [ContractClass(typeof(IMutableIncidenceGraphContract<,>))]
#endif
    public interface IMutableIncidenceGraph<TVertex,TEdge> 
        : IMutableGraph<TVertex,TEdge>
        , IIncidenceGraph<TVertex,TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Removes all out edges of <paramref name="v"/>
        /// where <paramref name="predicate"/> evalutes to true.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        int RemoveOutEdgeIf(
            TVertex v,
            EdgePredicate<TVertex, TEdge> predicate);

        /// <summary>
        /// Trims the out edges of vertex <paramref name="v"/>
        /// </summary>
        /// <param name="v"></param>
        void ClearOutEdges(TVertex v);

        /// <summary>
        /// Trims excess storage allocated for edges
        /// </summary>
        void TrimEdgeExcess();
    }
}
