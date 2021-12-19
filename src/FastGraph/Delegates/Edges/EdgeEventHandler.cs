#nullable enable

namespace FastGraph
{
    /// <summary>
    /// Delegate for an handler dealing with a edge.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    /// <param name="sender">Event sender.</param>
    /// <param name="args">Event arguments.</param>
    public delegate void EdgeEventHandler<TVertex, TEdge>(object sender, EdgeEventArgs<TVertex, TEdge> args)
        where TVertex : notnull
        where TEdge : IEdge<TVertex>;
}
