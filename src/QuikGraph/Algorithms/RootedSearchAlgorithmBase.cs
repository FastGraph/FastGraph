using System;
using JetBrains.Annotations;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using QuikGraph.Algorithms.Services;

namespace QuikGraph.Algorithms
{
    /// <summary>
    /// Base class for all graph algorithm performing a search in a graph.
    /// </summary>
    /// <remarks>Requires a starting vertex (root).</remarks>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TGraph">Graph type.</typeparam>
    public abstract class RootedSearchAlgorithmBase<TVertex, TGraph> : RootedAlgorithmBase<TVertex, TGraph>
    {
        private TVertex _target;
        private bool _hasTargetVertex;

        /// <summary>
        /// Initializes a new instance of the <see cref="RootedSearchAlgorithmBase{TVertex,TGraph}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        protected RootedSearchAlgorithmBase(
            [CanBeNull] IAlgorithmComponent host,
            [NotNull] TGraph visitedGraph)
            : base(host, visitedGraph)
        {
        }

        /// <summary>
        /// Tries to get the target vertex if set.
        /// </summary>
        /// <param name="target">Target vertex if set, otherwise null.</param>
        /// <returns>True if the target vertex was set, false otherwise.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        public bool TryGetTargetVertex(out TVertex target)
        {
            if (_hasTargetVertex)
            {
                target = _target;
                return true;
            }

            target = default(TVertex);
            return false;
        }

        /// <summary>
        /// Sets the target vertex.
        /// </summary>
        /// <param name="target">Target vertex.</param>
        public void SetTargetVertex([NotNull] TVertex target)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(target != null);
#endif

            bool changed = !Equals(_target, target);
            _target = target;
            if (changed)
                OnTargetVertexChanged(EventArgs.Empty);
            _hasTargetVertex = true;
        }

        /// <summary>
        /// Clears the target vertex.
        /// </summary>
        public void ClearTargetVertex()
        {
            _target = default(TVertex);
            _hasTargetVertex = false;
        }

        /// <summary>
        /// Fired when the target vertex is changed.
        /// </summary>
        public event EventHandler TargetVertexChanged;

        /// <summary>
        /// Called on each target vertex change.
        /// </summary>
        /// <param name="args"><see cref="EventArgs.Empty"/>.</param>
        protected virtual void OnTargetVertexChanged([NotNull] EventArgs args)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(args != null);
#endif

            TargetVertexChanged?.Invoke(this, args);
        }

        /// <summary>
        /// Fired when the target vertex is reached.
        /// </summary>
        public event EventHandler TargetReached;

        /// <summary>
        /// Called when the target vertex is reached.
        /// </summary>
        protected virtual void OnTargetReached()
        {
            TargetReached?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Runs the algorithm with the given <paramref name="root"/> vertex.
        /// </summary>
        /// <param name="root">Root vertex.</param>
        /// <param name="target">Target vertex.</param>
        public void Compute([NotNull] TVertex root, [NotNull] TVertex target)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(root != null);
            Contract.Requires(target != null);
#endif

            SetTargetVertex(target);
            Compute(root);
        }
    }
}
