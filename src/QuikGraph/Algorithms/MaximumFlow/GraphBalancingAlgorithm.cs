using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif

namespace QuikGraph.Algorithms.MaximumFlow
{
    /// <summary>
    /// Algorithm that computes a the graph balancing by finding vertices
    /// causing surplus or deficits.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class GraphBalancerAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        [NotNull]
        private readonly Dictionary<TEdge, int> _preFlow = new Dictionary<TEdge, int>();

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphBalancerAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="source">Flow source vertex.</param>
        /// <param name="sink">Flow sink vertex.</param>
        /// <param name="vertexFactory">Vertex factory method.</param>
        /// <param name="edgeFactory">Edge factory method.</param>
        public GraphBalancerAlgorithm(
            [NotNull] IMutableBidirectionalGraph<TVertex, TEdge> visitedGraph,
            [NotNull] TVertex source,
            [NotNull] TVertex sink,
            [NotNull] VertexFactory<TVertex> vertexFactory,
            [NotNull] EdgeFactory<TVertex, TEdge> edgeFactory)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(visitedGraph != null);
            Contract.Requires(vertexFactory != null);
            Contract.Requires(edgeFactory != null);
            Contract.Requires(source != null);
            Contract.Requires(visitedGraph.ContainsVertex(source));
            Contract.Requires(sink != null);
            Contract.Requires(visitedGraph.ContainsVertex(sink));
#endif

            VisitedGraph = visitedGraph;
            VertexFactory = vertexFactory;
            EdgeFactory = edgeFactory;
            Source = source;
            Sink = sink;

            foreach (TEdge edge in VisitedGraph.Edges)
            {
                // Setting capacities = u(e) = +infinity
                Capacities.Add(edge, double.MaxValue);

                // Setting preflow = l(e) = 1
                _preFlow.Add(edge, 1);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphBalancerAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="source">Flow source vertex.</param>
        /// <param name="sink">Flow sink vertex.</param>
        /// <param name="vertexFactory">Vertex factory method.</param>
        /// <param name="edgeFactory">Edge factory method.</param>
        /// <param name="capacities">Edges capacities.</param>
        public GraphBalancerAlgorithm(
            [NotNull] IMutableBidirectionalGraph<TVertex, TEdge> visitedGraph,
            [NotNull] VertexFactory<TVertex> vertexFactory,
            [NotNull] EdgeFactory<TVertex, TEdge> edgeFactory,
            [NotNull] TVertex source,
            [NotNull] TVertex sink,
            [NotNull] IDictionary<TEdge, double> capacities)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(visitedGraph != null);
            Contract.Requires(vertexFactory != null);
            Contract.Requires(edgeFactory != null);
            Contract.Requires(source != null);
            Contract.Requires(visitedGraph.ContainsVertex(source));
            Contract.Requires(sink != null);
            Contract.Requires(visitedGraph.ContainsVertex(sink));
            Contract.Requires(capacities != null);
#endif

            VisitedGraph = visitedGraph;
            VertexFactory = vertexFactory;
            EdgeFactory = edgeFactory;
            Source = source;
            Sink = sink;
            Capacities = capacities;

            // Setting preflow = l(e) = 1
            foreach (TEdge edge in VisitedGraph.Edges)
                _preFlow.Add(edge, 1);
        }

        /// <summary>
        /// Gets the graph to visit with this algorithm.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        public IMutableBidirectionalGraph<TVertex, TEdge> VisitedGraph { get; }

        /// <summary>
        /// Vertex factory method.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        public VertexFactory<TVertex> VertexFactory { get; }

        /// <summary>
        /// Edge factory method.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        public EdgeFactory<TVertex, TEdge> EdgeFactory { get; }

        /// <summary>
        /// Indicates if the graph has been balanced or not.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        public bool Balanced { get; private set; }

        /// <summary>
        /// Flow source vertex.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        public TVertex Source { get; }

        /// <summary>
        /// Flow sink vertex.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        public TVertex Sink { get; }

        /// <summary>
        /// Balancing flow source vertex.
        /// </summary>
        /// <remarks>Not null if the algorithm has been run (and not reverted).</remarks>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        public TVertex BalancingSource { get; private set; }

        /// <summary>
        /// Balancing source edge (between <see cref="BalancingSource"/> and <see cref="Source"/>).
        /// </summary>
        /// <remarks>Not null if the algorithm has been run (and not reverted).</remarks>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        public TEdge BalancingSourceEdge { get; private set; }

