#nullable enable

namespace FastGraph
{
    /// <summary>
    /// Delegate to create a vertex in a graph.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    /// <param name="graph">Graph in with adding the vertex.</param>
    /// <returns>The created vertex.</returns>
    public delegate TVertex CreateVertexDelegate<TVertex, TEdge>(IVertexListGraph<TVertex, TEdge> graph)
        where TVertex : notnull
        where TEdge : IEdge<TVertex>;
}
