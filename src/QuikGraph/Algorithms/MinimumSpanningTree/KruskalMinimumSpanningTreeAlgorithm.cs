using System;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using QuikGraph.Algorithms.Services;
using QuikGraph.Collections;

namespace QuikGraph.Algorithms.MinimumSpanningTree
{
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class KruskalMinimumSpanningTreeAlgorithm<TVertex, TEdge> 
        : AlgorithmBase<IUndirectedGraph<TVertex,TEdge>>
        , IMinimumSpanningTreeAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        readonly Func<TEdge, double> edgeWeights;

        public KruskalMinimumSpanningTreeAlgorithm(
            IUndirectedGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> edgeWeights
            )
            : this(null, visitedGraph, edgeWeights)
        {}

        public KruskalMinimumSpanningTreeAlgorithm(
            IAlgorithmComponent host,
            IUndirectedGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> edgeWeights
            )
            :base(host, visitedGraph)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(edgeWeights != null);
#endif

            this.edgeWeights = edgeWeights;
        }

        public event EdgeAction<TVertex, TEdge> ExamineEdge;
        private void OnExamineEdge(TEdge edge)
        {
            var eh = this.ExamineEdge;
            if (eh != null)
                eh(edge);
        }

        public event EdgeAction<TVertex, TEdge> TreeEdge;
        private void OnTreeEdge(TEdge edge)
        {
            var eh = this.TreeEdge;
            if (eh != null)
                eh(edge);
        }

        protected override void InternalCompute()
        {
            var cancelManager = this.Services.CancelManager;
            var ds = new ForestDisjointSet<TVertex>(this.VisitedGraph.VertexCount);
            foreach (var v in this.VisitedGraph.Vertices)
                ds.MakeSet(v);

            if (cancelManager.IsCancelling)
                return;

            var queue = new BinaryQueue<TEdge, double>(this.edgeWeights);
            foreach (var e in this.VisitedGraph.Edges)
                queue.Enqueue(e);

            if (cancelManager.IsCancelling)
                return;

            while (queue.Count > 0)
            {
                var e = queue.Dequeue();
                this.OnExamineEdge(e);
                if (!ds.AreInSameSet(e.Source, e.Target))
                {
                    this.OnTreeEdge(e);
                    ds.Union(e.Source, e.Target);
                }
            }
        }
    }
}