        /// <summary>
        /// Balancing flow sink vertex.
        /// </summary>
        /// <remarks>Not null if the algorithm has been run (and not reverted).</remarks>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        public TVertex BalancingSink { get; private set; }

        /// <summary>
        /// Balancing sink edge (between <see cref="Sink"/> and <see cref="BalancingSink"/>).
        /// </summary>
        /// <remarks>Not null if the algorithm has been run (and not reverted).</remarks>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        public TEdge BalancingSinkEdge { get; private set; }

        [NotNull, ItemNotNull]
        private readonly List<TVertex> _surplusVertices = new List<TVertex>();

        /// <summary>
        /// Enumerable of vertices that add surplus to the graph balance.
        /// </summary>
        [NotNull, ItemNotNull]
        public IEnumerable<TVertex> SurplusVertices => _surplusVertices.AsEnumerable();

        [NotNull, ItemNotNull]
        private readonly List<TEdge> _surplusEdges = new List<TEdge>();

        /// <summary>
        /// Enumerable of edges linked to vertices that add surplus to the graph balance.
        /// </summary>
        [NotNull, ItemNotNull]
        public IEnumerable<TEdge> SurplusEdges => _surplusEdges.AsEnumerable();

        [NotNull, ItemNotNull]
        private readonly List<TVertex> _deficientVertices = new List<TVertex>();

        /// <summary>
        /// Enumerable of vertices that add deficit to the graph balance.
        /// </summary>
        [NotNull, ItemNotNull]
        public IEnumerable<TVertex> DeficientVertices => _deficientVertices.AsEnumerable();

        [NotNull, ItemNotNull]
        private readonly List<TEdge> _deficientEdges = new List<TEdge>();

        /// <summary>
        /// Enumerable of edges linked to vertices that add deficit to the graph balance.
        /// </summary>
        [NotNull, ItemNotNull]
        public IEnumerable<TEdge> DeficientEdges => _deficientEdges.AsEnumerable();

        /// <summary>
        /// Edges capacities.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        public IDictionary<TEdge, double> Capacities { get; } = new Dictionary<TEdge, double>();

        /// <summary>
        /// Fired when the <see cref="BalancingSource"/> is added to the graph.
        /// </summary>
        public event VertexAction<TVertex> BalancingSourceAdded;

        private void OnBalancingSourceAdded()
        {
            BalancingSourceAdded?.Invoke(Source);
        }

        /// <summary>
        /// Fired when the <see cref="BalancingSink"/> is added to the graph.
        /// </summary>
        public event VertexAction<TVertex> BalancingSinkAdded;

        private void OnBalancingSinkAdded()
        {
            BalancingSinkAdded?.Invoke(Sink);
        }

        /// <summary>
        /// Fired when an edge is added to the graph.
        /// </summary>
        public event EdgeAction<TVertex, TEdge> EdgeAdded;

        private void OnEdgeAdded([NotNull] TEdge edge)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(edge != null);
#endif

            EdgeAdded?.Invoke(edge);
        }

        /// <summary>
        /// Fired when a vertex adding surplus to the balance is found and added to <see cref="SurplusVertices"/>.
        /// </summary>
        public event VertexAction<TVertex> SurplusVertexAdded;

        private void OnSurplusVertexAdded([NotNull] TVertex vertex)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(vertex != null);
#endif

            SurplusVertexAdded?.Invoke(vertex);
        }

        /// <summary>
        /// Fired when a vertex adding a deficit to the balance is found and added to <see cref="DeficientVertices"/>.
        /// </summary>
        public event VertexAction<TVertex> DeficientVertexAdded;

        private void OnDeficientVertexAdded([NotNull] TVertex vertex)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(vertex != null);
#endif

            DeficientVertexAdded?.Invoke(vertex);
        }

        /// <summary>
        /// Gets the balancing index of the given <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">Vertex to get balancing index.</param>
        /// <returns>Balancing index.</returns>
        public int GetBalancingIndex([NotNull] TVertex vertex)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(vertex != null);
