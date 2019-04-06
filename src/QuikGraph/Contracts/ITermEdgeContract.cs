using System;
using System.Collections.Generic;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif

namespace QuickGraph.Contracts
{
#if SUPPORTS_CONTRACTS
    [ContractClassFor(typeof(ITermEdge<>))]
#endif
    abstract class ITermEdgeContract<TVertex>
        : ITermEdge<TVertex>
    {
#if SUPPORTS_CONTRACTS
        [ContractInvariantMethod]
        void ITermEdgeInvariant()
        {
            ITermEdge<TVertex> ithis = this;
            Contract.Invariant(ithis.SourceTerminal >= 0);
            Contract.Invariant(ithis.TargetTerminal >= 0);
        }
#endif

        int ITermEdge<TVertex>.SourceTerminal
        {
            get
            {
#if SUPPORTS_CONTRACTS
                Contract.Ensures(Contract.Result<int>() >= 0);
#endif

                return -1;
            }
        }

        int ITermEdge<TVertex>.TargetTerminal
        {
            get
            {
#if SUPPORTS_CONTRACTS
                Contract.Ensures(Contract.Result<int>() >= 0);
#endif

                return -1;
            }
        }

#region IEdge<TVertex> Members

        TVertex IEdge<TVertex>.Source
        {
            get { throw new NotImplementedException(); }
        }

        TVertex IEdge<TVertex>.Target
        {
            get { throw new NotImplementedException(); }
        }

#endregion

    }
}
