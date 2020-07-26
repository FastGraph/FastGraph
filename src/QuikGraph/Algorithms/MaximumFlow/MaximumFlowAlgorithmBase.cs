using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using QuikGraph.Algorithms.Services;

namespace QuikGraph.Algorithms.MaximumFlow
{
    /// <summary>
    /// Base class for all maximum flow algorithms.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
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
            Capacities = capacities ?? throw new ArgumentNullException(nameof(capacities));
            EdgeFactory = edgeFactory ?? throw new ArgumentNullException(nameof(edgeFactory));
        }

        #region Properties

        /// <summary>
        /// Flow vertices predecessors.
        /// </summary>
        [NotNull]
        public Dictionary<TVertex, TEdge> Predecessors { get; } = new Dictionary<TVertex, TEdge>();

        /// <summary>
        /// Function that given an edge return the capacity of this edge.
        /// </summary>
        [NotNull]
        public Func<TEdge, double> Capacities { get; }

        /// <summary>
        /// Residual capacities per edge.
        /// </summary>
        [NotNull]
        public Dictionary<TEdge, double> ResidualCapacities { get; } = new Dictionary<TEdge, double>();

        /// <summary>
        /// Edge factory method.
        /// </summary>
        [NotNull]
        public EdgeFactory<TVertex, TEdge> EdgeFactory { get; }

        /// <summary>
        /// Graph reversed edges.
        /// </summary>
        /// <remarks>Should be not null but may be empty.</remarks>
        public IDictionary<TEdge, TEdge> ReversedEdges { get; protected set; }

        /// <summary>
        /// Flow source vertex.
        /// </summary>
        /// <remarks>Must not be null to run the algorithm.</remarks>
        public TVertex Source { get; set; }

        /// <summary>
        /// Flow sink vertex.
        /// </summary>
        /// <remarks>Must not be null to run the algorithm.</remarks>
        public TVertex Sink { get; set; }

        /// <summary>
        /// Maximal flow value.
        /// </summary>
        public double MaxFlow { get; protected set; }

        /// <summary>
        /// Stores vertices associated to their colors (treatment state).
        /// </summary>
        [NotNull]
        public IDictionary<TVertex, GraphColor> VerticesColors { get; } = new Dictionary<TVertex, GraphColor>();

        #region IVertexColorizerAlgorithm<TVertex>

        /// <inheritdoc />
        public GraphColor GetVertexColor(TVertex vertex)
        {
            if (VerticesColors.TryGetValue(vertex, out GraphColor color))
                return color;
            throw new VertexNotFoundException();
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
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (sink == null)
                throw new ArgumentNullException(nameof(sink));

            Source = source;
            Sink = sink;

            Compute();

            return MaxFlow;
        }
    }

}