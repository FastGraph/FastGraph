using System;
using System.Collections.Generic;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using static QuikGraph.Utils.DisposableHelpers;

namespace QuikGraph.Algorithms.Observers
{
    /// <summary>
    /// A distance recorder for directed tree builder algorithms
    /// </summary>
    /// <typeparam name="TVertex">type of a vertex</typeparam>
    /// <typeparam name="TEdge">type of an edge</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class VertexDistanceRecorderObserver<TVertex, TEdge>
        : IObserver<ITreeBuilderAlgorithm<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        private readonly IDistanceRelaxer distanceRelaxer;
        private readonly Func<TEdge, double> edgeWeights;
        private readonly IDictionary<TVertex, double> distances;

        public VertexDistanceRecorderObserver(Func<TEdge, double> edgeWeights)
            : this(edgeWeights, DistanceRelaxers.EdgeShortestDistance, new Dictionary<TVertex, double>())
        { }

        public VertexDistanceRecorderObserver(
            Func<TEdge, double> edgeWeights,
            IDistanceRelaxer distanceRelaxer,
            IDictionary<TVertex, double> distances)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(edgeWeights != null);
            Contract.Requires(distanceRelaxer != null);
            Contract.Requires(distances != null);
#endif

            this.edgeWeights = edgeWeights;
            this.distanceRelaxer = distanceRelaxer;
            this.distances = distances;
        }

        public IDistanceRelaxer DistanceRelaxer
        {
            get { return this.distanceRelaxer; }
        }

        public Func<TEdge, double> EdgeWeights
        {
            get { return this.edgeWeights; }
        }

        public IDictionary<TVertex, double> Distances
        {
            get { return this.distances; }
        }

        public IDisposable Attach(ITreeBuilderAlgorithm<TVertex, TEdge> algorithm)
        {
            algorithm.TreeEdge += TreeEdge;
            return Finally(() => algorithm.TreeEdge -= TreeEdge);
        }

        private void TreeEdge(TEdge edge)
        {
            var source = edge.Source;
            var target = edge.Target;

            double sourceDistance;
            if (!this.distances.TryGetValue(source, out sourceDistance))
                this.distances[source] = sourceDistance = this.distanceRelaxer.InitialDistance;
            this.distances[target] = this.DistanceRelaxer.Combine(sourceDistance, this.edgeWeights(edge));
        }
    }
}
