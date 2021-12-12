#nullable enable

using System.Diagnostics;
using JetBrains.Annotations;
using FastGraph.Algorithms.Search;
using FastGraph.Algorithms.Services;

namespace FastGraph.Algorithms.ConnectedComponents
{
    /// <summary>
    /// Algorithm that computes strongly connected components of a graph.
    /// </summary>
    /// <remarks>
    /// A strongly connected component is a sub graph where there is a path from every
    /// vertex to every other vertices.
    /// </remarks>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public sealed class StronglyConnectedComponentsAlgorithm<TVertex, TEdge>
        : AlgorithmBase<IVertexListGraph<TVertex, TEdge>>
        , IConnectedComponentAlgorithm<TVertex, TEdge, IVertexListGraph<TVertex, TEdge>>
        where TVertex : notnull
        where TEdge : IEdge<TVertex>
    {
        private readonly Stack<TVertex> _stack;

        private int _dfsTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="StronglyConnectedComponentsAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        public StronglyConnectedComponentsAlgorithm(
            IVertexListGraph<TVertex, TEdge> visitedGraph)
            : this(visitedGraph, new Dictionary<TVertex, int>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StronglyConnectedComponentsAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="components">Graph components.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="components"/> is <see langword="null"/>.</exception>
        public StronglyConnectedComponentsAlgorithm(
            IVertexListGraph<TVertex, TEdge> visitedGraph,
            IDictionary<TVertex, int> components)
            : this(default, visitedGraph, components)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StronglyConnectedComponentsAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="components">Graph components.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="components"/> is <see langword="null"/>.</exception>
        public StronglyConnectedComponentsAlgorithm(
            IAlgorithmComponent? host,
            IVertexListGraph<TVertex, TEdge> visitedGraph,
            IDictionary<TVertex, int> components)
            : base(host, visitedGraph)
        {
            Components = components ?? throw new ArgumentNullException(nameof(components));
            Roots = new Dictionary<TVertex, TVertex>();
            DiscoverTimes = new Dictionary<TVertex, int>();
            _stack = new Stack<TVertex>();
            ComponentCount = 0;
            _dfsTime = 0;
        }

        /// <summary>
        /// Root vertices associated to their minimal linked vertex.
        /// </summary>
        public IDictionary<TVertex, TVertex> Roots { get; }

        /// <summary>
        /// Times of vertices discover.
        /// </summary>
        public IDictionary<TVertex, int> DiscoverTimes { get; }

        /// <summary>
        /// Number of steps spent.
        /// </summary>
        public int Steps { get; private set; }

        /// <summary>
        /// Number of components discovered per step.
        /// </summary>
        public List<int>? ComponentsPerStep { get; private set; }

        /// <summary>
        /// Vertices treated per step.
        /// </summary>
        public List<TVertex>? VerticesPerStep { get; private set; }

        private BidirectionalGraph<TVertex, TEdge>[]? _graphs;

        /// <summary>
        /// Strongly connected components.
        /// </summary>
        public BidirectionalGraph<TVertex, TEdge>[] Graphs
        {
            get
            {
                _graphs = new BidirectionalGraph<TVertex, TEdge>[ComponentCount];
                for (int i = 0; i < ComponentCount; ++i)
                {
                    _graphs[i] = new BidirectionalGraph<TVertex, TEdge>();
                }

                foreach (TVertex componentName in Components.Keys)
                {
                    _graphs[Components[componentName]].AddVertex(componentName);
                }

                foreach (TVertex vertex in VisitedGraph.Vertices)
                {
                    foreach (TEdge edge in VisitedGraph.OutEdges(vertex))
                    {

                        if (Components[vertex] == Components[edge.Target])
                        {
                            _graphs[Components[vertex]].AddEdge(edge);
                        }
                    }
                }

                return _graphs;
            }
        }

        [Pure]
        private TVertex MinDiscoverTime(TVertex u, TVertex v)
        {
            // Min vertex
            return DiscoverTimes[u] < DiscoverTimes[v]
                ? u
                : v;
        }

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void Initialize()
        {
            base.Initialize();

            ComponentsPerStep = new List<int>();
            VerticesPerStep = new List<TVertex>();

            Components.Clear();
            Roots.Clear();
            DiscoverTimes.Clear();
            _stack.Clear();
            ComponentCount = 0;
            _dfsTime = 0;
        }

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            DepthFirstSearchAlgorithm<TVertex, TEdge>? dfs = default;
            try
            {
                dfs = new DepthFirstSearchAlgorithm<TVertex, TEdge>(
                    this,
                    VisitedGraph,
                    new Dictionary<TVertex, GraphColor>(VisitedGraph.VertexCount));
                dfs.DiscoverVertex += OnVertexDiscovered;
                dfs.FinishVertex += OnVertexFinished;

                dfs.Compute();
            }
            finally
            {
                if (dfs != default)
                {
                    dfs.DiscoverVertex -= OnVertexDiscovered;
                    dfs.FinishVertex -= OnVertexFinished;
                }
            }

            Debug.Assert(ComponentCount >= 0);
            Debug.Assert(VisitedGraph.VertexCount >= 0 || ComponentCount == 0);
            Debug.Assert(VisitedGraph.Vertices.All(v => Components.ContainsKey(v)));
            Debug.Assert(VisitedGraph.VertexCount == Components.Count);
            Debug.Assert(Components.Values.All(c => c <= ComponentCount));
        }

        #endregion

        #region IConnectedComponentAlgorithm<TVertex,TEdge,TGraph>

        /// <inheritdoc />
        public int ComponentCount { get; private set; }

        /// <inheritdoc />
        public IDictionary<TVertex, int> Components { get; }

        #endregion

        private void OnVertexDiscovered(TVertex vertex)
        {
            Roots[vertex] = vertex;
            Components[vertex] = int.MaxValue;

            ComponentsPerStep!.Add(ComponentCount);
            VerticesPerStep!.Add(vertex);
            ++Steps;

            DiscoverTimes[vertex] = _dfsTime++;
            _stack.Push(vertex);
        }

        private void OnVertexFinished(TVertex vertex)
        {
            foreach (TVertex target in VisitedGraph.OutEdges(vertex).Select(edge => edge.Target))
            {
                if (Components[target] == int.MaxValue)
                {
                    Roots[vertex] = MinDiscoverTime(Roots[vertex], Roots[target]);
                }
            }

            if (EqualityComparer<TVertex>.Default.Equals(Roots[vertex], vertex))
            {
                TVertex w;
                do
                {
                    w = _stack.Pop();
                    Components[w] = ComponentCount;

                    ComponentsPerStep!.Add(ComponentCount);
                    VerticesPerStep!.Add(w);
                    ++Steps;
                }
                while (!EqualityComparer<TVertex>.Default.Equals(w, vertex));

                ++ComponentCount;
            }
        }
    }
}
