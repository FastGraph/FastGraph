#nullable enable

namespace FastGraph
{
    /// <summary>
    /// Delegate to perform an action involving the <paramref name="edge"/>.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    /// <param name="edge">Edge to treat.</param>
    public delegate void EdgeAction<TVertex, in TEdge>(TEdge edge)
        where TVertex : notnull
        where TEdge : IEdge<TVertex>;
}
