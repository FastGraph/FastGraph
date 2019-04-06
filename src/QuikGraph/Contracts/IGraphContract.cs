#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif

namespace QuickGraph.Contracts
{
#if SUPPORTS_CONTRACTS
    [ContractClassFor(typeof(IGraph<,>))]
#endif
    abstract class IGraphContract<TVertex, TEdge>
        : IGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        bool IGraph<TVertex, TEdge>.IsDirected
        {
            get { return default(bool); }
        }

        bool IGraph<TVertex, TEdge>.AllowParallelEdges
        {
            get { return default(bool); }
        }
    }
}
