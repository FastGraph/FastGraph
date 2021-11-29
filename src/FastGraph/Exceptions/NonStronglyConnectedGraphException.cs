using System;
#if SUPPORTS_SERIALIZATION
using System.Runtime.Serialization;
#endif
using JetBrains.Annotations;

namespace FastGraph
{
    /// <summary>
    /// Exception raised when an algorithm detected a non-strongly connected graph.
    /// </summary>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public class NonStronglyConnectedGraphException : FastGraphException
    {
        /// <summary>
        /// Initializes a new instance of <see cref="NonStronglyConnectedGraphException"/> class.
        /// </summary>
        public NonStronglyConnectedGraphException()
            : base("The graph is not strongly connected.")
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="NonStronglyConnectedGraphException"/> class.
        /// </summary>
        public NonStronglyConnectedGraphException([NotNull] string message, [CanBeNull] Exception innerException = null)
            : base(message, innerException)
        {
        }

#if SUPPORTS_SERIALIZATION
        /// <summary>
        /// Constructor used during runtime serialization.
        /// </summary>
        protected NonStronglyConnectedGraphException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}