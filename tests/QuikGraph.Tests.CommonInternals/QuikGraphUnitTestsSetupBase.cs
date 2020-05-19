using System.IO;
using NUnit.Framework;
using static QuikGraph.Tests.QuikGraphUnitTestsHelpers;

namespace QuikGraph.Tests
{
    /// <summary>
    /// QuikGraph unit tests setup.
    /// </summary>
    internal abstract class QuikGraphUnitTestsSetupBase
    {
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