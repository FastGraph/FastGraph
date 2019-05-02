using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using static QuikGraph.Utils.DisposableHelpers;

namespace QuikGraph.Algorithms.Observers
{
    /// <summary>
    /// Recorder of encountered edges.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class EdgeRecorderObserver<TVertex, TEdge> : IObserver<ITreeBuilderAlgorithm<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EdgeRecorderObserver{TVertex,TEdge}"/> class.
        /// </summary>
        public EdgeRecorderObserver()
            : this(new List<TEdge>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EdgeRecorderObserver{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="edges">Set of edges.</param>
        public EdgeRecorderObserver([NotNull, ItemNotNull] IEnumerable<TEdge> edges)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(edges != null);
#endif

            _edges = edges.ToList();
        }

        [NotNull, ItemNotNull]
        private readonly IList<TEdge> _edges;

        /// <summary>
        /// Encountered edges.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull, ItemNotNull]
        public IEnumerable<TEdge> Edges => _edges.AsEnumerable();

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
            _edges.Add(edge);
        }
    }
}
