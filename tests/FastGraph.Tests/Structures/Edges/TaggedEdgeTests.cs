#nullable enable

using NUnit.Framework;

namespace FastGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="TaggedEdge{TVertex,TTag}"/>.
    ///</summary>
    [TestFixture]
    internal sealed class TaggedEdgeTests : EdgeTestsBase
    {
        [Test]
        public void Construction()
        {
            var tag = new TestObject(1);

            // Value type
            CheckTaggedEdge(new TaggedEdge<int, TestObject>(1, 2, default), 1, 2, (TestObject?)default);
            CheckTaggedEdge(new TaggedEdge<int, TestObject>(2, 1, default), 2, 1, (TestObject?)default);
            CheckTaggedEdge(new TaggedEdge<int, TestObject>(1, 1, default), 1, 1, (TestObject?)default);
            CheckTaggedEdge(new TaggedEdge<int, TestObject>(1, 2, tag), 1, 2, tag);

            // Reference type
            var v1 = new TestVertex("v1");
            var v2 = new TestVertex("v2");
            CheckTaggedEdge(new TaggedEdge<TestVertex, TestObject>(v1, v2, default), v1, v2, (TestObject?)default);
            CheckTaggedEdge(new TaggedEdge<TestVertex, TestObject>(v2, v1, default), v2, v1, (TestObject?)default);
            CheckTaggedEdge(new TaggedEdge<TestVertex, TestObject>(v1, v1, default), v1, v1, (TestObject?)default);
            CheckTaggedEdge(new TaggedEdge<TestVertex, TestObject>(v1, v2, tag), v1, v2, tag);
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new TaggedEdge<TestVertex, TestObject>(default, new TestVertex("v1"), default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new TaggedEdge<TestVertex, TestObject>(new TestVertex("v1"), default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new TaggedEdge<TestVertex, TestObject>(default, default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Equals()
        {
            var tag1 = new TestObject(1);
            var tag2 = new TestObject(2);
            var edge1 = new TaggedEdge<int, TestObject>(1, 2, tag1);
            var edge2 = new TaggedEdge<int, TestObject>(1, 2, tag1);
            var edge3 = new TaggedEdge<int, TestObject>(1, 2, tag2);
            var edge4 = new TaggedEdge<int, TestObject>(1, 2, default);

            edge1.Should().Be(edge1);

            edge2.Should().NotBe(edge1);
            edge1.Should().NotBe(edge2);
            edge1.Equals(edge2).Should().BeFalse();
            edge2.Equals(edge1).Should().BeFalse();

            edge3.Should().NotBe(edge1);
            edge1.Should().NotBe(edge3);
            edge1.Equals(edge3).Should().BeFalse();
            edge3.Equals(edge1).Should().BeFalse();

            edge4.Should().NotBe(edge1);
            edge1.Should().NotBe(edge4);
            edge1.Equals(edge4).Should().BeFalse();
            edge4.Equals(edge1).Should().BeFalse();

            edge1.Should().NotBe(default);
            edge1.Equals(default).Should().BeFalse();
        }

        [Test]
        public void TagChanged()
        {
            var edge = new TaggedEdge<int, TestObject>(1, 2, default);

            int changeCount = 0;
            edge.TagChanged += (_, _) => ++changeCount;

            edge.Tag = default;
            changeCount.Should().Be(0);

            var tag1 = new TestObject(1);
            edge.Tag = tag1;
            changeCount.Should().Be(1);

            edge.Tag = tag1;
            changeCount.Should().Be(1);

            var tag2 = new TestObject(2);
            edge.Tag = tag2;
            changeCount.Should().Be(2);

            edge.Tag = tag1;
            changeCount.Should().Be(3);
        }

        [Test]
        public void ObjectToString()
        {
            var edge1 = new TaggedEdge<int, TestObject>(1, 2, default);
            var edge2 = new TaggedEdge<int, TestObject>(1, 2, new TestObject(42));
            var edge3 = new TaggedEdge<int, TestObject>(2, 1, default);

            edge1.ToString().Should().Be("1 -> 2 (no tag)");
            edge2.ToString().Should().Be("1 -> 2 (42)");
            edge3.ToString().Should().Be("2 -> 1 (no tag)");
        }
    }
}
