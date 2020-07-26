using System;
using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// Base class for arguments of an event related to a vertex.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public class VertexEventArgs<TVertex> : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VertexEventArgs{TVertex}"/> class.
        /// </summary>
        /// <param name="vertex">Concerned vertex.</param>
        public VertexEventArgs([NotNull] TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            Vertex = vertex;
        }

        /// <summary>
        /// Vertex concerned by the event.
        /// </summary>
        [NotNull]
        public TVertex Vertex { get; }
    }
}