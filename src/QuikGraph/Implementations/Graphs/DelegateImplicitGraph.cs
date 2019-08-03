using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// A delegate-based implicit graph.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public class DelegateImplicitGraph<TVertex, TEdge> : IImplicitGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>, IEquatable<TEdge>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateImplicitGraph{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="tryGetOutEdges">Getter of out-edges.</param>
        public DelegateImplicitGraph([NotNull] TryFunc<TVertex, IEnumerable<TEdge>> tryGetOutEdges)
        {
            if (tryGetOutEdges is null)
                throw new ArgumentNullException(nameof(tryGetOutEdges));

            _tryGetOutEdgesFunc = tryGetOutEdges;
        }

        /// <summary>
        /// Getter of out-edges.
        /// </summary>
        [NotNull]
        private readonly TryFunc<TVertex, IEnumerable<TEdge>> _tryGetOutEdgesFunc;

        #region IGraph<TVertex,TEdge>

        /// <inheritdoc />
        public bool IsDirected => true;

        /// <inheritdoc />
        public bool AllowParallelEdges => true;

        #endregion

        #region IImplicitGraph<TVertex,TEdge>

        /// <inheritdoc />
        public bool IsOutEdgesEmpty(TVertex vertex)
        {
            return !OutEdges(vertex).Any();
        }

        /// <inheritdoc />
        public int OutDegree(TVertex vertex)
        {
            return OutEdges(vertex).Count();
        }

        /// <inheritdoc />
        public IEnumerable<TEdge> OutEdges(TVertex vertex)
        {
            if (_tryGetOutEdgesFunc(vertex, out IEnumerable<TEdge> result))
                return result;
            return Enumerable.Empty<TEdge>();
        }

        /// <inheritdoc />
        public bool TryGetOutEdges(TVertex vertex, out IEnumerable<TEdge> edges)
        {
            return _tryGetOutEdgesFunc(vertex, out edges);
        }

        /// <inheritdoc />
        public TEdge OutEdge(TVertex vertex, int index)
        {
            return OutEdges(vertex).ElementAt(index);
        }

        /// <inheritdoc />
        public bool ContainsVertex(TVertex vertex)
        {
            return _tryGetOutEdgesFunc(vertex, out _);
        }

        #endregion
    }
}
