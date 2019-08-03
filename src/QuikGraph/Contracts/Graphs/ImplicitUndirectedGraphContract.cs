#if SUPPORTS_CONTRACTS
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace QuikGraph.Contracts
{
    /// <summary>
    /// Contract class for <see cref="IImplicitUndirectedGraph{TVertex, TEdge}"/>.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    [ContractClassFor(typeof(IImplicitUndirectedGraph<,>))]
    internal abstract class ImplicitUndirectedGraphContract<TVertex, TEdge> : IImplicitUndirectedGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region IImplicitUndirectedGraph<TVertex,TEdge>

        [Pure]
        EdgeEqualityComparer<TVertex> IImplicitUndirectedGraph<TVertex, TEdge>.EdgeEqualityComparer
        {
            get
            {
                Contract.Ensures(Contract.Result<EdgeEqualityComparer<TVertex>>() != null);

                // ReSharper disable once AssignNullToNotNullAttribute, Justification: Contract class.
                return null;
            }
        }

        [Pure]
        IEnumerable<TEdge> IImplicitUndirectedGraph<TVertex, TEdge>.AdjacentEdges(TVertex vertex)
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IImplicitUndirectedGraph<TVertex, TEdge> explicitThis = this;

            Contract.Requires(vertex != null);
            Contract.Requires(explicitThis.ContainsVertex(vertex));
            Contract.Ensures(Contract.Result<IEnumerable<TEdge>>() != null);
            Contract.Ensures(
                Contract.Result<IEnumerable<TEdge>>().All(
                    edge => edge != null
                            && explicitThis.ContainsEdge(edge.Source, edge.Target)
                            && (edge.Source.Equals(vertex) || edge.Target.Equals(vertex))));

            return Enumerable.Empty<TEdge>();
        }

        [Pure]
        int IImplicitUndirectedGraph<TVertex, TEdge>.AdjacentDegree(TVertex vertex)
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IImplicitUndirectedGraph<TVertex, TEdge> explicitThis = this;

            Contract.Requires(vertex != null);
            Contract.Requires(explicitThis.ContainsVertex(vertex));
            Contract.Ensures(Contract.Result<int>() == explicitThis.AdjacentDegree(vertex));

            return default(int);
        }

        [Pure]
        bool IImplicitUndirectedGraph<TVertex, TEdge>.IsAdjacentEdgesEmpty(TVertex vertex)
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IImplicitUndirectedGraph<TVertex, TEdge> explicitThis = this;

            Contract.Requires(vertex != null);
            Contract.Requires(explicitThis.ContainsVertex(vertex));
            Contract.Ensures(Contract.Result<bool>() == (explicitThis.AdjacentDegree(vertex) == 0));

            return default(bool);
        }

        [Pure]
        TEdge IImplicitUndirectedGraph<TVertex, TEdge>.AdjacentEdge(TVertex vertex, int index)
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IImplicitUndirectedGraph<TVertex, TEdge> explicitThis = this;

            Contract.Requires(vertex != null);
            Contract.Requires(explicitThis.ContainsVertex(vertex));
            Contract.Ensures(Contract.Result<TEdge>() != null);
            Contract.Ensures(
                Contract.Result<TEdge>().Source.Equals(vertex)
                || Contract.Result<TEdge>().Target.Equals(vertex));

            // ReSharper disable once AssignNullToNotNullAttribute, Justification: Contract class.
            return default(TEdge);
        }

        [Pure]
        bool IImplicitUndirectedGraph<TVertex, TEdge>.TryGetEdge(TVertex source, TVertex target, out TEdge edge)
        {
            Contract.Requires(source != null);
            Contract.Requires(target != null);

            edge = default(TEdge);
            return default(bool);
        }

        [Pure]
        bool IImplicitUndirectedGraph<TVertex, TEdge>.ContainsEdge(TVertex source, TVertex target)
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IImplicitUndirectedGraph<TVertex, TEdge> explicitThis = this;

            Contract.Requires(source != null);
            Contract.Requires(target != null);
            Contract.Ensures(
                Contract.Result<bool>() == explicitThis.AdjacentEdges(source).Any(e => e.Target.Equals(target) || e.Source.Equals(target)));

            return default(bool);
        }
        #endregion

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