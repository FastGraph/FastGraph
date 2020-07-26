using System;
using NUnit.Framework;
using QuikGraph.Algorithms.Condensation;
using QuikGraph.Tests.Structures;

namespace QuikGraph.Tests.Algorithms.Condensation
{
    /// <summary>
    /// Tests for <see cref="CondensedEdge{TVertex,TEdge,TGraph}"/>.
    ///</summary>
    [TestFixture]
    internal class CondensedEdgeTests : EdgeTestsBase
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
            Assert.Throws<ArgumentNullException>(() => new CondensedEdge<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(null, graph));
            Assert.Throws<ArgumentNullException>(() => new CondensedEdge<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(graph, null));
            Assert.Throws<ArgumentNullException>(() => new CondensedEdge<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Edges()
        {
            var graph1 = new AdjacencyGraph<int, Edge<int>>();
            var graph2 = new AdjacencyGraph<int, Edge<int>>();

            var edge = new CondensedEdge<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(graph1, graph2);
            CollectionAssert.IsEmpty(edge.Edges);

            var subEdge = new Edge<int>(1, 2);
            edge.Edges.Add(subEdge);
            CollectionAssert.AreEqual(new[] { subEdge }, edge.Edges);

            edge.Edges.RemoveAt(0);
            CollectionAssert.IsEmpty(edge.Edges);
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

            Assert.AreEqual(edge1, edge1);
            Assert.AreNotEqual(edge1, edge2);
            Assert.AreNotEqual(edge1, edge3);
            Assert.AreNotEqual(edge1, edge4);

            Assert.AreNotEqual(edge1, null);
        }
    }
}