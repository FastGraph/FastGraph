using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using static QuikGraph.Utils.DisposableHelpers;

namespace QuikGraph.Algorithms.Observers
{
    /// <summary>
    /// A distance recorder for <see cref="ITreeBuilderAlgorithm{TVertex,TEdge}"/> algorithms.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class VertexDistanceRecorderObserver<TVertex, TEdge> : IObserver<ITreeBuilderAlgorithm<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VertexDistanceRecorderObserver{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="edgeWeights">Function that computes the weight for a given edge.</param>
        public VertexDistanceRecorderObserver(Func<TEdge, double> edgeWeights)
            : this(edgeWeights, DistanceRelaxers.EdgeShortestDistance, new Dictionary<TVertex, double>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexDistanceRecorderObserver{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="edgeWeights">Function that computes the weight for a given edge.</param>
        /// <param name="distanceRelaxer">Distance relaxer.</param>
        /// <param name="distances">Distances per vertex.</param>
        public VertexDistanceRecorderObserver(
            [NotNull] Func<TEdge, double> edgeWeights,
            [NotNull] IDistanceRelaxer distanceRelaxer,
            [NotNull] IDictionary<TVertex, double> distances)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(edgeWeights != null);
            Contract.Requires(distanceRelaxer != null);
            Contract.Requires(distances != null);
#endif

            EdgeWeights = edgeWeights;
            DistanceRelaxer = distanceRelaxer;
            Distances = distances;
        }

        /// <summary>
        /// Distance relaxer.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        public IDistanceRelaxer DistanceRelaxer { get; }

        /// <summary>
        /// Function that computes the weight for a given edge.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        public Func<TEdge, double> EdgeWeights { get; }

        /// <summary>
        /// Distances per vertex.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        public IDictionary<TVertex, double> Distances { get; }

        #region IObserver<TAlgorithm>

        /// <inheritdoc />
        public IDisposable Attach(ITreeBuilderAlgorithm<TVertex, TEdge> algorithm)
        {
            algorithm.TreeEdge += OnEdgeDiscovered;
            return Finally(() => algorithm.TreeEdge -= OnEdgeDiscovered);
        }

        #endregion

        private void OnEdgeDiscovered([NotNull] TEdge edge)
        {
            TVertex source = edge.Source;

            if (!Distances.TryGetValue(source, out double sourceDistance))
                Distances[source] = sourceDistance = DistanceRelaxer.InitialDistance;
            Distances[edge.Target] = DistanceRelaxer.Combine(sourceDistance, EdgeWeights(edge));
        }
    }
}
