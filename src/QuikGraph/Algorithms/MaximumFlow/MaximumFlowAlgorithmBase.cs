using System;
using System.Collections.Generic;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using JetBrains.Annotations;
using QuikGraph.Algorithms.Services;

namespace QuikGraph.Algorithms.MaximumFlow
{
    /// <summary>
    /// Base class for all maximum flow algorithms.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public abstract class MaximumFlowAlgorithm<TVertex, TEdge> 
        : AlgorithmBase<IMutableVertexAndEdgeListGraph<TVertex, TEdge>>
        , IVertexColorizerAlgorithm<TVertex>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MaximumFlowAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="capacities">Function that given an edge return the capacity of this edge.</param>
        /// <param name="edgeFactory">Edge factory method.</param>
        protected MaximumFlowAlgorithm(
            [CanBeNull] IAlgorithmComponent host,
            [NotNull] IMutableVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph,
            [NotNull] Func<TEdge, double> capacities,
            [NotNull] EdgeFactory<TVertex, TEdge> edgeFactory)
            : base(host, visitedGraph)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(capacities != null);
#endif

            Capacities = capacities;
            EdgeFactory = edgeFactory;
        }

        #region Properties

        /// <summary>
        /// Flow vertices predecessors.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        public Dictionary<TVertex, TEdge> Predecessors { get; } = new Dictionary<TVertex, TEdge>();

        /// <summary>
        /// Function that given an edge return the capacity of this edge.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        public Func<TEdge, double> Capacities { get; }

        /// <summary>
        /// Residual capacities per edge.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        public Dictionary<TEdge, double> ResidualCapacities { get; } = new Dictionary<TEdge, double>();

        /// <summary>
        /// Edge factory method.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        public EdgeFactory<TVertex, TEdge> EdgeFactory { get; }

        /// <summary>
        /// Graph reversed edges.
        /// </summary>
        /// <remarks>Should be not null but may be empty.</remarks>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        public Dictionary<TEdge, TEdge> ReversedEdges { get; protected set; }

        /// <summary>
        /// Flow source vertex.
        /// </summary>
        /// <remarks>Must not be null to run the algorithm.</remarks>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        public TVertex Source { get; set; }

        /// <summary>
        /// Flow sink vertex.
        /// </summary>
        /// <remarks>Must not be null to run the algorithm.</remarks>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        public TVertex Sink { get; set; }

        /// <summary>
        /// Maximal flow value.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        public double MaxFlow { get; protected set; }

        /// <summary>
        /// Stores vertices associated to their colors (treatment state).
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        public IDictionary<TVertex, GraphColor> VerticesColors { get; } = new Dictionary<TVertex, GraphColor>();

        #region IVertexColorizerAlgorithm<TVertex>

        /// <inheritdoc />
        public GraphColor GetVertexColor(TVertex vertex)
        {
            return VerticesColors[vertex];
        }

        #endregion

        #endregion

        /// <summary>
        /// Compute the maximum flow value.
        /// </summary>
        /// <param name="source">Flow source vertex.</param>
        /// <param name="sink">Flow sink vertex.</param>
        /// <returns>Maximum flow value.</returns>
        public double Compute([NotNull] TVertex source, [NotNull] TVertex sink)
        {
            Source = source;
            Sink = sink;

            Compute();

            return MaxFlow;
        }
    }

}
