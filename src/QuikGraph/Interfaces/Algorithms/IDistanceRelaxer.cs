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
        double InitialDistance { get; }

        /// <summary>
        /// Combines the distance and the weight in a single value.
        /// </summary>
        /// <param name="distance">Distance value.</param>
        /// <param name="weight">Weight value.</param>
        /// <returns>The combined value.</returns>
        [Pure]
        double Combine(double distance, double weight);
    }
}