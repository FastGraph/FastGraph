using System;
using System.Collections.Generic;
using System.Linq;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using JetBrains.Annotations;
using QuikGraph.Algorithms.Services;
using QuikGraph.Collections;

namespace QuikGraph.Algorithms.MinimumSpanningTree
{
    /// <summary>
    /// Prim minimum spanning tree algorithm implementation.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class PrimMinimumSpanningTreeAlgorithm<TVertex, TEdge>
        : AlgorithmBase<IUndirectedGraph<TVertex, TEdge>>
            , IMinimumSpanningTreeAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        [NotNull]
        private readonly Func<TEdge, double> _edgeWeights;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimMinimumSpanningTreeAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="edgeWeights">Function that computes the weight for a given edge.</param>
        public PrimMinimumSpanningTreeAlgorithm(
            [NotNull] IUndirectedGraph<TVertex, TEdge> visitedGraph,
            [NotNull] Func<TEdge, double> edgeWeights)
            : this(null, visitedGraph, edgeWeights)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimMinimumSpanningTreeAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="edgeWeights">Function that computes the weight for a given edge.</param>
        public PrimMinimumSpanningTreeAlgorithm(
            [CanBeNull] IAlgorithmComponent host,
            [NotNull] IUndirectedGraph<TVertex, TEdge> visitedGraph,
            [NotNull] Func<TEdge, double> edgeWeights)
            : base(host, visitedGraph)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(edgeWeights != null);
#endif

            _edgeWeights = edgeWeights;
        }

        /// <summary>
        /// Fired when an edge is going to be analyzed.
        /// </summary>
        public event EdgeAction<TVertex, TEdge> ExamineEdge;

        private void OnExamineEdge([NotNull] TEdge edge)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(edge != null);
#endif

            ExamineEdge?.Invoke(edge);
        }

        #region ITreeBuilderAlgorithm<TVertex,TEdge>

        /// <inheritdoc />
        public event EdgeAction<TVertex, TEdge> TreeEdge;

        private void OnTreeEdge([NotNull] TEdge edge)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(edge != null);
#endif

            TreeEdge?.Invoke(edge);
        }

        #endregion

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            ICancelManager cancelManager = Services.CancelManager;

            var verticesEdges = new Dictionary<TVertex, HashSet<TEdge>>();
            var visitedVertices = new HashSet<TVertex>();
            var edges = new HashSet<TEdge>();
            var queue = new BinaryQueue<TEdge, double>(_edgeWeights);
            var sets = new ForestDisjointSet<TVertex>(VisitedGraph.VertexCount);

            foreach (TVertex vertex in VisitedGraph.Vertices)
            {
                if (visitedVertices.Count == 0)
                {
                    visitedVertices.Add(vertex);
                }

                sets.MakeSet(vertex);
                verticesEdges.Add(vertex, new HashSet<TEdge>());
            }

            foreach (TEdge edge in VisitedGraph.Edges)
            {
                verticesEdges[edge.Source].Add(edge);
                verticesEdges[edge.Target].Add(edge);
            }

            if (cancelManager.IsCancelling)
                return;

            TVertex lastVertex = visitedVertices.First();
            foreach (TEdge edge in verticesEdges[lastVertex])
            {
                if (!edges.Contains(edge))
                {
                    edges.Add(edge);
                    queue.Enqueue(edge);
                }
            }

            if (cancelManager.IsCancelling)
                return;

            while (edges.Count > 0 && visitedVertices.Count < VisitedGraph.VertexCount)
            {
                TEdge minEdge = queue.Dequeue();
                OnExamineEdge(minEdge);

                if (!sets.AreInSameSet(minEdge.Source, minEdge.Target))
                {
                    OnTreeEdge(minEdge);
                    sets.Union(minEdge.Source, minEdge.Target);

                    if (visitedVertices.Contains(minEdge.Source))
                    {
                        lastVertex = minEdge.Target;
                        visitedVertices.Add(minEdge.Target);
                    }
                    else
                    {
                        lastVertex = minEdge.Source;
                        visitedVertices.Add(minEdge.Source);
                    }

                    foreach (TEdge edge in verticesEdges[lastVertex])
                    {
                        if (edges.Contains(edge))
                            continue;

                        edges.Add(edge);
                        queue.Enqueue(edge);
                    }
                }
            }
        }

        #endregion
    }
}