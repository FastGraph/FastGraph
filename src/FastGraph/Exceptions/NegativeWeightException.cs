#nullable enable

#if SUPPORTS_SERIALIZATION
using System.Runtime.Serialization;
#endif

namespace FastGraph
{
    /// <summary>
    /// Exception raised when an algorithm find or computed a negative weight in a graph.
    /// </summary>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public class NegativeWeightException : FastGraphException
    {
        /// <summary>
        /// Initializes a new instance of <see cref="NegativeWeightException"/> class.
        /// </summary>
        public NegativeWeightException()
            : base("The graph contains at least one negative weight.")
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="NegativeWeightException"/> class.
        /// </summary>
        public NegativeWeightException(string message, Exception? innerException = default)
            : base(message, innerException)
        {
        }

#if SUPPORTS_SERIALIZATION
        /// <summary>
        /// Constructor used during runtime serialization.
        /// </summary>
        protected NegativeWeightException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}
