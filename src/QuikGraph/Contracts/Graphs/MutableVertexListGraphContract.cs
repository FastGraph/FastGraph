#if SUPPORTS_CONTRACTS
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace QuikGraph.Contracts
{
    /// <summary>
    /// Contract class for <see cref="IMutableVertexListGraph{TVertex, TEdge}"/>.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    [ContractClassFor(typeof(IMutableVertexListGraph<,>))]
    internal abstract class MutableVertexListGraphContract<TVertex, TEdge> : IMutableVertexListGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region IMutableIncidenceGraph<TVertex,TEdge>

        int IMutableIncidenceGraph<TVertex, TEdge>.RemoveOutEdgeIf(TVertex vertex, EdgePredicate<TVertex, TEdge> predicate)
        {
            throw new NotImplementedException();
        }

        void IMutableIncidenceGraph<TVertex, TEdge>.ClearOutEdges(TVertex vertex)
        {
            throw new NotImplementedException();
        }

        void IMutableIncidenceGraph<TVertex, TEdge>.TrimEdgeExcess()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IGraph<TVertex,TEdge>

        bool IGraph<TVertex, TEdge>.IsDirected => throw new NotImplementedException();

        bool IGraph<TVertex, TEdge>.AllowParallelEdges => throw new NotImplementedException();

        #endregion

        #region IVertexSet<TVertex>

        bool IVertexSet<TVertex>.IsVerticesEmpty => throw new NotImplementedException();

        int IVertexSet<TVertex>.VertexCount => throw new NotImplementedException();

        IEnumerable<TVertex> IVertexSet<TVertex>.Vertices => throw new NotImplementedException();

        bool IImplicitVertexSet<TVertex>.ContainsVertex(TVertex vertex)
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

        bool IImplicitGraph<TVertex, TEdge>.TryGetOutEdges(TVertex v, out IEnumerable<TEdge> edges)
        {
            throw new NotImplementedException();
        }

        TEdge IImplicitGraph<TVertex, TEdge>.OutEdge(TVertex vertex, int index)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IMutableGraph<TVertex,TEdge>

        void IMutableGraph<TVertex, TEdge>.Clear()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IMutableVertexSet<TVertex>

        event VertexAction<TVertex> IMutableVertexSet<TVertex>.VertexAdded
        {
            add => throw new NotImplementedException();
            remove => throw new NotImplementedException();
        }

        bool IMutableVertexSet<TVertex>.AddVertex(TVertex vertex)
        {
            throw new NotImplementedException();
        }

        int IMutableVertexSet<TVertex>.AddVertexRange(IEnumerable<TVertex> vertices)
        {
            throw new NotImplementedException();
        }

        event VertexAction<TVertex> IMutableVertexSet<TVertex>.VertexRemoved
        {
            add => throw new NotImplementedException();
            remove => throw new NotImplementedException();
        }

        bool IMutableVertexSet<TVertex>.RemoveVertex(TVertex vertex)
        {
            throw new NotImplementedException();
        }

        int IMutableVertexSet<TVertex>.RemoveVertexIf(VertexPredicate<TVertex> predicate)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
#endif