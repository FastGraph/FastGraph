using System;
using System.IO;
using NUnit.Framework;
using QuikGraph.Graphviz.Dot;
using static QuikGraph.Tests.QuikGraphUnitTestsHelpers;

namespace QuikGraph.Graphviz.Tests
{
    /// <summary>
    /// Tests related to <see cref="FileDotEngine"/>.
    /// </summary>
    [TestFixture]
    internal class FileFotEngineTests
    {
        [Test]
        public void Run()
        {
            const string testFileName = "output";
            string dotFilePath = Path.Combine(GetTemporaryTestDirectory(), testFileName);
            const string content = "digraph G {}";

            var dotEngine = new FileDotEngine();
            dotEngine.Run(GraphvizImageType.Jpeg, content, dotFilePath);
            CheckFileContent($"{dotFilePath}.dot", content);

            const string testFileName2 = "output2.dot";
            string dotFilePath2 = Path.Combine(GetTemporaryTestDirectory(), testFileName2);
            const string content2 = "digraph G { 1; 2; 3; 1 -- 2; }";
            dotEngine.Run(GraphvizImageType.Jpeg, content2, dotFilePath2);
            CheckFileContent(dotFilePath2, content2);

            #region Local function

            void CheckFileContent(string filePath, string fileContent)
            {
                Assert.IsTrue(File.Exists(filePath));
                string loadedContent = File.ReadAllText(filePath);
                Assert.AreEqual(fileContent, loadedContent);
            }

            #endregion
        }

        [Test]
        public void Run_Throws()
        {
            const string dot = "digraph G {}";
            const string filePath = "NotSaved.dot";

            var dotEngine = new FileDotEngine();
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentException>(() => dotEngine.Run(GraphvizImageType.Jpeg, null, filePath));
            Assert.Throws<ArgumentException>(() => dotEngine.Run(GraphvizImageType.Jpeg, string.Empty, filePath));
            Assert.Throws<ArgumentException>(() => dotEngine.Run(GraphvizImageType.Jpeg, dot, null));
            Assert.Throws<ArgumentException>(() => dotEngine.Run(GraphvizImageType.Jpeg, dot, string.Empty));
            Assert.Throws<ArgumentException>(() => dotEngine.Run(GraphvizImageType.Jpeg, null, null));
            Assert.Throws<ArgumentException>(() => dotEngine.Run(GraphvizImageType.Jpeg, string.Empty, string.Empty));
            // ReSharper restore AssignNullToNotNullAttribute
        }
    }
}