#nullable enable

using JetBrains.Annotations;
using FastGraph.Algorithms.ShortestPath;

namespace FastGraph.Algorithms.TSP
{
    /// <summary>
    /// Algorithm to answer the TSP (Travelling Salesman Problem), meaning finding a path that best link
    /// every vertices.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    /// <typeparam name="TGraph">Graph type.</typeparam>
    // ReSharper disable once InconsistentNaming
    public class TSP<TVertex, TEdge, TGraph> : ShortestPathAlgorithmBase<TVertex, TEdge, TGraph>
        where TVertex : notnull
        where TEdge : EquatableEdge<TVertex>
        where TGraph : BidirectionalGraph<TVertex, TEdge>
    {
        private readonly TasksManager<TVertex, TEdge> _taskManager = new TasksManager<TVertex, TEdge>();

        /// <summary>
        /// Shortest path found, otherwise <see langword="null"/>.
        /// </summary>
        public BidirectionalGraph<TVertex, TEdge>? ResultPath { get; private set; }

        /// <summary>
        /// Best cost found to answer the problem.
        /// </summary>
        public double BestCost { get; private set; } = double.PositiveInfinity;

        /// <summary>
        /// Initializes a new instance of the <see cref="TSP{TVertex,TEdge,TGraph}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="edgeWeights">Function that computes the weight for a given edge.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="edgeWeights"/> is <see langword="null"/>.</exception>
        public TSP(
            TGraph visitedGraph,
            Func<TEdge, double> edgeWeights)
            : base(default, visitedGraph, edgeWeights)
        {
        }

        [Pure]
        private static Dictionary<EquatableEdge<TVertex>, double> BuildWeightsDictionary(
            TGraph visitedGraph,
            [InstantHandle] Func<TEdge, double> edgeWeights)
        {
            var weights = new Dictionary<EquatableEdge<TVertex>, double>();
            foreach (TEdge edge in visitedGraph.Edges)
            {
                weights[edge] = edgeWeights(edge);
            }

            return weights;
        }

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void Initialize()
        {
            base.Initialize();

            var path = new BidirectionalGraph<TVertex, TEdge>();
            path.AddVertexRange(VisitedGraph.Vertices);

            _taskManager.AddTask(
                new Task<TVertex, TEdge>(
                    VisitedGraph,
                    BuildWeightsDictionary(VisitedGraph, Weights),
                    path,
                    0));
        }

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            while (_taskManager.HasTasks())
            {
                ThrowIfCancellationRequested();

                Task<TVertex, TEdge> task = _taskManager.GetTask();

                if (task.IsResultReady())
                {
                    BestCost = task.MinCost;
                    ResultPath = task.Path;
                    return;
                }

                if (task.Split(
                    out Task<TVertex, TEdge>? task1,
                    out Task<TVertex, TEdge>? task2))
                {
                    _taskManager.AddTask(task1);
                    _taskManager.AddTask(task2);
                }
            }
        }

        #endregion
    }
}
