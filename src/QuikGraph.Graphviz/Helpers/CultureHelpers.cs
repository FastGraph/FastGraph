using System.Globalization;
using JetBrains.Annotations;

namespace QuikGraph.Graphviz.Helpers
{
    /// <summary>
    /// Helpers related to <see cref="CultureInfo"/>.
    /// </summary>
    internal static class CultureHelpers
    {
        /// <summary>
        /// Converts the given <see cref="float"/> <paramref name="value"/> to <see cref="string"/>
        /// using invariant culture.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns>Float as string.</returns>
        [Pure]
        [NotNull]
        public static string ToInvariantString(this float value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the given <see cref="double"/> <paramref name="value"/> to <see cref="string"/>
        /// using invariant culture.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns>Double as string.</returns>
        [Pure]
        [NotNull]
        public static string ToInvariantString(this double value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }
    }
}