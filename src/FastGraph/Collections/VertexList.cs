#nullable enable

#if SUPPORTS_SERIALIZATION || SUPPORTS_CLONEABLE
#endif

namespace FastGraph.Collections
{
    /// <summary>
    /// Stores a list of vertices.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class VertexList<TVertex> : List<TVertex>
#if SUPPORTS_CLONEABLE
        , ICloneable
#endif
        where TVertex : notnull
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VertexList{TVertex}"/> class.
        /// </summary>
        public VertexList()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexList{TVertex}"/> class.
        /// </summary>
        /// <param name="capacity">List capacity.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="capacity"/> is negative.</exception>
        public VertexList(int capacity)
            : base(capacity)
        {
        }

        /// <inheritdoc />
        /// <exception cref="T:System.ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        public VertexList(VertexList<TVertex> other)
            : base(other)
        {
        }

        /// <summary>
        /// Clones this vertex list.
        /// </summary>
        /// <returns>Cloned list.</returns>
        public VertexList<TVertex> Clone()
        {
            return new VertexList<TVertex>(this);
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
