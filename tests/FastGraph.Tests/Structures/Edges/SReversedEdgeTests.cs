#nullable enable

using NUnit.Framework;

namespace FastGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="SReversedEdge{TVertex,TEdge}"/>.
    ///</summary>
    [TestFixture]
    internal sealed class SReversedEdgeTests : EdgeTestsBase
    {
        [Test]
        public void Construction()
        {
            // Value type
            CheckEdge(new SReversedEdge<int, Edge<int>>(new Edge<int>(1, 2)), 2, 1);
            CheckEdge(new SReversedEdge<int, Edge<int>>(new Edge<int>(2, 1)), 1, 2);
            CheckEdge(new SReversedEdge<int, Edge<int>>(new Edge<int>(1, 1)), 1, 1);

            // Struct break the contract with their implicit default constructor
            var defaultEdge = default(SReversedEdge<int, Edge<int>>);
            // ReSharper disable HeuristicUnreachableCode
            // Justification: Since struct has implicit default constructor it allows initialization of invalid edge
            defaultEdge.OriginalEdge.Should().BeNull();
            // ReSharper disable  HeuristicUnreachableCode
            Invoking(() => { int _ = defaultEdge.Source; }).Should().Throw<NullReferenceException>();
            Invoking(() => { int _ = defaultEdge.Target; }).Should().Throw<NullReferenceException>();
            // ReSharper restore HeuristicUnreachableCode

            // Reference type
            var v1 = new TestVertex("v1");
            var v2 = new TestVertex("v2");
            CheckEdge(new SReversedEdge<TestVertex, Edge<TestVertex>>(new Edge<TestVertex>(v1, v2)), v2, v1);
            CheckEdge(new SReversedEdge<TestVertex, Edge<TestVertex>>(new Edge<TestVertex>(v2, v1)), v1, v2);
            CheckEdge(new SReversedEdge<TestVertex, Edge<TestVertex>>(new Edge<TestVertex>(v1, v1)), v1, v1);

            // Struct break the contract with their implicit default constructor
            var defaultEdge2 = default(SReversedEdge<TestVertex, Edge<TestVertex>>);
            // ReSharper disable HeuristicUnreachableCode
            // Justification: Since struct has implicit default constructor it allows initialization of invalid edge
            defaultEdge2.OriginalEdge.Should().BeNull();
            // ReSharper disable  HeuristicUnreachableCode
            Invoking(() => { TestVertex _ = defaultEdge2.Source; }).Should().Throw<NullReferenceException>();
            Invoking(() => { TestVertex _ = defaultEdge2.Target; }).Should().Throw<NullReferenceException>();
            // ReSharper restore HeuristicUnreachableCode
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new SReversedEdge<TestVertex, Edge<TestVertex>>(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        [Test]
        public void Equals()
        {
            var wrappedEdge = new Edge<int>(1, 2);
            var edge1 = new SReversedEdge<int, Edge<int>>(wrappedEdge);
            var edge2 = new SReversedEdge<int, Edge<int>>(wrappedEdge);
            var edge3 = new SReversedEdge<int, Edge<int>>(new Edge<int>(1, 2));
            var edge4 = new SReversedEdge<int, Edge<int>>(new Edge<int>(2, 1));

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

            edge4.Should().NotBe(edge1);
            edge1.Should().NotBe(edge4);
            edge1.Equals(edge4).Should().BeFalse();
            edge1.Equals(edge4).Should().BeFalse();
            edge4.Equals(edge1).Should().BeFalse();

            edge1.Equals(default).Should().BeFalse();
        }

        [Test]
        public void EqualsDefaultEdge_ReferenceTypeExtremities()
        {
            var edge1 = default(SReversedEdge<int, Edge<int>>);
            var edge2 = new SReversedEdge<int, Edge<int>>();

            edge2.Should().Be(edge1);
            edge1.Should().Be(edge2);
            edge1.Equals(edge2).Should().BeTrue();
            edge2.Equals(edge1).Should().BeTrue();
        }

        [Test]
        public void Equals2()
        {
            var edge1 = new SReversedEdge<int, EquatableEdge<int>>(new EquatableEdge<int>(1, 2));
            var edge2 = new SReversedEdge<int, EquatableEdge<int>>(new EquatableEdge<int>(1, 2));
            var edge3 = new SReversedEdge<int, EquatableEdge<int>>(new EquatableEdge<int>(2, 1));

            edge1.Should().Be(edge1);
            edge2.Should().Be(edge1);
            edge1.Equals(edge2).Should().BeTrue();
            edge3.Should().NotBe(edge1);

            edge1.Equals(default).Should().BeFalse();
            edge1.Should().NotBe(default);
        }

        [Test]
        public void Hashcode()
        {
            var wrappedEdge = new Edge<int>(1, 2);
            var edge1 = new SReversedEdge<int, Edge<int>>(wrappedEdge);
            var edge2 = new SReversedEdge<int, Edge<int>>(wrappedEdge);
            var edge3 = new SReversedEdge<int, Edge<int>>(new Edge<int>(1, 2));
            var edge4 = new SReversedEdge<int, Edge<int>>(new Edge<int>(2, 1));

            edge2.GetHashCode().Should().Be(edge1.GetHashCode());
            edge3.GetHashCode().Should().NotBe(edge1.GetHashCode());
            edge4.GetHashCode().Should().NotBe(edge1.GetHashCode());
        }

        [Test]
        public void HashcodeDefaultEdge_ReferenceTypeExtremities()
        {
            var edge1 = default(SReversedEdge<int, Edge<int>>);
            var edge2 = new SReversedEdge<int, Edge<int>>();

            edge2.GetHashCode().Should().Be(edge1.GetHashCode());
        }

        [Test]
        public void ObjectToString()
        {
            var edge1 = new SReversedEdge<int, Edge<int>>(new Edge<int>(1, 2));
            var edge2 = new SReversedEdge<int, Edge<int>>(new Edge<int>(2, 1));
            var edge3 = new SReversedEdge<int, UndirectedEdge<int>>(new UndirectedEdge<int>(1, 2));

            edge1.ToString().Should().Be("R(1 -> 2)");
            edge2.ToString().Should().Be("R(2 -> 1)");
            edge3.ToString().Should().Be("R(1 <-> 2)");
        }
    }
}
