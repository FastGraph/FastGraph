#nullable enable

using System.Diagnostics;
using FastGraph.Algorithms.Services;

namespace FastGraph.Algorithms.Search
{
    /// <summary>
    /// An edge depth first search algorithm for implicit directed graphs.
    /// </summary>
    /// <remarks>
    /// This is a variant of the classic DFS where the edges are color marked.
    /// </remarks>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public sealed class ImplicitEdgeDepthFirstSearchAlgorithm<TVertex, TEdge>
        : RootedAlgorithmBase<TVertex, IIncidenceGraph<TVertex, TEdge>>
        , IEdgeColorizerAlgorithm<TVertex, TEdge>
        , ITreeBuilderAlgorithm<TVertex, TEdge>
        where TVertex : notnull
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImplicitEdgeDepthFirstSearchAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        public ImplicitEdgeDepthFirstSearchAlgorithm(
            IIncidenceGraph<TVertex, TEdge> visitedGraph)
            : this(default, visitedGraph)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImplicitEdgeDepthFirstSearchAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        public ImplicitEdgeDepthFirstSearchAlgorithm(
            IAlgorithmComponent? host,
            IIncidenceGraph<TVertex, TEdge> visitedGraph)
            : base(host, visitedGraph)
        {
        }

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

        /// <summary>
        /// Fired when an edge is discovered.
        /// </summary>
        public event EdgeEdgeAction<TVertex, TEdge>? DiscoverTreeEdge;

        private void OnDiscoverTreeEdge(TEdge edge, TEdge targetEdge)
        {
            DiscoverTreeEdge?.Invoke(edge, targetEdge);
        }

        /// <summary>
        /// Invoked on each edge as it becomes a member of the edges that form
        /// the search tree. If you wish to record predecessors, do so at this
        /// event point.
        /// </summary>
        public event EdgeAction<TVertex, TEdge>? TreeEdge;

        private void OnTreeEdge(TEdge edge)
        {
            TreeEdge?.Invoke(edge);
        }

        /// <summary>
        /// Fired on the back edges in the graph.
        /// </summary>
        public event EdgeAction<TVertex, TEdge>? BackEdge;

        private void OnBackEdge(TEdge edge)
        {
            BackEdge?.Invoke(edge);
        }

        /// <summary>
        /// Fired on forward or cross edges in the graph.
        /// (In an undirected graph this method is never called.)
        /// </summary>
        public event EdgeAction<TVertex, TEdge>? ForwardOrCrossEdge;

        private void OnForwardOrCrossEdge(TEdge edge)
        {
            ForwardOrCrossEdge?.Invoke(edge);
        }

        /// <summary>
        /// Fired on an edge after all of its out edges have been added to
        /// the search tree and all of the adjacent vertices have been
        /// discovered (but before their out-edges have been examined).
        /// </summary>
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
            base.Initialize();

            EdgesColors.Clear();
        }

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            TVertex root = GetAndAssertRootInGraph();

            // Start with root vertex
            OnStartVertex(root);

            // Process each out edge of the root one
            foreach (TEdge edge in VisitedGraph.OutEdges(root))
            {
                ThrowIfCancellationRequested();

                if (!EdgesColors.ContainsKey(edge))
                {
                    OnStartEdge(edge);
                    Visit(edge, 0);
                }
            }
        }

        #endregion

        #region IEdgeColorizerAlgorithm<TVertex,TEdge>

        /// <inheritdoc />
        public IDictionary<TEdge, GraphColor> EdgesColors { get; } = new Dictionary<TEdge, GraphColor>();

        #endregion

        private void Visit(TEdge startingEdge, int depth)
        {
            Debug.Assert(depth >= 0);

            if (depth > MaxDepth)
                return;

            // Mark edge as gray
            EdgesColors[startingEdge] = GraphColor.Gray;
            // Add edge to the search tree
            OnTreeEdge(startingEdge);

            // Iterate over out-edges
            foreach (TEdge edge in VisitedGraph.OutEdges(startingEdge.Target))
            {
                ThrowIfCancellationRequested();

                // Check edge is not explored yet,
                // if not, explore it
                if (!EdgesColors.TryGetValue(edge, out GraphColor color))
                {
                    OnDiscoverTreeEdge(startingEdge, edge);
                    Visit(edge, depth + 1);
                }
                else
                {
                    if (color == GraphColor.Gray)
                    {
                        OnBackEdge(edge);
                    }
                    else
                    {
                        OnForwardOrCrossEdge(edge);
                    }
                }
            }

            // All out-edges have been explored
            EdgesColors[startingEdge] = GraphColor.Black;
            OnFinishEdge(startingEdge);
        }
    }
}
