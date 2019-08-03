using System;
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
        public static TryFunc<T, TResult> ToTryFunc<T, TResult>([NotNull] Func<T, TResult> func)
            where TResult : class
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));

            return (T value, out TResult result) =>
            {
                result = func(value);
                return result != null;
            };
        }
    }
}