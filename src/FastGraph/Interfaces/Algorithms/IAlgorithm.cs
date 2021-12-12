#nullable enable

namespace FastGraph.Algorithms
{
    /// <summary>
    /// Represents an algorithm to run on a graph.
    /// </summary>
    /// <typeparam name="TGraph">Graph type.</typeparam>
    public interface IAlgorithm<out TGraph> : IComputation
        where TGraph : notnull
    {
        /// <summary>
        /// Gets the graph to visit with this algorithm.
        /// </summary>
        TGraph VisitedGraph { get; }
    }
}
