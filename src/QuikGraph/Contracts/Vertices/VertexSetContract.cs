#if SUPPORTS_CONTRACTS
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace QuikGraph.Contracts
{
    /// <summary>
    /// Contract class for <see cref="IVertexSet{TVertex}"/>.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    [ContractClassFor(typeof(IVertexSet<>))]
    internal abstract class VertexSetContract<TVertex> : IVertexSet<TVertex>
    {
        bool IVertexSet<TVertex>.IsVerticesEmpty
        {
            get
            {
                // ReSharper disable once RedundantAssignment, Justification: Code contract.
                IVertexSet<TVertex> explicitThis = this;
                Contract.Ensures(Contract.Result<bool>() == (explicitThis.VertexCount == 0));

                return default(bool);
            }
        }

        int IVertexSet<TVertex>.VertexCount
        {
            get
            {
                // ReSharper disable once RedundantAssignment, Justification: Code contract.
                IVertexSet<TVertex> explicitThis = this;
                Contract.Ensures(Contract.Result<int>() == explicitThis.Vertices.Count());

                return default(int);
            }
        }

        IEnumerable<TVertex> IVertexSet<TVertex>.Vertices
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<TVertex>>() != null);

                return Enumerable.Empty<TVertex>();
            }
        }

        #region IImplicitVertexSet<TVertex>

        [Pure]
        public bool ContainsVertex(TVertex vertex)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
#endif