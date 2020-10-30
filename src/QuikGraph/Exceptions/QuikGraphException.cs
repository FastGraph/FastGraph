using System;
#if SUPPORTS_SERIALIZATION
using System.Runtime.Serialization;
#endif
using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// QuikGraph base exception.
    /// </summary>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public abstract class QuikGraphException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="QuikGraphException"/> with the given message.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        protected QuikGraphException([NotNull] string message, [CanBeNull] Exception innerException = null)
            : base(message, innerException)
        {
        }

#if SUPPORTS_SERIALIZATION
        /// <summary>
        /// Initializes a new instance of <see cref="QuikGraphException"/> with serialized data.
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/> that contains serialized data
        /// concerning the thrown exception.</param>
        /// <param name="context"><see cref="StreamingContext"/> that contains contextual information.</param>
        protected QuikGraphException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}