using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using QuikGraph.Algorithms.Search;

namespace QuikGraph.Algorithms.TopologicalSort
{
    /// <summary>
    /// Topological sort algorithm (can be performed on an acyclic graph).
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class TopologicalSortAlgorithm<TVertex, TEdge>
        : AlgorithmBase<IVertexListGraph<TVertex, TEdge>>
        , IVertexTimeStamperAlgorithm<TVertex>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TopologicalSortAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        public TopologicalSortAlgorithm([NotNull] IVertexListGraph<TVertex, TEdge> visitedGraph)
            : this(visitedGraph, new List<TVertex>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TopologicalSortAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="vertices">Set of sorted vertices.</param>
        public TopologicalSortAlgorithm(
            [NotNull] IVertexListGraph<TVertex, TEdge> visitedGraph,
            [NotNull, ItemNotNull] IList<TVertex> vertices)
            : base(visitedGraph)
        {
            SortedVertices = vertices ?? throw new ArgumentNullException(nameof(vertices));
        }

        /// <summary>
        /// Sorted vertices.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull, ItemNotNull]
        public IList<TVertex> SortedVertices { get; private set; }

        private static void OnBackEdge([NotNull] TEdge args)
        {
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
            if (vertices is null)
                throw new ArgumentNullException(nameof(vertices));

            SortedVertices = vertices;
            SortedVertices.Clear();
            Compute();
        }

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            DepthFirstSearchAlgorithm<TVertex, TEdge> dfs = null;
            try
            {
                dfs = new DepthFirstSearchAlgorithm<TVertex, TEdge>(
                    this,
                    VisitedGraph,
                    new Dictionary<TVertex, GraphColor>(VisitedGraph.VertexCount));
                dfs.BackEdge += OnBackEdge;
                dfs.FinishVertex += OnVertexFinished;
                dfs.DiscoverVertex += DiscoverVertex;
                dfs.FinishVertex += FinishVertex;

                dfs.Compute();
            }
            finally
            {
                if (dfs != null)
                {
                    dfs.BackEdge -= OnBackEdge;
                    dfs.FinishVertex -= OnVertexFinished;
                    dfs.DiscoverVertex -= DiscoverVertex;
                    dfs.FinishVertex -= FinishVertex;
                }
            }
        }

        #endregion

        #region IVertexTimeStamperAlgorithm<TVertex>

        /// <inheritdoc />
        public event VertexAction<TVertex> DiscoverVertex;

        /// <inheritdoc />
        public event VertexAction<TVertex> FinishVertex;

        #endregion
    }
}