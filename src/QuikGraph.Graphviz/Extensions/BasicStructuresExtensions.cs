#if SUPPORTS_BASIC_EXTENSIONS
using System.Drawing;
using JetBrains.Annotations;
using QuikGraph.Graphviz.Dot;

namespace QuikGraph.Graphviz
{
    /// <summary>
    /// Extensions related to basic structures types.
    /// </summary>
    public static class BasicStructuresExtensions
    {
        /// <summary>
        /// Converts a <see cref="GraphvizFont"/> into a <see cref="Font"/>.
        /// </summary>
        [Pure]
        [CanBeNull]
        [ContractAnnotation("font:null => null; font:notnull => notnull")]
        public static Font ToFont([CanBeNull] this GraphvizFont font)
        {
            return font is null ? null : new Font(font.Name, font.SizeInPoints);
        }

        /// <summary>
        /// Converts a <see cref="Font"/> into a <see cref="GraphvizFont"/>.
        /// </summary>
        [Pure]
        [CanBeNull]
        [ContractAnnotation("font:null => null; font:notnull => notnull")]
        public static GraphvizFont ToGraphvizFont([CanBeNull] this Font font)
        {
            return font is null ? null : new GraphvizFont(font.Name, font.SizeInPoints);
        }
    }
}
#endif