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
    public sealed class FormatEdgeEventArgs<TVertex, TEdge> : EdgeEventArgs<TVertex, TEdge>
        where TVertex : notnull
        where TEdge : IEdge<TVertex>
    {
        /// <summary />
        internal FormatEdgeEventArgs(TEdge edge, GraphvizEdge edgeFormat)
            : base(edge)
        {
            EdgeFormat = edgeFormat;
        }

        /// <summary>
        /// Edge format.
        /// </summary>
        public GraphvizEdge EdgeFormat { get; }
    }
}
