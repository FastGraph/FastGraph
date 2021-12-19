#nullable enable

using NUnit.Framework;
using FastGraph.Graphviz.Dot;

namespace FastGraph.Graphviz.Tests
{
    /// <summary>
    /// Tests for <see cref="GraphvizColor"/>.
    /// </summary>
    [TestFixture]
    internal sealed class GraphvizColorTests
    {
        #region Test classes

        private class TestClass
        {
        }

        #endregion

        [Test]
        public void Constructor()
        {
            var color = default(GraphvizColor);
            CheckColor(color, 0, 0, 0, 0);

            color = new GraphvizColor(250, 255, 125, 45);
            CheckColor(color, 250, 255, 125, 45);

            color = new GraphvizColor(125, 12, 25, 42);
            CheckColor(color, 125, 12, 25, 42);

            #region Local function

            void CheckColor(GraphvizColor c, byte a, byte r, byte g, byte b)
            {
                c.A.Should().Be(a);
                c.R.Should().Be(r);
                c.G.Should().Be(g);
                c.B.Should().Be(b);
            }

            #endregion
        }

        [Test]
        public void Equals()
        {
            var color1 = new GraphvizColor(255, 150, 160, 25);
            var color2 = new GraphvizColor(255, 150, 160, 25);
            var color3 = new GraphvizColor(125, 150, 160, 25);
            var color4 = new GraphvizColor(255, 155, 160, 25);
            var color5 = new GraphvizColor(255, 150, 120, 25);
            var color6 = new GraphvizColor(255, 150, 160, 30);

            color1.Should().Be(color1);

            (color1 == color2).Should().BeTrue();
            color1.Equals(color2).Should().BeTrue();
            color1.Equals(color2).Should().BeTrue();
            (color1 != color2).Should().BeFalse();
            color2.Should().Be(color1);

            (color1 == color3).Should().BeFalse();
            color1.Equals(color3).Should().BeFalse();
            color1.Equals(color3).Should().BeFalse();
            (color1 != color3).Should().BeTrue();
            color3.Should().NotBe(color1);

            (color1 == color4).Should().BeFalse();
            color1.Equals(color4).Should().BeFalse();
            color1.Equals(color4).Should().BeFalse();
            (color1 != color4).Should().BeTrue();
            color4.Should().NotBe(color1);

            (color1 == color5).Should().BeFalse();
            color1.Equals(color5).Should().BeFalse();
            color1.Equals(color5).Should().BeFalse();
            (color1 != color5).Should().BeTrue();
            color5.Should().NotBe(color1);

            (color1 == color6).Should().BeFalse();
            color1.Equals(color6).Should().BeFalse();
            color1.Equals(color6).Should().BeFalse();
            (color1 != color6).Should().BeTrue();
            color6.Should().NotBe(color1);

            color1.Should().NotBe(default);
            new TestClass().Should().NotBe(color1);
        }

        [Test]
        public void HashCode()
        {
            var color1 = new GraphvizColor();
            var color2 = new GraphvizColor();
            var color3 = new GraphvizColor(125, 150, 160, 25);
            var color4 = new GraphvizColor(125, 150, 160, 25);

            color2.GetHashCode().Should().Be(color1.GetHashCode());
            color3.GetHashCode().Should().NotBe(color1.GetHashCode());
            color4.GetHashCode().Should().Be(color3.GetHashCode());
        }
    }
}
