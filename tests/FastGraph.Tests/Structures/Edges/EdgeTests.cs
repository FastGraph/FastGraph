#nullable enable

using NUnit.Framework;

namespace FastGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="Edge{TVertex}"/>.
    ///</summary>
    [TestFixture]
    internal sealed class EdgeTests : EdgeTestsBase
    {
        [Test]
        public void Construction()
        {
            // Value type
            CheckEdge(new Edge<int>(1, 2), 1, 2);
            CheckEdge(new Edge<int>(2, 1), 2, 1);
            CheckEdge(new Edge<int>(1, 1), 1, 1);

            // Reference type
            var v1 = new TestVertex("v1");
            var v2 = new TestVertex("v2");
            CheckEdge(new Edge<TestVertex>(v1, v2), v1, v2);
            CheckEdge(new Edge<TestVertex>(v2, v1), v2, v1);
            CheckEdge(new Edge<TestVertex>(v1, v1), v1, v1);
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new Edge<TestVertex>(default, new TestVertex("v1"))).Should().Throw<ArgumentNullException>();
            Invoking(() => new Edge<TestVertex>(new TestVertex("v1"), default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new Edge<TestVertex>(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Equals()
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(2, 1);

            edge1.Should().Be(edge1);

            edge2.Should().NotBe(edge1);
            edge1.Should().NotBe(edge2);
            edge1.Equals(edge2).Should().BeFalse();
            edge2.Equals(edge1).Should().BeFalse();

            edge3.Should().NotBe(edge1);
            edge1.Should().NotBe(edge3);
            edge1.Equals(edge2).Should().BeFalse();
            edge2.Equals(edge1).Should().BeFalse();

            edge1.Should().NotBe(default);
            edge1.Equals(default).Should().BeFalse();
        }

        [Test]
        public void ObjectToString()
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(2, 1);

            edge1.ToString().Should().Be("1 -> 2");
            edge2.ToString().Should().Be("2 -> 1");
        }
    }
}
