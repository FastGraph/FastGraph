namespace QuikGraph.Serialization
{
    /// <summary>
    /// Base class for a serializer.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public abstract class SerializerBase<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Gets or sets the flag indicating if the document header should be serialized.
        /// </summary>
        public bool EmitDocumentDeclaration { get; set; }
    }
}
