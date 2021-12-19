#nullable enable

using NUnit.Framework;

namespace FastGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="EquatableTaggedEdge{TVertex,TTag}"/>.
    ///</summary>
    [TestFixture]
    internal sealed class EquatableTaggedEdgeTests : EdgeTestsBase
    {
        [Test]
        public void Construction()
        {
            var tag = new TestObject(1);

            // Value type
            CheckTaggedEdge(new EquatableTaggedEdge<int, TestObject>(1, 2, default), 1, 2, (TestObject?)default);
            CheckTaggedEdge(new EquatableTaggedEdge<int, TestObject>(2, 1, default), 2, 1, (TestObject?)default);
            CheckTaggedEdge(new EquatableTaggedEdge<int, TestObject>(1, 1, default), 1, 1, (TestObject?)default);
            CheckTaggedEdge(new EquatableTaggedEdge<int, TestObject>(1, 2, tag), 1, 2, tag);

            // Reference type
            var v1 = new TestVertex("v1");
            var v2 = new TestVertex("v2");
            CheckTaggedEdge(new EquatableTaggedEdge<TestVertex, TestObject>(v1, v2, default), v1, v2, (TestObject?)default);
            CheckTaggedEdge(new EquatableTaggedEdge<TestVertex, TestObject>(v2, v1, default), v2, v1, (TestObject?)default);
            CheckTaggedEdge(new EquatableTaggedEdge<TestVertex, TestObject>(v1, v1, default), v1, v1, (TestObject?)default);
            CheckTaggedEdge(new EquatableTaggedEdge<TestVertex, TestObject>(v1, v2, tag), v1, v2, tag);
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Assert.Throws<ArgumentNullException>(() => new EquatableTaggedEdge<TestVertex, TestObject>(default, new TestVertex("v1"), default));
            Assert.Throws<ArgumentNullException>(() => new EquatableTaggedEdge<TestVertex, TestObject>(new TestVertex("v1"), default, default));
            Assert.Throws<ArgumentNullException>(() => new EquatableTaggedEdge<TestVertex, TestObject>(default, default, default));
#pragma warning restore CS8625
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
            var edge4 = new EquatableTaggedEdge<int, TestObject>(1, 2, default);
            var edge5 = new EquatableTaggedEdge<int, TestObject>(1, 2, default);
            var edge6 = new EquatableTaggedEdge<int, TestObject>(2, 1, default);

            Assert.AreEqual(edge1, edge1);

            Assert.AreEqual(edge1, edge2);
            Assert.AreEqual(edge2, edge1);
            Assert.IsTrue(edge1.Equals((object)edge2));
            Assert.IsTrue(edge1.Equals(edge3));  // Tag is not taken into account for equality
            Assert.IsTrue(edge1.Equals(edge4));  // Tag is not taken into account for equality

            Assert.AreEqual(edge4, edge5);
            Assert.AreEqual(edge5, edge4);
            Assert.IsTrue(edge4.Equals((object)edge5));
            Assert.IsTrue(edge4.Equals(edge5));  // Tag is not taken into account for equality
            Assert.IsTrue(edge5.Equals(edge4));  // Tag is not taken into account for equality

            Assert.AreNotEqual(edge4, edge6);
            Assert.AreNotEqual(edge6, edge4);
            Assert.IsFalse(edge4.Equals((object)edge6));
            Assert.IsFalse(edge4.Equals(edge6));
            Assert.IsFalse(edge6.Equals(edge4));

            Assert.AreNotEqual(edge1, default);
            Assert.IsFalse(edge1.Equals(default));
        }

        [Test]
        public void TagChanged()
        {
            var edge = new EquatableTaggedEdge<int, TestObject>(1, 2, default);

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
        public void Hashcode()
        {
            var tag = new TestObject(1);

            var edge1 = new EquatableTaggedEdge<int, TestObject>(1, 2, default);
            var edge2 = new EquatableTaggedEdge<int, TestObject>(1, 2, default);
            var edge3 = new EquatableTaggedEdge<int, TestObject>(2, 1, default);
            var edge4 = new EquatableTaggedEdge<int, TestObject>(1, 2, tag);

            Assert.AreEqual(edge1.GetHashCode(), edge2.GetHashCode());
            Assert.AreNotEqual(edge1.GetHashCode(), edge3.GetHashCode());
            Assert.AreEqual(edge1.GetHashCode(), edge4.GetHashCode());  // Tag is not taken into account for hashcode
        }

        [Test]
        public void ObjectToString()
        {
            var edge1 = new EquatableTaggedEdge<int, TestObject>(1, 2, default);
            var edge2 = new EquatableTaggedEdge<int, TestObject>(1, 2, new TestObject(25));
            var edge3 = new EquatableTaggedEdge<int, TestObject>(2, 1, default);

            Assert.AreEqual("1 -> 2 (no tag)", edge1.ToString());
            Assert.AreEqual("1 -> 2 (25)", edge2.ToString());
            Assert.AreEqual("2 -> 1 (no tag)", edge3.ToString());
        }
    }
}
