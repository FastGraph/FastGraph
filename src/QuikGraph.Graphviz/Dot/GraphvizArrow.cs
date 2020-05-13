using System.IO;
using JetBrains.Annotations;

namespace QuikGraph.Graphviz.Dot
{
    /// <summary>
    /// GraphViz arrow.
    /// </summary>
    public class GraphvizArrow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphvizArrow"/> class.
        /// </summary>
        /// <param name="shape">Arrow shape.</param>
        public GraphvizArrow(GraphvizArrowShape shape)
        {
            Shape = shape;
            Clipping = GraphvizArrowClipping.None;
            Filling = GraphvizArrowFilling.Close;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphvizArrow"/> class.
        /// </summary>
        /// <param name="shape">Arrow shape.</param>
        /// <param name="clipping">Arrow clipping.</param>
        /// <param name="filling">Arrow filling.</param>
        public GraphvizArrow(GraphvizArrowShape shape, GraphvizArrowClipping clipping, GraphvizArrowFilling filling)
        {
            Shape = shape;
            Clipping = clipping;
            Filling = filling;
        }

        /// <summary>
        /// Arrow shape.
        /// </summary>
        public GraphvizArrowShape Shape { get; set; }

        /// <summary>
        /// Arrow clipping.
        /// </summary>
        public GraphvizArrowClipping Clipping { get; set; }

        /// <summary>
        /// Arrow filling.
        /// </summary>
        public GraphvizArrowFilling Filling { get; set; }

        /// <summary>
        /// Converts this arrow to DOT.
        /// </summary>
        /// <returns>Arrow as DOT.</returns>
        [Pure]
        [NotNull]
        public string ToDot()
        {
            using (var writer = new StringWriter())
            {
                if (Filling == GraphvizArrowFilling.Open)
                {
                    writer.Write('o');
                }

                switch (Clipping)
                {
                    case GraphvizArrowClipping.Left:
                        writer.Write('l');
                        break;

                    case GraphvizArrowClipping.Right:
                        writer.Write('r');
                        break;
                }

                writer.Write(Shape.ToString().ToLower());

                return writer.ToString();
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return ToDot();
        }
    }
}