using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// Delegate to create a vertex.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <returns>The created vertex.</returns>
#if SUPPORTS_CONTRACTS
    [System.Diagnostics.Contracts.Pure]
#endif
    [NotNull]
    public delegate TVertex VertexFactory<out TVertex>();
}
