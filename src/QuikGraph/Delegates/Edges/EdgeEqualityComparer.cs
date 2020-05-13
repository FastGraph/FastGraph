using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// Delegate to compare edge source and target vertex with given ones.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <param name="edge">Edge to compare.</param>
    /// <param name="source">Source vertex to compare with.</param>
    /// <param name="target">Target vertex to compare with.</param>
    /// <returns>True if the <paramref name="edge"/> vertices matches given ones, false otherwise.</returns>
    public delegate bool EdgeEqualityComparer<in TVertex>(
        [NotNull] IEdge<TVertex> edge,
        [NotNull] TVertex source,
        [NotNull] TVertex target);
}