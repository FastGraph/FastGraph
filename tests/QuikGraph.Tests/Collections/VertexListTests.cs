using System;
using NUnit.Framework;
using QuikGraph.Collections;

namespace QuikGraph.Tests.Collections
{
    /// <summary>
    /// Tests for <see cref="VertexList{TVertex}"/>.
    /// </summary>
    [TestFixture]
    internal class VertexListTests
    {
        [Test]
        public void Constructors()
        {
            // ReSharper disable ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new VertexList<int>());
            Assert.DoesNotThrow(() => new VertexList<int>(12));
            var list = new VertexList<int> { 1, 2, 3 };
            var otherList = new VertexList<int>(list);
            CollectionAssert.AreEqual(list, otherList);
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Clone()
        {
            var list = new VertexList<int>();

            VertexList<int> clonedList = list.Clone();
            CollectionAssert.IsEmpty(clonedList);

            clonedList = (VertexList<int>)((ICloneable)list).Clone();
            CollectionAssert.IsEmpty(clonedList);

            list.AddRange(new[] { 1, 2, 3 });
            clonedList = list.Clone();
            CollectionAssert.AreEqual(list, clonedList);

            clonedList = (VertexList<int>)((ICloneable)list).Clone();
            CollectionAssert.AreEqual(list, clonedList);
        }
    }
}