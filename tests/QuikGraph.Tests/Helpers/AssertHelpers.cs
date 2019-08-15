using System;
using JetBrains.Annotations;
using NUnit.Framework;

namespace QuikGraph.Tests
{
    /// <summary>
    /// Assert helpers for tests.
    /// </summary>
    internal static class AssertHelpers
    {
        /// <summary>
        /// Asserts that the <paramref name="action"/> throws an
        /// <see cref="ArgumentOutOfRangeException"/> or <see cref="IndexOutOfRangeException"/>.
        /// </summary>
        /// <param name="action">Test delegate.</param>
        public static void AssertIndexOutOfRange([NotNull, InstantHandle] TestDelegate action)
        {
            Assert.That(
                action,
                Throws.Exception
                    .TypeOf<ArgumentOutOfRangeException>()
                    .Or
                    .TypeOf<IndexOutOfRangeException>());
        }
    }
}