#nullable enable

using NUnit.Framework;

namespace FastGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="SEquatableEdge{TVertex}"/>.
    ///</summary>
    [TestFixture]
    internal sealed class SEquatableEdgeTests : EdgeTestsBase
    {
        [Test]
        public void Construction()
        {
            // Value type
            CheckEdge(new SEquatableEdge<int>(1, 2), 1, 2);
            CheckEdge(new SEquatableEdge<int>(2, 1), 2, 1);
            CheckEdge(new SEquatableEdge<int>(1, 1), 1, 1);

            // Reference type
            var v1 = new TestVertex("v1");
            var v2 = new TestVertex("v2");
            CheckEdge(new SEquatableEdge<TestVertex>(v1, v2), v1, v2);
            CheckEdge(new SEquatableEdge<TestVertex>(v2, v1), v2, v1);
            CheckEdge(new SEquatableEdge<TestVertex>(v1, v1), v1, v1);
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new SEquatableEdge<TestVertex>(default, new TestVertex("v1"))).Should().Throw<ArgumentNullException>();
            Invoking(() => new SEquatableEdge<TestVertex>(new TestVertex("v1"), default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new SEquatableEdge<TestVertex>(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Equals()
        {
            var edge1 = new SEquatableEdge<int>(1, 2);
            var edge2 = new SEquatableEdge<int>(1, 2);
            var edge3 = new SEquatableEdge<int>(2, 1);

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

            edge1.Should().NotBe(default);
            edge1.Equals(default).Should().BeFalse();
        }

        [Test]
        public void EqualsDefaultEdge_ReferenceTypeExtremities()
        {
            var edge1 = default(SEquatableEdge<TestVertex>);
            var edge2 = new SEquatableEdge<TestVertex>();

            edge2.Should().Be(edge1);
            edge1.Should().Be(edge2);
            edge1.Equals(edge2).Should().BeTrue();
            edge2.Equals(edge1).Should().BeTrue();
        }

        [Test]
        public void Hashcode()
        {
            var edge1 = new SEquatableEdge<int>(1, 2);
            var edge2 = new SEquatableEdge<int>(1, 2);
            var edge3 = new SEquatableEdge<int>(2, 1);

            edge2.GetHashCode().Should().Be(edge1.GetHashCode());
            edge3.GetHashCode().Should().NotBe(edge1.GetHashCode());
        }

        [Test]
        public void HashcodeDefaultEdge_ReferenceTypeExtremities()
        {
            var edge1 = default(SEquatableEdge<TestVertex>);
            var edge2 = new SEquatableEdge<TestVertex>();

            edge2.GetHashCode().Should().Be(edge1.GetHashCode());
        }

        [Test]
        public void ObjectToString()
        {
            var edge1 = new SEquatableEdge<int>(1, 2);
            var edge2 = new SEquatableEdge<int>(2, 1);

            edge1.ToString().Should().Be("1 -> 2");
            edge2.ToString().Should().Be("2 -> 1");
        }
    }
}
