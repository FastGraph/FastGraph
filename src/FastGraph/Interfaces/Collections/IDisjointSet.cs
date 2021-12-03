﻿using JetBrains.Annotations;

namespace FastGraph.Collections
{
    /// <summary>
    /// A disjoint-set data structure.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    public interface IDisjointSet<T>
    {
        /// <summary>
        /// Gets the current number of sets.
        /// </summary>
        int SetCount { get; }

        /// <summary>
        /// Gets the current number of elements.
        /// </summary>
        int ElementCount { get; }

        /// <summary>
        /// Creates a new set for the <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        void MakeSet([NotNull] T value);

        /// <summary>
        /// Finds the set containing the <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Value to search.</param>
        /// <returns>Root value of the set.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        [Pure]
        [NotNull]
        T FindSet([NotNull] T value);

        /// <summary>
        /// Gets a value indicating if left and right are contained in the same set.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>True if both values are in the same set, false otherwise.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        [Pure]
        bool AreInSameSet([NotNull] T left, [NotNull] T right);

        /// <summary>
        /// Merges the sets from the two values.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>True if <paramref name="left"/> and <paramref name="right"/> were unioned,
        /// false if they already belong to the same set.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="left"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="right"/> is <see langword="null"/>.</exception>
        bool Union([NotNull] T left, [NotNull] T right);

        /// <summary>
        /// Gets a value indicating whether the value is in the data structure.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>True if the value is already in the set, false otherwise.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        [Pure]
        bool Contains([NotNull] T value);
    }
}
