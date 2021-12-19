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
            CollectionAssert.IsEmpty(layerCollection);
            Assert.AreEqual(":", layerCollection.Separators);

            var layer1 = new GraphvizLayer("L1");
            layerCollection.Add(layer1);
            CollectionAssert.AreEqual(new[] { layer1 }, layerCollection);

            var layer2 = new GraphvizLayer("L2");
            var layerArray = new[] { layer1, layer2 };
            layerCollection = new GraphvizLayerCollection(layerArray);
            CollectionAssert.AreEqual(layerArray, layerCollection);
            Assert.AreEqual(":", layerCollection.Separators);

            var otherLayerCollection = new GraphvizLayerCollection(layerCollection);
            CollectionAssert.AreEqual(layerCollection, otherLayerCollection);
            Assert.AreEqual(":", otherLayerCollection.Separators);
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Assert.Throws<ArgumentNullException>(() => new GraphvizLayerCollection((GraphvizLayer[]?)default));
            Assert.Throws<ArgumentNullException>(() => new GraphvizLayerCollection((GraphvizLayerCollection?)default));
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
            Assert.AreEqual(":,-", layerCollection.Separators);
        }

        [Test]
        public void Separators_Throws()
        {
            var layerCollection = new GraphvizLayerCollection();
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Assert.Throws<ArgumentException>(() => layerCollection.Separators = default);
#pragma warning restore CS8625
            Assert.Throws<ArgumentException>(() => layerCollection.Separators = "");
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void ToDot()
        {
            var layerCollection = new GraphvizLayerCollection();
            Assert.AreEqual(string.Empty, layerCollection.ToDot());

            layerCollection = new GraphvizLayerCollection(new[]
            {
                new GraphvizLayer("L0")
            });
            Assert.AreEqual(
                "layers=\"L0\"; layersep=\":\"",
                layerCollection.ToDot());

            layerCollection = new GraphvizLayerCollection(new[]
            {
                new GraphvizLayer("L1"),
                new GraphvizLayer("L2"),
                new GraphvizLayer("L3")
            });
            Assert.AreEqual(
                "layers=\"L1:L2:L3\"; layersep=\":\"",
                layerCollection.ToDot());

            layerCollection.Separators = " ";
            Assert.AreEqual(
                "layers=\"L1 L2 L3\"; layersep=\" \"",
                layerCollection.ToDot());

            layerCollection.Separators = " -/";
            Assert.AreEqual(
                "layers=\"L1 -/L2 -/L3\"; layersep=\" -/\"",
                layerCollection.ToDot());
        }
    }
}
