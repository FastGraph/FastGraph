using System;
using JetBrains.Annotations;
using QuikGraph.Algorithms.Services;

namespace QuikGraph.Algorithms
{
    /// <summary>
    /// Base class for all graph algorithm requiring a starting vertex (root).
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TGraph">Graph type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public abstract class RootedAlgorithmBase<TVertex, TGraph> : AlgorithmBase<TGraph>
    {
        private TVertex _root;
        private bool _hasRootVertex;

        /// <summary>
        /// Initializes a new instance of the <see cref="RootedAlgorithmBase{TVertex,TGraph}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        protected RootedAlgorithmBase(
            [CanBeNull] IAlgorithmComponent host,
            [NotNull] TGraph visitedGraph)
            : base(host, visitedGraph)
        {
        }

        /// <summary>
        /// Tries to get the root vertex if set.
        /// </summary>
        /// <param name="root">Root vertex if set, otherwise null.</param>
        /// <returns>True if the root vertex was set, false otherwise.</returns>
        [Pure]
        public bool TryGetRootVertex(out TVertex root)
        {
            if (_hasRootVertex)
            {
                root = _root;
                return true;
            }

            root = default(TVertex);
            return false;
        }

        /// <summary>
        /// Sets the root vertex.
        /// </summary>
        /// <param name="root">Root vertex.</param>
        public void SetRootVertex([NotNull] TVertex root)
        {
            if (root == null)
                throw new ArgumentNullException(nameof(root));

            bool changed = !Equals(_root, root);
            _root = root;
            if (changed)
                OnRootVertexChanged(EventArgs.Empty);
            _hasRootVertex = true;
        }

        /// <summary>
        /// Clears the root vertex.
        /// </summary>
        public void ClearRootVertex()
        {
            _root = default(TVertex);
            _hasRootVertex = false;
        }

        /// <summary>
        /// Fired when the root vertex is changed.
        /// </summary>
        public event EventHandler RootVertexChanged;

        /// <summary>
        /// Called on each root vertex change.
        /// </summary>
        /// <param name="args"><see cref="EventArgs.Empty"/>.</param>
        protected virtual void OnRootVertexChanged([NotNull] EventArgs args)
        {
            if (args is null)
                throw new ArgumentNullException(nameof(args));

            RootVertexChanged?.Invoke(this, args);
        }

        /// <summary>
        /// Runs the algorithm with the given <paramref name="root"/> vertex.
        /// </summary>
        /// <param name="root">Root vertex.</param>
        public void Compute([NotNull] TVertex root)
        {
            if (root == null)
                throw new ArgumentNullException(nameof(root));

            SetRootVertex(root);
            Compute();
        }
    }
}
