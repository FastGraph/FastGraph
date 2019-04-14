#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif

namespace QuikGraph
{
#if SUPPORTS_CONTRACTS
        [Pure]
#endif
    public delegate bool EdgePredicate<TVertex, TEdge>(TEdge e)
        where TEdge : IEdge<TVertex>;
}
