#nullable enable

namespace FastGraph
{
    /// <summary>
    /// Represents a directed edge.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    public interface IEdge<out TVertex>
        where TVertex : notnull
    {
        /// <summary>
        /// Gets the source vertex.
        /// </summary>
        TVertex Source { get; }

        /// <summary>
        /// Gets the target vertex.
        /// </summary>
        TVertex Target { get; }
    }
}
