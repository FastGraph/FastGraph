#nullable enable

#if SUPPORTS_SERIALIZATION
#endif
using FastGraph.Graphviz.Dot;

namespace FastGraph.Graphviz
{
    /// <summary>
    /// Arguments of an event related to the formatting of an edge.
    /// </summary>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class FormatVertexEventArgs<TVertex> : VertexEventArgs<TVertex>
        where TVertex : notnull
    {
        /// <summary />
        internal FormatVertexEventArgs(TVertex vertex, GraphvizVertex vertexFormat)
            : base(vertex)
        {
            VertexFormat = vertexFormat;
        }

        /// <summary>
        /// Vertex format.
        /// </summary>
        public GraphvizVertex VertexFormat { get; }
    }
}
