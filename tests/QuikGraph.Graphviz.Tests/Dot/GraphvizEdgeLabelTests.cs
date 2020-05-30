using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Graphviz.Dot;

namespace QuikGraph.Graphviz.Tests
{
    /// <summary>
    /// Tests for <see cref="GraphvizEdgeLabel"/>.
    /// </summary>
    [TestFixture]
    internal class GraphvizEdgeLabelTests
    {
        [Test]
        public void Constructor()
        {
            var edgeLabel = new GraphvizEdgeLabel();
            Assert.AreEqual(-25.0, edgeLabel.Angle);
            Assert.AreEqual(1.0, edgeLabel.Distance);
            Assert.IsTrue(edgeLabel.Float);
            Assert.IsNull(edgeLabel.Font);
            Assert.AreEqual(GraphvizColor.Black, edgeLabel.FontColor);
            Assert.IsNull(edgeLabel.Value);
        }

        [NotNull, ItemNotNull]
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
            [NotNull] GraphvizEdgeLabel label,
            [NotNull] Dictionary<string, object> expectedParameters)
        {
            var parameters = new Dictionary<string, object>();
            label.AddParameters(parameters);
            CollectionAssert.AreEquivalent(expectedParameters, parameters);
        }

        [Test]
        public void AddParameters_Throws()
        {
            var extremity = new GraphvizEdgeLabel();
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => extremity.AddParameters(null));
        }
    }
}