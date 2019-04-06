using System;
using System.Collections.Generic;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using System.Linq;

namespace QuickGraph.Contracts
{
#if SUPPORTS_CONTRACTS
    [ContractClassFor(typeof(IImplicitUndirectedGraph<,>))]
#endif
    abstract class IImplicitUndirectedGraphContract<TVertex, TEdge> 
        : IImplicitUndirectedGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
#region IImplicitUndirectedGraph<TVertex,TEdge> Members

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        EdgeEqualityComparer<TVertex, TEdge> IImplicitUndirectedGraph<TVertex, TEdge>.EdgeEqualityComparer
        {
            get
            {
#if SUPPORTS_CONTRACTS
                Contract.Ensures(Contract.Result<EdgeEqualityComparer<TVertex, TEdge>>() != null);
#endif

                return null;
            }
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        IEnumerable<TEdge> IImplicitUndirectedGraph<TVertex, TEdge>.AdjacentEdges(TVertex v)
        {
            IImplicitUndirectedGraph<TVertex, TEdge> ithis = this;
#if SUPPORTS_CONTRACTS
            Contract.Requires(v != null);
            Contract.Requires(ithis.ContainsVertex(v));
            Contract.Ensures(Contract.Result<IEnumerable<TEdge>>() != null);
            Contract.Ensures(
                Enumerable.All(
                    Contract.Result<IEnumerable<TEdge>>(),
                    edge => 
                        edge != null && 
                        ithis.ContainsEdge(edge.Source, edge.Target) && 
                        (edge.Source.Equals(v) || edge.Target.Equals(v))
                    )
                );
#endif

            return default(IEnumerable<TEdge>);
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        int IImplicitUndirectedGraph<TVertex, TEdge>.AdjacentDegree(TVertex v)
        {
            IImplicitUndirectedGraph<TVertex, TEdge> ithis = this;
#if SUPPORTS_CONTRACTS
            Contract.Requires(v != null);
            Contract.Requires(ithis.ContainsVertex(v));
            Contract.Ensures(Contract.Result<int>() == ithis.AdjacentDegree(v));
#endif

            return default(int);
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        bool IImplicitUndirectedGraph<TVertex, TEdge>.IsAdjacentEdgesEmpty(TVertex v)
        {
            IImplicitUndirectedGraph<TVertex, TEdge> ithis = this;
#if SUPPORTS_CONTRACTS
            Contract.Requires(v != null);
            Contract.Requires(ithis.ContainsVertex(v));
            Contract.Ensures(Contract.Result<bool>() == (ithis.AdjacentDegree(v) == 0));
#endif

            return default(bool);
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        TEdge IImplicitUndirectedGraph<TVertex, TEdge>.AdjacentEdge(TVertex v, int index)
        {
            IImplicitUndirectedGraph<TVertex, TEdge> ithis = this;
#if SUPPORTS_CONTRACTS
            Contract.Requires(v != null);
            Contract.Requires(ithis.ContainsVertex(v));
            Contract.Ensures(Contract.Result<TEdge>() != null);
            Contract.Ensures(
                Contract.Result<TEdge>().Source.Equals(v)
                || Contract.Result<TEdge>().Target.Equals(v));
#endif

            return default(TEdge);
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        bool IImplicitUndirectedGraph<TVertex, TEdge>.TryGetEdge(TVertex source, TVertex target, out TEdge edge)
        {
            IImplicitUndirectedGraph<TVertex, TEdge> ithis = this;
#if SUPPORTS_CONTRACTS
            Contract.Requires(source != null);
            Contract.Requires(target != null);
#endif

            edge = default(TEdge);
            return default(bool);
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        bool IImplicitUndirectedGraph<TVertex, TEdge>.ContainsEdge(TVertex source, TVertex target)
        {
            IImplicitUndirectedGraph<TVertex, TEdge> ithis = this;
#if SUPPORTS_CONTRACTS
            Contract.Requires(source != null);
            Contract.Requires(target != null);
            Contract.Ensures(Contract.Result<bool>() == Enumerable.Any(ithis.AdjacentEdges(source), e => e.Target.Equals(target) || e.Source.Equals(target)));
#endif

            return default(bool);
        }
#endregion

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
