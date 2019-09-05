using System;
using JetBrains.Annotations;
using NUnit.Framework;

namespace QuikGraph.Tests.Structures
{
    internal partial class GraphTestsBase
    {
        #region Add Vertices & Edges

        protected static void AddVerticesAndEdge_Test(
            [NotNull] IMutableVertexAndEdgeSet<int, Edge<int>> graph)
        {
            int vertexAdded = 0;
            int edgeAdded = 0;

            AssertEmptyGraph(graph);
            graph.VertexAdded += v =>
            {
                Assert.IsNotNull(v);
                ++vertexAdded;
            };
            graph.EdgeAdded += e =>
            {
                Assert.IsNotNull(e);
                ++edgeAdded;
            };

            // Edge 1
            var edge1 = new Edge<int>(1, 2);
            Assert.IsTrue(graph.AddVerticesAndEdge(edge1));
            Assert.AreEqual(2, vertexAdded);
            Assert.AreEqual(1, edgeAdded);
            AssertHasVertices(graph, new[] { 1, 2 });
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 2
            var edge2 = new Edge<int>(1, 3);
            Assert.IsTrue(graph.AddVerticesAndEdge(edge2));
            Assert.AreEqual(3, vertexAdded);
            Assert.AreEqual(2, edgeAdded);
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge1, edge2 });

            // Edge 3
            var edge3 = new Edge<int>(2, 3);
            Assert.IsTrue(graph.AddVerticesAndEdge(edge3));
            Assert.AreEqual(3, vertexAdded);
            Assert.AreEqual(3, edgeAdded);
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge1, edge2, edge3 });
        }

        protected static void AddVerticesAndEdge_Throws_Test<TVertex, TEdge>(
            [NotNull] IMutableVertexAndEdgeSet<TVertex, TEdge> graph)
            where TEdge : class, IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.AddVerticesAndEdge(null));
            AssertEmptyGraph(graph);
        }

        protected static void AddVerticesAndEdge_Throws_EdgesOnly_Test(
            [NotNull] EdgeListGraph<int, Edge<int>> graph)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.AddVerticesAndEdge(null));
            AssertEmptyGraph(graph);
        }

        protected static void AddVerticesAndEdgeRange_Test(
            [NotNull] IMutableVertexAndEdgeSet<int, Edge<int>> graph)
        {
            int vertexAdded = 0;
            int edgeAdded = 0;

            AssertEmptyGraph(graph);
            graph.VertexAdded += v =>
            {
                Assert.IsNotNull(v);
                ++vertexAdded;
            };
            graph.EdgeAdded += e =>
            {
                Assert.IsNotNull(e);
                ++edgeAdded;
            };

            // Edge 1, 2
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            Assert.AreEqual(2, graph.AddVerticesAndEdgeRange(new[] { edge1, edge2 }));
            Assert.AreEqual(3, vertexAdded);
            Assert.AreEqual(2, edgeAdded);
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge1, edge2 });

            // Edge 1, 3
            var edge3 = new Edge<int>(2, 3);
            Assert.AreEqual(1, graph.AddVerticesAndEdgeRange(new[] { edge1, edge3 })); // Showcase the add of only one edge
            Assert.AreEqual(3, vertexAdded);
            Assert.AreEqual(3, edgeAdded);
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge1, edge2, edge3 });
        }

        protected static void AddVerticesAndEdgeRange_EdgesOnly_Test(
            [NotNull] EdgeListGraph<int, Edge<int>> graph)
        {
            int edgeAdded = 0;

            AssertEmptyGraph(graph);
            graph.EdgeAdded += e =>
            {
                Assert.IsNotNull(e);
                ++edgeAdded;
            };

            // Edge 1, 2
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            Assert.AreEqual(2, graph.AddVerticesAndEdgeRange(new[] { edge1, edge2 }));
            Assert.AreEqual(2, edgeAdded);
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge1, edge2 });

            // Edge 1, 3
            var edge3 = new Edge<int>(2, 3);
            Assert.AreEqual(1, graph.AddVerticesAndEdgeRange(new[] { edge1, edge3 })); // Showcase the add of only one edge
            Assert.AreEqual(3, edgeAdded);
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge1, edge2, edge3 });
        }

        protected static void AddVerticesAndEdgeRange_Throws_Test(
            [NotNull] IMutableVertexAndEdgeSet<int, Edge<int>> graph)
        {
            int vertexAdded = 0;
            int edgeAdded = 0;

            AssertEmptyGraph(graph);
            graph.VertexAdded += v =>
            {
                Assert.IsNotNull(v);
                ++vertexAdded;
            };
            graph.EdgeAdded += e =>
            {
                Assert.IsNotNull(e);
                ++edgeAdded;
            };

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.AddVerticesAndEdgeRange(null));

            // Edge 1, 2, 3
            var edge1 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            Assert.Throws<ArgumentNullException>(() => graph.AddVerticesAndEdgeRange(new[] { edge1, null, edge3 }));
            Assert.AreEqual(0, vertexAdded);
            Assert.AreEqual(0, edgeAdded);
            AssertEmptyGraph(graph);
        }

        protected static void AddVerticesAndEdgeRange_Throws_EdgesOnly_Test(
            [NotNull] EdgeListGraph<int, Edge<int>> graph)
        {
            int edgeAdded = 0;

            AssertEmptyGraph(graph);
            graph.EdgeAdded += e =>
            {
                Assert.IsNotNull(e);
                ++edgeAdded;
            };

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.AddVerticesAndEdgeRange(null));

            // Edge 1, 2, 3
            var edge1 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            Assert.Throws<ArgumentNullException>(() => graph.AddVerticesAndEdgeRange(new[] { edge1, null, edge3 }));
            Assert.AreEqual(0, edgeAdded);
            AssertEmptyGraph(graph);
        }

        #endregion
    }
}
