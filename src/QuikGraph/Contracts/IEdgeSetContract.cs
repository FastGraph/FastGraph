using System.Collections.Generic;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
using System.Linq;
#endif

namespace QuickGraph.Contracts
{
#if SUPPORTS_CONTRACTS
    [ContractClassFor(typeof(IEdgeSet<,>))]
#endif
    abstract class IEdgeSetContract<TVertex, TEdge> 
        : IEdgeSet<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        bool IEdgeSet<TVertex, TEdge>.IsEdgesEmpty
        {
            get 
            {
                IEdgeSet<TVertex, TEdge> ithis = this;
#if SUPPORTS_CONTRACTS
                Contract.Ensures(Contract.Result<bool>() == (ithis.EdgeCount == 0));
#endif

                return default(bool);
            }
        }

        int IEdgeSet<TVertex, TEdge>.EdgeCount
        {
            get
            {
                IEdgeSet<TVertex, TEdge> ithis = this;
#if SUPPORTS_CONTRACTS
                Contract.Ensures(Contract.Result<int>() == Enumerable.Count(ithis.Edges));
#endif

                return default(int);
            }
        }

        IEnumerable<TEdge> IEdgeSet<TVertex, TEdge>.Edges
        {
            get
            {
#if SUPPORTS_CONTRACTS
                Contract.Ensures(Contract.Result<IEnumerable<TEdge>>() != null);
                Contract.Ensures(Enumerable.All<TEdge>(Contract.Result<IEnumerable<TEdge>>(), e => e != null));
#endif

                return default(IEnumerable<TEdge>);            
            }
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        bool IEdgeSet<TVertex, TEdge>.ContainsEdge(TEdge edge)
        {
            IEdgeSet<TVertex, TEdge> ithis = this;
#if SUPPORTS_CONTRACTS
            Contract.Requires(edge != null);
            Contract.Ensures(Contract.Result<bool>() == Contract.Exists(ithis.Edges, e => e.Equals(edge)));
#endif

            return default(bool);
        }
    }
}
