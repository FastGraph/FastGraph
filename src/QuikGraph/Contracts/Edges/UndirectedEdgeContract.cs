#if SUPPORTS_CONTRACTS
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace QuikGraph.Contracts
{
    /// <summary>
    /// Contract class for <see cref="IUndirectedEdge{TVertex}"/>.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    [ContractClassFor(typeof(IUndirectedEdge<>))]
    internal abstract class UndirectedEdgeContract<TVertex> : IUndirectedEdge<TVertex>
    {
        [ContractInvariantMethod]
        private void UndirectedEdgeInvariant()
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IUndirectedEdge<TVertex> explicitThis = this;

            Contract.Invariant(
                Comparer<TVertex>.Default.Compare(explicitThis.Source, explicitThis.Target) <= 0);
        }

        #region IEdge<TVertex>

        public TVertex Source => throw new NotImplementedException();

        public TVertex Target => throw new NotImplementedException();

        #endregion
    }
}
#endif