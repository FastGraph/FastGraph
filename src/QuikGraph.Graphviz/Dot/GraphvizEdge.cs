using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using static QuikGraph.Utils.MathUtils;

namespace QuikGraph.Graphviz.Dot
{
    /// <summary>
    /// GraphViz edge.
    /// </summary>
    public class GraphvizEdge
    {
        /// <summary>
        /// Comment.
        /// <see href="https://www.graphviz.org/doc/info/attrs.html#d:comment">See more</see>
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Label.
        /// </summary>
        public GraphvizEdgeLabel Label { get; set; } = new GraphvizEdgeLabel();

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
        /// Direction.
        /// <see href="https://www.graphviz.org/doc/info/attrs.html#d:dir">See more</see>
        /// </summary>
        public GraphvizEdgeDirection Direction { get; set; } = GraphvizEdgeDirection.Forward;

        /// <summary>
        /// Font.
        /// <see href="https://www.graphviz.org/doc/info/attrs.html#d:fontname">See more</see> or
        /// <see href="https://www.graphviz.org/doc/info/attrs.html#d:fontsize">See more</see>
        /// </summary>
        public GraphvizFont Font { get; set; }

        /// <summary>
        /// Font color.
        /// <see href="https://www.graphviz.org/doc/info/attrs.html#d:fontcolor">See more</see>
        /// </summary>
        public GraphvizColor FontColor { get; set; } = GraphvizColor.Black;

        /// <summary>
        /// Edge head.
        /// </summary>
        public GraphvizEdgeExtremity Head { get; set; } = new GraphvizEdgeExtremity(true);

        /// <summary>
        /// Edge arrow.
        /// <see href="https://www.graphviz.org/doc/info/attrs.html#d:arrowhead">See more</see>
        /// </summary>
        public GraphvizArrow HeadArrow { get; set; }

        /// <summary>
        /// Head port.
        /// <see href="https://www.graphviz.org/doc/info/attrs.html#d:headport">See more</see>
        /// </summary>
        public string HeadPort { get; set; }

        /// <summary>
        /// Tail.
        /// </summary>
        public GraphvizEdgeExtremity Tail { get; set; } = new GraphvizEdgeExtremity(false);

        /// <summary>
        /// Tail arrow.
        /// <see href="https://www.graphviz.org/doc/info/attrs.html#d:arrowtail">See more</see>
        /// </summary>
        public GraphvizArrow TailArrow { get; set; }

        /// <summary>
        /// Tail port.
        /// <see href="https://www.graphviz.org/doc/info/attrs.html#d:tailport">See more</see>
        /// </summary>
        public string TailPort { get; set; }

        /// <summary>
        /// Indicates if edge is constrained.
        /// <see href="https://www.graphviz.org/doc/info/attrs.html#d:constraint">See more</see>
        /// </summary>
        public bool IsConstrained { get; set; } = true;

        /// <summary>
        /// Indicates if edge is decorated.
        /// <see href="https://www.graphviz.org/doc/info/attrs.html#d:decorate">See more</see>
        /// </summary>
        public bool IsDecorated { get; set; }

        /// <summary>
        /// Layer.
        /// <see href="https://www.graphviz.org/doc/info/attrs.html#d:layer">See more</see>
        /// </summary>
        public GraphvizLayer Layer { get; set; }

        /// <summary>
        /// Stroke color.
        /// <see href="https://www.graphviz.org/doc/info/attrs.html#d:color">See more</see>
        /// </summary>
        public GraphvizColor StrokeColor { get; set; } = GraphvizColor.Black;

        /// <summary>
        /// Edge style.
        /// <see href="https://www.graphviz.org/doc/info/attrs.html#d:style">See more</see>
        /// </summary>
        public GraphvizEdgeStyle Style { get; set; } = GraphvizEdgeStyle.Unspecified;

        /// <summary>
        /// Weight.
        /// <see href="https://www.graphviz.org/doc/info/attrs.html#d:weight">See more</see>
        /// </summary>
        public double Weight { get; set; } = 1;

