using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using QuikGraph.Algorithms.Services;

namespace QuikGraph.Algorithms.Search
{
    /// <summary>
    /// A depth first search algorithm for directed graph.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class DepthFirstSearchAlgorithm<TVertex, TEdge>
        : RootedAlgorithmBase<TVertex, IVertexListGraph<TVertex, TEdge>>
        , IDistanceRecorderAlgorithm<TVertex>
        , IVertexColorizerAlgorithm<TVertex>
        , IVertexPredecessorRecorderAlgorithm<TVertex, TEdge>
        , IVertexTimeStamperAlgorithm<TVertex>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DepthFirstSearchAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        public DepthFirstSearchAlgorithm([NotNull] IVertexListGraph<TVertex, TEdge> visitedGraph)
            : this(visitedGraph, new Dictionary<TVertex, GraphColor>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DepthFirstSearchAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="verticesColors">Vertices associated to their colors (treatment states).</param>
        public DepthFirstSearchAlgorithm(
            [NotNull] IVertexListGraph<TVertex, TEdge> visitedGraph,
            [NotNull] IDictionary<TVertex, GraphColor> verticesColors)
            : this(null, visitedGraph, verticesColors)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DepthFirstSearchAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        public DepthFirstSearchAlgorithm(
            [CanBeNull] IAlgorithmComponent host,
            [NotNull] IVertexListGraph<TVertex, TEdge> visitedGraph)
            : this(host, visitedGraph, new Dictionary<TVertex, GraphColor>(), edges => edges)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DepthFirstSearchAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="verticesColors">Vertices associated to their colors (treatment states).</param>
        public DepthFirstSearchAlgorithm(
            [CanBeNull] IAlgorithmComponent host,
            [NotNull] IVertexListGraph<TVertex, TEdge> visitedGraph,
            [NotNull] IDictionary<TVertex, GraphColor> verticesColors)
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
        public DepthFirstSearchAlgorithm(
            IAlgorithmComponent host,
            IVertexListGraph<TVertex, TEdge> visitedGraph,
            IDictionary<TVertex, GraphColor> verticesColors,
            Func<IEnumerable<TEdge>, IEnumerable<TEdge>> outEdgesFilter)
            : base(host, visitedGraph)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(verticesColors != null);
            Contract.Requires(outEdgesFilter != null);
#endif

            VerticesColors = verticesColors;
            OutEdgesFilter = outEdgesFilter;
        }

        /// <summary>
        /// Filter of edges.
        /// </summary>
        [NotNull]
        public Func<IEnumerable<TEdge>, IEnumerable<TEdge>> OutEdgesFilter { get; }

        private int _maxDepth = int.MaxValue;

        /// <summary>
        /// Gets or sets the maximum exploration depth, from the start vertex.
        /// </summary>
        /// <remarks>
        /// Defaulted at <see cref="int.MaxValue"/>.
        /// </remarks>
        /// <value>
        /// Maximum exploration depth.
        /// </value>
        public int MaxDepth
        {
            get => _maxDepth;
            set
            {
#if SUPPORTS_CONTRACTS
                Contract.Requires(value > 0);
#endif
                _maxDepth = value;
            }
        }

        #region Events

        /// <inheritdoc />
        public event VertexAction<TVertex> InitializeVertex;

        private void OnVertexInitialized([NotNull] TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            InitializeVertex?.Invoke(vertex);
        }

        /// <inheritdoc />
        public event VertexAction<TVertex> StartVertex;

        private void OnStartVertex([NotNull] TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            StartVertex?.Invoke(vertex);
        }

        /// <summary>
        /// Fired when a vertex is discovered and under treatment.
        /// </summary>
        public event VertexAction<TVertex> DiscoverVertex;

        private void OnDiscoverVertex([NotNull] TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            DiscoverVertex?.Invoke(vertex);
        }

        /// <summary>
        /// Fired when an edge is going to be analyzed.
        /// </summary>
        public event EdgeAction<TVertex, TEdge> ExamineEdge;

        private void OnExamineEdge([NotNull] TEdge edge)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            ExamineEdge?.Invoke(edge);
        }

        /// <summary>
        /// Fired when an edge is going to be treated when coming from a white vertex.
        /// </summary>
        public event EdgeAction<TVertex, TEdge> TreeEdge;

        private void OnTreeEdge([NotNull] TEdge edge)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            TreeEdge?.Invoke(edge);
        }

        /// <summary>
        /// Fired when an edge is going to be treated when coming from a gray vertex.
        /// </summary>
        public event EdgeAction<TVertex, TEdge> BackEdge;

        private void OnBackEdge([NotNull] TEdge edge)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            BackEdge?.Invoke(edge);
        }

        /// <summary>
        /// Fired when an edge is going to be treated when coming from a black vertex.
        /// </summary>
        public event EdgeAction<TVertex, TEdge> ForwardOrCrossEdge;

        private void OnForwardOrCrossEdge([NotNull] TEdge edge)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            ForwardOrCrossEdge?.Invoke(edge);
        }

        /// <inheritdoc cref="IVertexTimeStamperAlgorithm{TVertex}" />
        public event VertexAction<TVertex> FinishVertex;

        private void OnVertexFinished([NotNull] TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

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
            // If there is a starting vertex, start with him
            if (TryGetRootVertex(out TVertex root))
            {
                OnStartVertex(root);
                Visit(root);
            }
            else
            {
                ICancelManager cancelManager = Services.CancelManager;

                // Process each vertex 
                foreach (TVertex vertex in VisitedGraph.Vertices)
                {
                    if (cancelManager.IsCancelling)
                        return;

                    if (VerticesColors[vertex] == GraphColor.White)
                    {
                        OnStartVertex(vertex);
                        Visit(vertex);
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Stores vertices associated to their colors (treatment state).
        /// </summary>
        [NotNull]
        public IDictionary<TVertex, GraphColor> VerticesColors { get; }

        #region IVertexColorizerAlgorithm<TVertex>

        /// <inheritdoc />
        public GraphColor GetVertexColor(TVertex vertex)
        {
            return VerticesColors[vertex];
        }

        #endregion

        private struct SearchFrame
        {
            [NotNull]
            public TVertex Vertex { get; }

            [NotNull]
            public IEnumerator<TEdge> Edges { get; }

            public int Depth { get; }

            public SearchFrame([NotNull] TVertex vertex, [NotNull] IEnumerator<TEdge> edges, int depth)
            {
#if SUPPORTS_CONTRACTS
                Contract.Requires(vertex != null);
                Contract.Requires(edges != null);
                Contract.Requires(depth >= 0);
#endif

                Vertex = vertex;
                Edges = edges;
                Depth = depth;
            }
        }

        private void Visit([NotNull] TVertex root)
        {
            if (root == null)
                throw new ArgumentNullException(nameof(root));

            var todoStack = new Stack<SearchFrame>();
            VerticesColors[root] = GraphColor.Gray;
            OnDiscoverVertex(root);

            ICancelManager cancelManager = Services.CancelManager;
            var enumerable = OutEdgesFilter(VisitedGraph.OutEdges(root));
            todoStack.Push(new SearchFrame(root, enumerable.GetEnumerator(), 0));

            while (todoStack.Count > 0)
            {
                if (cancelManager.IsCancelling)
                    return;

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
                    TEdge edge = edges.Current;
                    if (cancelManager.IsCancelling)
                        return;

                    // ReSharper disable once AssignNullToNotNullAttribute
                    // Justification: Enumerable items are not null so if the MoveNext succeed it can't be null
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
