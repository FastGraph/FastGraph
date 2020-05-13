using JetBrains.Annotations;

namespace QuikGraph.Graphviz
{
    /// <summary>
    /// Delegate for a vertex formatting event.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <param name="sender">Event sender.</param>
    /// <param name="args">Event arguments.</param>
    public delegate void FormatVertexEventHandler<TVertex>(
        [NotNull] object sender,
        [NotNull] FormatVertexEventArgs<TVertex> args);
}