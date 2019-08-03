#if SUPPORTS_CONTRACTS
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace QuikGraph.Contracts
{
    /// <summary>
    /// Contract class for <see cref="IBidirectionalGraph{TVertex, TEdge}"/>.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    [ContractClassFor(typeof(IBidirectionalGraph<,>))]
    internal abstract class BidirectionalGraphContract<TVertex, TEdge> : IBidirectionalGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region IGraph<TVertex,TEdge>

        bool IGraph<TVertex, TEdge>.IsDirected => throw new NotImplementedException();

        bool IGraph<TVertex, TEdge>.AllowParallelEdges => throw new NotImplementedException();

        #endregion

        #region IVertexSet<TVertex>

        bool IVertexSet<TVertex>.IsVerticesEmpty => throw new NotImplementedException();

        int IVertexSet<TVertex>.VertexCount => throw new NotImplementedException();

        IEnumerable<TVertex> IVertexSet<TVertex>.Vertices => throw new NotImplementedException();

        #endregion

        #region IEdgeSet<TVertex,TEdge>

        bool IEdgeSet<TVertex, TEdge>.IsEdgesEmpty => throw new NotImplementedException();

        int IEdgeSet<TVertex, TEdge>.EdgeCount => throw new NotImplementedException();

        IEnumerable<TEdge> IEdgeSet<TVertex, TEdge>.Edges => throw new NotImplementedException();

        bool IEdgeSet<TVertex, TEdge>.ContainsEdge(TEdge edge)
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

        bool IImplicitGraph<TVertex, TEdge>.TryGetOutEdges(TVertex vertex, out IEnumerable<TEdge> edges)
        {
            throw new NotImplementedException();
        }

        TEdge IImplicitGraph<TVertex, TEdge>.OutEdge(TVertex vertex, int index)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IImplicitVertexSet<TVertex>

        bool IImplicitVertexSet<TVertex>.ContainsVertex(TVertex vertex)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IBidirectionalImplicitGraph<TVertex,TEdge>

        bool IBidirectionalIncidenceGraph<TVertex, TEdge>.IsInEdgesEmpty(TVertex vertex)
        {
            throw new NotImplementedException();
        }

        int IBidirectionalIncidenceGraph<TVertex, TEdge>.InDegree(TVertex vertex)
        {
            throw new NotImplementedException();
        }

        IEnumerable<TEdge> IBidirectionalIncidenceGraph<TVertex, TEdge>.InEdges(TVertex vertex)
        {
            throw new NotImplementedException();
        }

        bool IBidirectionalIncidenceGraph<TVertex, TEdge>.TryGetInEdges(TVertex vertex, out IEnumerable<TEdge> edges)
        {
            throw new NotImplementedException();
        }

        TEdge IBidirectionalIncidenceGraph<TVertex, TEdge>.InEdge(TVertex vertex, int index)
        {
            throw new NotImplementedException();
        }

        int IBidirectionalIncidenceGraph<TVertex, TEdge>.Degree(TVertex vertex)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
#endif