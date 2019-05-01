#if SUPPORTS_CONTRACTS
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace QuikGraph.Contracts
{
    /// <summary>
    /// Contract class for <see cref="IMutableVertexSet{TVertex}"/>.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    [ContractClassFor(typeof(IMutableVertexSet<>))]
    internal abstract class MutableVertexSetContract<TVertex> : IMutableVertexSet<TVertex>
    {
        #region IMutableVertexSet<TVertex>

        event VertexAction<TVertex> IMutableVertexSet<TVertex>.VertexAdded
        {
            add => throw new NotImplementedException();
            remove => throw new NotImplementedException();
        }

        bool IMutableVertexSet<TVertex>.AddVertex(TVertex vertex)
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IMutableVertexSet<TVertex> explicitThis = this;

            Contract.Requires(vertex != null);
            Contract.Ensures(
                Contract.Result<bool>() == Contract.OldValue(!explicitThis.ContainsVertex(vertex)));
            Contract.Ensures(explicitThis.ContainsVertex(vertex));
            Contract.Ensures(
                explicitThis.VertexCount == Contract.OldValue(explicitThis.VertexCount) + (Contract.Result<bool>() ? 1 : 0));

            return default(bool);
        }

        int IMutableVertexSet<TVertex>.AddVertexRange(IEnumerable<TVertex> vertices)
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IMutableVertexSet<TVertex> explicitThis = this;

            Contract.Requires(vertices != null);
            Contract.Requires(vertices.All(vertex => vertex != null));
            Contract.Ensures(vertices.All(vertex => explicitThis.ContainsVertex(vertex)));
            Contract.Ensures(
                explicitThis.VertexCount == Contract.OldValue(explicitThis.VertexCount) + Contract.Result<int>());

            return default(int);
        }

        event VertexAction<TVertex> IMutableVertexSet<TVertex>.VertexRemoved
        {
            add => throw new NotImplementedException();
            remove => throw new NotImplementedException();
        }

        bool IMutableVertexSet<TVertex>.RemoveVertex(TVertex vertex)
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IMutableVertexSet<TVertex> explicitThis = this;

            Contract.Requires(vertex != null);
            Contract.Ensures(Contract.Result<bool>() == Contract.OldValue(explicitThis.ContainsVertex(vertex)));
            Contract.Ensures(!explicitThis.ContainsVertex(vertex));
            Contract.Ensures(
                explicitThis.VertexCount == Contract.OldValue(explicitThis.VertexCount) - (Contract.Result<bool>() ? 1 : 0));

            return default(bool);
        }

        int IMutableVertexSet<TVertex>.RemoveVertexIf(VertexPredicate<TVertex> predicate)
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IMutableVertexSet<TVertex> explicitThis = this;

            Contract.Requires(predicate != null);
            Contract.Ensures(
                Contract.Result<int>() == Contract.OldValue(explicitThis.Vertices.Count(vertex => predicate(vertex))));
            Contract.Ensures(explicitThis.Vertices.All(vertex => !predicate(vertex)));
            Contract.Ensures(
                explicitThis.VertexCount == Contract.OldValue(explicitThis.VertexCount) - Contract.Result<int>());

            return default(int);
        }

        #endregion

        #region IVertexSet<TVertex>

        public bool IsVerticesEmpty => throw new NotImplementedException();

        public int VertexCount => throw new NotImplementedException();

        public IEnumerable<TVertex> Vertices => throw new NotImplementedException();

        #endregion

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