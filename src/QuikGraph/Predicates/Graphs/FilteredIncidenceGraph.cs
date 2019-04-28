#if SUPPORTS_SERIALIZATION
using System;
#endif
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace QuikGraph.Predicates
{
    /// <summary>
    /// Represents an incidence graph that is filtered with a vertex and an edge predicate.
    /// This means only vertex and edge matching predicates are "accessible".
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    /// <typeparam name="TGraph">Graph type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public class FilteredIncidenceGraph<TVertex, TEdge, TGraph>
        : FilteredImplicitGraph<TVertex, TEdge, TGraph>
        , IIncidenceGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
        where TGraph : IIncidenceGraph<TVertex, TEdge>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilteredIncidenceGraph{TVertex,TEdge,TGraph}"/> class.
        /// </summary>
        /// <param name="baseGraph">Graph in which applying predicates.</param>
        /// <param name="vertexPredicate">Predicate to match vertex that should be taken into account.</param>
        /// <param name="edgePredicate">Predicate to match edge that should be taken into account.</param>
        public FilteredIncidenceGraph(
            [NotNull] TGraph baseGraph,
            [NotNull] VertexPredicate<TVertex> vertexPredicate,
            [NotNull] EdgePredicate<TVertex, TEdge> edgePredicate)
            : base(baseGraph, vertexPredicate, edgePredicate)
        {
        }

        /// <inheritdoc />
        public bool ContainsEdge(TVertex source, TVertex target)
        {
            if (!VertexPredicate(source))
                return false;
            if (!VertexPredicate(target))
                return false;

            return BaseGraph.OutEdges(source).FirstOrDefault(
                       edge => edge.Target.Equals(target) && EdgePredicate(edge)) != null;
        }

        /// <inheritdoc />
        public bool TryGetEdge(TVertex source, TVertex target, out TEdge edge)
        {
            if (VertexPredicate(source)
                && VertexPredicate(target)
                && BaseGraph.TryGetEdges(source, target, out IEnumerable<TEdge> unfilteredEdges))
            {
                foreach (TEdge unfilteredEdge in unfilteredEdges)
                {
                    if (EdgePredicate(unfilteredEdge))
                    {
                        edge = unfilteredEdge;
                        return true;
                    }
                }
            }

            edge = default(TEdge);
            return false;
        }

        /// <inheritdoc />
        public bool TryGetEdges(TVertex source, TVertex target, out IEnumerable<TEdge> edges)
        {
            edges = null;
            if (!VertexPredicate(source))
                return false;
            if (!VertexPredicate(target))
                return false;

            if (BaseGraph.TryGetEdges(source, target, out IEnumerable<TEdge> unfilteredEdges))
            {
                TEdge[] filteredEdges = unfilteredEdges.Where(edge => EdgePredicate(edge)).ToArray();
                edges = filteredEdges;
                return filteredEdges.Length > 0;
            }

            return false;
        }
    }
}
