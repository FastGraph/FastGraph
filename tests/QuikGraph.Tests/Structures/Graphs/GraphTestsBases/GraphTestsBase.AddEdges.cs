using System;
using JetBrains.Annotations;
using NUnit.Framework;
using static QuikGraph.Tests.GraphTestHelpers;

namespace QuikGraph.Tests.Structures
{
    internal partial class GraphTestsBase
    {
        #region Add Edges

        protected static void AddEdge_ParallelEdges_Test<TGraph>([NotNull] TGraph graph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, Edge<int>>
        {
            int edgeAdded = 0;

            graph.AddVertex(1);
            graph.AddVertex(2);

            AssertNoEdge(graph);
            graph.EdgeAdded += e =>
            {
                Assert.IsNotNull(e);
                ++edgeAdded;
            };

            // Edge 1
            var edge1 = new Edge<int>(1, 2);
            Assert.IsTrue(graph.AddEdge(edge1));
            Assert.AreEqual(1, edgeAdded);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 2
            var edge2 = new Edge<int>(1, 2);
            Assert.IsTrue(graph.AddEdge(edge2));
            Assert.AreEqual(2, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge2 });

            // Edge 3
            var edge3 = new Edge<int>(2, 1);
            Assert.IsTrue(graph.AddEdge(edge3));
            Assert.AreEqual(3, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge2, edge3 });

            // Edge 1 bis
            Assert.IsTrue(graph.AddEdge(edge1));
            Assert.AreEqual(4, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge2, edge3, edge1 });

