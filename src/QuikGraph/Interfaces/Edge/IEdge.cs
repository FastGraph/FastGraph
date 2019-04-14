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
    [ContractClass(typeof(IEdgeContract<>))]
#endif
    public interface IEdge<out TVertex>
    {
        /// <summary>
        /// Gets the source vertex.
        /// </summary>
        [NotNull]
        TVertex Source { get; }

        /// <summary>
        /// Gets the target vertex.
        /// </summary>
        [NotNull]
        TVertex Target { get; }
    }
}
