#if SUPPORTS_SERIALIZATION
using System;
#endif
using System.Collections.Generic;
using JetBrains.Annotations;
using QuikGraph.Algorithms.Services;
using QuikGraph.Collections;

namespace QuikGraph.Algorithms.TopologicalSort
{
    /// <summary>
    /// Topological sort algorithm (can be performed on an acyclic bidirectional graph).
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class SourceFirstBidirectionalTopologicalSortAlgorithm<TVertex, TEdge> :
        AlgorithmBase<IBidirectionalGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        [NotNull]
        private readonly BinaryQueue<TVertex, int> _heap;

        private readonly TopologicalSortDirection _direction;

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceFirstBidirectionalTopologicalSortAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        public SourceFirstBidirectionalTopologicalSortAlgorithm(
            [NotNull] IBidirectionalGraph<TVertex, TEdge> visitedGraph)
            : this(visitedGraph, TopologicalSortDirection.Forward)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceFirstBidirectionalTopologicalSortAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="direction">Sort direction.</param>
        public SourceFirstBidirectionalTopologicalSortAlgorithm(
            [NotNull] IBidirectionalGraph<TVertex, TEdge> visitedGraph,
            TopologicalSortDirection direction)
            : base(visitedGraph)
        {
            _direction = direction;
            _heap = new BinaryQueue<TVertex, int>(vertex => InDegrees[vertex]);
        }

        /// <summary>
        /// Sorted vertices.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull, ItemNotNull]
        public ICollection<TVertex> SortedVertices { get; private set; } = new List<TVertex>();

        /// <summary>
        /// Vertices in degrees.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        public IDictionary<TVertex, int> InDegrees { get; } = new Dictionary<TVertex, int>();

        /// <summary>
        /// Fired when a vertex is added to the set of sorted vertices.
        /// </summary>
        public event VertexAction<TVertex> VertexAdded;

        private void OnVertexAdded([NotNull] TVertex vertex)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(vertex != null);
#endif

            VertexAdded?.Invoke(vertex);
        }

        private void InitializeInDegrees()
        {
            foreach (TVertex vertex in VisitedGraph.Vertices)
            {
                InDegrees.Add(vertex, 0);
            }

            foreach (TEdge edge in VisitedGraph.Edges)
            {
                if (edge.IsSelfEdge())
                    continue;

                TVertex successor = _direction == TopologicalSortDirection.Forward
                    ? edge.Target
                    : edge.Source;

                ++InDegrees[successor];
            }

            foreach (TVertex vertex in VisitedGraph.Vertices)
            {
                _heap.Enqueue(vertex);
            }
        }

        /// <summary>
        /// Runs the topological sort and puts the result in the provided list.
        /// </summary>
        /// <param name="vertices">Set of sorted vertices.</param>
        public void Compute([NotNull, ItemNotNull] IList<TVertex> vertices)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(vertices != null);
#endif

            SortedVertices = vertices;
            SortedVertices.Clear();
            Compute();
        }

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            ICancelManager cancelManager = Services.CancelManager;
            InitializeInDegrees();

            while (_heap.Count != 0)
            {
                if (cancelManager.IsCancelling)
                    break;

                TVertex vertex = _heap.Dequeue();
                if (InDegrees[vertex] != 0)
                    throw new NonAcyclicGraphException();

                SortedVertices.Add(vertex);
                OnVertexAdded(vertex);

                // Update the count of its successor vertices
                IEnumerable<TEdge> successorEdges = _direction == TopologicalSortDirection.Forward
                    ? VisitedGraph.OutEdges(vertex)
                    : VisitedGraph.InEdges(vertex);

                foreach (TEdge edge in successorEdges)
                {
                    if (edge.IsSelfEdge())
                        continue;

                    TVertex successor = _direction == TopologicalSortDirection.Forward
                        ? edge.Target
                        : edge.Source;

                    --InDegrees[successor];

#if SUPPORTS_CONTRACTS
                    Contract.Assert(InDegrees[successor] >= 0);
#endif

                    _heap.Update(successor);
                }
            }
        }

        #endregion
    }
}
