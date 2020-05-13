#if SUPPORTS_SERIALIZATION
using System;
#endif
using System.Diagnostics;
using JetBrains.Annotations;
using QuikGraph;
using QuickGraph.Graphviz.Dot;

namespace QuickGraph.Graphviz
{
    /// <summary>
    /// Arguments of an event related to the formatting of an edge.
    /// </summary>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class FormatEdgeEventArgs<TVertex, TEdge> : EdgeEventArgs<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary />
        internal FormatEdgeEventArgs([NotNull] TEdge edge, [NotNull] GraphvizEdge edgeFormat)
            : base(edge)
        {
            Debug.Assert(edgeFormat != null);

            EdgeFormat = edgeFormat;
        }

        /// <summary>
        /// Edge format.
        /// </summary>
        public GraphvizEdge EdgeFormat { get; }
    }
}