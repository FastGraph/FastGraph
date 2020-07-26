using System;
using JetBrains.Annotations;
using NUnit.Framework;
using static QuikGraph.Tests.GraphTestHelpers;

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

        protected static void AddVerticesAndEdge_Clusters_Test(
            [NotNull] ClusteredAdjacencyGraph<int, Edge<int>> graph1,
            [NotNull] ClusteredAdjacencyGraph<int, Edge<int>> parent2,
            [NotNull] ClusteredAdjacencyGraph<int, Edge<int>> graph2)
        {
            // Graph without parent
            AssertEmptyGraph(graph1);

            // Edge 1
            var edge1 = new Edge<int>(1, 2);
            Assert.IsTrue(graph1.AddVerticesAndEdge(edge1));
            AssertHasVertices(graph1, new[] { 1, 2 });
            AssertHasEdges(graph1, new[] { edge1 });

            // Edge 2
            var edge2 = new Edge<int>(1, 3);
            Assert.IsTrue(graph1.AddVerticesAndEdge(edge2));
            AssertHasVertices(graph1, new[] { 1, 2, 3 });
            AssertHasEdges(graph1, new[] { edge1, edge2 });

            // Edge 3
            var edge3 = new Edge<int>(2, 3);
            Assert.IsTrue(graph1.AddVerticesAndEdge(edge3));
            AssertHasVertices(graph1, new[] { 1, 2, 3 });
            AssertHasEdges(graph1, new[] { edge1, edge2, edge3 });


            // Graph with parent
            AssertEmptyGraph(parent2);
            AssertEmptyGraph(graph2);

            // Edge 1
            Assert.IsTrue(graph2.AddVerticesAndEdge(edge1));
            AssertHasVertices(parent2, new[] { 1, 2 });
            AssertHasVertices(graph2, new[] { 1, 2 });
            AssertHasEdges(parent2, new[] { edge1 });
            AssertHasEdges(graph2, new[] { edge1 });

            // Edge 2
            Assert.IsTrue(parent2.AddVerticesAndEdge(edge2));
            AssertHasVertices(parent2, new[] { 1, 2, 3 });
            AssertHasVertices(graph2, new[] { 1, 2 });
            AssertHasEdges(parent2, new[] { edge1, edge2 });
            AssertHasEdges(graph2, new[] { edge1 });

            Assert.IsTrue(graph2.AddVerticesAndEdge(edge2));
            AssertHasVertices(parent2, new[] { 1, 2, 3 });
            AssertHasVertices(graph2, new[] { 1, 2, 3 });
            AssertHasEdges(parent2, new[] { edge1, edge2 });
            AssertHasEdges(graph2, new[] { edge1, edge2 });

            // Edge 3
            Assert.IsTrue(graph2.AddVerticesAndEdge(edge3));
            AssertHasVertices(parent2, new[] { 1, 2, 3 });
            AssertHasVertices(graph2, new[] { 1, 2, 3 });
            AssertHasEdges(parent2, new[] { edge1, edge2, edge3 });
            AssertHasEdges(graph2, new[] { edge1, edge2, edge3 });
        }

        protected static void AddVerticesAndEdge_Throws_Test<TVertex, TEdge>(
            [NotNull] IMutableVertexAndEdgeSet<TVertex, TEdge> graph)
            where TEdge : class, IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.AddVerticesAndEdge(null));
            AssertEmptyGraph(graph);
        }

        protected static void AddVerticesAndEdge_Throws_EdgesOnly_Test<TVertex, TEdge>(
            [NotNull] EdgeListGraph<TVertex, TEdge> graph)
            where TEdge : class, IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.AddVerticesAndEdge(null));
            AssertEmptyGraph(graph);
        }

        protected static void AddVerticesAndEdge_Throws_Clusters_Test<TVertex, TEdge>(
            [NotNull] ClusteredAdjacencyGraph<TVertex, TEdge> graph)
            where TEdge : class, IEdge<TVertex>
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

        protected static void AddVerticesAndEdgeRange_Clusters_Test(
            [NotNull] ClusteredAdjacencyGraph<int, Edge<int>> graph1,
            [NotNull] ClusteredAdjacencyGraph<int, Edge<int>> parent2,
            [NotNull] ClusteredAdjacencyGraph<int, Edge<int>> graph2)
        {
            // Graph without parent
            AssertEmptyGraph(graph1);

            // Edge 1, 2
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            Assert.AreEqual(2, graph1.AddVerticesAndEdgeRange(new[] { edge1, edge2 }));
            AssertHasVertices(graph1, new[] { 1, 2, 3 });
            AssertHasEdges(graph1, new[] { edge1, edge2 });

            // Edge 1, 3
            var edge3 = new Edge<int>(2, 3);
            Assert.AreEqual(1, graph1.AddVerticesAndEdgeRange(new[] { edge1, edge3 })); // Showcase the add of only one edge
            AssertHasVertices(graph1, new[] { 1, 2, 3 });
            AssertHasEdges(graph1, new[] { edge1, edge2, edge3 });


            // Graph with parent
            AssertEmptyGraph(parent2);
            AssertEmptyGraph(graph2);

            // Edge 1, 2
            Assert.AreEqual(2, graph2.AddVerticesAndEdgeRange(new[] { edge1, edge2 }));
            AssertHasVertices(parent2, new[] { 1, 2, 3 });
            AssertHasVertices(graph2, new[] { 1, 2, 3 });
            AssertHasEdges(parent2, new[] { edge1, edge2 });
            AssertHasEdges(graph2, new[] { edge1, edge2 });

            // Edge 1, 3
            Assert.AreEqual(1, parent2.AddVerticesAndEdgeRange(new[] { edge1, edge3 })); // Showcase the add of only one edge
            AssertHasVertices(parent2, new[] { 1, 2, 3 });
            AssertHasVertices(graph2, new[] { 1, 2, 3 });
            AssertHasEdges(parent2, new[] { edge1, edge2, edge3 });
            AssertHasEdges(graph2, new[] { edge1, edge2 });

            Assert.AreEqual(1, graph2.AddVerticesAndEdgeRange(new[] { edge1, edge3 })); // Showcase the add of only one edge
            AssertHasVertices(parent2, new[] { 1, 2, 3 });
            AssertHasVertices(graph2, new[] { 1, 2, 3 });
            AssertHasEdges(parent2, new[] { edge1, edge2, edge3 });
            AssertHasEdges(graph2, new[] { edge1, edge2, edge3 });
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

        protected static void AddVerticesAndEdgeRange_Throws_Clusters_Test(
            [NotNull] ClusteredAdjacencyGraph<int, Edge<int>> graph)
        {
            AssertEmptyGraph(graph);

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.AddVerticesAndEdgeRange(null));

            // Edge 1, 2, 3
            var edge1 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            Assert.Throws<ArgumentNullException>(() => graph.AddVerticesAndEdgeRange(new[] { edge1, null, edge3 }));
            AssertEmptyGraph(graph);
        }

        #endregion
    }
}