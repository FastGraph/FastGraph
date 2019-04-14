#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif

namespace QuikGraph
{
#if SUPPORTS_CONTRACTS
        [Pure]
#endif
    public delegate bool VertexPredicate<TVertex>(TVertex v);
}
