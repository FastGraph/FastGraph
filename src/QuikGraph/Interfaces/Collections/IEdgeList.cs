﻿#if SUPPORTS_CLONEABLE
using System;
#endif
using System.Collections.Generic;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using JetBrains.Annotations;

namespace QuikGraph.Collections
{
    /// <summary>
    /// Represents a cloneable list of edges.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_CONTRACTS
    [ContractClass(typeof(EdgeListContract<,>))]
#endif
    public interface IEdgeList<TVertex, TEdge> : IList<TEdge>
#if SUPPORTS_CLONEABLE
        , ICloneable
#endif
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Trims excess allocated space.
        /// </summary>
        void TrimExcess();

        /// <summary>
        /// Gets a clone of this list.
        /// </summary>
        /// <returns>Cloned list.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        [NotNull]
#if SUPPORTS_CLONEABLE
        new
#endif
        IEdgeList<TVertex, TEdge> Clone();
    }
}