using System;
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
        /// Initializes a new instance of <see cref="ParallelEdgeNotAllowedException"/>.
        /// </summary>
        public ParallelEdgeNotAllowedException()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ParallelEdgeNotAllowedException"/> with the given message.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public ParallelEdgeNotAllowedException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ParallelEdgeNotAllowedException"/> with the given message
        /// and a reference to exception that triggers this one.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Exception that triggered this exception.</param>
        public ParallelEdgeNotAllowedException(string message, Exception innerException) 
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
