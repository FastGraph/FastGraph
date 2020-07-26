using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using QuikGraph.Algorithms.Search;
using QuikGraph.Algorithms.Services;
#if !SUPPORTS_SORTEDSET
using QuikGraph.Collections;
#endif

namespace QuikGraph.Algorithms.ConnectedComponents
{
    /// <summary>
    /// Algorithm that computes weakly connected components of a graph.
    /// </summary>
    /// <remarks>
    /// A weakly connected component is a maximal sub graph of a graph such that for
    /// every pair of vertices (u,v) in the sub graph, there is an undirected path from u to v
    /// and a directed path from v to u.
    /// </remarks>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public sealed class WeaklyConnectedComponentsAlgorithm<TVertex, TEdge>
        : AlgorithmBase<IVertexListGraph<TVertex, TEdge>>
        , IConnectedComponentAlgorithm<TVertex, TEdge, IVertexListGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        [NotNull]
        private readonly Dictionary<int, int> _componentEquivalences = new Dictionary<int, int>();

        private int _currentComponent;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeaklyConnectedComponentsAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        public WeaklyConnectedComponentsAlgorithm(
            [NotNull] IVertexListGraph<TVertex, TEdge> visitedGraph)
            : this(visitedGraph, new Dictionary<TVertex, int>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeaklyConnectedComponentsAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="components">Graph components.</param>
        public WeaklyConnectedComponentsAlgorithm(
            [NotNull] IVertexListGraph<TVertex, TEdge> visitedGraph,
            [NotNull] IDictionary<TVertex, int> components)
            : this(null, visitedGraph, components)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeaklyConnectedComponentsAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="components">Graph components.</param>
        public WeaklyConnectedComponentsAlgorithm(
            [CanBeNull] IAlgorithmComponent host,
            [NotNull] IVertexListGraph<TVertex, TEdge> visitedGraph,
            [NotNull] IDictionary<TVertex, int> components)
            : base(host, visitedGraph)
        {
            Components = components ?? throw new ArgumentNullException(nameof(components));
        }

        [ItemNotNull]
        private BidirectionalGraph<TVertex, TEdge>[] _graphs;

        /// <summary>
        /// Weakly connected components.
        /// </summary>
        [NotNull, ItemNotNull]
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

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void Initialize()
        {
            ComponentCount = 0;
            _currentComponent = 0;
            _componentEquivalences.Clear();
            Components.Clear();
        }

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            // Shortcut for empty graph
            if (VisitedGraph.IsVerticesEmpty)
                return;

            var dfs = new DepthFirstSearchAlgorithm<TVertex, TEdge>(VisitedGraph);
            try
            {
                dfs.StartVertex += OnStartVertex;
                dfs.TreeEdge += OnEdgeDiscovered;
                dfs.ForwardOrCrossEdge += OnForwardOrCrossEdge;

                dfs.Compute();
            }
            finally
            {
                dfs.StartVertex -= OnStartVertex;
                dfs.TreeEdge -= OnEdgeDiscovered;
                dfs.ForwardOrCrossEdge -= OnForwardOrCrossEdge;
            }
        }

        /// <inheritdoc />
        protected override void Clean()
        {
            base.Clean();

            // Merge component numbers
            MergeEquivalentComponents();

            _componentEquivalences.Clear();

            // If there are more than one component
            // then it can have some blanks between components indexes
            // Apply a compression to reduce spacing between components
            if (ComponentCount > 1)
            {
                ReduceComponentsIndexes();

                _componentEquivalences.Clear();
            }

            Debug.Assert(ComponentCount >= 0 && ComponentCount <= VisitedGraph.VertexCount);
            Debug.Assert(
                VisitedGraph.Vertices.All(
                    vertex => Components[vertex] >= 0 && Components[vertex] < ComponentCount));
        }

        private void MergeEquivalentComponents()
        {
            foreach (TVertex vertex in VisitedGraph.Vertices)
            {
                int component = Components[vertex];
                int equivalent = GetComponentEquivalence(component);
                if (component != equivalent)
                    Components[vertex] = equivalent;
            }
        }

        private void ReduceComponentsIndexes()
        {
            // Extract unique component indexes (sorted)
            var components = new SortedSet<int>();
            foreach (int componentNumber in Components.Values)
            {
                components.Add(componentNumber);
            }

            int[] sortedComponents = components.ToArray();

            // Compute component index reduction (via component equivalences)
            for (int i = 0; i < sortedComponents.Length - 1; ++i)
            {
                if (sortedComponents[i + 1] - sortedComponents[i] > 1)
                {
                    int newComponentNumber = sortedComponents[i] + 1;
                    _componentEquivalences.Add(sortedComponents[i + 1], newComponentNumber);
                    sortedComponents[i + 1] = newComponentNumber;
                }
            }

            // Apply the reduction of component indexes
            foreach (TVertex vertex in VisitedGraph.Vertices)
            {
                int component = Components[vertex];
                if (_componentEquivalences.TryGetValue(component, out int newComponentValue))
                    Components[vertex] = newComponentValue;
            }
        }

        #endregion

        #region IConnectedComponentAlgorithm<TVertex,TEdge,TGraph>

        /// <inheritdoc />
        public int ComponentCount { get; private set; }

        /// <inheritdoc />
        public IDictionary<TVertex, int> Components { get; }

        #endregion

        private void OnStartVertex([NotNull] TVertex vertex)
        {
            // We are looking on a new tree
            _currentComponent = _componentEquivalences.Count;
            _componentEquivalences.Add(_currentComponent, _currentComponent);
            ++ComponentCount;
            Components.Add(vertex, _currentComponent);
        }

        private void OnEdgeDiscovered([NotNull] TEdge edge)
        {
            // New edge, we store with the current component number
            Components.Add(edge.Target, _currentComponent);
        }

        private void OnForwardOrCrossEdge([NotNull] TEdge edge)
        {
            // We have touched another tree, updating count and current component
            int otherComponent = GetComponentEquivalence(Components[edge.Target]);
            if (otherComponent != _currentComponent)
            {
                --ComponentCount;

                Debug.Assert(ComponentCount > 0);
                if (_currentComponent > otherComponent)
                {
                    _componentEquivalences[_currentComponent] = otherComponent;
                    _currentComponent = otherComponent;
                }
                else
                {
                    _componentEquivalences[otherComponent] = _currentComponent;
                }
            }
        }

        private int GetComponentEquivalence(int component)
        {
            int equivalent = component;
            int temp = _componentEquivalences[equivalent];
            bool compress = false;
            while (temp != equivalent)
            {
                equivalent = temp;
                temp = _componentEquivalences[equivalent];
                compress = true;
            }

            // Path compression
            if (compress)
            {
                int c = component;
                temp = _componentEquivalences[c];
                while (temp != equivalent)
                {
                    temp = _componentEquivalences[c];
                    _componentEquivalences[c] = equivalent;
                }
            }

            return equivalent;
        }
    }
}