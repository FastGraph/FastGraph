using System;
using JetBrains.Annotations;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using QuikGraph.Algorithms.Observers;
using QuikGraph.Algorithms.Search;
using QuikGraph.Algorithms.Services;
using QuikGraph.Collections;
using QuikGraph.Predicates;

namespace QuikGraph.Algorithms.MaximumFlow
{
    /// <summary>
    /// Edmond and Karp maximum flow algorithm for directed graph with positive capacities and flows.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    /// <remarks>
    /// Will throw an exception in <see cref="ReversedEdgeAugmentorAlgorithm{TVertex, TEdge}.AddReversedEdges()"/> if TEdge is a value type,
    /// e.g. <see cref="SEdge{TVertex}"/>.
    /// <seealso href="https://github.com/YaccConstructor/QuickGraph/issues/183#issue-377613647"/>.
    /// </remarks>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class EdmondsKarpMaximumFlowAlgorithm<TVertex, TEdge> : MaximumFlowAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EdmondsKarpMaximumFlowAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="capacities">Function that given an edge return the capacity of this edge.</param>
        /// <param name="edgeFactory">Edge factory method.</param>
        /// <param name="reversedEdgeAugmentorAlgorithm">Algorithm that is in of charge of augmenting the graph (creating missing reversed edges).</param>
        public EdmondsKarpMaximumFlowAlgorithm(
            [NotNull] IMutableVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph,
            [NotNull] Func<TEdge, double> capacities,
            [NotNull] EdgeFactory<TVertex, TEdge> edgeFactory,
            [NotNull] ReversedEdgeAugmentorAlgorithm<TVertex, TEdge> reversedEdgeAugmentorAlgorithm)
            : this(null, visitedGraph, capacities, edgeFactory, reversedEdgeAugmentorAlgorithm)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EdmondsKarpMaximumFlowAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="capacities">Function that given an edge return the capacity of this edge.</param>
        /// <param name="edgeFactory">Edge factory method.</param>
        /// <param name="reversedEdgeAugmentorAlgorithm">Algorithm that is in of charge augmenting the graph (creating missing reversed edges).</param>
        public EdmondsKarpMaximumFlowAlgorithm(
            [CanBeNull] IAlgorithmComponent host,
            [NotNull] IMutableVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph,
            [NotNull] Func<TEdge, double> capacities,
            [NotNull] EdgeFactory<TVertex, TEdge> edgeFactory,
            [NotNull] ReversedEdgeAugmentorAlgorithm<TVertex, TEdge> reversedEdgeAugmentorAlgorithm)
            : base(host, visitedGraph, capacities, edgeFactory)
        {
            ReversedEdges = reversedEdgeAugmentorAlgorithm.ReversedEdges;
        }

        [NotNull]
        private IVertexListGraph<TVertex, TEdge> ResidualGraph =>
            new FilteredVertexListGraph<TVertex, TEdge, IVertexListGraph<TVertex, TEdge>>(
                VisitedGraph,
                vertex => true,
                new ResidualEdgePredicate<TVertex, TEdge>(ResidualCapacities).Test);

        private void Augment([NotNull] TVertex source, [NotNull] TVertex sink)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(source != null);
            Contract.Requires(sink != null);
#endif

            // Find minimum residual capacity along the augmenting path
            double delta = double.MaxValue;
            TVertex u = sink;
            TEdge e;
            do
            {
                e = Predecessors[u];
                delta = Math.Min(delta, ResidualCapacities[e]);
                u = e.Source;
            } while (!u.Equals(source));

            // Push delta units of flow along the augmenting path
            u = sink;
            do
            {
                e = Predecessors[u];
                ResidualCapacities[e] -= delta;
                if (ReversedEdges != null && ReversedEdges.ContainsKey(e))
                {
                    ResidualCapacities[ReversedEdges[e]] += delta;
                }
                u = e.Source;
            } while (!u.Equals(source));
        }

        /// <summary>
        /// Computes the maximum flow between Source and Sink.
        /// </summary>
        protected override void InternalCompute()
        {
            if (Source == null)
                throw new InvalidOperationException("Source is not specified.");
            if (Sink == null)
                throw new InvalidOperationException("Sink is not specified.");

            if (Services.CancelManager.IsCancelling)
                return;

            var graph = VisitedGraph;
            foreach (TVertex vertex in graph.Vertices)
            {
                foreach (TEdge edge in graph.OutEdges(vertex))
                {
                    double capacity = Capacities(edge);
                    if (capacity < 0)
                        throw new InvalidOperationException("Negative edge capacity.");
                    ResidualCapacities[edge] = capacity;
                }
            }

            VerticesColors[Sink] = GraphColor.Gray;
            while (VerticesColors[Sink] != GraphColor.White)
            {
                var verticesPredecessors = new VertexPredecessorRecorderObserver<TVertex, TEdge>(Predecessors);
                var queue = new Queue<TVertex>();
                var bfs = new BreadthFirstSearchAlgorithm<TVertex, TEdge>(
                    ResidualGraph,
                    queue,
                    VerticesColors);

                using (verticesPredecessors.Attach(bfs))
                    bfs.Compute(Source);

                if (VerticesColors[Sink] != GraphColor.White)
                    Augment(Source, Sink);
            }

            MaxFlow = 0;
            foreach (TEdge edge in graph.OutEdges(Source))
                MaxFlow += (Capacities(edge) - ResidualCapacities[edge]);
        }
    }
}