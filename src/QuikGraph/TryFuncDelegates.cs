#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif

namespace QuikGraph
{
#if SUPPORTS_CONTRACTS
        [Pure]
#endif
    public delegate bool TryFunc<T, TResult>(T arg, out TResult result);

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
    public delegate bool TryFunc<T1, T2, TResult>(T1 arg, T2 arg2, out TResult result);

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
    public delegate bool TryFunc<T1, T2, T3, TResult>(T1 arg, T2 arg2, T3 arg3, out TResult result);

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
    public delegate bool TryFunc<T1, T2, T3, T4, TResult>(T1 arg, T2 arg2, T3 arg3, T4 arg4, out TResult result);
}
