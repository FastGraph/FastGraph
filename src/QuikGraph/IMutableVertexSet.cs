using System.Collections.Generic;
using QuikGraph.Contracts;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;

#endif

namespace QuikGraph
{
    /// <summary>
    /// A mutable vertex set
    /// </summary>
    /// <typeparam name="TVertex"></typeparam>
#if SUPPORTS_CONTRACTS
    [ContractClass(typeof(IMutableVertexSetContract<>))]
#endif
    public interface IMutableVertexSet<TVertex>
        : IVertexSet<TVertex>
    {
        event VertexAction<TVertex> VertexAdded;
        bool AddVertex(TVertex v);
        int AddVertexRange(IEnumerable<TVertex> vertices);

        event VertexAction<TVertex> VertexRemoved;
        bool RemoveVertex(TVertex v);
        int RemoveVertexIf(VertexPredicate<TVertex> pred);
    }
}
