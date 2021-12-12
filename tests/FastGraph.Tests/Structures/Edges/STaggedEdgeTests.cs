#nullable enable

using NUnit.Framework;

namespace FastGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="STaggedEdge{TVertex,TTag}"/>.
    ///</summary>
    [TestFixture]
    internal sealed class STaggedEdgeTests : EdgeTestsBase
    {
        [Test]
        public void Construction()
        {
            var tag = new TestObject(1);

            // Value type
            CheckTaggedEdge(new STaggedEdge<int, TestObject>(1, 2, default), 1, 2, (TestObject?)default);
            CheckTaggedEdge(new STaggedEdge<int, TestObject>(2, 1, default), 2, 1, (TestObject?)default);
            CheckTaggedEdge(new STaggedEdge<int, TestObject>(1, 1, default), 1, 1, (TestObject?)default);
            CheckTaggedEdge(default(STaggedEdge<int, TestObject>), 0, 0, (TestObject?)default);
            CheckTaggedEdge(new STaggedEdge<int, TestObject>(1, 2, tag), 1, 2, tag);

            // Reference type
            var v1 = new TestVertex("v1");
            var v2 = new TestVertex("v2");
            CheckTaggedEdge(new STaggedEdge<TestVertex, TestObject>(v1, v2, default), v1, v2, (TestObject?)default);
            CheckTaggedEdge(new STaggedEdge<TestVertex, TestObject>(v2, v1, default), v2, v1, (TestObject?)default);
            CheckTaggedEdge(new STaggedEdge<TestVertex, TestObject>(v1, v1, default), v1, v1, (TestObject?)default);
            CheckStructTaggedEdge(default(STaggedEdge<TestVertex, TestObject>), (TestVertex?)default, default, (TestObject?)default);
            CheckTaggedEdge(new STaggedEdge<TestVertex, TestObject>(v1, v2, tag), v1, v2, tag);

            // Struct break the contract with their implicit default constructor
            // Non struct edge should be preferred
            var defaultEdge = default(STaggedEdge<TestVertex, int>);
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
#pragma warning disable CS8625
            Assert.Throws<ArgumentNullException>(() => new STaggedEdge<TestVertex, TestObject>(default, new TestVertex("v1"), default));
            Assert.Throws<ArgumentNullException>(() => new STaggedEdge<TestVertex, TestObject>(new TestVertex("v1"), default, default));
            Assert.Throws<ArgumentNullException>(() => new STaggedEdge<TestVertex, TestObject>(default, default, default));
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Equals()
        {
            var tag1 = new TestObject(1);
            var tag2 = new TestObject(2);
            var edge1 = default(STaggedEdge<int, TestObject>);
            var edge2 = new STaggedEdge<int, TestObject>(0, 0, default);
            var edge3 = new STaggedEdge<int, TestObject>(1, 2, default);
            var edge4 = new STaggedEdge<int, TestObject>(1, 2, default);
            var edge5 = new STaggedEdge<int, TestObject>(2, 1, default);
            var edge6 = new STaggedEdge<int, TestObject>(1, 2, tag1);
            var edge7 = new STaggedEdge<int, TestObject>(1, 2, tag1);
            var edge8 = new STaggedEdge<int, TestObject>(1, 2, tag2);

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

            Assert.AreNotEqual(edge3, edge5);
            Assert.AreNotEqual(edge5, edge3);
            Assert.IsFalse(edge3.Equals(edge5));
            Assert.IsFalse(edge5.Equals(edge3));

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

            Assert.AreNotEqual(edge1, default);
            Assert.IsFalse(edge1.Equals(default));
        }

        [Test]
        public void EqualsDefaultEdge_ReferenceTypeExtremities()
        {
            var edge1 = default(STaggedEdge<int, TestObject>);
            var edge2 = new STaggedEdge<int, TestObject>();

            Assert.AreEqual(edge1, edge2);
            Assert.AreEqual(edge2, edge1);
            Assert.IsTrue(edge1.Equals(edge2));
            Assert.IsTrue(edge2.Equals(edge1));
        }

        [Test]
        public void TagChanged()
        {
            var edge = new STaggedEdge<int, TestObject>(1, 2, default);

            int changeCount = 0;
            edge.TagChanged += (_, _) => ++changeCount;

            edge.Tag = default;
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
            var edge1 = new STaggedEdge<int, TestObject>(1, 2, default);
            var edge2 = new STaggedEdge<int, TestObject>(1, 2, new TestObject(42));
            var edge3 = new STaggedEdge<int, TestObject>(2, 1, default);

            Assert.AreEqual("1 -> 2 (no tag)", edge1.ToString());
            Assert.AreEqual("1 -> 2 (42)", edge2.ToString());
            Assert.AreEqual("2 -> 1 (no tag)", edge3.ToString());
        }
    }
}
