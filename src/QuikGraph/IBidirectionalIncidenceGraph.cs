using System.Collections.Generic;
using QuikGraph.Contracts;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;

#endif

namespace QuikGraph
{
    /// <summary>
    /// A directed graph data structure that is efficient
    /// to traverse both in and out edges.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
#if SUPPORTS_CONTRACTS
    [ContractClass(typeof(IBidirectionalIncidenceGraphContract<,>))]
#endif
    public interface IBidirectionalIncidenceGraph<TVertex, TEdge>
        : IIncidenceGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Determines whether <paramref name="v"/> has no in-edges.
        /// </summary>
        /// <param name="v">The vertex</param>
        /// <returns>
        /// 	<c>true</c> if <paramref name="v"/> has no in-edges; otherwise, <c>false</c>.
        /// </returns>
#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        bool IsInEdgesEmpty(TVertex v);

        /// <summary>
        /// Gets the number of in-edges of <paramref name="v"/>
        /// </summary>
        /// <param name="v">The vertex.</param>
        /// <returns>The number of in-edges pointing towards <paramref name="v"/></returns>
#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        int InDegree(TVertex v);

        /// <summary>
        /// Gets the collection of in-edges of <paramref name="v"/>.
        /// </summary>
        /// <param name="v">The vertex</param>
        /// <returns>The collection of in-edges of <paramref name="v"/></returns>
#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        IEnumerable<TEdge> InEdges(TVertex v);

        /// <summary>
        /// Tries to get the in-edges of <paramref name="v"/>
        /// </summary>
        /// <param name="v"></param>
        /// <param name="edges"></param>
        /// <returns></returns>
#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        bool TryGetInEdges(TVertex v, out IEnumerable<TEdge> edges);

        /// <summary>
        /// Gets the in-edge at location <paramref name="index"/>.
        /// </summary>
        /// <param name="v">The vertex.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        TEdge InEdge(TVertex v, int index);

        /// <summary>
        /// Gets the degree of <paramref name="v"/>, i.e.
        /// the sum of the out-degree and in-degree of <paramref name="v"/>.
        /// </summary>
        /// <param name="v">The vertex</param>
        /// <returns>The sum of OutDegree and InDegree of <paramref name="v"/></returns>
#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        int Degree(TVertex v);
    }
}
