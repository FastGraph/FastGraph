#nullable enable

using JetBrains.Annotations;
using NUnit.Framework;
using FastGraph.Graphviz.Dot;
using static FastGraph.Graphviz.Tests.CultureHelpers;

namespace FastGraph.Graphviz.Tests
{
    /// <summary>
    /// Tests related to <see cref="GraphvizVertex"/>.
    /// </summary>
    [TestFixture]
    internal sealed class GraphvizVertexTests
    {
        [Test]
        public void Constructor()
        {
            var vertex = new GraphvizVertex();
            vertex.Position.Should().BeNull();
            vertex.Comment.Should().BeNull();
            vertex.IsHtmlLabel.Should().BeFalse();
            vertex.Label.Should().BeNull();
            vertex.ToolTip.Should().BeNull();
            vertex.Url.Should().BeNull();
            vertex.Distortion.Should().BeApproximately(0, double.Epsilon);
            vertex.FillColor.Should().Be(GraphvizColor.White);
            vertex.Font.Should().BeNull();
            vertex.FontColor.Should().Be(GraphvizColor.Black);
            vertex.PenWidth.Should().Be(1.0);
            vertex.Group.Should().BeNull();
            vertex.Layer.Should().BeNull();
            vertex.Orientation.Should().BeApproximately(0, double.Epsilon);
            vertex.Peripheries.Should().Be(-1);
            vertex.Regular.Should().BeFalse();
            vertex.Record.Should().NotBeNull();
            vertex.Shape.Should().Be(GraphvizVertexShape.Unspecified);
            vertex.Sides.Should().Be(4);
            vertex.Size.Should().NotBeNull();
            vertex.Size.Width.Should().BeApproximately(0, float.Epsilon);
            vertex.Size.Height.Should().BeApproximately(0, float.Epsilon);
            vertex.FixedSize.Should().BeFalse();
            vertex.Skew.Should().BeApproximately(0, double.Epsilon);
            vertex.StrokeColor.Should().Be(GraphvizColor.Black);
            vertex.Style.Should().Be(GraphvizVertexStyle.Unspecified);
            vertex.Z.Should().Be(-1);
        }

