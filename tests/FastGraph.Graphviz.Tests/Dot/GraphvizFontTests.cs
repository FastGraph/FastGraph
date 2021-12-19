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
            font.Name.Should().Be("TestFont");
            font.SizeInPoints.Should().Be(12.5f);

            font = new GraphvizFont("OtherFont", 22.0f);
            font.Name.Should().Be("OtherFont");
            font.SizeInPoints.Should().Be(22.0f);
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Invoking(() => new GraphvizFont(default, 12.5f)).Should().Throw<ArgumentException>();
            Invoking(() => new GraphvizFont("", 12.5f)).Should().Throw<ArgumentException>();
            Invoking(() => new GraphvizFont("Font", 0.0f)).Should().Throw<ArgumentOutOfRangeException>();
            Invoking(() => new GraphvizFont("Font", -1.0f)).Should().Throw<ArgumentOutOfRangeException>();
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            // ReSharper restore ObjectCreationAsStatement
        }
    }
}
