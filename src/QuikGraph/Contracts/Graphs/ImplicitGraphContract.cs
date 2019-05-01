#if SUPPORTS_CONTRACTS
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace QuikGraph.Contracts
{
    /// <summary>
    /// Contract class for <see cref="IImplicitGraph{TVertex, TEdge}"/>.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    [ContractClassFor(typeof(IImplicitGraph<,>))]
    internal abstract class ImplicitGraphContract<TVertex, TEdge> : IImplicitGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        [Pure]
        bool IImplicitGraph<TVertex, TEdge>.IsOutEdgesEmpty(TVertex vertex)
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IImplicitGraph<TVertex, TEdge> explicitThis = this;

            Contract.Requires(vertex != null);
            Contract.Requires(explicitThis.ContainsVertex(vertex));
            Contract.Ensures(Contract.Result<bool>() == (explicitThis.OutDegree(vertex) == 0));

            return default(bool);
        }

        [Pure]
        int IImplicitGraph<TVertex, TEdge>.OutDegree(TVertex vertex)
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IImplicitGraph<TVertex, TEdge> explicitThis = this;

            Contract.Requires(vertex != null);
            Contract.Requires(explicitThis.ContainsVertex(vertex));
            Contract.Ensures(Contract.Result<int>() == explicitThis.OutEdges(vertex).Count());

            return default(int);
        }

        [Pure]
        IEnumerable<TEdge> IImplicitGraph<TVertex, TEdge>.OutEdges(TVertex vertex)
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IImplicitGraph<TVertex, TEdge> explicitThis = this;

            Contract.Requires(vertex != null);
            Contract.Requires(explicitThis.ContainsVertex(vertex));
            Contract.Ensures(Contract.Result<IEnumerable<TEdge>>() != null);
            Contract.Ensures(Contract.Result<IEnumerable<TEdge>>().All(e => e.Source.Equals(vertex)));

            return Enumerable.Empty<TEdge>();
        }

        [Pure]
        bool IImplicitGraph<TVertex, TEdge>.TryGetOutEdges(TVertex vertex, out IEnumerable<TEdge> edges)
        {
            Contract.Requires(vertex != null);
            Contract.Ensures(
                !Contract.Result<bool>()
                ||
                (Contract.ValueAtReturn(out edges) != null && Contract.ValueAtReturn(out edges).All(e => e.Source.Equals(vertex))));

            edges = null;
            return default(bool);
        }

        [Pure]
        TEdge IImplicitGraph<TVertex, TEdge>.OutEdge(TVertex vertex, int index)
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IImplicitGraph<TVertex, TEdge> explicitThis = this;

            Contract.Requires(vertex != null);
            Contract.Requires(explicitThis.ContainsVertex(vertex));
            Contract.Ensures(explicitThis.OutEdges(vertex).Any(e => e.Equals(Contract.Result<TEdge>())));

            // ReSharper disable once AssignNullToNotNullAttribute, Justification: Contract class.
            return default(TEdge);
        }

        #region IGraph<TVertex,TEdge>

        public bool IsDirected => throw new NotImplementedException();

        public bool AllowParallelEdges => throw new NotImplementedException();

        #endregion

        #region IImplicitVertexSet<TVertex>

        bool IImplicitVertexSet<TVertex>.ContainsVertex(TVertex vertex)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
#endif