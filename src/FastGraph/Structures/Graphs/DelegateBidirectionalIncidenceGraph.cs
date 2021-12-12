#nullable enable

using System.Diagnostics.CodeAnalysis;

namespace FastGraph
{
    /// <summary>
    /// A delegate-based directed bidirectional graph data structure.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public class DelegateBidirectionalIncidenceGraph<TVertex, TEdge> : DelegateIncidenceGraph<TVertex, TEdge>, IBidirectionalIncidenceGraph<TVertex, TEdge>
        where TVertex : notnull
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateBidirectionalIncidenceGraph{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="tryGetOutEdges">Getter of out-edges.</param>
        /// <param name="tryGetInEdges">Getter of in-edges.</param>
        /// <param name="allowParallelEdges">
        /// Indicates if parallel edges are allowed.
        /// Note that get of edges is delegated so you may have bugs related
        /// to parallel edges due to the delegated implementation.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="tryGetOutEdges"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="tryGetInEdges"/> is <see langword="null"/>.</exception>
        public DelegateBidirectionalIncidenceGraph(
            TryFunc<TVertex, IEnumerable<TEdge>> tryGetOutEdges,
            TryFunc<TVertex, IEnumerable<TEdge>> tryGetInEdges,
            bool allowParallelEdges = true)
            : base(tryGetOutEdges, allowParallelEdges)
        {
            _tryGetInEdgesFunc = tryGetInEdges ?? throw new ArgumentNullException(nameof(tryGetInEdges));
        }

        /// <summary>
        /// Getter of in-edges.
        /// </summary>
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
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            if (_tryGetInEdgesFunc(vertex, out IEnumerable<TEdge>? inEdges))
                return inEdges;
            throw new VertexNotFoundException();
        }

        /// <inheritdoc />
        public bool TryGetInEdges(TVertex vertex, [NotNullWhen(true)] out IEnumerable<TEdge>? edges)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

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
