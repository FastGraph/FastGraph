using System;
using System.Collections.Generic;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using JetBrains.Annotations;
using QuikGraph.Algorithms.Search;

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
    public sealed class UndirectedTopologicalSortAlgorithm<TVertex, TEdge> : AlgorithmBase<IUndirectedGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UndirectedTopologicalSortAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        public UndirectedTopologicalSortAlgorithm([NotNull] IUndirectedGraph<TVertex, TEdge> visitedGraph)
            : this(visitedGraph, new List<TVertex>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UndirectedTopologicalSortAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="vertices">Set of sorted vertices.</param>
        public UndirectedTopologicalSortAlgorithm(
            [NotNull] IUndirectedGraph<TVertex, TEdge> visitedGraph,
            [NotNull, ItemNotNull] IList<TVertex> vertices)
            : base(visitedGraph)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(vertices != null);
#endif

            SortedVertices = vertices;
        }

        /// <summary>
        /// Sorted vertices.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull, ItemNotNull]
        public IList<TVertex> SortedVertices { get; private set; }

        /// <summary>
        /// Gets or sets the flag that indicates if cyclic graph are supported or not.
        /// </summary>
        public bool AllowCyclicGraph { get; set; }

        private void BackEdge([NotNull] object sender, [NotNull] UndirectedEdgeEventArgs<TVertex, TEdge> args)
        {
            if (!AllowCyclicGraph)
                throw new NonAcyclicGraphException();
        }

        private void OnVertexFinished([NotNull] TVertex vertex)
        {
            SortedVertices.Insert(0, vertex);
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
            UndirectedDepthFirstSearchAlgorithm<TVertex, TEdge> dfs = null;
            try
            {
                dfs = new UndirectedDepthFirstSearchAlgorithm<TVertex, TEdge>(
                    this,
                    VisitedGraph,
                    new Dictionary<TVertex, GraphColor>(VisitedGraph.VertexCount));
                dfs.BackEdge += BackEdge;
                dfs.FinishVertex += OnVertexFinished;

                dfs.Compute();
            }
            finally
            {
                if (dfs != null)
                {
                    dfs.BackEdge -= BackEdge;
                    dfs.FinishVertex -= OnVertexFinished;
                }
            }
        }

        #endregion
    }
}
