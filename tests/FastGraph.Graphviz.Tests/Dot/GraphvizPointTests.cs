#nullable enable

using NUnit.Framework;
using FastGraph.Graphviz.Dot;

namespace FastGraph.Graphviz.Tests
{
    /// <summary>
    /// Tests for <see cref="GraphvizPoint"/>.
    /// </summary>
    [TestFixture]
    internal sealed class GraphvizPointTests
    {
        [Test]
        public void Constructor()
        {
            var point = new GraphvizPoint(0, 0);
            point.X.Should().Be(0);
            point.Y.Should().Be(0);

            point = new GraphvizPoint(1, 5);
            point.X.Should().Be(1);
            point.Y.Should().Be(5);

            point = new GraphvizPoint(-1, 3);
            point.X.Should().Be(-1);
            point.Y.Should().Be(3);
        }
    }
}
