using System;
using System.Collections.Generic;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif

namespace QuickGraph.Contracts
{
#if SUPPORTS_CONTRACTS
    [ContractClassFor(typeof(IUndirectedEdge<>))]
#endif
    abstract class IUndirectedEdgeContract<TVertex>
        : IUndirectedEdge<TVertex>
    {
#if SUPPORTS_CONTRACTS
        [ContractInvariantMethod]
        void IUndirectedEdgeInvariant()
        {
            IUndirectedEdge<TVertex> ithis = this;
            Contract.Invariant(Comparer<TVertex>.Default.Compare(ithis.Source, ithis.Target) <= 0);
        }
#endif

#region IEdge<TVertex> Members

        public TVertex Source {
          get { throw new NotImplementedException(); }
        }

        public TVertex Target {
          get { throw new NotImplementedException(); }
        }

#endregion
    }
}
