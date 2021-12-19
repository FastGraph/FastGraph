#nullable enable

using NUnit.Framework;

namespace FastGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="SEdge{TVertex}"/>.
    ///</summary>
    [TestFixture]
    internal sealed class SEdgeTests : EdgeTestsBase
    {
        [Test]
        public void Construction()
        {
            // Value type
            CheckEdge(new SEdge<int>(1, 2), 1, 2);
            CheckEdge(new SEdge<int>(2, 1), 2, 1);
            CheckEdge(new SEdge<int>(1, 1), 1, 1);
            CheckEdge(default(SEdge<int>), 0, 0);

            // Reference type
            var v1 = new TestVertex("v1");
            var v2 = new TestVertex("v2");
            CheckEdge(new SEdge<TestVertex>(v1, v2), v1, v2);
            CheckEdge(new SEdge<TestVertex>(v2, v1), v2, v1);
            CheckEdge(new SEdge<TestVertex>(v1, v1), v1, v1);

            // Struct break the contract with their implicit default constructor
            // Non struct edge should be preferred
            var defaultEdge = default(SEdge<TestVertex>);
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
            Invoking(() => new SEdge<TestVertex>(default, new TestVertex("v1"))).Should().Throw<ArgumentNullException>();
            Invoking(() => new SEdge<TestVertex>(new TestVertex("v1"), default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new SEdge<TestVertex>(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Equals()
        {
            var edge1 = default(SEdge<int>);
            var edge2 = new SEdge<int>(0, 0);
            var edge3 = new SEdge<int>(1, 2);
            var edge4 = new SEdge<int>(1, 2);
            var edge5 = new SEdge<int>(2, 1);

            edge1.Should().Be(edge1);

            edge2.Should().Be(edge1);  // Is equatable
            edge1.Should().Be(edge2);  // Is equatable
            edge1.Equals(edge2).Should().BeTrue();  // Is equatable
            edge2.Equals(edge1).Should().BeTrue();  // Is equatable

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

            edge1.Equals(default).Should().BeFalse();
        }

        [Test]
        public void EqualsDefaultEdge_ReferenceTypeExtremities()
        {
            var edge1 = default(SEdge<TestVertex>);
            var edge2 = new SEdge<TestVertex>();

            edge2.Should().Be(edge1);
            edge1.Should().Be(edge2);
            edge1.Equals(edge2).Should().BeTrue();
            edge2.Equals(edge1).Should().BeTrue();
        }

        [Test]
        public void ObjectToString()
        {
            var edge1 = new SEdge<int>(1, 2);
            var edge2 = new SEdge<int>(2, 1);

            edge1.ToString().Should().Be("1 -> 2");
            edge2.ToString().Should().Be("2 -> 1");
        }
    }
}
