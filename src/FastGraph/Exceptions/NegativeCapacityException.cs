using System;
#if SUPPORTS_SERIALIZATION
using System.Runtime.Serialization;
#endif
using JetBrains.Annotations;

namespace FastGraph
{
    /// <summary>
    /// Exception raised when an algorithm find a negative capacity in a graph.
    /// </summary>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public class NegativeCapacityException : FastGraphException
    {
        /// <summary>
        /// Initializes a new instance of <see cref="NegativeCapacityException"/> class.
        /// </summary>
        public NegativeCapacityException()
            : base("The graph contains at least one negative capacity.")
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="NegativeCapacityException"/> class.
        /// </summary>
        public NegativeCapacityException([NotNull] string message, [CanBeNull] Exception innerException = null)
            : base(message, innerException)
        {
        }

#if SUPPORTS_SERIALIZATION
        /// <summary>
        /// Constructor used during runtime serialization.
        /// </summary>
        protected NegativeCapacityException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}
