#nullable enable

using FastGraph.Algorithms.Services;
using FastGraph.Collections;

namespace FastGraph.Algorithms.MinimumSpanningTree
{
    /// <summary>
    /// Prim minimum spanning tree algorithm implementation.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public sealed class PrimMinimumSpanningTreeAlgorithm<TVertex, TEdge>
        : AlgorithmBase<IUndirectedGraph<TVertex, TEdge>>
            , IMinimumSpanningTreeAlgorithm<TVertex, TEdge>
        where TVertex : notnull
        where TEdge : IEdge<TVertex>
    {
        private readonly Func<TEdge, double>? _edgeWeights;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimMinimumSpanningTreeAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="edgeWeights">Function that computes the weight for a given edge.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="edgeWeights"/> is <see langword="null"/>.</exception>
        public PrimMinimumSpanningTreeAlgorithm(
            IUndirectedGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> edgeWeights)
            : this(default, visitedGraph, edgeWeights)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimMinimumSpanningTreeAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="edgeWeights">Function that computes the weight for a given edge.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="edgeWeights"/> is <see langword="null"/>.</exception>
        public PrimMinimumSpanningTreeAlgorithm(
            IAlgorithmComponent? host,
            IUndirectedGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> edgeWeights)
            : base(host, visitedGraph)
        {
            _edgeWeights = edgeWeights ?? throw new ArgumentNullException(nameof(edgeWeights));
        }

        /// <summary>
        /// Fired when an edge is going to be analyzed.
        /// </summary>
        public event EdgeAction<TVertex, TEdge>? ExamineEdge;

        private void OnExamineEdge(TEdge edge)
        {
            ExamineEdge?.Invoke(edge);
        }

        #region ITreeBuilderAlgorithm<TVertex,TEdge>

        /// <inheritdoc />
        public event EdgeAction<TVertex, TEdge>? TreeEdge;

        private void OnTreeEdge(TEdge edge)
        {
            TreeEdge?.Invoke(edge);
        }

        #endregion

        #region AlgorithmBase<TGraph>

        private Dictionary<TVertex, HashSet<TEdge>>? _verticesEdges;
        private HashSet<TVertex>? _visitedVertices;
        private ForestDisjointSet<TVertex>? _sets;

        private HashSet<TEdge>? _edges;
        private BinaryQueue<TEdge, double>? _queue;

        private void InitializeVerticesToEdges()
        {
            _verticesEdges = new Dictionary<TVertex, HashSet<TEdge>>();
            _visitedVertices = new HashSet<TVertex>();
            _sets = new ForestDisjointSet<TVertex>(VisitedGraph.VertexCount);

            foreach (TVertex vertex in VisitedGraph.Vertices)
            {
                if (_visitedVertices.Count == 0)
                {
                    _visitedVertices.Add(vertex);
                }

                _sets.MakeSet(vertex);
                _verticesEdges.Add(vertex, new HashSet<TEdge>());
            }

            foreach (TEdge edge in VisitedGraph.Edges)
            {
                _verticesEdges[edge.Source].Add(edge);
                _verticesEdges[edge.Target].Add(edge);
            }
        }

        private void InitializeQueue()
        {
            _edges = new HashSet<TEdge>();
            _queue = new BinaryQueue<TEdge, double>(_edgeWeights!);
            TVertex lastVertex = _visitedVertices!.First();
            foreach (TEdge edge in _verticesEdges![lastVertex])
            {
                if (!_edges.Contains(edge))
                {
                    _edges.Add(edge);
                    _queue.Enqueue(edge);
                }
            }
        }

        /// <inheritdoc />
        protected override void Initialize()
        {
            base.Initialize();

            InitializeVerticesToEdges();

            ThrowIfCancellationRequested();

            InitializeQueue();
        }

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            ThrowIfCancellationRequested();

            while (_edges!.Count > 0 && _visitedVertices!.Count < VisitedGraph.VertexCount)
            {
                TEdge minEdge = _queue!.Dequeue();
                OnExamineEdge(minEdge);

                if (!_sets!.AreInSameSet(minEdge.Source, minEdge.Target))
                {
                    OnTreeEdge(minEdge);
                    _sets.Union(minEdge.Source, minEdge.Target);

                    TVertex lastVertex;
                    if (_visitedVertices.Contains(minEdge.Source))
                    {
                        lastVertex = minEdge.Target;
                        _visitedVertices.Add(minEdge.Target);
                    }
                    else
                    {
                        lastVertex = minEdge.Source;
                        _visitedVertices.Add(minEdge.Source);
                    }

                    EnqueueEdgesFrom(lastVertex);
                }
            }
        }

        private void EnqueueEdgesFrom(TVertex vertex)
        {
            foreach (TEdge edge in _verticesEdges![vertex])
            {
                if (_edges!.Contains(edge))
                    continue;

                _edges!.Add(edge);
                _queue!.Enqueue(edge);
            }
        }

        #endregion
    }
}
