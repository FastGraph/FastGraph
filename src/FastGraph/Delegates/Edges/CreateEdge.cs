#nullable enable

namespace FastGraph
{
    /// <summary>
    /// Delegate to create an edge in a graph between two vertices.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    /// <param name="graph">Graph in with adding the edge.</param>
    /// <param name="source">Edge source vertex.</param>
    /// <param name="target">Edge target vertex.</param>
    /// <returns>The created edge.</returns>
    public delegate TEdge CreateEdge<TVertex, TEdge>(
        IVertexListGraph<TVertex, TEdge> graph,
        TVertex source,
        TVertex target)
        where TVertex : notnull
        where TEdge : IEdge<TVertex>;
}
