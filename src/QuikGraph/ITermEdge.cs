#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
using QuikGraph.Contracts;

#endif

namespace QuikGraph
{
    /// <summary>
    /// A directed edge with terminal indices
    /// </summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
#if SUPPORTS_CONTRACTS
    [ContractClass(typeof(ITermEdgeContract<>))]
#endif
    public interface ITermEdge<TVertex> : IEdge<TVertex>
    {
        /// <summary>
        /// Index of terminal on source vertex to which this edge is attached.
        /// </summary>
        int SourceTerminal { get; }

        /// <summary>
        /// Index of terminal on target vertex to which this edge is attached.
        /// </summary>
        int TargetTerminal { get; }
    }
}
