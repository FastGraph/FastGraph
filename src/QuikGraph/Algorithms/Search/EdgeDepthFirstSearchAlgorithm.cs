#if SUPPORTS_SERIALIZATION
using System;
#endif
using System.Collections.Generic;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using JetBrains.Annotations;
using QuikGraph.Algorithms.Services;

namespace QuikGraph.Algorithms.Search
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
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class EdgeDepthFirstSearchAlgorithm<TVertex, TEdge> 
        : RootedAlgorithmBase<TVertex, IEdgeListAndIncidenceGraph<TVertex, TEdge>>
        , IEdgeColorizerAlgorithm<TVertex, TEdge>
        , IEdgePredecessorRecorderAlgorithm<TVertex, TEdge>
        , ITreeBuilderAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EdgeDepthFirstSearchAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        public EdgeDepthFirstSearchAlgorithm(
            [NotNull] IEdgeListAndIncidenceGraph<TVertex, TEdge> visitedGraph)
            : this(visitedGraph, new Dictionary<TEdge, GraphColor>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EdgeDepthFirstSearchAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="edgesColors">Edges associated to their colors (treatment states).</param>
        public EdgeDepthFirstSearchAlgorithm(
            [NotNull] IEdgeListAndIncidenceGraph<TVertex, TEdge> visitedGraph,
            [NotNull] IDictionary<TEdge, GraphColor> edgesColors)
            : this(null, visitedGraph, edgesColors)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EdgeDepthFirstSearchAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="edgesColors">Edges associated to their colors (treatment states).</param>
        public EdgeDepthFirstSearchAlgorithm(
            [CanBeNull] IAlgorithmComponent host,
            [NotNull] IEdgeListAndIncidenceGraph<TVertex, TEdge> visitedGraph,
            [NotNull] IDictionary<TEdge, GraphColor> edgesColors)
            : base(host, visitedGraph)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(edgesColors != null);
#endif

            EdgesColors = edgesColors;
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

        /// <summary>
        /// Fired when an edge is initialized.
        /// </summary>
        public event EdgeAction<TVertex, TEdge> InitializeEdge;

        private void OnEdgeInitialized([NotNull] TEdge edge)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(edge != null);
#endif

            InitializeEdge?.Invoke(edge);
        }

        /// <summary>
        /// Fired on the root vertex once before the start of the search from it.
        /// </summary>
        public event VertexAction<TVertex> StartVertex;

        private void OnStartVertex([NotNull] TVertex vertex)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(vertex != null);
#endif

            StartVertex?.Invoke(vertex);
        }

        /// <summary>
        /// Fired when an edge starts to be treated.
        /// </summary>
        public event EdgeAction<TVertex, TEdge> StartEdge;

        private void OnStartEdge([NotNull] TEdge edge)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(edge != null);
#endif

            StartEdge?.Invoke(edge);
        }

        /// <inheritdoc />
        public event EdgeEdgeAction<TVertex, TEdge> DiscoverTreeEdge;

        private void OnDiscoverTreeEdge([NotNull] TEdge edge, [NotNull] TEdge targetEdge)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(edge != null);
            Contract.Requires(targetEdge != null);
#endif

            DiscoverTreeEdge?.Invoke(edge, targetEdge);
        }

        /// <summary>
        /// Fired when an edge is going to be treated when coming from a white edge.
        /// </summary>
        public event EdgeAction<TVertex, TEdge> TreeEdge;

        private void OnTreeEdge([NotNull] TEdge edge)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(edge != null);
#endif

            TreeEdge?.Invoke(edge);
        }

        /// <summary>
        /// Fired when an edge is going to be treated when coming from a gray edge.
        /// </summary>
        public event EdgeAction<TVertex, TEdge> BackEdge;

        private void OnBackEdge([NotNull] TEdge edge)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(edge != null);
#endif

            BackEdge?.Invoke(edge);
        }

        /// <summary>
        /// Fired when an edge is going to be treated when coming from a black edge.
        /// </summary>
        public event EdgeAction<TVertex, TEdge> ForwardOrCrossEdge;

        private void OnForwardOrCrossEdge([NotNull] TEdge edge)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(edge != null);
#endif

            ForwardOrCrossEdge?.Invoke(edge);
        }

        /// <inheritdoc />
        public event EdgeAction<TVertex, TEdge> FinishEdge;

        private void OnFinishEdge([NotNull] TEdge edge)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(edge != null);
#endif

            FinishEdge?.Invoke(edge);
        }

        #endregion

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void Initialize()
        {
            // Put all edges to white
            ICancelManager cancelManager = Services.CancelManager;
            foreach (TEdge edge in VisitedGraph.Edges)
            {
                if (cancelManager.IsCancelling)
                    return;

                EdgesColors[edge] = GraphColor.White;
                OnEdgeInitialized(edge);
            }
        }

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            ICancelManager cancelManager = Services.CancelManager;
            if (cancelManager.IsCancelling)
                return;

            // Start with root vertex
            if (TryGetRootVertex(out TVertex root))
            {
                OnStartVertex(root);

                // Process each out edge of the root one
                foreach (TEdge edge in VisitedGraph.OutEdges(root))
                {
                    if (cancelManager.IsCancelling)
                        return;

                    if (EdgesColors[edge] == GraphColor.White)
                    {
                        OnStartEdge(edge);
                        Visit(edge, 0);
                    }
                }
            }

            // Process the rest of the graph edges
            foreach (TEdge edge in VisitedGraph.Edges)
            {
                if (cancelManager.IsCancelling)
                    return;

                if (EdgesColors[edge] == GraphColor.White)
                {
                    OnStartEdge(edge);
                    Visit(edge, 0);
                }
            }
        }

        #endregion

        #region IEdgeColorizerAlgorithm<TVertex,TEdge>

        /// <inheritdoc />
        public IDictionary<TEdge, GraphColor> EdgesColors { get; }

        #endregion

        private void Visit([NotNull] TEdge rootEdge, int depth)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(rootEdge != null);
#endif

            if (depth > MaxDepth)
                return;

            ICancelManager cancelManager = Services.CancelManager;

            // Mark edge as gray
            EdgesColors[rootEdge] = GraphColor.Gray;
            // Add edge to the search tree
            OnTreeEdge(rootEdge);

            // Iterate over out-edges
            foreach (TEdge edge in VisitedGraph.OutEdges(rootEdge.Target))
            {
                if (cancelManager.IsCancelling)
                    return;

                // Check edge is not explored yet,
                // If not, explore it
                if (EdgesColors[edge] == GraphColor.White)
                {
                    OnDiscoverTreeEdge(rootEdge, edge);
                    Visit(edge, depth + 1);
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
