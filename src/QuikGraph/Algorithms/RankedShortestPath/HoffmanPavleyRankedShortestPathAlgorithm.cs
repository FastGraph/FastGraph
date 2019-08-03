using System;
using System.Collections.Generic;
using System.Diagnostics;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
using System.Linq;
#endif
using JetBrains.Annotations;
using QuikGraph.Algorithms.Observers;
using QuikGraph.Algorithms.Services;
using QuikGraph.Algorithms.ShortestPath;
using QuikGraph.Collections;

namespace QuikGraph.Algorithms.RankedShortestPath
{
    /// <summary>
    /// Hoffman and Pavley K-shortest path algorithm.
    /// </summary>
    /// <remarks>
    /// Reference:
    /// Hoffman, W. and Pavley, R. 1959. A Method for the Solution of the Nth Best Path Problem. 
    /// J. ACM 6, 4 (Oct. 1959), 506-514. DOI= http://doi.acm.org/10.1145/320998.321004
    /// </remarks>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public sealed class HoffmanPavleyRankedShortestPathAlgorithm<TVertex, TEdge>
        : RankedShortestPathAlgorithmBase<TVertex, TEdge, IBidirectionalGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        private TVertex _target;
        private bool _hasTargetVertex;

        [NotNull]
        private readonly Func<TEdge, double> _edgeWeights;

        /// <summary>
        /// Initializes a new instance of the <see cref="HoffmanPavleyRankedShortestPathAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="edgeWeights">Function that for a given edge provide its weight.</param>
        public HoffmanPavleyRankedShortestPathAlgorithm(
            [NotNull] IBidirectionalGraph<TVertex, TEdge> visitedGraph,
            [NotNull] Func<TEdge, double> edgeWeights)
            : this(null, visitedGraph, edgeWeights, DistanceRelaxers.ShortestDistance)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HoffmanPavleyRankedShortestPathAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="edgeWeights">Function that for a given edge provide its weight.</param>
        /// <param name="distanceRelaxer">Distance relaxer.</param>
        public HoffmanPavleyRankedShortestPathAlgorithm(
            [CanBeNull] IAlgorithmComponent host,
            [NotNull] IBidirectionalGraph<TVertex, TEdge> visitedGraph,
            [NotNull] Func<TEdge, double> edgeWeights,
            [NotNull] IDistanceRelaxer distanceRelaxer)
            : base(host, visitedGraph, distanceRelaxer)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(edgeWeights != null);
#endif

            _edgeWeights = edgeWeights;
        }

        /// <summary>
        /// Sets the target vertex.
        /// </summary>
        /// <param name="target">Target vertex.</param>
        public void SetTargetVertex([NotNull] TVertex target)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(target != null);
            Contract.Requires(VisitedGraph.ContainsVertex(target));
#endif

            _target = target;
            _hasTargetVertex = true;
        }

        /// <summary>
        /// Tries to get the target vertex if set.
        /// </summary>
        /// <param name="target">Target vertex if set, otherwise null.</param>
        /// <returns>True if the target vertex was set, false otherwise.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        public bool TryGetTargetVertex(out TVertex target)
        {
            if (_hasTargetVertex)
            {
                target = _target;
                return true;
            }

            target = default(TVertex);
            return false;
        }

        /// <summary>
        /// Runs the algorithm with the given <paramref name="root"/> vertex.
        /// </summary>
        /// <param name="root">Root vertex.</param>
        /// <param name="target">Target vertex.</param>
        public void Compute(TVertex root, TVertex target)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(root != null);
            Contract.Requires(target != null);
            Contract.Requires(VisitedGraph.ContainsVertex(root));
            Contract.Requires(VisitedGraph.ContainsVertex(target));
