#nullable enable

using System.Diagnostics;
using FastGraph.Algorithms.Services;

namespace FastGraph.Algorithms.Search
{
    /// <summary>
    /// A depth first search algorithm for directed graph.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public sealed class DepthFirstSearchAlgorithm<TVertex, TEdge>
        : RootedAlgorithmBase<TVertex, IVertexListGraph<TVertex, TEdge>>
        , IDistanceRecorderAlgorithm<TVertex>
        , IVertexColorizerAlgorithm<TVertex>
        , IVertexPredecessorRecorderAlgorithm<TVertex, TEdge>
        , IVertexTimeStamperAlgorithm<TVertex>
        where TVertex : notnull
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DepthFirstSearchAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        public DepthFirstSearchAlgorithm(IVertexListGraph<TVertex, TEdge> visitedGraph)
            : this(visitedGraph, new Dictionary<TVertex, GraphColor>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DepthFirstSearchAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="verticesColors">Vertices associated to their colors (treatment states).</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="verticesColors"/> is <see langword="null"/>.</exception>
        public DepthFirstSearchAlgorithm(
            IVertexListGraph<TVertex, TEdge> visitedGraph,
            IDictionary<TVertex, GraphColor> verticesColors)
            : this(default, visitedGraph, verticesColors)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DepthFirstSearchAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        public DepthFirstSearchAlgorithm(
            IAlgorithmComponent? host,
            IVertexListGraph<TVertex, TEdge> visitedGraph)
            : this(host, visitedGraph, new Dictionary<TVertex, GraphColor>(), edges => edges)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DepthFirstSearchAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="verticesColors">Vertices associated to their colors (treatment states).</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="verticesColors"/> is <see langword="null"/>.</exception>
        public DepthFirstSearchAlgorithm(
            IAlgorithmComponent? host,
            IVertexListGraph<TVertex, TEdge> visitedGraph,
            IDictionary<TVertex, GraphColor> verticesColors)
            : this(host, visitedGraph, verticesColors, edges => edges)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DepthFirstSearchAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="verticesColors">Vertices associated to their colors (treatment states).</param>
        /// <param name="outEdgesFilter">
        /// Delegate that takes the enumeration of out-edges and filters/reorders
        /// them. All vertices passed to the method should be enumerated once and only once.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="verticesColors"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="outEdgesFilter"/> is <see langword="null"/>.</exception>
        public DepthFirstSearchAlgorithm(
            IAlgorithmComponent? host,
            IVertexListGraph<TVertex, TEdge> visitedGraph,
            IDictionary<TVertex, GraphColor> verticesColors,
            Func<IEnumerable<TEdge>, IEnumerable<TEdge>> outEdgesFilter)
            : base(host, visitedGraph)
        {
            VerticesColors = verticesColors ?? throw new ArgumentNullException(nameof(verticesColors));
            OutEdgesFilter = outEdgesFilter ?? throw new ArgumentNullException(nameof(outEdgesFilter));
        }

        /// <summary>
        /// Filter of edges.
        /// </summary>
        public Func<IEnumerable<TEdge>, IEnumerable<TEdge>> OutEdgesFilter { get; }

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
                Debug.Assert(depth >= 0, "Must be positive.");

                Vertex = vertex;
                Edges = edges;
                Depth = depth;
            }
        }

        private void Visit(TVertex root)
        {
            var todoStack = new Stack<SearchFrame>();
            VerticesColors[root] = GraphColor.Gray;
            OnDiscoverVertex(root);

            IEnumerable<TEdge> enumerable = OutEdgesFilter(VisitedGraph.OutEdges(root));
            todoStack.Push(new SearchFrame(root, enumerable.GetEnumerator(), 0));

            while (todoStack.Count > 0)
            {
                ThrowIfCancellationRequested();

                SearchFrame frame = todoStack.Pop();
                TVertex u = frame.Vertex;
                int depth = frame.Depth;
                IEnumerator<TEdge> edges = frame.Edges;

                if (depth > MaxDepth)
                {
                    edges.Dispose();
                    VerticesColors[u] = GraphColor.Black;
                    OnVertexFinished(u);
                    continue;
                }

                while (edges.MoveNext())
                {
                    ThrowIfCancellationRequested();

                    TEdge edge = edges.Current;

                    // ReSharper disable once AssignNullToNotNullAttribute
                    // Justification: Enumerable items are not default so if the MoveNext succeed it can't be default
                    OnExamineEdge(edge);
                    TVertex v = edge.Target;
                    GraphColor vColor = VerticesColors[v];
                    switch (vColor)
                    {
                        case GraphColor.White:
                            OnTreeEdge(edge);
                            todoStack.Push(new SearchFrame(u, edges, depth));
                            u = v;
                            edges = OutEdgesFilter(VisitedGraph.OutEdges(u)).GetEnumerator();
                            ++depth;
                            VerticesColors[u] = GraphColor.Gray;
                            OnDiscoverVertex(u);
                            break;

                        case GraphColor.Gray:
                            OnBackEdge(edge);
                            break;

                        case GraphColor.Black:
                            OnForwardOrCrossEdge(edge);
                            break;
                    }
                }

                edges.Dispose();

                VerticesColors[u] = GraphColor.Black;
                OnVertexFinished(u);
            }
        }
    }
}
