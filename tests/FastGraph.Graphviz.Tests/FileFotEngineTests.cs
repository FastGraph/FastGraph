#nullable enable

using NUnit.Framework;
using FastGraph.Graphviz.Dot;
using static FastGraph.Tests.FastGraphUnitTestsHelpers;

namespace FastGraph.Graphviz.Tests
{
    /// <summary>
    /// Tests related to <see cref="FileDotEngine"/>.
    /// </summary>
    [TestFixture]
    internal sealed class FileFotEngineTests
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
                File.Exists(filePath).Should().BeTrue();
                string loadedContent = File.ReadAllText(filePath);
                loadedContent.Should().Be(fileContent);
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
#pragma warning disable CS8625
            Invoking(() => dotEngine.Run(GraphvizImageType.Jpeg, default, filePath)).Should().Throw<ArgumentException>();
            Invoking(() => dotEngine.Run(GraphvizImageType.Jpeg, string.Empty, filePath)).Should().Throw<ArgumentException>();
            Invoking(() => dotEngine.Run(GraphvizImageType.Jpeg, dot, default)).Should().Throw<ArgumentException>();
            Invoking(() => dotEngine.Run(GraphvizImageType.Jpeg, dot, string.Empty)).Should().Throw<ArgumentException>();
            Invoking(() => dotEngine.Run(GraphvizImageType.Jpeg, default, default)).Should().Throw<ArgumentException>();
            Invoking(() => dotEngine.Run(GraphvizImageType.Jpeg, string.Empty, string.Empty)).Should().Throw<ArgumentException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
        }
    }
}
