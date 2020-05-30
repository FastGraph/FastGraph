using System;
using NUnit.Framework;
using QuikGraph.Graphviz.Dot;

namespace QuikGraph.Graphviz.Tests
{
    /// <summary>
    /// Tests related to <see cref="GraphvizRecordCellCollection"/>.
    /// </summary>
    [TestFixture]
    internal class GraphvizRecordCellCollectionTests
    {
        [Test]
        public void Constructor()
        {
            var recordCollection = new GraphvizRecordCellCollection();
            CollectionAssert.IsEmpty(recordCollection);

            var cell1 = new GraphvizRecordCell();
            recordCollection.Add(cell1);
            CollectionAssert.AreEqual(new[] { cell1 }, recordCollection);
            
            var cell2 = new GraphvizRecordCell();
            var cellArray = new[] { cell1, cell2 };
            recordCollection = new GraphvizRecordCellCollection(cellArray);
            CollectionAssert.AreEqual(cellArray, recordCollection);
            
            var otherRecordCollection = new GraphvizRecordCellCollection(recordCollection);
            CollectionAssert.AreEqual(recordCollection, otherRecordCollection);
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new GraphvizRecordCellCollection((GraphvizRecordCell[])null));
            Assert.Throws<ArgumentNullException>(() => new GraphvizRecordCellCollection((GraphvizRecordCellCollection)null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }
    }
}