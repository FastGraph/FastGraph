using System;
using NUnit.Framework;
using QuikGraph.Graphviz.Dot;

namespace QuikGraph.Graphviz.Tests
{
    /// <summary>
    /// Tests related to <see cref="GraphvizLayer"/>.
    /// </summary>
    [TestFixture]
    internal class GraphvizLayerTests
    {
        [Test]
        public void Constructor()
        {
            var layer = new GraphvizLayer("TestLayer");
            Assert.AreEqual("TestLayer", layer.Name);
            
            layer = new GraphvizLayer("OtherLayer");
            Assert.AreEqual("OtherLayer", layer.Name);
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentException>(() => new GraphvizLayer(null));
            Assert.Throws<ArgumentException>(() => new GraphvizLayer(""));
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Name()
        {
            var layer = new GraphvizLayer("Layer");
            if (layer.Name != "Layer")
                throw new InvalidOperationException("Layer has wong name.");

            layer.Name = "LayerUpdated";
            Assert.AreEqual("LayerUpdated", layer.Name);
        }

        [Test]
        public void Name_Throws()
        {
            var layer = new GraphvizLayer("Layer");
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentException>(() => layer.Name = null);
            Assert.Throws<ArgumentException>(() => layer.Name = "");
            // ReSharper restore ObjectCreationAsStatement
        }
    }
}