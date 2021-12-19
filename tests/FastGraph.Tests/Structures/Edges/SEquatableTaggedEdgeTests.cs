#nullable enable

using NUnit.Framework;

namespace FastGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="SEquatableTaggedEdge{TVertex,TTag}"/>.
    ///</summary>
    [TestFixture]
    internal sealed class SEquatableTaggedEdgeTests : EdgeTestsBase
    {
        [Test]
        public void Construction()
        {
            var tag = new TestObject(1);

            // Value type
            CheckTaggedEdge(new SEquatableTaggedEdge<int, TestObject>(1, 2, default), 1, 2, (TestObject?)default);
            CheckTaggedEdge(new SEquatableTaggedEdge<int, TestObject>(2, 1, default), 2, 1, (TestObject?)default);
            CheckTaggedEdge(new SEquatableTaggedEdge<int, TestObject>(1, 1, default), 1, 1, (TestObject?)default);
            CheckTaggedEdge(default(SEquatableTaggedEdge<int, TestObject>), 0, 0, (TestObject?)default);
            CheckTaggedEdge(new SEquatableTaggedEdge<int, TestObject>(1, 2, tag), 1, 2, tag);

            // Reference type
            var v1 = new TestVertex("v1");
            var v2 = new TestVertex("v2");
            CheckTaggedEdge(new SEquatableTaggedEdge<TestVertex, TestObject>(v1, v2, default), v1, v2, (TestObject?)default);
            CheckTaggedEdge(new SEquatableTaggedEdge<TestVertex, TestObject>(v2, v1, default), v2, v1, (TestObject?)default);
            CheckTaggedEdge(new SEquatableTaggedEdge<TestVertex, TestObject>(v1, v1, default), v1, v1, (TestObject?)default);
            CheckStructTaggedEdge(default(SEquatableTaggedEdge<TestVertex, TestObject>), (TestVertex?)default, default, (TestObject?)default);
            CheckTaggedEdge(new SEquatableTaggedEdge<TestVertex, TestObject>(v1, v2, tag), v1, v2, tag);

            // Struct break the contract with their implicit default constructor
            // Non struct edge should be preferred
            var defaultEdge = default(SEquatableTaggedEdge<TestVertex, int>);
            defaultEdge.Source.Should().BeNull();
            // ReSharper disable once HeuristicUnreachableCode
            // Justification: Since struct has implicit default constructor it allows initialization of invalid edge
            defaultEdge.Target.Should().BeNull();
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new SEquatableTaggedEdge<TestVertex, TestObject>(default, new TestVertex("v1"), default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new SEquatableTaggedEdge<TestVertex, TestObject>(new TestVertex("v1"), default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new SEquatableTaggedEdge<TestVertex, TestObject>(default, default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Equals()
        {
            var tag1 = new TestObject(1);
            var tag2 = new TestObject(2);
            var edge1 = default(SEquatableTaggedEdge<int, TestObject>);
            var edge2 = new SEquatableTaggedEdge<int, TestObject>(0, 0, default);
            var edge3 = new SEquatableTaggedEdge<int, TestObject>(1, 2, default);
            var edge4 = new SEquatableTaggedEdge<int, TestObject>(1, 2, default);
            var edge5 = new SEquatableTaggedEdge<int, TestObject>(2, 1, default);
            var edge6 = new SEquatableTaggedEdge<int, TestObject>(1, 2, tag1);
            var edge7 = new SEquatableTaggedEdge<int, TestObject>(1, 2, tag1);
            var edge8 = new SEquatableTaggedEdge<int, TestObject>(1, 2, tag2);

            edge1.Should().Be(edge1);

            edge2.Should().Be(edge1);
            edge1.Should().Be(edge2);
            edge1.Equals(edge2).Should().BeTrue();
            edge1.Equals(edge2).Should().BeTrue();
            edge2.Equals(edge1).Should().BeTrue();

            edge3.Should().NotBe(edge1);
            edge1.Should().NotBe(edge3);
            edge1.Equals(edge3).Should().BeFalse();
            edge1.Equals(edge3).Should().BeFalse();
            edge3.Equals(edge1).Should().BeFalse();

            edge4.Should().Be(edge3);
            edge3.Should().Be(edge4);
            edge3.Equals(edge4).Should().BeTrue();
            edge3.Equals(edge4).Should().BeTrue();
            edge4.Equals(edge3).Should().BeTrue();

            edge5.Should().NotBe(edge3);
            edge3.Should().NotBe(edge5);
            edge3.Equals(edge5).Should().BeFalse();
            edge3.Equals(edge5).Should().BeFalse();
            edge5.Equals(edge3).Should().BeFalse();

            edge6.Should().Be(edge3);  // Tag is not taken into account for equality
            edge3.Should().Be(edge6);  // Tag is not taken into account for equality
            edge3.Equals(edge6).Should().BeTrue(); // Tag is not taken into account for equality
            edge3.Equals(edge6).Should().BeTrue();  // Tag is not taken into account for equality
            edge6.Equals(edge3).Should().BeTrue();  // Tag is not taken into account for equality

            edge7.Should().Be(edge6);
            edge6.Should().Be(edge7);
            edge6.Equals(edge7).Should().BeTrue();
            edge6.Equals(edge7).Should().BeTrue();
            edge7.Equals(edge6).Should().BeTrue();

            edge8.Should().Be(edge6);  // Tag is not taken into account for equality
            edge6.Should().Be(edge8);  // Tag is not taken into account for equality
            edge6.Equals(edge8).Should().BeTrue(); // Tag is not taken into account for equality
            edge6.Equals(edge8).Should().BeTrue();  // Tag is not taken into account for equality
            edge8.Equals(edge6).Should().BeTrue();  // Tag is not taken into account for equality

            edge1.Should().NotBe(default);
            edge1.Equals(default).Should().BeTrue();
        }

        [Test]
        public void EqualsDefaultEdge_ReferenceTypeExtremities()
        {
            var edge1 = default(SEquatableTaggedEdge<TestVertex, TestObject>);
            var edge2 = new SEquatableTaggedEdge<TestVertex, TestObject>();

            edge2.Should().Be(edge1);
            edge1.Should().Be(edge2);
            edge1.Equals(edge2).Should().BeTrue();
            edge2.Equals(edge1).Should().BeTrue();
        }

        [Test]
        public void Hashcode()
        {
            var tag = new TestObject(1);

            var edge1 = new SEquatableTaggedEdge<int, TestObject>(1, 2, default);
            var edge2 = new SEquatableTaggedEdge<int, TestObject>(1, 2, default);
            var edge3 = new SEquatableTaggedEdge<int, TestObject>(2, 1, default);
            var edge4 = new SEquatableTaggedEdge<int, TestObject>(1, 2, tag);

            edge2.GetHashCode().Should().Be(edge1.GetHashCode());
            edge3.GetHashCode().Should().NotBe(edge1.GetHashCode());
            edge4.GetHashCode().Should().Be(edge1.GetHashCode());  // Tag is not taken into account for hashcode
        }

        [Test]
        public void HashcodeDefaultEdge_ReferenceTypeExtremities()
        {
            var edge1 = default(SEquatableTaggedEdge<TestVertex, TestObject>);
            var edge2 = new SEquatableTaggedEdge<TestVertex, TestObject>();

            edge2.GetHashCode().Should().Be(edge1.GetHashCode());
        }

        [Test]
        public void TagChanged()
        {
            var edge = new SEquatableTaggedEdge<int, TestObject>(1, 2, default);

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
            var edge1 = new SEquatableTaggedEdge<int, TestObject>(1, 2, default);
            var edge2 = new SEquatableTaggedEdge<int, TestObject>(1, 2, new TestObject(42));
            var edge3 = new SEquatableTaggedEdge<int, TestObject>(2, 1, default);

            edge1.ToString().Should().Be("1 -> 2 (no tag)");
            edge2.ToString().Should().Be("1 -> 2 (42)");
            edge3.ToString().Should().Be("2 -> 1 (no tag)");
        }
    }
}
