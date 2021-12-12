#nullable enable

namespace FastGraph.Graphviz
{
    /// <summary>
    /// Delegate for a clustered graph formatting event.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    /// <param name="sender">Event sender.</param>
    /// <param name="args">Event arguments.</param>
    public delegate void FormatClusterEventHandler<TVertex, TEdge>(
        object sender,
        FormatClusterEventArgs<TVertex, TEdge> args)
        where TVertex : notnull
        where TEdge : IEdge<TVertex>;
}
