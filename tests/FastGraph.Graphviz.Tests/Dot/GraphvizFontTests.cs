#nullable enable

using NUnit.Framework;
using FastGraph.Graphviz.Dot;

namespace FastGraph.Graphviz.Tests
{
    /// <summary>
    /// Tests related to <see cref="GraphvizFont"/>.
    /// </summary>
    [TestFixture]
    internal sealed class GraphvizFontTests
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
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Assert.Throws<ArgumentException>(() => new GraphvizFont(default, 12.5f));
            Assert.Throws<ArgumentException>(() => new GraphvizFont("", 12.5f));
            Assert.Throws<ArgumentOutOfRangeException>(() => new GraphvizFont("Font", 0.0f));
            Assert.Throws<ArgumentOutOfRangeException>(() => new GraphvizFont("Font", -1.0f));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            // ReSharper restore ObjectCreationAsStatement
        }
    }
}
