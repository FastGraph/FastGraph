#nullable enable

using JetBrains.Annotations;
using NUnit.Framework;
using FastGraph.Graphviz.Dot;

namespace FastGraph.Graphviz.Tests
{
    /// <summary>
    /// Tests related to <see cref="GraphvizArrow"/>.
    /// </summary>
    [TestFixture]
    internal sealed class GraphvizArrowTests
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
                a.Shape.Should().Be(shape);
                a.Clipping.Should().Be(clipping);
                a.Filling.Should().Be(filling);
            }

            #endregion
        }

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
        public void ToDot([InstantHandle] Func<GraphvizArrow, string> convert)
        {
            // Box variants
            var arrow = new GraphvizArrow(GraphvizArrowShape.Box, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            convert(arrow).Should().Be("box");
            arrow = new GraphvizArrow(GraphvizArrowShape.Box, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            convert(arrow).Should().Be("lbox");
            arrow = new GraphvizArrow(GraphvizArrowShape.Box, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            convert(arrow).Should().Be("rbox");
            arrow = new GraphvizArrow(GraphvizArrowShape.Box, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            convert(arrow).Should().Be("obox");
            arrow = new GraphvizArrow(GraphvizArrowShape.Box, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            convert(arrow).Should().Be("olbox");
            arrow = new GraphvizArrow(GraphvizArrowShape.Box, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            convert(arrow).Should().Be("orbox");

            // Crow variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Crow, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            convert(arrow).Should().Be("crow");
            arrow = new GraphvizArrow(GraphvizArrowShape.Crow, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            convert(arrow).Should().Be("lcrow");
            arrow = new GraphvizArrow(GraphvizArrowShape.Crow, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            convert(arrow).Should().Be("rcrow");

            // Diamond variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Diamond, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            convert(arrow).Should().Be("diamond");
            arrow = new GraphvizArrow(GraphvizArrowShape.Diamond, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            convert(arrow).Should().Be("ldiamond");
            arrow = new GraphvizArrow(GraphvizArrowShape.Diamond, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            convert(arrow).Should().Be("rdiamond");
            arrow = new GraphvizArrow(GraphvizArrowShape.Diamond, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            convert(arrow).Should().Be("odiamond");
            arrow = new GraphvizArrow(GraphvizArrowShape.Diamond, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            convert(arrow).Should().Be("oldiamond");
            arrow = new GraphvizArrow(GraphvizArrowShape.Diamond, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            convert(arrow).Should().Be("ordiamond");

            // Dot variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Dot, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            convert(arrow).Should().Be("dot");
            arrow = new GraphvizArrow(GraphvizArrowShape.Dot, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            convert(arrow).Should().Be("odot");

            // Inv variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Inv, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            convert(arrow).Should().Be("inv");
            arrow = new GraphvizArrow(GraphvizArrowShape.Inv, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            convert(arrow).Should().Be("linv");
            arrow = new GraphvizArrow(GraphvizArrowShape.Inv, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            convert(arrow).Should().Be("rinv");
            arrow = new GraphvizArrow(GraphvizArrowShape.Inv, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            convert(arrow).Should().Be("oinv");
            arrow = new GraphvizArrow(GraphvizArrowShape.Inv, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            convert(arrow).Should().Be("olinv");
            arrow = new GraphvizArrow(GraphvizArrowShape.Inv, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            convert(arrow).Should().Be("orinv");

            // None
            arrow = new GraphvizArrow(GraphvizArrowShape.None, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            convert(arrow).Should().Be("none");

            // Normal variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Normal, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            convert(arrow).Should().Be("normal");
            arrow = new GraphvizArrow(GraphvizArrowShape.Normal, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            convert(arrow).Should().Be("lnormal");
            arrow = new GraphvizArrow(GraphvizArrowShape.Normal, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            convert(arrow).Should().Be("rnormal");
            arrow = new GraphvizArrow(GraphvizArrowShape.Normal, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            convert(arrow).Should().Be("onormal");
            arrow = new GraphvizArrow(GraphvizArrowShape.Normal, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            convert(arrow).Should().Be("olnormal");
            arrow = new GraphvizArrow(GraphvizArrowShape.Normal, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            convert(arrow).Should().Be("ornormal");

            // Tee variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Tee, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            convert(arrow).Should().Be("tee");
            arrow = new GraphvizArrow(GraphvizArrowShape.Tee, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            convert(arrow).Should().Be("ltee");
            arrow = new GraphvizArrow(GraphvizArrowShape.Tee, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            convert(arrow).Should().Be("rtee");

            // Vee variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Vee, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            convert(arrow).Should().Be("vee");
            arrow = new GraphvizArrow(GraphvizArrowShape.Vee, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            convert(arrow).Should().Be("lvee");
            arrow = new GraphvizArrow(GraphvizArrowShape.Vee, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            convert(arrow).Should().Be("rvee");

            // Curve variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Curve, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            convert(arrow).Should().Be("curve");
            arrow = new GraphvizArrow(GraphvizArrowShape.Curve, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            convert(arrow).Should().Be("lcurve");
            arrow = new GraphvizArrow(GraphvizArrowShape.Curve, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            convert(arrow).Should().Be("rcurve");

            // ICurve variants
            arrow = new GraphvizArrow(GraphvizArrowShape.ICurve, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            convert(arrow).Should().Be("icurve");
            arrow = new GraphvizArrow(GraphvizArrowShape.ICurve, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            convert(arrow).Should().Be("licurve");
            arrow = new GraphvizArrow(GraphvizArrowShape.ICurve, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            convert(arrow).Should().Be("ricurve");
        }

        [TestCaseSource(nameof(ToDotTestCases))]
        public void ToDot_SkippedModifiers([InstantHandle] Func<GraphvizArrow, string> convert)
        {
            // Skipped Crow variants
            var arrow = new GraphvizArrow(GraphvizArrowShape.Crow, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            convert(arrow).Should().Be("crow");
            arrow = new GraphvizArrow(GraphvizArrowShape.Crow, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            convert(arrow).Should().Be("lcrow");
            arrow = new GraphvizArrow(GraphvizArrowShape.Crow, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            convert(arrow).Should().Be("rcrow");

            // Skipped Dot variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Dot, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            convert(arrow).Should().Be("dot");
            arrow = new GraphvizArrow(GraphvizArrowShape.Dot, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            convert(arrow).Should().Be("dot");
            arrow = new GraphvizArrow(GraphvizArrowShape.Dot, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            convert(arrow).Should().Be("odot");
            arrow = new GraphvizArrow(GraphvizArrowShape.Dot, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            convert(arrow).Should().Be("odot");

            // Skipped None variants
            arrow = new GraphvizArrow(GraphvizArrowShape.None, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            convert(arrow).Should().Be("none");
            arrow = new GraphvizArrow(GraphvizArrowShape.None, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            convert(arrow).Should().Be("none");
            arrow = new GraphvizArrow(GraphvizArrowShape.None, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            convert(arrow).Should().Be("none");
            arrow = new GraphvizArrow(GraphvizArrowShape.None, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            convert(arrow).Should().Be("none");
            arrow = new GraphvizArrow(GraphvizArrowShape.None, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            convert(arrow).Should().Be("none");

            // Skipped Tee variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Tee, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            convert(arrow).Should().Be("tee");
            arrow = new GraphvizArrow(GraphvizArrowShape.Tee, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            convert(arrow).Should().Be("ltee");
            arrow = new GraphvizArrow(GraphvizArrowShape.Tee, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            convert(arrow).Should().Be("rtee");

            // Skipped Vee variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Vee, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            convert(arrow).Should().Be("vee");
            arrow = new GraphvizArrow(GraphvizArrowShape.Vee, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            convert(arrow).Should().Be("lvee");
            arrow = new GraphvizArrow(GraphvizArrowShape.Vee, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            convert(arrow).Should().Be("rvee");

            // Skipped Curve variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Curve, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            convert(arrow).Should().Be("curve");
            arrow = new GraphvizArrow(GraphvizArrowShape.Curve, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            convert(arrow).Should().Be("lcurve");
            arrow = new GraphvizArrow(GraphvizArrowShape.Curve, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            convert(arrow).Should().Be("rcurve");

            // Skipped ICurve variants
            arrow = new GraphvizArrow(GraphvizArrowShape.ICurve, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            convert(arrow).Should().Be("icurve");
            arrow = new GraphvizArrow(GraphvizArrowShape.ICurve, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            convert(arrow).Should().Be("licurve");
            arrow = new GraphvizArrow(GraphvizArrowShape.ICurve, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            convert(arrow).Should().Be("ricurve");
        }
    }
}
