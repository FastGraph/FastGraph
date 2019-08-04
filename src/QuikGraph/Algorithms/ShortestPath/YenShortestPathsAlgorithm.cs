using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using QuikGraph.Algorithms.Observers;

namespace QuikGraph.Algorithms.ShortestPath
{
    /// <summary>
    /// A single-source K-shortest loopless paths algorithm for graphs
    /// with non negative edge cost.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    public class YenShortestPathsAlgorithm<TVertex>
    {
        /// <summary>
        /// Class representing a sorted path.
        /// </summary>
        public struct SortedPath : IEnumerable<TaggedEquatableEdge<TVertex, double>>
        {
            [NotNull, ItemNotNull]
            private readonly IList<TaggedEquatableEdge<TVertex, double>> _edges;

            /// <summary>
            /// Initializes a new instance of the <see cref="SortedPath"/> struct.
            /// </summary>
            public SortedPath([NotNull, ItemNotNull] IEnumerable<TaggedEquatableEdge<TVertex, double>> edges)
            {
                _edges = edges.ToList();
            }

            /// <inheritdoc />
            public IEnumerator<TaggedEquatableEdge<TVertex, double>> GetEnumerator()
            {
                return _edges.GetEnumerator();
            }

            /// <inheritdoc />
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private readonly TVertex _sourceVertex;
        private readonly TVertex _targetVertex;

        [NotNull]
        private readonly Func<TaggedEquatableEdge<TVertex, double>, double> _weights;

        [NotNull]
        private readonly Func<IEnumerable<SortedPath>, IEnumerable<SortedPath>> _filter;

        // Limit for the amount of paths
        private readonly int _k;

        [NotNull]
        private readonly AdjacencyGraph<TVertex, TaggedEquatableEdge<TVertex, double>> _graph;

        [NotNull, ItemNotNull]
        private readonly List<TaggedEquatableEdge<TVertex, double>> _removedEdges = new List<TaggedEquatableEdge<TVertex, double>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="YenShortestPathsAlgorithm{TVertex}"/> class.
        /// </summary>
        /// <remarks>
        /// <see cref="double"/> for tag type (edge) which comes from Dijkstra’s algorithm, which is used to get one shortest path.
        /// </remarks>
        /// <param name="graph">Graph to visit.</param>
        /// <param name="source">Source vertex.</param>
        /// <param name="target">Target vertex.</param>
        /// <param name="k">Maximum number of path to search.</param>
        /// <param name="edgeWeights">Optional function that computes the weight for a given edge.</param>
        /// <param name="filter">Optional filter of found paths.</param>
        public YenShortestPathsAlgorithm(
            [NotNull] AdjacencyGraph<TVertex, TaggedEquatableEdge<TVertex, double>> graph,
            [NotNull] TVertex source,
            [NotNull] TVertex target,
            int k,
            [CanBeNull] Func<TaggedEquatableEdge<TVertex, double>, double> edgeWeights = null,
            [CanBeNull] Func<IEnumerable<SortedPath>, IEnumerable<SortedPath>> filter = null)
        {
            if (graph is null)
                throw new ArgumentNullException(nameof(graph));
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            _sourceVertex = source;
            _targetVertex = target;
            _k = k;
            _graph = graph.Clone();
            _weights = edgeWeights ?? DefaultGetWeights;
            _filter = filter ?? DefaultFilter;
        }

        [NotNull]
        private static IEnumerable<SortedPath> DefaultFilter([NotNull] IEnumerable<SortedPath> paths)
        {
            return paths;
        }

        private static double DefaultGetWeights([NotNull] TaggedEquatableEdge<TVertex, double> edge)
        {
            return edge.Tag;
        }

        private double GetPathDistance([ItemNotNull] SortedPath edges)
        {
            return edges.Sum(edge => _weights(edge));
        }

        private SortedPath? GetShortestPathInGraph(
            [NotNull] AdjacencyGraph<TVertex, TaggedEquatableEdge<TVertex, double>> graph)
        {
            // Compute distances between the start vertex and other
            var algorithm = new DijkstraShortestPathAlgorithm<TVertex, TaggedEquatableEdge<TVertex, double>>(graph, _weights);
            var recorder = new VertexPredecessorRecorderObserver<TVertex, TaggedEquatableEdge<TVertex, double>>();

            using (recorder.Attach(algorithm))
                algorithm.Compute(_sourceVertex);

            // Get shortest path from start (source) vertex to target
            return recorder.TryGetPath(_targetVertex, out IEnumerable<TaggedEquatableEdge<TVertex, double>> path)
                ? new SortedPath(path)
                : (SortedPath?)null;
        }

        /// <summary>
        /// Runs the algorithm.
        /// </summary>
        /// <returns>Found paths.</returns>
        public IEnumerable<SortedPath> Execute()
        {
            var shortestWays = new List<SortedPath>();
            // Find the first shortest way
            SortedPath? shortestWay = GetShortestPathInGraph(_graph);

            // In case of Dijkstra’s algorithm couldn't find any ways
            if (shortestWay is null)
                throw new NoPathFoundException();

            shortestWays.Add(shortestWay.Value);

            for (int i = 0; i < _k - 1; ++i)
            {
                double minDistance = double.MaxValue;
                SortedPath? pathSlot = null;
                TaggedEquatableEdge<TVertex, double> removedEdge = null;
                foreach (TaggedEquatableEdge<TVertex, double> edge in shortestWay.Value)
                {
                    _graph.RemoveEdge(edge);

                    // Find shortest way in the graph without this edge
                    SortedPath? newPath = GetShortestPathInGraph(_graph);
                    _graph.AddEdge(edge);

                    if (newPath is null)
                        continue;

                    double pathWeight = GetPathDistance(newPath.Value);
                    if (pathWeight >= minDistance)
                        continue;

                    minDistance = pathWeight;
                    pathSlot = newPath;
                    removedEdge = edge;
                }

                if (pathSlot is null)
                    break;

                shortestWays.Add(pathSlot.Value);
                _removedEdges.Add(removedEdge);
                shortestWay = pathSlot;
                _graph.RemoveEdge(removedEdge);
            }

            return _filter(shortestWays);
        }

        /// <summary>
        /// Gets removed edges during algorithm run.
        /// </summary>
        /// <returns>Removed edges.</returns>
        [NotNull, ItemNotNull]
        public IEnumerable<TaggedEquatableEdge<TVertex, double>> RemovedEdges()
        {
            return _removedEdges.AsEnumerable();
        }
    }
}
