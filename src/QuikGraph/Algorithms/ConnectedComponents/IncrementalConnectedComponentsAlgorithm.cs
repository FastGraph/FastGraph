using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using QuikGraph.Algorithms.Services;
using QuikGraph.Collections;

namespace QuikGraph.Algorithms.ConnectedComponents
{
    /// <summary>
    /// Algorithm that incrementally computes connected components of a growing graph.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public sealed class IncrementalConnectedComponentsAlgorithm<TVertex, TEdge>
        : AlgorithmBase<IMutableVertexAndEdgeSet<TVertex, TEdge>>
        , IDisposable
        where TEdge : IEdge<TVertex>
    {
        private ForestDisjointSet<TVertex> _sets;

        private bool _hooked;

        /// <summary>
        /// Initializes a new instance of the <see cref="IncrementalConnectedComponentsAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        public IncrementalConnectedComponentsAlgorithm(
            [NotNull] IMutableVertexAndEdgeSet<TVertex, TEdge> visitedGraph)
            : this(null, visitedGraph)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IncrementalConnectedComponentsAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        public IncrementalConnectedComponentsAlgorithm(
            [CanBeNull] IAlgorithmComponent host,
            [NotNull] IMutableVertexAndEdgeSet<TVertex, TEdge> visitedGraph)
            : base(host, visitedGraph)
        {
        }

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            _sets = new ForestDisjointSet<TVertex>(VisitedGraph.VertexCount);

            // Initialize one set per vertex
            foreach (TVertex vertex in VisitedGraph.Vertices)
                _sets.MakeSet(vertex);

            // Join existing edges
            foreach (TEdge edge in VisitedGraph.Edges)
                _sets.Union(edge.Source, edge.Target);

            // Hook to graph event
            if (_hooked)
                return;

            VisitedGraph.EdgeAdded += OnEdgeAdded;
            VisitedGraph.EdgeRemoved += OnEdgeRemoved;
            VisitedGraph.VertexAdded += OnVertexAdded;
            VisitedGraph.VertexRemoved += OnVertexRemoved;

            _hooked = true;
        }

        #endregion

        /// <summary>
        /// Number of components.
        /// </summary>
        public int ComponentCount
        {
            get
            {
                if (_sets is null)
                    throw new InvalidOperationException("Run the algorithm before getting the number of components.");
                return _sets.SetCount;
            }
        }

        private Dictionary<TVertex, int> _components;

        /// <summary>
        /// Gets a copy of the connected components. Key is the number of components,
        /// Value contains the vertex -> component index map.
        /// </summary>
        /// <returns>Number of components associated to components vertex mapping.</returns>
        public KeyValuePair<int, IDictionary<TVertex, int>> GetComponents()
        {
            if (_sets is null)
                throw new InvalidOperationException("Run the algorithm before.");

            var representatives = new Dictionary<TVertex, int>(_sets.SetCount);
            if (_components is null)
                _components = new Dictionary<TVertex, int>(VisitedGraph.VertexCount);
            foreach (TVertex vertex in VisitedGraph.Vertices)
            {
                TVertex representative = _sets.FindSet(vertex);
                // ReSharper disable once AssignNullToNotNullAttribute, Justification: All graph vertices are in a set
                if (!representatives.TryGetValue(representative, out int index))
                    representatives[representative] = index = representatives.Count;
                _components[vertex] = index;
            }

            Debug.Assert(_sets.SetCount == ComponentCount);
            Debug.Assert(_components.Count == VisitedGraph.VertexCount);
            return new KeyValuePair<int, IDictionary<TVertex, int>>(_sets.SetCount, _components);
        }

        private void OnVertexAdded([NotNull] TVertex vertex)
        {
            _sets.MakeSet(vertex);
        }

        private void OnEdgeAdded([NotNull] TEdge edge)
        {
            _sets.Union(edge.Source, edge.Target);
        }

        private static void OnVertexRemoved([NotNull] TVertex vertex)
        {
            throw new InvalidOperationException("Vertex removal is not supported for incremental connected components.");
        }

        private static void OnEdgeRemoved([NotNull] TEdge edge)
        {
            throw new InvalidOperationException("Edge removal is not supported for incremental connected components.");
        }

        #region IDisposable

        /// <inheritdoc />
        public void Dispose()
        {
            // Unhook from graph event
            if (!_hooked)
                return;

            VisitedGraph.EdgeAdded -= OnEdgeAdded;
            VisitedGraph.EdgeRemoved -= OnEdgeRemoved;
            VisitedGraph.VertexAdded -= OnVertexAdded;
            VisitedGraph.VertexRemoved -= OnVertexRemoved;
        }

        #endregion
    }
}