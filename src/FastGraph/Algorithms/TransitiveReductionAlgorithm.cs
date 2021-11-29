using JetBrains.Annotations;

namespace FastGraph.Algorithms
{
    /// <summary>
    /// Algorithm that computes the transitive reduction of a graph, which is another directed graph
    /// with the same vertices and as few edges as possible.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public class TransitiveReductionAlgorithm<TVertex, TEdge> : AlgorithmBase<BidirectionalGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransitiveReductionAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        public TransitiveReductionAlgorithm(
            [NotNull] BidirectionalGraph<TVertex, TEdge> visitedGraph)
            : base(visitedGraph)
        {
            TransitiveReduction = new BidirectionalGraph<TVertex, TEdge>();
        }

        /// <summary>
        /// Transitive reduction graph.
        /// </summary>
        [NotNull]
        public BidirectionalGraph<TVertex, TEdge> TransitiveReduction { get; }

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            // Clone the visited graph
            TransitiveReduction.AddVertexRange(VisitedGraph.Vertices);
            TransitiveReduction.AddEdgeRange(VisitedGraph.Edges);

            var algorithmHelper = new TransitiveAlgorithmHelper<TVertex, TEdge>(TransitiveReduction);
            algorithmHelper.InternalCompute((graph, u, v, found, edge) =>
            {
                if (found)
                {
                    graph.RemoveEdge(edge);
                }
            });
        }

        #endregion
    }
}