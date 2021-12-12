#nullable enable

#if SUPPORTS_SERIALIZATION
using System.Runtime.Serialization;
#endif

namespace FastGraph
{
    /// <summary>
    /// Exception raised when an algorithm detected a cyclic graph when required acyclic.
    /// </summary>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public class NonAcyclicGraphException : FastGraphException
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
        public NonAcyclicGraphException(string message, Exception? innerException = default)
            : base(message, innerException)
        {
        }

#if SUPPORTS_SERIALIZATION
        /// <summary>
        /// Constructor used during runtime serialization.
        /// </summary>
        protected NonAcyclicGraphException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}
