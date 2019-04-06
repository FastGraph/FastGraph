using System;
using System.Collections.Generic;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif

namespace QuickGraph.Predicates
{
#if !SILVERLIGHT
    [Serializable]
#endif
    public class FilteredBidirectionalGraph<TVertex, TEdge, TGraph> 
        : FilteredVertexListGraph<TVertex, TEdge, TGraph>
        , IBidirectionalGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
        where TGraph : IBidirectionalGraph<TVertex, TEdge>
    {
        public FilteredBidirectionalGraph(
            TGraph baseGraph,
            VertexPredicate<TVertex> vertexPredicate,
            EdgePredicate<TVertex, TEdge> edgePredicate
            )
            :base(baseGraph,vertexPredicate,edgePredicate)
        { }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public bool IsInEdgesEmpty(TVertex v)
        {
            return this.InDegree(v) == 0;
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public int InDegree(TVertex v)
        {
            int count = 0;
            foreach (var edge in this.BaseGraph.InEdges(v))
                if (this.TestEdge(edge))
                    count++;
            return count;
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public IEnumerable<TEdge> InEdges(TVertex v)
        {
            foreach (var edge in this.BaseGraph.InEdges(v))
                if (this.TestEdge(edge))
                    yield return edge;
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public bool TryGetInEdges(TVertex v, out IEnumerable<TEdge> edges)
        {
            if (this.ContainsVertex(v))
            {
                edges = this.InEdges(v);
                return true;
            }
            else
            {
                edges = null;
                return false;
            }
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public int Degree(TVertex v)
        {
            return this.OutDegree(v) + this.InDegree(v);
        }

        public bool IsEdgesEmpty
        {
            get
            {
                foreach (var edge in this.BaseGraph.Edges)
                    if (TestEdge(edge))
                        return false;
                return true;
            }
        }

        public int EdgeCount
        {
            get
            {
                int count = 0;
                foreach (var edge in this.BaseGraph.Edges)
                    if (TestEdge(edge))
                        count++;
                return count;
            }
        }

        public IEnumerable<TEdge> Edges
        {
            get
            {
                foreach (var edge in this.BaseGraph.Edges)
                    if (TestEdge(edge))
                        yield return edge;
            }
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public bool ContainsEdge(TEdge edge)
        {
            if (!this.TestEdge(edge))
                return false;
            return this.BaseGraph.ContainsEdge(edge);
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public TEdge InEdge(TVertex v, int index)
        {
            throw new NotSupportedException();
        }
    }
}
