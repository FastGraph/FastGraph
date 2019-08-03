using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// Enumerable helpers.
    /// </summary>
    internal static class EnumerableHelpers
    {
        /// <summary>
        /// Performs an <see cref="Action{T}"/> on each item in an enumerable,
        /// used to shortcut a "foreach" loop.
        /// </summary>
        /// <typeparam name="T">Enumerable element type.</typeparam>
        /// <param name="enumerable">Enumerable to enumerate over.</param>
        /// <param name="action">Acton to be performed on all elements.</param>
        public static void ForEach<T>(
            [NotNull, ItemCanBeNull] this IEnumerable<T> enumerable, 
            [NotNull, InstantHandle] Action<T> action)
        {
            if (enumerable is null)
                throw new ArgumentNullException(nameof(enumerable));
            if (action is null)
                throw new ArgumentNullException(nameof(action));

            foreach (T elem in enumerable)
                action(elem);
        }
    }
}