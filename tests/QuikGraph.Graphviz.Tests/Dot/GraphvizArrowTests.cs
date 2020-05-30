using System;
using System.Collections.Generic;
using JetBrains.Annotations;
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

        [NotNull, ItemNotNull]
        private static IEnumerable<TestCaseData> ToDotTestCases
        {
            get
            {
                Func<GraphvizArrow, string> toDot = arrow => arrow.ToDot();
                yield return new TestCaseData(toDot);

                Func<GraphvizArrow, string> toString = arrow => arrow.ToString();
                yield return new TestCaseData(toString);
            }
        }

        [TestCaseSource(nameof(ToDotTestCases))]
        public void ToDot([NotNull, InstantHandle] Func<GraphvizArrow, string> convert)
        {
            // Box variants
            var arrow = new GraphvizArrow(GraphvizArrowShape.Box, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            Assert.AreEqual("box", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.Box, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            Assert.AreEqual("lbox", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.Box, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            Assert.AreEqual("rbox", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.Box, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            Assert.AreEqual("obox", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.Box, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            Assert.AreEqual("olbox", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.Box, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            Assert.AreEqual("orbox", convert(arrow));

            // Crow variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Crow, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            Assert.AreEqual("crow", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.Crow, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            Assert.AreEqual("lcrow", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.Crow, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            Assert.AreEqual("rcrow", convert(arrow));

            // Diamond variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Diamond, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            Assert.AreEqual("diamond", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.Diamond, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            Assert.AreEqual("ldiamond", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.Diamond, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            Assert.AreEqual("rdiamond", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.Diamond, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            Assert.AreEqual("odiamond", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.Diamond, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            Assert.AreEqual("oldiamond", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.Diamond, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            Assert.AreEqual("ordiamond", convert(arrow));

            // Dot variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Dot, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            Assert.AreEqual("dot", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.Dot, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            Assert.AreEqual("odot", convert(arrow));

            // Inv variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Inv, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            Assert.AreEqual("inv", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.Inv, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            Assert.AreEqual("linv", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.Inv, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            Assert.AreEqual("rinv", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.Inv, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            Assert.AreEqual("oinv", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.Inv, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            Assert.AreEqual("olinv", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.Inv, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            Assert.AreEqual("orinv", convert(arrow));

            // None
            arrow = new GraphvizArrow(GraphvizArrowShape.None, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            Assert.AreEqual("none", convert(arrow));

            // Normal variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Normal, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            Assert.AreEqual("normal", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.Normal, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            Assert.AreEqual("lnormal", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.Normal, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            Assert.AreEqual("rnormal", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.Normal, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            Assert.AreEqual("onormal", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.Normal, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            Assert.AreEqual("olnormal", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.Normal, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            Assert.AreEqual("ornormal", convert(arrow));

            // Tee variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Tee, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            Assert.AreEqual("tee", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.Tee, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            Assert.AreEqual("ltee", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.Tee, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            Assert.AreEqual("rtee", convert(arrow));

            // Vee variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Vee, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            Assert.AreEqual("vee", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.Vee, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            Assert.AreEqual("lvee", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.Vee, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            Assert.AreEqual("rvee", convert(arrow));

            // Curve variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Curve, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            Assert.AreEqual("curve", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.Curve, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            Assert.AreEqual("lcurve", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.Curve, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            Assert.AreEqual("rcurve", convert(arrow));

            // ICurve variants
            arrow = new GraphvizArrow(GraphvizArrowShape.ICurve, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            Assert.AreEqual("icurve", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.ICurve, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            Assert.AreEqual("licurve", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.ICurve, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            Assert.AreEqual("ricurve", convert(arrow));
        }

        [TestCaseSource(nameof(ToDotTestCases))]
        public void ToDot_SkippedModifiers([NotNull, InstantHandle] Func<GraphvizArrow, string> convert)
        {
            // Skipped Crow variants
            var arrow = new GraphvizArrow(GraphvizArrowShape.Crow, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            Assert.AreEqual("crow", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.Crow, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            Assert.AreEqual("lcrow", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.Crow, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            Assert.AreEqual("rcrow", convert(arrow));

            // Skipped Dot variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Dot, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            Assert.AreEqual("dot", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.Dot, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            Assert.AreEqual("dot", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.Dot, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            Assert.AreEqual("odot", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.Dot, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            Assert.AreEqual("odot", convert(arrow));

            // Skipped None variants
            arrow = new GraphvizArrow(GraphvizArrowShape.None, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            Assert.AreEqual("none", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.None, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            Assert.AreEqual("none", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.None, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            Assert.AreEqual("none", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.None, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            Assert.AreEqual("none", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.None, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            Assert.AreEqual("none", convert(arrow));

            // Skipped Tee variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Tee, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            Assert.AreEqual("tee", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.Tee, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            Assert.AreEqual("ltee", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.Tee, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            Assert.AreEqual("rtee", convert(arrow));

            // Skipped Vee variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Vee, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            Assert.AreEqual("vee", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.Vee, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            Assert.AreEqual("lvee", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.Vee, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            Assert.AreEqual("rvee", convert(arrow));

            // Skipped Curve variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Curve, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            Assert.AreEqual("curve", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.Curve, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            Assert.AreEqual("lcurve", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.Curve, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            Assert.AreEqual("rcurve", convert(arrow));

            // Skipped ICurve variants
            arrow = new GraphvizArrow(GraphvizArrowShape.ICurve, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            Assert.AreEqual("icurve", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.ICurve, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            Assert.AreEqual("licurve", convert(arrow));
            arrow = new GraphvizArrow(GraphvizArrowShape.ICurve, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            Assert.AreEqual("ricurve", convert(arrow));
        }
    }
}