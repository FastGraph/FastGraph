using System;
using System.Collections;
using JetBrains.Annotations;

namespace QuikGraph.Graphviz.Dot
{
    /// <summary>
    /// Graphviz edge extremity.
    /// </summary>
    public class GraphvizEdgeExtremity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphvizEdgeExtremity"/> class.
        /// </summary>
        /// <param name="isHead">Indicates if this edge extremity is the head.</param>
        public GraphvizEdgeExtremity(bool isHead)
        {
            IsHead = isHead;
            Url = null;
            IsClipped = true;
            Label = null;
            ToolTip = null;
            Logical = null;
            Same = null;
        }

        /// <summary>
        /// Indicates if this extremity is edge head.
        /// </summary>
        public bool IsHead { get; }

        /// <summary>
        /// Is clipped edge?
        /// </summary>
        public bool IsClipped { get; set; }

        /// <summary>
        /// Label.
        /// <see href="https://www.graphviz.org/doc/info/attrs.html#d:label">See more</see>
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Tooltip.
        /// <see href="https://www.graphviz.org/doc/info/attrs.html#d:tooltip">See more</see>
        /// </summary>
        public string ToolTip { get; set; }

        /// <summary>
        /// URL.
        /// <see href="https://www.graphviz.org/doc/info/attrs.html#d:URL">See more</see>
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Logical.
        /// </summary>
        public string Logical { get; set; }

        /// <summary>
        /// Same.
        /// </summary>
        public string Same { get; set; }

        /// <summary>
        /// Adds this edge extremity parameters to the given <paramref name="parameters"/> map.
        /// </summary>
        /// <param name="parameters">Parameter map to fill.</param>
        public void AddParameters([NotNull] IDictionary parameters)
        {
            if (parameters is null)
                throw new ArgumentNullException(nameof(parameters));

            string text = IsHead ? "head" : "tail";
            if (Url != null)
            {
                parameters.Add(text + "URL", Url);
            }
            if (!IsClipped)
            {
                parameters.Add(text + "clip", IsClipped);
            }
            if (Label != null)
            {
                parameters.Add(text + "label", Label);
            }
            if (ToolTip != null)
            {
                parameters.Add(text + "tooltip", ToolTip);
            }
            if (Logical != null)
            {
                parameters.Add("l" + text, Logical);
            }
            if (Same != null)
            {
                parameters.Add("same" + text, Same);
            }
        }
    }
}