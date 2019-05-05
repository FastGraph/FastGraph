using System.Collections.Generic;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
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
#if SUPPORTS_CONTRACTS
            Contract.Requires(distanceRelaxer != null);
#endif

            DistanceRelaxer = distanceRelaxer;
        }

        private int _shortestPathCount = 3;

        /// <summary>
        /// Gets or sets the maximum number of shortest path to find.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        public int ShortestPathCount
        {
            get => _shortestPathCount;
            set
            {
#if SUPPORTS_CONTRACTS
                Contract.Requires(value > 1);
                Contract.Ensures(ShortestPathCount == value);
#endif

                _shortestPathCount = value;
            }
        }

        /// <summary>
        /// Gets the number of shortest path found.
        /// </summary>
        public int ComputedShortestPathCount
        {
            get
            {
#if SUPPORTS_CONTRACTS
                Contract.Ensures(Contract.Result<int>() == ComputedShortestPaths.Count());
#endif

                return _computedShortestPaths?.Count ?? 0;
            }
        }

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
#if SUPPORTS_CONTRACTS
            Contract.Requires(path != null);
            Contract.Requires(path.All(edge => edge != null));
#endif

            TEdge[] pathArray = path.ToArray();
            _computedShortestPaths.Add(pathArray);
        }

        /// <summary>
        /// Distance relaxer.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
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
