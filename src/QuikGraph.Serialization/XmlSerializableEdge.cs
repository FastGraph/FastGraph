#if SUPPORTS_SERIALIZATION
using System;
#endif
using System.Xml.Serialization;

namespace FastGraph.Serialization
{
    /// <summary>
    /// XML serializable edge class.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public class XmlSerializableEdge<TVertex> : IEdge<TVertex>
    {
        /// <inheritdoc />
        [XmlElement]
        // ReSharper disable once NotNullMemberIsNotInitialized, Justification: Because it's a serializable structure.
        public TVertex Source { get; set; }

        /// <inheritdoc />
        [XmlElement]
        // ReSharper disable once NotNullMemberIsNotInitialized, Justification: Because it's a serializable structure.
        public TVertex Target { get; set; }
    }
}