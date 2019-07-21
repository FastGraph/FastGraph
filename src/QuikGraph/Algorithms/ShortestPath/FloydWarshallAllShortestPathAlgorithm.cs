using System;
using System.Collections.Generic;
using System.Diagnostics;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using QuikGraph.Algorithms.Services;
using QuikGraph.Collections;

namespace QuikGraph.Algorithms.ShortestPath
{
    /// <summary>
    /// Floyd-Warshall all shortest path algorithm.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public class FloydWarshallAllShortestPathAlgorithm<TVertex, TEdge> : AlgorithmBase<IVertexAndEdgeListGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        [NotNull]
        private readonly Func<TEdge, double> _weights;

        [NotNull]
        private readonly IDistanceRelaxer _distanceRelaxer;

        [NotNull]
        private readonly Dictionary<SEquatableEdge<TVertex>, VertexData> _data;

        private struct VertexData
        {
            public double Distance { get; }

            private readonly TVertex _predecessor;
            private readonly TEdge _edge;

            private readonly bool _edgeStored;

            // Null edge for self edge data
            public VertexData(double distance, [CanBeNull] TEdge edge)
            {
                Distance = distance;
                _predecessor = default(TVertex);
                _edge = edge;
                _edgeStored = true;
            }

            public VertexData(double distance, [NotNull] TVertex predecessor)
            {
#if SUPPORTS_CONTRACTS
                Contract.Requires(predecessor != null);
#endif

                Distance = distance;
                _predecessor = predecessor;
                _edge = default(TEdge);
                _edgeStored = false;
            }

#if SUPPORTS_CONTRACTS
            [ContractInvariantMethod]
            private void ObjectInvariant()
            {
                Contract.Invariant(_edgeStored 
                    ? _edge != null 
                    : _predecessor != null);
            }
#endif

            [JetBrains.Annotations.Pure]
            public bool TryGetPredecessor(out TVertex predecessor)
            {
                predecessor = _predecessor;
                return !_edgeStored;
            }

            [JetBrains.Annotations.Pure]
            public bool TryGetEdge(out TEdge edge)
            {
                edge = _edge;
                return _edgeStored;
            }

            public override string ToString()
            {
                if (_edgeStored)
                    return $"e:{Distance}-{_edge}";
                return $"p:{Distance}-{_predecessor}";
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FloydWarshallAllShortestPathAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="edgeWeights">Function that computes the weight for a given edge.</param>
        public FloydWarshallAllShortestPathAlgorithm(
            [NotNull] IVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph,
            [NotNull] Func<TEdge, double> edgeWeights)
            : this(visitedGraph, edgeWeights, DistanceRelaxers.ShortestDistance)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FloydWarshallAllShortestPathAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="edgeWeights">Function that computes the weight for a given edge.</param>
        /// <param name="distanceRelaxer">Distance relaxer.</param>
        public FloydWarshallAllShortestPathAlgorithm(
            [NotNull] IVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph,
            [NotNull] Func<TEdge, double> edgeWeights,
            [NotNull] IDistanceRelaxer distanceRelaxer)
            : this(null, visitedGraph, edgeWeights, distanceRelaxer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FloydWarshallAllShortestPathAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="edgeWeights">Function that computes the weight for a given edge.</param>
        /// <param name="distanceRelaxer">Distance relaxer.</param>
        public FloydWarshallAllShortestPathAlgorithm(
            [CanBeNull] IAlgorithmComponent host,
            [NotNull] IVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph,
            [NotNull] Func<TEdge, double> edgeWeights,
            [NotNull] IDistanceRelaxer distanceRelaxer)
            : base(host, visitedGraph)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(edgeWeights != null);
            Contract.Requires(distanceRelaxer != null);
#endif

            _weights = edgeWeights;
            _distanceRelaxer = distanceRelaxer;
            _data = new Dictionary<SEquatableEdge<TVertex>, VertexData>();
        }

        /// <summary>
        /// Tries to get the distance (<paramref name="cost"/>) between
        /// <paramref name="source"/> and <paramref name="target"/>.
        /// </summary>
        /// <param name="source">Source vertex.</param>
        /// <param name="target">Target vertex.</param>
        /// <param name="cost">Associated distance (cost).</param>
        /// <returns>True if the distance was found, false otherwise.</returns>
        public bool TryGetDistance([NotNull] TVertex source, [NotNull] TVertex target, out double cost)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(source != null);
            Contract.Requires(target != null);
#endif

            if (_data.TryGetValue(new SEquatableEdge<TVertex>(source, target), out VertexData data))
            {
                cost = data.Distance;
                return true;
            }

            cost = -1;
            return false;
        }

        /// <summary>
        /// Tries to get the path that links both <paramref name="source"/>
        /// and <paramref name="target"/> vertices.
        /// </summary>
        /// <param name="source">Source vertex.</param>
        /// <param name="target">Target vertex.</param>
        /// <param name="path">The found path, otherwise null.</param>
        /// <returns>True if a path linking both vertices was found, false otherwise.</returns>
        public bool TryGetPath(
            [NotNull] TVertex source,
            [NotNull] TVertex target,
            out IEnumerable<TEdge> path)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(source != null);
            Contract.Requires(target != null);
#endif

