using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace QuikGraph.Algorithms.RandomWalks
{
    /// <summary>
    /// Markov chain with weight.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public sealed class WeightedMarkovEdgeChain<TVertex, TEdge> : WeightedMarkovEdgeChainBase<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WeightedMarkovEdgeChainBase{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="edgeWeights">Map that contains edge weights.</param>
        public WeightedMarkovEdgeChain([NotNull] IDictionary<TEdge, double> edgeWeights)
            : base(edgeWeights)
        {
        }

        /// <inheritdoc />
        public override bool TryGetSuccessor(IImplicitGraph<TVertex, TEdge> graph, TVertex vertex, out TEdge successor)
        {
            if (!graph.IsOutEdgesEmpty(vertex))
            {
                // Compute out-edge su
                double outWeight = GetOutWeight(graph, vertex);
                // Scale and get next edge
                double random = Rand.NextDouble() * outWeight;
                return TryGetSuccessor(graph, vertex, random, out successor);
            }

            successor = default(TEdge);
            return false;
        }

        /// <inheritdoc />
        public override bool TryGetSuccessor(IEnumerable<TEdge> edges, TVertex vertex, out TEdge successor)
        {
            TEdge[] edgeArray = edges.ToArray();
            // Compute out-edge su
            double outWeight = GetWeights(edgeArray);
            // Scale and get next edge
            double random = Rand.NextDouble() * outWeight;
            return TryGetSuccessor(edgeArray, random, out successor);
        }
    }
}