            // Edge 4 self edge
            var edge4 = new Edge<int>(2, 2);
            Assert.IsTrue(graph.AddEdge(edge4));
            Assert.AreEqual(5, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge2, edge3, edge1, edge4 });
        }

        protected static void AddEdge_ParallelEdges_Clusters_Test(
            [NotNull] ClusteredAdjacencyGraph<int, Edge<int>> graph1,
            [NotNull] ClusteredAdjacencyGraph<int, Edge<int>> parent2,
            [NotNull] ClusteredAdjacencyGraph<int, Edge<int>> graph2)
        {
            // Graph without parent
            graph1.AddVertex(1);
            graph1.AddVertex(2);

            AssertNoEdge(graph1);

            // Edge 1
            var edge1 = new Edge<int>(1, 2);
            Assert.IsTrue(graph1.AddEdge(edge1));
            AssertHasEdges(graph1, new[] { edge1 });

            // Edge 2
            var edge2 = new Edge<int>(1, 2);
            Assert.IsTrue(graph1.AddEdge(edge2));
            AssertHasEdges(graph1, new[] { edge1, edge2 });

            // Edge 3
            var edge3 = new Edge<int>(2, 1);
            Assert.IsTrue(graph1.AddEdge(edge3));
            AssertHasEdges(graph1, new[] { edge1, edge2, edge3 });

            // Edge 1 bis
            Assert.IsTrue(graph1.AddEdge(edge1));
            AssertHasEdges(graph1, new[] { edge1, edge2, edge3, edge1 });

            // Edge 4 self edge
            var edge4 = new Edge<int>(2, 2);
            Assert.IsTrue(graph1.AddEdge(edge4));
            AssertHasEdges(graph1, new[] { edge1, edge2, edge3, edge1, edge4 });


            // Graph with parent
            graph2.AddVertex(1);
            graph2.AddVertex(2);

            AssertNoEdge(parent2);
            AssertNoEdge(graph2);

            // Edge 1
            Assert.IsTrue(graph2.AddEdge(edge1));
            AssertHasEdges(parent2, new[] { edge1 });
            AssertHasEdges(graph2, new[] { edge1 });

            // Edge 2
            Assert.IsTrue(parent2.AddEdge(edge2));
            AssertHasEdges(parent2, new[] { edge1, edge2 });
            AssertHasEdges(graph2, new[] { edge1 });

            Assert.IsTrue(graph2.AddEdge(edge2));
            AssertHasEdges(parent2, new[] { edge1, edge2 });
            AssertHasEdges(graph2, new[] { edge1, edge2 });

            // Edge 3
            Assert.IsTrue(graph2.AddEdge(edge3));
            AssertHasEdges(parent2, new[] { edge1, edge2, edge3 });
            AssertHasEdges(graph2, new[] { edge1, edge2, edge3 });

            // Edge 1 bis
            Assert.IsTrue(graph2.AddEdge(edge1));
            AssertHasEdges(parent2, new[] { edge1, edge2, edge3 });
            AssertHasEdges(graph2, new[] { edge1, edge2, edge3, edge1 });

            // Edge 4 self edge
            Assert.IsTrue(graph2.AddEdge(edge4));
            AssertHasEdges(parent2, new[] { edge1, edge2, edge3, edge4 });
            AssertHasEdges(graph2, new[] { edge1, edge2, edge3, edge1, edge4 });
        }

        protected static void AddEdge_ParallelEdges_EquatableEdge_Test<TGraph>([NotNull] TGraph graph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, EquatableEdge<int>>
        {
            int edgeAdded = 0;

            graph.AddVertex(1);
            graph.AddVertex(2);

            AssertNoEdge(graph);
            graph.EdgeAdded += e =>
            {
                Assert.IsNotNull(e);
                ++edgeAdded;
            };

            // Edge 1
            var edge1 = new EquatableEdge<int>(1, 2);
            Assert.IsTrue(graph.AddEdge(edge1));
            Assert.AreEqual(1, edgeAdded);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 2
            var edge2 = new EquatableEdge<int>(1, 2);
            Assert.IsTrue(graph.AddEdge(edge2));
            Assert.AreEqual(2, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge2 });

            // Edge 3
            var edge3 = new EquatableEdge<int>(2, 1);
            Assert.IsTrue(graph.AddEdge(edge3));
            Assert.AreEqual(3, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge2, edge3 });

            // Edge 1 bis
            Assert.IsTrue(graph.AddEdge(edge1));
            Assert.AreEqual(4, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge2, edge3, edge1 });

            // Edge 4 self edge
            var edge4 = new EquatableEdge<int>(2, 2);
            Assert.IsTrue(graph.AddEdge(edge4));
            Assert.AreEqual(5, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge2, edge3, edge1, edge4 });
        }

        protected static void AddEdge_ParallelEdges_EquatableEdge_Clusters_Test(
            [NotNull] ClusteredAdjacencyGraph<int, EquatableEdge<int>> graph1,
            [NotNull] ClusteredAdjacencyGraph<int, EquatableEdge<int>> parent2,
            [NotNull] ClusteredAdjacencyGraph<int, EquatableEdge<int>> graph2)
        {
            // Graph without parent
            graph1.AddVertex(1);
            graph1.AddVertex(2);

            AssertNoEdge(graph1);

            // Edge 1
            var edge1 = new EquatableEdge<int>(1, 2);
            Assert.IsTrue(graph1.AddEdge(edge1));
            AssertHasEdges(graph1, new[] { edge1 });

            // Edge 2
            var edge2 = new EquatableEdge<int>(1, 2);
            Assert.IsTrue(graph1.AddEdge(edge2));
            AssertHasEdges(graph1, new[] { edge1, edge2 });

            // Edge 3
            var edge3 = new EquatableEdge<int>(2, 1);
            Assert.IsTrue(graph1.AddEdge(edge3));
            AssertHasEdges(graph1, new[] { edge1, edge2, edge3 });

            // Edge 1 bis
            Assert.IsTrue(graph1.AddEdge(edge1));
            AssertHasEdges(graph1, new[] { edge1, edge2, edge3, edge1 });

            // Edge 4 self edge
            var edge4 = new EquatableEdge<int>(2, 2);
            Assert.IsTrue(graph1.AddEdge(edge4));
            AssertHasEdges(graph1, new[] { edge1, edge2, edge3, edge1, edge4 });


            // Graph with parent
            graph2.AddVertex(1);
            graph2.AddVertex(2);

            AssertNoEdge(parent2);
            AssertNoEdge(graph2);

            // Edge 1
            Assert.IsTrue(graph2.AddEdge(edge1));
            AssertHasEdges(parent2, new[] { edge1 });
            AssertHasEdges(graph2, new[] { edge1 });

            // Edge 2
            Assert.IsTrue(parent2.AddEdge(edge2));
            AssertHasEdges(parent2, new[] { edge1, edge2 });
            AssertHasEdges(graph2, new[] { edge1 });

            Assert.IsTrue(graph2.AddEdge(edge2));
            AssertHasEdges(parent2, new[] { edge1, edge2 });
            AssertHasEdges(graph2, new[] { edge1, edge2 });

            // Edge 3
            Assert.IsTrue(graph2.AddEdge(edge3));
            AssertHasEdges(parent2, new[] { edge1, edge2, edge3 });
            AssertHasEdges(graph2, new[] { edge1, edge2, edge3 });

            // Edge 1 bis
            Assert.IsTrue(graph2.AddEdge(edge1));
            AssertHasEdges(parent2, new[] { edge1, edge2, edge3 });
            AssertHasEdges(graph2, new[] { edge1, edge2, edge3, edge1 });

            // Edge 4 self edge
            Assert.IsTrue(graph2.AddEdge(edge4));
            AssertHasEdges(parent2, new[] { edge1, edge2, edge3, edge4 });
            AssertHasEdges(graph2, new[] { edge1, edge2, edge3, edge1, edge4 });
        }

        protected static void AddEdge_NoParallelEdges_Test<TGraph>([NotNull] TGraph graph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, Edge<int>>
        {
            int edgeAdded = 0;

            graph.AddVertex(1);
            graph.AddVertex(2);

            AssertNoEdge(graph);
            graph.EdgeAdded += e =>
            {
                Assert.IsNotNull(e);
                ++edgeAdded;
            };

            // Edge 1
            var edge1 = new Edge<int>(1, 2);
            Assert.IsTrue(graph.AddEdge(edge1));
            Assert.AreEqual(1, edgeAdded);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 2
            var edge2 = new Edge<int>(1, 2);
            Assert.IsFalse(graph.AddEdge(edge2));
            Assert.AreEqual(1, edgeAdded);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 3
            var edge3 = new Edge<int>(2, 1);
            Assert.IsTrue(graph.AddEdge(edge3));
            Assert.AreEqual(2, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge3 });

            // Edge 1 bis
            Assert.IsFalse(graph.AddEdge(edge1));
            Assert.AreEqual(2, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge3 });

            // Edge 4 self edge
            var edge4 = new Edge<int>(2, 2);
            Assert.IsTrue(graph.AddEdge(edge4));
            Assert.AreEqual(3, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge3, edge4 });
        }

        protected static void AddEdge_NoParallelEdges_UndirectedGraph_Test<TGraph>([NotNull] TGraph graph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, Edge<int>>
        {
            int edgeAdded = 0;

            graph.AddVertex(1);
            graph.AddVertex(2);

            AssertNoEdge(graph);
            graph.EdgeAdded += e =>
            {
                Assert.IsNotNull(e);
                ++edgeAdded;
            };

            // Edge 1
            var edge1 = new Edge<int>(1, 2);
            Assert.IsTrue(graph.AddEdge(edge1));
            Assert.AreEqual(1, edgeAdded);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 2
            var edge2 = new Edge<int>(1, 2);
            Assert.IsFalse(graph.AddEdge(edge2));
            Assert.AreEqual(1, edgeAdded);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 3
            var edge3 = new Edge<int>(2, 1);
            Assert.IsFalse(graph.AddEdge(edge3));   // Parallel to edge 1
            Assert.AreEqual(1, edgeAdded);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 1 bis
            Assert.IsFalse(graph.AddEdge(edge1));
            Assert.AreEqual(1, edgeAdded);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 4 self edge
            var edge4 = new Edge<int>(2, 2);
            Assert.IsTrue(graph.AddEdge(edge4));
            Assert.AreEqual(2, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge4 });
        }

        protected static void AddEdge_NoParallelEdges_Clusters_Test(
            [NotNull] ClusteredAdjacencyGraph<int, Edge<int>> graph1,
            [NotNull] ClusteredAdjacencyGraph<int, Edge<int>> parent2,
            [NotNull] ClusteredAdjacencyGraph<int, Edge<int>> graph2)
        {
            // Graph without parent
            graph1.AddVertex(1);
            graph1.AddVertex(2);

            AssertNoEdge(graph1);

            // Edge 1
            var edge1 = new Edge<int>(1, 2);
            Assert.IsTrue(graph1.AddEdge(edge1));
            AssertHasEdges(graph1, new[] { edge1 });

            // Edge 2
            var edge2 = new Edge<int>(1, 2);
            Assert.IsFalse(graph1.AddEdge(edge2));
            AssertHasEdges(graph1, new[] { edge1 });

            // Edge 3
            var edge3 = new Edge<int>(2, 1);
            Assert.IsTrue(graph1.AddEdge(edge3));
            AssertHasEdges(graph1, new[] { edge1, edge3 });

            // Edge 1 bis
            Assert.IsFalse(graph1.AddEdge(edge1));
            AssertHasEdges(graph1, new[] { edge1, edge3 });

            // Edge 4 self edge
            var edge4 = new Edge<int>(2, 2);
            Assert.IsTrue(graph1.AddEdge(edge4));
            AssertHasEdges(graph1, new[] { edge1, edge3, edge4 });


            // Graph with parent
            graph2.AddVertex(1);
            graph2.AddVertex(2);

            AssertNoEdge(parent2);
            AssertNoEdge(graph2);

            // Edge 1
            Assert.IsTrue(graph2.AddEdge(edge1));
            AssertHasEdges(parent2, new[] { edge1 });
            AssertHasEdges(graph2, new[] { edge1 });

            // Edge 2
            Assert.IsFalse(graph2.AddEdge(edge2));
            AssertHasEdges(parent2, new[] { edge1 });
            AssertHasEdges(graph2, new[] { edge1 });

            // Edge 3
            Assert.IsTrue(parent2.AddEdge(edge3));
            AssertHasEdges(parent2, new[] { edge1, edge3 });
            AssertHasEdges(graph2, new[] { edge1 });

            Assert.IsTrue(graph2.AddEdge(edge3));
            AssertHasEdges(parent2, new[] { edge1, edge3 });
            AssertHasEdges(graph2, new[] { edge1, edge3 });

            // Edge 1 bis
            Assert.IsFalse(graph2.AddEdge(edge1));
            AssertHasEdges(parent2, new[] { edge1, edge3 });
            AssertHasEdges(graph2, new[] { edge1, edge3 });

            // Edge 4 self edge
            Assert.IsTrue(graph2.AddEdge(edge4));
            AssertHasEdges(parent2, new[] { edge1, edge3, edge4 });
            AssertHasEdges(graph2, new[] { edge1, edge3, edge4 });
        }

        protected static void AddEdge_NoParallelEdges_EquatableEdge_Test<TGraph>([NotNull] TGraph graph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, EquatableEdge<int>>
        {
            int edgeAdded = 0;

            graph.AddVertex(1);
            graph.AddVertex(2);

            AssertNoEdge(graph);
            graph.EdgeAdded += e =>
            {
                Assert.IsNotNull(e);
                ++edgeAdded;
            };

            // Edge 1
            var edge1 = new EquatableEdge<int>(1, 2);
            Assert.IsTrue(graph.AddEdge(edge1));
            Assert.AreEqual(1, edgeAdded);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 2
            var edge2 = new EquatableEdge<int>(1, 2);
            Assert.IsFalse(graph.AddEdge(edge2));
            Assert.AreEqual(1, edgeAdded);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 3
            var edge3 = new EquatableEdge<int>(2, 1);
            Assert.IsTrue(graph.AddEdge(edge3));
            Assert.AreEqual(2, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge3 });

            // Edge 1 bis
            Assert.IsFalse(graph.AddEdge(edge1));
            Assert.AreEqual(2, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge3 });

            // Edge 4 self edge
            var edge4 = new EquatableEdge<int>(2, 2);
            Assert.IsTrue(graph.AddEdge(edge4));
            Assert.AreEqual(3, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge3, edge4 });
        }

        protected static void AddEdge_NoParallelEdges_EquatableEdge_UndirectedGraph_Test<TGraph>([NotNull] TGraph graph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, EquatableEdge<int>>
        {
            int edgeAdded = 0;

            graph.AddVertex(1);
            graph.AddVertex(2);

            AssertNoEdge(graph);
            graph.EdgeAdded += e =>
            {
                Assert.IsNotNull(e);
                ++edgeAdded;
            };

            // Edge 1
            var edge1 = new EquatableEdge<int>(1, 2);
            Assert.IsTrue(graph.AddEdge(edge1));
            Assert.AreEqual(1, edgeAdded);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 2
            var edge2 = new EquatableEdge<int>(1, 2);
            Assert.IsFalse(graph.AddEdge(edge2));
            Assert.AreEqual(1, edgeAdded);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 3
            var edge3 = new EquatableEdge<int>(2, 1);
            Assert.IsFalse(graph.AddEdge(edge3));   // Parallel to edge 1
            Assert.AreEqual(1, edgeAdded);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 1 bis
            Assert.IsFalse(graph.AddEdge(edge1));
            Assert.AreEqual(1, edgeAdded);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 4 self edge
            var edge4 = new EquatableEdge<int>(2, 2);
            Assert.IsTrue(graph.AddEdge(edge4));
            Assert.AreEqual(2, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge4 });
        }

        protected static void AddEdge_NoParallelEdges_EquatableEdge_Clusters_Test(
            [NotNull] ClusteredAdjacencyGraph<int, EquatableEdge<int>> graph1,
            [NotNull] ClusteredAdjacencyGraph<int, EquatableEdge<int>> parent2,
            [NotNull] ClusteredAdjacencyGraph<int, EquatableEdge<int>> graph2)
        {
            // Graph without parent
            graph1.AddVertex(1);
            graph1.AddVertex(2);

            AssertNoEdge(graph1);

            // Edge 1
            var edge1 = new EquatableEdge<int>(1, 2);
            Assert.IsTrue(graph1.AddEdge(edge1));
            AssertHasEdges(graph1, new[] { edge1 });

            // Edge 2
            var edge2 = new EquatableEdge<int>(1, 2);
            Assert.IsFalse(graph1.AddEdge(edge2));
            AssertHasEdges(graph1, new[] { edge1 });

            // Edge 3
            var edge3 = new EquatableEdge<int>(2, 1);
            Assert.IsTrue(graph1.AddEdge(edge3));
            AssertHasEdges(graph1, new[] { edge1, edge3 });

            // Edge 1 bis
            Assert.IsFalse(graph1.AddEdge(edge1));
            AssertHasEdges(graph1, new[] { edge1, edge3 });

            // Edge 4 self edge
            var edge4 = new EquatableEdge<int>(2, 2);
            Assert.IsTrue(graph1.AddEdge(edge4));
            AssertHasEdges(graph1, new[] { edge1, edge3, edge4 });


            // Graph with parent
            graph2.AddVertex(1);
            graph2.AddVertex(2);

            AssertNoEdge(parent2);
            AssertNoEdge(graph2);

            // Edge 1
            Assert.IsTrue(graph2.AddEdge(edge1));
            AssertHasEdges(parent2, new[] { edge1 });
            AssertHasEdges(graph2, new[] { edge1 });

            // Edge 2
            Assert.IsFalse(graph2.AddEdge(edge2));
            AssertHasEdges(parent2, new[] { edge1 });
            AssertHasEdges(graph2, new[] { edge1 });

            // Edge 3
            Assert.IsTrue(parent2.AddEdge(edge3));
            AssertHasEdges(parent2, new[] { edge1, edge3 });
            AssertHasEdges(graph2, new[] { edge1 });

            Assert.IsTrue(graph2.AddEdge(edge3));
            AssertHasEdges(parent2, new[] { edge1, edge3 });
            AssertHasEdges(graph2, new[] { edge1, edge3 });

            // Edge 1 bis
            Assert.IsFalse(graph2.AddEdge(edge1));
            AssertHasEdges(parent2, new[] { edge1, edge3 });
            AssertHasEdges(graph2, new[] { edge1, edge3 });

            // Edge 4 self edge
            Assert.IsTrue(graph2.AddEdge(edge4));
            AssertHasEdges(parent2, new[] { edge1, edge3, edge4 });
            AssertHasEdges(graph2, new[] { edge1, edge3, edge4 });
        }

        protected static void AddEdge_ForbiddenParallelEdges_Test(
            [NotNull] BidirectionalMatrixGraph<Edge<int>> graph)
        {
            int edgeAdded = 0;

            AssertNoEdge(graph);
            graph.EdgeAdded += e =>
            {
                Assert.IsNotNull(e);
                ++edgeAdded;
            };

            // Edge 1
            var edge1 = new Edge<int>(1, 2);
            Assert.IsTrue(graph.AddEdge(edge1));
            Assert.AreEqual(1, edgeAdded);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 2
            var edge2 = new Edge<int>(2, 1);
            Assert.IsTrue(graph.AddEdge(edge2));
            Assert.AreEqual(2, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge2 });

            // Edge 3 self edge
            var edge3 = new Edge<int>(2, 2);
            Assert.IsTrue(graph.AddEdge(edge3));
            Assert.AreEqual(3, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge2, edge3 });
        }

        protected static void AddEdge_EquatableEdge_ForbiddenParallelEdges_Test(
            [NotNull] BidirectionalMatrixGraph<EquatableEdge<int>> graph)
        {
            int edgeAdded = 0;

            AssertNoEdge(graph);
            graph.EdgeAdded += e =>
            {
                Assert.IsNotNull(e);
                ++edgeAdded;
            };

            // Edge 1
            var edge1 = new EquatableEdge<int>(1, 2);
            Assert.IsTrue(graph.AddEdge(edge1));
            Assert.AreEqual(1, edgeAdded);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 2
            var edge2 = new EquatableEdge<int>(2, 1);
            Assert.IsTrue(graph.AddEdge(edge2));
            Assert.AreEqual(2, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge2 });

            // Edge 3 self edge
            var edge3 = new EquatableEdge<int>(2, 2);
            Assert.IsTrue(graph.AddEdge(edge3));
            Assert.AreEqual(3, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge2, edge3 });
        }

        protected static void AddEdge_Throws_EdgesOnly_Test(
            [NotNull] IMutableEdgeListGraph<int, Edge<int>> graph)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.AddEdge(null));
            AssertNoEdge(graph);
        }

        protected static void AddEdge_Throws_Test<TGraph>([NotNull] TGraph graph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, Edge<int>>
        {
            AddEdge_Throws_EdgesOnly_Test(graph);

            // Both vertices not in graph
            Assert.Throws<VertexNotFoundException>(() => graph.AddEdge(new Edge<int>(0, 1)));
            AssertNoEdge(graph);

            // Source not in graph
            graph.AddVertex(1);
            Assert.Throws<VertexNotFoundException>(() => graph.AddEdge(new Edge<int>(0, 1)));
            AssertNoEdge(graph);

            // Target not in graph
            Assert.Throws<VertexNotFoundException>(() => graph.AddEdge(new Edge<int>(1, 0)));
            AssertNoEdge(graph);
        }

        protected static void AddEdge_Throws_Clusters_Test(
            [NotNull] ClusteredAdjacencyGraph<int, Edge<int>> graph)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.AddEdge(null));
            AssertNoEdge(graph);

            // Both vertices not in graph
            Assert.Throws<VertexNotFoundException>(() => graph.AddEdge(new Edge<int>(0, 1)));
            AssertNoEdge(graph);

            // Source not in graph
            graph.AddVertex(1);
            Assert.Throws<VertexNotFoundException>(() => graph.AddEdge(new Edge<int>(0, 1)));
            AssertNoEdge(graph);

            // Target not in graph
            Assert.Throws<VertexNotFoundException>(() => graph.AddEdge(new Edge<int>(1, 0)));
            AssertNoEdge(graph);
        }

        protected static void AddEdgeRange_EdgesOnly_Test(
            [NotNull] IMutableEdgeListGraph<int, Edge<int>> graph)
        {
            int edgeAdded = 0;

            AssertNoEdge(graph);
            graph.EdgeAdded += e =>
            {
                Assert.IsNotNull(e);
                ++edgeAdded;
            };

            // Edge 1, 2, 3
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 3);
            Assert.AreEqual(3, graph.AddEdgeRange(new[] { edge1, edge2, edge3 }));
            Assert.AreEqual(3, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge2, edge3 });

            // Edge 1, 4
            var edge4 = new Edge<int>(2, 2);
            Assert.AreEqual(1, graph.AddEdgeRange(new[] { edge1, edge4 })); // Showcase the add of only one edge
            Assert.AreEqual(4, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge2, edge3, edge4 });
        }

        protected static void AddEdgeRange_Test<TGraph>([NotNull] TGraph graph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, Edge<int>>
        {
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);

            AddEdgeRange_EdgesOnly_Test(graph);
        }

        protected static void AddEdgeRange_ForbiddenParallelEdges_Test()
        {
            int edgeAdded = 0;
            var graph = new BidirectionalMatrixGraph<Edge<int>>(3);

            AssertNoEdge(graph);
            graph.EdgeAdded += e =>
            {
                Assert.IsNotNull(e);
                ++edgeAdded;
            };

            // Edge 1, 2, 3
            var edge1 = new Edge<int>(0, 1);
            var edge2 = new Edge<int>(0, 2);
            var edge3 = new Edge<int>(1, 2);
            Assert.AreEqual(3, graph.AddEdgeRange(new[] { edge1, edge2, edge3 }));
            Assert.AreEqual(3, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge2, edge3 });

            // Edge 4
            var edge4 = new Edge<int>(2, 2);
            Assert.AreEqual(1, graph.AddEdgeRange(new[] { edge4 }));
            Assert.AreEqual(4, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge2, edge3, edge4 });
        }

        protected static void AddEdgeRange_Clusters_Test(
            [NotNull] ClusteredAdjacencyGraph<int, Edge<int>> graph1,
            [NotNull] ClusteredAdjacencyGraph<int, Edge<int>> parent2,
            [NotNull] ClusteredAdjacencyGraph<int, Edge<int>> graph2)
        {
            // Graph without parent
            graph1.AddVertex(1);
            graph1.AddVertex(2);
            graph1.AddVertex(3);

            AssertNoEdge(graph1);

            // Edge 1, 2, 3
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 3);
            Assert.AreEqual(3, graph1.AddEdgeRange(new[] { edge1, edge2, edge3 }));
            AssertHasEdges(graph1, new[] { edge1, edge2, edge3 });

            // Edge 1, 4
            var edge4 = new Edge<int>(2, 2);
            Assert.AreEqual(1, graph1.AddEdgeRange(new[] { edge1, edge4 })); // Showcase the add of only one edge
            AssertHasEdges(graph1, new[] { edge1, edge2, edge3, edge4 });


            // Graph with parent
            graph2.AddVertex(1);
            graph2.AddVertex(2);
            graph2.AddVertex(3);

            AssertNoEdge(parent2);
            AssertNoEdge(graph2);

            // Edge 1, 2, 3
            Assert.AreEqual(3, graph2.AddEdgeRange(new[] { edge1, edge2, edge3 }));
            AssertHasEdges(parent2, new[] { edge1, edge2, edge3 });
            AssertHasEdges(graph2, new[] { edge1, edge2, edge3 });

            // Edge 1, 4
            Assert.AreEqual(1, parent2.AddEdgeRange(new[] { edge1, edge4 })); // Showcase the add of only one edge
            AssertHasEdges(parent2, new[] { edge1, edge2, edge3, edge4 });
            AssertHasEdges(graph2, new[] { edge1, edge2, edge3 });

            Assert.AreEqual(1, graph2.AddEdgeRange(new[] { edge1, edge4 })); // Showcase the add of only one edge
            AssertHasEdges(parent2, new[] { edge1, edge2, edge3, edge4 });
            AssertHasEdges(graph2, new[] { edge1, edge2, edge3, edge4 });
        }

        protected static void AddEdgeRange_Throws_EdgesOnly_Test(
            [NotNull] IMutableEdgeListGraph<int, Edge<int>> graph)
        {
            int edgeAdded = 0;

            AssertNoEdge(graph);
            graph.EdgeAdded += e =>
            {
                Assert.IsNotNull(e);
                ++edgeAdded;
            };

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.AddEdgeRange(null));
            AssertNoEdge(graph);
            Assert.AreEqual(0, edgeAdded);

            // Edge 1, 2, 3
            var edge1 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(2, 3);
            Assert.Throws<ArgumentNullException>(() => graph.AddEdgeRange(new[] { edge1, null, edge3 }));
            Assert.AreEqual(0, edgeAdded);
            AssertNoEdge(graph);
        }

        protected static void AddEdgeRange_Throws_Test<TGraph>([NotNull] TGraph graph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, Edge<int>>
        {
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);

            AddEdgeRange_Throws_EdgesOnly_Test(graph);
        }

        protected static void AddEdgeRange_Throws_Clusters_Test(
            [NotNull] ClusteredAdjacencyGraph<int, Edge<int>> graph)
        {
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);

            AssertNoEdge(graph);

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.AddEdgeRange(null));
            AssertNoEdge(graph);

            // Edge 1, 2, 3
            var edge1 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(2, 3);
            Assert.Throws<ArgumentNullException>(() => graph.AddEdgeRange(new[] { edge1, null, edge3 }));
            AssertNoEdge(graph);
        }

        protected static void AddEdgeRange_ForbiddenParallelEdges_Throws_Test(
            [NotNull] BidirectionalMatrixGraph<Edge<int>> graph)
        {
            int edgeAdded = 0;

            AssertNoEdge(graph);
            graph.EdgeAdded += e =>
            {
                Assert.IsNotNull(e);
                ++edgeAdded;
            };

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.AddEdgeRange(null));
            AssertNoEdge(graph);
            Assert.AreEqual(0, edgeAdded);

            // Edge 1, 2, 3
            var edge1 = new Edge<int>(0, 1);
            var edge3 = new Edge<int>(1, 2);
            Assert.Throws<ArgumentNullException>(() => graph.AddEdgeRange(new[] { edge1, null, edge3 }));
            Assert.AreEqual(0, edgeAdded);
            AssertNoEdge(graph);

            // Edge 1, 3, 4
            var edge4 = new Edge<int>(0, 1);
            Assert.Throws<ParallelEdgeNotAllowedException>(() => graph.AddEdgeRange(new[] { edge1, edge3, edge4 }));
            Assert.AreEqual(2, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge3 });

            // Out of range => vertex not found
            Assert.Throws<VertexNotFoundException>(() => graph.AddEdgeRange(new[] { new Edge<int>(4, 5), }));
            Assert.AreEqual(2, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge3 });
        }


        protected static void AddEdge_ParallelEdges_EdgesOnly_Test(
            [NotNull] EdgeListGraph<int, Edge<int>> directedGraph,
            [NotNull] EdgeListGraph<int, Edge<int>> undirectedGraph,
            [NotNull, InstantHandle] Func<
                EdgeListGraph<int, Edge<int>>,
                Edge<int>,
                bool> addEdge)
        {
            if (!directedGraph.IsDirected && directedGraph.AllowParallelEdges)
                throw new InvalidOperationException("Graph must be directed and allow parallel edges.");
            if (undirectedGraph.IsDirected && undirectedGraph.AllowParallelEdges)
                throw new InvalidOperationException("Graph must be undirected and allow parallel edges.");

            int directedEdgeAdded = 0;
            int undirectedEdgeAdded = 0;

            AssertNoEdge(directedGraph);
            directedGraph.EdgeAdded += e =>
            {
                Assert.IsNotNull(e);
                ++directedEdgeAdded;
            };

            AssertNoEdge(undirectedGraph);
            undirectedGraph.EdgeAdded += e =>
            {
                Assert.IsNotNull(e);
                ++undirectedEdgeAdded;
            };

            // Edge 1
            var edge1 = new Edge<int>(1, 2);
            Assert.IsTrue(addEdge(directedGraph, edge1));
            Assert.AreEqual(1, directedEdgeAdded);
            AssertHasEdges(directedGraph, new[] { edge1 });

            Assert.IsTrue(addEdge(undirectedGraph, edge1));
            Assert.AreEqual(1, undirectedEdgeAdded);
            AssertHasEdges(undirectedGraph, new[] { edge1 });

            // Edge 2
            var edge2 = new Edge<int>(1, 2);
            Assert.IsTrue(addEdge(directedGraph, edge2));
            Assert.AreEqual(2, directedEdgeAdded);
            AssertHasEdges(directedGraph, new[] { edge1, edge2 });

            Assert.IsTrue(addEdge(undirectedGraph, edge2));
            Assert.AreEqual(2, undirectedEdgeAdded);
            AssertHasEdges(undirectedGraph, new[] { edge1, edge2 });

            // Edge 3
            var edge3 = new Edge<int>(2, 1);
            Assert.IsTrue(addEdge(directedGraph, edge3));
            Assert.AreEqual(3, directedEdgeAdded);
            AssertHasEdges(directedGraph, new[] { edge1, edge2, edge3 });

            Assert.IsTrue(addEdge(undirectedGraph, edge3));
            Assert.AreEqual(3, undirectedEdgeAdded);
            AssertHasEdges(undirectedGraph, new[] { edge1, edge2, edge3 });

            // Edge 1 bis
            Assert.IsFalse(addEdge(directedGraph, edge1));
            Assert.AreEqual(3, directedEdgeAdded);
            AssertHasEdges(directedGraph, new[] { edge1, edge2, edge3 });

            Assert.IsFalse(addEdge(undirectedGraph, edge1));
            Assert.AreEqual(3, undirectedEdgeAdded);
            AssertHasEdges(undirectedGraph, new[] { edge1, edge2, edge3 });

            // Edge 4 self edge
            var edge4 = new Edge<int>(2, 2);
            Assert.IsTrue(addEdge(directedGraph, edge4));
            Assert.AreEqual(4, directedEdgeAdded);
            AssertHasEdges(directedGraph, new[] { edge1, edge2, edge3, edge4 });

            Assert.IsTrue(addEdge(undirectedGraph, edge4));
            Assert.AreEqual(4, undirectedEdgeAdded);
            AssertHasEdges(undirectedGraph, new[] { edge1, edge2, edge3, edge4 });
        }

        protected static void AddEdge_ParallelEdges_EquatableEdge_EdgesOnly_Test(
            [NotNull] EdgeListGraph<int, EquatableEdge<int>> directedGraph,
            [NotNull] EdgeListGraph<int, EquatableEdge<int>> undirectedGraph,
            [NotNull, InstantHandle] Func<
                EdgeListGraph<int, EquatableEdge<int>>,
                EquatableEdge<int>,
                bool> addEdge)
        {
            if (!directedGraph.IsDirected && directedGraph.AllowParallelEdges)
                throw new InvalidOperationException("Graph must be directed and allow parallel edges.");
            if (undirectedGraph.IsDirected && undirectedGraph.AllowParallelEdges)
                throw new InvalidOperationException("Graph must be undirected and allow parallel edges.");

            int directedEdgeAdded = 0;
            int undirectedEdgeAdded = 0;

            AssertNoEdge(directedGraph);
            directedGraph.EdgeAdded += e =>
            {
                Assert.IsNotNull(e);
                ++directedEdgeAdded;
            };

            AssertNoEdge(undirectedGraph);
            undirectedGraph.EdgeAdded += e =>
            {
                Assert.IsNotNull(e);
                ++undirectedEdgeAdded;
            };

            // Edge 1
            var edge1 = new EquatableEdge<int>(1, 2);
            Assert.IsTrue(addEdge(directedGraph, edge1));
            Assert.AreEqual(1, directedEdgeAdded);
            AssertHasEdges(directedGraph, new[] { edge1 });

            Assert.IsTrue(addEdge(undirectedGraph, edge1));
            Assert.AreEqual(1, undirectedEdgeAdded);
            AssertHasEdges(undirectedGraph, new[] { edge1 });

            // Edge 2
            var edge2 = new EquatableEdge<int>(1, 2);
            Assert.IsFalse(addEdge(directedGraph, edge2));
            Assert.AreEqual(1, directedEdgeAdded);
            AssertHasEdges(directedGraph, new[] { edge1 });

            Assert.IsFalse(addEdge(undirectedGraph, edge2));
            Assert.AreEqual(1, undirectedEdgeAdded);
            AssertHasEdges(undirectedGraph, new[] { edge1 });

            // Edge 3
            var edge3 = new EquatableEdge<int>(2, 1);
            Assert.IsTrue(addEdge(directedGraph, edge3));
            Assert.AreEqual(2, directedEdgeAdded);
            AssertHasEdges(directedGraph, new[] { edge1, edge3 });

            Assert.IsTrue(addEdge(undirectedGraph, edge3));
            Assert.AreEqual(2, undirectedEdgeAdded);
            AssertHasEdges(undirectedGraph, new[] { edge1, edge3 });

            // Edge 1 bis
            Assert.IsFalse(addEdge(directedGraph, edge1));
            Assert.AreEqual(2, directedEdgeAdded);
            AssertHasEdges(directedGraph, new[] { edge1, edge3 });

            Assert.IsFalse(addEdge(undirectedGraph, edge1));
            Assert.AreEqual(2, undirectedEdgeAdded);
            AssertHasEdges(undirectedGraph, new[] { edge1, edge3 });

            // Edge 4 self edge
            var edge4 = new EquatableEdge<int>(2, 2);
            Assert.IsTrue(addEdge(directedGraph, edge4));
            Assert.AreEqual(3, directedEdgeAdded);
            AssertHasEdges(directedGraph, new[] { edge1, edge3, edge4 });

            Assert.IsTrue(addEdge(undirectedGraph, edge4));
            Assert.AreEqual(3, undirectedEdgeAdded);
            AssertHasEdges(undirectedGraph, new[] { edge1, edge3, edge4 });
        }

        protected static void AddEdge_NoParallelEdges_EdgesOnly_Test(
            [NotNull] EdgeListGraph<int, Edge<int>> directedGraph,
            [NotNull] EdgeListGraph<int, Edge<int>> undirectedGraph,
            [NotNull, InstantHandle] Func<
                EdgeListGraph<int, Edge<int>>,
                Edge<int>,
                bool> addEdge)
        {
            if (!directedGraph.IsDirected && !directedGraph.AllowParallelEdges)
                throw new InvalidOperationException("Graph must be directed and not allow parallel edges.");
            if (undirectedGraph.IsDirected && !undirectedGraph.AllowParallelEdges)
                throw new InvalidOperationException("Graph must be undirected and not allow parallel edges.");

            int directedEdgeAdded = 0;
            int undirectedEdgeAdded = 0;

            AssertNoEdge(directedGraph);
            directedGraph.EdgeAdded += e =>
            {
                Assert.IsNotNull(e);
                ++directedEdgeAdded;
            };

            AssertNoEdge(undirectedGraph);
            undirectedGraph.EdgeAdded += e =>
            {
                Assert.IsNotNull(e);
                ++undirectedEdgeAdded;
            };

            // Edge 1
            var edge1 = new Edge<int>(1, 2);
            Assert.IsTrue(addEdge(directedGraph, edge1));
            Assert.AreEqual(1, directedEdgeAdded);
            AssertHasEdges(directedGraph, new[] { edge1 });

            Assert.IsTrue(addEdge(undirectedGraph, edge1));
            Assert.AreEqual(1, undirectedEdgeAdded);
            AssertHasEdges(undirectedGraph, new[] { edge1 });

            // Edge 2
            var edge2 = new Edge<int>(1, 2);
            Assert.IsFalse(addEdge(directedGraph, edge2));
            Assert.AreEqual(1, directedEdgeAdded);
            AssertHasEdges(directedGraph, new[] { edge1 });

            Assert.IsFalse(addEdge(undirectedGraph, edge2));
            Assert.AreEqual(1, undirectedEdgeAdded);
            AssertHasEdges(undirectedGraph, new[] { edge1 });

            // Edge 3
            var edge3 = new Edge<int>(2, 1);
            Assert.IsTrue(addEdge(directedGraph, edge3));
            Assert.AreEqual(2, directedEdgeAdded);
            AssertHasEdges(directedGraph, new[] { edge1, edge3 });

            Assert.IsFalse(addEdge(undirectedGraph, edge3));
            Assert.AreEqual(1, undirectedEdgeAdded);
            AssertHasEdges(undirectedGraph, new[] { edge1 });

            // Edge 1 bis
            Assert.IsFalse(addEdge(directedGraph, edge1));
            Assert.AreEqual(2, directedEdgeAdded);
            AssertHasEdges(directedGraph, new[] { edge1, edge3 });

            Assert.IsFalse(addEdge(undirectedGraph, edge1));
            Assert.AreEqual(1, undirectedEdgeAdded);
            AssertHasEdges(undirectedGraph, new[] { edge1 });

            // Edge 4 self edge
            var edge4 = new Edge<int>(2, 2);
            Assert.IsTrue(addEdge(directedGraph, edge4));
            Assert.AreEqual(3, directedEdgeAdded);
            AssertHasEdges(directedGraph, new[] { edge1, edge3, edge4 });

            Assert.IsTrue(addEdge(undirectedGraph, edge4));
            Assert.AreEqual(2, undirectedEdgeAdded);
            AssertHasEdges(undirectedGraph, new[] { edge1, edge4 });
        }

        protected static void AddEdge_NoParallelEdges_EquatableEdge_EdgesOnly_Test(
            [NotNull] EdgeListGraph<int, EquatableEdge<int>> directedGraph,
            [NotNull] EdgeListGraph<int, EquatableEdge<int>> undirectedGraph,
            [NotNull, InstantHandle] Func<
                EdgeListGraph<int, EquatableEdge<int>>,
                EquatableEdge<int>,
                bool> addEdge)
        {
            if (!directedGraph.IsDirected && !directedGraph.AllowParallelEdges)
                throw new InvalidOperationException("Graph must be directed and not allow parallel edges.");
            if (undirectedGraph.IsDirected && !undirectedGraph.AllowParallelEdges)
                throw new InvalidOperationException("Graph must be undirected and not allow parallel edges.");

            int directedEdgeAdded = 0;
            int undirectedEdgeAdded = 0;

            AssertNoEdge(directedGraph);
            directedGraph.EdgeAdded += e =>
            {
                Assert.IsNotNull(e);
                ++directedEdgeAdded;
            };

            AssertNoEdge(undirectedGraph);
            undirectedGraph.EdgeAdded += e =>
            {
                Assert.IsNotNull(e);
                ++undirectedEdgeAdded;
            };

            // Edge 1
            var edge1 = new EquatableEdge<int>(1, 2);
            Assert.IsTrue(addEdge(directedGraph, edge1));
            Assert.AreEqual(1, directedEdgeAdded);
            AssertHasEdges(directedGraph, new[] { edge1 });

            Assert.IsTrue(addEdge(undirectedGraph, edge1));
            Assert.AreEqual(1, undirectedEdgeAdded);
            AssertHasEdges(undirectedGraph, new[] { edge1 });

            // Edge 2
            var edge2 = new EquatableEdge<int>(1, 2);
            Assert.IsFalse(addEdge(directedGraph, edge2));
            Assert.AreEqual(1, directedEdgeAdded);
            AssertHasEdges(directedGraph, new[] { edge1 });

            Assert.IsFalse(addEdge(undirectedGraph, edge2));
            Assert.AreEqual(1, undirectedEdgeAdded);
            AssertHasEdges(undirectedGraph, new[] { edge1 });

            // Edge 3
            var edge3 = new EquatableEdge<int>(2, 1);
            Assert.IsTrue(addEdge(directedGraph, edge3));
            Assert.AreEqual(2, directedEdgeAdded);
            AssertHasEdges(directedGraph, new[] { edge1, edge3 });

            Assert.IsFalse(addEdge(undirectedGraph, edge3));
            Assert.AreEqual(1, undirectedEdgeAdded);
            AssertHasEdges(undirectedGraph, new[] { edge1 });

            // Edge 1 bis
            Assert.IsFalse(addEdge(directedGraph, edge1));
            Assert.AreEqual(2, directedEdgeAdded);
            AssertHasEdges(directedGraph, new[] { edge1, edge3 });

            Assert.IsFalse(addEdge(undirectedGraph, edge1));
            Assert.AreEqual(1, undirectedEdgeAdded);
            AssertHasEdges(undirectedGraph, new[] { edge1 });

            // Edge 4 self edge
            var edge4 = new EquatableEdge<int>(2, 2);
            Assert.IsTrue(addEdge(directedGraph, edge4));
            Assert.AreEqual(3, directedEdgeAdded);
            AssertHasEdges(directedGraph, new[] { edge1, edge3, edge4 });

            Assert.IsTrue(addEdge(undirectedGraph, edge4));
            Assert.AreEqual(2, undirectedEdgeAdded);
            AssertHasEdges(undirectedGraph, new[] { edge1, edge4 });
        }


        protected static void AddEdge_ImmutableGraph_NoUpdate<TGraph>(
            [NotNull] TGraph wrappedGraph,
            [NotNull, InstantHandle] Func<IEdgeSet<int, Edge<int>>> createGraph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, Edge<int>>
        {
            IEdgeSet<int, Edge<int>> graph = createGraph();

            var edge = new Edge<int>(1, 2);
            wrappedGraph.AddVertex(1);
            wrappedGraph.AddVertex(2);
            wrappedGraph.AddEdge(edge);

            AssertNoEdge(graph);  // Graph is not updated
        }

        protected static void AddEdge_ImmutableGraph_WithUpdate<TGraph>(
            [NotNull] TGraph wrappedGraph,
            [NotNull, InstantHandle] Func<IEdgeSet<int, Edge<int>>> createGraph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, Edge<int>>
        {
            IEdgeSet<int, Edge<int>> graph = createGraph();

            var edge = new Edge<int>(1, 2);
            wrappedGraph.AddVertex(1);
            wrappedGraph.AddVertex(2);
            wrappedGraph.AddEdge(edge);

            AssertHasEdges(graph, new[] { edge });  // Graph is updated
        }

        #endregion
    }
}