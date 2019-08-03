using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// A delegate-based bidirectional implicit graph.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public class DelegateBidirectionalIncidenceGraph<TVertex, TEdge> : DelegateIncidenceGraph<TVertex, TEdge>, IBidirectionalIncidenceGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>, IEquatable<TEdge>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateBidirectionalIncidenceGraph{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="tryGetOutEdges">Getter of out-edges.</param>
        /// <param name="tryGetInEdges">Getter of in-edges.</param>
        public DelegateBidirectionalIncidenceGraph(
            [NotNull] TryFunc<TVertex, IEnumerable<TEdge>> tryGetOutEdges,
            [NotNull] TryFunc<TVertex, IEnumerable<TEdge>> tryGetInEdges)
            : base(tryGetOutEdges)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(tryGetInEdges != null);
#endif
            _tryGetInEdgesFunc = tryGetInEdges;
        }

        /// <summary>
        /// Getter of in-edges.
        /// </summary>
        [NotNull]
        private readonly TryFunc<TVertex, IEnumerable<TEdge>> _tryGetInEdgesFunc;

        #region IBidirectionalImplicitGraph<TVertex,TEdge>

        /// <inheritdoc />
        public bool IsInEdgesEmpty(TVertex vertex)
        {
            return !InEdges(vertex).Any();
        }

        /// <inheritdoc />
        public int InDegree(TVertex vertex)
        {
            return InEdges(vertex).Count();
        }

        /// <inheritdoc />
        public IEnumerable<TEdge> InEdges(TVertex vertex)
        {
            if (_tryGetInEdgesFunc(vertex, out IEnumerable<TEdge> result))
                return result;
            return Enumerable.Empty<TEdge>();
        }

        /// <inheritdoc />
        public bool TryGetInEdges(TVertex vertex, out IEnumerable<TEdge> edges)
        {
            return _tryGetInEdgesFunc(vertex, out edges);
        }

        /// <inheritdoc />
        public TEdge InEdge(TVertex vertex, int index)
        {
            return InEdges(vertex).ElementAt(index);
        }

        /// <inheritdoc />
        public int Degree(TVertex vertex)
        {
            return InDegree(vertex) + OutDegree(vertex);
        }

        #endregion
    }
}
