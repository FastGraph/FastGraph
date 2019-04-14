#if SUPPORTS_SERIALIZATION || SUPPORTS_CLONEABLE
using System;
#endif
using System.Collections.Generic;

namespace QuikGraph.Collections
{
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class VertexList<TVertex> : List<TVertex>
#if SUPPORTS_CLONEABLE
        , ICloneable
#endif
    {
        public VertexList()
        {
        }

        public VertexList(int capacity)
            : base(capacity)
        {
        }

        public VertexList(VertexList<TVertex> other)
            : base(other)
        {
        }

        public VertexList<TVertex> Clone()
        {
            return new VertexList<TVertex>(this);
        }

#if SUPPORTS_CLONEABLE
        object ICloneable.Clone()
        {
            return this.Clone();
        }
#endif
    }
}
