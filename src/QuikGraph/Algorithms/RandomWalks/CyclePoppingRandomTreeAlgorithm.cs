using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using QuikGraph.Algorithms.Services;

namespace QuikGraph.Algorithms.RandomWalks
{
    /// <summary>
    /// Wilson-Propp Cycle-Popping algorithm for Random Tree Generation.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class CyclePoppingRandomTreeAlgorithm<TVertex, TEdge>
        : RootedAlgorithmBase<TVertex, IVertexListGraph<TVertex, TEdge>>
        , IVertexColorizerAlgorithm<TVertex>
        , ITreeBuilderAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CyclePoppingRandomTreeAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        public CyclePoppingRandomTreeAlgorithm([NotNull] IVertexListGraph<TVertex, TEdge> visitedGraph)
            : this(visitedGraph, new NormalizedMarkovEdgeChain<TVertex, TEdge>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CyclePoppingRandomTreeAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="edgeChain">Edge chain strategy to use.</param>
        public CyclePoppingRandomTreeAlgorithm(
            [NotNull] IVertexListGraph<TVertex, TEdge> visitedGraph,
            [NotNull] IMarkovEdgeChain<TVertex, TEdge> edgeChain)
            : this(null, visitedGraph, edgeChain)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CyclePoppingRandomTreeAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="edgeChain">Edge chain strategy to use.</param>
        public CyclePoppingRandomTreeAlgorithm(
            [CanBeNull] IAlgorithmComponent host,
            [NotNull] IVertexListGraph<TVertex, TEdge> visitedGraph,
            [NotNull] IMarkovEdgeChain<TVertex, TEdge> edgeChain)
            : base(host, visitedGraph)
        {
            if (edgeChain is null)
                throw new ArgumentNullException(nameof(edgeChain));
            EdgeChain = edgeChain;
        }

        /// <summary>
        /// Stores vertices associated to their colors (treatment state).
        /// </summary>
        [NotNull]
        public IDictionary<TVertex, GraphColor> VerticesColors { get; } = new Dictionary<TVertex, GraphColor>();

        #region IVertexColorizerAlgorithm<TVertex>

        /// <inheritdoc />
        public GraphColor GetVertexColor(TVertex vertex)
        {
            return VerticesColors[vertex];
        }

        #endregion

        /// <summary>
        /// Edge chain strategy for the random walk.
        /// </summary>
        [NotNull]
        public IMarkovEdgeChain<TVertex, TEdge> EdgeChain { get; }

        [NotNull]
        private Random _rand = new Random((int)DateTime.Now.Ticks);

        /// <summary>
        /// Gets or sets the random number generator used in <see cref="RandomTree"/>.
        /// </summary>
        [NotNull]
        public Random Rand
        {
            get
            {
                return _rand;
            }
            set
            {
                _rand = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        /// <summary>
        /// Map vertices associated to their edge successors.
        /// </summary>
        [NotNull]
        public IDictionary<TVertex, TEdge> Successors { get; } = new Dictionary<TVertex, TEdge>();

        #region Events

        /// <summary>
        /// Fired when a vertex is initialized.
        /// </summary>
        public event VertexAction<TVertex> InitializeVertex;

        private void OnInitializeVertex([NotNull] TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            InitializeVertex?.Invoke(vertex);
        }

        /// <summary>
        /// Fired when a vertex is treated and considered as in the random tree.
        /// </summary>
        public event VertexAction<TVertex> FinishVertex;

        private void OnFinishVertex([NotNull] TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            FinishVertex?.Invoke(vertex);
        }

        /// <inheritdoc />
        public event EdgeAction<TVertex, TEdge> TreeEdge;

        private void OnTreeEdge([NotNull] TEdge edge)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            TreeEdge?.Invoke(edge);
        }

        /// <summary>
        /// Fired when a vertex is removed from the random tree.
        /// </summary>
        public event VertexAction<TVertex> ClearTreeVertex;

        private void OnClearTreeVertex([NotNull] TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

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
            if (!TryGetRootVertex(out TVertex rootVertex))
                throw new InvalidOperationException("Root vertex not set.");

            ICancelManager cancelManager = Services.CancelManager;
            // Process root
            ClearTree(rootVertex);
            SetInTree(rootVertex);

            foreach (TVertex vertex in VisitedGraph.Vertices)
            {
                if (cancelManager.IsCancelling)
                    break;

                // First pass: exploration 
                {
                    var visitedEdges = new Dictionary<TEdge, int>();
                    TVertex current = vertex;
                    while (NotInTree(current) && TryGetSuccessor(visitedEdges, current, out TEdge successor))
                    {
                        visitedEdges[successor] = 0;
                        Tree(current, successor);
                        if (!TryGetNextInTree(current, out current))
                            break;
                    }
                }

                // Second pass: coloration
                {
                    TVertex current = vertex;
                    while (NotInTree(current))
                    {
                        SetInTree(current);
                        if (!TryGetNextInTree(current, out current))
                            break;
                    }
                }
            }
        }

        #endregion

        private bool NotInTree([NotNull] TVertex vertex)
        {
            return VerticesColors[vertex] == GraphColor.White;
        }

        private void SetInTree([NotNull] TVertex vertex)
        {
            VerticesColors[vertex] = GraphColor.Black;
            OnFinishVertex(vertex);
        }

        private bool TryGetSuccessor([NotNull] IDictionary<TEdge, int> visited, [NotNull] TVertex vertex, out TEdge successor)
        {
            IEnumerable<TEdge> outEdges = VisitedGraph.OutEdges(vertex);
            IEnumerable<TEdge> edges = outEdges.Where(edge => !visited.ContainsKey(edge));
            return EdgeChain.TryGetSuccessor(edges, vertex, out successor);
        }

        private void Tree([NotNull] TVertex vertex, [NotNull] TEdge next)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));
            if (next == null)
                throw new ArgumentNullException(nameof(next));

