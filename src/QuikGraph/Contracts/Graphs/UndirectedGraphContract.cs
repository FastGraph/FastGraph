#if SUPPORTS_CONTRACTS
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace QuikGraph.Contracts
{
    /// <summary>
    /// Contract class for <see cref="IUndirectedGraph{TVertex, TEdge}"/>.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    [ContractClassFor(typeof(IUndirectedGraph<,>))]
    internal abstract class UndirectedGraphContract<TVertex, TEdge> : IUndirectedGraph<TVertex, TEdge>
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

        bool IImplicitVertexSet<TVertex>.ContainsVertex(TVertex vertex) => throw new NotImplementedException();

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

        #region IImplicitUndirectedGraph<TVertex,TEdge>

        [Pure]
        EdgeEqualityComparer<TVertex> IImplicitUndirectedGraph<TVertex, TEdge>.EdgeEqualityComparer => 
            throw new NotImplementedException();

        IEnumerable<TEdge> IImplicitUndirectedGraph<TVertex, TEdge>.AdjacentEdges(TVertex vertex)
        {
            throw new NotImplementedException();
        }

        int IImplicitUndirectedGraph<TVertex, TEdge>.AdjacentDegree(TVertex vertex)
        {
            throw new NotImplementedException();
        }

        bool IImplicitUndirectedGraph<TVertex, TEdge>.IsAdjacentEdgesEmpty(TVertex vertex)
        {
            throw new NotImplementedException();
        }

        TEdge IImplicitUndirectedGraph<TVertex, TEdge>.AdjacentEdge(TVertex vertex, int index)
        {
            throw new NotImplementedException();
        }

        bool IImplicitUndirectedGraph<TVertex, TEdge>.ContainsEdge(TVertex source, TVertex target)
        {
            throw new NotImplementedException();
        }

        bool IImplicitUndirectedGraph<TVertex, TEdge>.TryGetEdge(TVertex source, TVertex target, out TEdge edge)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
#endif