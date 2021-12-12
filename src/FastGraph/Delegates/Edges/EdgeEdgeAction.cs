#nullable enable

namespace FastGraph
{
    /// <summary>
    /// Delegate to perform an action involving the 2 edges.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    /// <param name="edge">First edge.</param>
    /// <param name="targetEdge">Second edge.</param>
    public delegate void EdgeEdgeAction<TVertex, in TEdge>(TEdge edge, TEdge targetEdge)
        where TVertex : notnull
        where TEdge : IEdge<TVertex>;
}
