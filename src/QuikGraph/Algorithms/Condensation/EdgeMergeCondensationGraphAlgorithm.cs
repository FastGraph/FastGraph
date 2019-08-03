using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace QuikGraph.Algorithms.Condensation
{
    /// <summary>
    /// Algorithm that condensate edges of a graph.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public sealed class EdgeMergeCondensationGraphAlgorithm<TVertex, TEdge> : AlgorithmBase<IBidirectionalGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EdgeMergeCondensationGraphAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="condensedGraph">Graph that will contains the condensation of the <paramref name="visitedGraph"/>.</param>
        /// <param name="vertexPredicate">Vertex predicate used to filter the vertices to put in the condensed graph.</param>
        public EdgeMergeCondensationGraphAlgorithm(
            [NotNull] IBidirectionalGraph<TVertex, TEdge> visitedGraph,
            [NotNull] IMutableBidirectionalGraph<TVertex, MergedEdge<TVertex, TEdge>> condensedGraph,
            [NotNull] VertexPredicate<TVertex> vertexPredicate)
            : base(visitedGraph)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(condensedGraph != null);
            Contract.Requires(vertexPredicate != null);
#endif

            CondensedGraph = condensedGraph;
            VertexPredicate = vertexPredicate;
        }

        /// <summary>
        /// Condensed graph.
        /// </summary>
        [NotNull]
        public IMutableBidirectionalGraph<TVertex, MergedEdge<TVertex, TEdge>> CondensedGraph { get; }

        /// <summary>
        /// Vertex predicate used to filter the vertices to put in the condensed graph.
        /// </summary>
        [NotNull]
        public VertexPredicate<TVertex> VertexPredicate { get; }

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            // Adding vertices to the new graph
            // and pushing filtered vertices in queue
            var filteredVertices = new Queue<TVertex>();
            foreach (TVertex vertex in VisitedGraph.Vertices)
            {
                CondensedGraph.AddVertex(vertex);
                if (!VertexPredicate(vertex))
                    filteredVertices.Enqueue(vertex);
            }

            // Adding all edges
            foreach (TEdge edge in VisitedGraph.Edges)
            {
                var mergedEdge = new MergedEdge<TVertex, TEdge>(edge.Source, edge.Target);
                mergedEdge.Edges.Add(edge);

                CondensedGraph.AddEdge(mergedEdge);
            }

            // Remove vertices
            while (filteredVertices.Count > 0)
            {
                TVertex filteredVertex = filteredVertices.Dequeue();

                // Do the cross product between in-edges and out-edges
                MergeVertex(filteredVertex);
            }
        }

        #endregion

        private void MergeVertex([NotNull] TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            // Get in-edges and out-edges
            var inEdges = new List<MergedEdge<TVertex, TEdge>>(CondensedGraph.InEdges(vertex));
            var outEdges = new List<MergedEdge<TVertex, TEdge>>(CondensedGraph.OutEdges(vertex));

            // Remove vertex
            CondensedGraph.RemoveVertex(vertex);

            // Add condensed edges
            foreach (MergedEdge<TVertex, TEdge> inEdge in inEdges)
            {
                if (inEdge.Source.Equals(vertex))
                    continue;

                foreach (MergedEdge<TVertex, TEdge> outEdge in outEdges)
                {
                    if (outEdge.Target.Equals(vertex))
                        continue;

                    var newEdge = MergedEdge<TVertex, TEdge>.Merge(inEdge, outEdge);
                    CondensedGraph.AddEdge(newEdge);
                }
            }
        }
    }
}