#endif

            SetRootVertex(root);
            SetTargetVertex(target);
            Compute();
        }

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            ICancelManager cancelManager = Services.CancelManager;

            if (!TryGetRootVertex(out TVertex root))
                throw new InvalidOperationException("Root vertex not set.");
            if (!TryGetTargetVertex(out TVertex target))
                throw new InvalidOperationException("Target vertex not set.");

            // Start by building the minimum tree starting from the target vertex.
            ComputeMinimumTree(
                target,
                out IDictionary<TVertex, TEdge> successors,
                out IDictionary<TVertex, double> distances);

            if (cancelManager.IsCancelling)
                return;

            var queue = new FibonacciQueue<DeviationPath, double>(deviationPath => deviationPath.Weight);
            int vertexCount = VisitedGraph.VertexCount;

            // First shortest path
            EnqueueFirstShortestPath(queue, successors, distances, root);

            while (queue.Count > 0
                   && ComputedShortestPathCount < ShortestPathCount)
            {
                if (cancelManager.IsCancelling)
                    return;

                DeviationPath deviation = queue.Dequeue();

                // Turn into path
                var path = new List<TEdge>();
                for (int i = 0; i < deviation.DeviationIndex; ++i)
                    path.Add(deviation.ParentPath[i]);
                path.Add(deviation.DeviationEdge);

                int startEdge = path.Count;
                AppendShortestPath(path, successors, deviation.DeviationEdge.Target);

#if SUPPORTS_CONTRACTS
                Contract.Assert(Math.Abs(deviation.Weight - path.Sum(e => _edgeWeights(e))) < float.Epsilon);
                Contract.Assert(path.Count > 0);
#endif

                // Add to list if has no cycle
                if (!path.HasCycles<TVertex, TEdge>())
                    AddComputedShortestPath(path);

                // Append new deviation paths
                if (path.Count < vertexCount)
                {
                    EnqueueDeviationPaths(
                        queue,
                        root,
                        distances,
                        path.ToArray(),
                        startEdge);
                }
            }
        }

        #endregion

        private void EnqueueFirstShortestPath(
            [NotNull] IQueue<DeviationPath> queue,
            [NotNull] IDictionary<TVertex, TEdge> successors,
            [NotNull] IDictionary<TVertex, double> distances,
            [NotNull] TVertex root)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(queue != null);
            Contract.Requires(queue.Count == 0);
            Contract.Requires(successors != null);
            Contract.Requires(distances != null);
            Contract.Requires(root != null);
#endif

            var path = new List<TEdge>();
            AppendShortestPath(path, successors, root);

            if (path.Count == 0)
                return; // Unreachable vertices

            if (!path.HasCycles<TVertex, TEdge>())
                AddComputedShortestPath(path);

            // Create deviation paths
            EnqueueDeviationPaths(
                queue,
                root,
                distances,
                path.ToArray(),
                0);
        }

        private void ComputeMinimumTree(
            [NotNull] TVertex target,
            out IDictionary<TVertex, TEdge> successors,
            out IDictionary<TVertex, double> distances)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(target != null);
#endif

            var reversedGraph =
                new ReversedBidirectionalGraph<TVertex, TEdge>(VisitedGraph);
            var successorsObserver =
                new VertexPredecessorRecorderObserver<TVertex, SReversedEdge<TVertex, TEdge>>();
            var distancesObserver =
                new VertexDistanceRecorderObserver<TVertex, SReversedEdge<TVertex, TEdge>>(ReversedEdgeWeight);
            var shortestPath =
                new DijkstraShortestPathAlgorithm<TVertex, SReversedEdge<TVertex, TEdge>>(
                    this, 
                    reversedGraph, 
                    ReversedEdgeWeight, 
                    DistanceRelaxer);

            using (successorsObserver.Attach(shortestPath))
            using (distancesObserver.Attach(shortestPath))
                shortestPath.Compute(target);

            successors = new Dictionary<TVertex, TEdge>();
            foreach (KeyValuePair<TVertex, SReversedEdge<TVertex, TEdge>> pair in successorsObserver.VertexPredecessors)
                successors.Add(pair.Key, pair.Value.OriginalEdge);

            distances = distancesObserver.Distances;

            #region Local function

            double ReversedEdgeWeight(SReversedEdge<TVertex, TEdge> edge)
            {
                return _edgeWeights(edge.OriginalEdge);
            }

            #endregion
        }

        private void EnqueueDeviationPaths(
            [NotNull] IQueue<DeviationPath> queue,
            [NotNull] TVertex root,
            [NotNull] IDictionary<TVertex, double> distances,
            [NotNull, ItemNotNull] TEdge[] path,
            int startEdge)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(queue != null);
            Contract.Requires(root != null);
            Contract.Requires(distances != null);
            Contract.Requires(path != null);
            Contract.Requires(path[0].IsAdjacent(root));
            Contract.Requires(0 <= startEdge && startEdge < path.Length);
