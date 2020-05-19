using JetBrains.Annotations;

namespace QuikGraph.Tests
{
    /// <summary>
    /// Unit test categories.
    /// </summary>
    public static class TestCategories
    {
        /// <summary>
        /// Long unit tests.
        /// </summary>
        [NotNull]
        public const string LongRunning = "LongRunning";

        /// <summary>
        /// Unit tests skipped by the CI.
        /// </summary>
        [NotNull]
        public const string CISkip = "CISkip";

        /// <summary>
        /// Verbose unit tests (not really relevant to test a feature).
        /// </summary>
        [NotNull]
        public const string Verbose = "VerboseTest";
    }
}