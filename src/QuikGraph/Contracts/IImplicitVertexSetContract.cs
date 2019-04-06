#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif

namespace QuickGraph.Contracts
{
#if SUPPORTS_CONTRACTS
    [ContractClassFor(typeof(IImplicitVertexSet<>))]
#endif
    abstract class IImplicitVertexSetContract<TVertex>
        : IImplicitVertexSet<TVertex>
    {
#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        bool IImplicitVertexSet<TVertex>.ContainsVertex(TVertex vertex)
        {
            IImplicitVertexSet<TVertex> ithis = this;
#if SUPPORTS_CONTRACTS
            Contract.Requires(vertex != null);
#endif

            return default(bool);
        }
    }
}
