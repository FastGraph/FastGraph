#if SUPPORTS_FONT
using System.Drawing;
using NUnit.Framework;
using QuikGraph.Graphviz.Dot;

namespace QuikGraph.Graphviz.Tests
{
    /// <summary>
    /// Tests for <see cref="BasicStructuresExtensions"/>.
    /// </summary>
    [TestFixture]
    internal class BasicStructuresExtensionsTests
    {
        #region Test helpers

        private static void AssertEqual(Font font, GraphvizFont qFont)
        {
            if (font is null)
            {
                Assert.IsNull(qFont);
            }
            else
            {
                Assert.AreEqual(qFont.Name, font.Name);
                Assert.AreEqual(qFont.SizeInPoints, font.SizeInPoints);
            }
        }

        #endregion

        [Test]
        public void ToFont()
        {
            var qFont = new GraphvizFont("Microsoft Sans Serif", 12.5f);
            Font font = qFont.ToFont();
            AssertEqual(font, qFont);

            qFont = new GraphvizFont("Comic Sans MS", 25.0f);
            font = qFont.ToFont();
            AssertEqual(font, qFont);

            // ReSharper disable ExpressionIsAlwaysNull
            qFont = null;
            font = qFont.ToFont();
            AssertEqual(font, qFont);
            // ReSharper restore ExpressionIsAlwaysNull
        }

        [Test]
        public void ToGraphvizFont()
        {
            var font = new Font("Microsoft Sans Serif", 12.5f);
            GraphvizFont qFont = font.ToGraphvizFont();
            AssertEqual(font, qFont);

            font = new Font("Comic Sans MS", 25.0f);
            qFont = font.ToGraphvizFont();
            AssertEqual(font, qFont);

            // ReSharper disable ExpressionIsAlwaysNull
            font = null;
            qFont = font.ToGraphvizFont();
            AssertEqual(font, qFont);
            // ReSharper restore ExpressionIsAlwaysNull
        }
    }
}
#endif