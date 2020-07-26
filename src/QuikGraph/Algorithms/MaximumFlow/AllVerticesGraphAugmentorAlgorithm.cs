using JetBrains.Annotations;
using QuikGraph.Algorithms.Services;

namespace QuikGraph.Algorithms.MaximumFlow
{
    /// <summary>
    /// Algorithm that augment all vertices of a graph by adding edge between
    /// all vertices from super source and to super sink.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public sealed class AllVerticesGraphAugmentorAlgorithm<TVertex, TEdge>
        : GraphAugmentorAlgorithmBase<TVertex, TEdge, IMutableVertexAndEdgeSet<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AllVerticesGraphAugmentorAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="vertexFactory">Vertex factory method.</param>
        /// <param name="edgeFactory">Edge factory method.</param>
        public AllVerticesGraphAugmentorAlgorithm(
            [NotNull] IMutableVertexAndEdgeSet<TVertex, TEdge> visitedGraph,
            [NotNull] VertexFactory<TVertex> vertexFactory,
            [NotNull] EdgeFactory<TVertex, TEdge> edgeFactory)
            : this(null, visitedGraph, vertexFactory, edgeFactory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllVerticesGraphAugmentorAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="vertexFactory">Vertex factory method.</param>
        /// <param name="edgeFactory">Edge factory method.</param>
        public AllVerticesGraphAugmentorAlgorithm(
            [CanBeNull] IAlgorithmComponent host,
            [NotNull] IMutableVertexAndEdgeSet<TVertex, TEdge> visitedGraph,
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

                AddAugmentedEdge(SuperSource, vertex);
                AddAugmentedEdge(vertex, SuperSink);
            }
        }
    }
}