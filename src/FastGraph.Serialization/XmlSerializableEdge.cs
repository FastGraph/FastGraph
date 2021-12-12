#nullable enable

#if SUPPORTS_SERIALIZATION
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
        where TVertex : notnull
    {
        /// <inheritdoc />
        [XmlElement]
        // ReSharper disable once NotNullMemberIsNotInitialized, Justification: Because it's a serializable structure.
        public TVertex Source { get; set; } = default!;

        /// <inheritdoc />
        [XmlElement]
        // ReSharper disable once NotNullMemberIsNotInitialized, Justification: Because it's a serializable structure.
        public TVertex Target { get; set; } = default!;
    }
}
