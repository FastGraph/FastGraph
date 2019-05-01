using System;
using System.Collections.Generic;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using JetBrains.Annotations;
using QuikGraph.Algorithms.MaximumFlow;
using QuikGraph.Algorithms.Services;

namespace QuikGraph.Algorithms
{
    /// <summary>
    /// Algorithm that computes a maximum bipartite matching in a graph, meaning
    /// the maximum number of edges not sharing any vertex.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public sealed class MaximumBipartiteMatchingAlgorithm<TVertex, TEdge> : AlgorithmBase<IMutableVertexAndEdgeListGraph<TVertex, TEdge>>
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
        public MaximumBipartiteMatchingAlgorithm(
            [NotNull] IMutableVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph,
            [NotNull, ItemNotNull] IEnumerable<TVertex> sourceToVertices,
            [NotNull, ItemNotNull] IEnumerable<TVertex> verticesToSink,
            [NotNull] VertexFactory<TVertex> vertexFactory,
            [NotNull] EdgeFactory<TVertex, TEdge> edgeFactory)
            : base(visitedGraph)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(vertexFactory != null);
            Contract.Requires(edgeFactory != null);
#endif

            SourceToVertices = sourceToVertices;
            VerticesToSink = verticesToSink;
            VertexFactory = vertexFactory;
            EdgeFactory = edgeFactory;
            MatchedEdges = new List<TEdge>();
        }

        /// <summary>
        /// Vertices to which augmented edge from super source are created with augmentation.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull, ItemNotNull]
        public IEnumerable<TVertex> SourceToVertices { get; }

        /// <summary>
        /// Vertices from which augmented edge to super sink are created with augmentation.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull, ItemNotNull]
        public IEnumerable<TVertex> VerticesToSink { get; }

        /// <summary>
        /// Vertex factory method.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        public VertexFactory<TVertex> VertexFactory { get; }

        /// <summary>
        /// Edge factory method.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        public EdgeFactory<TVertex, TEdge> EdgeFactory { get; }

        /// <summary>
        /// Maximal edges matching.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        public ICollection<TEdge> MatchedEdges { get; }

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            ICancelManager cancelManager = Services.CancelManager;
            MatchedEdges.Clear();

            BipartiteToMaximumFlowGraphAugmentorAlgorithm<TVertex, TEdge> augmentor = null;
            ReversedEdgeAugmentorAlgorithm<TVertex, TEdge> reverser = null;

            try
            {
                if (cancelManager.IsCancelling)
                    return;

                // Augmenting the graph
                augmentor = new BipartiteToMaximumFlowGraphAugmentorAlgorithm<TVertex, TEdge>(
                    this,
                    VisitedGraph,
                    SourceToVertices,
                    VerticesToSink,
                    VertexFactory,
                    EdgeFactory);
                augmentor.Compute();

                if (cancelManager.IsCancelling)
                    return;

                // Adding reverse edges
                reverser = new ReversedEdgeAugmentorAlgorithm<TVertex, TEdge>(
                    this,
                    VisitedGraph,
                    EdgeFactory);
                reverser.AddReversedEdges();

                if (cancelManager.IsCancelling)
                    return;

                // Compute maximum flow
                var flow = new EdmondsKarpMaximumFlowAlgorithm<TVertex, TEdge>(
                    this,
                    VisitedGraph,
                    edge => 1,
                    EdgeFactory,
                    reverser);

                flow.Compute(augmentor.SuperSource, augmentor.SuperSink);

                if (cancelManager.IsCancelling)
                    return;

                foreach (TEdge edge in VisitedGraph.Edges)
                {
                    if (Math.Abs(flow.ResidualCapacities[edge]) < float.Epsilon)
                    {
                        if (edge.Source.Equals(augmentor.SuperSource) 
                            || edge.Source.Equals(augmentor.SuperSink) 
                            || edge.Target.Equals(augmentor.SuperSource) 
                            || edge.Target.Equals(augmentor.SuperSink))
                        {
                            // Skip all edges that connect to SuperSource or SuperSink
                            continue;
                        }

                        MatchedEdges.Add(edge);
                    }
                }
            }
            finally
            {
                if (reverser != null && reverser.Augmented)
                {
                    reverser.RemoveReversedEdges();
                }
                if (augmentor != null && augmentor.Augmented)
                {
                    augmentor.Rollback();
                }
            }
        }

        #endregion
    }
}
