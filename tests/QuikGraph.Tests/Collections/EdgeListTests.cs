using System;
using NUnit.Framework;
using QuikGraph.Collections;

namespace QuikGraph.Tests.Collections
{
    /// <summary>
    /// Tests for <see cref="EdgeList{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class EdgeListTests
    {
        [Test]
        public void Constructors()
        {
            // ReSharper disable ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new EdgeList<int, Edge<int>>());
            Assert.DoesNotThrow(() => new EdgeList<int, Edge<int>>(12));
            var list = new EdgeList<int, EquatableEdge<int>>
            {
                new EquatableEdge<int>(1, 2),
                new EquatableEdge<int>(2, 3)
            };
            var otherList = new EdgeList<int, EquatableEdge<int>>(list);
            CollectionAssert.AreEqual(list, otherList);
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Clone()
        {
            var list = new EdgeList<int, EquatableEdge<int>>();

            EdgeList<int, EquatableEdge<int>> clonedList = list.Clone();
            CollectionAssert.IsEmpty(clonedList);

            clonedList = (EdgeList<int, EquatableEdge<int>>)((IEdgeList<int, EquatableEdge<int>>)list).Clone();
            CollectionAssert.IsEmpty(clonedList);

            clonedList = (EdgeList<int, EquatableEdge<int>>)((ICloneable)list).Clone();
            CollectionAssert.IsEmpty(clonedList);

            list.AddRange(new[]
            {
                new EquatableEdge<int>(1, 2),
                new EquatableEdge<int>(2, 3)
            });
            clonedList = list.Clone();
            CollectionAssert.AreEqual(list, clonedList);

            clonedList = (EdgeList<int, EquatableEdge<int>>)((IEdgeList<int, EquatableEdge<int>>)list).Clone();
            CollectionAssert.AreEqual(list, clonedList);

            clonedList = (EdgeList<int, EquatableEdge<int>>)((ICloneable)list).Clone();
            CollectionAssert.AreEqual(list, clonedList);
        }
    }
}