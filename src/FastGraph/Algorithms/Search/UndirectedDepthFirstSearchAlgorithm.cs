#nullable enable

using System.Diagnostics;
using FastGraph.Algorithms.Services;

namespace FastGraph.Algorithms.Search
{
    /// <summary>
    /// A depth first search algorithm for undirected graph.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public sealed class UndirectedDepthFirstSearchAlgorithm<TVertex, TEdge>
        : RootedAlgorithmBase<TVertex, IUndirectedGraph<TVertex, TEdge>>
        , IDistanceRecorderAlgorithm<TVertex>
        , IVertexColorizerAlgorithm<TVertex>
        , IUndirectedVertexPredecessorRecorderAlgorithm<TVertex, TEdge>
        , IVertexTimeStamperAlgorithm<TVertex>
        where TVertex : notnull
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UndirectedDepthFirstSearchAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        public UndirectedDepthFirstSearchAlgorithm(
            IUndirectedGraph<TVertex, TEdge> visitedGraph)
            : this(visitedGraph, new Dictionary<TVertex, GraphColor>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UndirectedDepthFirstSearchAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="verticesColors">Vertices associated to their colors (treatment states).</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="verticesColors"/> is <see langword="null"/>.</exception>
        public UndirectedDepthFirstSearchAlgorithm(
            IUndirectedGraph<TVertex, TEdge> visitedGraph,
            IDictionary<TVertex, GraphColor> verticesColors)
            : this(default, visitedGraph, verticesColors)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UndirectedDepthFirstSearchAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="verticesColors">Vertices associated to their colors (treatment states).</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="verticesColors"/> is <see langword="null"/>.</exception>
        public UndirectedDepthFirstSearchAlgorithm(
            IAlgorithmComponent? host,
            IUndirectedGraph<TVertex, TEdge> visitedGraph,
            IDictionary<TVertex, GraphColor> verticesColors)
            : this(host, visitedGraph, verticesColors, edges => edges)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BidirectionalDepthFirstSearchAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="verticesColors">Vertices associated to their colors (treatment states).</param>
        /// <param name="adjacentEdgesFilter">
        /// Delegate that takes the enumeration of out-edges and filters/reorders
        /// them. All vertices passed to the method should be enumerated once and only once.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="verticesColors"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="adjacentEdgesFilter"/> is <see langword="null"/>.</exception>
        public UndirectedDepthFirstSearchAlgorithm(
            IAlgorithmComponent? host,
            IUndirectedGraph<TVertex, TEdge> visitedGraph,
            IDictionary<TVertex, GraphColor> verticesColors,
            Func<IEnumerable<TEdge>, IEnumerable<TEdge>> adjacentEdgesFilter)
            : base(host, visitedGraph)
        {
            VerticesColors = verticesColors ?? throw new ArgumentNullException(nameof(verticesColors));
            AdjacentEdgesFilter = adjacentEdgesFilter ?? throw new ArgumentNullException(nameof(adjacentEdgesFilter));
        }

        /// <summary>
        /// Filter of adjacent edges.
        /// </summary>
        public Func<IEnumerable<TEdge>, IEnumerable<TEdge>> AdjacentEdgesFilter { get; }

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
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "Must be positive");
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
        /// Fired when the maximal authorized depth is reached.
        /// </summary>
        public event VertexAction<TVertex>? VertexMaxDepthReached;

