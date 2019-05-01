#if SUPPORTS_CONTRACTS
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace QuikGraph.Contracts
{
    /// <summary>
    /// Contract class for <see cref="IMutableUndirectedGraph{TVertex, TEdge}"/>.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    [ContractClassFor(typeof(IMutableUndirectedGraph<,>))]
    internal abstract class MutableUndirectedGraphContract<TVertex, TEdge> : IMutableUndirectedGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region IMutableUndirectedGraph<TVertex,TEdge>

        int IMutableUndirectedGraph<TVertex, TEdge>.RemoveAdjacentEdgeIf(TVertex vertex, EdgePredicate<TVertex, TEdge> predicate)
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IMutableUndirectedGraph<TVertex, TEdge> explicitThis = this;

            Contract.Requires(vertex != null);
            Contract.Requires(predicate != null);
            Contract.Ensures(
                Contract.Result<int>() == Contract.OldValue(explicitThis.AdjacentEdges(vertex).Count(e => predicate(e))));
            Contract.Ensures(explicitThis.AdjacentEdges(vertex).All(v => !predicate(v)));

            return default(int);
        }

        void IMutableUndirectedGraph<TVertex, TEdge>.ClearAdjacentEdges(TVertex vertex)
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IMutableUndirectedGraph<TVertex, TEdge> explicitThis = this;

            Contract.Requires(vertex != null);
            Contract.Ensures(explicitThis.AdjacentDegree(vertex) == 0);
        }

        #endregion

        #region IMutableEdgeListGraph<TVertex,TEdge>

        bool IMutableEdgeListGraph<TVertex, TEdge>.AddEdge(TEdge edge)
        {
            throw new NotImplementedException();
        }

        event EdgeAction<TVertex, TEdge> IMutableEdgeListGraph<TVertex, TEdge>.EdgeAdded
        {
            add => throw new NotImplementedException();
            remove => throw new NotImplementedException();
        }

        int IMutableEdgeListGraph<TVertex, TEdge>.AddEdgeRange(IEnumerable<TEdge> edges)
        {
            throw new NotImplementedException();
        }

        bool IMutableEdgeListGraph<TVertex, TEdge>.RemoveEdge(TEdge edge)
        {
            throw new NotImplementedException();
        }

        event EdgeAction<TVertex, TEdge> IMutableEdgeListGraph<TVertex, TEdge>.EdgeRemoved
        {
            add => throw new NotImplementedException();
            remove => throw new NotImplementedException();
        }

        int IMutableEdgeListGraph<TVertex, TEdge>.RemoveEdgeIf(EdgePredicate<TVertex, TEdge> predicate)
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

        #region IVertexSet<TVertex>

        bool IVertexSet<TVertex>.IsVerticesEmpty => throw new NotImplementedException();

        int IVertexSet<TVertex>.VertexCount => throw new NotImplementedException();

        IEnumerable<TVertex> IVertexSet<TVertex>.Vertices => throw new NotImplementedException();

        bool IImplicitVertexSet<TVertex>.ContainsVertex(TVertex vertex)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IImplicitUndirectedGraph<TVertex,TEdge>

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

        #region IMutableVertexAndEdgeSet<TVertex,TEdge>

        bool IMutableVertexAndEdgeSet<TVertex, TEdge>.AddVerticesAndEdge(TEdge edge)
        {
            throw new NotImplementedException();
        }

        int IMutableVertexAndEdgeSet<TVertex, TEdge>.AddVerticesAndEdgeRange(IEnumerable<TEdge> edges)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
#endif