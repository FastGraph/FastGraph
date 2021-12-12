#nullable enable

using JetBrains.Annotations;

namespace FastGraph.Serialization.Tests
{
    /// <summary>
    /// <see cref="IEqualityComparer{T}"/> that can be constructed from lambdas.
    /// </summary>
    /// <typeparam name="T">Element to compare type.</typeparam>
    internal sealed class LambdaEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _comparer;

        private readonly Func<T, int> _hashGenerator;

        private LambdaEqualityComparer(Func<T, T, bool> comparer, Func<T, int> hash)
        {
            _comparer = comparer;
            _hashGenerator = hash;
        }

        /// <inheritdoc />
#pragma warning disable CS8767
        public bool Equals(T x, T y)
#pragma warning restore CS8767
        {
            return _comparer(x, y);
        }

        /// <inheritdoc />
        public int GetHashCode(T value)
        {
            return _hashGenerator(value);
        }

        /// <summary>
        /// Creates <see cref="IEqualityComparer{T}"/> from given <paramref name="comparer"/> and <paramref name="hash"/> lambdas.
        /// </summary>
        [Pure]
        public static IEqualityComparer<T> Create(Func<T, T, bool> comparer, Func<T, int> hash)
        {
            return new LambdaEqualityComparer<T>(comparer, hash);
        }
    }
}
