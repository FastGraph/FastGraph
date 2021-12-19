#nullable enable

using NUnit.Framework;

namespace FastGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="EquatableUndirectedEdge{TVertex}"/>.
    ///</summary>
    [TestFixture]
    internal sealed class EquatableUndirectedEdgeTests : EdgeTestsBase
    {
        [Test]
        public void Construction()
        {
            // Value type
            CheckEdge(new EquatableUndirectedEdge<int>(1, 2), 1, 2);
            CheckEdge(new EquatableUndirectedEdge<int>(1, 1), 1, 1);

            // Reference type
            var v1 = new ComparableTestVertex("v1");
            var v2 = new ComparableTestVertex("v2");
            CheckEdge(new EquatableUndirectedEdge<ComparableTestVertex>(v1, v2), v1, v2);
            CheckEdge(new EquatableUndirectedEdge<ComparableTestVertex>(v1, v1), v1, v1);
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new EquatableUndirectedEdge<TestVertex>(default, new TestVertex("v1"))).Should().Throw<ArgumentNullException>();
            Invoking(() => new EquatableUndirectedEdge<TestVertex>(new TestVertex("v1"), default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EquatableUndirectedEdge<TestVertex>(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute

            Invoking(() => new EquatableUndirectedEdge<int>(2, 1)).Should().Throw<ArgumentException>();

            // Not comparable
            var v1 = new TestVertex("v1");
            var v2 = new TestVertex("v2");
            Invoking(() => new EquatableUndirectedEdge<TestVertex>(v1, v2)).Should().Throw<ArgumentException>();

            var comparableV1 = new ComparableTestVertex("v1");
            var comparableV2 = new ComparableTestVertex("v2");
            Invoking(() => new EquatableUndirectedEdge<ComparableTestVertex>(comparableV2, comparableV1)).Should().Throw<ArgumentException>();
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Equals()
        {
            var edge1 = new EquatableUndirectedEdge<int>(1, 2);
            var edge2 = new EquatableUndirectedEdge<int>(1, 2);

            edge1.Should().Be(edge1);

            edge2.Should().Be(edge1);
            edge1.Should().Be(edge2);
            edge1.Equals(edge2).Should().BeTrue();
            edge1.Equals(edge2).Should().BeTrue();
            edge2.Equals(edge1).Should().BeTrue();

            edge1.Should().NotBe(default);
            edge1.Equals(default).Should().BeFalse();
        }

        [Test]
        public void Hashcode()
        {
            var edge1 = new EquatableUndirectedEdge<int>(1, 2);
            var edge2 = new EquatableUndirectedEdge<int>(1, 2);

            edge2.GetHashCode().Should().Be(edge1.GetHashCode());
        }

        [Test]
        public void ObjectToString()
        {
            var edge = new EquatableUndirectedEdge<int>(1, 2);

            edge.ToString().Should().Be("1 <-> 2");
        }
    }
}
