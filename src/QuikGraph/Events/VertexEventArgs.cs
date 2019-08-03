using System;
using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// Base class for arguments of an event related to a vertex.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    public class VertexEventArgs<TVertex> : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VertexEventArgs{TVertex}"/> class.
        /// </summary>
        /// <param name="vertex">Concerned vertex.</param>
        public VertexEventArgs([NotNull] TVertex vertex)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(vertex != null);
#endif

            Vertex = vertex;
        }

        /// <summary>
        /// Vertex concerned by the event.
        /// </summary>
        [NotNull]
        public TVertex Vertex { get; }
    }
}
