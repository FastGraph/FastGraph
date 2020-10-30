using System;
using JetBrains.Annotations;
#if SUPPORTS_SERIALIZATION
using System.Runtime.Serialization;
#endif

namespace QuikGraph
{
    /// <summary>
    /// Exception raised when an algorithm detected a parallel edge that is not allowed.
    /// </summary>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public class ParallelEdgeNotAllowedException : QuikGraphException
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ParallelEdgeNotAllowedException"/> class.
        /// </summary>
        public ParallelEdgeNotAllowedException()
            : base("Parallel edges are not allowed in the graph.")
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ParallelEdgeNotAllowedException"/> class.
        /// </summary>
        public ParallelEdgeNotAllowedException([NotNull] string message, [CanBeNull] Exception innerException = null)
            : base(message, innerException)
        {
        }

#if SUPPORTS_SERIALIZATION
        /// <summary>
        /// Initializes a new instance of <see cref="ParallelEdgeNotAllowedException"/> with serialized data.
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/> that contains serialized data
        /// concerning the thrown exception.</param>
        /// <param name="context"><see cref="StreamingContext"/> that contains contextual information.</param>
        protected ParallelEdgeNotAllowedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}