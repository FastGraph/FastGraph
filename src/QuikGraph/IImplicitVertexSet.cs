#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
using QuikGraph.Contracts;

#endif

namespace QuikGraph
{
    /// <summary>
    /// An implicit set of vertices
    /// </summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
#if SUPPORTS_CONTRACTS
    [ContractClass(typeof(IImplicitVertexSetContract<>))]
#endif
    public interface IImplicitVertexSet<TVertex>
    {
        /// <summary>
        /// Determines whether the specified vertex contains vertex.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>
        /// 	<c>true</c> if the specified vertex contains vertex; otherwise, <c>false</c>.
        /// </returns>
#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        bool ContainsVertex(TVertex vertex);
    }
}
