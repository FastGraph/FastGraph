#nullable enable

using NUnit.Framework;
using FastGraph.Graphviz.Dot;

namespace FastGraph.Graphviz.Tests
{
    /// <summary>
    /// Tests related to <see cref="GraphvizLayer"/>.
    /// </summary>
    [TestFixture]
    internal sealed class GraphvizLayerTests
    {
        [Test]
        public void Constructor()
        {
            var layer = new GraphvizLayer("TestLayer");
            layer.Name.Should().Be("TestLayer");

            layer = new GraphvizLayer("OtherLayer");
            layer.Name.Should().Be("OtherLayer");
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new GraphvizLayer(default)).Should().Throw<ArgumentException>();
#pragma warning restore CS8625
            Invoking(() => new GraphvizLayer("")).Should().Throw<ArgumentException>();
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Name()
        {
            var layer = new GraphvizLayer("Layer");
            if (layer.Name != "Layer")
                throw new InvalidOperationException("Layer has wong name.");

            layer.Name = "LayerUpdated";
            layer.Name.Should().Be("LayerUpdated");
        }

        [Test]
        public void Name_Throws()
        {
            var layer = new GraphvizLayer("Layer");
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => layer.Name = default).Should().Throw<ArgumentException>();
#pragma warning restore CS8625
            Invoking(() => layer.Name = "").Should().Throw<ArgumentException>();
            // ReSharper restore ObjectCreationAsStatement
        }
    }
}
