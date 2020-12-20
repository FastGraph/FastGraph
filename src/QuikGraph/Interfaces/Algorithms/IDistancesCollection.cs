using System.Collections.Generic;
using JetBrains.Annotations;

namespace QuikGraph.Algorithms
{
    /// <summary>
    /// Represents an object that stores information about distances between vertices.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    public interface IDistancesCollection<TVertex>
    {
        /// <summary>
        /// Tries to get the distance associated to the given <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="distance">Associated distance.</param>
        /// <returns>True if the distance was found, false otherwise.</returns>
        [Pure]
        bool TryGetDistance([NotNull] TVertex vertex, out double distance);

        /// <summary>
        /// Gets the distance associated to the given <paramref name="vertex"/>.
        /// Will throw a <see cref="KeyNotFoundException"/> if the vertex does not exist in the collection.
        /// </summary>
        /// <param name="vertex">The vertex to get the distance for.</param>
        /// <returns>The distance associated with the vertex.</returns>
        [Pure]
        double GetDistance([NotNull] TVertex vertex);

        /// <summary>
        /// Gets the distances for all vertices currently known.
        /// </summary>
        /// <returns>The <see cref="KeyValuePair{Vertex,Distance}"/> for the known vertices.</returns>
        [Pure]
        [NotNull]
        IEnumerable<KeyValuePair<TVertex, double>> GetDistances();
    }
}