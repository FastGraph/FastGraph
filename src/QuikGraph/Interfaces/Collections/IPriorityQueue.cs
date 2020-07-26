namespace QuikGraph.Collections
{
    /// <summary>
    /// Represents a queue with priority.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    public interface IPriorityQueue<T> : IQueue<T>
    {
        /// <summary>
        /// Updates the given <paramref name="value"/> priority.
        /// </summary>
        /// <param name="value">The value.</param>
        void Update(T value);
    }
}