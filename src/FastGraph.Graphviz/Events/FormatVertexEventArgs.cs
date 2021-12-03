﻿#if SUPPORTS_SERIALIZATION
using System;
#endif
using System.Diagnostics;
using JetBrains.Annotations;
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
    {
        /// <summary />
        internal FormatVertexEventArgs([NotNull] TVertex vertex, [NotNull] GraphvizVertex vertexFormat)
            : base(vertex)
        {
            Debug.Assert(vertexFormat != null);

            VertexFormat = vertexFormat;
        }

        /// <summary>
        /// Vertex format.
        /// </summary>
        [NotNull]
        public GraphvizVertex VertexFormat { get; }
    }
}
