using JetBrains.Annotations;

namespace QuikGraph.Algorithms
{
    /// <summary>
    /// Represents an algorithm that puts colors on vertices and allow to get that color.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    public interface IVertexColorizerAlgorithm<in TVertex>
    {
        /// <summary>
        /// Gets the <see cref="GraphColor"/> associated to the given <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>The vertex <see cref="GraphColor"/>.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [Pure]
        GraphColor GetVertexColor([NotNull] TVertex vertex);
    }
}
