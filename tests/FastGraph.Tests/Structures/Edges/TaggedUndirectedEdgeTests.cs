#nullable enable

using NUnit.Framework;

namespace FastGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="TaggedUndirectedEdge{TVertex,TTag}"/>.
    ///</summary>
    [TestFixture]
    internal sealed class TaggedUndirectedEdgeTests : EdgeTestsBase
    {
        [Test]
        public void Construction()
        {
            var tag = new TestObject(1);

            // Value type
            CheckTaggedEdge(new TaggedUndirectedEdge<int, TestObject>(1, 2, default), 1, 2, (TestObject?)default);
            CheckTaggedEdge(new TaggedUndirectedEdge<int, TestObject>(1, 1, default), 1, 1, (TestObject?)default);
            CheckTaggedEdge(new TaggedUndirectedEdge<int, TestObject>(1, 2, tag), 1, 2, tag);

            // Reference type
            var v1 = new ComparableTestVertex("v1");
            var v2 = new ComparableTestVertex("v2");
            CheckTaggedEdge(new TaggedUndirectedEdge<ComparableTestVertex, TestObject>(v1, v2, default), v1, v2, (TestObject?)default);
            CheckTaggedEdge(new TaggedUndirectedEdge<ComparableTestVertex, TestObject>(v1, v1, default), v1, v1, (TestObject?)default);
            CheckTaggedEdge(new TaggedUndirectedEdge<ComparableTestVertex, TestObject>(v1, v2, tag), v1, v2, tag);
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Assert.Throws<ArgumentNullException>(() => new TaggedUndirectedEdge<TestVertex, TestObject>(default, new TestVertex("v1"), default));
            Assert.Throws<ArgumentNullException>(() => new TaggedUndirectedEdge<TestVertex, TestObject>(new TestVertex("v1"), default, default));
            Assert.Throws<ArgumentNullException>(() => new TaggedUndirectedEdge<TestVertex, TestObject>(default, default, default));
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute

            Assert.Throws<ArgumentException>(() => new TaggedUndirectedEdge<int, TestObject>(2, 1, default));

            // Not comparable
            var v1 = new TestVertex("v1");
            var v2 = new TestVertex("v2");
            Assert.Throws<ArgumentException>(() => new TaggedUndirectedEdge<TestVertex, TestObject>(v1, v2, default));

            var comparableV1 = new ComparableTestVertex("v1");
            var comparableV2 = new ComparableTestVertex("v2");
            Assert.Throws<ArgumentException>(() => new TaggedUndirectedEdge<ComparableTestVertex, TestObject>(comparableV2, comparableV1, default));
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Equals()
        {
            var tag1 = new TestObject(1);
            var tag2 = new TestObject(2);
            var edge1 = new TaggedUndirectedEdge<int, TestObject>(1, 2, tag1);
            var edge2 = new TaggedUndirectedEdge<int, TestObject>(1, 2, tag1);
            var edge3 = new TaggedUndirectedEdge<int, TestObject>(1, 2, tag2);
            var edge4 = new TaggedUndirectedEdge<int, TestObject>(1, 2, default);

            Assert.AreEqual(edge1, edge1);

            Assert.AreNotEqual(edge1, edge2);
            Assert.AreNotEqual(edge2, edge1);
            Assert.IsFalse(edge1.Equals(edge2));
            Assert.IsFalse(edge2.Equals(edge1));

            Assert.AreNotEqual(edge1, edge3);
            Assert.AreNotEqual(edge3, edge1);
            Assert.IsFalse(edge1.Equals(edge3));
            Assert.IsFalse(edge3.Equals(edge1));

            Assert.AreNotEqual(edge1, edge4);
            Assert.AreNotEqual(edge4, edge1);
            Assert.IsFalse(edge1.Equals(edge4));
            Assert.IsFalse(edge4.Equals(edge1));

            Assert.AreNotEqual(edge1, default);
            Assert.IsFalse(edge1.Equals(default));
        }

        [Test]
        public void TagChanged()
        {
            var edge = new TaggedUndirectedEdge<int, TestObject>(1, 2, default);

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
            var edge1 = new TaggedUndirectedEdge<int, TestObject>(1, 2, default);
            var edge2 = new TaggedUndirectedEdge<int, TestObject>(1, 2, new TestObject(12));

            Assert.AreEqual("1 <-> 2 (no tag)", edge1.ToString());
            Assert.AreEqual("1 <-> 2 (12)", edge2.ToString());
        }
    }
}
