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

        /// <summary>
        /// Asserts that both values are equal (or same if reference type).
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="arg1">First object.</param>
        /// <param name="arg2">Second object.</param>
        public static void AssertEqual<T>([CanBeNull] T arg1, [CanBeNull] T arg2)
        {
            if (typeof(T).IsValueType)
                Assert.AreEqual(arg1, arg2);
            else
                Assert.AreSame(arg1, arg2);
        }
    }
}