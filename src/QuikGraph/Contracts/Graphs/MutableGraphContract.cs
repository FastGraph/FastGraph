#if SUPPORTS_CONTRACTS
using System;
using System.Diagnostics.Contracts;

namespace QuikGraph.Contracts
{
    /// <summary>
    /// Contract class for <see cref="IMutableGraph{TVertex, TEdge}"/>.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    [ContractClassFor(typeof(IMutableGraph<,>))]
    internal abstract class MutableGraphContract<TVertex, TEdge> : IMutableGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region IMutableGraph<TVertex,TEdge>

        void IMutableGraph<TVertex, TEdge>.Clear()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IGraph<TVertex,TEdge>

        bool IGraph<TVertex, TEdge>.IsDirected => throw new NotImplementedException();

        bool IGraph<TVertex, TEdge>.AllowParallelEdges => throw new NotImplementedException();

        #endregion
    }
}
#endif