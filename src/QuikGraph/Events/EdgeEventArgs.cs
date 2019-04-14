using System;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// Base class for arguments of an event related to an edge.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public class EdgeEventArgs<TVertex, TEdge> : EventArgs
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EdgeEventArgs{TVertex, TEdge}"/> class.
        /// </summary>
        /// <param name="edge">The edge.</param>
        public EdgeEventArgs([NotNull] TEdge edge)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(edge != null);
#endif

            Edge = edge;
        }

        /// <summary>
        /// Edge concerned by the event.
        /// </summary>
        [NotNull]
        public TEdge Edge { get; }
    }
}
