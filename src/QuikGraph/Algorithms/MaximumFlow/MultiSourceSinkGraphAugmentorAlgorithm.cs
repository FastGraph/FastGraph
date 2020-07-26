using JetBrains.Annotations;
using QuikGraph.Algorithms.Services;

namespace QuikGraph.Algorithms.MaximumFlow
{
    /// <summary>
    /// Multi source and sink graph augmentor algorithm.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public sealed class MultiSourceSinkGraphAugmentorAlgorithm<TVertex, TEdge>
        : GraphAugmentorAlgorithmBase<TVertex, TEdge, IMutableBidirectionalGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiSourceSinkGraphAugmentorAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="vertexFactory">Vertex factory method.</param>
        /// <param name="edgeFactory">Edge factory method.</param>
        public MultiSourceSinkGraphAugmentorAlgorithm(
            [NotNull] IMutableBidirectionalGraph<TVertex, TEdge> visitedGraph,
            [NotNull] VertexFactory<TVertex> vertexFactory,
            [NotNull] EdgeFactory<TVertex, TEdge> edgeFactory)
            : this(null, visitedGraph, vertexFactory, edgeFactory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiSourceSinkGraphAugmentorAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="vertexFactory">Vertex factory method.</param>
        /// <param name="edgeFactory">Edge factory method.</param>
        public MultiSourceSinkGraphAugmentorAlgorithm(
            [CanBeNull] IAlgorithmComponent host,
            [NotNull] IMutableBidirectionalGraph<TVertex, TEdge> visitedGraph,
            [NotNull] VertexFactory<TVertex> vertexFactory,
            [NotNull] EdgeFactory<TVertex, TEdge> edgeFactory)
            : base(host, visitedGraph, vertexFactory, edgeFactory)
        {
        }

        /// <inheritdoc />
        protected override void AugmentGraph()
        {
            foreach (TVertex vertex in VisitedGraph.Vertices)
            {
                ThrowIfCancellationRequested();

                // Is source
                if (VisitedGraph.IsInEdgesEmpty(vertex))
                    AddAugmentedEdge(SuperSource, vertex);

                // Is sink
                if (VisitedGraph.IsOutEdgesEmpty(vertex))
                    AddAugmentedEdge(vertex, SuperSink);
            }
        }
    }
}