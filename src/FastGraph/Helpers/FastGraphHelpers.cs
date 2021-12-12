#nullable enable

using JetBrains.Annotations;

namespace FastGraph
{
    /// <summary>
    /// FastGraph helpers.
    /// </summary>
    internal static class FastGraphHelpers
    {
        /// <summary>
        /// Converts a <see cref="Func{T,TResult}"/> into a <see cref="TryFunc{T,TResult}"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        [Pure]
        public static TryFunc<T, TResult> ToTryFunc<T, TResult>(Func<T, TResult?> func)
            where TResult : class
        {
            return (T value, out TResult? result) =>
            {
                result = func(value);
                return result != default;
            };
        }
    }
}
