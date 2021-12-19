#nullable enable

using JetBrains.Annotations;
using NUnit.Framework;
using FastGraph.Graphviz.Dot;
using static FastGraph.Graphviz.Tests.CultureHelpers;

namespace FastGraph.Graphviz.Tests
{
    /// <summary>
    /// Tests related to <see cref="GraphvizGraph"/>.
    /// </summary>
    [TestFixture]
    internal sealed class GraphvizGraphTests
    {
        [Test]
        public void Constructor()
        {
            var graph = new GraphvizGraph();
            graph.Comment.Should().BeNull();
            graph.Url.Should().BeNull();
            graph.BackgroundColor.Should().Be(GraphvizColor.White);
            graph.ClusterRank.Should().Be(GraphvizClusterMode.Local);
            graph.Font.Should().BeNull();
            graph.FontColor.Should().Be(GraphvizColor.Black);
            graph.PenWidth.Should().Be(1.0);
            graph.IsCentered.Should().BeFalse();
            graph.IsCompounded.Should().BeFalse();
            graph.IsConcentrated.Should().BeFalse();
            graph.IsLandscape.Should().BeFalse();
            graph.IsReMinCross.Should().BeFalse();
            graph.IsHtmlLabel.Should().BeFalse();
            graph.Label.Should().BeNull();
            graph.LabelJustification.Should().Be(GraphvizLabelJustification.C);
            graph.Layers.Should().NotBeNull();
            graph.McLimit.Should().Be(1.0);
            graph.NodeSeparation.Should().Be(0.25);
            graph.RankDirection.Should().Be(GraphvizRankDirection.TB);
            graph.RankSeparation.Should().Be(0.5);
            graph.NsLimit.Should().Be(-1);
            graph.NsLimit1.Should().Be(-1);
            graph.OutputOrder.Should().Be(GraphvizOutputMode.BreadthFirst);
            graph.PageDirection.Should().Be(GraphvizPageDirection.BL);
            graph.PageSize.Should().NotBeNull();
            graph.PageSize.Width.Should().BeApproximately(0, float.Epsilon);
            graph.PageSize.Height.Should().BeApproximately(0, float.Epsilon);
            graph.Quantum.Should().BeApproximately(0, double.Epsilon);
            graph.Ratio.Should().Be(GraphvizRatioMode.Auto);
            graph.Resolution.Should().Be(0.96);
            graph.Rotate.Should().Be(0);
            graph.SamplePoints.Should().Be(8);
            graph.SearchSize.Should().Be(30);
            graph.Size.Should().NotBeNull();
            graph.Size.Width.Should().BeApproximately(0, float.Epsilon);
            graph.Size.Height.Should().BeApproximately(0, float.Epsilon);
            graph.Splines.Should().Be(GraphvizSplineType.Spline);
            graph.StyleSheet.Should().BeNull();
        }

        [Test]
        public void Name()
        {
            var graph = new GraphvizGraph();
            if (graph.Name is null)
                throw new InvalidOperationException($"Graph has default {nameof(GraphvizGraph.Name)}.");

            graph.Name = "GraphName";
            graph.Name.Should().BeSameAs("GraphName");
        }

        [Test]
        public void Name_Throws()
        {
            var graph = new GraphvizGraph();
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => graph.Name = default).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

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

                graph = new GraphvizGraph
                {
                    IsHtmlLabel = true,
                    Label = "<i>\"The Label\"</i>\n &amp;/&lt;&gt;@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\\[]() Content ∴∞⇐ℜΩ÷嗷娪"
                };
                yield return new TestCaseData(
                    graph,
                    @"label=<<i>""The Label""</i>" + '\n' + @" &amp;/&lt;&gt;@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\[]() Content ∴∞⇐ℜΩ÷嗷娪>");
            }
        }

        [TestCaseSource(nameof(ToDotTestCases))]
        public void ToDot(GraphvizGraph graph, string expectedDot)
        {
            graph.ToDot().Should().Be(expectedDot);
            graph.ToString().Should().Be(expectedDot);
        }

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
        public void ToDot_InvariantCulture([InstantHandle] Func<GraphvizGraph, string> convert)
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
                convert(graph).Should().Be(expectedDot);
            }

            using (CultureScope(FrenchCulture))
            {
                convert(graph).Should().Be(expectedDot);
            }
        }
    }
}