            Successors[vertex] = next;
            OnTreeEdge(next);
        }

        private bool TryGetNextInTree([NotNull] TVertex vertex, out TVertex next)
        {
            if (Successors.TryGetValue(vertex, out TEdge nextEdge))
            {
                next = nextEdge.Target;
                return true;
            }

            next = default(TVertex);
            return false;
        }

        private bool Chance(double eps)
        {
            return Rand.NextDouble() <= eps;
        }

        private void ClearTree([NotNull] TVertex vertex)
        {
            Successors[vertex] = default(TEdge);
            OnClearTreeVertex(vertex);
        }

        /// <summary>
        /// Runs a random tree generation starting at <paramref name="root"/> vertex.
        /// </summary>
        /// <param name="root">Tree starting vertex.</param>
        public void RandomTreeWithRoot([NotNull] TVertex root)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(root != null);
            Contract.Requires(VisitedGraph.ContainsVertex(root));
#endif

            SetRootVertex(root);
            Compute();
        }

        /// <summary>
        /// Runs a random tree generation.
        /// </summary>
        public void RandomTree()
        {
            ICancelManager cancelManager = Services.CancelManager;

            double eps = 1;
            bool success;
            do
            {
                if (cancelManager.IsCancelling)
                    break;

                eps /= 2;
                success = Attempt(eps);
            } while (!success);
        }

        private bool Attempt(double eps)
        {
            Initialize();
            int numRoots = 0;
            ICancelManager cancelManager = Services.CancelManager;

            foreach (TVertex vertex in VisitedGraph.Vertices)
            {
                if (cancelManager.IsCancelling)
                    break;

                // First pass: exploration
                if (!Explore(eps, vertex, ref numRoots))
                    return false;

                // Second pass: coloration
                Colorize(vertex);
            }

            return true;
        }

        private bool Explore(double eps, [NotNull] TVertex vertex, ref int numRoots)
        {
            var visited = new Dictionary<TEdge, int>();
            TVertex current = vertex;
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
                    if (!TryGetSuccessor(visited, current, out TEdge successor))
                        break;

                    visited[successor] = 0;
                    Tree(current, successor);
                    if (!TryGetNextInTree(current, out current))
                        break;
                }
            }

            return true;
        }

        private void Colorize([NotNull] TVertex vertex)
        {
            TVertex current = vertex;
            while (NotInTree(current))
            {
                SetInTree(current);
                if (!TryGetNextInTree(current, out current))
                    break;
            }
        }
    }
}
