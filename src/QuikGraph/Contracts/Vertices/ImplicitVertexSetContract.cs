#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;

namespace QuikGraph.Contracts
{
    /// <summary>
    /// Contract class for <see cref="IImplicitVertexSet{TVertex}"/>.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    [ContractClassFor(typeof(IImplicitVertexSet<>))]
    internal abstract class ImplicitVertexSetContract<TVertex> : IImplicitVertexSet<TVertex>
    {
        [Pure]
        bool IImplicitVertexSet<TVertex>.ContainsVertex(TVertex vertex)
        {
            Contract.Requires(vertex != null);

            return default(bool);
        }
    }
}
#endif