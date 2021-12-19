#nullable enable

using FastGraph.Algorithms.Observers;
using FastGraph.Algorithms.Search;
using FastGraph.Algorithms.Services;
using FastGraph.Predicates;

namespace FastGraph.Algorithms.MaximumFlow
{
    /// <summary>
    /// Edmond and Karp maximum flow algorithm for directed graph with positive capacities and flows.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public sealed class EdmondsKarpMaximumFlowAlgorithm<TVertex, TEdge> : MaximumFlowAlgorithm<TVertex, TEdge>
        where TVertex : notnull
        where TEdge : IEdge<TVertex>
    {
        private readonly ReversedEdgeAugmentorAlgorithm<TVertex, TEdge> _reverserAlgorithm;

        /// <summary>
        /// Initializes a new instance of the <see cref="EdmondsKarpMaximumFlowAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="capacities">Function that given an edge return the capacity of this edge.</param>
        /// <param name="edgeFactory">Edge factory method.</param>
        /// <param name="reverseEdgesAugmentorAlgorithm">Algorithm that is in of charge of augmenting the graph (creating missing reversed edges).</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="capacities"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="edgeFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="reverseEdgesAugmentorAlgorithm"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="reverseEdgesAugmentorAlgorithm"/> targets a graph different from <paramref name="visitedGraph"/>.</exception>
        public EdmondsKarpMaximumFlowAlgorithm(
            IMutableVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> capacities,
            EdgeFactory<TVertex, TEdge> edgeFactory,
            ReversedEdgeAugmentorAlgorithm<TVertex, TEdge> reverseEdgesAugmentorAlgorithm)
            : this(default, visitedGraph, capacities, edgeFactory, reverseEdgesAugmentorAlgorithm)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EdmondsKarpMaximumFlowAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="capacities">Function that given an edge return the capacity of this edge.</param>
        /// <param name="edgeFactory">Edge factory method.</param>
        /// <param name="reverseEdgesAugmentorAlgorithm">Algorithm that is in of charge augmenting the graph (creating missing reversed edges).</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="capacities"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="edgeFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="reverseEdgesAugmentorAlgorithm"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="reverseEdgesAugmentorAlgorithm"/> targets a graph different from <paramref name="visitedGraph"/>.</exception>
        public EdmondsKarpMaximumFlowAlgorithm(
            IAlgorithmComponent? host,
            IMutableVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> capacities,
            EdgeFactory<TVertex, TEdge> edgeFactory,
            ReversedEdgeAugmentorAlgorithm<TVertex, TEdge> reverseEdgesAugmentorAlgorithm)
            : base(host, visitedGraph, capacities, edgeFactory)
        {
            if (reverseEdgesAugmentorAlgorithm is null)
                throw new ArgumentNullException(nameof(reverseEdgesAugmentorAlgorithm));
            if (!ReferenceEquals(visitedGraph, reverseEdgesAugmentorAlgorithm.VisitedGraph))
                throw new ArgumentException("Must target the same graph.", nameof(reverseEdgesAugmentorAlgorithm));

            _reverserAlgorithm = reverseEdgesAugmentorAlgorithm;
            ReversedEdges = reverseEdgesAugmentorAlgorithm.ReversedEdges;
        }

        private IVertexListGraph<TVertex, TEdge> ResidualGraph =>
            new FilteredVertexListGraph<TVertex, TEdge, IVertexListGraph<TVertex, TEdge>>(
                VisitedGraph,
                _ => true,
                new ResidualEdgePredicate<TVertex, TEdge>(ResidualCapacities).Test);

        private void Augment(TVertex source, TVertex sink)
        {
            // Find minimum residual capacity along the augmenting path
            double delta = double.MaxValue;
            TVertex u = sink;
            TEdge e;
            do
            {
                e = Predecessors[u];
                delta = Math.Min(delta, ResidualCapacities[e]);
                u = e.Source;
            } while (!EqualityComparer<TVertex>.Default.Equals(u, source));

            // Push delta units of flow along the augmenting path
            u = sink;
            do
            {
                e = Predecessors[u];
                ResidualCapacities[e] -= delta;
                if (ReversedEdges != default && ReversedEdges.ContainsKey(e))
                {
                    ResidualCapacities[ReversedEdges[e]] += delta;
                }
                u = e.Source;
            } while (!EqualityComparer<TVertex>.Default.Equals(u, source));
        }

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void Initialize()
        {
            base.Initialize();

            if (!_reverserAlgorithm.Augmented)
            {
                throw new InvalidOperationException(
                    $"The graph has not been augmented yet.{Environment.NewLine}" +
                    $"Call {nameof(ReversedEdgeAugmentorAlgorithm<int, Edge<int>>)}.{nameof(ReversedEdgeAugmentorAlgorithm<int, Edge<int>>.AddReversedEdges)}() before running this algorithm.");
            }

            if (Source == null)
                throw new InvalidOperationException("Source is not specified.");
            if (Sink == null)
                throw new InvalidOperationException("Sink is not specified.");
            if (!VisitedGraph.ContainsVertex(Source))
                throw new VertexNotFoundException("Source vertex is not part of the graph.");
            if (!VisitedGraph.ContainsVertex(Sink))
                throw new VertexNotFoundException("Sink vertex is not part of the graph.");
        }

        /// <summary>
        /// Computes the maximum flow between <see cref="MaximumFlowAlgorithm{TVertex,TEdge}.Source"/>
        /// and <see cref="MaximumFlowAlgorithm{TVertex,TEdge}.Sink"/>.
        /// </summary>
        protected override void InternalCompute()
        {
            ThrowIfCancellationRequested();

            IMutableVertexAndEdgeListGraph<TVertex, TEdge> graph = VisitedGraph;
            foreach (TVertex vertex in graph.Vertices)
            {
                foreach (TEdge edge in graph.OutEdges(vertex))
                {
                    double capacity = Capacities(edge);
                    if (capacity < 0)
                        throw new NegativeCapacityException();
                    ResidualCapacities[edge] = capacity;
                }
            }

            VerticesColors[Sink!] = GraphColor.Gray;
            while (VerticesColors[Sink!] != GraphColor.White)
            {
                var verticesPredecessors = new VertexPredecessorRecorderObserver<TVertex, TEdge>(Predecessors);
                var queue = new Collections.Queue<TVertex>();
                var bfs = new BreadthFirstSearchAlgorithm<TVertex, TEdge>(
                    ResidualGraph,
                    queue,
                    VerticesColors);

                using (verticesPredecessors.Attach(bfs))
                    bfs.Compute(Source!);

                if (VerticesColors[Sink!] != GraphColor.White)
                {
                    Augment(Source!, Sink!);
                }
            }

            MaxFlow = 0;
            foreach (TEdge edge in graph.OutEdges(Source!))
            {
                MaxFlow += Capacities(edge) - ResidualCapacities[edge];
            }
        }

        #endregion
    }
}
