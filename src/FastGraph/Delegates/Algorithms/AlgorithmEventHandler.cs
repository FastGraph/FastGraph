#nullable enable

namespace FastGraph.Algorithms
{
    /// <summary>
    /// Delegate to react to an algorithm event.
    /// </summary>
    /// <param name="sender">Algorithm that sent the event.</param>
    /// <param name="args">Event arguments.</param>
    /// <typeparam name="TGraph"></typeparam>
    public delegate void AlgorithmEventHandler<in TGraph>(
        IAlgorithm<TGraph> sender,
        EventArgs args)
        where TGraph : notnull;
}
