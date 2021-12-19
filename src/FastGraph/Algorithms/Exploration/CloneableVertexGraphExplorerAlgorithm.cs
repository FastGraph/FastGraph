#nullable enable

#if SUPPORTS_CLONEABLE
using JetBrains.Annotations;
using FastGraph.Algorithms.Services;

namespace FastGraph.Algorithms.Exploration
{
    /// <summary>
    /// Algorithm that explores a graph starting from a given vertex.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public sealed class CloneableVertexGraphExplorerAlgorithm<TVertex, TEdge>
        : RootedAlgorithmBase<TVertex, IMutableVertexAndEdgeSet<TVertex, TEdge>>
        , ITreeBuilderAlgorithm<TVertex, TEdge>
        where TVertex : ICloneable
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CloneableVertexGraphExplorerAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        public CloneableVertexGraphExplorerAlgorithm(
            IMutableVertexAndEdgeSet<TVertex, TEdge> visitedGraph)
            : this(default, visitedGraph)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CloneableVertexGraphExplorerAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        public CloneableVertexGraphExplorerAlgorithm(
            IAlgorithmComponent? host,
            IMutableVertexAndEdgeSet<TVertex, TEdge> visitedGraph)
            : base(host, visitedGraph)
        {
        }

        /// <summary>
        /// Transitions factories.
        /// </summary>
        private readonly List<ITransitionFactory<TVertex, TEdge>> _transitionFactories =
            new List<ITransitionFactory<TVertex, TEdge>>();

        private VertexPredicate<TVertex> _addVertexPredicate = _ => true;

        /// <summary>
        /// Predicate that a vertex must match to be added in the graph.
        /// </summary>
        /// <exception cref="T:System.ArgumentNullException">Set value is <see langword="null"/>.</exception>
        public VertexPredicate<TVertex> AddVertexPredicate
        {
            get => _addVertexPredicate;
            set => _addVertexPredicate = value ?? throw new ArgumentNullException(nameof(value));
        }

        private VertexPredicate<TVertex> _exploreVertexPredicate = _ => true;

        /// <summary>
        /// Predicate that checks if a given vertex should be explored or ignored.
        /// </summary>
        /// <exception cref="T:System.ArgumentNullException">Set value is <see langword="null"/>.</exception>
        public VertexPredicate<TVertex> ExploreVertexPredicate
        {
            get => _exploreVertexPredicate;
            set => _exploreVertexPredicate = value ?? throw new ArgumentNullException(nameof(value));
        }

        private EdgePredicate<TVertex, TEdge> _addEdgePredicate = _ => true;

        /// <summary>
        /// Predicate that an edge must match to be added in the graph.
        /// </summary>
        /// <exception cref="T:System.ArgumentNullException">Set value is <see langword="null"/>.</exception>
        public EdgePredicate<TVertex, TEdge> AddEdgePredicate
        {
            get => _addEdgePredicate;
            set => _addEdgePredicate = value ?? throw new ArgumentNullException(nameof(value));
        }

        private Predicate<CloneableVertexGraphExplorerAlgorithm<TVertex, TEdge>> _finishedPredicate =
            new DefaultFinishedPredicate().Test;

        /// <summary>
        /// Predicate that checks if the exploration is finished or not.
        /// </summary>
        /// <exception cref="T:System.ArgumentNullException">Set value is <see langword="null"/>.</exception>
        public Predicate<CloneableVertexGraphExplorerAlgorithm<TVertex, TEdge>> FinishedPredicate
        {
            get => _finishedPredicate;
            set => _finishedPredicate = value ?? throw new ArgumentNullException(nameof(value));
        }

        private readonly Queue<TVertex> _unExploredVertices = new Queue<TVertex>();

        /// <summary>
        /// Gets the enumeration of unexplored vertices.
        /// </summary>
        public IEnumerable<TVertex> UnExploredVertices => _unExploredVertices.AsEnumerable();

        /// <summary>
        /// Indicates if the algorithm finished successfully or not.
        /// </summary>
        public bool FinishedSuccessfully { get; private set; }

        #region Events

        /// <summary>
        /// Fired when a vertex is discovered.
        /// </summary>
        public event VertexAction<TVertex>? DiscoverVertex;

        private void OnVertexDiscovered(TVertex vertex)
        {
            VisitedGraph.AddVertex(vertex);
            _unExploredVertices.Enqueue(vertex);

            DiscoverVertex?.Invoke(vertex);
        }

        /// <summary>
        /// Fired when an edge is encountered.
        /// </summary>
        public event EdgeAction<TVertex, TEdge>? TreeEdge;

        private void OnTreeEdge(TEdge edge)
        {
            TreeEdge?.Invoke(edge);
        }

        /// <summary>
        /// Fired when a back edge is encountered.
        /// </summary>
        public event EdgeAction<TVertex, TEdge>? BackEdge;

        private void OnBackEdge(TEdge edge)
        {
            BackEdge?.Invoke(edge);
        }

        /// <summary>
        /// Fired when an edge was skipped from exploration due to failed vertex or edge predicate check.
        /// </summary>
        public event EdgeAction<TVertex, TEdge>? EdgeSkipped;

        private void OnEdgeSkipped(TEdge edge)
        {
            EdgeSkipped?.Invoke(edge);
        }

        #endregion

        /// <summary>
        /// Adds a new <see cref="ITransitionFactory{TVertex,TEdge}"/> to this algorithm.
        /// </summary>
        /// <param name="transitionFactory">Transition factory to add.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="transitionFactory"/> is <see langword="null"/>.</exception>
        public void AddTransitionFactory(ITransitionFactory<TVertex, TEdge> transitionFactory)
        {
            if (transitionFactory is null)
                throw new ArgumentNullException(nameof(transitionFactory));

            _transitionFactories.Add(transitionFactory);
        }

