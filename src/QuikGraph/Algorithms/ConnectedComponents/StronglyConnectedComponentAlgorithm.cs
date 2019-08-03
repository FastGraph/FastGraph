#if SUPPORTS_SERIALIZATION
using System;
#endif
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using QuikGraph.Algorithms.Search;
using QuikGraph.Algorithms.Services;

namespace QuikGraph.Algorithms.ConnectedComponents
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
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class StronglyConnectedComponentsAlgorithm<TVertex, TEdge> :
        AlgorithmBase<IVertexListGraph<TVertex, TEdge>>,
        IConnectedComponentAlgorithm<TVertex, TEdge, IVertexListGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        private readonly Stack<TVertex> _stack;

        private int _dfsTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="StronglyConnectedComponentsAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        public StronglyConnectedComponentsAlgorithm(
            [NotNull] IVertexListGraph<TVertex, TEdge> visitedGraph)
            : this(visitedGraph, new Dictionary<TVertex, int>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StronglyConnectedComponentsAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="components">Graph components.</param>
        public StronglyConnectedComponentsAlgorithm(
            [NotNull] IVertexListGraph<TVertex, TEdge> visitedGraph,
            [NotNull] IDictionary<TVertex, int> components)
            : this(null, visitedGraph, components)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StronglyConnectedComponentsAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="components">Graph components.</param>
        public StronglyConnectedComponentsAlgorithm(
            [CanBeNull] IAlgorithmComponent host,
            [NotNull] IVertexListGraph<TVertex, TEdge> visitedGraph,
            [NotNull] IDictionary<TVertex, int> components)
            : base(host, visitedGraph)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(components != null);
#endif

            Components = components;
            Roots = new Dictionary<TVertex, TVertex>();
            DiscoverTimes = new Dictionary<TVertex, int>();
            _stack = new Stack<TVertex>();
            ComponentCount = 0;
            _dfsTime = 0;
        }

        /// <summary>
        /// Root vertices associated to their minimal linked vertex.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        public IDictionary<TVertex, TVertex> Roots { get; }

        /// <summary>
        /// Times of vertices discover.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        public IDictionary<TVertex, int> DiscoverTimes { get; }

        /// <summary>
        /// Number of steps spent.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        public int Steps { get; private set; }

        /// <summary>
        /// Number of components discovered per step.
        /// </summary>
        public List<int> ComponentsPerStep { get; private set; }

        /// <summary>
        /// Vertices treated per step.
        /// </summary>
        public List<TVertex> VerticesPerStep { get; private set; }

        [ItemNotNull]
        private List<BidirectionalGraph<TVertex, TEdge>> _graphs;

        /// <summary>
        /// Strongly connected components.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull, ItemNotNull]
        public List<BidirectionalGraph<TVertex, TEdge>> Graphs
        {
            get
            {
                _graphs = new List<BidirectionalGraph<TVertex, TEdge>>(ComponentCount + 1);
                for (int i = 0; i < ComponentCount; i++)
                {
                    _graphs.Add(new BidirectionalGraph<TVertex, TEdge>());
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

#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        [NotNull]
        private TVertex MinDiscoverTime([NotNull] TVertex u, [NotNull] TVertex v)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(u != null);
            Contract.Requires(v != null);
            Contract.Ensures(DiscoverTimes[u] < DiscoverTimes[v]
                ? Contract.Result<TVertex>().Equals(u)
                : Contract.Result<TVertex>().Equals(v));
#endif

            // Min vertex
            return DiscoverTimes[u] < DiscoverTimes[v]
                ? u
                : v;
        }

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void InternalCompute()
        {
#if SUPPORTS_CONTRACTS
            Contract.Ensures(ComponentCount >= 0);
            Contract.Ensures(VisitedGraph.VertexCount == 0 || ComponentCount > 0);
            Contract.Ensures(VisitedGraph.Vertices.All(vertex => Components.ContainsKey(vertex)));
            Contract.Ensures(VisitedGraph.VertexCount == Components.Count);
            Contract.Ensures(Components.Values.All(component => component <= ComponentCount));
#endif

            ComponentsPerStep = new List<int>();
            VerticesPerStep = new List<TVertex>();

            Components.Clear();
            Roots.Clear();
            DiscoverTimes.Clear();
            _stack.Clear();
            ComponentCount = 0;
            _dfsTime = 0;

            DepthFirstSearchAlgorithm<TVertex, TEdge> dfs = null;
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
                if (dfs != null)
                {
                    dfs.DiscoverVertex -= OnVertexDiscovered;
                    dfs.FinishVertex -= OnVertexFinished;
                }
            }
        }

        #endregion

        #region IConnectedComponentAlgorithm<TVertex,TEdge,TGraph>

        /// <inheritdoc />
        public int ComponentCount { get; private set; }

        /// <inheritdoc />
        public IDictionary<TVertex, int> Components { get; }

        #endregion

        private void OnVertexDiscovered([NotNull] TVertex vertex)
        {
            Roots[vertex] = vertex;
            Components[vertex] = int.MaxValue;

            ComponentsPerStep.Add(ComponentCount);
            VerticesPerStep.Add(vertex);
            ++Steps;

            DiscoverTimes[vertex] = _dfsTime++;
            _stack.Push(vertex);
        }

        private void OnVertexFinished([NotNull] TVertex vertex)
        {
            foreach (TEdge edge in VisitedGraph.OutEdges(vertex))
            {
                TVertex target = edge.Target;
                if (Components[target] == int.MaxValue)
                    Roots[vertex] = MinDiscoverTime(Roots[vertex], Roots[target]);
            }

            if (Roots[vertex].Equals(vertex))
            {
                TVertex w;
                do
                {
                    w = _stack.Pop();
                    Components[w] = ComponentCount;

                    ComponentsPerStep.Add(ComponentCount);
                    VerticesPerStep.Add(w);
                    ++Steps;
                }
                while (!w.Equals(vertex));

                ++ComponentCount;
            }
        }
    }
}