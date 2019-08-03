using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using static QuikGraph.Utils.DisposableHelpers;

namespace QuikGraph.Algorithms.Observers
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
        public VertexPredecessorPathRecorderObserver(
            [NotNull] IDictionary<TVertex, TEdge> verticesPredecessors)
        {
            if (verticesPredecessors is null)
                throw new ArgumentNullException(nameof(verticesPredecessors));

            VerticesPredecessors = verticesPredecessors;
        }

        /// <summary>
        /// Vertex predecessors.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        public IDictionary<TVertex, TEdge> VerticesPredecessors { get; }

        /// <summary>
        /// Path ending vertices.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        public ICollection<TVertex> EndPathVertices { get; } = new List<TVertex>();

        /// <summary>
        /// Gets all paths.
        /// </summary>
        /// <returns>Enumerable of paths.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        [NotNull, ItemNotNull]
        public IEnumerable<IEnumerable<TEdge>> AllPaths()
        {
            return EndPathVertices
                .Select(vertex =>
                {
                    if (VerticesPredecessors.TryGetPath(vertex, out IEnumerable<TEdge> path))
                        return path;
                    return null;
                })
                .Where(path => path != null);
        }

        #region IObserver<TAlgorithm>

        /// <inheritdoc />
        public IDisposable Attach(IVertexPredecessorRecorderAlgorithm<TVertex, TEdge> algorithm)
        {
            algorithm.TreeEdge += OnEdgeDiscovered;
            algorithm.FinishVertex += OnVertexFinished;
            return Finally(() =>
            {
                algorithm.TreeEdge -= OnEdgeDiscovered;
                algorithm.FinishVertex -= OnVertexFinished;
            });
        }

        #endregion

        private void OnEdgeDiscovered([NotNull] TEdge edge)
        {
            VerticesPredecessors[edge.Target] = edge;
        }

        private void OnVertexFinished([NotNull] TVertex vertex)
        {
            foreach (TEdge edge in VerticesPredecessors.Values)
            {
                if (edge.Source.Equals(vertex))
                    return;
            }

            EndPathVertices.Add(vertex);
        }
    }
}