        private static IEnumerable<TestCaseData> ToDotTestCases
        {
            get
            {
                var vertex = new GraphvizVertex();
                yield return new TestCaseData(vertex, string.Empty);

                vertex = new GraphvizVertex
                {
                    Label = "Test Vertex"
                };
                yield return new TestCaseData(vertex, @"label=""Test Vertex""");

                vertex = new GraphvizVertex
                {
                    Shape = GraphvizVertexShape.DoubleCircle,
                    Style = GraphvizVertexStyle.Filled,
                    FillColor = GraphvizColor.Blue
                };
                yield return new TestCaseData(vertex, @"shape=doublecircle, style=filled, fillcolor=""#0000FFFF""");

                vertex = new GraphvizVertex
                {
                    Label = "<b>Bold</b> text"
                };
                yield return new TestCaseData(vertex, @"label=""<b>Bold</b> text""");

                vertex = new GraphvizVertex
                {
                    IsHtmlLabel = true,
                    Label = "<b>Bold</b> text"
                };
                yield return new TestCaseData(vertex, @"label=<<b>Bold</b> text>");

                vertex = new GraphvizVertex
                {
                    Position = new GraphvizPoint(10, -20),
                    Comment = "Test comment",
                    Label = "Test label",
                    ToolTip = "Test tooltip",
                    Url = "https://test.com",
                    FillColor = GraphvizColor.Red,
                    Font = new GraphvizFont("Test font", 12.0f),
                    FontColor = GraphvizColor.Green,
                    PenWidth = 2.0,
                    Group = "Test group",
                    Layer = new GraphvizLayer("Vertex Layer"),
                    Orientation = 12,
                    Peripheries = 1,
                    Regular = true,
                    Shape = GraphvizVertexShape.Diamond,
                    StrokeColor = GraphvizColor.Azure,
                    Style = GraphvizVertexStyle.Dashed,
                    Z = 100
                };
                yield return new TestCaseData(
                    vertex,
                    @"fontname=""Test font"", fontsize=12, fontcolor=""#008000FF"", penwidth=2, tooltip=""Test tooltip"", "
                    + @"comment=""Test comment"", URL=""https://test.com"", shape=diamond, label=""Test label"", "
                    + @"style=dashed, color=""#F0FFFFFF"", fillcolor=""#FF0000FF"", orientation=12, regular=true, "
                    + @"group=""Test group"", layer=""Vertex Layer"", peripheries=1, "
                    + @"z=100, pos=""10,-20!""");

                // Not fixed size
                vertex = new GraphvizVertex
                {
                    FixedSize = false,
                    Size = new GraphvizSizeF(10.0f, 15.0f)
                };
                yield return new TestCaseData(vertex, string.Empty);

                // Fixed size
                vertex = new GraphvizVertex
                {
                    FixedSize = true,
                    Size = new GraphvizSizeF(10.0f, 15.0f)
                };
                yield return new TestCaseData(vertex, "fixedsize=true, height=15, width=10");

                vertex = new GraphvizVertex
                {
                    FixedSize = true,
                    Size = new GraphvizSizeF(0.0f, 15.0f)
                };
                yield return new TestCaseData(vertex, "fixedsize=true, height=15");

                vertex = new GraphvizVertex
                {
                    FixedSize = true,
                    Size = new GraphvizSizeF(10.0f, 0.0f)
                };
                yield return new TestCaseData(vertex, "fixedsize=true, width=10");

                // Polygon specific properties
                vertex = new GraphvizVertex
                {
                    Shape = GraphvizVertexShape.Box,
                    Sides = 8,
                    Skew = 2.0,
                    Distortion = 2.0
                };
                yield return new TestCaseData(vertex, "shape=box");

                vertex = new GraphvizVertex
                {
                    Shape = GraphvizVertexShape.Polygon,
                    Sides = 8,
                    Skew = 2.0,
                    Distortion = 2.0
                };
                yield return new TestCaseData(vertex, @"shape=polygon, sides=8, skew=2, distortion=2");

                vertex = new GraphvizVertex
                {
                    Shape = GraphvizVertexShape.Polygon,
                    Skew = 2.0,
                    Distortion = 2.0
                };
                yield return new TestCaseData(vertex, @"shape=polygon, sides=4, skew=2, distortion=2");

                vertex = new GraphvizVertex
                {
                    Shape = GraphvizVertexShape.Polygon,
                    Sides = 0,
                    Skew = 2.0,
                    Distortion = 2.0
                };
                yield return new TestCaseData(vertex, @"shape=polygon, skew=2, distortion=2");

                vertex = new GraphvizVertex
                {
                    Shape = GraphvizVertexShape.Polygon,
                    Sides = 8,
                    Distortion = 2.0
                };
                yield return new TestCaseData(vertex, @"shape=polygon, sides=8, distortion=2");

                vertex = new GraphvizVertex
                {
                    Shape = GraphvizVertexShape.Polygon,
                    Sides = 8,
                    Skew = 2.0
                };
                yield return new TestCaseData(vertex, @"shape=polygon, sides=8, skew=2");

                // With record
                vertex = new GraphvizVertex
                {
                    Shape = GraphvizVertexShape.Rectangle,
                    Label = "Test label",
                    Record = new GraphvizRecord
                    {
                        Cells = new GraphvizRecordCellCollection(new[]
                        {
                            new GraphvizRecordCell
                            {
                                Text = "Test Cell"
                            }
                        })
                    }
                };
                yield return new TestCaseData(vertex, @"shape=rectangle, label=""Test label""");

                vertex = new GraphvizVertex
                {
                    Shape = GraphvizVertexShape.Record,
                    Label = "Test label",   // => priority to label
                    Record = new GraphvizRecord
                    {
                        Cells = new GraphvizRecordCellCollection(new[]
                        {
                            new GraphvizRecordCell
                            {
                                Text = "Test Cell"
                            }
                        })
                    }
                };
                yield return new TestCaseData(vertex, @"shape=record, label=""Test label""");

                vertex = new GraphvizVertex
                {
                    Shape = GraphvizVertexShape.Record,
                    Label = "Start\\ Cell | { <Port_A> a | b | c }",    // => priority to label
                    Record = new GraphvizRecord
                    {
                        Cells = new GraphvizRecordCellCollection(new[]
                        {
                            new GraphvizRecordCell
                            {
                                Text = "Test Cell"
                            }
                        })
                    }
                };
                yield return new TestCaseData(vertex, @"shape=record, label=""Start\ Cell | { <Port_A> a | b | c }""");

                vertex = new GraphvizVertex
                {
                    Shape = GraphvizVertexShape.Record,
                    Record = new GraphvizRecord
                    {
                        Cells = new GraphvizRecordCellCollection(new[]
                        {
                            new GraphvizRecordCell
                            {
                                Text = "Test Cell1"
                            },
                            new GraphvizRecordCell
                            {
                                Port = "Test Port2",
                                Text = "Test Cell2"
                            },
                            new GraphvizRecordCell
                            {
                                Cells = new GraphvizRecordCellCollection(new[]
                                {
                                    new GraphvizRecordCell
                                    {
                                        Port = "Sub Test Port1",
                                        Text = "Sub Test Cell1"
                                    },
                                    new GraphvizRecordCell
                                    {
                                        Port = "Sub Test Port2",
                                        Text = "Sub Test Cell2"
                                    }
                                })
                            }
                        })
                    }
                };
                yield return new TestCaseData(vertex, @"shape=record, label=""Test\ Cell1 | <Test_Port2> Test\ Cell2 | { <Sub_Test_Port1> Sub\ Test\ Cell1 | <Sub_Test_Port2> Sub\ Test\ Cell2 }""");

                // With escape
                vertex = new GraphvizVertex
                {
                    Comment = "\"The Comment\"\n &/<>@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\\[]() Content ∴∞⇐ℜΩ÷嗷娪",
                    Label = "\"The Label\"\n &/<>@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\\[]() Content ∴∞⇐ℜΩ÷嗷娪",
                    ToolTip = "\"The Tooltip\"\n &/<>@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\\[]() Content ∴∞⇐ℜΩ÷嗷娪"
                };
                yield return new TestCaseData(
                    vertex,
                    @"tooltip=""\""The Tooltip\""\n &/<>@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\\[]() Content ∴∞⇐ℜΩ÷嗷娪"", "
                    + @"comment=""\""The Comment\""\n &/<>@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\\[]() Content ∴∞⇐ℜΩ÷嗷娪"", "
                    + @"label=""\""The Label\""\n &/<>@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\\[]() Content ∴∞⇐ℜΩ÷嗷娪""");

                vertex = new GraphvizVertex
                {
                    IsHtmlLabel = true,
                    Label = "<i>\"The Label\"</i>\n &amp;/&lt;&gt;@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\\[]() Content ∴∞⇐ℜΩ÷嗷娪"
                };
                yield return new TestCaseData(
                    vertex,
                    @"label=<<i>""The Label""</i>" + '\n' + @" &amp;/&lt;&gt;@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\[]() Content ∴∞⇐ℜΩ÷嗷娪>");
            }
        }

        [TestCaseSource(nameof(ToDotTestCases))]
        public void ToDot(GraphvizVertex vertex, string expectedDot)
        {
            vertex.ToDot().Should().Be(expectedDot);
            vertex.ToString().Should().Be(expectedDot);
        }

        private static IEnumerable<TestCaseData> ToDotCultureInvariantTestCases
        {
            get
            {
                Func<GraphvizVertex, string> toDot = vertex => vertex.ToDot();
                yield return new TestCaseData(toDot);

                Func<GraphvizVertex, string> toString = vertex => vertex.ToString();
                yield return new TestCaseData(toString);
            }
        }

        [TestCaseSource(nameof(ToDotCultureInvariantTestCases))]
        public void ToDot_InvariantCulture([InstantHandle] Func<GraphvizVertex, string> convert)
        {
            var vertex = new GraphvizVertex
            {
                Shape = GraphvizVertexShape.Polygon,
                Distortion = 2.5,
                Font = new GraphvizFont("Test font", 12.5f),
                PenWidth = 2.5,
                Orientation = 45.8,
                FixedSize = true,
                Size = new GraphvizSizeF(14.5f, 21.6f),
                Skew = 2.2,
                Z = 12.3
            };

            const string expectedDot =
                @"fontname=""Test font"", fontsize=12.5, penwidth=2.5, shape=polygon, sides=4, skew=2.2, "
                + @"distortion=2.5, fixedsize=true, height=21.6, width=14.5, orientation=45.8, z=12.3";

            using (CultureScope(EnglishCulture))
            {
                convert(vertex).Should().Be(expectedDot);
            }

            using (CultureScope(FrenchCulture))
            {
                convert(vertex).Should().Be(expectedDot);
            }
        }
    }
}
