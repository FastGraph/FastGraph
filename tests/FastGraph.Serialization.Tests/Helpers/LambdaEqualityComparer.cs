using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace FastGraph.Serialization.Tests
{
    /// <summary>
    /// <see cref="IEqualityComparer{T}"/> that can be constructed from lambdas.
    /// </summary>
    /// <typeparam name="T">Element to compare type.</typeparam>
    internal sealed class LambdaEqualityComparer<T> : IEqualityComparer<T>
    {
        [NotNull]
        private readonly Func<T, T, bool> _comparer;

        [NotNull]
        private readonly Func<T, int> _hashGenerator;

        private LambdaEqualityComparer([NotNull] Func<T, T, bool> comparer, [NotNull] Func<T, int> hash)
        {
            _comparer = comparer;
            _hashGenerator = hash;
        }

        /// <inheritdoc />
        public bool Equals(T x, T y)
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
        [NotNull]
        public static IEqualityComparer<T> Create([NotNull] Func<T, T, bool> comparer, [NotNull] Func<T, int> hash)
        {
            return new LambdaEqualityComparer<T>(comparer, hash);
        }
    }
}