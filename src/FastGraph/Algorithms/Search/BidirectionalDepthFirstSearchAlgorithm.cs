#nullable enable

using FastGraph.Algorithms.Services;

namespace FastGraph.Algorithms.Search
{
    /// <summary>
    /// A depth and height first search algorithm for directed graphs.
    /// </summary>
    /// <remarks>
    /// This is a modified version of the classic DFS algorithm
    /// where the search is performed both in depth and height.
    /// </remarks>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public sealed class BidirectionalDepthFirstSearchAlgorithm<TVertex, TEdge>
        : RootedAlgorithmBase<TVertex, IBidirectionalGraph<TVertex, TEdge>>
        , IVertexPredecessorRecorderAlgorithm<TVertex, TEdge>
        , IDistanceRecorderAlgorithm<TVertex>
        , IVertexColorizerAlgorithm<TVertex>
        where TVertex : notnull
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BidirectionalDepthFirstSearchAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        public BidirectionalDepthFirstSearchAlgorithm(
            IBidirectionalGraph<TVertex, TEdge> visitedGraph)
            : this(visitedGraph, new Dictionary<TVertex, GraphColor>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BidirectionalDepthFirstSearchAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="verticesColors">Vertices associated to their colors (treatment states).</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="verticesColors"/> is <see langword="null"/>.</exception>
        public BidirectionalDepthFirstSearchAlgorithm(
            IBidirectionalGraph<TVertex, TEdge> visitedGraph,
            IDictionary<TVertex, GraphColor> verticesColors)
            : this(default, visitedGraph, verticesColors)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BidirectionalDepthFirstSearchAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="verticesColors">Vertices associated to their colors (treatment states).</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="verticesColors"/> is <see langword="null"/>.</exception>
        public BidirectionalDepthFirstSearchAlgorithm(
            IAlgorithmComponent? host,
            IBidirectionalGraph<TVertex, TEdge> visitedGraph,
            IDictionary<TVertex, GraphColor> verticesColors)
            : base(host, visitedGraph)
        {
            VerticesColors = verticesColors ?? throw new ArgumentNullException(nameof(verticesColors));
        }

        /// <summary>
        /// In case a root vertex has been set, indicates if the algorithm should
        /// walk through graph parts of other components than the root component.
        /// </summary>
        public bool ProcessAllComponents { get; set; }

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

        /// <inheritdoc />
        public event VertexAction<TVertex>? InitializeVertex;

        private void OnVertexInitialized(TVertex vertex)
        {
            InitializeVertex?.Invoke(vertex);
        }

        /// <inheritdoc />
        public event VertexAction<TVertex>? StartVertex;

        private void OnStartVertex(TVertex vertex)
        {
            StartVertex?.Invoke(vertex);
        }

        /// <summary>
        /// Fired when a vertex is discovered and under treatment.
        /// </summary>
        public event VertexAction<TVertex>? DiscoverVertex;

        private void OnDiscoverVertex(TVertex vertex)
        {
            DiscoverVertex?.Invoke(vertex);
        }

        /// <summary>
        /// Fired when an edge is going to be analyzed.
        /// </summary>
        public event EdgeAction<TVertex, TEdge>? ExamineEdge;

        private void OnExamineEdge(TEdge edge)
        {
            ExamineEdge?.Invoke(edge);
        }

        /// <summary>
        /// Fired when an edge is going to be treated when coming from a white vertex.
        /// </summary>
        public event EdgeAction<TVertex, TEdge>? TreeEdge;

        private void OnTreeEdge(TEdge edge)
        {
            TreeEdge?.Invoke(edge);
        }

        /// <summary>
        /// Fired when an edge is going to be treated when coming from a gray vertex.
        /// </summary>
        public event EdgeAction<TVertex, TEdge>? BackEdge;

        private void OnBackEdge(TEdge edge)
        {
            BackEdge?.Invoke(edge);
        }

        /// <summary>
        /// Fired when an edge is going to be treated when coming from a black vertex.
        /// </summary>
        public event EdgeAction<TVertex, TEdge>? ForwardOrCrossEdge;

        private void OnForwardOrCrossEdge(TEdge edge)
        {
            ForwardOrCrossEdge?.Invoke(edge);
        }

        /// <inheritdoc />
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

            // Put all vertex to white
            VerticesColors.Clear();
            foreach (TVertex vertex in VisitedGraph.Vertices)
            {
                VerticesColors[vertex] = GraphColor.White;
                OnVertexInitialized(vertex);
            }
        }

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            // If there is a starting vertex, start with it
            if (TryGetRootVertex(out TVertex? root))
            {
                AssertRootInGraph(root);

                OnStartVertex(root);
                Visit(root, 0);

                if (ProcessAllComponents)
                {
                    VisitAllWhiteVertices(); // All remaining vertices (because there are not white marked)
                }
            }
            else
            {
                VisitAllWhiteVertices();
            }

            #region Local function

            void VisitAllWhiteVertices()
            {
                // Process each vertex
                foreach (TVertex vertex in VisitedGraph.Vertices)
                {
                    ThrowIfCancellationRequested();

                    if (VerticesColors[vertex] == GraphColor.White)
                    {
                        OnStartVertex(vertex);
                        Visit(vertex, 0);
                    }
                }
            }

            #endregion
        }

        #endregion

        /// <summary>
        /// Stores vertices associated to their colors (treatment state).
        /// </summary>
        public IDictionary<TVertex, GraphColor> VerticesColors { get; }

        #region IVertexColorizerAlgorithm<TVertex>

        /// <inheritdoc />
        public GraphColor GetVertexColor(TVertex vertex)
        {
            if (VerticesColors.TryGetValue(vertex, out GraphColor color))
                return color;
            throw new VertexNotFoundException();
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
                ProcessEdge(depth, v, edge);
            }

            foreach (TEdge edge in VisitedGraph.InEdges(u))
            {
                ThrowIfCancellationRequested();

                OnExamineEdge(edge);
                TVertex v = edge.Source;
                ProcessEdge(depth, v, edge);
            }

            VerticesColors[u] = GraphColor.Black;
            OnVertexFinished(u);
        }

        private void ProcessEdge(int depth, TVertex vertex, TEdge edge)
        {
            GraphColor color = VerticesColors[vertex];
            if (color == GraphColor.White)
            {
                OnTreeEdge(edge);
                Visit(vertex, depth + 1);
            }
            else if (color == GraphColor.Gray)
            {
                OnBackEdge(edge);
            }
            else
            {
                OnForwardOrCrossEdge(edge);
            }
        }
    }
}
