using System;
using System.Collections.Generic;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using System.Linq;

namespace QuickGraph.Contracts
{
#if SUPPORTS_CONTRACTS
    [ContractClassFor(typeof(IImplicitGraph<,>))]
#endif
    abstract class IImplicitGraphContract<TVertex, TEdge> 
        : IImplicitGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        bool IImplicitGraph<TVertex, TEdge>.IsOutEdgesEmpty(TVertex v)
        {
            IImplicitGraph<TVertex, TEdge> ithis = this;
#if SUPPORTS_CONTRACTS
            Contract.Requires(v != null);
            Contract.Requires(ithis.ContainsVertex(v));
            Contract.Ensures(Contract.Result<bool>() == (ithis.OutDegree(v) == 0));
#endif

            return default(bool);
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        int IImplicitGraph<TVertex, TEdge>.OutDegree(TVertex v)
        {
            IImplicitGraph<TVertex, TEdge> ithis = this;
#if SUPPORTS_CONTRACTS
            Contract.Requires(v != null);
            Contract.Requires(ithis.ContainsVertex(v));
            Contract.Ensures(Contract.Result<int>() == Enumerable.Count<TEdge>(ithis.OutEdges(v)));
#endif

            return default(int);
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        IEnumerable<TEdge> IImplicitGraph<TVertex, TEdge>.OutEdges(TVertex v)
        {
            IImplicitGraph<TVertex, TEdge> ithis = this;
#if SUPPORTS_CONTRACTS
            Contract.Requires(v != null);
            Contract.Requires(ithis.ContainsVertex(v));
            Contract.Ensures(Contract.Result<IEnumerable<TEdge>>() != null);
            Contract.Ensures(Enumerable.All(Contract.Result<IEnumerable<TEdge>>(), e => e.Source.Equals(v)));
#endif

            return default(IEnumerable<TEdge>);
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        bool IImplicitGraph<TVertex, TEdge>.TryGetOutEdges(TVertex v, out IEnumerable<TEdge> edges)
        {
            IImplicitGraph<TVertex, TEdge> ithis = this;
#if SUPPORTS_CONTRACTS
            Contract.Requires(v != null);
            Contract.Ensures(!Contract.Result<bool>() || 
                (Contract.ValueAtReturn(out edges) != null && 
                 Enumerable.All(Contract.ValueAtReturn(out edges), e => e.Source.Equals(v)))
                );
#endif

            edges = null;
            return default(bool);
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        TEdge IImplicitGraph<TVertex, TEdge>.OutEdge(TVertex v, int index)
        {
            IImplicitGraph<TVertex, TEdge> ithis = this;
#if SUPPORTS_CONTRACTS
            Contract.Requires(v != null);
            Contract.Requires(ithis.ContainsVertex(v));
            Contract.Ensures(Enumerable.Any(ithis.OutEdges(v), e => e.Equals(Contract.Result<TEdge>())));
#endif

            return default(TEdge);
        }

#region IImplicitVertexSet<TVertex> Members
        bool IImplicitVertexSet<TVertex>.ContainsVertex(TVertex vertex)
        {
            throw new NotImplementedException();
        }
#endregion

#region IGraph<TVertex,TEdge> Members

        public bool IsDirected {
          get { throw new NotImplementedException(); }
        }

        public bool AllowParallelEdges {
          get { throw new NotImplementedException(); }
        }

#endregion
    }
}
