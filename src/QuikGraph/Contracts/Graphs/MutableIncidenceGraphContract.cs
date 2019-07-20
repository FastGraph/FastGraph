#if SUPPORTS_CONTRACTS
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace QuikGraph.Contracts
{
    /// <summary>
    /// Contract class for <see cref="IMutableIncidenceGraph{TVertex, TEdge}"/>.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    [ContractClassFor(typeof(IMutableIncidenceGraph<,>))]
    internal abstract class MutableIncidenceGraphContract<TVertex, TEdge> : IMutableIncidenceGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region IMutableIncidenceGraph<TVertex,TEdge>

        int IMutableIncidenceGraph<TVertex, TEdge>.RemoveOutEdgeIf(TVertex vertex, EdgePredicate<TVertex, TEdge> predicate)
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IMutableIncidenceGraph<TVertex, TEdge> explicitThis = this;

            Contract.Requires(vertex != null);
            Contract.Requires(explicitThis.ContainsVertex(vertex));
            Contract.Requires(predicate != null);
            Contract.Ensures(
                Contract.Result<int>() == Contract.OldValue(explicitThis.OutEdges(vertex).Count(ve => predicate(ve))));
            Contract.Ensures(explicitThis.OutEdges(vertex).All(ve => !predicate(ve)));

            return default(int);
        }

        void IMutableIncidenceGraph<TVertex, TEdge>.ClearOutEdges(TVertex vertex)
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IMutableIncidenceGraph<TVertex, TEdge> explicitThis = this;

            Contract.Requires(vertex != null);
            Contract.Requires(explicitThis.ContainsVertex(vertex));
            Contract.Ensures(explicitThis.OutDegree(vertex) == 0);
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

        #region IImplicitVertexSet<TVertex>

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

        bool IImplicitGraph<TVertex, TEdge>.TryGetOutEdges(TVertex vertex, out IEnumerable<TEdge> edges)
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
    }
}
#endif