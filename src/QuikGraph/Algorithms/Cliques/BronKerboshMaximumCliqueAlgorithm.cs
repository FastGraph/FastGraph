// TODO: Under construction
//using System.Collections.Generic;
//using JetBrains.Annotations;
//using QuikGraph.Algorithms.Services;

//namespace QuikGraph.Algorithms.Cliques
//{
//    public class BronKerboshMaximumCliqueAlgorithm<TVertex, TEdge> : MaximumCliqueAlgorithmBase<TVertex, TEdge>
//        where TEdge : IEdge<TVertex>
//    {
//        /// <summary>
//        /// Initializes a new instance of the <see cref="BronKerboshMaximumCliqueAlgorithm{TVertex,TEdge}"/> class.
//        /// </summary>
//        /// <param name="host">Host to use if set, otherwise use this reference.</param>
//        /// <param name="visitedGraph">Graph to visit.</param>
//        protected BronKerboshMaximumCliqueAlgorithm(
//            [CanBeNull] IAlgorithmComponent host,
//            [NotNull] IUndirectedGraph<TVertex, TEdge> visitedGraph)
//            : base(host, visitedGraph)
//        {
//        }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="BronKerboshMaximumCliqueAlgorithm{TVertex,TEdge}"/> class.
//        /// </summary>
//        /// <param name="visitedGraph">Graph to visit.</param>
//        protected BronKerboshMaximumCliqueAlgorithm(
//            [NotNull] IUndirectedGraph<TVertex, TEdge> visitedGraph)
//            : base(visitedGraph)
//        {
//        }

//        #region AlgorithmBase<TGraph>

//        /// <inheritdoc />
//        protected override void InternalCompute()
//        {
//            // the currently growing clique;
//            var R = new List<TVertex>();
//            // prospective nodes which are connected to all nodes in R 
//            // and using which R can be expanded
//            var P = new List<TVertex>();
//            // nodes already processed i.e. nodes which were previously in P 
//            // and hence all maximal cliques containing them have already been reported
//            var X = new List<TVertex>();

//            // An important invariant is that all nodes which are connected to every node 
//            // of R are either in P or X.

//        }

//        #endregion
//    }
//}
