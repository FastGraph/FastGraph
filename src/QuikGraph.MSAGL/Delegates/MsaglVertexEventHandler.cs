using JetBrains.Annotations;

namespace QuikGraph.MSAGL
{
    /// <summary>
    /// Delegate to for an handler dealing with a MSAGL vertex.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <param name="sender">Event sender.</param>
    /// <param name="args">Event arguments.</param>
    public delegate void MsaglVertexNodeEventHandler<TVertex>(
        [NotNull] object sender,
        [NotNull] MsaglVertexEventArgs<TVertex> args);
}