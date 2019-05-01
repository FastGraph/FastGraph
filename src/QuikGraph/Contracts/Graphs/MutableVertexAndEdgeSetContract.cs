#if SUPPORTS_CONTRACTS
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
#if !SUPPORTS_TYPE_FULL_FEATURES
using System.Reflection;
#endif

namespace QuikGraph.Contracts
{
    /// <summary>
    /// Contract class for <see cref="IMutableVertexAndEdgeSet{TVertex, TEdge}"/>.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    [ContractClassFor(typeof(IMutableVertexAndEdgeSet<,>))]
    internal abstract class MutableVertexAndEdgeSetContract<TVertex, TEdge> : IMutableVertexAndEdgeSet<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        bool IMutableVertexAndEdgeSet<TVertex, TEdge>.AddVerticesAndEdge(TEdge edge)
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IMutableVertexAndEdgeSet<TVertex, TEdge> explicitThis = this;

            Contract.Requires(edge != null);
            Contract.Ensures(explicitThis.ContainsEdge(edge));
            Contract.Ensures(
                explicitThis.AllowParallelEdges 
                || Contract.Result<bool>() == Contract.OldValue(!explicitThis.ContainsEdge(edge)));
            Contract.Ensures(
                explicitThis.EdgeCount == Contract.OldValue(explicitThis.EdgeCount) + (Contract.Result<bool>() ? 1 : 0));

            return default(bool);
        }

        int IMutableVertexAndEdgeSet<TVertex, TEdge>.AddVerticesAndEdgeRange(IEnumerable<TEdge> edges)
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IMutableVertexAndEdgeSet<TVertex, TEdge> explicitThis = this;

            Contract.Requires(edges != null);
            Contract.Requires(
#if SUPPORTS_TYPE_FULL_FEATURES
                typeof(TEdge).IsValueType
#else
                typeof(TEdge).GetTypeInfo().IsValueType
#endif
                || edges.All(edge => edge != null));
            Contract.Ensures(
                explicitThis.EdgeCount == Contract.OldValue(explicitThis.EdgeCount) + Contract.Result<int>());

            return default(int);
        }

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

        #region IEdgeSet<TVertex,TEdge>

        bool IEdgeSet<TVertex, TEdge>.IsEdgesEmpty => throw new NotImplementedException();

        int IEdgeSet<TVertex, TEdge>.EdgeCount => throw new NotImplementedException();

        IEnumerable<TEdge> IEdgeSet<TVertex, TEdge>.Edges => throw new NotImplementedException();

        bool IEdgeSet<TVertex, TEdge>.ContainsEdge(TEdge edge)
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
    }
}
#endif