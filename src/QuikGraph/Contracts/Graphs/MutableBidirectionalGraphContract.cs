#if SUPPORTS_CONTRACTS
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace QuikGraph.Contracts
{
    /// <summary>
    /// Contract class for <see cref="IMutableBidirectionalGraph{TVertex, TEdge}"/>.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    [ContractClassFor(typeof(IMutableBidirectionalGraph<,>))]
    internal abstract class MutableBidirectionalGraphContract<TVertex, TEdge> : IMutableBidirectionalGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        int IMutableBidirectionalGraph<TVertex, TEdge>.RemoveInEdgeIf(
            TVertex vertex, 
            EdgePredicate<TVertex, TEdge> predicate)
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IMutableBidirectionalGraph<TVertex, TEdge> explicitThis = this;

            Contract.Requires(vertex != null);
            Contract.Requires(predicate != null);
            Contract.Requires(explicitThis.ContainsVertex(vertex));
            Contract.Ensures(explicitThis.ContainsVertex(vertex));
            Contract.Ensures(explicitThis.InEdges(vertex).All(e => predicate(e)));
            Contract.Ensures(
                Contract.Result<int>() == Contract.OldValue(explicitThis.InEdges(vertex).Count(e => predicate(e))));
            Contract.Ensures(
                explicitThis.InDegree(vertex) == Contract.OldValue(explicitThis.InDegree(vertex)) - Contract.Result<int>());

            return default(int);
        }

        void IMutableBidirectionalGraph<TVertex, TEdge>.ClearInEdges(TVertex vertex)
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IMutableBidirectionalGraph<TVertex, TEdge> explicitThis = this;

            Contract.Requires(vertex != null);
            Contract.Requires(explicitThis.ContainsVertex(vertex));
            Contract.Ensures(
                explicitThis.EdgeCount == Contract.OldValue(explicitThis.EdgeCount) - Contract.OldValue(explicitThis.InDegree(vertex)));
            Contract.Ensures(explicitThis.InDegree(vertex) == 0);
        }

        void IMutableBidirectionalGraph<TVertex, TEdge>.ClearEdges(TVertex vertex)
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IMutableBidirectionalGraph<TVertex, TEdge> explicitThis = this;

            Contract.Requires(vertex != null);
            Contract.Requires(explicitThis.ContainsVertex(vertex));
            Contract.Ensures(!explicitThis.ContainsVertex(vertex));
        }

        #region IMutableVertexAndEdgeListGraph<TVertex,TEdge>

        bool IMutableVertexAndEdgeSet<TVertex, TEdge>.AddVerticesAndEdge(TEdge edge)
        {
            throw new NotImplementedException();
        }

        int IMutableVertexAndEdgeSet<TVertex, TEdge>.AddVerticesAndEdgeRange(IEnumerable<TEdge> edges)
        {
            throw new NotImplementedException();
        }

        #endregion

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

        #region IGraph<TVertex,TEdge>

        public bool IsDirected => throw new NotImplementedException();

        public bool AllowParallelEdges => throw new NotImplementedException();

        #endregion

        #region IIncidenceGraph<TVertex,TEdge>

        public bool ContainsEdge(TVertex source, TVertex target)
        {
            throw new NotImplementedException();
        }

        public bool TryGetEdges(TVertex source, TVertex target, out IEnumerable<TEdge> edges)
        {
            throw new NotImplementedException();
        }

        public bool TryGetEdge(TVertex source, TVertex target, out TEdge edge)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IImplicitGraph<TVertex,TEdge>

        public bool IsOutEdgesEmpty(TVertex vertex)
        {
            throw new NotImplementedException();
        }

        public int OutDegree(TVertex vertex)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEdge> OutEdges(TVertex vertex)
        {
            throw new NotImplementedException();
        }

        public bool TryGetOutEdges(TVertex vertex, out IEnumerable<TEdge> edges)
        {
            throw new NotImplementedException();
        }

        public TEdge OutEdge(TVertex vertex, int index)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IImplicitVertexSet<TVertex>

        public bool ContainsVertex(TVertex vertex)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IVertexSet<TVertex>

        public bool IsVerticesEmpty => throw new NotImplementedException();

        public int VertexCount => throw new NotImplementedException();

        public IEnumerable<TVertex> Vertices => throw new NotImplementedException();

        #endregion

        #region IEdgeSet<TVertex,TEdge>

        public bool IsEdgesEmpty => throw new NotImplementedException();

        public int EdgeCount => throw new NotImplementedException();

        public IEnumerable<TEdge> Edges => throw new NotImplementedException();

        public bool ContainsEdge(TEdge edge)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IBidirectionalIncidenceGraph<TVertex,TEdge>

        public bool IsInEdgesEmpty(TVertex vertex)
        {
            throw new NotImplementedException();
        }

        public int InDegree(TVertex vertex)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEdge> InEdges(TVertex vertex)
        {
            throw new NotImplementedException();
        }

        public bool TryGetInEdges(TVertex vertex, out IEnumerable<TEdge> edges)
        {
            throw new NotImplementedException();
        }

        public TEdge InEdge(TVertex vertex, int index)
        {
            throw new NotImplementedException();
        }

        public int Degree(TVertex vertex)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
#endif