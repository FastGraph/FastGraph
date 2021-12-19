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
            Invoking(() => new STaggedEdge<TestVertex, TestObject>(default, new TestVertex("v1"), default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new STaggedEdge<TestVertex, TestObject>(new TestVertex("v1"), default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new STaggedEdge<TestVertex, TestObject>(default, default, default)).Should().Throw<ArgumentNullException>();
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

            edge1.Should().Be(edge1);

            edge2.Should().Be(edge1);
            edge1.Should().Be(edge2);
            edge1.Equals(edge2).Should().BeTrue();
            edge2.Equals(edge1).Should().BeTrue();

            edge3.Should().NotBe(edge1);
            edge1.Should().NotBe(edge3);
            edge1.Equals(edge3).Should().BeFalse();
            edge3.Equals(edge1).Should().BeFalse();

            edge4.Should().Be(edge3);
            edge3.Should().Be(edge4);
            edge3.Equals(edge4).Should().BeTrue();
            edge4.Equals(edge3).Should().BeTrue();

            edge5.Should().NotBe(edge3);
            edge3.Should().NotBe(edge5);
            edge3.Equals(edge5).Should().BeFalse();
            edge5.Equals(edge3).Should().BeFalse();

            edge6.Should().NotBe(edge3);
            edge3.Should().NotBe(edge6);
            edge3.Equals(edge6).Should().BeFalse();
            edge6.Equals(edge3).Should().BeFalse();

            edge7.Should().Be(edge6);
            edge6.Should().Be(edge7);
            edge6.Equals(edge7).Should().BeTrue();
            edge7.Equals(edge6).Should().BeTrue();

            edge8.Should().NotBe(edge6);
            edge6.Should().NotBe(edge8);
            edge6.Equals(edge8).Should().BeFalse();
            edge8.Equals(edge6).Should().BeFalse();

            edge1.Equals(default).Should().BeFalse();
        }

        [Test]
        public void EqualsDefaultEdge_ReferenceTypeExtremities()
        {
            var edge1 = default(STaggedEdge<int, TestObject>);
            var edge2 = new STaggedEdge<int, TestObject>();

            edge2.Should().Be(edge1);
            edge1.Should().Be(edge2);
            edge1.Equals(edge2).Should().BeTrue();
            edge2.Equals(edge1).Should().BeTrue();
        }

        [Test]
        public void TagChanged()
        {
            var edge = new STaggedEdge<int, TestObject>(1, 2, default);

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
            var edge1 = new STaggedEdge<int, TestObject>(1, 2, default);
            var edge2 = new STaggedEdge<int, TestObject>(1, 2, new TestObject(42));
            var edge3 = new STaggedEdge<int, TestObject>(2, 1, default);

            edge1.ToString().Should().Be("1 -> 2 (no tag)");
            edge2.ToString().Should().Be("1 -> 2 (42)");
            edge3.ToString().Should().Be("2 -> 1 (no tag)");
        }
    }
}
