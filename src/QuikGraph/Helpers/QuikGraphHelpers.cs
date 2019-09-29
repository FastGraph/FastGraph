using System;
using System.Diagnostics;
using JetBrains.Annotations;

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
        [Pure]
        [NotNull]
        public static TryFunc<T, TResult> ToTryFunc<T, TResult>([NotNull] Func<T, TResult> func)
            where TResult : class
        {
            Debug.Assert(func != null);

            return (T value, out TResult result) =>
            {
                result = func(value);
                return result != null;
            };
        }
    }
}