#endif

            TVertex previousVertex = root;
            double previousWeight = 0;
            var pathVertices = new Dictionary<TVertex, int>(path.Length);
            for (int edgeIndex = 0; edgeIndex < path.Length; ++edgeIndex)
            {
                TEdge edge = path[edgeIndex];
                if (edgeIndex >= startEdge)
                {
                    EnqueueDeviationPaths(
                        queue,
                        distances,
                        path,
                        edgeIndex,
                        previousVertex,
                        previousWeight);
                }

                // Update counter
                previousVertex = edge.Target;
                previousWeight += _edgeWeights(edge);

                // Detection of loops
                if (edgeIndex == 0)
                    pathVertices[edge.Source] = 0;
                
                // We should really allow only one key
                if (pathVertices.ContainsKey(edge.Target))
                    break;

                pathVertices[edge.Target] = 0;
            }
        }

        private void EnqueueDeviationPaths(
            [NotNull] IQueue<DeviationPath> queue,
            [NotNull] IDictionary<TVertex, double> distances,
            [NotNull, ItemNotNull] TEdge[] path,
            int edgeIndex,
            [NotNull] TVertex previousVertex,
            double previousWeight)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(queue != null);
            Contract.Requires(distances != null);
            Contract.Requires(path != null);
            Contract.Requires(previousVertex != null);
#endif

            TEdge edge = path[edgeIndex];
            foreach (TEdge deviationEdge in VisitedGraph.OutEdges(previousVertex))
            {
                // Skip self edges and equal edges
                if (deviationEdge.Equals(edge) || deviationEdge.IsSelfEdge())
                    continue;

                // Any edge obviously creating a loop
                TVertex target = deviationEdge.Target;
                if (distances.TryGetValue(target, out double distance))
                {
                    double deviationWeight = DistanceRelaxer.Combine(
                        previousWeight,
                        DistanceRelaxer.Combine(
                            _edgeWeights(deviationEdge),
                            distance));

                    var deviation = new DeviationPath(
                        path,
                        edgeIndex,
                        deviationEdge,
                        deviationWeight);

                    queue.Enqueue(deviation);
                }
            }
        }

        private void AppendShortestPath(
            [NotNull, ItemNotNull] IList<TEdge> path,
            [NotNull] IDictionary<TVertex, TEdge> successors,
            [NotNull] TVertex startVertex)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(path != null);
            Contract.Requires(successors != null);
            Contract.Requires(startVertex != null);
            Contract.Ensures(path[path.Count - 1].Target.Equals(_target));
#endif

            TVertex current = startVertex;
            while (successors.TryGetValue(current, out TEdge edge))
            {
                path.Add(edge);
                current = edge.Target;
            }
        }

        [DebuggerDisplay("Weight = {" + nameof(Weight) + "}, Index = {" + nameof(DeviationIndex) + "}, Edge = {" + nameof(DeviationEdge) + "}")]
        private struct DeviationPath
        {
#if SUPPORTS_CONTRACTS
            [System.Diagnostics.Contracts.Pure]
#endif
            [NotNull, ItemNotNull]
            public TEdge[] ParentPath { get; }

#if SUPPORTS_CONTRACTS
            [System.Diagnostics.Contracts.Pure]
#endif
            public int DeviationIndex { get; }

#if SUPPORTS_CONTRACTS
            [System.Diagnostics.Contracts.Pure]
#endif
            [NotNull]
            public TEdge DeviationEdge { get; }

#if SUPPORTS_CONTRACTS
            [System.Diagnostics.Contracts.Pure]
#endif
            public double Weight { get; }

            public DeviationPath(
                [NotNull, ItemNotNull] TEdge[] parentPath,
                int deviationIndex,
                [NotNull] TEdge deviationEdge,
                double weight)
            {
#if SUPPORTS_CONTRACTS
                Contract.Requires(parentPath != null);
                Contract.Requires(0 <= deviationIndex && deviationIndex < parentPath.Length);
                Contract.Requires(deviationEdge != null);
                Contract.Requires(weight >= 0);
#endif

                ParentPath = parentPath;
                DeviationIndex = deviationIndex;
                DeviationEdge = deviationEdge;
                Weight = weight;
            }

            /// <inheritdoc />
            public override string ToString()
            {
                return $"{Weight} at {DeviationIndex} ({DeviationEdge})";
            }
        }
    }
}
