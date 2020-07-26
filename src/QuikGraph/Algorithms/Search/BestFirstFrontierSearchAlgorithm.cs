using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using QuikGraph.Algorithms.Services;
using QuikGraph.Collections;

namespace QuikGraph.Algorithms.Search
{
    /// <summary>
    /// Best first frontier search algorithm.
    /// </summary>
    /// <remarks>
    /// Algorithm from Frontier Search, Korkf, Zhand, Thayer, Hohwald.
    /// </remarks>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public sealed class BestFirstFrontierSearchAlgorithm<TVertex, TEdge>
        : RootedSearchAlgorithmBase<TVertex, IBidirectionalIncidenceGraph<TVertex, TEdge>>
        , ITreeBuilderAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        [NotNull]
        private readonly Func<TEdge, double> _edgeWeights;

        [NotNull]
        private readonly IDistanceRelaxer _distanceRelaxer;

        /// <summary>
        /// Initializes a new instance of the <see cref="BestFirstFrontierSearchAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="edgeWeights">Function that for a given edge provide its weight.</param>
        /// <param name="distanceRelaxer">Distance relaxer.</param>
        public BestFirstFrontierSearchAlgorithm(
            [NotNull] IBidirectionalIncidenceGraph<TVertex, TEdge> visitedGraph,
            [NotNull] Func<TEdge, double> edgeWeights,
            [NotNull] IDistanceRelaxer distanceRelaxer)
            : this(null, visitedGraph, edgeWeights, distanceRelaxer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BestFirstFrontierSearchAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="edgeWeights">Function that for a given edge provide its weight.</param>
        /// <param name="distanceRelaxer">Distance relaxer.</param>
        public BestFirstFrontierSearchAlgorithm(
            [CanBeNull] IAlgorithmComponent host,
            [NotNull] IBidirectionalIncidenceGraph<TVertex, TEdge> visitedGraph,
            [NotNull] Func<TEdge, double> edgeWeights,
            [NotNull] IDistanceRelaxer distanceRelaxer)
            : base(host, visitedGraph)
        {
            _edgeWeights = edgeWeights ?? throw new ArgumentNullException(nameof(edgeWeights));
            _distanceRelaxer = distanceRelaxer ?? throw new ArgumentNullException(nameof(distanceRelaxer));
        }

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            TVertex root = GetAndAssertRootInGraph();
            if (!TryGetTargetVertex(out TVertex target))
                throw new InvalidOperationException("Target vertex not set.");
            if (!VisitedGraph.ContainsVertex(target))
                throw new VertexNotFoundException("Target vertex is not part of the graph.");

            // Little shortcut
            if (EqualityComparer<TVertex>.Default.Equals(root, target))
            {
                OnTargetReached();
                return; // Found it
            }

            var open = new BinaryHeap<double, TVertex>(_distanceRelaxer.Compare);

            // (1) Place the initial node in Open, with all its operators marked unused
            open.Add(0, root);
            Dictionary<TEdge, GraphColor> operators = VisitedGraph.OutEdges(root).ToDictionary(edge => edge, edge => GraphColor.White);

            while (open.Count > 0)
            {
                ThrowIfCancellationRequested();

                // (3) Else, choose an Open node n of lowest cost for expansion
                KeyValuePair<double, TVertex> entry = open.RemoveMinimum();
                double cost = entry.Key;
                TVertex n = entry.Value;

                // (4) If node n is a target node, terminate with success
                if (EqualityComparer<TVertex>.Default.Equals(n, target))
                {
                    OnTargetReached();
                    return;
                }

                // (5) Else, expand node n, generating all
                // successors n' reachable via unused legal operators,
                // compute their cost and delete node n
                ExpandNode(n, operators, cost, open);

#if DEBUG
                OperatorMaxCount = Math.Max(OperatorMaxCount, operators.Count);
#endif

                // (6) In a directed graph, generate each predecessor node n via an unused operator
                // and create dummy nodes for each with costs of infinity
                foreach (TEdge edge in VisitedGraph.InEdges(n))
                {
                    if (operators.TryGetValue(edge, out GraphColor edgeColor)
                        && edgeColor == GraphColor.Gray)
                    {
                        // Delete node n
                        operators.Remove(edge);
                    }
                }
            }
        }

        private void ExpandNode(
            [NotNull] TVertex n,
            [NotNull] IDictionary<TEdge, GraphColor> operators,
            double cost,
            [NotNull] BinaryHeap<double, TVertex> open)
        {
            // Skip self-edges
            foreach (TEdge edge in VisitedGraph.OutEdges(n).Where(e => !e.IsSelfEdge()))
            {
                bool hasColor = operators.TryGetValue(edge, out GraphColor edgeColor);
                if (!hasColor || edgeColor == GraphColor.White)
                {
                    double weight = _edgeWeights(edge);
                    double nCost = _distanceRelaxer.Combine(cost, weight);

                    // (7) For each neighboring node of n' mark the operator from n to n' as used
                    // (8) For each node n', if there is no copy of n' in Open add it
                    // else save in open on the copy of n' with lowest cost. Mark as used all operators
                    // as used in any of the copies
                    operators[edge] = GraphColor.Gray;
                    if (open.MinimumUpdate(nCost, edge.Target))
                        OnTreeEdge(edge);
                }
                else
                {
                    Debug.Assert(edgeColor == GraphColor.Gray);

                    // Edge already seen, remove it
                    operators.Remove(edge);
                }
            }
        }

        #endregion

#if DEBUG
        /// <summary>
        /// Gets the maximum number of operators.
        /// </summary>
        public int OperatorMaxCount { get; private set; } = -1;
#endif

        #region ITreeBuilderAlgorithm<TVertex,TEdge>

        /// <inheritdoc />
        public event EdgeAction<TVertex, TEdge> TreeEdge;

        private void OnTreeEdge([NotNull] TEdge edge)
        {
            Debug.Assert(edge != null);

            TreeEdge?.Invoke(edge);
        }

        #endregion
    }
}