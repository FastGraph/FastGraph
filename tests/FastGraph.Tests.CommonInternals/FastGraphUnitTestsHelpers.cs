#nullable enable

using NUnit.Framework;
using static FastGraph.Utils.DisposableHelpers;

namespace FastGraph.Tests
{
    /// <summary>
    /// Helpers for FastGraph unit tests.
    /// </summary>
    internal static class FastGraphUnitTestsHelpers
    {
        /// <summary>
        /// Returns the path to the current test directory.
        /// </summary>
        public static string GetTestDirectory()
        {
            return TestContext.CurrentContext.TestDirectory;
        }

        /// <summary>
        /// Returns the path to the temporary test directory.
        /// </summary>
        public static string GetTemporaryTestDirectory()
        {
            return Path.Combine(GetTestDirectory(), "Temp");
        }

        /// <summary>
        /// Sets the working directory to the given <paramref name="directory"/>
        /// and restore the previous one at the end of the scope.
        /// </summary>
        /// <param name="directory">Directory to set as working directory.</param>
        public static IDisposable SetWorkingDirectory(string directory)
        {
            string backupDirectory = Environment.CurrentDirectory;
            Environment.CurrentDirectory = directory;

            return Finally(() => Environment.CurrentDirectory = backupDirectory);
        }

        /// <summary>
        /// Sets the working directory to a reserved test folder
        /// and restore the previous one at the end of the scope.
        /// </summary>
        public static IDisposable SetTemporaryTestWorkingDirectory()
        {
            return SetWorkingDirectory(GetTemporaryTestDirectory());
        }
    }
}
