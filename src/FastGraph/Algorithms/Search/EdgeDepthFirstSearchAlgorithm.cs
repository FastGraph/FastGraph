#nullable enable

using FastGraph.Algorithms.Services;

namespace FastGraph.Algorithms.Search
{
    /// <summary>
    /// A edge depth first search algorithm for directed graphs.
    /// </summary>
    /// <remarks>
    /// This is a variant of the classic DFS algorithm where the
    /// edges are color marked instead of the vertices.
    /// </remarks>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public sealed class EdgeDepthFirstSearchAlgorithm<TVertex, TEdge>
        : RootedAlgorithmBase<TVertex, IEdgeListAndIncidenceGraph<TVertex, TEdge>>
        , IEdgeColorizerAlgorithm<TVertex, TEdge>
        , IEdgePredecessorRecorderAlgorithm<TVertex, TEdge>
        , ITreeBuilderAlgorithm<TVertex, TEdge>
        where TVertex : notnull
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EdgeDepthFirstSearchAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        public EdgeDepthFirstSearchAlgorithm(
            IEdgeListAndIncidenceGraph<TVertex, TEdge> visitedGraph)
            : this(visitedGraph, new Dictionary<TEdge, GraphColor>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EdgeDepthFirstSearchAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="edgesColors">Edges associated to their colors (treatment states).</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="edgesColors"/> is <see langword="null"/>.</exception>
        public EdgeDepthFirstSearchAlgorithm(
            IEdgeListAndIncidenceGraph<TVertex, TEdge> visitedGraph,
            IDictionary<TEdge, GraphColor> edgesColors)
            : this(default, visitedGraph, edgesColors)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EdgeDepthFirstSearchAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="edgesColors">Edges associated to their colors (treatment states).</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="edgesColors"/> is <see langword="null"/>.</exception>
        public EdgeDepthFirstSearchAlgorithm(
            IAlgorithmComponent? host,
            IEdgeListAndIncidenceGraph<TVertex, TEdge> visitedGraph,
            IDictionary<TEdge, GraphColor> edgesColors)
            : base(host, visitedGraph)
        {
            EdgesColors = edgesColors ?? throw new ArgumentNullException(nameof(edgesColors));
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

        /// <summary>
        /// Fired when an edge is initialized.
        /// </summary>
        public event EdgeAction<TVertex, TEdge>? InitializeEdge;

        private void OnEdgeInitialized(TEdge edge)
        {
            InitializeEdge?.Invoke(edge);
        }

        /// <summary>
        /// Fired on the root vertex once before the start of the search from it.
        /// </summary>
        public event VertexAction<TVertex>? StartVertex;

        private void OnStartVertex(TVertex vertex)
        {
            StartVertex?.Invoke(vertex);
        }

        /// <summary>
        /// Fired when an edge starts to be treated.
        /// </summary>
        public event EdgeAction<TVertex, TEdge>? StartEdge;

        private void OnStartEdge(TEdge edge)
        {
            StartEdge?.Invoke(edge);
        }

        /// <inheritdoc />
        public event EdgeEdgeAction<TVertex, TEdge>? DiscoverTreeEdge;

        private void OnDiscoverTreeEdge(TEdge edge, TEdge targetEdge)
        {
            DiscoverTreeEdge?.Invoke(edge, targetEdge);
        }

        /// <summary>
        /// Fired when an edge is going to be treated when coming from a white edge.
        /// </summary>
        public event EdgeAction<TVertex, TEdge>? TreeEdge;

        private void OnTreeEdge(TEdge edge)
        {
            TreeEdge?.Invoke(edge);
        }

        /// <summary>
        /// Fired when an edge is going to be treated when coming from a gray edge.
        /// </summary>
        public event EdgeAction<TVertex, TEdge>? BackEdge;

        private void OnBackEdge(TEdge edge)
        {
            BackEdge?.Invoke(edge);
        }

        /// <summary>
        /// Fired when an edge is going to be treated when coming from a black edge.
        /// </summary>
        public event EdgeAction<TVertex, TEdge>? ForwardOrCrossEdge;

        private void OnForwardOrCrossEdge(TEdge edge)
        {
            ForwardOrCrossEdge?.Invoke(edge);
        }

        /// <inheritdoc />
        public event EdgeAction<TVertex, TEdge>? FinishEdge;

        private void OnFinishEdge(TEdge edge)
        {
            FinishEdge?.Invoke(edge);
        }

        #endregion

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void Initialize()
        {
            // Put all edges to white
            foreach (TEdge edge in VisitedGraph.Edges)
            {
                ThrowIfCancellationRequested();

                EdgesColors[edge] = GraphColor.White;
                OnEdgeInitialized(edge);
            }
        }

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            ThrowIfCancellationRequested();

            // Start with root vertex
            if (TryGetRootVertex(out TVertex? root))
            {
                AssertRootInGraph(root);

                OnStartVertex(root);

                // Process each out edge of the root one
                VisitAllWhiteEdgesSet(VisitedGraph.OutEdges(root));

                // Process the rest of the graph edges
                if (ProcessAllComponents)
                {
                    VisitAllWhiteEdges();
                }
            }
            else
            {
                VisitAllWhiteEdges();
            }

            #region Local functions

            void VisitAllWhiteEdgesSet(IEnumerable<TEdge> edges)
            {
                foreach (TEdge edge in edges)
                {
                    ThrowIfCancellationRequested();

                    if (EdgesColors[edge] == GraphColor.White)
                    {
                        OnStartEdge(edge);
                        Visit(edge, 0);
                    }
                }
            }

            void VisitAllWhiteEdges()
            {
                VisitAllWhiteEdgesSet(VisitedGraph.Edges);
            }

            #endregion
        }

        #endregion

        #region IEdgeColorizerAlgorithm<TVertex,TEdge>

        /// <inheritdoc />
        public IDictionary<TEdge, GraphColor> EdgesColors { get; }

        #endregion

        private void Visit(TEdge rootEdge, int depth)
        {
            // Mark edge as gray
            EdgesColors[rootEdge] = GraphColor.Gray;
            // Add edge to the search tree
            OnTreeEdge(rootEdge);

            // Iterate over out-edges
            foreach (TEdge edge in VisitedGraph.OutEdges(rootEdge.Target))
            {
                ThrowIfCancellationRequested();

                // Check edge is not explored yet,
                // If not, explore it
                if (EdgesColors[edge] == GraphColor.White)
                {
                    OnDiscoverTreeEdge(rootEdge, edge);

                    int newDepth = depth + 1;
                    if (newDepth <= MaxDepth)
                    {
                        Visit(edge, newDepth);
                    }
                    else
                    {
                        EdgesColors[edge] = GraphColor.Black;
                        OnFinishEdge(edge);
                    }
                }
                else if (EdgesColors[edge] == GraphColor.Gray)
                {
                    // Edge is being explored
                    OnBackEdge(edge);
                }
                else
                {
                    // Edge is black
                    OnForwardOrCrossEdge(edge);
                }
            }

            // All out-edges have been explored
            EdgesColors[rootEdge] = GraphColor.Black;
            OnFinishEdge(rootEdge);
        }
    }
}
