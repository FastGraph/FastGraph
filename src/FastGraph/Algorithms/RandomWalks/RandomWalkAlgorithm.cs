#nullable enable

using System.Diagnostics.CodeAnalysis;

namespace FastGraph.Algorithms.RandomWalks
{
    /// <summary>
    /// Random walk algorithm (using edge chain).
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public sealed class RandomWalkAlgorithm<TVertex, TEdge>
        : RootedAlgorithmBase<TVertex, IImplicitGraph<TVertex, TEdge>>
        , ITreeBuilderAlgorithm<TVertex, TEdge>
        where TVertex : notnull
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RandomWalkAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        public RandomWalkAlgorithm(IImplicitGraph<TVertex, TEdge> visitedGraph)
            : this(visitedGraph, new NormalizedMarkovEdgeChain<TVertex, TEdge>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomWalkAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="edgeChain">Edge chain strategy to use.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="edgeChain"/> is <see langword="null"/>.</exception>
        public RandomWalkAlgorithm(
            IImplicitGraph<TVertex, TEdge> visitedGraph,
            IEdgeChain<TVertex, TEdge> edgeChain)
            : base(default, visitedGraph)
        {
            _edgeChain = edgeChain ?? throw new ArgumentNullException(nameof(edgeChain));
        }

        private IEdgeChain<TVertex, TEdge> _edgeChain;

        /// <summary>
        /// Edge chain strategy for the random walk.
        /// </summary>
        public IEdgeChain<TVertex, TEdge> EdgeChain
        {
            get => _edgeChain;
            set => _edgeChain = value ?? throw new ArgumentNullException(nameof(value), $"{nameof(EdgeChain)} cannot be default.");
        }

        /// <summary>
        /// Predicate to prematurely ends the walk.
        /// </summary>
        public EdgePredicate<TVertex, TEdge>? EndPredicate { get; set; }

        /// <summary>
        /// Fired on a starting vertex once before the start of the walk from it.
        /// </summary>
        public event VertexAction<TVertex>? StartVertex;

        private void OnStartVertex(TVertex vertex)
        {
            StartVertex?.Invoke(vertex);
        }

        /// <summary>
        /// Fired when the walk ends.
        /// </summary>
        public event VertexAction<TVertex>? EndVertex;

        private void OnEndVertex(TVertex vertex)
        {
            EndVertex?.Invoke(vertex);
        }

        /// <summary>
        /// Fired when an edge is encountered.
        /// </summary>
        public event EdgeAction<TVertex, TEdge>? TreeEdge;

        private void OnTreeEdge(TEdge edge)
        {
            TreeEdge?.Invoke(edge);
        }

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            if (!TryGetRootVertex(out TVertex? root))
                throw new InvalidOperationException("Root vertex not set.");
            Generate(root);
        }

        #endregion

        /// <summary>
        /// Generates a random walk with 100 steps.
        /// </summary>
        /// <param name="root">Root vertex.</param>
        /// <exception cref="VertexNotFoundException"><paramref name="root"/> is not part of <see cref="AlgorithmBase{TGraph}.VisitedGraph"/>.</exception>
        public void Generate(TVertex root)
        {
            Generate(root, 100);
        }

        /// <summary>
        /// Generates a random walk with <paramref name="walkCount"/> steps.
        /// </summary>
        /// <param name="root">Root vertex.</param>
        /// <param name="walkCount">Number of steps for the random walk.</param>VertexNotFoundException
        /// <exception cref="VertexNotFoundException"><paramref name="root"/> is not part of <see cref="AlgorithmBase{TGraph}.VisitedGraph"/>.</exception>
        public void Generate(TVertex root, int walkCount)
        {
            AssertRootInGraph(root);

            int count = 0;
            TVertex current = root;

            OnStartVertex(root);

            while (count < walkCount && TryGetSuccessor(current, out TEdge? edge))
            {
                // If dead end stop
                if (edge == null)
                    break;

                // If end predicate
                if (EndPredicate != default && EndPredicate(edge))
                    break;

                OnTreeEdge(edge);
                current = edge.Target;

                // Upgrade count
                ++count;
            }

            OnEndVertex(current);

            #region Local function

            bool TryGetSuccessor(TVertex vertex, [NotNullWhen(true)] out TEdge? successor)
            {
                return EdgeChain.TryGetSuccessor(VisitedGraph, vertex, out successor);
            }

            #endregion
        }
    }
}
