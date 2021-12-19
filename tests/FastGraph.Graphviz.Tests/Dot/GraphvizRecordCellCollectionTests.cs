#nullable enable

using NUnit.Framework;
using FastGraph.Graphviz.Dot;

namespace FastGraph.Graphviz.Tests
{
    /// <summary>
    /// Tests related to <see cref="GraphvizRecordCellCollection"/>.
    /// </summary>
    [TestFixture]
    internal sealed class GraphvizRecordCellCollectionTests
    {
        [Test]
        public void Constructor()
        {
            var recordCollection = new GraphvizRecordCellCollection();
            recordCollection.Should().BeEmpty();

            var cell1 = new GraphvizRecordCell();
            recordCollection.Add(cell1);
            recordCollection.Should().BeEquivalentTo(new[] { cell1 });

            var cell2 = new GraphvizRecordCell();
            var cellArray = new[] { cell1, cell2 };
            recordCollection = new GraphvizRecordCellCollection(cellArray);
            recordCollection.Should().BeEquivalentTo(cellArray);

            var otherRecordCollection = new GraphvizRecordCellCollection(recordCollection);
            otherRecordCollection.Should().BeEquivalentTo(recordCollection);
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new GraphvizRecordCellCollection((GraphvizRecordCell[]?)default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphvizRecordCellCollection((GraphvizRecordCellCollection?)default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }
    }
}
