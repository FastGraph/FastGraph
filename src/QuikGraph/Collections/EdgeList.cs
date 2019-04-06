#if SUPPORTS_SERIALIZATION || SUPPORTS_CLONEABLE
using System;
#endif
using System.Collections.Generic;

namespace QuickGraph.Collections
{
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class EdgeList<TVertex, TEdge>
        : List<TEdge>
        , IEdgeList<TVertex, TEdge>
#if SUPPORTS_CLONEABLE
        , ICloneable
#endif
        where TEdge : IEdge<TVertex>
    {
        public EdgeList() 
        { }

        public EdgeList(int capacity)
            : base(capacity)
        { }

        public EdgeList(EdgeList<TVertex, TEdge> list)
            : base(list)
        {}

        public EdgeList<TVertex, TEdge> Clone()
        {
            return new EdgeList<TVertex, TEdge>(this);
        }

        IEdgeList<TVertex, TEdge> IEdgeList<TVertex,TEdge>.Clone()
        {
            return this.Clone();
        }

#if SUPPORTS_CLONEABLE
        object ICloneable.Clone()
        {
            return this.Clone();
        }
#endif
    }
}
