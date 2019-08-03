using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// Implementation for a bidirectional undirected graph.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    [DebuggerDisplay("VertexCount = {" + nameof(VertexCount) + "}, EdgeCount = {" + nameof(EdgeCount) + "}")]
    public sealed class UndirectedBidirectionalGraph<TVertex, TEdge> : IUndirectedGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UndirectedBidirectionalGraph{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Bidirectional graph.</param>
        public UndirectedBidirectionalGraph([NotNull] IBidirectionalGraph<TVertex, TEdge> visitedGraph)
        {
            if (visitedGraph is null)
                throw new ArgumentNullException(nameof(visitedGraph));

            VisitedGraph = visitedGraph;
        }

        /// <inheritdoc />
        public EdgeEqualityComparer<TVertex> EdgeEqualityComparer { get; } =
            EdgeExtensions.GetUndirectedVertexEquality<TVertex, TEdge>();

        /// <summary>
        /// Underlying bidirectional graph.
        /// </summary>
        public IBidirectionalGraph<TVertex, TEdge> VisitedGraph { get; }

        #region IGraph<TVertex,TEdge>

        /// <inheritdoc />
        public bool IsDirected => false;

        /// <inheritdoc />
        public bool AllowParallelEdges => VisitedGraph.AllowParallelEdges;

        #endregion

        #region IVertexSet<TVertex,TEdge>

        /// <inheritdoc />
        public bool IsVerticesEmpty => VisitedGraph.IsVerticesEmpty;

        /// <inheritdoc />
        public int VertexCount => VisitedGraph.VertexCount;

        /// <inheritdoc />
        public IEnumerable<TVertex> Vertices => VisitedGraph.Vertices;

        /// <inheritdoc />
        public bool ContainsVertex(TVertex vertex)
        {
            return VisitedGraph.ContainsVertex(vertex);
        }

        #endregion

        #region IEdgeSet<TVertex,TEdge>

        /// <inheritdoc />
        public bool IsEdgesEmpty => VisitedGraph.IsEdgesEmpty;

        /// <inheritdoc />
        public int EdgeCount => VisitedGraph.EdgeCount;

        /// <inheritdoc />
        public IEnumerable<TEdge> Edges => VisitedGraph.Edges;

        /// <inheritdoc />
        public bool ContainsEdge(TEdge edge)
        {
            return VisitedGraph.ContainsEdge(edge);
        }

        #endregion
        
        #region IUndirectedGraph<TVertex,TEdge>

        /// <inheritdoc />
        public IEnumerable<TEdge> AdjacentEdges(TVertex vertex)
        {
            return
                VisitedGraph.OutEdges(vertex)
                    .Concat(
                        VisitedGraph.InEdges(vertex)
                            // We skip self edges here since
                            // We already did those in the out-edge run
                            .Where(inEdge => !inEdge.IsSelfEdge()));
        }

        /// <inheritdoc />
        public int AdjacentDegree(TVertex vertex)
        {
            return VisitedGraph.Degree(vertex);
        }

        /// <inheritdoc />
        public bool IsAdjacentEdgesEmpty(TVertex vertex)
        {
            return VisitedGraph.IsOutEdgesEmpty(vertex) && VisitedGraph.IsInEdgesEmpty(vertex);
        }

        /// <summary>
        /// <see cref="AdjacentEdge"/> is not supported for this kind of graph.
        /// </summary>
        public TEdge AdjacentEdge(TVertex vertex, int index)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public bool ContainsEdge(TVertex source, TVertex target)
        {
            return TryGetEdge(source, target, out _);
        }

        /// <inheritdoc />
        public bool TryGetEdge(TVertex source, TVertex target, out TEdge edge)
        {
            foreach (var adjacentEdge in AdjacentEdges(source))
            {
                if (EdgeEqualityComparer(adjacentEdge, source, target))
                {
                    edge = adjacentEdge;
                    return true;
                }
            }

            edge = default(TEdge);
            return false;
        }

        #endregion
    }
}
