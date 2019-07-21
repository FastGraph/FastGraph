using System;
using System.Collections.Generic;
using System.Linq;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using JetBrains.Annotations;
using QuikGraph.Algorithms.Services;
using QuikGraph.Collections;

namespace QuikGraph.Algorithms.TopologicalSort
{
    /// <summary>
    /// Undirected topological sort algorithm.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class UndirectedFirstTopologicalSortAlgorithm<TVertex, TEdge> : AlgorithmBase<IUndirectedGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        [NotNull]
        private readonly BinaryQueue<TVertex, int> _heap;

        /// <summary>
        /// Initializes a new instance of the <see cref="UndirectedFirstTopologicalSortAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        public UndirectedFirstTopologicalSortAlgorithm([NotNull] IUndirectedGraph<TVertex, TEdge> visitedGraph)
            : base(visitedGraph)
        {
            _heap = new BinaryQueue<TVertex, int>(vertex => Degrees[vertex]);
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
        /// Vertices degrees.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        public IDictionary<TVertex, int> Degrees { get; } = new Dictionary<TVertex, int>();

        /// <summary>
        /// Gets or sets the flag that indicates if cyclic graph are supported or not.
        /// </summary>
        public bool AllowCyclicGraph { get; set; }

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
                Degrees.Add(vertex, VisitedGraph.AdjacentDegree(vertex));
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
                    return;

                TVertex vertex = _heap.Dequeue();
                if (Degrees[vertex] != 0 && !AllowCyclicGraph)
                    throw new NonAcyclicGraphException();

                SortedVertices.Add(vertex);
                OnVertexAdded(vertex);

                // Update the count of its adjacent vertices
                UpdateAdjacentDegree(vertex);
            }

            #region Local function

            void UpdateAdjacentDegree(TVertex vertex)
            {
                foreach (TEdge edge in VisitedGraph.AdjacentEdges(vertex).Where(e => !e.IsSelfEdge()))
                {
                    --Degrees[edge.Target];

                    if (Degrees[edge.Target] < 0 && !AllowCyclicGraph)
                        throw new InvalidOperationException("Degree is negative, and cannot be.");

                    if (_heap.Contains(edge.Target))
                        _heap.Update(edge.Target);
                }
            }

            #endregion
        }

        #endregion
    }
}
