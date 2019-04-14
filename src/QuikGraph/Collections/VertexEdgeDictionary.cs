#if SUPPORTS_SERIALIZATION || SUPPORTS_CLONEABLE
using System;
#endif
using System.Collections.Generic;
#if SUPPORTS_SERIALIZATION
using System.Runtime.Serialization;
#endif

namespace QuikGraph.Collections
{
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class VertexEdgeDictionary<TVertex,TEdge>
        : Dictionary<TVertex, IEdgeList<TVertex, TEdge>>
        , IVertexEdgeDictionary<TVertex, TEdge>
#if SUPPORTS_CLONEABLE
        , ICloneable
#endif
#if SUPPORTS_SERIALIZATION
        , ISerializable
#endif
        where TEdge : IEdge<TVertex>
    {
        public VertexEdgeDictionary()
        {
        }

        public VertexEdgeDictionary(int capacity)
            : base(capacity)
        {
        }

#if SUPPORTS_SERIALIZATION
        public VertexEdgeDictionary(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif

        public VertexEdgeDictionary<TVertex, TEdge> Clone()
        {
            var clone = new VertexEdgeDictionary<TVertex, TEdge>(this.Count);
            foreach (var kv in this)
                clone.Add(kv.Key, kv.Value.Clone());
            return clone;
        }

        IVertexEdgeDictionary<TVertex, TEdge> IVertexEdgeDictionary<TVertex,TEdge>.Clone()
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
