using NUnit.Framework;
using QuikGraph.Graphviz.Dot;

namespace QuikGraph.Graphviz.Tests
{
    /// <summary>
    /// Tests for <see cref="GraphvizPoint"/>.
    /// </summary>
    [TestFixture]
    internal class GraphvizPointTests
    {
        [Test]
        public void Constructor()
        {
            var point = new GraphvizPoint(0, 0);
            Assert.AreEqual(0, point.X);
            Assert.AreEqual(0, point.Y);

            point = new GraphvizPoint(1, 5);
            Assert.AreEqual(1, point.X);
            Assert.AreEqual(5, point.Y);

            point = new GraphvizPoint(-1, 3);
            Assert.AreEqual(-1, point.X);
            Assert.AreEqual(3, point.Y);
        }
    }
}