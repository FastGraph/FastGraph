#if SUPPORTS_SERIALIZATION || SUPPORTS_CLONEABLE
using System;
#endif
using System.Collections.Generic;
#if SUPPORTS_SERIALIZATION
using System.Runtime.Serialization;
#endif
using JetBrains.Annotations;

namespace QuikGraph.Collections
{
    /// <summary>
    /// Stores association of vertices to edges.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class EdgeEdgeDictionary<TVertex, TEdge> : Dictionary<TEdge, TEdge>
#if SUPPORTS_CLONEABLE
        , ICloneable
#endif
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EdgeEdgeDictionary{TVertex,TEdge}"/> class.
        /// </summary>
        public EdgeEdgeDictionary()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EdgeEdgeDictionary{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="capacity">Dictionary capacity.</param>
        public EdgeEdgeDictionary(int capacity)
            : base(capacity)
        {
        }

#if SUPPORTS_SERIALIZATION
        /// <summary>
        /// Initializes a new instance of <see cref="EdgeEdgeDictionary{TVertex,TEdge}"/> with serialized data.
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/> that contains serialized data.</param>
        /// <param name="context"><see cref="StreamingContext"/> that contains contextual information.</param>
        private EdgeEdgeDictionary(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif

        /// <summary>
        /// Clones this vertices/edges dictionary.
        /// </summary>
        /// <returns>Cloned dictionary.</returns>
        [NotNull]
        public EdgeEdgeDictionary<TVertex, TEdge> Clone()
        {
            var clone = new EdgeEdgeDictionary<TVertex, TEdge>(Count);
            foreach (KeyValuePair<TEdge, TEdge> pair in this)
                clone.Add(pair.Key, pair.Value);
            return clone;
        }

#if SUPPORTS_CLONEABLE
        /// <inheritdoc />
        object ICloneable.Clone()
        {
            return Clone();
        }
#endif
    }
}