            if (source.Equals(target))
            {
                path = null;
                return false;
            }

#if DEBUG && !NET20
            var set = new HashSet<TVertex> { source, target };
#endif

            var edges = new EdgeList<TVertex, TEdge>();
            var todo = new Stack<SEquatableEdge<TVertex>>();
            todo.Push(new SEquatableEdge<TVertex>(source, target));
            while (todo.Count > 0)
            {
                SEquatableEdge<TVertex> current = todo.Pop();
#if SUPPORTS_CONTRACTS
                Contract.Assert(!current.Source.Equals(current.Target));
#endif

                if (_data.TryGetValue(current, out VertexData data))
                {
                    if (data.TryGetEdge(out TEdge edge))
                    {
                        edges.Add(edge);
                    }
                    else
                    {
                        if (data.TryGetPredecessor(out TVertex intermediate))
                        {
#if SUPPORTS_CONTRACTS && DEBUG && !NET20
                            Contract.Assert(set.Add(intermediate));
#endif

                            todo.Push(new SEquatableEdge<TVertex>(intermediate, current.Target));
                            todo.Push(new SEquatableEdge<TVertex>(current.Source, intermediate));
                        }
                        else
                        {
#if SUPPORTS_CONTRACTS
                            Contract.Assert(false, "Cannot find predecessor.");
#else
                            throw new InvalidOperationException("Cannot find predecessor.");
#endif
                        }
                    }
                }
                else
                {
                    // No path found
                    path = null;
                    return false;
                }
            }

#if SUPPORTS_CONTRACTS
            Contract.Assert(todo.Count == 0);
            Contract.Assert(edges.Count > 0);
#endif

            path = edges;
            return true;
        }

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void Initialize()
        {
            base.Initialize();

            // Matrix i,j -> path
            _data.Clear();

            // Prepare the matrix with initial costs
            // Walk each edge and add entry in cost dictionary
            foreach (TEdge edge in VisitedGraph.Edges)
            {
                SEquatableEdge<TVertex> ij = edge.ToVertexPair();
                double cost = _weights(edge);
                if (!_data.TryGetValue(ij, out VertexData data))
                    _data[ij] = new VertexData(cost, edge);
                else if (cost < data.Distance)
                    _data[ij] = new VertexData(cost, edge);
            }
        }

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            ICancelManager cancelManager = Services.CancelManager;
            if (cancelManager.IsCancelling)
                return;

            TVertex[] vertices = VisitedGraph.Vertices.ToArray();

            // Walk each vertices and make sure cost self-cost 0
            foreach (TVertex vertex in vertices)
                _data[new SEquatableEdge<TVertex>(vertex, vertex)] = new VertexData(0, default(TEdge));

            if (cancelManager.IsCancelling)
                return;

            // Iterate k, i, j
            foreach (TVertex vk in vertices)
            {
                if (cancelManager.IsCancelling)
                    return;

                FillIData(vertices, vk);
            }

            // Check negative cycles
            CheckNegativeCycles(vertices);
        }

        private void FillIData([NotNull, ItemNotNull] TVertex[] vertices, [NotNull] TVertex vk)
        {
            foreach (TVertex vi in vertices)
            {
                var ik = new SEquatableEdge<TVertex>(vi, vk);
                if (_data.TryGetValue(ik, out VertexData pathIk))
                {
                    FillJData(vertices, vi, vk, pathIk);
                }
            }
        }

        private void FillJData(
            [NotNull, ItemNotNull] IEnumerable<TVertex> vertices,
            [NotNull] TVertex vi,
            [NotNull] TVertex vk,
            VertexData pathIk)
        {
            foreach (TVertex vj in vertices)
            {
                var kj = new SEquatableEdge<TVertex>(vk, vj);
                if (_data.TryGetValue(kj, out VertexData pathKj))
                {
                    double combined = _distanceRelaxer.Combine(
                        pathIk.Distance,
                        pathKj.Distance);

                    var ij = new SEquatableEdge<TVertex>(vi, vj);
                    if (_data.TryGetValue(ij, out VertexData pathIj))
                    {
                        if (_distanceRelaxer.Compare(combined, pathIj.Distance) < 0)
                            _data[ij] = new VertexData(combined, vk);
                    }
                    else
                    {
                        _data[ij] = new VertexData(combined, vk);
                    }
                }
            }
        }

        private void CheckNegativeCycles([NotNull, ItemNotNull] IEnumerable<TVertex> vertices)
        {
            foreach (TVertex vi in vertices)
            {
                var ii = new SEquatableEdge<TVertex>(vi, vi);
                if (_data.TryGetValue(ii, out VertexData data) && data.Distance < 0)
                    throw new NegativeCycleGraphException();
            }
        }

        #endregion

        /// <summary>
        /// Dumps current data state to stream <paramref name="writer"/>.
        /// </summary>
        [Conditional("DEBUG")]
        public void Dump([NotNull] TextWriter writer)
        {
            writer.WriteLine("data:");
            foreach (KeyValuePair<SEquatableEdge<TVertex>, VertexData> kv in _data)
            {
                writer.WriteLine(
                    $"{kv.Key.Source}->{kv.Key.Target}: {kv.Value.ToString()}");
            }
        }
    }
}
