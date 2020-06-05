using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Graphviz.Dot;
using static QuikGraph.Graphviz.Tests.CultureHelpers;

namespace QuikGraph.Graphviz.Tests
{
    /// <summary>
    /// Tests related to <see cref="GraphvizGraph"/>.
    /// </summary>
    [TestFixture]
    internal class GraphvizGraphTests
    {
        [Test]
        public void Constructor()
        {
            var graph = new GraphvizGraph();
            Assert.IsNull(graph.Comment);
            Assert.IsNull(graph.Url);
            Assert.AreEqual(GraphvizColor.White, graph.BackgroundColor);
            Assert.AreEqual(GraphvizClusterMode.Local, graph.ClusterRank);
            Assert.IsNull(graph.Font);
            Assert.AreEqual(GraphvizColor.Black, graph.FontColor);
            Assert.AreEqual(1.0, graph.PenWidth);
            Assert.IsFalse(graph.IsCentered);
            Assert.IsFalse(graph.IsCompounded);
            Assert.IsFalse(graph.IsConcentrated);
            Assert.IsFalse(graph.IsLandscape);
            Assert.IsFalse(graph.IsReMinCross);
            Assert.IsNull(graph.Label);
            Assert.AreEqual(GraphvizLabelJustification.C, graph.LabelJustification);
            Assert.IsNotNull(graph.Layers);
            Assert.AreEqual(1.0, graph.McLimit);
            Assert.AreEqual(0.25, graph.NodeSeparation);
            Assert.AreEqual(GraphvizRankDirection.TB, graph.RankDirection);
            Assert.AreEqual(0.5, graph.RankSeparation);
            Assert.AreEqual(-1, graph.NsLimit);
            Assert.AreEqual(-1, graph.NsLimit1);
            Assert.AreEqual(GraphvizOutputMode.BreadthFirst, graph.OutputOrder);
            Assert.AreEqual(GraphvizPageDirection.BL, graph.PageDirection);
            Assert.IsNotNull(graph.PageSize);
            Assert.Zero(graph.PageSize.Width);
            Assert.Zero(graph.PageSize.Height);
            Assert.Zero(graph.Quantum);
            Assert.AreEqual(GraphvizRatioMode.Auto, graph.Ratio);
            Assert.AreEqual(0.96, graph.Resolution);
            Assert.Zero(graph.Rotate);
            Assert.AreEqual(8, graph.SamplePoints);
            Assert.AreEqual(30, graph.SearchSize);
            Assert.IsNotNull(graph.Size);
            Assert.Zero(graph.Size.Width);
            Assert.Zero(graph.Size.Height);
            Assert.AreEqual(GraphvizSplineType.Spline, graph.Splines);
            Assert.IsNull(graph.StyleSheet);
        }

        [Test]
        public void Name()
        {
            var graph = new GraphvizGraph();
            if (graph.Name is null)
                throw new InvalidOperationException($"Graph has null {nameof(GraphvizGraph.Name)}.");

            graph.Name = "GraphName";
            Assert.AreSame("GraphName", graph.Name);
        }

        [Test]
        public void Name_Throws()
        {
            var graph = new GraphvizGraph();
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.Name = null);
        }

