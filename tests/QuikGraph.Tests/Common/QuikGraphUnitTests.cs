using System;
using System.IO;
using NUnit.Framework;
using static QuikGraph.Utils.DisposableHelpers;

namespace QuikGraph.Tests
{
    /// <summary>
    /// Base class for QuikGraph unit tests.
    /// </summary>
    [SetUpFixture]
    internal class QuikGraphUnitTests
    {
        /// <summary>
        /// Returns the path to the current test directory.
        /// </summary>
        protected string GetTestDirectory()
        {
            return TestContext.CurrentContext.TestDirectory;
        }

        /// <summary>
        /// Returns the path to the current test ML graph directory.
        /// </summary>
        protected string GetTestGraphDirectory()
        {
            return Path.Combine(GetTestDirectory(), "GraphML");
        }

        /// <summary>
        /// Returns the path to the temporary test directory.
        /// </summary>
        protected string GetTemporaryTestDirectory()
        {
            return Path.Combine(GetTestDirectory(), "Temp");
        }

        /// <summary>
        /// Returns the path to the current test ML graph directory.
        /// </summary>
        protected string GetGraphFilePath(string fileName)
        {
            return Path.Combine(GetTestGraphDirectory(), fileName);
        }

        /// <summary>
        /// Sets the working directory to the given <paramref name="directory"/>
        /// and restore the previous one at the end of the scope.
        /// </summary>
        /// <param name="directory">Directory to set as working directory.</param>
        protected IDisposable SetWorkingDirectory(string directory)
        {
            string backupDirectory = Environment.CurrentDirectory;
            Environment.CurrentDirectory = directory;

            return Finally(() => Environment.CurrentDirectory = backupDirectory);
        }

        /// <summary>
        /// Sets the working directory to a reserved test folder
        /// and restore the previous one at the end of the scope.
        /// </summary>
        protected IDisposable SetTemporaryTestWorkingDirectory()
        {
            return SetWorkingDirectory(GetTemporaryTestDirectory());
        }

        /// <summary>
        /// Fixture setup.
        /// </summary>
        [OneTimeSetUp]
        public void OnOneTimeSetup()
        {
            string tmpDirectory = GetTemporaryTestDirectory();
            if (Directory.Exists(tmpDirectory))
                Directory.Delete(tmpDirectory, true);

            Directory.CreateDirectory(tmpDirectory);
        }

        /// <summary>
        /// Fixture tear down.
        /// </summary>
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            string tmpDirectory = GetTemporaryTestDirectory();
            if (Directory.Exists(tmpDirectory))
                Directory.Delete(tmpDirectory, true);
        }
    }
}