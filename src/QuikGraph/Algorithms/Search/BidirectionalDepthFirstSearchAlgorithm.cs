using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using QuikGraph.Algorithms.Services;

namespace QuikGraph.Algorithms.Search
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
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class BidirectionalDepthFirstSearchAlgorithm<TVertex, TEdge>
        : RootedAlgorithmBase<TVertex, IBidirectionalGraph<TVertex, TEdge>>
        , IVertexPredecessorRecorderAlgorithm<TVertex, TEdge>
        , IDistanceRecorderAlgorithm<TVertex>
        , IVertexColorizerAlgorithm<TVertex>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BidirectionalDepthFirstSearchAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        public BidirectionalDepthFirstSearchAlgorithm(
            [NotNull] IBidirectionalGraph<TVertex, TEdge> visitedGraph)
            : this(visitedGraph, new Dictionary<TVertex, GraphColor>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BidirectionalDepthFirstSearchAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="verticesColors">Vertices associated to their colors (treatment states).</param>
        public BidirectionalDepthFirstSearchAlgorithm(
            [NotNull] IBidirectionalGraph<TVertex, TEdge> visitedGraph,
            [NotNull] IDictionary<TVertex, GraphColor> verticesColors)
            : this(null, visitedGraph, verticesColors)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BidirectionalDepthFirstSearchAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="verticesColors">Vertices associated to their colors (treatment states).</param>
        public BidirectionalDepthFirstSearchAlgorithm(
            [CanBeNull] IAlgorithmComponent host,
            [NotNull] IBidirectionalGraph<TVertex, TEdge> visitedGraph,
            [NotNull] IDictionary<TVertex, GraphColor> verticesColors)
            : base(host, visitedGraph)
        {
            if (verticesColors is null)
                throw new ArgumentNullException(nameof(verticesColors));

            VerticesColors = verticesColors;
        }

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
        public int MaxDepth { get; set; } = int.MaxValue;

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

        /// <inheritdoc />
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
            // If there is a starting vertex, start with him
            if (TryGetRootVertex(out TVertex root))
            {
                OnStartVertex(root);
                Visit(root, 0);
            }

            // Process each vertex 
            ICancelManager cancelManager = Services.CancelManager;
            foreach (TVertex vertex in VisitedGraph.Vertices)
            {
                if (cancelManager.IsCancelling)
                    return;

                if (VerticesColors[vertex] == GraphColor.White)
                {
                    OnStartVertex(vertex);
                    Visit(vertex, 0);
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

        private void Visit([NotNull] TVertex u, int depth)
        {
            if (u == null)
                throw new ArgumentNullException(nameof(u));

            if (depth > MaxDepth)
                return;

            VerticesColors[u] = GraphColor.Gray;
            OnDiscoverVertex(u);

            ICancelManager cancelManager = Services.CancelManager;
            foreach (TEdge edge in VisitedGraph.OutEdges(u))
            {
                if (cancelManager.IsCancelling)
                    return;

                OnExamineEdge(edge);
                TVertex v = edge.Target;
                ProcessEdge(depth, v, edge);
            }

            foreach (TEdge edge in VisitedGraph.InEdges(u))
            {
                if (cancelManager.IsCancelling)
                    return;

                OnExamineEdge(edge);
                TVertex v = edge.Source;
                ProcessEdge(depth, v, edge);
            }

            VerticesColors[u] = GraphColor.Black;
            OnVertexFinished(u);
        }

        private void ProcessEdge(int depth, [NotNull] TVertex vertex, [NotNull] TEdge edge)
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
