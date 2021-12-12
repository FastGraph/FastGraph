#nullable enable

using JetBrains.Annotations;
using static FastGraph.Utils.DisposableHelpers;

namespace FastGraph.Algorithms.Observers
{
    /// <summary>
    /// Recorder of edges predecessors.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class EdgePredecessorRecorderObserver<TVertex, TEdge> : IObserver<IEdgePredecessorRecorderAlgorithm<TVertex, TEdge>>
        where TVertex : notnull
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EdgePredecessorRecorderObserver{TVertex,TEdge}"/> class.
        /// </summary>
        public EdgePredecessorRecorderObserver()
            : this(new Dictionary<TEdge, TEdge>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EdgePredecessorRecorderObserver{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="edgesPredecessors">Edges predecessors.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="edgesPredecessors"/> is <see langword="null"/>.</exception>
        public EdgePredecessorRecorderObserver(
            IDictionary<TEdge, TEdge> edgesPredecessors)
        {
            EdgesPredecessors = edgesPredecessors ?? throw new ArgumentNullException(nameof(edgesPredecessors));
            EndPathEdges = new List<TEdge>();
        }

        /// <summary>
        /// Edges predecessors.
        /// </summary>
        public IDictionary<TEdge, TEdge> EdgesPredecessors { get; }

        /// <summary>
        /// Path ending edges.
        /// </summary>
        public ICollection<TEdge> EndPathEdges { get; }

        #region IObserver<TAlgorithm>

        /// <inheritdoc />
        public IDisposable Attach(IEdgePredecessorRecorderAlgorithm<TVertex, TEdge> algorithm)
        {
            if (algorithm is null)
                throw new ArgumentNullException(nameof(algorithm));

            algorithm.DiscoverTreeEdge += OnEdgeDiscovered;
            algorithm.FinishEdge += OnEdgeFinished;

            return Finally(() =>
            {
                algorithm.DiscoverTreeEdge -= OnEdgeDiscovered;
                algorithm.FinishEdge -= OnEdgeFinished;
            });
        }

        #endregion

        /// <summary>
        /// Gets a path starting with <paramref name="startingEdge"/>.
        /// </summary>
        /// <param name="startingEdge">Starting edge.</param>
        /// <returns>Edge path.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="startingEdge"/> is <see langword="null"/>.</exception>
        [Pure]
        public ICollection<TEdge> Path(TEdge startingEdge)
        {
            if (startingEdge == null)
                throw new ArgumentNullException(nameof(startingEdge));

            var path = new List<TEdge> { startingEdge };

            TEdge currentEdge = startingEdge;
            while (EdgesPredecessors.TryGetValue(currentEdge, out TEdge? edge))
            {
                path.Add(edge);
                currentEdge = edge;
            }

            path.Reverse();
            return path;
        }

        /// <summary>
        /// Gets all paths.
        /// </summary>
        /// <returns>Enumerable of paths.</returns>
        [Pure]
        public IEnumerable<ICollection<TEdge>> AllPaths()
        {
            return EndPathEdges.Select(Path);
        }

        /// <summary>
        /// Merges the path starting at <paramref name="startingEdge"/> with remaining edges.
        /// </summary>
        /// <param name="startingEdge">Starting edge.</param>
        /// <param name="colors">Edges colors mapping.</param>
        /// <returns>Merged path.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="startingEdge"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="colors"/> is <see langword="null"/>.</exception>
        [Pure]
        public ICollection<TEdge> MergedPath(
            TEdge startingEdge,
            IDictionary<TEdge, GraphColor> colors)
        {
            if (startingEdge == null)
                throw new ArgumentNullException(nameof(startingEdge));
            if (colors is null)
                throw new ArgumentNullException(nameof(colors));

            var path = new List<TEdge>();

            TEdge currentEdge = startingEdge;
            GraphColor color = colors[currentEdge];
            if (color != GraphColor.White)
                return path;
            colors[currentEdge] = GraphColor.Black;

            path.Add(currentEdge);
            while (EdgesPredecessors.TryGetValue(currentEdge, out TEdge? edge))
            {
                color = colors[edge];
                if (color != GraphColor.White)
                {
                    path.Reverse();
                    return path;
                }

                colors[edge] = GraphColor.Black;

                path.Add(edge);
                currentEdge = edge;
            }

            path.Reverse();
            return path;
        }

        /// <summary>
        /// Gets all merged path.
        /// </summary>
        /// <returns>Enumerable of merged paths.</returns>
        [Pure]
        public IEnumerable<ICollection<TEdge>> AllMergedPaths()
        {
            var colors = new Dictionary<TEdge, GraphColor>();

            foreach (KeyValuePair<TEdge, TEdge> pair in EdgesPredecessors)
            {
                colors[pair.Key] = GraphColor.White;
                colors[pair.Value] = GraphColor.White;
            }

            return EndPathEdges.Select(edge => MergedPath(edge, colors));
        }

        private void OnEdgeDiscovered(TEdge edge, TEdge targetEdge)
        {
            if (!EqualityComparer<TEdge?>.Default.Equals(edge, targetEdge))
            {
                EdgesPredecessors[targetEdge] = edge;
            }
        }

        private void OnEdgeFinished(TEdge finishedEdge)
        {
            if (EdgesPredecessors.Values.Any(edge => EqualityComparer<TEdge?>.Default.Equals(finishedEdge, edge)))
                return;

            EndPathEdges.Add(finishedEdge);
        }
    }
}
