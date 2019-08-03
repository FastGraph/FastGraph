using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using QuikGraph.Algorithms.Services;
using QuikGraph.Predicates;

namespace QuikGraph.Algorithms.Ranking
{
    /// <summary>
    /// Algorithm that computes the page rank of a graph.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class PageRankAlgorithm<TVertex, TEdge> : AlgorithmBase<IBidirectionalGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PageRankAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        public PageRankAlgorithm([NotNull] IBidirectionalGraph<TVertex, TEdge> visitedGraph)
            : base(visitedGraph)
        {
        }

        /// <summary>
        /// Ranks per vertices.
        /// </summary>
        [NotNull]
        public IDictionary<TVertex, double> Ranks { get; private set; } = new Dictionary<TVertex, double>();

        /// <summary>
        /// Gets or sets the damping rate.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        public double Damping { get; set; } = 0.85;

        /// <summary>
        /// Gets or sets the error tolerance (used to stop the algorithm).
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        public double Tolerance { get; set; } = 2 * double.Epsilon;

        /// <summary>
        /// Gets or sets the maximum number of iterations.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        public int MaxIterations { get; set; } = 60;

        /// <summary>
        /// Initializes all vertices ranks to 0.
        /// </summary>
        public void InitializeRanks()
        {
            Ranks.Clear();
            foreach (TVertex vertex in VisitedGraph.Vertices)
            {
                Ranks.Add(vertex, 0);
            }
        }

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            ICancelManager cancelManager = Services.CancelManager;
            IDictionary<TVertex, double> tempRanks = new Dictionary<TVertex, double>();

            // Create filtered graph
            var filterGraph = new FilteredBidirectionalGraph<TVertex, TEdge, IBidirectionalGraph<TVertex, TEdge>>(
                VisitedGraph,
                new InDictionaryVertexPredicate<TVertex, double>(Ranks).Test,
                edge => true);

            int iteration = 0;
            double error;
            do
            {
                if (cancelManager.IsCancelling)
                    return;

                // Compute page ranks
                error = 0;
                foreach (KeyValuePair<TVertex, double> pair in Ranks)
                {
                    if (cancelManager.IsCancelling)
                        return;

                    TVertex vertex = pair.Key;
                    double rank = pair.Value;

                    // Compute ARi
                    double r = 0;
                    foreach (TEdge edge in filterGraph.InEdges(vertex))
                    {
                        r += Ranks[edge.Source] / filterGraph.OutDegree(edge.Source);
                    }

                    // Add sourceRank and store it
                    double newRank = (1 - Damping) + Damping * r;
                    tempRanks[vertex] = newRank;
                    
                    // Compute deviation
                    error += Math.Abs(rank - newRank);
                }

                // Swap ranks
                IDictionary<TVertex, double> temp = Ranks;
                Ranks = tempRanks;
                tempRanks = temp;

                ++iteration;
            } while (error > Tolerance && iteration < MaxIterations);
        }

        /// <summary>
        /// Gets the sum of all ranks.
        /// </summary>
        /// <returns>Ranks sum.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [Pure]
        public double GetRanksSum()
        {
            return Ranks.Values.Sum();
        }

        /// <summary>
        /// Gets the rank average.
        /// </summary>
        /// <returns>Rank average.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [Pure]
        public double GetRanksMean()
        {
            return GetRanksSum() / Ranks.Count;
        }
    }
}
