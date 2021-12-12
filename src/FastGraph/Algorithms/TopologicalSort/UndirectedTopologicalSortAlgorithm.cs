#nullable enable

using FastGraph.Algorithms.Search;

namespace FastGraph.Algorithms.TopologicalSort
{
    /// <summary>
    /// Undirected topological sort algorithm.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public sealed class UndirectedTopologicalSortAlgorithm<TVertex, TEdge> : AlgorithmBase<IUndirectedGraph<TVertex, TEdge>>
        where TVertex : notnull
        where TEdge : IEdge<TVertex>
    {
        private readonly IList<TVertex> _sortedVertices;

        /// <summary>
        /// Initializes a new instance of the <see cref="UndirectedTopologicalSortAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="capacity">Sorted vertices capacity.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        public UndirectedTopologicalSortAlgorithm(
            IUndirectedGraph<TVertex, TEdge> visitedGraph,
            int capacity = -1)
            : base(visitedGraph)
        {
            _sortedVertices = capacity > 0 ? new List<TVertex>(capacity) : new List<TVertex>();
        }

        /// <summary>
        /// Sorted vertices.
        /// </summary>
        public TVertex[]? SortedVertices { get; private set; }

        /// <summary>
        /// Gets or sets the flag that indicates if cyclic graph are supported or not.
        /// </summary>
        public bool AllowCyclicGraph { get; set; }

        private void BackEdge(object sender, UndirectedEdgeEventArgs<TVertex, TEdge> args)
        {
            if (!AllowCyclicGraph)
                throw new NonAcyclicGraphException();
        }

        private void OnVertexFinished(TVertex vertex)
        {
            _sortedVertices.Add(vertex);
        }

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void Initialize()
        {
            base.Initialize();

            SortedVertices = default;
            _sortedVertices.Clear();
        }

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            UndirectedDepthFirstSearchAlgorithm<TVertex, TEdge>? dfs = default;
            try
            {
                dfs = new UndirectedDepthFirstSearchAlgorithm<TVertex, TEdge>(
                    this,
                    VisitedGraph,
                    new Dictionary<TVertex, GraphColor>(VisitedGraph.VertexCount));
                dfs.BackEdge += BackEdge;
                dfs.FinishVertex += OnVertexFinished;

                dfs.Compute();
            }
            finally
            {
                if (dfs != default)
                {
                    dfs.BackEdge -= BackEdge;
                    dfs.FinishVertex -= OnVertexFinished;

                    SortedVertices = _sortedVertices.Reverse().ToArray();
                }
            }
        }

        #endregion
    }
}
