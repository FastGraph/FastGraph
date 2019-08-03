#if SUPPORTS_CONTRACTS
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace QuikGraph.Contracts
{
    /// <summary>
    /// Contract class for <see cref="IEdgeListGraph{TVertex, TEdge}"/>.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    [ContractClassFor(typeof(IEdgeListGraph<,>))]
    internal abstract class EdgeListGraphContract<TVertex, TEdge> : IEdgeListGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region IGraph<TVertex,TEdge>

        bool IGraph<TVertex, TEdge>.IsDirected => throw new NotImplementedException();

        bool IGraph<TVertex, TEdge>.AllowParallelEdges => throw new NotImplementedException();

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

        #region IVertexSet<TVertex>

        public bool IsVerticesEmpty => throw new NotImplementedException();

        public int VertexCount => throw new NotImplementedException();

        public IEnumerable<TVertex> Vertices => throw new NotImplementedException();

        #endregion

        #region IImplicitVertexSet<TVertex>

        public bool ContainsVertex(TVertex vertex)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
#endif