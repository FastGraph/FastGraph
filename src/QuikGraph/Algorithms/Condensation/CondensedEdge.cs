#if SUPPORTS_SERIALIZATION
using System;
#endif
using System.Collections.Generic;

namespace QuikGraph.Algorithms.Condensation
{
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class CondensedEdge<TVertex, TEdge, TGraph> : Edge<TGraph>
        where TEdge : IEdge<TVertex>
        where TGraph : IMutableVertexAndEdgeSet<TVertex, TEdge>, new()
    {
        private List<TEdge> edges = new List<TEdge>();
        public CondensedEdge(TGraph source, TGraph target)
            :base(source,target)
        { }

        public IList<TEdge> Edges
        {
            get { return this.edges; }
        }
    }
}
