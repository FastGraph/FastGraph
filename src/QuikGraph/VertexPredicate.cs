#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif

namespace QuickGraph
{
#if SUPPORTS_CONTRACTS
        [Pure]
#endif
    public delegate bool VertexPredicate<TVertex>(TVertex v);
}
