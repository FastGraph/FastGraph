#if SUPPORTS_SERIALIZATION
using System;
using System.Runtime.Serialization;
#endif

namespace QuikGraph
{
    /// <summary>
    /// Exception raised when an algorithm could not find a path in a graph.
    /// </summary>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public class NoPathFoundException : QuikGraphException
    {
        /// <summary>
        /// Initializes a new instance of <see cref="NoPathFoundException"/> class.
        /// </summary>
        public NoPathFoundException()
            : base("No path found to join vertices in the graph.")
        {
        }

#if SUPPORTS_SERIALIZATION
        /// <summary>
        /// Initializes a new instance of <see cref="NoPathFoundException"/> with serialized data.
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/> that contains serialized data
        /// concerning the thrown exception.</param>
        /// <param name="context"><see cref="StreamingContext"/> that contains contextual information.</param>
        protected NoPathFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}