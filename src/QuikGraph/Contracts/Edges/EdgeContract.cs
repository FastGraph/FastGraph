#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;

namespace QuikGraph.Contracts
{
    /// <summary>
    /// Contract class for <see cref="IEdge{TVertex}"/>.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    [ContractClassFor(typeof(IEdge<>))]
    internal abstract class EdgeContract<TVertex> : IEdge<TVertex>
    {
        [ContractInvariantMethod]
        private void EdgeInvariant()
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IEdge<TVertex> explicitThis = this;

            Contract.Invariant(explicitThis.Source != null);
            Contract.Invariant(explicitThis.Target != null);
        }

        TVertex IEdge<TVertex>.Source
        {
            get
            {
                Contract.Ensures(Contract.Result<TVertex>() != null);

                // ReSharper disable once AssignNullToNotNullAttribute, Justification: Contract class.
                return default(TVertex);
            }
        }

        TVertex IEdge<TVertex>.Target
        {
            get
            {
                Contract.Ensures(Contract.Result<TVertex>() != null);

                // ReSharper disable once AssignNullToNotNullAttribute, Justification: Contract class.
                return default(TVertex);
            }
        }
    }
}
#endif