using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Graphviz.Dot;

namespace QuikGraph.Graphviz.Tests
{
    /// <summary>
    /// Tests for <see cref="GraphvizEdgeExtremity"/>.
    /// </summary>
    [TestFixture]
    internal class GraphvizEdgeExtremityTests
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
                Assert.AreEqual(head, extremity.IsHead);
                Assert.IsTrue(extremity.IsClipped);
                Assert.IsNull(extremity.Url);
                Assert.IsNull(extremity.Label);
                Assert.IsNull(extremity.ToolTip);
                Assert.IsNull(extremity.Logical);
                Assert.IsNull(extremity.Same);
            }

            #endregion
        }

        [NotNull, ItemNotNull]
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
            [NotNull] GraphvizEdgeExtremity extremity,
            [NotNull] Dictionary<string, object> expectedParameters)
        {
            var parameters = new Dictionary<string, object>();
            extremity.AddParameters(parameters);
            CollectionAssert.AreEquivalent(expectedParameters, parameters);
        }

        [Test]
        public void AddParameters_Throws()
        {
            // ReSharper disable AssignNullToNotNullAttribute
            var extremity = new GraphvizEdgeExtremity(false);
            Assert.Throws<ArgumentNullException>(() => extremity.AddParameters(null));
            extremity = new GraphvizEdgeExtremity(true);
            Assert.Throws<ArgumentNullException>(() => extremity.AddParameters(null));
            // ReSharper restore AssignNullToNotNullAttribute
        }
    }
}