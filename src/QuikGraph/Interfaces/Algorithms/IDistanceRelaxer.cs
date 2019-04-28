using System.Collections.Generic;
using JetBrains.Annotations;

namespace QuikGraph.Algorithms
{
    /// <summary>
    /// Represents a distance relaxer.
    /// </summary>
    public interface IDistanceRelaxer : IComparer<double>
    {
        /// <summary>
        /// Initial distance.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        double InitialDistance { get; }

        /// <summary>
        /// Combines the distance and the weight in a single value.
        /// </summary>
        /// <param name="distance">Distance value.</param>
        /// <param name="weight">Weight value.</param>
        /// <returns>The combined value.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [Pure]
        double Combine(double distance, double weight);
    }
}
