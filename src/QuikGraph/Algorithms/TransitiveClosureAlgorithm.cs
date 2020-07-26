using System;
using JetBrains.Annotations;

namespace QuikGraph.Algorithms
{
    /// <summary>
    /// Algorithm that computes the transitive closure of a graph, which is another directed graph
    /// with the same vertices and every reachable vertices by a given one linked by a single edge.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public class TransitiveClosureAlgorithm<TVertex, TEdge> : AlgorithmBase<BidirectionalGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransitiveClosureAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="edgeFactory">Function that create an edge between the 2 given vertices.</param>
        public TransitiveClosureAlgorithm(
            [NotNull] BidirectionalGraph<TVertex, TEdge> visitedGraph,
            [NotNull] Func<TVertex, TVertex, TEdge> edgeFactory)
            : base(visitedGraph)
        {
            TransitiveClosure = new BidirectionalGraph<TVertex, TEdge>();
            _createEdge = edgeFactory ?? throw new ArgumentNullException(nameof(edgeFactory));
        }

        /// <summary>
        /// Transitive closure graph.
        /// </summary>
        public BidirectionalGraph<TVertex, TEdge> TransitiveClosure { get; }

        [NotNull]
        private readonly Func<TVertex, TVertex, TEdge> _createEdge;

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            // Clone the visited graph
            TransitiveClosure.AddVertexRange(VisitedGraph.Vertices);
            TransitiveClosure.AddEdgeRange(VisitedGraph.Edges);

            var algorithmHelper = new TransitiveAlgorithmHelper<TVertex, TEdge>(TransitiveClosure);
            algorithmHelper.InternalCompute((graph, u, v, found, edge) =>
            {
                if (!found)
                    graph.AddEdge(_createEdge(u, v));
            });
        }

        #endregion
    }
}