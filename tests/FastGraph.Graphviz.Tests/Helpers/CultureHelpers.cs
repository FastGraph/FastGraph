#nullable enable

using System.Globalization;
using static FastGraph.Utils.DisposableHelpers;

namespace FastGraph.Graphviz.Tests
{
    /// <summary>
    /// Helpers related to <see cref="CultureInfo"/>.
    /// </summary>
    internal static class CultureHelpers
    {
        /// <summary>
        /// French culture.
        /// </summary>
        public static readonly CultureInfo FrenchCulture = new CultureInfo("fr-FR");

        /// <summary>
        /// US culture.
        /// </summary>
        public static readonly CultureInfo EnglishCulture = new CultureInfo("en-US");

        /// <summary>
        /// Creates a scope using the given <paramref name="culture"/>.
        /// </summary>
        public static IDisposable CultureScope(CultureInfo culture)
        {
            CultureInfo backupCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = culture;
            return Finally(() => Thread.CurrentThread.CurrentCulture = backupCulture);
        }
    }
}