#endif

            int balancingIndex = 0;
            foreach (TEdge edge in VisitedGraph.OutEdges(vertex))
            {
                int preFlow = _preFlow[edge];
                balancingIndex += preFlow;
            }

            foreach (TEdge edge in VisitedGraph.InEdges(vertex))
            {
                int preFlow = _preFlow[edge];
                balancingIndex -= preFlow;
            }

            return balancingIndex;
        }

        /// <summary>
        /// Runs the graph balancing algorithm.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the graph is already balanced.</exception>
        public void Balance()
        {
            if (Balanced)
                throw new InvalidOperationException("Graph already balanced.");

            // Step 0
            // Create new balancing source and sink
            BalancingSource = VertexFactory();
            VisitedGraph.AddVertex(BalancingSource);
            OnBalancingSourceAdded();

            BalancingSink = VertexFactory();
            VisitedGraph.AddVertex(BalancingSink);
            OnBalancingSinkAdded();

            // Step 1
            // Link balancing source to the flow source
            BalancingSourceEdge = EdgeFactory(BalancingSource, Source);
            VisitedGraph.AddEdge(BalancingSourceEdge);
            Capacities.Add(BalancingSourceEdge, double.MaxValue);
            _preFlow.Add(BalancingSourceEdge, 0);
            OnEdgeAdded(BalancingSourceEdge);

            // Link the flow sink to the balancing sink
            BalancingSinkEdge = EdgeFactory(Sink, BalancingSink);
            VisitedGraph.AddEdge(BalancingSinkEdge);
            Capacities.Add(BalancingSinkEdge, double.MaxValue);
            _preFlow.Add(BalancingSinkEdge, 0);
            OnEdgeAdded(BalancingSinkEdge);

            // Step 2
            // For each surplus vertex v, add (source -> v)
            foreach (TVertex vertex in VisitedGraph.Vertices.Where(v => !IsSourceOrSink(v)))
            {
                int balancingIndex = GetBalancingIndex(vertex);
                if (balancingIndex == 0)
                    continue;

                if (balancingIndex < 0)
                {
                    // Surplus vertex
                    TEdge edge = EdgeFactory(BalancingSource, vertex);
                    VisitedGraph.AddEdge(edge);

                    _surplusEdges.Add(edge);
                    _surplusVertices.Add(vertex);

                    _preFlow.Add(edge, 0);

                    Capacities.Add(edge, -balancingIndex);

                    OnSurplusVertexAdded(vertex);
                    OnEdgeAdded(edge);
                }
                else
                {
                    // Deficient vertex
                    TEdge edge = EdgeFactory(vertex, BalancingSink);

                    _deficientEdges.Add(edge);
                    _deficientVertices.Add(vertex);

                    _preFlow.Add(edge, 0);

                    Capacities.Add(edge, balancingIndex);

                    OnDeficientVertexAdded(vertex);
                    OnEdgeAdded(edge);
                }
            }

            Balanced = true;

            #region Local function

            bool IsSourceOrSink(TVertex v)
            {
                return v.Equals(BalancingSource)
                       || v.Equals(BalancingSink)
                       || v.Equals(Source)
                       || v.Equals(Sink);
            }

            #endregion
        }

        /// <summary>
        /// Runs the graph unbalancing algorithm.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the graph is not balanced.</exception>
        public void UnBalance()
        {
            if (!Balanced)
                throw new InvalidOperationException("Graph is not balanced.");

            foreach (TEdge edge in _surplusEdges)
            {
                VisitedGraph.RemoveEdge(edge);
                Capacities.Remove(edge);
                _preFlow.Remove(edge);
            }

            foreach (TEdge edge in _deficientEdges)
            {
                VisitedGraph.RemoveEdge(edge);
                Capacities.Remove(edge);
                _preFlow.Remove(edge);
            }

            Capacities.Remove(BalancingSinkEdge);
            Capacities.Remove(BalancingSourceEdge);

            _preFlow.Remove(BalancingSinkEdge);
            _preFlow.Remove(BalancingSourceEdge);

            VisitedGraph.RemoveEdge(BalancingSourceEdge);
            VisitedGraph.RemoveEdge(BalancingSinkEdge);
            VisitedGraph.RemoveVertex(BalancingSource);
            VisitedGraph.RemoveVertex(BalancingSink);

            BalancingSource = default(TVertex);
            BalancingSink = default(TVertex);
            BalancingSourceEdge = default(TEdge);
            BalancingSinkEdge = default(TEdge);

            _surplusEdges.Clear();
            _deficientEdges.Clear();
            _surplusVertices.Clear();
            _deficientVertices.Clear();

            Balanced = false;
        }
    }
}
