using JetBrains.Annotations;

namespace QuikGraph
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
    [NotNull]
    public delegate TEdge CreateEdge<TVertex, TEdge>(
        [NotNull] IVertexListGraph<TVertex, TEdge> graph,
        [NotNull] TVertex source,
        [NotNull] TVertex target)
        where TEdge : IEdge<TVertex>;
}