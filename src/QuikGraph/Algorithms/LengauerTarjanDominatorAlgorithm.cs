// TODO: Finish the implementation
//using System.Collections.Generic;
//using JetBrains.Annotations;
//using QuikGraph.Algorithms.Observers;
//using QuikGraph.Algorithms.Search;
//using QuikGraph.Algorithms.Services;

//namespace QuikGraph.Algorithms
//{
//    /// <summary>
//    /// Algorithm that computes the domination map of a directed graph.
//    /// </summary>
//    /// <remarks>
//    /// Thomas Lengauer and Robert Endre Tarjan.
//    /// A fast algorithm for finding dominator in a flow graph.
//    /// ACM Transactions on Programming Language and Systems, 1(1):121-141, 1979. 
//    /// </remarks>
//    /// <typeparam name="TVertex">Vertex type.</typeparam>
//    /// <typeparam name="TEdge">Edge type.</typeparam>
//    internal class LengauerTarjanDominatorAlgorithm<TVertex, TEdge> : RootedAlgorithmBase<TVertex, IBidirectionalGraph<TVertex, TEdge>>
//        where TEdge : IEdge<TVertex>
//    {
//        /// <summary>
//        /// Initializes a new instance of the <see cref="LengauerTarjanDominatorAlgorithm{TVertex,TEdge}"/> class.
//        /// </summary>
//        /// <param name="host">Host to use if set, otherwise use this reference.</param>
//        /// <param name="visitedGraph">Graph to visit.</param>
//        public LengauerTarjanDominatorAlgorithm(
//            [CanBeNull] IAlgorithmComponent host,
//            [NotNull] IBidirectionalGraph<TVertex, TEdge> visitedGraph)
//            : base(host, visitedGraph)
//        {
//        }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="LengauerTarjanDominatorAlgorithm{TVertex,TEdge}"/> class.
//        /// </summary>
//        /// <param name="visitedGraph">Graph to visit.</param>
//        public LengauerTarjanDominatorAlgorithm([NotNull] IBidirectionalGraph<TVertex, TEdge> visitedGraph)
//            : this(null, visitedGraph)
//        {
//        }

//        #region AlgorithmBase<TGraph>

//        /// <inheritdoc />
//        protected override void InternalCompute()
//        {
//            ICancelManager cancelManager = Services.CancelManager;
//            int vertexCount = VisitedGraph.VertexCount;
//            IEnumerable<TVertex> vertices = VisitedGraph.Vertices;

//            var timeStamps = new Dictionary<TVertex, int>(vertexCount);
//            //var stamps = new List<TVertex>(vertexCount);
//            var predecessors = new Dictionary<TVertex, TEdge>(vertexCount);

//            // Phase 1: DFS over the graph and record vertices indexes
//            var dfs = new DepthFirstSearchAlgorithm<TVertex, TEdge>(this, VisitedGraph);
//            //using (new TimeStampObserver(stamps).Attach(dfs))
//            using (new VertexTimeStamperObserver<TVertex, TEdge>(timeStamps).Attach(dfs))
//            using (new VertexPredecessorRecorderObserver<TVertex, TEdge>(predecessors).Attach(dfs))
//                dfs.Compute();

//            if (cancelManager.IsCancelling)
//                return;

//            // Phase 2: Find semi dominator
//            var semi = new Dictionary<TVertex, TVertex>(vertexCount);
//            foreach (TVertex v in vertices)
//            {
//                if (!timeStamps.TryGetValue(v, out int vTime) 
//                    || !predecessors.TryGetValue(v, out TEdge dominatorEdge))
//                    continue; // Skip unreachable

//                TVertex dominator = dominatorEdge.Source;
//                if (!timeStamps.TryGetValue(dominator, out int dominatorTime))
//                    continue;

//                foreach (TEdge edge in VisitedGraph.InEdges(v))
//                {
//                    var u = edge.Source;
//                    if (!timeStamps.TryGetValue(u, out int uTime))
//                        continue;

//                    TVertex candidate;
//                    if (uTime < vTime)
//                    {
//                        candidate = u;
//                    }
//                    else
//                    {
//                        TVertex ancestor = default(TVertex);
//                        candidate = semi[ancestor];
//                    }

//                    int candidateTime = timeStamps[candidate];
//                    if (candidateTime < dominatorTime)
//                    {
//                        dominator = candidate;
//                        dominatorTime = candidateTime;
//                    }
//                }

//                semi[v] = dominator;
//            }

//            // Phase 3: TODO
//        }

//        #endregion

//        private class TimeStampObserver : Observers.IObserver<IVertexTimeStamperAlgorithm<TVertex>>
//        {
//            private readonly List<TVertex> _vertices;

//            public TimeStampObserver([NotNull, ItemNotNull] List<TVertex> vertices)
//            {
//#if SUPPORTS_CONTRACTS
//                Contract.Requires(vertices != null);
//#endif

//                _vertices = vertices;
//            }

//            /// <inheritdoc />
//            public IDisposable Attach(IVertexTimeStamperAlgorithm<TVertex> algorithm)
//            {
//                algorithm.DiscoverVertex += OnDiscoveredVertex;
//                return Finally(() => algorithm.DiscoverVertex -= OnDiscoveredVertex);
//            }

//            private void OnDiscoveredVertex([NotNull] TVertex vertex)
//            {
//                _vertices.Add(vertex);
//            }
//        }
//    }
//}
