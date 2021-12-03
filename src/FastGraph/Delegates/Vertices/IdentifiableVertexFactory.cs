using JetBrains.Annotations;

namespace FastGraph
{
    /// <summary>
    /// Delegate to create an identifiable vertex.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <param name="id">Vertex id.</param>
    /// <returns>The created vertex.</returns>
    [NotNull]
    public delegate TVertex IdentifiableVertexFactory<out TVertex>([NotNull] string id);
}
