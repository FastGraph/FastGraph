using System;
using System.Collections.Generic;
#if SUPPORTS_CRYPTO_RANDOM
using QuikGraph.Utils;
#endif

namespace QuikGraph.Algorithms.RandomWalks
{
    /// <summary>
    /// Base class for any implementation of a Markov chain.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public abstract class MarkovEdgeChainBase<TVertex, TEdge> : IMarkovEdgeChain<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <inheritdoc />
        public Random Rand { get; set; } =
#if SUPPORTS_CRYPTO_RANDOM
            new CryptoRandom();
#else
            new Random();
#endif

        /// <inheritdoc />
        public abstract bool TryGetSuccessor(IImplicitGraph<TVertex, TEdge> graph, TVertex vertex, out TEdge successor);

        /// <inheritdoc />
        public abstract bool TryGetSuccessor(IEnumerable<TEdge> edges, TVertex vertex, out TEdge successor);
    }
}