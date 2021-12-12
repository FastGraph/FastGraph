#nullable enable

using JetBrains.Annotations;

namespace FastGraph.Algorithms
{
    /// <summary>
    /// Represents an object that stores information about distances between vertices.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    public interface IDistancesCollection<TVertex>
        where TVertex : notnull
    {
        /// <summary>
        /// Tries to get the distance associated to the given <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="distance">Associated distance.</param>
        /// <returns>True if the distance was found, false otherwise.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="vertex"/> is <see langword="null"/>.</exception>
        [Pure]
        bool TryGetDistance(TVertex vertex, out double distance);

        /// <summary>
        /// Gets the distance associated to the given <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex to get the distance for.</param>
        /// <returns>The distance associated with the vertex.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="vertex"/> is <see langword="null"/>.</exception>
        /// <exception cref="VertexNotFoundException"><paramref name="vertex"/> has no recorded distance.</exception>
        [Pure]
        double GetDistance(TVertex vertex);

        /// <summary>
        /// Gets the distances for all vertices currently known.
        /// </summary>
        /// <returns>The <see cref="T:System.Collections.Generic.KeyValuePair{Vertex,Distance}"/> for the known vertices.</returns>
        [Pure]
        IEnumerable<KeyValuePair<TVertex, double>> GetDistances();
    }
}
