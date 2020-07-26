namespace QuikGraph.Tests.Algorithms
{
    /// <summary>
    /// Represents a pair source and target vertices.
    /// </summary>
    internal readonly struct Vertices
    {
        public Vertices(int source, int target)
        {
            Source = source;
            Target = target;
        }

        /// <summary>
        /// Source vertex.
        /// </summary>
        public int Source { get; }

        /// <summary>
        /// Target vertex.
        /// </summary>
        public int Target { get; }
    }
}