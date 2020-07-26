using System;
using JetBrains.Annotations;
using NUnit.Framework;

namespace QuikGraph.Tests.Structures
{
    internal partial class GraphTestsBase
    {
        #region Degree

        protected static void Degree_Test(
            [NotNull] IMutableBidirectionalGraph<int, Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(1, 4);
            var edge4 = new Edge<int>(2, 4);
            var edge5 = new Edge<int>(3, 2);
            var edge6 = new Edge<int>(3, 3);

            graph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });
            graph.AddVertex(5);

            Assert.AreEqual(3, graph.Degree(1));
            Assert.AreEqual(3, graph.Degree(2));
            Assert.AreEqual(4, graph.Degree(3)); // Self edge
            Assert.AreEqual(2, graph.Degree(4));
            Assert.AreEqual(0, graph.Degree(5));
        }

        protected static void Degree_ImmutableGraph_Test(
            [NotNull] IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [NotNull, InstantHandle] Func<IBidirectionalIncidenceGraph<int, Edge<int>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(1, 4);
            var edge4 = new Edge<int>(2, 4);
            var edge5 = new Edge<int>(3, 2);
            var edge6 = new Edge<int>(3, 3);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });
            wrappedGraph.AddVertex(5);
            IBidirectionalIncidenceGraph<int, Edge<int>> graph = createGraph();

            Assert.AreEqual(3, graph.Degree(1));
            Assert.AreEqual(3, graph.Degree(2));
            Assert.AreEqual(4, graph.Degree(3)); // Self edge
            Assert.AreEqual(2, graph.Degree(4));
            Assert.AreEqual(0, graph.Degree(5));
        }

        protected static void Degree_ImmutableVertices_Test(
            [NotNull] BidirectionalMatrixGraph<Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(1, 4);
            var edge4 = new Edge<int>(2, 4);
            var edge5 = new Edge<int>(3, 2);
            var edge6 = new Edge<int>(3, 3);

            graph.AddEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });

            Assert.AreEqual(0, graph.Degree(0));
            Assert.AreEqual(3, graph.Degree(1));
            Assert.AreEqual(3, graph.Degree(2));
            Assert.AreEqual(4, graph.Degree(3)); // Self edge
            Assert.AreEqual(2, graph.Degree(4));
        }

        protected static void Degree_ImmutableGraph_ReversedTest(
            [NotNull] IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [NotNull, InstantHandle] Func<IBidirectionalIncidenceGraph<int, SReversedEdge<int, Edge<int>>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(1, 4);
            var edge4 = new Edge<int>(2, 4);
            var edge5 = new Edge<int>(3, 2);
            var edge6 = new Edge<int>(3, 3);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });
            wrappedGraph.AddVertex(5);
            IBidirectionalIncidenceGraph<int, SReversedEdge<int, Edge<int>>> graph = createGraph();

            Assert.AreEqual(3, graph.Degree(1));
            Assert.AreEqual(3, graph.Degree(2));
            Assert.AreEqual(4, graph.Degree(3)); // Self edge
            Assert.AreEqual(2, graph.Degree(4));
            Assert.AreEqual(0, graph.Degree(5));
        }

        protected static void Degree_Throws_Test<TVertex, TEdge>(
            [NotNull] IBidirectionalIncidenceGraph<TVertex, TEdge> graph)
            where TVertex : class, IEquatable<TVertex>, new()
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.Degree(null));
            Assert.Throws<VertexNotFoundException>(() => graph.Degree(new TVertex()));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        protected static void Degree_Throws_Matrix_Test<TEdge>(
            [NotNull] BidirectionalMatrixGraph<TEdge> graph)
            where TEdge : class, IEdge<int>
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<VertexNotFoundException>(() => graph.Degree(-1));
            Assert.Throws<VertexNotFoundException>(() => graph.Degree(10));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        #endregion
    }
}