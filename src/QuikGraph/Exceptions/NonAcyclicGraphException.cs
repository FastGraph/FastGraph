using System;
#if SUPPORTS_SERIALIZATION
using System.Runtime.Serialization;
#endif

namespace QuikGraph
{
    /// <summary>
    /// Exception raised when an algorithm detected a cyclic graph when required acyclic.
    /// </summary>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public class NonAcyclicGraphException : QuikGraphException
    {
        /// <summary>
        /// Initializes a new instance of <see cref="NonAcyclicGraphException"/>.
        /// </summary>
        public NonAcyclicGraphException()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="NonAcyclicGraphException"/> with the given message.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public NonAcyclicGraphException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="NonAcyclicGraphException"/> with the given message
        /// and a reference to exception that triggers this one.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Exception that triggered this exception.</param>
        public NonAcyclicGraphException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

#if SUPPORTS_SERIALIZATION
        /// <summary>
        /// Initializes a new instance of <see cref="NonAcyclicGraphException"/> with serialized data.
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/> that contains serialized data
        /// concerning the thrown exception.</param>
        /// <param name="context"><see cref="StreamingContext"/> that contains contextual information.</param>
        protected NonAcyclicGraphException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}


