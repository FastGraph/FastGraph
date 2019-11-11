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
        /// Verbose unit tests (not really relevant to test a feature).
        /// </summary>
        [NotNull]
        public const string Verbose = "VerboseTest";
    }
}
