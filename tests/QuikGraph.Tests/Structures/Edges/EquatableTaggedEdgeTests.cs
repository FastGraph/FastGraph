using System;
using NUnit.Framework;

namespace QuikGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="EquatableTaggedEdge{TVertex,TTag}"/>.
    ///</summary>
    [TestFixture]
    internal class EquatableTaggedEdgeTests : EdgeTestsBase
    {
        [Test]
        public void Construction()
        {
            var tag = new TestObject(1);

            // Value type
            CheckTaggedEdge(new EquatableTaggedEdge<int, TestObject>(1, 2, null), 1, 2, (TestObject)null);
            CheckTaggedEdge(new EquatableTaggedEdge<int, TestObject>(2, 1, null), 2, 1, (TestObject)null);
            CheckTaggedEdge(new EquatableTaggedEdge<int, TestObject>(1, 1, null), 1, 1, (TestObject)null);
            CheckTaggedEdge(new EquatableTaggedEdge<int, TestObject>(1, 2, tag), 1, 2, tag);

            // Reference type
            var v1 = new TestVertex("v1");
            var v2 = new TestVertex("v2");
            CheckTaggedEdge(new EquatableTaggedEdge<TestVertex, TestObject>(v1, v2, null), v1, v2, (TestObject)null);
            CheckTaggedEdge(new EquatableTaggedEdge<TestVertex, TestObject>(v2, v1, null), v2, v1, (TestObject)null);
            CheckTaggedEdge(new EquatableTaggedEdge<TestVertex, TestObject>(v1, v1, null), v1, v1, (TestObject)null);
            CheckTaggedEdge(new EquatableTaggedEdge<TestVertex, TestObject>(v1, v2, tag), v1, v2, tag);
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new EquatableTaggedEdge<TestVertex, TestObject>(null, new TestVertex("v1"), null));
            Assert.Throws<ArgumentNullException>(() => new EquatableTaggedEdge<TestVertex, TestObject>(new TestVertex("v1"), null, null));
            Assert.Throws<ArgumentNullException>(() => new EquatableTaggedEdge<TestVertex, TestObject>(null, null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Equals()
        {
            var tag1 = new TestObject(1);
            var tag2 = new TestObject(2);
            var edge1 = new EquatableTaggedEdge<int, TestObject>(1, 2, tag1);
            var edge2 = new EquatableTaggedEdge<int, TestObject>(1, 2, tag1);
            var edge3 = new EquatableTaggedEdge<int, TestObject>(1, 2, tag2);
            var edge4 = new EquatableTaggedEdge<int, TestObject>(1, 2, null);
            var edge5 = new EquatableTaggedEdge<int, TestObject>(1, 2, null);

            Assert.AreEqual(edge1, edge1);
            Assert.AreEqual(edge1, edge2);
            Assert.IsTrue(edge1.Equals((object)edge2));
            Assert.AreEqual(edge1, edge3);  // Tag is not taken into account for equality
            Assert.AreEqual(edge1, edge4);  // Tag is not taken into account for equality

            Assert.AreEqual(edge4, edge5);

            Assert.IsFalse(edge1.Equals(null));
            Assert.AreNotEqual(edge1, null);
        }

        [Test]
        public void TagChanged()
        {
            var edge = new EquatableTaggedEdge<int, TestObject>(1, 2, null);

            int changeCount = 0;
            edge.TagChanged += (sender, args) => ++changeCount;

            edge.Tag = null;
            Assert.AreEqual(0, changeCount);

            var tag1 = new TestObject(1);
            edge.Tag = tag1;
            Assert.AreEqual(1, changeCount);

            edge.Tag = tag1;
            Assert.AreEqual(1, changeCount);

            var tag2 = new TestObject(2);
            edge.Tag = tag2;
            Assert.AreEqual(2, changeCount);

            edge.Tag = tag1;
            Assert.AreEqual(3, changeCount);
        }

        [Test]
        public void ObjectToString()
        {
            var edge1 = new EquatableTaggedEdge<int, TestObject>(1, 2, null);
            var edge2 = new EquatableTaggedEdge<int, TestObject>(1, 2, new TestObject(25));
            var edge3 = new EquatableTaggedEdge<int, TestObject>(2, 1, null);

            Assert.AreEqual("1 -> 2 (no tag)", edge1.ToString());
            Assert.AreEqual("1 -> 2 (25)", edge2.ToString());
            Assert.AreEqual("2 -> 1 (no tag)", edge3.ToString());
        }
    }
}
