#nullable enable

using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace FastGraph
{
    /// <summary>
    /// A delegate-based directed implicit graph data structure.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public class DelegateImplicitGraph<TVertex, TEdge> : IImplicitGraph<TVertex, TEdge>
        where TVertex : notnull
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateImplicitGraph{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="tryGetOutEdges">Getter of out-edges.</param>
        /// <param name="allowParallelEdges">
        /// Indicates if parallel edges are allowed.
        /// Note that get of edges is delegated so you may have bugs related
        /// to parallel edges due to the delegated implementation.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="tryGetOutEdges"/> is <see langword="null"/>.</exception>
        public DelegateImplicitGraph(
            TryFunc<TVertex, IEnumerable<TEdge>> tryGetOutEdges,
            bool allowParallelEdges = true)
        {
            _tryGetOutEdgesFunc = tryGetOutEdges ?? throw new ArgumentNullException(nameof(tryGetOutEdges));
            AllowParallelEdges = allowParallelEdges;
        }

        /// <summary>
        /// Getter of out-edges.
        /// </summary>
        private readonly TryFunc<TVertex, IEnumerable<TEdge>> _tryGetOutEdgesFunc;

        #region IGraph<TVertex,TEdge>

        /// <inheritdoc />
        public bool IsDirected => true;

        /// <inheritdoc />
        public bool AllowParallelEdges { get; }

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

        [Pure]
        internal virtual IEnumerable<TEdge> OutEdgesInternal(TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            if (_tryGetOutEdgesFunc(vertex, out IEnumerable<TEdge>? outEdges))
                return outEdges;
            throw new VertexNotFoundException();
        }

        /// <inheritdoc />
        public IEnumerable<TEdge> OutEdges(TVertex vertex)
        {
            return OutEdgesInternal(vertex);
        }

        [Pure]
        internal virtual bool TryGetOutEdgesInternal(TVertex vertex, [NotNullWhen(true)] out IEnumerable<TEdge>? edges)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            return _tryGetOutEdgesFunc(vertex, out edges);
        }

        /// <inheritdoc />
        public bool TryGetOutEdges(TVertex vertex, [NotNullWhen(true)] out IEnumerable<TEdge>? edges)
        {
            return TryGetOutEdgesInternal(vertex, out edges);
        }

        /// <inheritdoc />
        public TEdge OutEdge(TVertex vertex, int index)
        {
            return OutEdges(vertex).ElementAt(index);
        }

        [Pure]
        internal virtual bool ContainsVertexInternal(TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            return _tryGetOutEdgesFunc(vertex, out _);
        }

        /// <inheritdoc />
        public bool ContainsVertex(TVertex vertex)
        {
            return ContainsVertexInternal(vertex);
        }

        #endregion
    }
}
