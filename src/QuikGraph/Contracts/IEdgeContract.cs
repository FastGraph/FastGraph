#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif

namespace QuickGraph.Contracts
{
#if SUPPORTS_CONTRACTS
    [ContractClassFor(typeof(IEdge<>))]
#endif
    abstract class IEdgeContract<TVertex>
        : IEdge<TVertex>
    {
#if SUPPORTS_CONTRACTS
        [ContractInvariantMethod]
        void IEdgeInvariant()
        {
            IEdge<TVertex> ithis = this;
            Contract.Invariant(ithis.Source != null);
            Contract.Invariant(ithis.Target != null);
        }
#endif

        TVertex IEdge<TVertex>.Source
        {
            get
            {
#if SUPPORTS_CONTRACTS
                Contract.Ensures(Contract.Result<TVertex>() != null);
#endif

                return default(TVertex);
            }
        }

        TVertex IEdge<TVertex>.Target
        {
            get
            {
#if SUPPORTS_CONTRACTS
                Contract.Ensures(Contract.Result<TVertex>() != null);
#endif

                return default(TVertex);
            }
        }
    }
}
