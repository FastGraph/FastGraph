using System;
using System.IO;
using JetBrains.Annotations;
using NUnit.Framework;
using static QuikGraph.Utils.DisposableHelpers;

namespace QuikGraph.Tests
{
    /// <summary>
    /// Helpers for QuikGraph unit tests.
    /// </summary>
    internal static class QuikGraphUnitTestsHelpers
    {
        /// <summary>
        /// Returns the path to the current test directory.
        /// </summary>
        [NotNull]
        public static string GetTestDirectory()
        {
            return TestContext.CurrentContext.TestDirectory;
        }

        /// <summary>
        /// Returns the path to the current test ML graph directory.
        /// </summary>
        [NotNull]
        public static string GetTestGraphDirectory()
        {
            return Path.Combine(GetTestDirectory(), "GraphML");
        }

        /// <summary>
        /// Returns the path to the temporary test directory.
        /// </summary>
        [NotNull]
        public static string GetTemporaryTestDirectory()
        {
            return Path.Combine(GetTestDirectory(), "Temp");
        }

        /// <summary>
        /// Returns the path to the current test ML graph directory.
        /// </summary>
        [NotNull]
        public static string GetGraphFilePath(string fileName)
        {
            return Path.Combine(GetTestGraphDirectory(), fileName);
        }

        /// <summary>
        /// Sets the working directory to the given <paramref name="directory"/>
        /// and restore the previous one at the end of the scope.
        /// </summary>
        /// <param name="directory">Directory to set as working directory.</param>
        [NotNull]
        public static IDisposable SetWorkingDirectory([NotNull] string directory)
        {
            string backupDirectory = Environment.CurrentDirectory;
            Environment.CurrentDirectory = directory;

            return Finally(() => Environment.CurrentDirectory = backupDirectory);
        }

        /// <summary>
        /// Sets the working directory to a reserved test folder
        /// and restore the previous one at the end of the scope.
        /// </summary>
        [NotNull]
        public static IDisposable SetTemporaryTestWorkingDirectory()
        {
            return SetWorkingDirectory(GetTemporaryTestDirectory());
        }
    }
}