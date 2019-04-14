#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using QuikGraph.Contracts;

namespace QuikGraph
{
    /// <summary>
    /// An undirected edge. 
    /// </summary>
    /// <remarks>
    /// Invariant: source must be less or equal to target (using the default comparer)
    /// </remarks>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
#if SUPPORTS_CONTRACTS
    [ContractClass(typeof(IUndirectedEdgeContract<>))]
#endif  
    public interface IUndirectedEdge<TVertex>
        : IEdge<TVertex>
    {
    }
}
