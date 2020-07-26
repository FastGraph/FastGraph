using JetBrains.Annotations;

namespace QuikGraph.Collections
{
    /// <summary>
    /// Represents a queue (First in, First out).
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    public interface IQueue<T>
    {
        /// <summary>
        /// Number of elements.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Checks if this queue contains the given <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>True if the <paramref name="value"/> is contained in the queue, false otherwise.</returns>
        [Pure]
        bool Contains(T value);

        /// <summary>
        /// Enqueues an element in the queue.
        /// </summary>
        /// <param name="value">Value to add.</param>
        void Enqueue(T value);

        /// <summary>
        /// Dequeues an element from the queue.
        /// </summary>
        /// <returns>Removed element.</returns>
        T Dequeue();

        /// <summary>
        /// Returns the element at the beginning of the queue.
        /// </summary>
        /// <returns>The top queue element.</returns>
        [Pure]
        T Peek();

        /// <summary>
        /// Converts this queue to an array.
        /// </summary>
        /// <returns>Array composed of elements.</returns>
        [Pure]
        [NotNull]
        T[] ToArray();
    }
}