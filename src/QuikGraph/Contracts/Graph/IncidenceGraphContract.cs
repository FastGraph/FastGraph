#if SUPPORTS_CONTRACTS
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace QuikGraph.Contracts
{
    /// <summary>
    /// Contract class for <see cref="IIncidenceGraph{TVertex, TEdge}"/>.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    [ContractClassFor(typeof(IIncidenceGraph<,>))]
    internal abstract class IncidenceGraphContract<TVertex, TEdge> : IIncidenceGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        bool IIncidenceGraph<TVertex, TEdge>.ContainsEdge(TVertex source, TVertex target)
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IIncidenceGraph<TVertex, TEdge> explicitThis = this;

            Contract.Requires(source != null);
            Contract.Requires(target != null);
            Contract.Requires(explicitThis.ContainsVertex(source));
            Contract.Requires(explicitThis.ContainsVertex(target));

            return default(bool);
        }

        bool IIncidenceGraph<TVertex, TEdge>.TryGetEdges(TVertex source, TVertex target, out IEnumerable<TEdge> edges)
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IIncidenceGraph<TVertex, TEdge> explicitThis = this;

            Contract.Requires(source != null);
            Contract.Requires(target != null);
            Contract.Requires(explicitThis.ContainsVertex(source));
            Contract.Requires(explicitThis.ContainsVertex(target));

            edges = null;
            return default(bool);
        }

        bool IIncidenceGraph<TVertex, TEdge>.TryGetEdge(TVertex source, TVertex target, out TEdge edge)
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IIncidenceGraph<TVertex, TEdge> explicitThis = this;

            Contract.Requires(source != null);
            Contract.Requires(target != null);
            Contract.Requires(explicitThis.ContainsVertex(source));
            Contract.Requires(explicitThis.ContainsVertex(target));

            edge = default(TEdge);
            return default(bool);
        }

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

        #region IGraph<TVertex,TEdge>

        public bool IsDirected => throw new NotImplementedException();

        public bool AllowParallelEdges => throw new NotImplementedException();

        #endregion

        #region IImplicitVertexSet<TVertex> Members

        public bool ContainsVertex(TVertex vertex)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
#endif