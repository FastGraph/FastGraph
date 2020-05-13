using JetBrains.Annotations;

namespace QuikGraph.Graphviz
{
    /// <summary>
    /// Delegate for an edge formatting event.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    /// <param name="sender">Event sender.</param>
    /// <param name="args">Event arguments.</param>
    public delegate void FormatEdgeAction<TVertex, TEdge>(
        [NotNull] object sender,
        [NotNull] FormatEdgeEventArgs<TVertex, TEdge> args)
        where TEdge : IEdge<TVertex>;
}