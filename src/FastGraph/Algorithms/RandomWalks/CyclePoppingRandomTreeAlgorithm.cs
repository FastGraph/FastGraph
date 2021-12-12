#nullable enable

using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using FastGraph.Algorithms.Services;
#if SUPPORTS_CRYPTO_RANDOM
using FastGraph.Utils;
#endif

namespace FastGraph.Algorithms.RandomWalks
{
    /// <summary>
    /// Wilson-Propp Cycle-Popping algorithm for Random Tree Generation.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public sealed class CyclePoppingRandomTreeAlgorithm<TVertex, TEdge>
        : RootedAlgorithmBase<TVertex, IVertexListGraph<TVertex, TEdge>>
        , IVertexColorizerAlgorithm<TVertex>
        , ITreeBuilderAlgorithm<TVertex, TEdge>
        where TVertex : notnull
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CyclePoppingRandomTreeAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        public CyclePoppingRandomTreeAlgorithm(IVertexListGraph<TVertex, TEdge> visitedGraph)
            : this(visitedGraph, new NormalizedMarkovEdgeChain<TVertex, TEdge>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CyclePoppingRandomTreeAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="edgeChain">Edge chain strategy to use.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="edgeChain"/> is <see langword="null"/>.</exception>
        public CyclePoppingRandomTreeAlgorithm(
            IVertexListGraph<TVertex, TEdge> visitedGraph,
            IMarkovEdgeChain<TVertex, TEdge> edgeChain)
            : this(default, visitedGraph, edgeChain)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CyclePoppingRandomTreeAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="edgeChain">Edge chain strategy to use.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="edgeChain"/> is <see langword="null"/>.</exception>
        public CyclePoppingRandomTreeAlgorithm(
            IAlgorithmComponent? host,
            IVertexListGraph<TVertex, TEdge> visitedGraph,
            IMarkovEdgeChain<TVertex, TEdge> edgeChain)
            : base(host, visitedGraph)
        {
            EdgeChain = edgeChain ?? throw new ArgumentNullException(nameof(edgeChain));
        }

        /// <summary>
        /// Stores vertices associated to their colors (treatment state).
        /// </summary>
        public IDictionary<TVertex, GraphColor> VerticesColors { get; } = new Dictionary<TVertex, GraphColor>();

        #region IVertexColorizerAlgorithm<TVertex>

        /// <inheritdoc />
        public GraphColor GetVertexColor(TVertex vertex)
        {
            if (VerticesColors.TryGetValue(vertex, out GraphColor color))
                return color;
            throw new VertexNotFoundException();
        }

        #endregion

        /// <summary>
        /// Edge chain strategy for the random walk.
        /// </summary>
        public IMarkovEdgeChain<TVertex, TEdge> EdgeChain { get; }

        private Random _rand =
#if SUPPORTS_CRYPTO_RANDOM
            new CryptoRandom((int)DateTime.Now.Ticks);
#else
            new Random((int)DateTime.Now.Ticks);
#endif

        /// <summary>
        /// Gets or sets the random number generator used in <see cref="RandomTree"/>.
        /// </summary>
        /// <exception cref="T:System.ArgumentNullException">Set value is <see langword="null"/>.</exception>
        public Random Rand
        {
            get => _rand;
            set => _rand = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Map vertices associated to their edge successors.
        /// </summary>
        public IDictionary<TVertex, TEdge> Successors { get; } = new Dictionary<TVertex, TEdge>();

        #region Events

        /// <summary>
        /// Fired when a vertex is initialized.
        /// </summary>
        public event VertexAction<TVertex>? InitializeVertex;

        private void OnInitializeVertex(TVertex vertex)
        {
            InitializeVertex?.Invoke(vertex);
        }

        /// <summary>
        /// Fired when a vertex is treated and considered as in the random tree.
        /// </summary>
        public event VertexAction<TVertex>? FinishVertex;

        private void OnFinishVertex(TVertex vertex)
        {
            FinishVertex?.Invoke(vertex);
        }

        /// <inheritdoc />
        public event EdgeAction<TVertex, TEdge>? TreeEdge;

        private void OnTreeEdge(TEdge edge)
        {
            TreeEdge?.Invoke(edge);
        }

        /// <summary>
        /// Fired when a vertex is removed from the random tree.
        /// </summary>
        public event VertexAction<TVertex>? ClearTreeVertex;

        private void OnClearTreeVertex(TVertex vertex)
        {
            ClearTreeVertex?.Invoke(vertex);
        }

        #endregion

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void Initialize()
        {
            base.Initialize();

            Successors.Clear();
            VerticesColors.Clear();
            foreach (TVertex vertex in VisitedGraph.Vertices)
            {
                VerticesColors.Add(vertex, GraphColor.White);
                OnInitializeVertex(vertex);
            }
        }

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            TVertex root = GetAndAssertRootInGraph();

            // Process root
            ClearTree(root);
            SetInTree(root);

            foreach (TVertex vertex in VisitedGraph.Vertices)
            {
                ThrowIfCancellationRequested();

                // First pass: exploration
                Explore(vertex);

                // Second pass: coloration
                Colorize(vertex);
            }
        }

        private void Explore(TVertex vertex)
        {
            var visitedEdges = new Dictionary<TEdge, int>();
            TVertex? current = vertex;
            while (NotInTree(current) && TryGetSuccessor(visitedEdges, current, out TEdge? successor))
            {
                visitedEdges[successor] = 0;
                Tree(current, successor);
                if (!TryGetNextInTree(current, out current))
                    break;
            }
        }

        [Pure]
        private bool Explore(double eps, TVertex vertex, ref int numRoots)
        {
            var visited = new Dictionary<TEdge, int>();
            TVertex? current = vertex;
            while (NotInTree(current))
            {
                if (Chance(eps))
                {
                    ClearTree(current);
                    SetInTree(current);
                    ++numRoots;
                    if (numRoots > 1)
                        return false;
                }
                else
                {
                    if (!TryGetSuccessor(visited, current, out TEdge? successor))
                        break;

                    visited[successor] = 0;
                    Tree(current, successor);
                    if (!TryGetNextInTree(current, out current))
                        break;
                }
            }

            return true;
        }

        private void Colorize(TVertex vertex)
        {
            TVertex? current = vertex;
            while (NotInTree(current))
            {
                SetInTree(current);
                if (!TryGetNextInTree(current, out current))
                    break;
            }
        }

        #endregion

        [Pure]
        private bool NotInTree(TVertex vertex)
        {
            return VerticesColors[vertex] == GraphColor.White;
        }

        private void SetInTree(TVertex vertex)
        {
            VerticesColors[vertex] = GraphColor.Black;
            OnFinishVertex(vertex);
        }

        [Pure]
        private bool TryGetSuccessor(IDictionary<TEdge, int> visited, TVertex vertex, [NotNullWhen(true)] out TEdge? successor)
        {
            IEnumerable<TEdge> outEdges = VisitedGraph.OutEdges(vertex);
            IEnumerable<TEdge> edges = outEdges.Where(edge => !visited.ContainsKey(edge));
            return EdgeChain.TryGetSuccessor(edges, vertex, out successor);
        }

        private void Tree(TVertex vertex, TEdge next)
        {
            Successors[vertex] = next;
            OnTreeEdge(next);
        }

        [Pure]
        private bool TryGetNextInTree(TVertex vertex, [NotNullWhen(true)] out TVertex? next)
        {
            if (Successors.TryGetValue(vertex, out TEdge? nextEdge))
            {
                next = nextEdge.Target;
                return true;
            }

            next = default(TVertex);
            return false;
        }

        [Pure]
        private bool Chance(double eps)
        {
            return Rand.NextDouble() <= eps;
        }

        private void ClearTree(TVertex vertex)
        {
            Successors[vertex] = default!;
            OnClearTreeVertex(vertex);
        }

        [Pure]
        private bool Attempt(double epsilon)
        {
            Initialize();
            int numRoots = 0;

            foreach (TVertex vertex in VisitedGraph.Vertices)
            {
                ThrowIfCancellationRequested();

                // First pass: exploration
                if (!Explore(epsilon, vertex, ref numRoots))
                    return false;

                // Second pass: coloration
                Colorize(vertex);
            }

            return true;
        }

        /// <summary>
        /// Runs a random tree generation starting at <paramref name="root"/> vertex.
        /// </summary>
        /// <param name="root">Tree starting vertex.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="root"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="root"/> is part of <see cref="AlgorithmBase{TGraph}.VisitedGraph"/>.</exception>
        public void RandomTreeWithRoot(TVertex root)
        {
            if (!VisitedGraph.ContainsVertex(root))
                throw new ArgumentException("The vertex must be in the graph.", nameof(root));

            SetRootVertex(root);
            Compute();
        }

        /// <summary>
        /// Runs a random tree generation.
        /// </summary>
        public void RandomTree()
        {
            double epsilon = 1;
            bool success;
            do
            {
                ThrowIfCancellationRequested();

                epsilon /= 2;
                success = Attempt(epsilon);
            } while (!success);
        }
    }
}
