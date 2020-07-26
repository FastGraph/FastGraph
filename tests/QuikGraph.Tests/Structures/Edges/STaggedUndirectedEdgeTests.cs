using System;
using NUnit.Framework;

namespace QuikGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="STaggedUndirectedEdge{TVertex,TTag}"/>.
    ///</summary>
    [TestFixture]
    internal class STaggedUndirectedEdgeTests : EdgeTestsBase
    {
        [Test]
        public void Construction()
        {
            var tag = new TestObject(1);

            // Value type
            CheckTaggedEdge(new STaggedUndirectedEdge<int, TestObject>(1, 2, null), 1, 2, (TestObject)null);
            CheckTaggedEdge(new STaggedUndirectedEdge<int, TestObject>(1, 1, null), 1, 1, (TestObject)null);
            CheckTaggedEdge(default(STaggedUndirectedEdge<int, TestObject>), 0, 0, (TestObject)null);
            CheckTaggedEdge(new STaggedUndirectedEdge<int, TestObject>(1, 2, tag), 1, 2, tag);

            // Reference type
            var v1 = new ComparableTestVertex("v1");
            var v2 = new ComparableTestVertex("v2");
            CheckTaggedEdge(new STaggedUndirectedEdge<ComparableTestVertex, TestObject>(v1, v2, null), v1, v2, (TestObject)null);
            CheckTaggedEdge(new STaggedUndirectedEdge<ComparableTestVertex, TestObject>(v1, v1, null), v1, v1, (TestObject)null);
            CheckStructTaggedEdge(default(STaggedUndirectedEdge<ComparableTestVertex, TestObject>), (ComparableTestVertex)null, null, (TestObject)null);
            CheckTaggedEdge(new STaggedUndirectedEdge<ComparableTestVertex, TestObject>(v1, v2, tag), v1, v2, tag);

            // Struct break the contract with their implicit default constructor
            // Non struct edge should be preferred
            var defaultEdge = default(STaggedUndirectedEdge<ComparableTestVertex, int>);
            Assert.IsNull(defaultEdge.Source);
            // ReSharper disable once HeuristicUnreachableCode
            // Justification: Since struct has implicit default constructor it allows initialization of invalid edge
            Assert.IsNull(defaultEdge.Target);
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new STaggedUndirectedEdge<TestVertex, TestObject>(null, new TestVertex("v1"), null));
            Assert.Throws<ArgumentNullException>(() => new STaggedUndirectedEdge<TestVertex, TestObject>(new TestVertex("v1"), null, null));
            Assert.Throws<ArgumentNullException>(() => new STaggedUndirectedEdge<TestVertex, TestObject>(null, null, null));
            // ReSharper restore AssignNullToNotNullAttribute

            Assert.Throws<ArgumentException>(() => new STaggedUndirectedEdge<int, TestObject>(2, 1, null));

            // Not comparable
            var v1 = new TestVertex("v1");
            var v2 = new TestVertex("v2");
            Assert.Throws<ArgumentException>(() => new STaggedUndirectedEdge<TestVertex, TestObject>(v1, v2, null));

            var comparableV1 = new ComparableTestVertex("v1");
            var comparableV2 = new ComparableTestVertex("v2");
            Assert.Throws<ArgumentException>(() => new STaggedUndirectedEdge<ComparableTestVertex, TestObject>(comparableV2, comparableV1, null));
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Equals()
        {
            var tag1 = new TestObject(1);
            var tag2 = new TestObject(2);
            var edge1 = default(STaggedUndirectedEdge<int, TestObject>);
            var edge2 = new STaggedUndirectedEdge<int, TestObject>(0, 0, null);
            var edge3 = new STaggedUndirectedEdge<int, TestObject>(1, 2, null);
            var edge4 = new STaggedUndirectedEdge<int, TestObject>(1, 2, null);
            var edge6 = new STaggedUndirectedEdge<int, TestObject>(1, 2, tag1);
            var edge7 = new STaggedUndirectedEdge<int, TestObject>(1, 2, tag1);
            var edge8 = new STaggedUndirectedEdge<int, TestObject>(1, 2, tag2);

            Assert.AreEqual(edge1, edge1);

            Assert.AreEqual(edge1, edge2);
            Assert.AreEqual(edge2, edge1);
            Assert.IsTrue(edge1.Equals(edge2));
            Assert.IsTrue(edge2.Equals(edge1));

            Assert.AreNotEqual(edge1, edge3);
            Assert.AreNotEqual(edge3, edge1);
            Assert.IsFalse(edge1.Equals(edge3));
            Assert.IsFalse(edge3.Equals(edge1));

            Assert.AreEqual(edge3, edge4);
            Assert.AreEqual(edge4, edge3);
            Assert.IsTrue(edge3.Equals(edge4));
            Assert.IsTrue(edge4.Equals(edge3));

            Assert.AreNotEqual(edge3, edge6);
            Assert.AreNotEqual(edge6, edge3);
            Assert.IsFalse(edge3.Equals(edge6));
            Assert.IsFalse(edge6.Equals(edge3));

            Assert.AreEqual(edge6, edge7);
            Assert.AreEqual(edge7, edge6);
            Assert.IsTrue(edge6.Equals(edge7));
            Assert.IsTrue(edge7.Equals(edge6));

            Assert.AreNotEqual(edge6, edge8);
            Assert.AreNotEqual(edge8, edge6);
            Assert.IsFalse(edge6.Equals(edge8));
            Assert.IsFalse(edge8.Equals(edge6));

            Assert.AreNotEqual(edge1, null);
            Assert.IsFalse(edge1.Equals(null));
        }

        [Test]
        public void EqualsDefaultEdge_ReferenceTypeExtremities()
        {
            var edge1 = default(STaggedUndirectedEdge<int, TestObject>);
            var edge2 = new STaggedUndirectedEdge<int, TestObject>();

            Assert.AreEqual(edge1, edge2);
            Assert.AreEqual(edge2, edge1);
            Assert.IsTrue(edge1.Equals(edge2));
            Assert.IsTrue(edge2.Equals(edge1));
        }

        [Test]
        public void TagChanged()
        {
            var edge = new STaggedUndirectedEdge<int, TestObject>(1, 2, null);

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
            var edge1 = new STaggedUndirectedEdge<int, TestObject>(1, 2, null);
            var edge2 = new STaggedUndirectedEdge<int, TestObject>(1, 2, new TestObject(42));

            Assert.AreEqual("1 <-> 2 (no tag)", edge1.ToString());
            Assert.AreEqual("1 <-> 2 (42)", edge2.ToString());
        }
    }
}