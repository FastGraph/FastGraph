#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif

namespace QuickGraph
{
#if SUPPORTS_CONTRACTS
        [Pure]
#endif
    public delegate string VertexIdentity<TVertex>(TVertex v);
}
