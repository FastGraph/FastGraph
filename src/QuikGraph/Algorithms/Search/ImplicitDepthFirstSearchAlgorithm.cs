using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using QuikGraph.Algorithms.Services;

namespace QuikGraph.Algorithms.Search
{
    /// <summary>
    /// A depth first search algorithm for implicit directed graphs.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class ImplicitDepthFirstSearchAlgorithm<TVertex, TEdge>
        : RootedAlgorithmBase<TVertex, IIncidenceGraph<TVertex, TEdge>>
        , IVertexPredecessorRecorderAlgorithm<TVertex, TEdge>
        , IVertexTimeStamperAlgorithm<TVertex>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImplicitDepthFirstSearchAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        public ImplicitDepthFirstSearchAlgorithm(
            [NotNull] IIncidenceGraph<TVertex, TEdge> visitedGraph)
            : this(null, visitedGraph)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImplicitDepthFirstSearchAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        public ImplicitDepthFirstSearchAlgorithm(
            [CanBeNull] IAlgorithmComponent host,
            [NotNull] IIncidenceGraph<TVertex, TEdge> visitedGraph)
            : base(host, visitedGraph)
        {
        }

        /// <summary>
        /// Stores vertices associated to their colors (treatment state).
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        public IDictionary<TVertex, GraphColor> VertexColors { get; } = new Dictionary<TVertex, GraphColor>();

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

        /// <summary>
        /// Fired on the root vertex once before the start of the search from it.
        /// </summary>
        public event VertexAction<TVertex> StartVertex;

        private void OnStartVertex([NotNull] TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            StartVertex?.Invoke(vertex);
        }

        /// <summary>
        /// Invoked when a vertex is encountered for the first time. 
        /// </summary>
        public event VertexAction<TVertex> DiscoverVertex;

        private void OnDiscoverVertex([NotNull] TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            DiscoverVertex?.Invoke(vertex);
        }

        /// <summary>
        /// Invoked on every out-edge of each vertex after it is discovered. 
        /// </summary>
        public event EdgeAction<TVertex, TEdge> ExamineEdge;

        private void OnExamineEdge([NotNull] TEdge edge)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            ExamineEdge?.Invoke(edge);
        }

        /// <summary>
        /// Fired on each edge as it becomes a member of the edges that form
        /// the search tree. If you wish to record predecessors, do so at this
        /// event point.
        /// </summary>
        public event EdgeAction<TVertex, TEdge> TreeEdge;

        private void OnTreeEdge([NotNull] TEdge edge)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            TreeEdge?.Invoke(edge);
        }

        /// <summary>
        /// Fired on the back edges in the graph.
        /// </summary>
        public event EdgeAction<TVertex, TEdge> BackEdge;

        private void OnBackEdge([NotNull] TEdge edge)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            BackEdge?.Invoke(edge);
        }

        /// <summary>
        /// Fired on forward or cross edges in the graph.
        /// (In an undirected graph this method is never called.)
        /// </summary>
        public event EdgeAction<TVertex, TEdge> ForwardOrCrossEdge;

        private void OnForwardOrCrossEdge([NotNull] TEdge edge)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            ForwardOrCrossEdge?.Invoke(edge);
        }

        /// <summary>
        /// Fired on a vertex after all of its out edges have been added to
        /// the search tree and all of the adjacent vertices have been
        /// discovered (but before their out-edges have been examined).
        /// </summary>
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

            VertexColors.Clear();
        }

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            if (!TryGetRootVertex(out TVertex root))
                throw new InvalidOperationException("Root vertex not set.");

            OnStartVertex(root);
            Visit(root, 0);
        }

        #endregion

        private void Visit([NotNull] TVertex u, int depth)
        {
            if (u == null)
                throw new ArgumentNullException(nameof(u));

            if (depth > MaxDepth)
                return;

            VertexColors[u] = GraphColor.Gray;
            OnDiscoverVertex(u);

            ICancelManager cancelManager = Services.CancelManager;
            foreach (TEdge edge in VisitedGraph.OutEdges(u))
            {
                if (cancelManager.IsCancelling)
                    return;

                OnExamineEdge(edge);
                TVertex v = edge.Target;

                if (!VertexColors.TryGetValue(v, out GraphColor vColor))
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

            VertexColors[u] = GraphColor.Black;
            OnVertexFinished(u);
        }
    }
}