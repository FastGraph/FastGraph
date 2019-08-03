#if SUPPORTS_CLONEABLE
using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using QuikGraph.Collections;

namespace QuikGraph.Algorithms.Exploration
{
    /// <summary>
    /// Implementation for a graph data structure that support growth
    /// by transitions made by out edges of its vertices.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class TransitionFactoryImplicitGraph<TVertex, TEdge> : IImplicitGraph<TVertex, TEdge>
        where TVertex : ICloneable
        where TEdge : IEdge<TVertex>
    {
        [NotNull]
        private readonly VertexEdgeDictionary<TVertex, TEdge> _verticesEdgesCache =
            new VertexEdgeDictionary<TVertex, TEdge>();

        /// <summary>
        /// Transitions factories.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull, ItemNotNull]
        public IList<ITransitionFactory<TVertex, TEdge>> TransitionFactories { get; } =
            new List<ITransitionFactory<TVertex, TEdge>>();

        /// <summary>
        /// Predicate that a vertex must match to be the successor (target) of an edge.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        public VertexPredicate<TVertex> SuccessorVertexPredicate { get; set; } = vertex => true;

        /// <summary>
        /// Predicate that an edge must match to be the successor of a source vertex.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        public EdgePredicate<TVertex, TEdge> SuccessorEdgePredicate { get; set; } = edge => true;

        #region IGraph<TVertex,TEdge>

        /// <inheritdoc />
        public bool IsDirected => true;

        /// <inheritdoc />
        public bool AllowParallelEdges => true;

        #endregion

        /// <inheritdoc />
        public bool ContainsVertex(TVertex vertex)
        {
            return _verticesEdgesCache.ContainsKey(vertex);
        }

        #region IImplicitGraph<TVertex,TEdge>

        /// <inheritdoc />
        public bool IsOutEdgesEmpty(TVertex vertex)
        {
            return OutDegree(vertex) == 0;
        }

        /// <inheritdoc />
        public int OutDegree(TVertex vertex)
        {
            return OutEdges(vertex).Count();
        }

        /// <inheritdoc />
        public IEnumerable<TEdge> OutEdges(TVertex vertex)
        {
            if (!_verticesEdgesCache.TryGetValue(vertex, out IEdgeList<TVertex, TEdge> edges))
            {
                edges = new EdgeList<TVertex, TEdge>();
                foreach (ITransitionFactory<TVertex, TEdge> transitionFactory in TransitionFactories)
                {
                    if (!transitionFactory.IsValid(vertex))
                        continue;

                    foreach (TEdge edge in transitionFactory.Apply(vertex))
                    {
                        if (SuccessorVertexPredicate(edge.Target)
                            && SuccessorEdgePredicate(edge))
                        {
                            edges.Add(edge);
                        }
                    }
                }

                _verticesEdgesCache[vertex] = edges;
            }

            return edges;
        }

        /// <inheritdoc />
        public bool TryGetOutEdges(TVertex vertex, out IEnumerable<TEdge> edges)
        {
            TEdge[] outEdges = OutEdges(vertex).ToArray();
            edges = outEdges;
            return outEdges.Length > 0;
        }

        /// <inheritdoc />
        public TEdge OutEdge(TVertex vertex, int index)
        {
            int i = 0;
            foreach (TEdge edge in OutEdges(vertex))
            {
                if (i++ == index)
                    return edge;
            }

            throw new ArgumentOutOfRangeException(nameof(index));
        }

        #endregion
    }
}
#endif