using System;
using System.Collections.Generic;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using System.Linq;
using QuikGraph.Algorithms.Services;

namespace QuikGraph.Algorithms.RankedShortestPath
{
    public abstract class RankedShortestPathAlgorithmBase<TVertex, TEdge, TGraph>
        : RootedAlgorithmBase<TVertex, TGraph>
        where TEdge : IEdge<TVertex>
        where TGraph : IGraph<TVertex, TEdge>
    {
        private readonly IDistanceRelaxer distanceRelaxer;
        private int shortestPathCount = 3;
        private List<IEnumerable<TEdge>> computedShortestPaths;

        public int ShortestPathCount
        {
            get { return this.shortestPathCount; }
            set
            {
#if SUPPORTS_CONTRACTS
                Contract.Requires(value > 1);
                Contract.Ensures(this.ShortestPathCount == value);
#endif

                this.shortestPathCount = value;
            }
        }

        public int ComputedShortestPathCount
        {
            get
            {
#if SUPPORTS_CONTRACTS
                Contract.Ensures(Contract.Result<int>() == Enumerable.Count(this.ComputedShortestPaths));
#endif

                return this.computedShortestPaths == null ? 0 : this.computedShortestPaths.Count;
            }
        }

        public IEnumerable<IEnumerable<TEdge>> ComputedShortestPaths
        {
            get
            {
                if (this.computedShortestPaths == null)
                    yield break;
                else
                    foreach (var path in this.computedShortestPaths)
                        yield return path;
            }
        }

        protected void AddComputedShortestPath(List<TEdge> path)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(path != null);
            Contract.Requires(Enumerable.All(path, e => e != null));
#endif

            var pathArray = path.ToArray();
            this.computedShortestPaths.Add(pathArray);
            Console.WriteLine("found shortest path {0}", path.Count);
        }

        public IDistanceRelaxer DistanceRelaxer
        {
            get { return this.distanceRelaxer; }
        }

        protected RankedShortestPathAlgorithmBase(
            IAlgorithmComponent host, 
            TGraph visitedGraph,
            IDistanceRelaxer distanceRelaxer)
            : base(host, visitedGraph)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(distanceRelaxer != null);
#endif

            this.distanceRelaxer = distanceRelaxer;
        }

        protected override void Initialize()
        {
            base.Initialize();
            this.computedShortestPaths = new List<IEnumerable<TEdge>>(this.ShortestPathCount);
        }
    }
}
