#nullable enable

using NUnit.Framework;
using FastGraph.Algorithms.Condensation;
using FastGraph.Tests.Structures;

namespace FastGraph.Tests.Algorithms.Condensation
{
    /// <summary>
    /// Tests for <see cref="MergedEdge{TVertex,TEdge}"/>.
    ///</summary>
    [TestFixture]
    internal sealed class MergedEdgeTests : EdgeTestsBase
    {
        [Test]
        public void Construction()
        {
            // Value type
            CheckEdge(new MergedEdge<int, Edge<int>>(1, 2), 1, 2);
            CheckEdge(new MergedEdge<int, Edge<int>>(2, 1), 2, 1);
            CheckEdge(new MergedEdge<int, Edge<int>>(1, 1), 1, 1);

            // Reference type
            var v1 = new TestVertex("v1");
            var v2 = new TestVertex("v2");
            CheckEdge(new MergedEdge<TestVertex, Edge<TestVertex>>(v1, v2), v1, v2);
            CheckEdge(new MergedEdge<TestVertex, Edge<TestVertex>>(v2, v1), v2, v1);
            CheckEdge(new MergedEdge<TestVertex, Edge<TestVertex>>(v1, v1), v1, v1);
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new MergedEdge<TestVertex, Edge<TestVertex>>(default, new TestVertex("v1"))).Should().Throw<ArgumentNullException>();
            Invoking(() => new MergedEdge<TestVertex, Edge<TestVertex>>(new TestVertex("v1"), default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MergedEdge<TestVertex, Edge<TestVertex>>(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Edges()
        {
            var edge = new MergedEdge<int, Edge<int>>(1, 2);
            edge.Edges.Should().BeEmpty();

            var subEdge1 = new Edge<int>(1, 2);
            edge.Edges.Add(subEdge1);
            new[] { subEdge1 }.Should().BeEquivalentTo(edge.Edges);

            var subEdge2 = new MergedEdge<int, Edge<int>>(1, 2);
            edge.Edges.Add(subEdge2);
            new[] { subEdge1, subEdge2 }.Should().BeEquivalentTo(edge.Edges);

            edge.Edges.RemoveAt(1);
            new[] { subEdge1 }.Should().BeEquivalentTo(edge.Edges);

            edge.Edges.Remove(subEdge1);
            edge.Edges.Should().BeEmpty();
        }

        [Test]
        public void Merge()
        {
            var emptyEdge1 = new MergedEdge<int, Edge<int>>(1, 2);
            var emptyEdge2 = new MergedEdge<int, Edge<int>>(1, 2);
            var subEdge1 = new Edge<int>(1, 2);
            var subEdge2 = new Edge<int>(1, 2);
            var subEdge3 = new Edge<int>(1, 2);
            var edge1 = new MergedEdge<int, Edge<int>>(1, 2);
            edge1.Edges.Add(subEdge1);
            var edge2 = new MergedEdge<int, Edge<int>>(1, 2);
            edge2.Edges.Add(subEdge2);
            edge2.Edges.Add(subEdge3);

            var mergedEdge = MergedEdge.Merge(emptyEdge1, emptyEdge2);
            mergedEdge.Should().NotBeNull();
            mergedEdge.Edges.Should().BeEmpty();

            mergedEdge = MergedEdge.Merge(emptyEdge1, edge1);
            mergedEdge.Should().NotBeNull();
            new[] { subEdge1 }.Should().BeEquivalentTo(mergedEdge.Edges);

            mergedEdge = MergedEdge.Merge(edge1, emptyEdge1);
            mergedEdge.Should().NotBeNull();
            new[] { subEdge1 }.Should().BeEquivalentTo(mergedEdge.Edges);

            mergedEdge = MergedEdge.Merge(edge1, edge2);
            mergedEdge.Should().NotBeNull();
            new[] { subEdge1, subEdge2, subEdge3 }.Should().BeEquivalentTo(mergedEdge.Edges);
        }

        [Test]
        public void Merge_Throws()
        {
            var edge = new MergedEdge<int, Edge<int>>(1, 2);

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => MergedEdge.Merge(edge, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => MergedEdge.Merge(default, edge)).Should().Throw<ArgumentNullException>();
            Invoking(() => MergedEdge.Merge<int, Edge<int>>(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void Equals()
        {
            var edge1 = new MergedEdge<int, Edge<int>>(1, 2);
            var edge2 = new MergedEdge<int, Edge<int>>(1, 2);
            var edge3 = new MergedEdge<int, Edge<int>>(2, 1);
            var edge4 = new MergedEdge<int, Edge<int>>(1, 2);
            edge4.Edges.Add(edge1);

            edge1.Should().Be(edge1);
            edge2.Should().NotBe(edge1);
            edge3.Should().NotBe(edge1);
            edge4.Should().NotBe(edge1);

            edge1.Should().NotBe(default);
        }

        [Test]
        public void ObjectToString()
        {
            var edge1 = new MergedEdge<int, Edge<int>>(1, 2);
            var edge2 = new MergedEdge<int, Edge<int>>(2, 1);

            edge1.ToString().Should().Be("1 -> 2");
            edge2.ToString().Should().Be("2 -> 1");
        }
    }
}
