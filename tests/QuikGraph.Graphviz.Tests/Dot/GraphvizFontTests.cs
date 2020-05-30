using System;
using NUnit.Framework;
using QuikGraph.Graphviz.Dot;

namespace QuikGraph.Graphviz.Tests
{
    /// <summary>
    /// Tests related to <see cref="GraphvizFont"/>.
    /// </summary>
    [TestFixture]
    internal class GraphvizFontTests
    {
        [Test]
        public void Constructor()
        {
            var font = new GraphvizFont("TestFont", 12.5f);
            Assert.AreEqual("TestFont", font.Name);
            Assert.AreEqual(12.5f, font.SizeInPoints);

            font = new GraphvizFont("OtherFont", 22.0f);
            Assert.AreEqual("OtherFont", font.Name);
            Assert.AreEqual(22.0f, font.SizeInPoints);
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentException>(() => new GraphvizFont(null, 12.5f));
            Assert.Throws<ArgumentException>(() => new GraphvizFont("", 12.5f));
            Assert.Throws<ArgumentOutOfRangeException>(() => new GraphvizFont("Font", 0.0f));
            Assert.Throws<ArgumentOutOfRangeException>(() => new GraphvizFont("Font", -1.0f));
            // ReSharper restore ObjectCreationAsStatement
        }
    }
}