        [NotNull, ItemNotNull]
        private static IEnumerable<TestCaseData> ToDotTestCases
        {
            get
            {
                var graph = new GraphvizGraph();
                yield return new TestCaseData(graph, string.Empty);

                graph = new GraphvizGraph
                {
                    Comment = "Test comment"
                };
                yield return new TestCaseData(graph, @"comment=""Test comment""");

                graph = new GraphvizGraph
                {
                    Url = "https://test.fr",
                    PageDirection = GraphvizPageDirection.LB
                };
                yield return new TestCaseData(graph, @"URL=""https://test.fr""; pagedir=LB;");

                graph = new GraphvizGraph
                {
                    IsNormalized = true,
                    NodeSeparation = 2,
                    RankSeparation = 2,
                    BackgroundColor = GraphvizColor.Coral
                };
                yield return new TestCaseData(graph, @"bgcolor=""#FF7F50FF""; nodesep=2; ranksep=2; normalize=true;");

                graph = new GraphvizGraph
                {
                    Name = "GraphName", // Not written there
                    Comment = "Test comment",
                    Url = "https://test.com",
                    BackgroundColor = GraphvizColor.Bisque,
                    ClusterRank = GraphvizClusterMode.Global,
                    Font = new GraphvizFont("Test font", 12),
                    FontColor = GraphvizColor.DarkOrange,
                    PenWidth = 2.0,
                    IsCentered = true,
                    IsCompounded = true,
                    IsConcentrated = true,
                    IsNormalized = true,
                    IsReMinCross = true,
                    Label = "Test label",
                    LabelJustification = GraphvizLabelJustification.L,
                    LabelLocation = GraphvizLabelLocation.T,
                    Layers = { new GraphvizLayer("Layer1"), new GraphvizLayer("Layer2") },
                    McLimit = 2.0,
                    NodeSeparation = 1.0,
                    RankDirection = GraphvizRankDirection.LR,
                    RankSeparation = 2.0,
                    NsLimit = 2,
                    NsLimit1 = 3,
                    OutputOrder = GraphvizOutputMode.NodesFirst,
                    PageDirection = GraphvizPageDirection.RT,
                    PageSize = new GraphvizSizeF(10.0f, 15.0f),
                    Quantum = 1.0,
                    Ratio = GraphvizRatioMode.Fill,
                    Resolution = 1,
                    SamplePoints = 9,
                    SearchSize = 25,
                    Size = new GraphvizSizeF(25.0f, 45.0f),
                    Splines = GraphvizSplineType.Curved,
                    StyleSheet = "stylesheet.xml"
                };
                graph.Layers.Separators = ":-:";
                yield return new TestCaseData(
                    graph,
                    @"URL=""https://test.com""; bgcolor=""#FFE4C4FF""; center=true; clusterrank=""global""; "
                    + @"comment=""Test comment""; compound=true; concentrate=true; fontname=""Test font""; "
                    + @"fontsize=12; fontcolor=""#FF8C00FF""; penwidth=2; label=""Test label""; labeljust=""l""; "
                    + @"labelloc=""t""; layers=""Layer1:-:Layer2""; layersep="":-:""; mclimit=2; "
                    + @"nodesep=1; rankdir=LR; ranksep=2; normalize=true; nslimit=2; nslimit1=3; "
                    + @"outputorder=""nodesfirst""; page=""10,15""; pagedir=RT; quantum=1; ratio=""fill""; "
                    + @"remincross=true; resolution=1; samplepoints=9; searchsize=25; size=""25,45""; "
                    + @"splines=curved; stylesheet=""stylesheet.xml"";");

                // Orientation
                graph = new GraphvizGraph
                {
                    Rotate = 12
                };
                yield return new TestCaseData(graph, "rotate=12");

                graph = new GraphvizGraph
                {
                    IsLandscape = true
                };
                yield return new TestCaseData(graph, @"orientation=""[1L]*""");

                graph = new GraphvizGraph
                {
                    Rotate = 14,    // Priority to rotation over landscape (rotate 90)
                    IsLandscape = true
                };
                yield return new TestCaseData(graph, @"rotate=14");

                graph = new GraphvizGraph
                {
                    Comment = "\"The Comment\"\n &/<>@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\\[]() Content ∴∞⇐ℜΩ÷嗷娪",
                    Label = "\"The Label\"\n &/<>@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\\[]() Content ∴∞⇐ℜΩ÷嗷娪"
                };
                yield return new TestCaseData(
                    graph,
                    @"comment=""\""The Comment\""\n &/<>@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\\[]() Content ∴∞⇐ℜΩ÷嗷娪""; "
                    + @"label=""\""The Label\""\n &/<>@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\\[]() Content ∴∞⇐ℜΩ÷嗷娪"";");
            }
        }

        [TestCaseSource(nameof(ToDotTestCases))]
        public void ToDot([NotNull] GraphvizGraph graph, [NotNull] string expectedDot)
        {
            Assert.AreEqual(expectedDot, graph.ToDot());
            Assert.AreEqual(expectedDot, graph.ToString());
        }

        [NotNull, ItemNotNull]
        private static IEnumerable<TestCaseData> ToDotCultureInvariantTestCases
        {
            get
            {
                Func<GraphvizGraph, string> toDot = graph => graph.ToDot();
                yield return new TestCaseData(toDot);

                Func<GraphvizGraph, string> toString = graph => graph.ToString();
                yield return new TestCaseData(toString);
            }
        }

        [TestCaseSource(nameof(ToDotCultureInvariantTestCases))]
        public void ToDot_InvariantCulture([NotNull, InstantHandle] Func<GraphvizGraph, string> convert)
        {
            var graph = new GraphvizGraph
            {
                Font = new GraphvizFont("Test font", 12.5f),
                PenWidth = 2.5,
                McLimit = 1.5,
                NodeSeparation = 0.5,
                RankSeparation = 0.75,
                PageSize = new GraphvizSizeF(500.5f, 425.6f),
                Quantum = 1.5,
                Resolution = 0.95,
                Size = new GraphvizSizeF(350.5f, 260.4f)
            };

            const string expectedDot =
                @"fontname=""Test font""; fontsize=12.5; penwidth=2.5; mclimit=1.5; nodesep=0.5; ranksep=0.75; "
                + @"page=""500.5,425.6""; quantum=1.5; resolution=0.95; size=""350.5,260.4"";";

            using (CultureScope(EnglishCulture))
            {
                Assert.AreEqual(expectedDot, convert(graph));
            }

            using (CultureScope(FrenchCulture))
            {
                Assert.AreEqual(expectedDot, convert(graph));
            }
        }
    }
}