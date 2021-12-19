#nullable enable

using NUnit.Framework;
using FastGraph.Algorithms.Condensation;
using FastGraph.Tests.Structures;

namespace FastGraph.Tests.Algorithms.Condensation
{
    /// <summary>
    /// Tests for <see cref="CondensedEdge{TVertex,TEdge,TGraph}"/>.
    ///</summary>
    [TestFixture]
    internal sealed class CondensedEdgeTests : EdgeTestsBase
    {
        [Test]
        public void Construction()
        {
            var graph1 = new AdjacencyGraph<int, Edge<int>>();
            var graph2 = new AdjacencyGraph<int, Edge<int>>();

            // Value type
            CheckEdge(
                new CondensedEdge<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(graph1, graph2),
                graph1,
                graph2);
            CheckEdge(
                new CondensedEdge<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(graph2, graph1),
                graph2,
                graph1);
            CheckEdge(
                new CondensedEdge<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(graph1, graph1),
                graph1,
                graph1);

            // Reference type
            var graph3 = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph4 = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();

            CheckEdge(
                new CondensedEdge<TestVertex, Edge<TestVertex>, AdjacencyGraph<TestVertex, Edge<TestVertex>>>(graph3, graph4),
                graph3,
                graph4);
            CheckEdge(
                new CondensedEdge<TestVertex, Edge<TestVertex>, AdjacencyGraph<TestVertex, Edge<TestVertex>>>(graph4, graph3),
                graph4,
                graph3);
            CheckEdge(
                new CondensedEdge<TestVertex, Edge<TestVertex>, AdjacencyGraph<TestVertex, Edge<TestVertex>>>(graph3, graph3),
                graph3,
                graph3);
        }

        [Test]
        public void Construction_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();

            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new CondensedEdge<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(default, graph)).Should().Throw<ArgumentNullException>();
            Invoking(() => new CondensedEdge<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(graph, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new CondensedEdge<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Edges()
        {
            var graph1 = new AdjacencyGraph<int, Edge<int>>();
            var graph2 = new AdjacencyGraph<int, Edge<int>>();

            var edge = new CondensedEdge<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(graph1, graph2);
            edge.Edges.Should().BeEmpty();

            var subEdge = new Edge<int>(1, 2);
            edge.Edges.Add(subEdge);
            new[] { subEdge }.Should().BeEquivalentTo(edge.Edges);

            edge.Edges.RemoveAt(0);
            edge.Edges.Should().BeEmpty();
        }

        [Test]
        public void Equals()
        {
            var graph1 = new AdjacencyGraph<int, Edge<int>>();
            var graph2 = new AdjacencyGraph<int, Edge<int>>();

            var edge1 = new CondensedEdge<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(graph1, graph2);
            var edge2 = new CondensedEdge<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(graph1, graph2);
            var edge3 = new CondensedEdge<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(graph2, graph1);
            var edge4 = new CondensedEdge<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(graph1, graph2);

            var subEdge = new Edge<int>(1, 2);
            edge4.Edges.Add(subEdge);

            edge1.Should().Be(edge1);
            edge2.Should().NotBe(edge1);
            edge3.Should().NotBe(edge1);
            edge4.Should().NotBe(edge1);

            edge1.Should().NotBe(default);
        }
    }
}
