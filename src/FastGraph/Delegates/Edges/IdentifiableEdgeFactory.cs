#nullable enable

namespace FastGraph
{
    /// <summary>
    /// Delegate to create an identifiable edge.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    /// <param name="source">Edge source vertex.</param>
    /// <param name="target">Edge target vertex.</param>
    /// <param name="id">Edge id.</param>
    /// <returns>The created vertex.</returns>
    public delegate TEdge IdentifiableEdgeFactory<in TVertex, out TEdge>(TVertex source, TVertex target, string id)
        where TVertex : notnull
        where TEdge : IEdge<TVertex>;
}
