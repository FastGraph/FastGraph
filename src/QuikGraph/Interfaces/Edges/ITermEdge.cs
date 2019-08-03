#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
using QuikGraph.Contracts;
#endif

namespace QuikGraph
{
    /// <summary>
    /// Represents a directed edge with terminal indexes.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
#if SUPPORTS_CONTRACTS
    [ContractClass(typeof(TermEdgeContract<>))]
#endif
    public interface ITermEdge<out TVertex> : IEdge<TVertex>
    {
        /// <summary>
        /// Index of terminal on source vertex to which this edge is attached.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        int SourceTerminal { get; }

        /// <summary>
        /// Index of terminal on target vertex to which this edge is attached.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        int TargetTerminal { get; }
    }
}
