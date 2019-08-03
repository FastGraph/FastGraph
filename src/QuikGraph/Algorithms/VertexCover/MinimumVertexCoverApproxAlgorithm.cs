using System;
using System.Linq;
using JetBrains.Annotations;
using QuikGraph.Collections;

namespace QuikGraph.Algorithms.VertexCover
{
    /// <summary>
    /// A minimum vertices cover approximation algorithm for undirected graphs.
    /// </summary>
    /// <remarks>
    /// This is a modified version (by Batov Nikita) of the original
    /// Mihalis Yannakakis and Fanica Gavril algorithm.
    /// </remarks>
    public sealed class MinimumVertexCoverApproxAlgorithm<TVertex, TEdge> : AlgorithmBase<IUndirectedGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        [NotNull]
        private readonly VertexList<TVertex> _coverSet = new VertexList<TVertex>();

        [NotNull]
        private readonly Random _rng = new Random();

        /// <summary>
        /// Initializes a new instance of the <see cref="MinimumVertexCoverApproxAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="graph">Graph to compute the cover.</param>
        public MinimumVertexCoverApproxAlgorithm([NotNull] IUndirectedGraph<TVertex, TEdge> graph)
            : base(graph)
        {
        }

        /// <summary>
        /// Set of covering vertices.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
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
                var graphEdges = graph.Edges.ToArray();

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
