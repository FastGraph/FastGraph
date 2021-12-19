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
            Invoking(() => new EquatableTaggedEdge<TestVertex, TestObject>(default, new TestVertex("v1"), default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EquatableTaggedEdge<TestVertex, TestObject>(new TestVertex("v1"), default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EquatableTaggedEdge<TestVertex, TestObject>(default, default, default)).Should().Throw<ArgumentNullException>();
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

            edge1.Should().Be(edge1);

            edge2.Should().Be(edge1);
            edge1.Should().Be(edge2);
            edge1.Equals(edge2).Should().BeTrue();
            edge1.Equals(edge3).Should().BeTrue();  // Tag is not taken into account for equality
            edge1.Equals(edge4).Should().BeTrue();  // Tag is not taken into account for equality

            edge5.Should().Be(edge4);
            edge4.Should().Be(edge5);
            edge4.Equals(edge5).Should().BeTrue();
            edge4.Equals(edge5).Should().BeTrue();  // Tag is not taken into account for equality
            edge5.Equals(edge4).Should().BeTrue();  // Tag is not taken into account for equality

            edge6.Should().NotBe(edge4);
            edge4.Should().NotBe(edge6);
            edge4.Equals(edge6).Should().BeFalse();
            edge4.Equals(edge6).Should().BeFalse();
            edge6.Equals(edge4).Should().BeFalse();

            edge1.Should().NotBe(default);
            edge1.Equals(default).Should().BeFalse();
        }

        [Test]
        public void TagChanged()
        {
            var edge = new EquatableTaggedEdge<int, TestObject>(1, 2, default);

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
        public void Hashcode()
        {
            var tag = new TestObject(1);

            var edge1 = new EquatableTaggedEdge<int, TestObject>(1, 2, default);
            var edge2 = new EquatableTaggedEdge<int, TestObject>(1, 2, default);
            var edge3 = new EquatableTaggedEdge<int, TestObject>(2, 1, default);
            var edge4 = new EquatableTaggedEdge<int, TestObject>(1, 2, tag);

            edge2.GetHashCode().Should().Be(edge1.GetHashCode());
            edge3.GetHashCode().Should().NotBe(edge1.GetHashCode());
            edge4.GetHashCode().Should().Be(edge1.GetHashCode());  // Tag is not taken into account for hashcode
        }

        [Test]
        public void ObjectToString()
        {
            var edge1 = new EquatableTaggedEdge<int, TestObject>(1, 2, default);
            var edge2 = new EquatableTaggedEdge<int, TestObject>(1, 2, new TestObject(25));
            var edge3 = new EquatableTaggedEdge<int, TestObject>(2, 1, default);

            edge1.ToString().Should().Be("1 -> 2 (no tag)");
            edge2.ToString().Should().Be("1 -> 2 (25)");
            edge3.ToString().Should().Be("2 -> 1 (no tag)");
        }
    }
}
