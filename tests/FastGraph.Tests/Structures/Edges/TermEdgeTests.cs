#nullable enable

using NUnit.Framework;

namespace FastGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="TermEdge{TVertex}"/>.
    ///</summary>
    [TestFixture]
    internal sealed class TermEdgeTests : EdgeTestsBase
    {
        [Test]
        public void Construction()
        {
            // Value type
            CheckEdge(new TermEdge<int>(1, 2), 1, 2);
            CheckEdge(new TermEdge<int>(2, 1), 2, 1);
            CheckEdge(new TermEdge<int>(1, 1), 1, 1);

            CheckTermEdge(new TermEdge<int>(1, 2, 0, 1), 1, 2, 0, 1);
            CheckTermEdge(new TermEdge<int>(2, 1, 1, 0), 2, 1, 1, 0);
            CheckTermEdge(new TermEdge<int>(1, 1, 0, 0), 1, 1, 0, 0);

            // Reference type
            var v1 = new TestVertex("v1");
            var v2 = new TestVertex("v2");
            CheckEdge(new TermEdge<TestVertex>(v1, v2), v1, v2);
            CheckEdge(new TermEdge<TestVertex>(v2, v1), v2, v1);
            CheckEdge(new TermEdge<TestVertex>(v1, v1), v1, v1);

            CheckTermEdge(new TermEdge<TestVertex>(v1, v2, 0, 1), v1, v2, 0, 1);
            CheckTermEdge(new TermEdge<TestVertex>(v2, v1, 1, 0), v2, v1, 1, 0);
            CheckTermEdge(new TermEdge<TestVertex>(v1, v1, 0, 0), v1, v1, 0, 0);
        }

        [Test]
        public void Construction_Throws()
        {
            var v1 = new TestVertex("v1");
            var v2 = new TestVertex("v2");
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new TermEdge<TestVertex>(default, v1)).Should().Throw<ArgumentNullException>();
            Invoking(() => new TermEdge<TestVertex>(v1, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new TermEdge<TestVertex>(default, default)).Should().Throw<ArgumentNullException>();

            Invoking(() => new TermEdge<TestVertex>(default, v1, 0, 1)).Should().Throw<ArgumentNullException>();
            Invoking(() => new TermEdge<TestVertex>(v1, default, 0, 1)).Should().Throw<ArgumentNullException>();
            Invoking(() => new TermEdge<TestVertex>(default, default, 0, 1)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute

            Invoking(() => new TermEdge<TestVertex>(v1, v2, -1, 0)).Should().Throw<ArgumentException>();
            Invoking(() => new TermEdge<TestVertex>(v1, v2, 0, -1)).Should().Throw<ArgumentException>();
            Invoking(() => new TermEdge<TestVertex>(v1, v2, -1, -1)).Should().Throw<ArgumentException>();
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Equals()
        {
            var edge1 = new TermEdge<int>(1, 2);
            var edge2 = new TermEdge<int>(1, 2);
            var edge3 = new TermEdge<int>(1, 2, 0, 0);
            var edge4 = new TermEdge<int>(1, 2, 0, 0);
            var edge5 = new TermEdge<int>(1, 2, 0, 1);
            var edge6 = new TermEdge<int>(1, 2, 0, 1);

            edge1.Should().Be(edge1);
            edge3.Should().Be(edge3);
            edge5.Should().Be(edge5);

            edge2.Should().NotBe(edge1);
            edge1.Should().NotBe(edge2);
            edge1.Equals(edge2).Should().BeFalse();
            edge2.Equals(edge1).Should().BeFalse();

            edge3.Should().NotBe(edge1);
            edge1.Should().NotBe(edge3);
            edge1.Equals(edge3).Should().BeFalse();
            edge3.Equals(edge1).Should().BeFalse();

            edge5.Should().NotBe(edge1);
            edge1.Should().NotBe(edge5);
            edge1.Equals(edge5).Should().BeFalse();
            edge5.Equals(edge1).Should().BeFalse();

            edge4.Should().NotBe(edge3);
            edge3.Should().NotBe(edge4);
            edge3.Equals(edge4).Should().BeFalse();
            edge4.Equals(edge3).Should().BeFalse();

            edge6.Should().NotBe(edge5);
            edge5.Should().NotBe(edge6);
            edge5.Equals(edge6).Should().BeFalse();
            edge6.Equals(edge5).Should().BeFalse();

            edge1.Equals(default).Should().BeFalse();
        }

        [Test]
        public void ObjectToString()
        {
            var edge1 = new TermEdge<int>(1, 2);
            var edge2 = new TermEdge<int>(1, 2, 1, 5);
            var edge3 = new TermEdge<int>(2, 1);

            edge1.ToString().Should().Be("1 (0) -> 2 (0)");
            edge2.ToString().Should().Be("1 (1) -> 2 (5)");
            edge3.ToString().Should().Be("2 (0) -> 1 (0)");
        }
    }
}
