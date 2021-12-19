#nullable enable

using NUnit.Framework;
using FastGraph.Graphviz.Dot;

namespace FastGraph.Graphviz.Tests
{
    /// <summary>
    /// Tests for <see cref="GraphvizEdgeExtremity"/>.
    /// </summary>
    [TestFixture]
    internal sealed class GraphvizEdgeExtremityTests
    {
        [Test]
        public void Constructor()
        {
            var edgeExtremity = new GraphvizEdgeExtremity(false);
            CheckEdgeExtremity(edgeExtremity, false);

            edgeExtremity = new GraphvizEdgeExtremity(true);
            CheckEdgeExtremity(edgeExtremity, true);

            #region Local function

            void CheckEdgeExtremity(
                GraphvizEdgeExtremity extremity,
                bool head)
            {
                extremity.IsHead.Should().Be(head);
                extremity.IsClipped.Should().BeTrue();
                extremity.Url.Should().BeNull();
                extremity.IsHtmlLabel.Should().BeFalse();
                extremity.Label.Should().BeNull();
                extremity.ToolTip.Should().BeNull();
                extremity.Logical.Should().BeNull();
                extremity.Same.Should().BeNull();
            }

            #endregion
        }

        private static IEnumerable<TestCaseData> AddParametersTestCases
        {
            get
            {
                var extremity = new GraphvizEdgeExtremity(true);
                yield return new TestCaseData(
                    extremity,
                    new Dictionary<string, object>());

                extremity = new GraphvizEdgeExtremity(false);
                yield return new TestCaseData(
                    extremity,
                    new Dictionary<string, object>());

                extremity = new GraphvizEdgeExtremity(true)
                {
                    IsClipped = false
                };
                yield return new TestCaseData(
                    extremity,
                    new Dictionary<string, object>
                    {
                        ["headclip"] = false
                    });

                extremity = new GraphvizEdgeExtremity(false)
                {
                    IsClipped = false
                };
                yield return new TestCaseData(
                    extremity,
                    new Dictionary<string, object>
                    {
                        ["tailclip"] = false
                    });

                extremity = new GraphvizEdgeExtremity(true)
                {
                    Url = "http://test.com",
                    Label = "Test Head"
                };
                yield return new TestCaseData(
                    extremity,
                    new Dictionary<string, object>
                    {
                        ["headURL"] = "http://test.com",
                        ["headlabel"] = "Test Head"
                    });

                extremity = new GraphvizEdgeExtremity(false)
                {
                    Url = "http://test.com",
                    Label = "Test Tail"
                };
                yield return new TestCaseData(
                    extremity,
                    new Dictionary<string, object>
                    {
                        ["tailURL"] = "http://test.com",
                        ["taillabel"] = "Test Tail"
                    });

                extremity = new GraphvizEdgeExtremity(true)
                {
                    Url = "http://test.url.com",
                    IsClipped = false,
                    Label = "TheHead",
                    ToolTip = "Head tooltip",
                    Logical = "Head logical",
                    Same = "SameHead"
                };
                yield return new TestCaseData(
                    extremity,
                    new Dictionary<string, object>
                    {
                        ["headURL"] = "http://test.url.com",
                        ["headclip"] = false,
                        ["headlabel"] = "TheHead",
                        ["headtooltip"] = "Head tooltip",
                        ["lhead"] = "Head logical",
                        ["samehead"] = "SameHead"
                    });

                extremity = new GraphvizEdgeExtremity(false)
                {
                    Url = "http://test.url.com",
                    IsClipped = false,
                    Label = "TheTail",
                    ToolTip = "Tail tooltip",
                    Logical = "Tail logical",
                    Same = "SameTail"
                };
                yield return new TestCaseData(
                    extremity,
                    new Dictionary<string, object>
                    {
                        ["tailURL"] = "http://test.url.com",
                        ["tailclip"] = false,
                        ["taillabel"] = "TheTail",
                        ["tailtooltip"] = "Tail tooltip",
                        ["ltail"] = "Tail logical",
                        ["sametail"] = "SameTail"
                    });

                // With escape
                extremity = new GraphvizEdgeExtremity(true)
                {
                    Label = "\"The Head\"\nLabel &/<>@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\\[]() Content ∴∞⇐ℜΩ÷嗷娪",
                    ToolTip = "\"The Head\"\nTooltip &/<>@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\\[]() Content ∴∞⇐ℜΩ÷嗷娪"
                };
                yield return new TestCaseData(
                    extremity,
                    new Dictionary<string, object>
                    {
                        ["headlabel"] = @"\""The Head\""\nLabel &/<>@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\\[]() Content ∴∞⇐ℜΩ÷嗷娪",
                        ["headtooltip"] = @"\""The Head\""\nTooltip &/<>@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\\[]() Content ∴∞⇐ℜΩ÷嗷娪"
                    });
            }
        }

        [TestCaseSource(nameof(AddParametersTestCases))]
        public void AddParameters(
            GraphvizEdgeExtremity extremity,
            Dictionary<string, object> expectedParameters)
        {
            var parameters = new Dictionary<string, object>();
            extremity.AddParameters(parameters);
            parameters.Should().BeEquivalentTo(expectedParameters);
        }

        [Test]
        public void AddParameters_Throws()
        {
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            var extremity = new GraphvizEdgeExtremity(false);
            Invoking(() => extremity.AddParameters(default)).Should().Throw<ArgumentNullException>();
            extremity = new GraphvizEdgeExtremity(true);
            Invoking(() => extremity.AddParameters(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
        }
    }
}
