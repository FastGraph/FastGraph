#nullable enable

using System.Diagnostics.CodeAnalysis;

namespace FastGraph.Algorithms.RandomWalks
{
    /// <summary>
    /// Normalized Markov chain implementation.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public sealed class NormalizedMarkovEdgeChain<TVertex, TEdge> : MarkovEdgeChainBase<TVertex, TEdge>
        where TVertex : notnull
        where TEdge : IEdge<TVertex>
    {
        /// <inheritdoc />
        public override bool TryGetSuccessor(IImplicitGraph<TVertex, TEdge> graph, TVertex vertex, [NotNullWhen(true)] out TEdge? successor)
        {
            int outDegree = graph.OutDegree(vertex);
            if (outDegree > 0)
            {
                int index = Rand.Next(0, outDegree);
                successor = graph.OutEdge(vertex, index);
                return true;
            }

            successor = default(TEdge);
            return false;
        }

        /// <inheritdoc />
        public override bool TryGetSuccessor(IEnumerable<TEdge> edges, TVertex vertex, [NotNullWhen(true)] out TEdge? successor)
        {
            TEdge[] edgeArray = edges.ToArray();
            if (edgeArray.Length > 0)
            {
                int index = Rand.Next(0, edgeArray.Length);
                successor = edgeArray[index];
                return true;
            }

            successor = default(TEdge);
            return false;
        }
    }
}
