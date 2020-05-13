using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// Delegate to create an edge.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    /// <param name="source">Edge source vertex.</param>
    /// <param name="target">Edge target vertex.</param>
    /// <returns>The created edge.</returns>
    [NotNull]
    public delegate TEdge EdgeFactory<in TVertex, out TEdge>([NotNull] TVertex source, [NotNull] TVertex target)
        where TEdge : IEdge<TVertex>;
}