        private void OnVertexMaxDepthReached(TVertex vertex)
        {
            VertexMaxDepthReached?.Invoke(vertex);
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
        public event UndirectedEdgeAction<TVertex, TEdge>? ExamineEdge;

        private void OnExamineEdge(TEdge edge, bool reversed)
        {
            ExamineEdge?.Invoke(
                this,
                new UndirectedEdgeEventArgs<TVertex, TEdge>(edge, reversed));
        }

        /// <summary>
        /// Fired when an edge is going to be treated when coming from a white vertex.
        /// </summary>
        public event UndirectedEdgeAction<TVertex, TEdge>? TreeEdge;

        private void OnTreeEdge(TEdge edge, bool reversed)
        {
            TreeEdge?.Invoke(
                this,
                new UndirectedEdgeEventArgs<TVertex, TEdge>(edge, reversed));
        }

        /// <summary>
        /// Fired when an edge is going to be treated when coming from a gray vertex.
        /// </summary>
        public event UndirectedEdgeAction<TVertex, TEdge>? BackEdge;

        private void OnBackEdge(TEdge edge, bool reversed)
        {
            BackEdge?.Invoke(
                this,
                new UndirectedEdgeEventArgs<TVertex, TEdge>(edge, reversed));
        }

        /// <summary>
        /// Fired when an edge is going to be treated when coming from a black vertex.
        /// </summary>
        public event UndirectedEdgeAction<TVertex, TEdge>? ForwardOrCrossEdge;

        private void OnForwardOrCrossEdge(TEdge edge, bool reversed)
        {
            ForwardOrCrossEdge?.Invoke(
                this,
                new UndirectedEdgeEventArgs<TVertex, TEdge>(edge, reversed));
        }

        /// <inheritdoc cref="IVertexTimeStamperAlgorithm{TVertex}" />
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
                Visit(root);

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
                        Visit(vertex);
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

        private struct SearchFrame
        {
            public TVertex Vertex { get; }

            public IEnumerator<TEdge> Edges { get; }

            public int Depth { get; }

            public SearchFrame(TVertex vertex, IEnumerator<TEdge> edges, int depth)
            {
                Debug.Assert(depth >= 0);

                Vertex = vertex;
                Edges = edges;
                Depth = depth;
            }
        }

        private void Visit(TVertex root)
        {
            var todoStack = new Stack<SearchFrame>();
            var visitedEdges = new Dictionary<TEdge, int>(VisitedGraph.EdgeCount);

            VerticesColors[root] = GraphColor.Gray;
            OnDiscoverVertex(root);

            IEnumerable<TEdge> enumerable = AdjacentEdgesFilter(VisitedGraph.AdjacentEdges(root));
            todoStack.Push(new SearchFrame(root, enumerable.GetEnumerator(), 0));

            while (todoStack.Count > 0)
            {
                ThrowIfCancellationRequested();

                SearchFrame frame = todoStack.Pop();
                TVertex u = frame.Vertex;
                int depth = frame.Depth;

                if (depth > MaxDepth)
                {
                    OnVertexMaxDepthReached(u);
                    VerticesColors[u] = GraphColor.Black;
                    OnVertexFinished(u);
                    continue;
                }

                IEnumerator<TEdge> edges = frame.Edges;
                while (edges.MoveNext())
                {
                    ThrowIfCancellationRequested();

                    TEdge edge = edges.Current;

                    // ReSharper disable once AssignNullToNotNullAttribute
                    // Justification: Enumerable items are not default so if the MoveNext succeed it can't be default
                    if (visitedEdges.ContainsKey(edge))
                        continue; // Edge already visited

                    visitedEdges.Add(edge, 0);
                    bool reversed = EqualityComparer<TVertex>.Default.Equals(edge.Target, u);
                    OnExamineEdge(edge, reversed);
                    TVertex v = reversed ? edge.Source : edge.Target;
                    GraphColor vColor = VerticesColors[v];
                    switch (vColor)
                    {
                        case GraphColor.White:
                            OnTreeEdge(edge, reversed);
                            todoStack.Push(new SearchFrame(u, edges, frame.Depth + 1));
                            u = v;
                            edges = AdjacentEdgesFilter(VisitedGraph.AdjacentEdges(u)).GetEnumerator();
                            VerticesColors[u] = GraphColor.Gray;
                            OnDiscoverVertex(u);
                            break;

                        case GraphColor.Gray:
                            OnBackEdge(edge, reversed);
                            break;

                        case GraphColor.Black:
                            OnForwardOrCrossEdge(edge, reversed);
                            break;
                    }
                }

                VerticesColors[u] = GraphColor.Black;
                OnVertexFinished(u);
            }
        }
    }
}
