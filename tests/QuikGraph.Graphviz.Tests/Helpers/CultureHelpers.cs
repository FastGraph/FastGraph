using System;
using System.Globalization;
using System.Threading;
using JetBrains.Annotations;
using static QuikGraph.Utils.DisposableHelpers;

namespace QuikGraph.Graphviz.Tests
{
    /// <summary>
    /// Helpers related to <see cref="CultureInfo"/>.
    /// </summary>
    internal static class CultureHelpers
    {
        /// <summary>
        /// French culture.
        /// </summary>
        [NotNull]
        public static readonly CultureInfo FrenchCulture = new CultureInfo("fr-FR");

        /// <summary>
        /// US culture.
        /// </summary>
        [NotNull]
        public static readonly CultureInfo EnglishCulture = new CultureInfo("en-US");

        /// <summary>
        /// Creates a scope using the given <paramref name="culture"/>.
        /// </summary>
        [NotNull]
        public static IDisposable CultureScope([NotNull] CultureInfo culture)
        {
            CultureInfo backupCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = culture;
            return Finally(() => Thread.CurrentThread.CurrentCulture = backupCulture);
        }
    }
}