#nullable enable

using FastGraph.Algorithms.Services;
using FastGraph.Collections;

namespace FastGraph.Algorithms.Search
{
    /// <summary>
    /// A breath first search algorithm for undirected graphs.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public sealed class UndirectedBreadthFirstSearchAlgorithm<TVertex, TEdge>
        : RootedAlgorithmBase<TVertex, IUndirectedGraph<TVertex, TEdge>>
        , IUndirectedVertexPredecessorRecorderAlgorithm<TVertex, TEdge>
        , IDistanceRecorderAlgorithm<TVertex>
        , IVertexColorizerAlgorithm<TVertex>
        where TVertex : notnull
        where TEdge : IEdge<TVertex>
    {
        private readonly IQueue<TVertex> _vertexQueue;

        /// <summary>
        /// Initializes a new instance of the <see cref="UndirectedBreadthFirstSearchAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        public UndirectedBreadthFirstSearchAlgorithm(
            IUndirectedGraph<TVertex, TEdge> visitedGraph)
            : this(visitedGraph, new Collections.Queue<TVertex>(), new Dictionary<TVertex, GraphColor>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UndirectedBreadthFirstSearchAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="vertexQueue">Queue of vertices to treat.</param>
        /// <param name="verticesColors">Vertices associated to their colors (treatment states).</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="vertexQueue"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="verticesColors"/> is <see langword="null"/>.</exception>
        public UndirectedBreadthFirstSearchAlgorithm(
            IUndirectedGraph<TVertex, TEdge> visitedGraph,
            IQueue<TVertex> vertexQueue,
            IDictionary<TVertex, GraphColor> verticesColors)
            : this(default, visitedGraph, vertexQueue, verticesColors)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UndirectedBreadthFirstSearchAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="vertexQueue">Queue of vertices to treat.</param>
        /// <param name="verticesColors">Vertices associated to their colors (treatment states).</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="vertexQueue"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="verticesColors"/> is <see langword="null"/>.</exception>
        public UndirectedBreadthFirstSearchAlgorithm(
            IAlgorithmComponent? host,
            IUndirectedGraph<TVertex, TEdge> visitedGraph,
            IQueue<TVertex> vertexQueue,
            IDictionary<TVertex, GraphColor> verticesColors)
            : base(host, visitedGraph)
        {
            VerticesColors = verticesColors ?? throw new ArgumentNullException(nameof(verticesColors));
            _vertexQueue = vertexQueue ?? throw new ArgumentNullException(nameof(vertexQueue));
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
        /// Fired when a vertex is going to be analyzed.
        /// </summary>
        public event VertexAction<TVertex>? ExamineVertex;

        private void OnExamineVertex(TVertex vertex)
        {
            ExamineVertex?.Invoke(vertex);
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
        public event UndirectedEdgeAction<TVertex, TEdge>? NonTreeEdge;

        private void OnNonTreeEdge(TEdge edge, bool reversed)
        {
            NonTreeEdge?.Invoke(
                this,
                new UndirectedEdgeEventArgs<TVertex, TEdge>(edge, reversed));
        }

        /// <summary>
        /// Fired when the target vertex of an out-edge from the currently treated vertex is marked as gray.
        /// </summary>
        public event UndirectedEdgeAction<TVertex, TEdge>? GrayTarget;

        private void OnGrayTarget(TEdge edge, bool reversed)
        {
            GrayTarget?.Invoke(
                this,
                new UndirectedEdgeEventArgs<TVertex, TEdge>(edge, reversed));
        }

        /// <summary>
        /// Fired when the target vertex of an out-edge from the currently treated vertex is marked as black.
        /// </summary>
        public event UndirectedEdgeAction<TVertex, TEdge>? BlackTarget;

        private void OnBlackTarget(TEdge edge, bool reversed)
        {
            BlackTarget?.Invoke(
                this,
                new UndirectedEdgeEventArgs<TVertex, TEdge>(edge, reversed));
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

            // Initialize vertices
            ThrowIfCancellationRequested();

            foreach (TVertex vertex in VisitedGraph.Vertices)
            {
                VerticesColors[vertex] = GraphColor.White;
                OnVertexInitialized(vertex);
            }
        }

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            TVertex root = GetAndAssertRootInGraph();
            EnqueueRoot(root);
            FlushVisitQueue();
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

        internal void Visit(TVertex root)
        {
            EnqueueRoot(root);
            FlushVisitQueue();
        }

        private void EnqueueRoot(TVertex root)
        {
            OnStartVertex(root);

            VerticesColors[root] = GraphColor.Gray;

            OnDiscoverVertex(root);
            _vertexQueue.Enqueue(root);
        }

        private void FlushVisitQueue()
        {
            while (_vertexQueue.Count > 0)
            {
                ThrowIfCancellationRequested();

                TVertex u = _vertexQueue.Dequeue();

                OnExamineVertex(u);

                ExploreAdjacentEdges(u);

                VerticesColors[u] = GraphColor.Black;
                OnVertexFinished(u);
            }
        }

        private void ExploreAdjacentEdges(TVertex u)
        {
            foreach (TEdge edge in VisitedGraph.AdjacentEdges(u))
            {
                bool reversed = EqualityComparer<TVertex?>.Default.Equals(edge.Target, u);
                TVertex v = reversed ? edge.Source : edge.Target;
                OnExamineEdge(edge);

                GraphColor vColor = VerticesColors[v];
                if (vColor == GraphColor.White)
                {
                    OnTreeEdge(edge, reversed);
                    VerticesColors[v] = GraphColor.Gray;
                    OnDiscoverVertex(v);
                    _vertexQueue.Enqueue(v);
                }
                else
                {
                    OnNonTreeEdge(edge, reversed);
                    if (vColor == GraphColor.Gray)
                    {
                        OnGrayTarget(edge, reversed);
                    }
                    else
                    {
                        OnBlackTarget(edge, reversed);
                    }
                }
            }
        }
    }
}
