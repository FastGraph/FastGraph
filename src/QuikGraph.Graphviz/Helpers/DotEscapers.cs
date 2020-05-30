using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace QuikGraph.Graphviz
{
    /// <summary>
    /// Dot escape helpers.
    /// </summary>
    public static class DotEscapers
    {
        [NotNull]
        private const string EolGroupName = "Eol";

        [NotNull]
        private const string CommonGroupName = "Common";

        [NotNull]
        private const string EolPattern = "\\r\\n|\\n|\\r";

        [NotNull]
        private const string DoubleQuotePattern = "\"";

        [NotNull]
        private const string BackslashPattern = "\\\\";

        [NotNull]
        private static readonly Regex RecordEscapeRegex = new Regex(
            $"(?<{EolGroupName}>{EolPattern})|(?<{CommonGroupName}>\\||<|>|{DoubleQuotePattern}| |{BackslashPattern})",
            RegexOptions.ExplicitCapture | RegexOptions.Multiline | RegexOptions.Compiled);

        [NotNull]
        private static readonly Regex GeneralEscapeRegex = new Regex(
            $"(?<{EolGroupName}>{EolPattern})|(?<{CommonGroupName}>{DoubleQuotePattern}|{BackslashPattern})",
            RegexOptions.Multiline | RegexOptions.Compiled);

        /// <summary>
        /// Escapes the given <paramref name="value"/> as being a record port value.
        /// </summary>
        /// <param name="value">String value to escape.</param>
        /// <returns>Escaped string.</returns>
        [Pure]
        [NotNull]
        public static string EscapePort([NotNull] string value)
        {
            return RecordEscapeRegex.Replace(
                value,
                match => "_");
        }

        /// <summary>
        /// Escapes the given <paramref name="value"/> as being a record value.
        /// </summary>
        /// <param name="value">String value to escape.</param>
        /// <returns>Escaped string.</returns>
        [Pure]
        [NotNull]
        public static string EscapeRecord([NotNull] string value)
        {
            return RecordEscapeRegex.Replace(
                value,
                match => match.Groups[CommonGroupName].Success
                    ? $@"\{match.Value}"
                    : @"\n");
        }

        /// <summary>
        /// Escapes the given <paramref name="value"/>.
        /// </summary>
        /// <param name="value">String value to escape.</param>
        /// <returns>Escaped string.</returns>
        [Pure]
        [NotNull]
        public static string Escape([NotNull] string value)
        {
            return GeneralEscapeRegex.Replace(
                value,
                match => match.Groups[CommonGroupName].Success
                    ? $@"\{match.Value}"
                    : @"\n");
        }
    }
}