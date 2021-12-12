#nullable enable

using FastGraph.Algorithms.Services;

namespace FastGraph.Algorithms.Search
{
    /// <summary>
    /// A depth first search algorithm for implicit directed graphs.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public sealed class ImplicitDepthFirstSearchAlgorithm<TVertex, TEdge>
        : RootedAlgorithmBase<TVertex, IIncidenceGraph<TVertex, TEdge>>
        , IVertexPredecessorRecorderAlgorithm<TVertex, TEdge>
        , IVertexTimeStamperAlgorithm<TVertex>
        where TVertex : notnull
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImplicitDepthFirstSearchAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        public ImplicitDepthFirstSearchAlgorithm(
            IIncidenceGraph<TVertex, TEdge> visitedGraph)
            : this(default, visitedGraph)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImplicitDepthFirstSearchAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        public ImplicitDepthFirstSearchAlgorithm(
            IAlgorithmComponent? host,
            IIncidenceGraph<TVertex, TEdge> visitedGraph)
            : base(host, visitedGraph)
        {
        }

        /// <summary>
        /// Stores vertices associated to their colors (treatment state).
        /// </summary>
        public IDictionary<TVertex, GraphColor> VerticesColors { get; } = new Dictionary<TVertex, GraphColor>();

        private int _maxDepth = int.MaxValue;

        /// <summary>
        /// Gets or sets the maximum exploration depth, from the start vertex.
        /// </summary>
        /// <remarks>
        /// Defaulted to <see cref="F:int.MaxValue"/>.
        /// </remarks>
        /// <value>
        /// Maximum exploration depth.
        /// </value>
        /// <exception cref="T:System.ArgumentOutOfRangeException">Value is negative or equal to 0.</exception>
        public int MaxDepth
        {
            get => _maxDepth;
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "Must be positive.");
                _maxDepth = value;
            }
        }

        #region Events

        /// <summary>
        /// Fired on the root vertex once before the start of the search from it.
        /// </summary>
        public event VertexAction<TVertex>? StartVertex;

        private void OnStartVertex(TVertex vertex)
        {
            StartVertex?.Invoke(vertex);
        }

        /// <summary>
        /// Invoked when a vertex is encountered for the first time.
        /// </summary>
        public event VertexAction<TVertex>? DiscoverVertex;

        private void OnDiscoverVertex(TVertex vertex)
        {
            DiscoverVertex?.Invoke(vertex);
        }

        /// <summary>
        /// Invoked on every out-edge of each vertex after it is discovered.
        /// </summary>
        public event EdgeAction<TVertex, TEdge>? ExamineEdge;

        private void OnExamineEdge(TEdge edge)
        {
            ExamineEdge?.Invoke(edge);
        }

        /// <summary>
        /// Fired on each edge as it becomes a member of the edges that form
        /// the search tree. If you wish to record predecessors, do so at this
        /// event point.
        /// </summary>
        public event EdgeAction<TVertex, TEdge>? TreeEdge;

        private void OnTreeEdge(TEdge edge)
        {
            TreeEdge?.Invoke(edge);
        }

        /// <summary>
        /// Fired on the back edges in the graph.
        /// </summary>
        public event EdgeAction<TVertex, TEdge>? BackEdge;

        private void OnBackEdge(TEdge edge)
        {
            BackEdge?.Invoke(edge);
        }

        /// <summary>
        /// Fired on forward or cross edges in the graph.
        /// (In an undirected graph this method is never called.)
        /// </summary>
        public event EdgeAction<TVertex, TEdge>? ForwardOrCrossEdge;

        private void OnForwardOrCrossEdge(TEdge edge)
        {
            ForwardOrCrossEdge?.Invoke(edge);
        }

        /// <summary>
        /// Fired on a vertex after all of its out edges have been added to
        /// the search tree and all of the adjacent vertices have been
        /// discovered (but before their out-edges have been examined).
        /// </summary>
        public event VertexAction<TVertex>? FinishVertex;

        private void OnVertexFinished(TVertex vertex)
        {
            FinishVertex?.Invoke(vertex);
        }

        #endregion

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void Initialize()
        {
            base.Initialize();

            VerticesColors.Clear();
        }

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            TVertex root = GetAndAssertRootInGraph();
            OnStartVertex(root);
            Visit(root, 0);
        }

        #endregion

        private void Visit(TVertex u, int depth)
        {
            if (depth > MaxDepth)
                return;

            VerticesColors[u] = GraphColor.Gray;
            OnDiscoverVertex(u);

            foreach (TEdge edge in VisitedGraph.OutEdges(u))
            {
                ThrowIfCancellationRequested();

                OnExamineEdge(edge);
                TVertex v = edge.Target;

                if (!VerticesColors.TryGetValue(v, out GraphColor vColor))
                {
                    OnTreeEdge(edge);
                    Visit(v, depth + 1);
                }
                else
                {
                    if (vColor == GraphColor.Gray)
                    {
                        OnBackEdge(edge);
                    }
                    else
                    {
                        OnForwardOrCrossEdge(edge);
                    }
                }
            }

            VerticesColors[u] = GraphColor.Black;
            OnVertexFinished(u);
        }
    }
}
