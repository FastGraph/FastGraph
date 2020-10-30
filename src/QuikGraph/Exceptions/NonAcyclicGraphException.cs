using System;
#if SUPPORTS_SERIALIZATION
using System.Runtime.Serialization;
#endif
using JetBrains.Annotations;

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
        /// Initializes a new instance of <see cref="NonAcyclicGraphException"/> class.
        /// </summary>
        public NonAcyclicGraphException()
            : base("The graph contains at least one cycle.")
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="NonAcyclicGraphException"/> class.
        /// </summary>
        public NonAcyclicGraphException([NotNull] string message, [CanBeNull] Exception innerException = null)
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