using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using QuikGraph.Algorithms.Observers;
using QuikGraph.Algorithms.ShortestPath;

namespace QuikGraph.Algorithms
{
    /// <summary>
    /// Algorithm that computes the most important vertices in a graph.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class CentralityApproximationAlgorithm<TVertex, TEdge> : AlgorithmBase<IVertexListGraph<TVertex, TEdge>>, IDisposable
        where TEdge : IEdge<TVertex>
    {
        [NotNull]
        private readonly DijkstraShortestPathAlgorithm<TVertex, TEdge> _dijkstra;

        [NotNull]
        private readonly IDisposable _predecessorRecorderSubscription;

        [NotNull]
        private readonly IDictionary<TVertex, double> _centralities = new Dictionary<TVertex, double>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CentralityApproximationAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="distances">Function to compute the distance given an edge.</param>
        public CentralityApproximationAlgorithm(
            [NotNull] IVertexListGraph<TVertex, TEdge> visitedGraph,
            [NotNull] Func<TEdge, double> distances)
            : base(visitedGraph)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(distances != null);
#endif

            _dijkstra = new DijkstraShortestPathAlgorithm<TVertex, TEdge>(
                VisitedGraph,
                distances,
                DistanceRelaxers.ShortestDistance);
            var predecessorRecorder = new VertexPredecessorRecorderObserver<TVertex, TEdge>();
            _predecessorRecorderSubscription = predecessorRecorder.Attach(_dijkstra);
        }

        /// <summary>
        /// Function to gets the distance of a given edge.
        /// </summary>
        public Func<TEdge, double> Distances => _dijkstra.Weights;

        /// <summary>
        /// Gets or sets the random number generator.
        /// </summary>
        [NotNull]
        public Random Rand { get; set; } = new Random();

        /// <summary>
        /// Maximum number of iterations.
        /// </summary>
        public int MaxIterationCount { get; set; } = 50;

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void Initialize()
        {
            base.Initialize();
            _centralities.Clear();
            foreach (TVertex vertex in VisitedGraph.Vertices)
                _centralities.Add(vertex, 0);
        }

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            if (VisitedGraph.VertexCount == 0)
                return;

            // Compute temporary values
            int n = VisitedGraph.VertexCount;
            for (int i = 0; i < MaxIterationCount; ++i)
            {
                TVertex vertex = RandomGraphFactory.GetVertex(VisitedGraph, Rand);
                _dijkstra.Compute(vertex);

                foreach (TVertex u in VisitedGraph.Vertices)
                {
                    if (_dijkstra.TryGetDistance(u, out double d))
                        _centralities[u] += n * d / (MaxIterationCount * (n - 1));
                }
            }

            // Update
            foreach (TVertex vertex in _centralities.Keys)
                _centralities[vertex] = 1.0 / _centralities[vertex];
        }

        #endregion

        #region IDisposable

        /// <inheritdoc />
        public void Dispose()
        {
            _predecessorRecorderSubscription.Dispose();
        }

        #endregion
    }
}
