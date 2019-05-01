#if SUPPORTS_CONTRACTS
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace QuikGraph.Contracts
{
    /// <summary>
    /// Contract class for <see cref="IBidirectionalIncidenceGraph{TVertex, TEdge}"/>.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    [ContractClassFor(typeof(IBidirectionalIncidenceGraph<,>))]
    internal abstract class BidirectionalIncidenceGraphContract<TVertex, TEdge> : IBidirectionalIncidenceGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region IBidirectionalImplicitGraph<TVertex,TEdge>

        [Pure]
        bool IBidirectionalIncidenceGraph<TVertex, TEdge>.IsInEdgesEmpty(TVertex vertex)
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IBidirectionalIncidenceGraph<TVertex, TEdge> explicitThis = this;

            Contract.Requires(vertex != null);
            Contract.Requires(explicitThis.ContainsVertex(vertex));
            Contract.Ensures(Contract.Result<bool>() == (explicitThis.InDegree(vertex) == 0));

            return default(bool);
        }

        [Pure]
        int IBidirectionalIncidenceGraph<TVertex, TEdge>.InDegree(TVertex vertex)
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IBidirectionalIncidenceGraph<TVertex, TEdge> explicitThis = this;

            Contract.Requires(vertex != null);
            Contract.Requires(explicitThis.ContainsVertex(vertex));
            Contract.Ensures(Contract.Result<int>() == explicitThis.InEdges(vertex).Count());

            return default(int);
        }

        [Pure]
        IEnumerable<TEdge> IBidirectionalIncidenceGraph<TVertex, TEdge>.InEdges(TVertex vertex)
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IBidirectionalIncidenceGraph<TVertex, TEdge> explicitThis = this;

            Contract.Requires(vertex != null);
            Contract.Requires(explicitThis.ContainsVertex(vertex));
            Contract.Ensures(Contract.Result<IEnumerable<TEdge>>() != null);
            Contract.Ensures(
                Contract.Result<IEnumerable<TEdge>>().All(edge => edge != null && edge.Target.Equals(vertex)));

            return Enumerable.Empty<TEdge>();
        }

        [Pure]
        bool IBidirectionalIncidenceGraph<TVertex, TEdge>.TryGetInEdges(TVertex vertex, out IEnumerable<TEdge> edges)
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IBidirectionalIncidenceGraph<TVertex, TEdge> explicitThis = this;

            Contract.Requires(vertex != null);
            Contract.Requires(explicitThis.ContainsVertex(vertex));
            Contract.Ensures(Contract.Result<bool>() == explicitThis.ContainsVertex(vertex));
            Contract.Ensures(!Contract.Result<bool>() || Contract.ValueAtReturn(out edges) != null);
            Contract.Ensures(
                !Contract.Result<bool>() 
                || Contract.ValueAtReturn(out edges).All(edge => edge != null && edge.Target.Equals(vertex)));

            edges = null;
            return default(bool);
        }

        [Pure]
        TEdge IBidirectionalIncidenceGraph<TVertex, TEdge>.InEdge(TVertex vertex, int index)
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IBidirectionalIncidenceGraph<TVertex, TEdge> explicitThis = this;

            Contract.Requires(vertex != null);
            Contract.Requires(explicitThis.ContainsVertex(vertex));
            Contract.Ensures(Contract.Result<TEdge>().Equals(explicitThis.InEdges(vertex).ElementAt(index)));

            // ReSharper disable once AssignNullToNotNullAttribute, Justification: Contract class.
            return default(TEdge);
        }

        [Pure]
        int IBidirectionalIncidenceGraph<TVertex, TEdge>.Degree(TVertex vertex)
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IBidirectionalIncidenceGraph<TVertex, TEdge> explicitThis = this;

            Contract.Requires(vertex != null);
            Contract.Requires(explicitThis.ContainsVertex(vertex));
            Contract.Ensures(
                Contract.Result<int>() == explicitThis.InDegree(vertex) + explicitThis.OutDegree(vertex));

            return default(int);
        }

        #endregion

        #region IGraph<TVertex,TEdge>

        bool IGraph<TVertex, TEdge>.IsDirected => throw new NotImplementedException();

        bool IGraph<TVertex, TEdge>.AllowParallelEdges => throw new NotImplementedException();

        #endregion

        #region IImplicitVertexSet<TVertex>

        bool IImplicitVertexSet<TVertex>.ContainsVertex(TVertex vertex)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IImplicitGraph<TVertex,TEdge>

        bool IImplicitGraph<TVertex, TEdge>.IsOutEdgesEmpty(TVertex vertex)
        {
            throw new NotImplementedException();
        }

        int IImplicitGraph<TVertex, TEdge>.OutDegree(TVertex vertex)
        {
            throw new NotImplementedException();
        }

        IEnumerable<TEdge> IImplicitGraph<TVertex, TEdge>.OutEdges(TVertex vertex)
        {
            throw new NotImplementedException();
        }

        bool IImplicitGraph<TVertex, TEdge>.TryGetOutEdges(TVertex vertex, out IEnumerable<TEdge> edges)
        {
            throw new NotImplementedException();
        }

        TEdge IImplicitGraph<TVertex, TEdge>.OutEdge(TVertex vertex, int index)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IIncidenceGraph<TVertex,TEdge>

        bool IIncidenceGraph<TVertex, TEdge>.ContainsEdge(TVertex source, TVertex target)
        {
            throw new NotImplementedException();
        }

        bool IIncidenceGraph<TVertex, TEdge>.TryGetEdges(TVertex source, TVertex target, out IEnumerable<TEdge> edges)
        {
            throw new NotImplementedException();
        }

        bool IIncidenceGraph<TVertex, TEdge>.TryGetEdge(TVertex source, TVertex target, out TEdge edge)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
#endif