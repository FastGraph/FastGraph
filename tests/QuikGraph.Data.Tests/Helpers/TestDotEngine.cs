using NUnit.Framework;
using QuikGraph.Graphviz;
using QuikGraph.Graphviz.Dot;

namespace QuikGraph.Data.Tests
{
    /// <summary>
    /// Test dot engine.
    /// </summary>
    internal class TestDotEngine : IDotEngine
    {
        /// <summary>
        /// Expected dot.
        /// </summary>
        public string ExpectedDot { get; set; }

        /// <inheritdoc />
        public string Run(GraphvizImageType imageType, string dot, string outputFilePath)
        {
            Assert.AreEqual(ExpectedDot, dot);
            return outputFilePath;
        }
    }
}