using JetBrains.Annotations;

namespace FastGraph
{
    /// <summary>
    /// Delegate to create a vertex.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <returns>The created vertex.</returns>
    [NotNull]
    public delegate TVertex VertexFactory<out TVertex>();
}
