using System;
using System.Collections.Generic;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using JetBrains.Annotations;
using static QuikGraph.Utils.DisposableHelpers;

namespace QuikGraph.Algorithms.Observers
{
    /// <summary>
    /// Recorder of vertices discover timestamps.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class VertexTimeStamperObserver<TVertex> : IObserver<IVertexTimeStamperAlgorithm<TVertex>>
    {
        private int _currentTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexTimeStamperObserver{TVertex}"/> class.
        /// </summary>
        public VertexTimeStamperObserver()
            : this(new Dictionary<TVertex, int>(), new Dictionary<TVertex, int>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexTimeStamperObserver{TVertex}"/> class.
        /// </summary>
        /// <param name="discoverTimes">Vertices discover times.</param>
        public VertexTimeStamperObserver([NotNull] IDictionary<TVertex, int> discoverTimes)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(discoverTimes != null);
#endif

            DiscoverTimes = discoverTimes;
            FinishTimes = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexTimeStamperObserver{TVertex}"/> class.
        /// </summary>
        /// <param name="discoverTimes">Vertices discover times.</param>
        /// <param name="finishTimes">Vertices fully treated times.</param>
        public VertexTimeStamperObserver(
            [NotNull] IDictionary<TVertex, int> discoverTimes,
            [NotNull] IDictionary<TVertex, int> finishTimes)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(discoverTimes != null);
            Contract.Requires(finishTimes != null);
#endif

            DiscoverTimes = discoverTimes;
            FinishTimes = finishTimes;
        }

        /// <summary>
        /// Times of vertices discover.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        public IDictionary<TVertex, int> DiscoverTimes { get; }

        /// <summary>
        /// Times of vertices fully treated.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [CanBeNull]
        public IDictionary<TVertex, int> FinishTimes { get; }

        #region IObserver<TAlgorithm>

        /// <inheritdoc />
        public IDisposable Attach(IVertexTimeStamperAlgorithm<TVertex> algorithm)
        {
            algorithm.DiscoverVertex += OnVertexDiscovered;
            if (FinishTimes != null)
                algorithm.FinishVertex += OnVertexFinished;

            return Finally(() =>
            {
                algorithm.DiscoverVertex -= OnVertexDiscovered;
                if (FinishTimes != null)
                    algorithm.FinishVertex -= OnVertexFinished;
            });
        }

        #endregion

        private void OnVertexDiscovered([NotNull] TVertex vertex)
        {
            DiscoverTimes[vertex] = _currentTime++;
        }

        private void OnVertexFinished([NotNull] TVertex vertex)
        {
            // ReSharper disable once PossibleNullReferenceException, Justification: not null if the handler is attached
            FinishTimes[vertex] = _currentTime++;
        }
    }
}
