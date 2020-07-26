using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace QuikGraph.Predicates
{
    /// <summary>
    /// Predicate that tests if an edge is residual.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public sealed class ResidualEdgePredicate<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResidualEdgePredicate{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="residualCapacities">Residual capacities per edge.</param>
        public ResidualEdgePredicate([NotNull] IDictionary<TEdge, double> residualCapacities)
        {
            ResidualCapacities = residualCapacities ?? throw new ArgumentNullException(nameof(residualCapacities));
        }

        /// <summary>
        /// Residual capacities map.
        /// </summary>
        [NotNull]
        public IDictionary<TEdge, double> ResidualCapacities { get; }

        /// <summary>
        /// Checks if the given <paramref name="edge"/> is residual.
        /// </summary>
        /// <remarks>Check if the implemented predicate is matched.</remarks>
        /// <param name="edge">Edge to use in predicate.</param>
        /// <returns>True if the edge is residual, false otherwise.</returns>
        [Pure]
        public bool Test([NotNull] TEdge edge)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            return 0 < ResidualCapacities[edge];
        }
    }
}