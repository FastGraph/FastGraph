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
    /// Contract class for <see cref="IMutableEdgeListGraph{TVertex, TEdge}"/>.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    [ContractClassFor(typeof(IMutableEdgeListGraph<,>))]
    internal abstract class MutableEdgeListGraphContract<TVertex, TEdge> : IMutableEdgeListGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region IMutableEdgeListGraph<TVertex,TEdge>

        bool IMutableEdgeListGraph<TVertex, TEdge>.AddEdge(TEdge edge)
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IMutableEdgeListGraph<TVertex, TEdge> explicitThis = this;

            Contract.Requires(edge != null);
            Contract.Requires(explicitThis.ContainsVertex(edge.Source));
            Contract.Requires(explicitThis.ContainsVertex(edge.Target));
            Contract.Ensures(explicitThis.ContainsEdge(edge));
            Contract.Ensures(
                explicitThis.AllowParallelEdges || Contract.Result<bool>() == Contract.OldValue(!explicitThis.ContainsEdge(edge)));
            Contract.Ensures(
                explicitThis.EdgeCount == Contract.OldValue(explicitThis.EdgeCount) + (Contract.Result<bool>() ? 1 : 0));

            return default(bool);
        }

        event EdgeAction<TVertex, TEdge> IMutableEdgeListGraph<TVertex, TEdge>.EdgeAdded
        {
            add => throw new NotImplementedException();
            remove => throw new NotImplementedException();
        }

        int IMutableEdgeListGraph<TVertex, TEdge>.AddEdgeRange(IEnumerable<TEdge> edges)
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IMutableEdgeListGraph<TVertex, TEdge> explicitThis = this;

            Contract.Requires(edges != null);
            Contract.Requires(
#if SUPPORTS_TYPE_FULL_FEATURES
                typeof(TEdge).IsValueType
#else
                typeof(TEdge).GetTypeInfo().IsValueType
#endif
                || edges.All(edge => edge != null));
            Contract.Requires(
                edges.All(edge => explicitThis.ContainsVertex(edge.Source) && explicitThis.ContainsVertex(edge.Target)));
            Contract.Ensures(
                edges.All(edge => explicitThis.ContainsEdge(edge)), "All edge from edges belong to the graph.");
            Contract.Ensures(
                Contract.Result<int>() == Contract.OldValue(edges.Count(edge => !explicitThis.ContainsEdge(edge))));
            Contract.Ensures(
                explicitThis.EdgeCount == Contract.OldValue(explicitThis.EdgeCount) + Contract.Result<int>());

            return default(int);
        }

        bool IMutableEdgeListGraph<TVertex, TEdge>.RemoveEdge(TEdge edge)
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IMutableEdgeListGraph<TVertex, TEdge> explicitThis = this;

            Contract.Requires(edge != null);
            Contract.Ensures(
                Contract.Result<bool>() == Contract.OldValue(explicitThis.ContainsEdge(edge)));
            Contract.Ensures(!explicitThis.ContainsEdge(edge));
            Contract.Ensures(
                explicitThis.EdgeCount == Contract.OldValue(explicitThis.EdgeCount) - (Contract.Result<bool>() ? 1 : 0));

            return default(bool);
        }

        event EdgeAction<TVertex, TEdge> IMutableEdgeListGraph<TVertex, TEdge>.EdgeRemoved
        {
            add => throw new NotImplementedException();
            remove => throw new NotImplementedException();
        }

        int IMutableEdgeListGraph<TVertex, TEdge>.RemoveEdgeIf(EdgePredicate<TVertex, TEdge> predicate)
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IMutableEdgeListGraph<TVertex, TEdge> explicitThis = this;

            Contract.Requires(predicate != null);
            Contract.Ensures(
                Contract.Result<int>() == Contract.OldValue(explicitThis.Edges.Count(e => predicate(e))));
            Contract.Ensures(explicitThis.Edges.All(e => !predicate(e)));
            Contract.Ensures(
                explicitThis.EdgeCount == Contract.OldValue(explicitThis.EdgeCount) - Contract.Result<int>());

            return default(int);
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
    }
}
#endif