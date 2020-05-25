using NUnit.Framework;
using QuikGraph.Graphviz.Dot;

namespace QuikGraph.Graphviz.Tests
{
    /// <summary>
    /// Tests related to <see cref="GraphvizArrow"/>.
    /// </summary>
    [TestFixture]
    internal class GraphvizArrowTests
    {
        [Test]
        public void Constructor()
        {
            var arrow = new GraphvizArrow(GraphvizArrowShape.Box);
            CheckArrow(arrow, GraphvizArrowShape.Box);

            arrow = new GraphvizArrow(GraphvizArrowShape.Diamond);
            CheckArrow(arrow, GraphvizArrowShape.Diamond);

            arrow = new GraphvizArrow(
                GraphvizArrowShape.Dot,
                GraphvizArrowClipping.Left,
                GraphvizArrowFilling.Open);
            CheckArrow(
                arrow,
                GraphvizArrowShape.Dot,
                GraphvizArrowClipping.Left,
                GraphvizArrowFilling.Open);

            arrow = new GraphvizArrow(
                GraphvizArrowShape.Normal,
                GraphvizArrowClipping.Right,
                GraphvizArrowFilling.Close);
            CheckArrow(
                arrow,
                GraphvizArrowShape.Normal,
                GraphvizArrowClipping.Right);

            #region Local function

            void CheckArrow(
                GraphvizArrow a,
                GraphvizArrowShape shape,
                GraphvizArrowClipping clipping = GraphvizArrowClipping.None,
                GraphvizArrowFilling filling = GraphvizArrowFilling.Close)
            {
                Assert.AreEqual(shape, a.Shape);
                Assert.AreEqual(clipping, a.Clipping);
                Assert.AreEqual(filling, a.Filling);
            }

            #endregion
        }

        [Test]
        public void ToDot()
        {
            // Box variants
            var arrow = new GraphvizArrow(GraphvizArrowShape.Box, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            Assert.AreEqual("box", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.Box, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            Assert.AreEqual("lbox", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.Box, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            Assert.AreEqual("rbox", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.Box, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            Assert.AreEqual("obox", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.Box, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            Assert.AreEqual("olbox", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.Box, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            Assert.AreEqual("orbox", arrow.ToString());

            // Crow variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Crow, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            Assert.AreEqual("crow", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.Crow, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            Assert.AreEqual("lcrow", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.Crow, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            Assert.AreEqual("rcrow", arrow.ToString());

            // Diamond variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Diamond, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            Assert.AreEqual("diamond", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.Diamond, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            Assert.AreEqual("ldiamond", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.Diamond, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            Assert.AreEqual("rdiamond", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.Diamond, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            Assert.AreEqual("odiamond", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.Diamond, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            Assert.AreEqual("oldiamond", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.Diamond, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            Assert.AreEqual("ordiamond", arrow.ToString());

            // Dot variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Dot, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            Assert.AreEqual("dot", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.Dot, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            Assert.AreEqual("odot", arrow.ToString());

            // Inv variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Inv, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            Assert.AreEqual("inv", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.Inv, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            Assert.AreEqual("linv", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.Inv, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            Assert.AreEqual("rinv", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.Inv, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            Assert.AreEqual("oinv", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.Inv, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            Assert.AreEqual("olinv", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.Inv, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            Assert.AreEqual("orinv", arrow.ToString());

            // None
            arrow = new GraphvizArrow(GraphvizArrowShape.None, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            Assert.AreEqual("none", arrow.ToString());

            // Normal variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Normal, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            Assert.AreEqual("normal", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.Normal, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            Assert.AreEqual("lnormal", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.Normal, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            Assert.AreEqual("rnormal", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.Normal, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            Assert.AreEqual("onormal", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.Normal, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            Assert.AreEqual("olnormal", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.Normal, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            Assert.AreEqual("ornormal", arrow.ToString());

            // Tee variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Tee, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            Assert.AreEqual("tee", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.Tee, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            Assert.AreEqual("ltee", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.Tee, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            Assert.AreEqual("rtee", arrow.ToString());

            // Vee variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Vee, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            Assert.AreEqual("vee", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.Vee, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            Assert.AreEqual("lvee", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.Vee, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            Assert.AreEqual("rvee", arrow.ToString());

            // Curve variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Curve, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            Assert.AreEqual("curve", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.Curve, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            Assert.AreEqual("lcurve", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.Curve, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            Assert.AreEqual("rcurve", arrow.ToString());

            // ICurve variants
            arrow = new GraphvizArrow(GraphvizArrowShape.ICurve, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            Assert.AreEqual("icurve", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.ICurve, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            Assert.AreEqual("licurve", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.ICurve, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            Assert.AreEqual("ricurve", arrow.ToString());
        }

        [Test]
        public void ToDot_SkippedModifiers()
        {
            // Skipped Crow variants
            var arrow = new GraphvizArrow(GraphvizArrowShape.Crow, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            Assert.AreEqual("crow", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.Crow, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            Assert.AreEqual("lcrow", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.Crow, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            Assert.AreEqual("rcrow", arrow.ToString());

            // Skipped Dot variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Dot, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            Assert.AreEqual("dot", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.Dot, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            Assert.AreEqual("dot", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.Dot, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            Assert.AreEqual("odot", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.Dot, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            Assert.AreEqual("odot", arrow.ToString());

            // Skipped None variants
            arrow = new GraphvizArrow(GraphvizArrowShape.None, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            Assert.AreEqual("none", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.None, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            Assert.AreEqual("none", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.None, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            Assert.AreEqual("none", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.None, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            Assert.AreEqual("none", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.None, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            Assert.AreEqual("none", arrow.ToString());

            // Skipped Tee variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Tee, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            Assert.AreEqual("tee", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.Tee, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            Assert.AreEqual("ltee", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.Tee, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            Assert.AreEqual("rtee", arrow.ToString());

            // Skipped Vee variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Vee, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            Assert.AreEqual("vee", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.Vee, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            Assert.AreEqual("lvee", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.Vee, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            Assert.AreEqual("rvee", arrow.ToString());

            // Skipped Curve variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Curve, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            Assert.AreEqual("curve", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.Curve, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            Assert.AreEqual("lcurve", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.Curve, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            Assert.AreEqual("rcurve", arrow.ToString());

            // Skipped ICurve variants
            arrow = new GraphvizArrow(GraphvizArrowShape.ICurve, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            Assert.AreEqual("icurve", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.ICurve, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            Assert.AreEqual("licurve", arrow.ToString());
            arrow = new GraphvizArrow(GraphvizArrowShape.ICurve, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            Assert.AreEqual("ricurve", arrow.ToString());
        }
    }
}
