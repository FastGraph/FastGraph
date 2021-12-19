#nullable enable

using FastGraph.Algorithms.MaximumFlow;

namespace FastGraph.Algorithms
{
    /// <summary>
    /// Algorithm that computes a maximum bipartite matching in a graph, meaning
    /// the maximum number of edges not sharing any vertex.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public sealed class MaximumBipartiteMatchingAlgorithm<TVertex, TEdge> : AlgorithmBase<IMutableVertexAndEdgeListGraph<TVertex, TEdge>>
        where TVertex : notnull
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MaximumBipartiteMatchingAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="sourceToVertices">Vertices to which creating augmented edge from super source.</param>
        /// <param name="verticesToSink">Vertices from which creating augmented edge to super sink.</param>
        /// <param name="vertexFactory">Vertex factory method.</param>
        /// <param name="edgeFactory">Edge factory method.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="sourceToVertices"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="verticesToSink"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="vertexFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="edgeFactory"/> is <see langword="null"/>.</exception>
        public MaximumBipartiteMatchingAlgorithm(
            IMutableVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph,
            IEnumerable<TVertex> sourceToVertices,
            IEnumerable<TVertex> verticesToSink,
            VertexFactory<TVertex> vertexFactory,
            EdgeFactory<TVertex, TEdge> edgeFactory)
            : base(visitedGraph)
        {
            SourceToVertices = sourceToVertices ?? throw new ArgumentNullException(nameof(sourceToVertices));
            VerticesToSink = verticesToSink ?? throw new ArgumentNullException(nameof(verticesToSink));
            VertexFactory = vertexFactory ?? throw new ArgumentNullException(nameof(vertexFactory));
            EdgeFactory = edgeFactory ?? throw new ArgumentNullException(nameof(edgeFactory));
        }

        /// <summary>
        /// Vertices to which augmented edge from super source are created with augmentation.
        /// </summary>
        public IEnumerable<TVertex> SourceToVertices { get; }

        /// <summary>
        /// Vertices from which augmented edge to super sink are created with augmentation.
        /// </summary>
        public IEnumerable<TVertex> VerticesToSink { get; }

        /// <summary>
        /// Vertex factory method.
        /// </summary>
        public VertexFactory<TVertex> VertexFactory { get; }

        /// <summary>
        /// Edge factory method.
        /// </summary>
        public EdgeFactory<TVertex, TEdge> EdgeFactory { get; }


        private readonly List<TEdge> _matchedEdges = new List<TEdge>();

        /// <summary>
        /// Maximal edges matching.
        /// </summary>
        public TEdge[] MatchedEdges => _matchedEdges.ToArray();

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void Initialize()
        {
            base.Initialize();

            _matchedEdges.Clear();
        }

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            BipartiteToMaximumFlowGraphAugmentorAlgorithm<TVertex, TEdge>? augmentor = default;
            ReversedEdgeAugmentorAlgorithm<TVertex, TEdge>? reverser = default;

            try
            {
                ThrowIfCancellationRequested();

                // Augmenting the graph
                augmentor = new BipartiteToMaximumFlowGraphAugmentorAlgorithm<TVertex, TEdge>(
                    this,
                    VisitedGraph,
                    SourceToVertices,
                    VerticesToSink,
                    VertexFactory,
                    EdgeFactory);
                augmentor.Compute();

                ThrowIfCancellationRequested();

                // Adding reverse edges
                reverser = new ReversedEdgeAugmentorAlgorithm<TVertex, TEdge>(
                    VisitedGraph,
                    EdgeFactory);
                reverser.AddReversedEdges();

                ThrowIfCancellationRequested();

                // Compute maximum flow
                var flow = new EdmondsKarpMaximumFlowAlgorithm<TVertex, TEdge>(
                    this,
                    VisitedGraph,
                    _ => 1.0,
                    EdgeFactory,
                    reverser);

                flow.Compute(augmentor.SuperSource!, augmentor.SuperSink!);

                ThrowIfCancellationRequested();

                foreach (TEdge edge in VisitedGraph.Edges)
                {
                    if (Math.Abs(flow.ResidualCapacities[edge]) < float.Epsilon)
                    {
                        if (EqualityComparer<TVertex?>.Default.Equals(edge.Source, augmentor.SuperSource)
                            || EqualityComparer<TVertex?>.Default.Equals(edge.Source, augmentor.SuperSink)
                            || EqualityComparer<TVertex?>.Default.Equals(edge.Target, augmentor.SuperSource)
                            || EqualityComparer<TVertex?>.Default.Equals(edge.Target, augmentor.SuperSink))
                        {
                            // Skip all edges that connect to SuperSource or SuperSink
                            continue;
                        }

                        _matchedEdges.Add(edge);
                    }
                }
            }
            finally
            {
                if (reverser != default && reverser.Augmented)
                {
                    reverser.RemoveReversedEdges();
                }
                if (augmentor != default && augmentor.Augmented)
                {
                    augmentor.Rollback();
                }
            }
        }

        #endregion
    }
}
