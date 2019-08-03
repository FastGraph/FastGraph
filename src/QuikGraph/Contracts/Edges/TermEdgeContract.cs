#if SUPPORTS_CONTRACTS
using System;
using System.Diagnostics.Contracts;

namespace QuikGraph.Contracts
{
    /// <summary>
    /// Contract class for <see cref="ITermEdge{TVertex}"/>.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    [ContractClassFor(typeof(ITermEdge<>))]
    internal abstract class TermEdgeContract<TVertex> : ITermEdge<TVertex>
    {
        [ContractInvariantMethod]
        private void TermEdgeInvariant()
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            ITermEdge<TVertex> explicitThis = this;

            Contract.Invariant(explicitThis.SourceTerminal >= 0);
            Contract.Invariant(explicitThis.TargetTerminal >= 0);
        }

        int ITermEdge<TVertex>.SourceTerminal
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);

                return -1;
            }
        }

        int ITermEdge<TVertex>.TargetTerminal
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);

                return -1;
            }
        }

        #region IEdge<TVertex>

        TVertex IEdge<TVertex>.Source => throw new NotImplementedException();

        TVertex IEdge<TVertex>.Target => throw new NotImplementedException();

        #endregion
    }
}
#endif