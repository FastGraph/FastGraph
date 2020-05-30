using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace QuikGraph.Graphviz
{
    /// <summary>
    /// Dot escape helpers.
    /// </summary>
    public sealed class DotEscapers
    {
        [NotNull]
        private const string EolGroupName = "Eol";

        [NotNull]
        private const string CommonGroupName = "Common";

        [NotNull]
        private static readonly Regex EscapeRegex = new Regex(
            $"(?<{EolGroupName}>\\r\\n|\\n|\\r)|(?<{CommonGroupName}>\\||<|>|\"| |\\\\)",
            RegexOptions.ExplicitCapture | RegexOptions.Multiline | RegexOptions.Compiled);

        /// <summary>
        /// Escapes the given <paramref name="value"/> as being a record port value.
        /// </summary>
        /// <param name="value">String value to escape.</param>
        /// <returns>Escaped string.</returns>
        [Pure]
        [NotNull]
        public string EscapePort([NotNull] string value)
        {
            return EscapeRegex.Replace(
                value,
                match => "_");
        }

        /// <summary>
        /// Escapes the given <paramref name="value"/>.
        /// </summary>
        /// <param name="value">String value to escape.</param>
        /// <returns>Escaped string.</returns>
        [Pure]
        [NotNull]
        public string Escape([NotNull] string value)
        {
            return EscapeRegex.Replace(
                value,
                match => match.Groups[CommonGroupName].Success
                    ? $@"\{match.Value}"
                    : @"\n");
        }
    }
}