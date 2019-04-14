using System;
#if SUPPORTS_SERIALIZATION
using System.Runtime.Serialization;
#endif

namespace QuikGraph
{
    /// <summary>
    /// Exception raised when an algorithm detected a negative cycle in a graph.
    /// </summary>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public class NegativeCycleGraphException : QuikGraphException
    {
        /// <summary>
        /// Initializes a new instance of <see cref="NegativeCycleGraphException"/>.
        /// </summary>
        public NegativeCycleGraphException()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="NegativeCycleGraphException"/> with the given message.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public NegativeCycleGraphException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="NegativeCycleGraphException"/> with the given message
        /// and a reference to exception that triggers this one.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Exception that triggered this exception.</param>
        public NegativeCycleGraphException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

#if SUPPORTS_SERIALIZATION
        /// <summary>
        /// Initializes a new instance of <see cref="NegativeCycleGraphException"/> with serialized data.
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/> that contains serialized data
        /// concerning the thrown exception.</param>
        /// <param name="context"><see cref="StreamingContext"/> that contains contextual information.</param>
        protected NegativeCycleGraphException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}
