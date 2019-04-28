using JetBrains.Annotations;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
using QuikGraph.Algorithms.Contracts;
#endif

namespace QuikGraph.Algorithms
{
    /// <summary>
    /// Represents an algorithm to run on a graph.
    /// </summary>
    /// <typeparam name="TGraph">Graph type.</typeparam>
#if SUPPORTS_CONTRACTS
    [ContractClass(typeof(AlgorithmContract<>))]
#endif
    public interface IAlgorithm<out TGraph> : IComputation
    {
        /// <summary>
        /// Gets the graph to visit with this algorithm.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        TGraph VisitedGraph { get; }
    }
}
