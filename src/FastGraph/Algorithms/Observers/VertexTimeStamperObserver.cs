#nullable enable

using static FastGraph.Utils.DisposableHelpers;

namespace FastGraph.Algorithms.Observers
{
    /// <summary>
    /// Recorder of vertices discover timestamps.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class VertexTimeStamperObserver<TVertex> : IObserver<IVertexTimeStamperAlgorithm<TVertex>>
        where TVertex : notnull
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
        /// <exception cref="T:System.ArgumentNullException"><paramref name="discoverTimes"/> is <see langword="null"/>.</exception>
        public VertexTimeStamperObserver(IDictionary<TVertex, int> discoverTimes)
        {
            DiscoverTimes = discoverTimes ?? throw new ArgumentNullException(nameof(discoverTimes));
            FinishTimes = default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexTimeStamperObserver{TVertex}"/> class.
        /// </summary>
        /// <param name="discoverTimes">Vertices discover times.</param>
        /// <param name="finishTimes">Vertices fully treated times.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="discoverTimes"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="finishTimes"/> is <see langword="null"/>.</exception>
        public VertexTimeStamperObserver(
            IDictionary<TVertex, int> discoverTimes,
            IDictionary<TVertex, int> finishTimes)
        {
            DiscoverTimes = discoverTimes ?? throw new ArgumentNullException(nameof(discoverTimes));
            FinishTimes = finishTimes ?? throw new ArgumentNullException(nameof(finishTimes));
        }

        /// <summary>
        /// Times of vertices discover.
        /// </summary>
        public IDictionary<TVertex, int> DiscoverTimes { get; }

        /// <summary>
        /// Times of vertices fully treated.
        /// </summary>
        public IDictionary<TVertex, int>? FinishTimes { get; }

        #region IObserver<TAlgorithm>

        /// <inheritdoc />
        public IDisposable Attach(IVertexTimeStamperAlgorithm<TVertex> algorithm)
        {
            if (algorithm is null)
                throw new ArgumentNullException(nameof(algorithm));

            algorithm.DiscoverVertex += OnVertexDiscovered;
            if (FinishTimes != default)
            {
                algorithm.FinishVertex += OnVertexFinished;
            }

            return Finally(() =>
            {
                algorithm.DiscoverVertex -= OnVertexDiscovered;
                if (FinishTimes != default)
                {
                    algorithm.FinishVertex -= OnVertexFinished;
                }
            });
        }

        #endregion

        private void OnVertexDiscovered(TVertex vertex)
        {
            DiscoverTimes[vertex] = _currentTime++;
        }

        private void OnVertexFinished(TVertex vertex)
        {
            // ReSharper disable once PossibleNullReferenceException, Justification: Not default if the handler is attached
            FinishTimes![vertex] = _currentTime++;
        }
    }
}
