#if SUPPORTS_SERIALIZATION || SUPPORTS_CLONEABLE
using System;
#endif
using System.Collections.Generic;
#if SUPPORTS_SERIALIZATION
using System.Runtime.Serialization;
#endif

namespace QuickGraph.Collections
{
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public class EdgeEdgeDictionary<TVertex, TEdge>
        : Dictionary<TEdge, TEdge>
#if SUPPORTS_CLONEABLE
        , ICloneable
#endif
#if SUPPORTS_SERIALIZATION
        , ISerializable
#endif
        where TEdge : IEdge<TVertex>
    {
        public EdgeEdgeDictionary()
        {
        }

        public EdgeEdgeDictionary(int capacity)
            : base(capacity)
        {
        }

#if SUPPORTS_SERIALIZATION
        protected EdgeEdgeDictionary(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif

        public EdgeEdgeDictionary<TVertex, TEdge> Clone()
        {
            var clone = new EdgeEdgeDictionary<TVertex, TEdge>(this.Count);
            foreach (var kv in this)
                clone.Add(kv.Key, kv.Value);
            return clone;
        }

#if SUPPORTS_CLONEABLE
        object ICloneable.Clone()
        {
            return this.Clone();
        }
#endif
    }
}
