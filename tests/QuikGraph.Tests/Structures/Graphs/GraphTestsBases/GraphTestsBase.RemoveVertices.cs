using System;
using JetBrains.Annotations;
using NUnit.Framework;
using static QuikGraph.Tests.GraphTestHelpers;

namespace QuikGraph.Tests.Structures
{
    internal partial class GraphTestsBase
    {
        #region Remove Vertices

        protected static void RemoveVertex_Test(
            [NotNull] IMutableVertexAndEdgeSet<int, Edge<int>> graph)
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            graph.VertexRemoved += v =>
            {
                Assert.IsNotNull(v);
                // ReSharper disable once AccessToModifiedClosure
                ++verticesRemoved;
            };
            graph.EdgeRemoved += e =>
            {
                Assert.IsNotNull(e);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge14, edge24, edge31, edge33 });

            Assert.IsFalse(graph.RemoveVertex(5));
            CheckCounters(0, 0);

            Assert.IsTrue(graph.RemoveVertex(3));
            CheckCounters(1, 3);
            AssertHasVertices(graph, new[] { 1, 2, 4 });
            AssertHasEdges(graph, new[] { edge12, edge14, edge24 });

            Assert.IsTrue(graph.RemoveVertex(1));
            CheckCounters(1, 2);
            AssertHasVertices(graph, new[] { 2, 4 });
            AssertHasEdges(graph, new[] { edge24 });

            Assert.IsTrue(graph.RemoveVertex(2));
            CheckCounters(1, 1);
            AssertHasVertices(graph, new[] { 4 });
            AssertNoEdge(graph);

            Assert.IsTrue(graph.RemoveVertex(4));
            CheckCounters(1, 0);
            AssertEmptyGraph(graph);

            #region Local function

            void CheckCounters(int expectedRemovedVertices, int expectedRemovedEdges)
            {
                Assert.AreEqual(expectedRemovedVertices, verticesRemoved);
                Assert.AreEqual(expectedRemovedEdges, edgesRemoved);
                verticesRemoved = 0;
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveVertex_Clusters_Test(
            [NotNull] ClusteredAdjacencyGraph<int, Edge<int>> graph)
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge14, edge24, edge31, edge33 });

            Assert.IsFalse(graph.RemoveVertex(5));

            Assert.IsTrue(graph.RemoveVertex(3));
            AssertHasVertices(graph, new[] { 1, 2, 4 });
            AssertHasEdges(graph, new[] { edge12, edge14, edge24 });

            Assert.IsTrue(graph.RemoveVertex(1));
            AssertHasVertices(graph, new[] { 2, 4 });
            AssertHasEdges(graph, new[] { edge24 });

            Assert.IsTrue(graph.RemoveVertex(2));
            AssertHasVertices(graph, new[] { 4 });
            AssertNoEdge(graph);

            Assert.IsTrue(graph.RemoveVertex(4));
            AssertEmptyGraph(graph);


            // With cluster
            ClusteredAdjacencyGraph<int, Edge<int>> cluster1 = graph.AddCluster();
            ClusteredAdjacencyGraph<int, Edge<int>> cluster2 = graph.AddCluster();
            ClusteredAdjacencyGraph<int, Edge<int>> cluster3 = graph.AddCluster();

            cluster1.AddVertexRange(new[] { 1, 2 });
            AssertHasVertices(cluster1, new[] { 1, 2 });

            cluster2.AddVertexRange(new[] { 1, 2, 4 });
            AssertHasVertices(cluster2, new[] { 1, 2, 4 });

            cluster3.AddVertex(2);
            AssertHasVertices(cluster3, new[] { 2 });

            graph.AddVertexRange(new[] { 1, 2, 3, 4 });
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });


            graph.RemoveVertex(2);
            AssertHasVertices(graph, new[] { 1, 3, 4 });
            AssertHasVertices(cluster1, new[] { 1 });
            AssertHasVertices(cluster2, new[] { 1, 4 });
            AssertNoVertex(cluster3);

            graph.RemoveVertex(1);
            AssertHasVertices(graph, new[] { 3, 4 });
            AssertNoVertex(cluster1);
            AssertHasVertices(cluster2, new[] { 4 });
            AssertNoVertex(cluster3);

            graph.RemoveVertex(3);
            AssertHasVertices(graph, new[] { 4 });
            AssertNoVertex(cluster1);
            AssertHasVertices(cluster2, new[] { 4 });
            AssertNoVertex(cluster3);

            graph.RemoveVertex(4);
            AssertNoVertex(graph);
            AssertNoVertex(cluster1);
            AssertNoVertex(cluster2);
            AssertNoVertex(cluster3);
        }

        protected static void RemoveVertex_Throws_Test<TVertex>(
            [NotNull] IMutableVertexSet<TVertex> graph)
            where TVertex : class
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.RemoveVertex(null));
        }

        protected static void RemoveVertex_Throws_Clusters_Test<TVertex, TEdge>(
            [NotNull] ClusteredAdjacencyGraph<TVertex, TEdge> graph)
            where TVertex : class
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.RemoveVertex(null));
        }

        protected static void RemoveVertexIf_Test(
            [NotNull] IMutableVertexAndEdgeSet<int, Edge<int>> graph)
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            graph.VertexRemoved += v =>
            {
                Assert.IsNotNull(v);
                // ReSharper disable once AccessToModifiedClosure
                ++verticesRemoved;
            };
            graph.EdgeRemoved += e =>
            {
                Assert.IsNotNull(e);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge14, edge24, edge31, edge33 });

            Assert.AreEqual(0, graph.RemoveVertexIf(vertex => vertex > 10));
            CheckCounters(0, 0);

            Assert.AreEqual(2, graph.RemoveVertexIf(vertex => vertex > 2));
            CheckCounters(2, 5);
            AssertHasVertices(graph, new[] { 1, 2 });
            AssertHasEdges(graph, new[] { edge12 });

            Assert.AreEqual(2, graph.RemoveVertexIf(vertex => true));
            CheckCounters(2, 1);
            AssertEmptyGraph(graph);

            #region Local function

            void CheckCounters(int expectedRemovedVertices, int expectedRemovedEdges)
            {
                Assert.AreEqual(expectedRemovedVertices, verticesRemoved);
                Assert.AreEqual(expectedRemovedEdges, edgesRemoved);
                verticesRemoved = 0;
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveVertexIf_Clusters_Test(
            [NotNull] ClusteredAdjacencyGraph<int, Edge<int>> graph)
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge14, edge24, edge31, edge33 });

            Assert.AreEqual(0, graph.RemoveVertexIf(vertex => vertex > 10));

            Assert.AreEqual(2, graph.RemoveVertexIf(vertex => vertex > 2));
            AssertHasVertices(graph, new[] { 1, 2 });
            AssertHasEdges(graph, new[] { edge12 });

            Assert.AreEqual(2, graph.RemoveVertexIf(vertex => true));
            AssertEmptyGraph(graph);
        }

        protected static void RemoveVertexIf_Throws_Test<TVertex>(
            [NotNull] IMutableVertexSet<TVertex> graph)
            where TVertex : class
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.RemoveVertexIf(null));
        }

        protected static void RemoveVertexIf_Throws_Clusters_Test<TVertex, TEdge>(
            [NotNull] ClusteredAdjacencyGraph<TVertex, TEdge> graph)
            where TVertex : class
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.RemoveVertexIf(null));
        }

        #endregion
    }
}