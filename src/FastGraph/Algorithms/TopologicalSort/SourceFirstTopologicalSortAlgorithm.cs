#nullable enable

using System.Diagnostics;
using FastGraph.Collections;

namespace FastGraph.Algorithms.TopologicalSort
{
    /// <summary>
    /// Topological sort algorithm (can be performed on an acyclic graph).
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public sealed class SourceFirstTopologicalSortAlgorithm<TVertex, TEdge> : AlgorithmBase<IVertexAndEdgeListGraph<TVertex, TEdge>>
        where TVertex : notnull
        where TEdge : IEdge<TVertex>
    {
        private readonly BinaryQueue<TVertex, int> _heap;

        private readonly IList<TVertex> _sortedVertices;

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceFirstTopologicalSortAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="capacity">Sorted vertices capacity.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        public SourceFirstTopologicalSortAlgorithm(
            IVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph,
            int capacity = -1)
            : base(visitedGraph)
        {
            _heap = new BinaryQueue<TVertex, int>(vertex => InDegrees[vertex]);
            _sortedVertices = capacity > 0 ? new List<TVertex>(capacity) : new List<TVertex>();
        }

        /// <summary>
        /// Sorted vertices.
        /// </summary>
        public TVertex[]? SortedVertices { get; private set; }

        /// <summary>
        /// Vertices in-degrees.
        /// </summary>
        public IDictionary<TVertex, int> InDegrees { get; } = new Dictionary<TVertex, int>();

        /// <summary>
        /// Fired when a vertex is added to the set of sorted vertices.
        /// </summary>
        public event VertexAction<TVertex>? VertexAdded;

        private void OnVertexAdded(TVertex vertex)
        {
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
                    throw new NonAcyclicGraphException();

                ++InDegrees[edge.Target];
            }

            foreach (TVertex vertex in VisitedGraph.Vertices)
            {
                _heap.Enqueue(vertex);
            }
        }

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void Initialize()
        {
            base.Initialize();

            SortedVertices = default;
            _sortedVertices.Clear();
            InDegrees.Clear();

            InitializeInDegrees();
        }

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            while (_heap.Count != 0)
            {
                ThrowIfCancellationRequested();

                TVertex vertex = _heap.Dequeue();
                if (InDegrees[vertex] != 0)
                    throw new NonAcyclicGraphException();

                _sortedVertices.Add(vertex);
                OnVertexAdded(vertex);

                // Update the count of its adjacent vertices
                foreach (TVertex target in VisitedGraph.OutEdges(vertex).Select(outEdge => outEdge.Target))
                {
                    --InDegrees[target];

                    Debug.Assert(InDegrees[target] >= 0);

                    _heap.Update(target);
                }
            }

            SortedVertices = _sortedVertices.ToArray();
        }

        #endregion
    }
}
