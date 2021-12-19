#nullable enable

using NUnit.Framework;
using FastGraph.Graphviz.Dot;

namespace FastGraph.Graphviz.Tests
{
    /// <summary>
    /// Tests related to <see cref="GraphvizLayerCollection"/>.
    /// </summary>
    [TestFixture]
    internal sealed class GraphvizLayerCollectionTests
    {
        [Test]
        public void Constructor()
        {
            var layerCollection = new GraphvizLayerCollection();
            layerCollection.Should().BeEmpty();
            layerCollection.Separators.Should().Be(":");

            layerCollection.Add(new GraphvizLayer("L1"));
            layerCollection.Should().BeEquivalentTo(new[] { (GraphvizLayer?)new GraphvizLayer("L1") });

            layerCollection = new GraphvizLayerCollection(new[] { new GraphvizLayer("L1"), new GraphvizLayer("L2") });
            layerCollection.Should().BeEquivalentTo(new[] { (GraphvizLayer?)new GraphvizLayer("L1"), new GraphvizLayer("L2") });
            layerCollection.Separators.Should().Be(":");

            var otherLayerCollection = new GraphvizLayerCollection(layerCollection);
            otherLayerCollection.Should().BeEquivalentTo(layerCollection);
            otherLayerCollection.Separators.Should().Be(":");
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new GraphvizLayerCollection((GraphvizLayer[]?)default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphvizLayerCollection((GraphvizLayerCollection?)default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Separators()
        {
            var layerCollection = new GraphvizLayerCollection();
            if (layerCollection.Separators != ":")
                throw new InvalidOperationException("Collection has wong separators.");

            layerCollection.Separators = ":,-";
            layerCollection.Separators.Should().Be(":,-");
        }

        [Test]
        public void Separators_Throws()
        {
            var layerCollection = new GraphvizLayerCollection();
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => layerCollection.Separators = default).Should().Throw<ArgumentException>();
#pragma warning restore CS8625
            Invoking(() => layerCollection.Separators = "").Should().Throw<ArgumentException>();
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void ToDot()
        {
            var layerCollection = new GraphvizLayerCollection();
            layerCollection.ToDot().Should().BeEmpty();

            layerCollection = new GraphvizLayerCollection(new[]
            {
                new GraphvizLayer("L0")
            });
            layerCollection.ToDot().Should().Be("layers=\"L0\"; layersep=\":\"");

            layerCollection = new GraphvizLayerCollection(new[]
            {
                new GraphvizLayer("L1"),
                new GraphvizLayer("L2"),
                new GraphvizLayer("L3")
            });
            layerCollection.ToDot().Should().Be("layers=\"L1:L2:L3\"; layersep=\":\"");

            layerCollection.Separators = " ";
            layerCollection.ToDot().Should().Be("layers=\"L1 L2 L3\"; layersep=\" \"");

            layerCollection.Separators = " -/";
            layerCollection.ToDot().Should().Be("layers=\"L1 -/L2 -/L3\"; layersep=\" -/\"");
        }
    }
}