        /// <summary>
        /// Length.
        /// <see href="https://www.graphviz.org/doc/info/attrs.html#d:len">See more</see>
        /// </summary>
        public int Length { get; set; } = 1;

        /// <summary>
        /// Minimal length.
        /// <see href="https://www.graphviz.org/doc/info/attrs.html#d:minlen">See more</see>
        /// </summary>
        public int MinLength { get; set; } = 1;

        [Pure]
        [NotNull]
        internal string GenerateDot([NotNull] Dictionary<string, object> properties)
        {
            using (var writer = new StringWriter())
            {
                bool flag = false;
                foreach (KeyValuePair<string, object> pair in properties)
                {
                    if (flag)
                    {
                        writer.Write(", ");
                    }
                    else
                    {
                        flag = true;
                    }

                    switch (pair.Value)
                    {
                        case string strValue:
                            writer.Write($"{pair.Key}=\"{strValue}\"");
                            continue;

                        case GraphvizEdgeDirection direction:
                            writer.Write($"{pair.Key}={direction.ToString().ToLower()}");
                            continue;

                        case GraphvizEdgeStyle edgeStyle:
                            writer.Write($"{pair.Key}={edgeStyle.ToString().ToLower()}");
                            continue;

                        case GraphvizColor color:
                            writer.Write(
                                "{0}=\"#{1}{2}{3}{4}\"",
                                pair.Key,
                                color.R.ToString("x2").ToUpper(),
                                color.G.ToString("x2").ToUpper(),
                                color.B.ToString("x2").ToUpper(),
                                color.A.ToString("x2").ToUpper());
                            continue;

                        default:
                            writer.Write($" {pair.Key}={pair.Value.ToString().ToLower()}");
                            break;
                    }
                }

                return writer.ToString();
            }
        }

        /// <summary>
        /// Converts this edge to DOT.
        /// </summary>
        /// <returns>Edge as DOT.</returns>
        [Pure]
        [NotNull]
        public string ToDot()
        {
            var properties = new Dictionary<string, object>();
            if (Comment != null)
            {
                properties["comment"] = Comment;
            }
            if (Direction != GraphvizEdgeDirection.Forward)
            {
                properties["dir"] = Direction.ToString().ToLower();
            }
            if (Font != null)
            {
                properties["fontname"] = Font.Name;
                properties["fontsize"] = Font.SizeInPoints;
            }
            if (FontColor != GraphvizColor.Black)
            {
                properties["fontcolor"] = FontColor;
            }
            Head.AddParameters(properties);
            if (HeadArrow != null)
            {
                properties["arrowhead"] = HeadArrow.ToDot();
            }
            if (!IsConstrained)
            {
                properties["constraint"] = IsConstrained;
            }
            if (IsDecorated)
            {
                properties["decorate"] = IsDecorated;
            }
            Label.AddParameters(properties);
            if (Layer != null)
            {
                properties["layer"] = Layer.Name;
            }
            if (MinLength != 1)
            {
                properties["minlen"] = MinLength;
            }
            if (StrokeColor != GraphvizColor.Black)
            {
                properties["color"] = StrokeColor;
            }
            if (Style != GraphvizEdgeStyle.Unspecified)
            {
                properties["style"] = Style.ToString().ToLower();
            }
            Tail.AddParameters(properties);
            if (TailArrow != null)
            {
                properties["arrowtail"] = TailArrow.ToDot();
            }
            if (ToolTip != null)
            {
                properties["tooltip"] = ToolTip;
            }
            if (Url != null)
            {
                properties["URL"] = Url;
            }
            if (!NearEqual(Weight, 1.0))
            {
                properties["weight"] = Weight;
            }
            if (HeadPort != null)
            {
                properties["headport"] = HeadPort;
            }
            if (TailPort != null)
            {
                properties["tailport"] = TailPort;
            }
            if (Length != 1)
            {
                properties["len"] = Length;
            }
            return GenerateDot(properties);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return ToDot();
        }
    }
}