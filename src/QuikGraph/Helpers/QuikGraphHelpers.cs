using System;
using JetBrains.Annotations;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif

namespace QuikGraph
{
    /// <summary>
    /// QuikGraph helpers.
    /// </summary>
    internal static class QuikGraphHelpers
    {
        /// <summary>
        /// Converts a <see cref="Func{T,TResult}"/> into a <see cref="TryFunc{T,TResult}"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        public static TryFunc<T, TResult> ToTryFunc<T, TResult>([NotNull] Func<T, TResult> func)
            where TResult : class
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(func != null);
#endif

            return (T value, out TResult result) =>
            {
                result = func(value);
                return result != null;
            };
        }
    }
}