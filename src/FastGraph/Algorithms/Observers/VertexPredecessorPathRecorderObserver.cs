#nullable enable

using JetBrains.Annotations;
using static FastGraph.Utils.DisposableHelpers;

namespace FastGraph.Algorithms.Observers
{
    /// <summary>
    /// Recorder of vertices predecessors paths.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class VertexPredecessorPathRecorderObserver<TVertex, TEdge> :
        IObserver<IVertexPredecessorRecorderAlgorithm<TVertex, TEdge>>
        where TVertex : notnull
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VertexPredecessorPathRecorderObserver{TVertex,TEdge}"/> class.
        /// </summary>
        public VertexPredecessorPathRecorderObserver()
            : this(new Dictionary<TVertex, TEdge>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexPredecessorPathRecorderObserver{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="verticesPredecessors">Vertices predecessors.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="verticesPredecessors"/> is <see langword="null"/>.</exception>
        public VertexPredecessorPathRecorderObserver(
            IDictionary<TVertex, TEdge> verticesPredecessors)
        {
            VerticesPredecessors = verticesPredecessors ?? throw new ArgumentNullException(nameof(verticesPredecessors));
        }

        /// <summary>
        /// Vertices predecessors.
        /// </summary>
        public IDictionary<TVertex, TEdge> VerticesPredecessors { get; }

        /// <summary>
        /// Path ending vertices.
        /// </summary>
        public ICollection<TVertex> EndPathVertices { get; } = new List<TVertex>();

        /// <summary>
        /// Gets all paths.
        /// </summary>
        /// <returns>Enumerable of paths.</returns>
        [Pure]
        public IEnumerable<IEnumerable<TEdge>> AllPaths()
        {
            return EndPathVertices
                .Select(vertex => VerticesPredecessors.TryGetPath(vertex, out IEnumerable<TEdge>? path) ? path : default)
                .Where(path => path != default)!;
        }

        #region IObserver<TAlgorithm>

        /// <inheritdoc />
        public IDisposable Attach(IVertexPredecessorRecorderAlgorithm<TVertex, TEdge> algorithm)
        {
            if (algorithm is null)
                throw new ArgumentNullException(nameof(algorithm));

            algorithm.TreeEdge += OnEdgeDiscovered;
            algorithm.FinishVertex += OnVertexFinished;
            return Finally(() =>
            {
                algorithm.TreeEdge -= OnEdgeDiscovered;
                algorithm.FinishVertex -= OnVertexFinished;
            });
        }

        #endregion

        private void OnEdgeDiscovered(TEdge edge)
        {
            VerticesPredecessors[edge.Target] = edge;
        }

        private void OnVertexFinished(TVertex vertex)
        {
            if (VerticesPredecessors.Values.Any(edge => EqualityComparer<TVertex?>.Default.Equals(edge.Source, vertex)))
                return;

            EndPathVertices.Add(vertex);
        }
    }
}
