#nullable enable

namespace FastGraph
{
    /// <summary>
    /// Delegate to create an edge.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    /// <param name="source">Edge source vertex.</param>
    /// <param name="target">Edge target vertex.</param>
    /// <returns>The created edge.</returns>
    public delegate TEdge EdgeFactory<in TVertex, out TEdge>(TVertex source, TVertex target)
        where TVertex : notnull
        where TEdge : IEdge<TVertex>;
}
