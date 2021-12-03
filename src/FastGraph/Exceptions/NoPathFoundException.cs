using System;
#if SUPPORTS_SERIALIZATION
using System.Runtime.Serialization;
#endif
using JetBrains.Annotations;

namespace FastGraph
{
    /// <summary>
    /// Exception raised when an algorithm could not find a path in a graph.
    /// </summary>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public class NoPathFoundException : FastGraphException
    {
        /// <summary>
        /// Initializes a new instance of <see cref="NoPathFoundException"/> class.
        /// </summary>
        public NoPathFoundException()
            : base("No path found to join vertices in the graph.")
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="NoPathFoundException"/> class.
        /// </summary>
        public NoPathFoundException([NotNull] string message, [CanBeNull] Exception innerException = null)
            : base(message, innerException)
        {
        }

#if SUPPORTS_SERIALIZATION
        /// <summary>
        /// Constructor used during runtime serialization.
        /// </summary>
        protected NoPathFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}
