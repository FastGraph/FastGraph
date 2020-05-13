using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace QuikGraph.Graphviz.Dot
{
    /// <summary>
    /// Record escape helpers.
    /// </summary>
    public sealed class GraphvizRecordEscaper
    {
        [NotNull]
        private const string EolGroupName = "Eol";

        [NotNull]
        private const string CommonGroupName = "Common";

        [NotNull]
        private static readonly Regex EscapeRegex = new Regex(
            $"(?<{EolGroupName}>\\n)|(?<{CommonGroupName}>\\[|\\]|\\||<|>|\"| )",
            RegexOptions.ExplicitCapture | RegexOptions.Multiline | RegexOptions.Compiled);

        /// <summary>
        /// Escapes the given <paramref name="value"/>.
        /// </summary>
        /// <param name="value">String value top escape.</param>
        /// <returns>Escaped string.</returns>
        [Pure]
        [NotNull]
        public string Escape([NotNull] string value)
        {
            return EscapeRegex.Replace(
                value,
                match =>
                {
                    if (match.Groups[CommonGroupName] != null)
                    {
                        return $@"\{match.Value}";
                    }
                    return @"\n";
                });
        }
    }
}