        /// <summary>
        /// Adds new <see cref="ITransitionFactory{TVertex,TEdge}"/>s to this algorithm.
        /// </summary>
        /// <param name="transitionFactories">Transition factories to add.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="transitionFactories"/> is <see langword="null"/>.</exception>
        public void AddTransitionFactories(
            IEnumerable<ITransitionFactory<TVertex, TEdge>> transitionFactories)
        {
            if (transitionFactories is null)
                throw new ArgumentNullException(nameof(transitionFactories));

            _transitionFactories.AddRange(transitionFactories);
        }

        /// <summary>
        /// Removes the given <paramref name="transitionFactory"/> from this algorithm.
        /// </summary>
        /// <param name="transitionFactory">Transition factory to remove.</param>
        public bool RemoveTransitionFactory(ITransitionFactory<TVertex, TEdge> transitionFactory)
        {
            return _transitionFactories.Remove(transitionFactory);
        }

        /// <summary>
        /// Clears all <see cref="ITransitionFactory{TVertex,TEdge}"/> from this algorithm.
        /// </summary>
        public void ClearTransitionFactories()
        {
            _transitionFactories.Clear();
        }

        /// <summary>
        /// Checks if this algorithm contains the given <paramref name="transitionFactory"/>.
        /// </summary>
        /// <param name="transitionFactory">Transition factory to check.</param>
        [Pure]
        public bool ContainsTransitionFactory(ITransitionFactory<TVertex, TEdge> transitionFactory)
        {
            return _transitionFactories.Contains(transitionFactory);
        }

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            if (!TryGetRootVertex(out TVertex? root))
                throw new InvalidOperationException("Root vertex not set.");

            VisitedGraph.Clear();
            _unExploredVertices.Clear();
            FinishedSuccessfully = false;

            if (!AddVertexPredicate(root))
                throw new InvalidOperationException($"Starting vertex does not satisfy the {nameof(AddVertexPredicate)}.");
            OnVertexDiscovered(root);

            while (_unExploredVertices.Count > 0)
            {
                // Are we done yet?
                if (!FinishedPredicate(this))
                {
                    FinishedSuccessfully = false;
                    return;
                }

                TVertex current = _unExploredVertices.Dequeue();
                var clone = (TVertex)current.Clone();

                // Let's make sure we want to explore this one
                if (!ExploreVertexPredicate(clone))
                    continue;

                foreach (ITransitionFactory<TVertex, TEdge> transitionFactory in _transitionFactories)
                {
                    GenerateFromTransitionFactory(clone, transitionFactory);
                }
            }

            FinishedSuccessfully = true;
        }

        /// <inheritdoc />
        public override void Compute(TVertex root)
        {
            SetRootVertex(root);
            Compute();
        }

        #endregion

        private void GenerateFromTransitionFactory(
            TVertex current,
            ITransitionFactory<TVertex, TEdge> transitionFactory)
        {
            if (!transitionFactory.IsValid(current))
                return;

            foreach (TEdge transition in transitionFactory.Apply(current))
            {
                if (!AddVertexPredicate(transition.Target)
                    || !AddEdgePredicate(transition))
                {
                    OnEdgeSkipped(transition);
                    continue;
                }

                bool backEdge = VisitedGraph.ContainsVertex(transition.Target);
                if (!backEdge)
                    OnVertexDiscovered(transition.Target);

                VisitedGraph.AddEdge(transition);
                if (backEdge)
                    OnBackEdge(transition);
                else
                    OnTreeEdge(transition);
            }
        }

        /// <summary>
        /// Default implementation of the finished predicate.
        /// </summary>
        public sealed class DefaultFinishedPredicate
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DefaultFinishedPredicate"/> class.
            /// </summary>
            public DefaultFinishedPredicate()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="DefaultFinishedPredicate"/> class.
            /// </summary>
            /// <param name="maxVertexCount">Maximum number of vertices.</param>
            /// <param name="maxEdgeCount">Maximum number of edges.</param>
            public DefaultFinishedPredicate(int maxVertexCount, int maxEdgeCount)
            {
                MaxVertexCount = maxVertexCount;
                MaxEdgeCount = maxEdgeCount;
            }

            /// <summary>
            /// Maximum number of vertices (for exploration).
            /// </summary>
            public int MaxVertexCount { get; set; } = 1000;

            /// <summary>
            /// Maximum number of edges (for exploration).
            /// </summary>
            public int MaxEdgeCount { get; set; } = 1000;

            /// <summary>
            /// Checks if the given <paramref name="algorithm"/> explorer has finished or not.
            /// </summary>
            /// <param name="algorithm">Algorithm explorer to check.</param>
            /// <returns>True if the explorer can continue to explore, false otherwise.</returns>
            /// <exception cref="T:System.ArgumentNullException"><paramref name="algorithm"/> is <see langword="null"/>.</exception>
            [Pure]
            public bool Test(CloneableVertexGraphExplorerAlgorithm<TVertex, TEdge> algorithm)
            {
                if (algorithm is null)
                    throw new ArgumentNullException(nameof(algorithm));

                if (algorithm.VisitedGraph.VertexCount > MaxVertexCount)
                    return false;

                if (algorithm.VisitedGraph.EdgeCount > MaxEdgeCount)
                    return false;

                return true;
            }
        }
    }
}
#endif
