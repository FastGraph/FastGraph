using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Graphviz.Dot;

namespace QuikGraph.Graphviz.Tests
{
    /// <summary>
    /// Tests related to <see cref="GraphvizRecord"/>.
    /// </summary>
    [TestFixture]
    internal class GraphvizRecordTests
    {
        [Test]
        public void Constructor()
        {
            var record = new GraphvizRecord();
            CollectionAssert.IsEmpty(record.Cells);
        }

        [Test]
        public void Cells()
        {
            var record = new GraphvizRecord();
            if (record.Cells is null)
                throw new InvalidOperationException($"Cell has null {nameof(GraphvizRecord.Cells)}.");

            var recordCollection = new GraphvizRecordCellCollection();
            record.Cells = recordCollection;
            Assert.AreSame(recordCollection, record.Cells);
        }

        [Test]
        public void Cells_Throws()
        {
            var record = new GraphvizRecord();
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => record.Cells = null);
        }

        [NotNull, ItemNotNull]
        private static IEnumerable<TestCaseData> ToDotTestCases
        {
            get
            {
                var emptyRecord = new GraphvizRecord();
                yield return new TestCaseData(emptyRecord, string.Empty);

                var recordWithChildCell = new GraphvizRecord
                {
                    Cells = new GraphvizRecordCellCollection(new[]
                    {
                        new GraphvizRecordCell
                        {
                            Port = "SubPort",
                            Text = "SubText"
                        }
                    })
                };
                yield return new TestCaseData(recordWithChildCell, @"<SubPort> SubText");

                var recordWithChildrenCells = new GraphvizRecord
                {
                    Cells = new GraphvizRecordCellCollection(new[]
                    {
                        new GraphvizRecordCell
                        {
                            Port = "SubPort1"
                        },
                        new GraphvizRecordCell
                        {
                            Port = "SubPort2",
                            Text = "SubText2"
                        },
                        new GraphvizRecordCell
                        {
                            Text = "SubText3"
                        }
                    })
                };
                yield return new TestCaseData(
                    recordWithChildrenCells,
                    @"<SubPort1>  | <SubPort2> SubText2 | SubText3");

                var recordWithChildrenCellsAndNestedCells = new GraphvizRecord
                {
                    Cells = new GraphvizRecordCellCollection(new[]
                    {
                        new GraphvizRecordCell
                        {
                            Port = "SubPort1"
                        },
                        new GraphvizRecordCell
                        {
                            Cells = new GraphvizRecordCellCollection(new[]
                            {
                                new GraphvizRecordCell
                                {
                                    Text = "NestedText1"
                                },
                                new GraphvizRecordCell
                                {
                                    Port = "NestedPort2",
                                    Text = "NestedText2"
                                },
                                new GraphvizRecordCell
                                {
                                    Text = "NestedText3"
                                }
                            })
                        },
                        new GraphvizRecordCell
                        {
                            Text = "SubText3"
                        }
                    })
                };
                yield return new TestCaseData(
                    recordWithChildrenCellsAndNestedCells,
                    @"<SubPort1>  | { NestedText1 | <NestedPort2> NestedText2 | NestedText3 } | SubText3");

                var recordWithChildrenCellsAndNestedCells2 = new GraphvizRecord
                {
                    Cells = new GraphvizRecordCellCollection(new[]
                    {
                        new GraphvizRecordCell
                        {
                            Port = "SubPort1"
                        },
                        new GraphvizRecordCell
                        {
                            Port = "SubPort2",
                            Text = "SubText2",
                            Cells = new GraphvizRecordCellCollection(new[]
                            {
                                new GraphvizRecordCell
                                {
                                    Text = "NestedText1"
                                },
                                new GraphvizRecordCell
                                {
                                    Port = "NestedPort2",
                                    Text = "NestedText2"
                                },
                                new GraphvizRecordCell
                                {
                                    Text = "NestedText3"
                                }
                            })
                        },
                        new GraphvizRecordCell
                        {
                            Text = "SubText3"
                        }
                    })
                };
                yield return new TestCaseData(
                    recordWithChildrenCellsAndNestedCells2,
                    @"<SubPort1>  | <SubPort2> SubText2 | { NestedText1 | <NestedPort2> NestedText2 | NestedText3 } | SubText3");

            }
        }

        [TestCaseSource(nameof(ToDotTestCases))]
        public void ToDot([NotNull] GraphvizRecord record, [NotNull] string expectedDot)
        {
            Assert.AreEqual(expectedDot, record.ToDot());
            Assert.AreEqual(expectedDot, record.ToString());
        }
    }
}