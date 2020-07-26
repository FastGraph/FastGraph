namespace QuikGraph.Serialization
{
    /// <summary>
    /// Base class for a serializer.
    /// </summary>
    public abstract class SerializerBase
    {
        /// <summary>
        /// Gets or sets the flag indicating if the document header should be serialized.
        /// </summary>
        public bool EmitDocumentDeclaration { get; set; }
    }
}