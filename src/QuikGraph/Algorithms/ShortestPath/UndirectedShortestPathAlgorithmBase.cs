using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using QuikGraph.Algorithms.Services;

namespace QuikGraph.Algorithms.ShortestPath
{
    /// <summary>
    /// Base class for all shortest path finder algorithms in undirected graphs.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public abstract class UndirectedShortestPathAlgorithmBase<TVertex, TEdge>
        : RootedAlgorithmBase<TVertex, IUndirectedGraph<TVertex, TEdge>>
        , IVertexColorizerAlgorithm<TVertex>
        , IUndirectedTreeBuilderAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UndirectedShortestPathAlgorithmBase{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="edgeWeights">Function that computes the weight for a given edge.</param>
        protected UndirectedShortestPathAlgorithmBase(
            [CanBeNull] IAlgorithmComponent host,
            [NotNull] IUndirectedGraph<TVertex, TEdge> visitedGraph,
            [NotNull] Func<TEdge, double> edgeWeights)
            : this(host, visitedGraph, edgeWeights, DistanceRelaxers.ShortestDistance)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UndirectedShortestPathAlgorithmBase{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="edgeWeights">Function that computes the weight for a given edge.</param>
        /// <param name="distanceRelaxer">Distance relaxer.</param>
        protected UndirectedShortestPathAlgorithmBase(
            [CanBeNull] IAlgorithmComponent host,
            [NotNull] IUndirectedGraph<TVertex, TEdge> visitedGraph,
            [NotNull] Func<TEdge, double> edgeWeights,
            [NotNull] IDistanceRelaxer distanceRelaxer)
            : base(host, visitedGraph)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(Weights != null);
            Contract.Requires(distanceRelaxer != null);
#endif

            Weights = edgeWeights;
            DistanceRelaxer = distanceRelaxer;
        }

        /// <summary>
        /// Tries to get the distance associated to the given <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="distance">Associated distance.</param>
        /// <returns>True if the distance was found, false otherwise.</returns>
        public bool TryGetDistance([NotNull] TVertex vertex, out double distance)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            return Distances.TryGetValue(vertex, out distance);
        }

        /// <summary>
        /// Vertices distances.
        /// </summary>
        public IDictionary<TVertex, double> Distances { get; private set; }

        /// <summary>
        /// Gets the function that gives access to distances from a vertex.
        /// </summary>
        [JetBrains.Annotations.Pure]
        [NotNull]
        protected Func<TVertex, double> DistancesIndexGetter()
        {
            return AlgorithmExtensions.GetIndexer(Distances);
        }

        /// <summary>
        /// Function that given an edge return the weight of this edge.
        /// </summary>
        [NotNull]
        public Func<TEdge, double> Weights { get; }

        /// <summary>
        /// Distance relaxer.
        /// </summary>
        [NotNull]
        public IDistanceRelaxer DistanceRelaxer { get; }

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void Initialize()
        {
            base.Initialize();

            VerticesColors = new Dictionary<TVertex, GraphColor>(VisitedGraph.VertexCount);
            Distances = new Dictionary<TVertex, double>(VisitedGraph.VertexCount);
        }

        #endregion

        #region IVertexColorizerAlgorithm<TVertex>

        /// <summary>
        /// Stores vertices associated to their colors (treatment state).
        /// </summary>
        public IDictionary<TVertex, GraphColor> VerticesColors { get; private set; }

        /// <inheritdoc />
        public GraphColor GetVertexColor(TVertex vertex)
        {
            return VerticesColors[vertex];
        }

        #endregion

        /// <summary>
        /// Fired when the distance label for the target vertex is decreased.
        /// The edge that participated in the last relaxation for vertex v is
        /// an edge in the shortest paths tree.
        /// </summary>
        public event UndirectedEdgeAction<TVertex, TEdge> TreeEdge;

        /// <summary>
        /// Called on each <see cref="TreeEdge"/> event.
        /// </summary>
        /// <param name="edge">Concerned edge.</param>
        /// <param name="reversed">Indicates if the edge is reversed.</param>
        protected virtual void OnTreeEdge([NotNull] TEdge edge, bool reversed)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            TreeEdge?.Invoke(
                this,
                new UndirectedEdgeEventArgs<TVertex, TEdge>(edge, reversed));
        }

        /// <summary>
        /// Runs the relaxation algorithm on the given <paramref name="edge"/>.
        /// </summary>
        /// <param name="edge">Edge to relax.</param>
        /// <param name="source">Source vertex.</param>
        /// <param name="target">Target vertex.</param>
        /// <returns>True if relaxation decreased the target vertex distance, false otherwise.</returns>
        protected bool Relax(TEdge edge, TVertex source, TVertex target)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(edge != null);
            Contract.Requires(source != null);
            Contract.Requires(target != null);
            Contract.Requires(
                (edge.Source.Equals(source) && edge.Target.Equals(target))
                ||
                (edge.Source.Equals(target) && edge.Target.Equals(source)));
#endif

            double du = Distances[source];
            double dv = Distances[target];
            double we = Weights(edge);

            IDistanceRelaxer relaxer = DistanceRelaxer;
            double duwe = relaxer.Combine(du, we);
            if (relaxer.Compare(duwe, dv) < 0)
            {
                Distances[target] = duwe;
                return true;
            }

            return false;
        }
    }
}
