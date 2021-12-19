#nullable enable

using FastGraph.Graphviz;
using FastGraph.Graphviz.Dot;

namespace FastGraph.Data.Tests
{
    /// <summary>
    /// Test dot engine.
    /// </summary>
    internal sealed class TestDotEngine : IDotEngine
    {
        /// <summary>
        /// Expected dot.
        /// </summary>
        public string ExpectedDot { get; set; } = default!;

        /// <inheritdoc />
        public string Run(GraphvizImageType imageType, string dot, string outputFilePath)
        {
            dot.Should().Be(ExpectedDot);
            return outputFilePath;
        }
    }
}
