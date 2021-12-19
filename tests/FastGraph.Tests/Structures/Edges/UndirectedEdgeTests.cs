#nullable enable

using NUnit.Framework;

namespace FastGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="UndirectedEdge{TVertex}"/>.
    ///</summary>
    [TestFixture]
    internal sealed class UndirectedEdgeTests : EdgeTestsBase
    {
        [Test]
        public void Construction()
        {
            // Value type
            CheckEdge(new UndirectedEdge<int>(1, 2), 1, 2);
            CheckEdge(new UndirectedEdge<int>(1, 1), 1, 1);

            // Reference type
            var v1 = new ComparableTestVertex("v1");
            var v2 = new ComparableTestVertex("v2");
            CheckEdge(new UndirectedEdge<ComparableTestVertex>(v1, v2), v1, v2);
            CheckEdge(new UndirectedEdge<ComparableTestVertex>(v1, v1), v1, v1);
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new UndirectedEdge<TestVertex>(default, new TestVertex("v1"))).Should().Throw<ArgumentNullException>();
            Invoking(() => new UndirectedEdge<TestVertex>(new TestVertex("v1"), default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new UndirectedEdge<TestVertex>(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute

            Invoking(() => new UndirectedEdge<int>(2, 1)).Should().Throw<ArgumentException>();

            // Not comparable
            var v1 = new TestVertex("v1");
            var v2 = new TestVertex("v2");
            Invoking(() => new UndirectedEdge<TestVertex>(v1, v2)).Should().Throw<ArgumentException>();

            var comparableV1 = new ComparableTestVertex("v1");
            var comparableV2 = new ComparableTestVertex("v2");
            Invoking(() => new UndirectedEdge<ComparableTestVertex>(comparableV2, comparableV1)).Should().Throw<ArgumentException>();
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Equals()
        {
            var edge1 = new UndirectedEdge<int>(1, 2);
            var edge2 = new UndirectedEdge<int>(1, 2);

            edge1.Should().Be(edge1);

            edge2.Should().NotBe(edge1);
            edge1.Should().NotBe(edge2);
            edge1.Equals(edge2).Should().BeFalse();
            edge2.Equals(edge1).Should().BeFalse();

            edge1.Should().NotBe(default);
            edge1.Equals(default).Should().BeFalse();
        }

        [Test]
        public void ObjectToString()
        {
            var edge = new UndirectedEdge<int>(1, 2);

            edge.ToString().Should().Be("1 <-> 2");
        }
    }
}
