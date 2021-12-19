#nullable enable

using NUnit.Framework;
using FastGraph.Graphviz.Dot;

namespace FastGraph.Graphviz.Tests
{
    /// <summary>
    /// Tests for <see cref="GraphvizEdgeLabel"/>.
    /// </summary>
    [TestFixture]
    internal sealed class GraphvizEdgeLabelTests
    {
        [Test]
        public void Constructor()
        {
            var edgeLabel = new GraphvizEdgeLabel();
            edgeLabel.Angle.Should().Be(-25.0);
            edgeLabel.Distance.Should().Be(1.0);
            edgeLabel.Float.Should().BeTrue();
            edgeLabel.Font.Should().BeNull();
            edgeLabel.FontColor.Should().Be(GraphvizColor.Black);
            edgeLabel.IsHtmlLabel.Should().BeFalse();
            edgeLabel.Value.Should().BeNull();
        }

        private static IEnumerable<TestCaseData> AddParametersTestCases
        {
            get
            {
                var label = new GraphvizEdgeLabel();
                yield return new TestCaseData(
                    label,
                    new Dictionary<string, object>());

                label = new GraphvizEdgeLabel
                {
                    FontColor = GraphvizColor.Azure
                };
                yield return new TestCaseData(
                    label,
                    new Dictionary<string, object>());

                label = new GraphvizEdgeLabel
                {
                    Value = "Test Label",
                    FontColor = GraphvizColor.Azure
                };
                yield return new TestCaseData(
                    label,
                    new Dictionary<string, object>
                    {
                        ["label"] = "Test Label",
                        ["labelfontcolor"] = GraphvizColor.Azure
                    });

                label = new GraphvizEdgeLabel
                {
                    Value = "The Demo Label",
                    Angle = 35.5,
                    Distance = 2.0,
                    Float = false,
                    Font = new GraphvizFont("Test font", 16.0f),
                    FontColor = GraphvizColor.BlueViolet
                };
                yield return new TestCaseData(
                    label,
                    new Dictionary<string, object>
                    {
                        ["label"] = "The Demo Label",
                        ["labelangle"] = 35.5,
                        ["labeldistance"] = 2.0,
                        ["labelfloat"] = false,
                        ["labelfontname"] = "Test font",
                        ["labelfontsize"] = 16.0f,
                        ["labelfontcolor"] = GraphvizColor.BlueViolet
                    });

                label = new GraphvizEdgeLabel
                {
                    Value = "\"The Label\"\n &/<>@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\\[]() Content ∴∞⇐ℜΩ÷嗷娪"
                };
                yield return new TestCaseData(
                    label,
                    new Dictionary<string, object>
                    {
                        ["label"] = @"\""The Label\""\n &/<>@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\\[]() Content ∴∞⇐ℜΩ÷嗷娪"
                    });
            }
        }

        [TestCaseSource(nameof(AddParametersTestCases))]
        public void AddParameters(
            GraphvizEdgeLabel label,
            Dictionary<string, object> expectedParameters)
        {
            var parameters = new Dictionary<string, object>();
            label.AddParameters(parameters);
            parameters.Should().BeEquivalentTo(expectedParameters);
        }

        [Test]
        public void AddParameters_Throws()
        {
            var extremity = new GraphvizEdgeLabel();
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => extremity.AddParameters(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }
    }
}
