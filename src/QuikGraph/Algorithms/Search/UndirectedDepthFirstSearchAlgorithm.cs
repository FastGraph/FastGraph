using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using QuikGraph.Algorithms.Services;

namespace QuikGraph.Algorithms.Search
{
    /// <summary>
    /// A depth first search algorithm for undirected graph.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class UndirectedDepthFirstSearchAlgorithm<TVertex, TEdge>
        : RootedAlgorithmBase<TVertex, IUndirectedGraph<TVertex, TEdge>>
        , IDistanceRecorderAlgorithm<TVertex>
        , IVertexColorizerAlgorithm<TVertex>
        , IUndirectedVertexPredecessorRecorderAlgorithm<TVertex, TEdge>
        , IVertexTimeStamperAlgorithm<TVertex>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UndirectedDepthFirstSearchAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        public UndirectedDepthFirstSearchAlgorithm(
            [NotNull] IUndirectedGraph<TVertex, TEdge> visitedGraph)
            : this(visitedGraph, new Dictionary<TVertex, GraphColor>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UndirectedDepthFirstSearchAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="verticesColors">Vertices associated to their colors (treatment states).</param>
        public UndirectedDepthFirstSearchAlgorithm(
            [NotNull] IUndirectedGraph<TVertex, TEdge> visitedGraph,
            [NotNull] IDictionary<TVertex, GraphColor> verticesColors)
            : this(null, visitedGraph, verticesColors)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UndirectedDepthFirstSearchAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="verticesColors">Vertices associated to their colors (treatment states).</param>
        public UndirectedDepthFirstSearchAlgorithm(
            [CanBeNull] IAlgorithmComponent host,
            [NotNull] IUndirectedGraph<TVertex, TEdge> visitedGraph,
            [NotNull] IDictionary<TVertex, GraphColor> verticesColors)
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
        public UndirectedDepthFirstSearchAlgorithm(
            [CanBeNull] IAlgorithmComponent host,
            [NotNull] IUndirectedGraph<TVertex, TEdge> visitedGraph,
            [NotNull] IDictionary<TVertex, GraphColor> verticesColors,
            [NotNull] Func<IEnumerable<TEdge>, IEnumerable<TEdge>> adjacentEdgesFilter)
            : base(host, visitedGraph)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(verticesColors != null);
            Contract.Requires(adjacentEdgesFilter != null);
#endif

            VerticesColors = verticesColors;
            AdjacentEdgesFilter = adjacentEdgesFilter;
        }

        /// <summary>
        /// Filter of adjacent edges.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        public Func<IEnumerable<TEdge>, IEnumerable<TEdge>> AdjacentEdgesFilter { get; }

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
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
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
#if SUPPORTS_CONTRACTS
            Contract.Requires(vertex != null);
#endif

            InitializeVertex?.Invoke(vertex);
        }

        /// <inheritdoc />
        public event VertexAction<TVertex> StartVertex;

        private void OnStartVertex([NotNull] TVertex vertex)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(vertex != null);
#endif

            StartVertex?.Invoke(vertex);
        }

        /// <summary>
        /// Fired when the maximal authorized depth is reached.
        /// </summary>
        public event VertexAction<TVertex> VertexMaxDepthReached;

        private void OnVertexMaxDepthReached([NotNull] TVertex vertex)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(vertex != null);
#endif

            VertexMaxDepthReached?.Invoke(vertex);
        }

        /// <summary>
        /// Fired when a vertex is discovered and under treatment.
        /// </summary>
        public event VertexAction<TVertex> DiscoverVertex;

        private void OnDiscoverVertex([NotNull] TVertex vertex)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(vertex != null);
#endif

            DiscoverVertex?.Invoke(vertex);
        }

        /// <summary>
        /// Fired when an edge is going to be analyzed.
        /// </summary>
        public event UndirectedEdgeAction<TVertex, TEdge> ExamineEdge;

        private void OnExamineEdge([NotNull] TEdge edge, bool reversed)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(edge != null);
#endif

            ExamineEdge?.Invoke(
                this,
                new UndirectedEdgeEventArgs<TVertex, TEdge>(edge, reversed));
        }

        /// <summary>
        /// Fired when an edge is going to be treated when coming from a white vertex.
        /// </summary>
        public event UndirectedEdgeAction<TVertex, TEdge> TreeEdge;

        private void OnTreeEdge([NotNull] TEdge edge, bool reversed)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(edge != null);
#endif

            TreeEdge?.Invoke(
                this,
                new UndirectedEdgeEventArgs<TVertex, TEdge>(edge, reversed));
        }

        /// <summary>
        /// Fired when an edge is going to be treated when coming from a gray vertex.
        /// </summary>
        public event UndirectedEdgeAction<TVertex, TEdge> BackEdge;

        private void OnBackEdge([NotNull] TEdge edge, bool reversed)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(edge != null);
#endif

            BackEdge?.Invoke(
                this,
                new UndirectedEdgeEventArgs<TVertex, TEdge>(edge, reversed));
        }

        /// <summary>
        /// Fired when an edge is going to be treated when coming from a black vertex.
        /// </summary>
        public event UndirectedEdgeAction<TVertex, TEdge> ForwardOrCrossEdge;

        private void OnForwardOrCrossEdge([NotNull] TEdge edge, bool reversed)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(edge != null);
#endif

            ForwardOrCrossEdge?.Invoke(
                this,
                new UndirectedEdgeEventArgs<TVertex, TEdge>(edge, reversed));
        }

        /// <inheritdoc cref="IVertexTimeStamperAlgorithm{TVertex}" />
        public event VertexAction<TVertex> FinishVertex;

        private void OnVertexFinished([NotNull] TVertex vertex)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(vertex != null);
#endif

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
            if (TryGetRootVertex(out TVertex rootVertex))
            {
                OnStartVertex(rootVertex);
                Visit(rootVertex);
            }
            else
            {
                ICancelManager cancelManager = Services.CancelManager;

                // Process each vertex 
                foreach (TVertex root in VisitedGraph.Vertices)
                {
                    if (cancelManager.IsCancelling)
                        return;

                    if (VerticesColors[root] == GraphColor.White)
                    {
                        OnStartVertex(root);
                        Visit(root);
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Stores vertices associated to their colors (treatment state).
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
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
#if SUPPORTS_CONTRACTS
            Contract.Requires(root != null);
#endif

            var todoStack = new Stack<SearchFrame>();
            var visitedEdges = new Dictionary<TEdge, int>(VisitedGraph.EdgeCount);

            VerticesColors[root] = GraphColor.Gray;
            OnDiscoverVertex(root);

            ICancelManager cancelManager = Services.CancelManager;
            IEnumerable<TEdge> enumerable = AdjacentEdgesFilter(VisitedGraph.AdjacentEdges(root));
            todoStack.Push(new SearchFrame(root, enumerable.GetEnumerator(), 0));

            while (todoStack.Count > 0)
            {
                if (cancelManager.IsCancelling)
                    return;

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
                    TEdge edge = edges.Current;
                    if (cancelManager.IsCancelling)
                        return;

                    // ReSharper disable once AssignNullToNotNullAttribute
                    // Justification: Enumerable items are not null so if the MoveNext succeed it can't be null
                    if (visitedEdges.ContainsKey(edge))
                        continue; // Edge already visited

                    visitedEdges.Add(edge, 0);
                    bool reversed = edge.Target.Equals(u);
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
