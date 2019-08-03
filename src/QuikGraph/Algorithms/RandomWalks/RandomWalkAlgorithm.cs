using System;
using JetBrains.Annotations;

namespace QuikGraph.Algorithms.RandomWalks
{
    /// <summary>
    /// Random walk algorithm (using edge chain).
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class RandomWalkAlgorithm<TVertex, TEdge>
        : RootedAlgorithmBase<TVertex, IImplicitGraph<TVertex, TEdge>>
        , ITreeBuilderAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RandomWalkAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        public RandomWalkAlgorithm([NotNull] IImplicitGraph<TVertex, TEdge> visitedGraph)
            : this(visitedGraph, new NormalizedMarkovEdgeChain<TVertex, TEdge>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomWalkAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="edgeChain">Edge chain strategy to use.</param>
        public RandomWalkAlgorithm(
            [NotNull] IImplicitGraph<TVertex, TEdge> visitedGraph,
            [NotNull] IEdgeChain<TVertex, TEdge> edgeChain)
            : base(null, visitedGraph)
        {
            if (edgeChain is null)
                throw new ArgumentNullException(nameof(edgeChain));

            EdgeChain = edgeChain;
        }

        /// <summary>
        /// Edge chain strategy for the random walk.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        public IEdgeChain<TVertex, TEdge> EdgeChain { get; set; }

        /// <summary>
        /// Predicate to prematurely ends the walk.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [CanBeNull]
        public EdgePredicate<TVertex, TEdge> EndPredicate { get; set; }

        /// <summary>
        /// Fired on a starting vertex once before the start of the walk from it.
        /// </summary>
        public event VertexAction<TVertex> StartVertex;

        private void OnStartVertex([NotNull] TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            StartVertex?.Invoke(vertex);
        }

        /// <summary>
        /// Fired when the walk ends.
        /// </summary>
        public event VertexAction<TVertex> EndVertex;

        private void OnEndVertex(TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            EndVertex?.Invoke(vertex);
        }

        /// <summary>
        /// Fired when an edge is encountered.
        /// </summary>
        public event EdgeAction<TVertex, TEdge> TreeEdge;

        private void OnTreeEdge([NotNull] TEdge edge)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            TreeEdge?.Invoke(edge);
        }

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            if (!TryGetRootVertex(out TVertex root))
                throw new InvalidOperationException("Root vertex not set.");

            OnStartVertex(root);
            Generate(root);
        }

        #endregion

        /// <summary>
        /// Generates a random walk with 100 steps.
        /// </summary>
        /// <param name="root">Root vertex.</param>
        public void Generate([NotNull] TVertex root)
        {
            Generate(root, 100);
        }

        /// <summary>
        /// Generates a random walk with <paramref name="walkCount"/> steps.
        /// </summary>
        /// <param name="root">Root vertex.</param>
        /// <param name="walkCount">Number of steps for the random walk.</param>
        public void Generate([NotNull] TVertex root, int walkCount)
        {
            if (root == null)
                throw new ArgumentNullException(nameof(root));

            int count = 0;
            TVertex current = root;

            OnStartVertex(root);
            while (count < walkCount && TryGetSuccessor(current, out TEdge edge))
            {
                // If dead end stop
                if (edge == null)
                    break;

                // If end predicate
                if (EndPredicate != null && EndPredicate(edge))
                    break;

                OnTreeEdge(edge);
                current = edge.Target;

                // Upgrade count
                ++count;
            }

            OnEndVertex(current);

            #region Local function

            bool TryGetSuccessor(TVertex vertex, out TEdge successor)
            {
                return EdgeChain.TryGetSuccessor(VisitedGraph, vertex, out successor);
            }

            #endregion
        }
    }
}
