#nullable enable

using FastGraph.Algorithms.Services;
using FastGraph.Collections;

namespace FastGraph.Algorithms.MinimumSpanningTree
{
    /// <summary>
    /// Kruskal minimum spanning tree algorithm implementation.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public sealed class KruskalMinimumSpanningTreeAlgorithm<TVertex, TEdge>
        : AlgorithmBase<IUndirectedGraph<TVertex, TEdge>>
        , IMinimumSpanningTreeAlgorithm<TVertex, TEdge>
        where TVertex : notnull
        where TEdge : IEdge<TVertex>
    {
        private readonly Func<TEdge, double> _edgeWeights;

        /// <summary>
        /// Initializes a new instance of the <see cref="KruskalMinimumSpanningTreeAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="edgeWeights">Function that computes the weight for a given edge.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="edgeWeights"/> is <see langword="null"/>.</exception>
        public KruskalMinimumSpanningTreeAlgorithm(
            IUndirectedGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> edgeWeights)
            : this(default, visitedGraph, edgeWeights)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KruskalMinimumSpanningTreeAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="edgeWeights">Function that computes the weight for a given edge.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="edgeWeights"/> is <see langword="null"/>.</exception>
        public KruskalMinimumSpanningTreeAlgorithm(
            IAlgorithmComponent? host,
            IUndirectedGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> edgeWeights)
            : base(host, visitedGraph)
        {
            _edgeWeights = edgeWeights ?? throw new ArgumentNullException(nameof(edgeWeights));
        }

        /// <summary>
        /// Fired when an edge is going to be analyzed.
        /// </summary>
        public event EdgeAction<TVertex, TEdge>? ExamineEdge;

        private void OnExamineEdge(TEdge edge)
        {
            ExamineEdge?.Invoke(edge);
        }

        #region ITreeBuilderAlgorithm<TVertex,TEdge>

        /// <inheritdoc />
        public event EdgeAction<TVertex, TEdge>? TreeEdge;

        private void OnTreeEdge(TEdge edge)
        {
            TreeEdge?.Invoke(edge);
        }

        #endregion

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            var sets = new ForestDisjointSet<TVertex>(VisitedGraph.VertexCount);
            foreach (TVertex vertex in VisitedGraph.Vertices)
            {
                sets.MakeSet(vertex);
            }

            ThrowIfCancellationRequested();

            var queue = new BinaryQueue<TEdge, double>(_edgeWeights);
            foreach (TEdge edge in VisitedGraph.Edges)
            {
                queue.Enqueue(edge);
            }

            ThrowIfCancellationRequested();

            while (queue.Count > 0)
            {
                TEdge edge = queue.Dequeue();
                OnExamineEdge(edge);

                if (!sets.AreInSameSet(edge.Source, edge.Target))
                {
                    OnTreeEdge(edge);
                    sets.Union(edge.Source, edge.Target);
                }
            }
        }

        #endregion
    }
}
