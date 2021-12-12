#nullable enable

namespace FastGraph
{
    /// <summary>
    /// Represents a set of vertices.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    public interface IVertexSet<TVertex> : IImplicitVertexSet<TVertex>
        where TVertex : notnull
    {
        /// <summary>
        /// Gets a value indicating whether there are no vertices in this set.
        /// It is true if this vertex set is empty, otherwise false.
        /// </summary>
        bool IsVerticesEmpty { get; }

        /// <summary>
        /// Gets the vertex count.
        /// </summary>
        int VertexCount { get; }

        /// <summary>
        /// Gets the vertices.
        /// </summary>
        IEnumerable<TVertex> Vertices { get; }
    }
}
