#if SUPPORTS_CLONEABLE
using System;
#endif
using System.Collections.Generic;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif

namespace QuickGraph.Collections
{
    /// <summary>
    /// A cloneable list of edges
    /// </summary>
    /// <typeparam name="TVertex"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
#if SUPPORTS_CONTRACTS
    [ContractClass(typeof(IEdgeListContract<,>))]
#endif
    public interface IEdgeList<TVertex, TEdge>
        : IList<TEdge>
#if SUPPORTS_CLONEABLE
        , ICloneable
#endif
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Trims excess allocated space
        /// </summary>
        void TrimExcess();
        /// <summary>
        /// Gets a clone of this list
        /// </summary>
        /// <returns></returns>
#if SUPPORTS_CLONEABLE
        new 
#endif
        IEdgeList<TVertex, TEdge> Clone();
    }
}
