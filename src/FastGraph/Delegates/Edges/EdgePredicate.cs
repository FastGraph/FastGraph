#nullable enable

namespace FastGraph
{
    /// <summary>
    /// Delegate to perform a check on the given <paramref name="edge"/>.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    /// <param name="edge">Edge to check condition.</param>
    /// <returns>True if the <paramref name="edge"/> matches the predicate, false otherwise.</returns>
    public delegate bool EdgePredicate<TVertex, in TEdge>(TEdge edge)
        where TVertex : notnull
        where TEdge : IEdge<TVertex>;
}
