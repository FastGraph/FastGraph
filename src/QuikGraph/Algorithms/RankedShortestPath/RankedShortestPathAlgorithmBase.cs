using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using QuikGraph.Algorithms.Services;

namespace QuikGraph.Algorithms.RankedShortestPath
{
    /// <summary>
    /// Base class for shortest path finder algorithms.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    /// <typeparam name="TGraph">Graph type.</typeparam>
    public abstract class RankedShortestPathAlgorithmBase<TVertex, TEdge, TGraph> : RootedAlgorithmBase<TVertex, TGraph>
        where TEdge : IEdge<TVertex>
        where TGraph : IGraph<TVertex, TEdge>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RankedShortestPathAlgorithmBase{TVertex,TEdge,TGraph}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="distanceRelaxer">Distance relaxer.</param>
        protected RankedShortestPathAlgorithmBase(
            [CanBeNull] IAlgorithmComponent host,
            [NotNull] TGraph visitedGraph,
            [NotNull] IDistanceRelaxer distanceRelaxer)
            : base(host, visitedGraph)
        {
            DistanceRelaxer = distanceRelaxer ?? throw new ArgumentNullException(nameof(distanceRelaxer));
        }

        private int _shortestPathCount = 3;

        /// <summary>
        /// Gets or sets the maximum number of shortest path to find.
        /// </summary>
        public int ShortestPathCount
        {
            get => _shortestPathCount;
            set
            {
                if (value <= 1)
                    throw new ArgumentException("Must be more than 1.", nameof(value));

                _shortestPathCount = value;
            }
        }

        /// <summary>
        /// Gets the number of shortest path found.
        /// </summary>
        public int ComputedShortestPathCount => _computedShortestPaths?.Count ?? 0;

        [ItemNotNull]
        private List<IEnumerable<TEdge>> _computedShortestPaths;

        /// <summary>
        /// Enumerable of shortest paths found.
        /// </summary>
        [NotNull, ItemNotNull]
        public IEnumerable<IEnumerable<TEdge>> ComputedShortestPaths
        {
            get
            {
                if (_computedShortestPaths is null)
                    yield break;

                foreach (IEnumerable<TEdge> path in _computedShortestPaths)
                    yield return path;
            }
        }

        /// <summary>
        /// Adds the given <paramref name="path"/> to the set of found shortest paths.
        /// </summary>
        /// <param name="path">Path to add.</param>
        protected void AddComputedShortestPath([NotNull, ItemNotNull] IEnumerable<TEdge> path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));
            TEdge[] pathArray = path.ToArray();
            if (pathArray.Any(edge => edge == null))
                throw new ArgumentException("There is at least one null edge is the path.", nameof(path));

            _computedShortestPaths.Add(pathArray);
        }

        /// <summary>
        /// Distance relaxer.
        /// </summary>
        [NotNull]
        public IDistanceRelaxer DistanceRelaxer { get; }

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void Initialize()
        {
            base.Initialize();
            _computedShortestPaths = new List<IEnumerable<TEdge>>(ShortestPathCount);
        }

        #endregion
    }
}
