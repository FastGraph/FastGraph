using JetBrains.Annotations;

namespace QuikGraph.Algorithms
{
    /// <summary>
    /// Represents an algorithm to run on a graph.
    /// </summary>
    /// <typeparam name="TGraph">Graph type.</typeparam>
    public interface IAlgorithm<out TGraph> : IComputation
    {
        /// <summary>
        /// Gets the graph to visit with this algorithm.
        /// </summary>
        [NotNull]
        TGraph VisitedGraph { get; }
    }
}