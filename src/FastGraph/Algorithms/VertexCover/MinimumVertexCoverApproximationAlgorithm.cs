using System;
using System.Linq;
using JetBrains.Annotations;
using FastGraph.Collections;
#if SUPPORTS_CRYPTO_RANDOM
using FastGraph.Utils;
#endif

namespace FastGraph.Algorithms.VertexCover
{
    /// <summary>
    /// A minimum vertices cover approximation algorithm for undirected graphs.
    /// </summary>
    /// <remarks>
    /// This is a modified version (by Batov Nikita) of the original
    /// Mihalis Yannakakis and Fanica Gavril algorithm.
    /// </remarks>
    public sealed class MinimumVertexCoverApproximationAlgorithm<TVertex, TEdge> : AlgorithmBase<IUndirectedGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        [NotNull]
        private readonly VertexList<TVertex> _coverSet = new VertexList<TVertex>();

        [NotNull]
        private readonly Random _rng;

#if SUPPORTS_CRYPTO_RANDOM
        /// <summary>
        /// Initializes a new instance of the <see cref="MinimumVertexCoverApproximationAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <remarks>This constructor will use <see cref="CryptoRandom"/> ad random number generator.</remarks>
        /// <param name="graph">Graph to compute the cover.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="graph"/> is <see langword="null"/>.</exception>
        public MinimumVertexCoverApproximationAlgorithm(
            [NotNull] IUndirectedGraph<TVertex, TEdge> graph)
            : this(graph, new CryptoRandom())
        {
        }
#else
        /// <summary>
        /// Initializes a new instance of the <see cref="MinimumVertexCoverApproximationAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="graph">Graph to compute the cover.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="graph"/> is <see langword="null"/>.</exception>
        public MinimumVertexCoverApproximationAlgorithm(
            [NotNull] IUndirectedGraph<TVertex, TEdge> graph)
            : this(graph, new Random())
        {
        }
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="MinimumVertexCoverApproximationAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="graph">Graph to compute the cover.</param>
        /// <param name="rng">Random number generator.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="graph"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="rng"/> is <see langword="null"/>.</exception>
        public MinimumVertexCoverApproximationAlgorithm(
            [NotNull] IUndirectedGraph<TVertex, TEdge> graph,
            [NotNull] Random rng)
            : base(graph)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
        }

        /// <summary>
        /// Set of covering vertices.
        /// </summary>
        public VertexList<TVertex> CoverSet =>
            State == ComputationState.Finished
                ? _coverSet
                : null;

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            var graph = new UndirectedGraph<TVertex, TEdge>(
                VisitedGraph.AllowParallelEdges,
                VisitedGraph.EdgeEqualityComparer);
            graph.AddVerticesAndEdgeRange(VisitedGraph.Edges);

            while (!graph.IsEdgesEmpty)
            {
                TEdge[] graphEdges = graph.Edges.ToArray();

                // Get a random edge
                int randomEdgeIndex = _rng.Next(graphEdges.Length - 1);
                TEdge randomEdge = graphEdges[randomEdgeIndex];

                TVertex source = randomEdge.Source;
                TVertex target = randomEdge.Target;

                if (graph.AdjacentDegree(randomEdge.Source) > 1 && !_coverSet.Contains(source))
                {
                    _coverSet.Add(source);
                }

                if (graph.AdjacentDegree(randomEdge.Target) > 1 && !_coverSet.Contains(target))
                {
                    _coverSet.Add(target);
                }

                if (graph.AdjacentDegree(randomEdge.Target) == 1
                    && graph.AdjacentDegree(randomEdge.Source) == 1)
                {
                    if (!_coverSet.Contains(source))
                    {
                        _coverSet.Add(source);
                    }

                    graph.RemoveEdges(
                        graph.AdjacentEdges(source).ToArray());
                }
                else
                {
                    TEdge[] edgesToRemove = graph.AdjacentEdges(target)
                        .Concat(graph.AdjacentEdges(source))
                        .ToArray();

                    graph.RemoveEdges(edgesToRemove);
                }
            }
        }

        #endregion
    }
}