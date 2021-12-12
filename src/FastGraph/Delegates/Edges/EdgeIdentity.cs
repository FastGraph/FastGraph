#nullable enable

namespace FastGraph
{
    /// <summary>
    /// Delegate to compute the identity of the given <paramref name="edge"/>.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    /// <param name="edge">Edge to compute identity.</param>
    /// <returns>The <paramref name="edge"/> identity.</returns>
    public delegate string EdgeIdentity<TVertex, in TEdge>(TEdge edge)
        where TVertex : notnull
        where TEdge : IEdge<TVertex>;
}
