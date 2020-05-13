using System;

namespace QuikGraph.Graphviz.Dot
{
    /// <summary>
    /// Represents a color.
    /// </summary>
    public struct GraphvizColor : IEquatable<GraphvizColor>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphvizColor"/> struct.
        /// </summary>
        /// <param name="a">Alpha channel value.</param>
        /// <param name="r">Red channel value.</param>
        /// <param name="g">Green channel value.</param>
        /// <param name="b">Blue channel value.</param>
        public GraphvizColor(byte a, byte r, byte g, byte b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        /// <summary>
        /// Alpha channel value.
        /// </summary>
        public byte A { get; }

        /// <summary>
        /// Red channel value.
        /// </summary>
        public byte R { get; }

        /// <summary>
        /// Green channel value.
        /// </summary>
        public byte G { get; }

        /// <summary>
        /// Blue channel value.
        /// </summary>
        public byte B { get; }

        /// <inheritdoc />
        public bool Equals(GraphvizColor other)
        {
            return A == other.A && R == other.R && G == other.G && B == other.B;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return A << 24 | R << 16 | G << 8 | B;
        }

        /// <summary>
        /// Black color.
        /// </summary>
        public static GraphvizColor Black { get; } = new GraphvizColor(0xFF, 0, 0, 0);

        /// <summary>
        /// White color.
        /// </summary>
        public static GraphvizColor White { get; } = new GraphvizColor(0xFF, 0xFF, 0xFF, 0xFF);

        /// <summary>
        /// Light yellow color.
        /// </summary>
        public static GraphvizColor LightYellow { get; } = new GraphvizColor(0xFF, 0xFF, 0xFF, 0xE0);
    }
}