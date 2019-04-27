using JetBrains.Annotations;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
using QuikGraph.Contracts;
#endif

namespace QuikGraph
{
    /// <summary>
    /// Represents a directed edge.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
#if SUPPORTS_CONTRACTS
    [ContractClass(typeof(EdgeContract<>))]
#endif
    public interface IEdge<out TVertex>
    {
        /// <summary>
        /// Gets the source vertex.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        TVertex Source { get; }

        /// <summary>
        /// Gets the target vertex.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        TVertex Target { get; }
    }
}
