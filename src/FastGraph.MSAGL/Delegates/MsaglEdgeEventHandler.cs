#nullable enable

namespace FastGraph.MSAGL
{
    /// <summary>
    /// Delegate for an handler dealing with a edge.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    /// <param name="sender">Event sender.</param>
    /// <param name="args">Event arguments.</param>
    public delegate void MsaglEdgeEventHandler<TVertex, TEdge>(
        object sender,
        MsaglEdgeEventArgs<TVertex, TEdge> args)
        where TVertex : notnull
        where TEdge : IEdge<TVertex>;
}
