#nullable enable

namespace FastGraph.Graphviz
{
    /// <summary>
    /// Delegate for an edge formatting event.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    /// <param name="sender">Event sender.</param>
    /// <param name="args">Event arguments.</param>
    public delegate void FormatEdgeAction<TVertex, TEdge>(
        object sender,
        FormatEdgeEventArgs<TVertex, TEdge> args)
        where TVertex : notnull
        where TEdge : IEdge<TVertex>;
}
