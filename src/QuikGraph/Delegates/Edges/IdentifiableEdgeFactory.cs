using JetBrains.Annotations;

namespace QuikGraph
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
    [NotNull]
    public delegate TEdge IdentifiableEdgeFactory<in TVertex, out TEdge>([NotNull] TVertex source, [NotNull] TVertex target, [NotNull] string id)
        where TEdge : IEdge<TVertex>;
}