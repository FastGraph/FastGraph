using JetBrains.Annotations;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
using QuikGraph.Contracts;
#endif

namespace QuikGraph
{
    /// <summary>
    /// Represents an implicit set of vertices.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
#if SUPPORTS_CONTRACTS
    [ContractClass(typeof(ImplicitVertexSetContract<>))]
#endif
    public interface IImplicitVertexSet<in TVertex>
    {
        /// <summary>
        /// Determines whether this set contains the specified <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">Vertex to check.</param>
        /// <returns>True if the specified <paramref name="vertex"/> is contained in this set, false otherwise.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        bool ContainsVertex([NotNull] TVertex vertex);
    }
}
