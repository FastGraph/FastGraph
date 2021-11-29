using System;
#if SUPPORTS_SERIALIZATION
using System.Runtime.Serialization;
#endif
using JetBrains.Annotations;

namespace FastGraph
{
    /// <summary>
    /// Exception raised when trying to use a vertex that is not inside the manipulated graph.
    /// </summary>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public class VertexNotFoundException : FastGraphException
    {
        /// <summary>
        /// Initializes a new instance of <see cref="VertexNotFoundException"/> class.
        /// </summary>
        public VertexNotFoundException()
            : base("Vertex is not present in the graph.")
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="VertexNotFoundException"/> class.
        /// </summary>
        public VertexNotFoundException([NotNull] string message, [CanBeNull] Exception innerException = null)
            : base(message, innerException)
        {
        }

#if SUPPORTS_SERIALIZATION
        /// <summary>
        /// Constructor used during runtime serialization.
        /// </summary>
        protected VertexNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}