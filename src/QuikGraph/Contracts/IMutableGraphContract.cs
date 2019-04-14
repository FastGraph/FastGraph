using System;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif

namespace QuikGraph.Contracts
{
#if SUPPORTS_CONTRACTS
    [ContractClassFor(typeof(IMutableGraph<,>))]
#endif
    abstract class IMutableGraphContract<TVertex, TEdge>
        : IMutableGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
#region IMutableGraph<TVertex,TEdge> Members
        void IMutableGraph<TVertex, TEdge>.Clear()
        {
            IMutableGraph<TVertex, TEdge> ithis = this;
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
    }
}
