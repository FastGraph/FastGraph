using System;
using System.Collections.Generic;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
using System.Linq;
#endif

namespace QuickGraph.Contracts
{
#if SUPPORTS_CONTRACTS
    [ContractClassFor(typeof(IBidirectionalIncidenceGraph<,>))]
#endif
    abstract class IBidirectionalIncidenceGraphContract<TVertex, TEdge>
        : IBidirectionalIncidenceGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region IBidirectionalImplicitGraph<TVertex,TEdge> Members

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        bool IBidirectionalIncidenceGraph<TVertex, TEdge>.IsInEdgesEmpty(TVertex v)
        {
            IBidirectionalIncidenceGraph<TVertex, TEdge> ithis = this;
#if SUPPORTS_CONTRACTS
            Contract.Requires(v != null);
            Contract.Requires(ithis.ContainsVertex(v));
            Contract.Ensures(Contract.Result<bool>() == (ithis.InDegree(v) == 0));
#endif

            return default(bool);
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        int IBidirectionalIncidenceGraph<TVertex, TEdge>.InDegree(TVertex v)
        {
            IBidirectionalIncidenceGraph<TVertex, TEdge> ithis = this;
#if SUPPORTS_CONTRACTS
            Contract.Requires(v != null);
            Contract.Requires(ithis.ContainsVertex(v));
            Contract.Ensures(Contract.Result<int>() == Enumerable.Count(ithis.InEdges(v)));
#endif

            return default(int);
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        IEnumerable<TEdge> IBidirectionalIncidenceGraph<TVertex, TEdge>.InEdges(TVertex v)
        {
            IBidirectionalIncidenceGraph<TVertex, TEdge> ithis = this;
#if SUPPORTS_CONTRACTS
            Contract.Requires(v != null);
            Contract.Requires(ithis.ContainsVertex(v));
            Contract.Ensures(Contract.Result<IEnumerable<TEdge>>() != null);
            Contract.Ensures(Enumerable.All(
                Contract.Result<IEnumerable<TEdge>>(), 
                edge => edge != null && edge.Target.Equals(v)
                )
            );
#endif

            return default(IEnumerable<TEdge>);
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        bool IBidirectionalIncidenceGraph<TVertex, TEdge>.TryGetInEdges(TVertex v, out IEnumerable<TEdge> edges)
        {
            IBidirectionalIncidenceGraph<TVertex, TEdge> ithis = this;
#if SUPPORTS_CONTRACTS
            Contract.Requires(v != null);
            Contract.Requires(ithis.ContainsVertex(v));
            Contract.Ensures(Contract.Result<bool>() == ithis.ContainsVertex(v));
            Contract.Ensures(!Contract.Result<bool>() || Contract.ValueAtReturn(out edges) != null);
            Contract.Ensures(!Contract.Result<bool>() || 
                Enumerable.All(
                Contract.ValueAtReturn<IEnumerable<TEdge>>(out edges),
                edge => edge != null && edge.Target.Equals(v)
                )
            );
#endif

            edges = null;
            return default(bool);
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        TEdge IBidirectionalIncidenceGraph<TVertex, TEdge>.InEdge(TVertex v, int index)
        {
            IBidirectionalIncidenceGraph<TVertex, TEdge> ithis = this;
#if SUPPORTS_CONTRACTS
            Contract.Requires(v != null);
            Contract.Requires(ithis.ContainsVertex(v));
            Contract.Ensures(Contract.Result<TEdge>().Equals(Enumerable.ElementAt(ithis.InEdges(v), index)));
#endif

            return default(TEdge);
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        int IBidirectionalIncidenceGraph<TVertex, TEdge>.Degree(TVertex v)
        {
            IBidirectionalIncidenceGraph<TVertex, TEdge> ithis = this;
#if SUPPORTS_CONTRACTS
            Contract.Requires(v != null);
            Contract.Requires(ithis.ContainsVertex(v));
            Contract.Ensures(Contract.Result<int>() == ithis.InDegree(v) + ithis.OutDegree(v));
#endif

            return default(int);
        }

#endregion

#region IImplicitGraph<TVertex,TEdge> Members

        bool IImplicitGraph<TVertex, TEdge>.IsOutEdgesEmpty(TVertex v)
        {
            throw new NotImplementedException();
        }

        int IImplicitGraph<TVertex, TEdge>.OutDegree(TVertex v)
        {
            throw new NotImplementedException();
        }

        IEnumerable<TEdge> IImplicitGraph<TVertex, TEdge>.OutEdges(TVertex v)
        {
            throw new NotImplementedException();
        }

        bool IImplicitGraph<TVertex, TEdge>.TryGetOutEdges(TVertex v, out IEnumerable<TEdge> edges)
        {
            throw new NotImplementedException();
        }

        TEdge IImplicitGraph<TVertex, TEdge>.OutEdge(TVertex v, int index)
        {
            throw new NotImplementedException();
        }

#endregion

#region IGraph<TVertex,TEdge> Members

        bool IGraph<TVertex, TEdge>.IsDirected
        {
            get { throw new NotImplementedException(); }
        }

        bool IGraph<TVertex, TEdge>.AllowParallelEdges
        {
            get { throw new NotImplementedException(); }
        }

#endregion

#region IImplicitVertexSet<TVertex> Members

        bool IImplicitVertexSet<TVertex>.ContainsVertex(TVertex vertex)
        {
            throw new NotImplementedException();
        }

#endregion

#region IIncidenceGraph<TVertex,TEdge> Members

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
