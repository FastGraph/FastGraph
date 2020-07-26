using System;
using JetBrains.Annotations;
using NUnit.Framework;
using static QuikGraph.Tests.GraphTestHelpers;

namespace QuikGraph.Tests.Structures
{
    internal partial class GraphTestsBase
    {
        #region Remove Edges

        protected static void RemoveEdge_Test(
            [NotNull] IMutableVertexAndEdgeSet<int, Edge<int>> graph)
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            graph.VertexRemoved += v =>
            {
                Assert.IsNotNull(v);
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
            var edge13Bis = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            var edgeNotInGraph = new Edge<int>(3, 4);
            var edgeWithVertexNotInGraph1 = new Edge<int>(2, 10);
            var edgeWithVertexNotInGraph2 = new Edge<int>(10, 2);
            var edgeWithVerticesNotInGraph = new Edge<int>(10, 11);
            var edgeNotEquatable = new Edge<int>(1, 2);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge13Bis, edge14, edge24, edge31, edge33 });

            Assert.IsFalse(graph.RemoveEdge(edgeNotInGraph));
            CheckCounters(0);

            Assert.IsFalse(graph.RemoveEdge(edgeWithVertexNotInGraph1));
            CheckCounters(0);

            Assert.IsFalse(graph.RemoveEdge(edgeWithVertexNotInGraph2));
            CheckCounters(0);

            Assert.IsFalse(graph.RemoveEdge(edgeWithVerticesNotInGraph));
            CheckCounters(0);

            Assert.IsFalse(graph.RemoveEdge(edgeNotEquatable));
            CheckCounters(0);

            Assert.IsTrue(graph.RemoveEdge(edge13Bis));
            CheckCounters(1);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge13, edge14, edge24, edge31, edge33 });

            Assert.IsTrue(graph.RemoveEdge(edge31));
            CheckCounters(1);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge13, edge14, edge24, edge33 });

            Assert.IsTrue(graph.RemoveEdge(edge12));
            Assert.IsTrue(graph.RemoveEdge(edge13));
            Assert.IsTrue(graph.RemoveEdge(edge14));
            Assert.IsTrue(graph.RemoveEdge(edge24));
            Assert.IsTrue(graph.RemoveEdge(edge33));
            CheckCounters(5);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertNoEdge(graph);

            #region Local function

            void CheckCounters(int expectedRemovedEdges)
            {
                Assert.AreEqual(0, verticesRemoved);
                Assert.AreEqual(expectedRemovedEdges, edgesRemoved);
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveEdge_EdgesOnly_Test(
            [NotNull] EdgeListGraph<int, Edge<int>> graph)
        {
            int edgesRemoved = 0;

            graph.EdgeRemoved += e =>
            {
                Assert.IsNotNull(e);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge13Bis = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            var edgeNotInGraph = new Edge<int>(3, 4);
            var edgeNotEquatable = new Edge<int>(1, 2);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge13Bis, edge14, edge24, edge31, edge33 });

            Assert.IsFalse(graph.RemoveEdge(edgeNotInGraph));
            CheckCounter(0);

            Assert.IsFalse(graph.RemoveEdge(edgeNotEquatable));
            CheckCounter(0);

            Assert.IsTrue(graph.RemoveEdge(edge13Bis));
            CheckCounter(1);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge13, edge14, edge24, edge31, edge33 });

            Assert.IsTrue(graph.RemoveEdge(edge31));
            CheckCounter(1);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge13, edge14, edge24, edge33 });

            Assert.IsTrue(graph.RemoveEdge(edge12));
            Assert.IsTrue(graph.RemoveEdge(edge13));
            Assert.IsTrue(graph.RemoveEdge(edge14));
            Assert.IsTrue(graph.RemoveEdge(edge24));
            Assert.IsTrue(graph.RemoveEdge(edge33));
            CheckCounter(5);
            AssertEmptyGraph(graph);    // Vertices removed in the same time as edges

            #region Local function

            void CheckCounter(int expectedRemovedEdges)
            {
                Assert.AreEqual(expectedRemovedEdges, edgesRemoved);
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveEdge_ImmutableVertices_Test(
            [NotNull] BidirectionalMatrixGraph<Edge<int>> graph)
        {
            int edgesRemoved = 0;

            graph.EdgeRemoved += e =>
            {
                Assert.IsNotNull(e);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            var edge01 = new Edge<int>(0, 1);
            var edge02 = new Edge<int>(0, 2);
            var edge03 = new Edge<int>(0, 3);
            var edge13 = new Edge<int>(1, 3);
            var edge20 = new Edge<int>(2, 0);
            var edge22 = new Edge<int>(2, 2);
            var edgeNotInGraph = new Edge<int>(2, 3);
            var edgeWithVertexNotInGraph1 = new Edge<int>(2, 10);
            var edgeWithVertexNotInGraph2 = new Edge<int>(10, 2);
            var edgeWithVerticesNotInGraph = new Edge<int>(10, 11);
            var edgeNotEquatable = new Edge<int>(0, 1);
            graph.AddEdgeRange(new[] { edge01, edge02, edge03, edge13, edge20, edge22 });

            Assert.IsFalse(graph.RemoveEdge(edgeNotInGraph));
            CheckCounter(0);

            Assert.IsFalse(graph.RemoveEdge(edgeWithVertexNotInGraph1));
            CheckCounter(0);

            Assert.IsFalse(graph.RemoveEdge(edgeWithVertexNotInGraph2));
            CheckCounter(0);

            Assert.IsFalse(graph.RemoveEdge(edgeWithVerticesNotInGraph));
            CheckCounter(0);

            Assert.IsTrue(graph.RemoveEdge(edgeNotEquatable));
            CheckCounter(1);
            AssertHasVertices(graph, new[] { 0, 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge02, edge03, edge13, edge20, edge22 });

            Assert.IsTrue(graph.RemoveEdge(edge02));
            CheckCounter(1);
            AssertHasVertices(graph, new[] { 0, 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge03, edge13, edge20, edge22 });

            Assert.IsTrue(graph.RemoveEdge(edge20));
            CheckCounter(1);
            AssertHasVertices(graph, new[] { 0, 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge03, edge13, edge22 });

            Assert.IsTrue(graph.RemoveEdge(edge03));
            Assert.IsTrue(graph.RemoveEdge(edge13));
            Assert.IsTrue(graph.RemoveEdge(edge22));
            CheckCounter(3);
            AssertHasVertices(graph, new[] { 0, 1, 2, 3 });
            AssertNoEdge(graph);

            #region Local function

            void CheckCounter(int expectedRemovedEdges)
            {
                Assert.AreEqual(expectedRemovedEdges, edgesRemoved);
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveEdge_Clusters_Test(
            [NotNull] ClusteredAdjacencyGraph<int, Edge<int>> graph)
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge13Bis = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            var edgeNotInGraph = new Edge<int>(3, 4);
            var edgeWithVertexNotInGraph1 = new Edge<int>(2, 10);
            var edgeWithVertexNotInGraph2 = new Edge<int>(10, 2);
            var edgeWithVerticesNotInGraph = new Edge<int>(10, 11);
            var edgeNotEquatable = new Edge<int>(1, 2);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge13Bis, edge14, edge24, edge31, edge33 });

            Assert.IsFalse(graph.RemoveEdge(edgeNotInGraph));
            Assert.IsFalse(graph.RemoveEdge(edgeWithVertexNotInGraph1));
            Assert.IsFalse(graph.RemoveEdge(edgeWithVertexNotInGraph2));
            Assert.IsFalse(graph.RemoveEdge(edgeWithVerticesNotInGraph));
            Assert.IsFalse(graph.RemoveEdge(edgeNotEquatable));

            Assert.IsTrue(graph.RemoveEdge(edge13Bis));
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge13, edge14, edge24, edge31, edge33 });

            Assert.IsTrue(graph.RemoveEdge(edge31));
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge13, edge14, edge24, edge33 });

            Assert.IsTrue(graph.RemoveEdge(edge12));
            Assert.IsTrue(graph.RemoveEdge(edge13));
            Assert.IsTrue(graph.RemoveEdge(edge14));
            Assert.IsTrue(graph.RemoveEdge(edge24));
            Assert.IsTrue(graph.RemoveEdge(edge33));
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertNoEdge(graph);


            // With cluster
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge14, edge24, edge31 });
            AssertHasEdges(graph, new[] { edge12, edge13, edge14, edge24, edge31 });

            ClusteredAdjacencyGraph<int, Edge<int>> cluster1 = graph.AddCluster();
            ClusteredAdjacencyGraph<int, Edge<int>> cluster2 = graph.AddCluster();
            ClusteredAdjacencyGraph<int, Edge<int>> cluster3 = graph.AddCluster();

            cluster1.AddVerticesAndEdgeRange(new[] { edge12, edge13 });
            AssertHasEdges(cluster1, new[] { edge12, edge13 });

            cluster2.AddVerticesAndEdgeRange(new[] { edge12, edge14, edge24 });
            AssertHasEdges(cluster2, new[] { edge12, edge14, edge24 });

            cluster3.AddVerticesAndEdge(edge12);
            AssertHasEdges(cluster3, new[] { edge12 });


            graph.RemoveEdge(edge12);
            AssertHasEdges(graph, new[] { edge13, edge14, edge24, edge31 });
            AssertHasEdges(cluster1, new[] { edge13 });
            AssertHasEdges(cluster2, new[] { edge14, edge24 });
            AssertNoEdge(cluster3);

            graph.RemoveEdge(edge13);
            AssertHasEdges(graph, new[] { edge14, edge24, edge31 });
            AssertNoEdge(cluster1);
            AssertHasEdges(cluster2, new[] { edge14, edge24 });
            AssertNoEdge(cluster3);

            graph.RemoveEdge(edge24);
            AssertHasEdges(graph, new[] { edge14, edge31 });
            AssertNoEdge(cluster1);
            AssertHasEdges(cluster2, new[] { edge14 });
            AssertNoEdge(cluster3);

            graph.RemoveEdge(edge14);
            AssertHasEdges(graph, new[] { edge31 });
            AssertNoEdge(cluster1);
            AssertNoEdge(cluster2);
            AssertNoEdge(cluster3);
        }

        protected static void RemoveEdge_EquatableEdge_Test(
            [NotNull] IMutableVertexAndEdgeSet<int, EquatableEdge<int>> graph)
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            graph.VertexRemoved += v =>
            {
                Assert.IsNotNull(v);
                ++verticesRemoved;
            };
            graph.EdgeRemoved += e =>
            {
                Assert.IsNotNull(e);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            var edge12 = new EquatableEdge<int>(1, 2);
            var edge13 = new EquatableEdge<int>(1, 3);
            var edge13Bis = new EquatableEdge<int>(1, 3);
            var edge14 = new EquatableEdge<int>(1, 4);
            var edge24 = new EquatableEdge<int>(2, 4);
            var edge31 = new EquatableEdge<int>(3, 1);
            var edge33 = new EquatableEdge<int>(3, 3);
            var edgeNotInGraph = new EquatableEdge<int>(3, 4);
            var edgeWithVertexNotInGraph1 = new EquatableEdge<int>(2, 10);
            var edgeWithVertexNotInGraph2 = new EquatableEdge<int>(10, 2);
            var edgeWithVerticesNotInGraph = new EquatableEdge<int>(10, 11);
            var edgeEquatable = new EquatableEdge<int>(1, 2);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge13Bis, edge14, edge24, edge31, edge33 });

            Assert.IsFalse(graph.RemoveEdge(edgeNotInGraph));
            CheckCounters(0);

            Assert.IsFalse(graph.RemoveEdge(edgeWithVertexNotInGraph1));
            CheckCounters(0);

            Assert.IsFalse(graph.RemoveEdge(edgeWithVertexNotInGraph2));
            CheckCounters(0);

            Assert.IsFalse(graph.RemoveEdge(edgeWithVerticesNotInGraph));
            CheckCounters(0);

            Assert.IsTrue(graph.RemoveEdge(edgeEquatable));
            CheckCounters(1);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge13, edge13Bis, edge14, edge24, edge31, edge33 });

            Assert.IsTrue(graph.RemoveEdge(edge13Bis));
            CheckCounters(1);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge13, edge14, edge24, edge31, edge33 });

            Assert.IsTrue(graph.RemoveEdge(edge31));
            CheckCounters(1);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge13, edge14, edge24, edge33 });

            Assert.IsTrue(graph.RemoveEdge(edge13));
            Assert.IsTrue(graph.RemoveEdge(edge14));
            Assert.IsTrue(graph.RemoveEdge(edge24));
            Assert.IsTrue(graph.RemoveEdge(edge33));
            CheckCounters(4);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertNoEdge(graph);

            #region Local function

            void CheckCounters(int expectedRemovedEdges)
            {
                Assert.AreEqual(0, verticesRemoved);
                Assert.AreEqual(expectedRemovedEdges, edgesRemoved);
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveEdge_EquatableEdge_EdgesOnly_Test(
            [NotNull] EdgeListGraph<int, EquatableEdge<int>> graph)
        {
            int edgesRemoved = 0;

            graph.EdgeRemoved += e =>
            {
                Assert.IsNotNull(e);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            var edge12 = new EquatableEdge<int>(1, 2);
            var edge13 = new EquatableEdge<int>(1, 3);
            var edge14 = new EquatableEdge<int>(1, 4);
            var edge24 = new EquatableEdge<int>(2, 4);
            var edge31 = new EquatableEdge<int>(3, 1);
            var edge33 = new EquatableEdge<int>(3, 3);
            var edgeNotInGraph = new EquatableEdge<int>(3, 4);
            var edgeEquatable = new EquatableEdge<int>(1, 2);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge14, edge24, edge31, edge33 });

            Assert.IsFalse(graph.RemoveEdge(edgeNotInGraph));
            CheckCounter(0);

            Assert.IsTrue(graph.RemoveEdge(edgeEquatable));
            CheckCounter(1);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge13, edge14, edge24, edge31, edge33 });

            Assert.IsTrue(graph.RemoveEdge(edge13));
            CheckCounter(1);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge14, edge24, edge31, edge33 });

            Assert.IsTrue(graph.RemoveEdge(edge31));
            CheckCounter(1);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge14, edge24, edge33 });

            Assert.IsTrue(graph.RemoveEdge(edge14));
            Assert.IsTrue(graph.RemoveEdge(edge24));
            Assert.IsTrue(graph.RemoveEdge(edge33));
            CheckCounter(3);
            AssertEmptyGraph(graph);    // Vertices removed in the same time as edges

            #region Local function

            void CheckCounter(int expectedRemovedEdges)
            {
                Assert.AreEqual(expectedRemovedEdges, edgesRemoved);
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveEdge_EquatableEdge_ImmutableVertices_Test(
            [NotNull] BidirectionalMatrixGraph<EquatableEdge<int>> graph)
        {
            int edgesRemoved = 0;

            graph.EdgeRemoved += e =>
            {
                Assert.IsNotNull(e);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            var edge01 = new EquatableEdge<int>(0, 1);
            var edge02 = new EquatableEdge<int>(0, 2);
            var edge03 = new EquatableEdge<int>(0, 3);
            var edge13 = new EquatableEdge<int>(1, 3);
            var edge20 = new EquatableEdge<int>(2, 0);
            var edge22 = new EquatableEdge<int>(2, 2);
            var edgeNotInGraph = new EquatableEdge<int>(2, 3);
            var edgeWithVertexNotInGraph1 = new EquatableEdge<int>(2, 10);
            var edgeWithVertexNotInGraph2 = new EquatableEdge<int>(10, 2);
            var edgeWithVerticesNotInGraph = new EquatableEdge<int>(10, 11);
            var edgeNotEquatable = new EquatableEdge<int>(0, 1);
            graph.AddEdgeRange(new[] { edge01, edge02, edge03, edge13, edge20, edge22 });

            Assert.IsFalse(graph.RemoveEdge(edgeNotInGraph));
            CheckCounter(0);

            Assert.IsFalse(graph.RemoveEdge(edgeWithVertexNotInGraph1));
            CheckCounter(0);

            Assert.IsFalse(graph.RemoveEdge(edgeWithVertexNotInGraph2));
            CheckCounter(0);

            Assert.IsFalse(graph.RemoveEdge(edgeWithVerticesNotInGraph));
            CheckCounter(0);

            Assert.IsTrue(graph.RemoveEdge(edgeNotEquatable));
            CheckCounter(1);
            AssertHasVertices(graph, new[] { 0, 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge02, edge03, edge13, edge20, edge22 });

            Assert.IsTrue(graph.RemoveEdge(edge02));
            CheckCounter(1);
            AssertHasVertices(graph, new[] { 0, 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge03, edge13, edge20, edge22 });

            Assert.IsTrue(graph.RemoveEdge(edge20));
            CheckCounter(1);
            AssertHasVertices(graph, new[] { 0, 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge03, edge13, edge22 });

            Assert.IsTrue(graph.RemoveEdge(edge03));
            Assert.IsTrue(graph.RemoveEdge(edge13));
            Assert.IsTrue(graph.RemoveEdge(edge22));
            CheckCounter(3);
            AssertHasVertices(graph, new[] { 0, 1, 2, 3 });
            AssertNoEdge(graph);

            #region Local function

            void CheckCounter(int expectedRemovedEdges)
            {
                Assert.AreEqual(expectedRemovedEdges, edgesRemoved);
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveEdge_EquatableEdge_Clusters_Test(
            [NotNull] ClusteredAdjacencyGraph<int, EquatableEdge<int>> graph)
        {
            var edge12 = new EquatableEdge<int>(1, 2);
            var edge13 = new EquatableEdge<int>(1, 3);
            var edge13Bis = new EquatableEdge<int>(1, 3);
            var edge14 = new EquatableEdge<int>(1, 4);
            var edge24 = new EquatableEdge<int>(2, 4);
            var edge31 = new EquatableEdge<int>(3, 1);
            var edge33 = new EquatableEdge<int>(3, 3);
            var edgeNotInGraph = new EquatableEdge<int>(3, 4);
            var edgeWithVertexNotInGraph1 = new EquatableEdge<int>(2, 10);
            var edgeWithVertexNotInGraph2 = new EquatableEdge<int>(10, 2);
            var edgeWithVerticesNotInGraph = new EquatableEdge<int>(10, 11);
            var edgeEquatable = new EquatableEdge<int>(1, 2);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge13Bis, edge14, edge24, edge31, edge33 });

            Assert.IsFalse(graph.RemoveEdge(edgeNotInGraph));
            Assert.IsFalse(graph.RemoveEdge(edgeWithVertexNotInGraph1));
            Assert.IsFalse(graph.RemoveEdge(edgeWithVertexNotInGraph2));
            Assert.IsFalse(graph.RemoveEdge(edgeWithVerticesNotInGraph));

            Assert.IsTrue(graph.RemoveEdge(edgeEquatable));
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge13, edge13Bis, edge14, edge24, edge31, edge33 });

            Assert.IsTrue(graph.RemoveEdge(edge13Bis));
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge13, edge14, edge24, edge31, edge33 });

            Assert.IsTrue(graph.RemoveEdge(edge31));
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge13, edge14, edge24, edge33 });

            Assert.IsTrue(graph.RemoveEdge(edge13));
            Assert.IsTrue(graph.RemoveEdge(edge14));
            Assert.IsTrue(graph.RemoveEdge(edge24));
            Assert.IsTrue(graph.RemoveEdge(edge33));
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertNoEdge(graph);
        }

        protected static void RemoveEdge_Throws_Test<TVertex, TEdge>(
            [NotNull] IMutableEdgeListGraph<TVertex, TEdge> graph)
            where TEdge : class, IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.RemoveEdge(null));
        }

        protected static void RemoveEdge_Throws_Clusters_Test<TVertex, TEdge>(
            [NotNull] ClusteredAdjacencyGraph<TVertex, TEdge> graph)
            where TEdge : class, IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.RemoveEdge(null));
        }

        protected static void RemoveEdgeIf_Test<TGraph>([NotNull] TGraph graph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, Edge<int>>
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            graph.VertexRemoved += v =>
            {
                Assert.IsNotNull(v);
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
            var edge13Bis = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            graph.AddVertexRange(new[] { 1, 2, 3, 4 });
            graph.AddEdgeRange(new[] { edge12, edge13, edge13Bis, edge14, edge24, edge31, edge33 });

            Assert.AreEqual(0, graph.RemoveEdgeIf(edge => edge.Target == 5));
            CheckCounters(0);

            Assert.AreEqual(2, graph.RemoveEdgeIf(edge => edge.Source == 3));
            CheckCounters(2);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge13, edge13Bis, edge14, edge24 });

            Assert.AreEqual(5, graph.RemoveEdgeIf(edge => true));
            CheckCounters(5);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertNoEdge(graph);

            #region Local function

            void CheckCounters(int expectedRemovedEdges)
            {
                Assert.AreEqual(0, verticesRemoved);
                Assert.AreEqual(expectedRemovedEdges, edgesRemoved);
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveEdgeIf_EdgesOnly_Test(
            [NotNull] EdgeListGraph<int, Edge<int>> graph)
        {
            int edgesRemoved = 0;

            graph.EdgeRemoved += e =>
            {
                Assert.IsNotNull(e);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge13Bis = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            graph.AddEdgeRange(new[] { edge12, edge13, edge13Bis, edge14, edge24, edge31, edge33 });

            Assert.AreEqual(0, graph.RemoveEdgeIf(edge => edge.Target == 5));
            CheckCounter(0);

            Assert.AreEqual(2, graph.RemoveEdgeIf(edge => edge.Source == 3));
            CheckCounter(2);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge13, edge13Bis, edge14, edge24 });

            Assert.AreEqual(5, graph.RemoveEdgeIf(edge => true));
            CheckCounter(5);
            AssertEmptyGraph(graph);    // Vertices removed in the same time as edges

            #region Local function

            void CheckCounter(int expectedRemovedEdges)
            {
                Assert.AreEqual(expectedRemovedEdges, edgesRemoved);
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveEdgeIf_Clusters_Test(
            [NotNull] ClusteredAdjacencyGraph<int, Edge<int>> graph)
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge13Bis = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            graph.AddVertexRange(new[] { 1, 2, 3, 4 });
            graph.AddEdgeRange(new[] { edge12, edge13, edge13Bis, edge14, edge24, edge31, edge33 });

            Assert.AreEqual(0, graph.RemoveEdgeIf(edge => edge.Target == 5));

            Assert.AreEqual(2, graph.RemoveEdgeIf(edge => edge.Source == 3));
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge13, edge13Bis, edge14, edge24 });

            Assert.AreEqual(5, graph.RemoveEdgeIf(edge => true));
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertNoEdge(graph);
        }

        protected static void RemoveEdgeIf_Throws_Test<TVertex, TEdge>(
            [NotNull] IMutableEdgeListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.RemoveEdgeIf(null));
        }

        protected static void RemoveEdgeIf_Throws_Clusters_Test<TVertex, TEdge>(
            [NotNull] ClusteredAdjacencyGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.RemoveEdgeIf(null));
        }

        protected static void RemoveOutEdgeIf_Test(
            [NotNull] IMutableVertexAndEdgeListGraph<int, Edge<int>> graph)
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            graph.VertexRemoved += v =>
            {
                Assert.IsNotNull(v);
                ++verticesRemoved;
            };
            graph.EdgeRemoved += e =>
            {
                Assert.IsNotNull(e);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            Assert.AreEqual(0, graph.RemoveOutEdgeIf(1, edge => true));
            CheckCounters(0);
            AssertEmptyGraph(graph);

            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge13Bis = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge13Bis, edge14, edge24, edge31, edge33 });

            Assert.AreEqual(3, graph.RemoveOutEdgeIf(1, edge => edge.Target >= 3));
            CheckCounters(3);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge24, edge31, edge33 });

            Assert.AreEqual(0, graph.RemoveOutEdgeIf(3, edge => edge.Target > 5));
            CheckCounters(0);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge24, edge31, edge33 });

            Assert.AreEqual(2, graph.RemoveOutEdgeIf(3, edge => true));
            CheckCounters(2);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge24 });

            #region Local function

            void CheckCounters(int expectedRemovedEdges)
            {
                Assert.AreEqual(0, verticesRemoved);
                Assert.AreEqual(expectedRemovedEdges, edgesRemoved);
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveOutEdgeIf_ImmutableVertices_Test(
            [NotNull] BidirectionalMatrixGraph<Edge<int>> graph)
        {
            int edgesRemoved = 0;

            graph.EdgeRemoved += e =>
            {
                Assert.IsNotNull(e);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            Assert.AreEqual(0, graph.RemoveOutEdgeIf(6, edge => true));
            CheckCounter(0);
            AssertNoEdge(graph);

            var edge01 = new Edge<int>(0, 1);
            var edge02 = new Edge<int>(0, 2);
            var edge03 = new Edge<int>(0, 3);
            var edge13 = new Edge<int>(1, 3);
            var edge20 = new Edge<int>(2, 0);
            var edge22 = new Edge<int>(2, 2);
            graph.AddEdgeRange(new[] { edge01, edge02, edge03, edge13, edge20, edge22 });

            Assert.AreEqual(2, graph.RemoveOutEdgeIf(0, edge => edge.Target >= 2));
            CheckCounter(2);
            AssertHasEdges(graph, new[] { edge01, edge13, edge20, edge22 });

            Assert.AreEqual(0, graph.RemoveOutEdgeIf(2, edge => edge.Target > 4));
            CheckCounter(0);
            AssertHasEdges(graph, new[] { edge01, edge13, edge20, edge22 });

            Assert.AreEqual(2, graph.RemoveOutEdgeIf(2, edge => true));
            CheckCounter(2);
            AssertHasEdges(graph, new[] { edge01, edge13 });

            #region Local function

            void CheckCounter(int expectedRemovedEdges)
            {
                Assert.AreEqual(expectedRemovedEdges, edgesRemoved);
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveOutEdgeIf_Clusters_Test(
            [NotNull] ClusteredAdjacencyGraph<int, Edge<int>> graph)
        {
            Assert.AreEqual(0, graph.RemoveOutEdgeIf(1, edge => true));
            AssertEmptyGraph(graph);

            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge13Bis = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge13Bis, edge14, edge24, edge31, edge33 });

            Assert.AreEqual(3, graph.RemoveOutEdgeIf(1, edge => edge.Target >= 3));
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge24, edge31, edge33 });

            Assert.AreEqual(0, graph.RemoveOutEdgeIf(3, edge => edge.Target > 5));
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge24, edge31, edge33 });

            Assert.AreEqual(2, graph.RemoveOutEdgeIf(3, edge => true));
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge24 });
        }

        protected static void RemoveOutEdgeIf_Throws_Test<TEdge>(
            [NotNull] BidirectionalMatrixGraph<TEdge> graph)
            where TEdge : class, IEdge<int>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.RemoveOutEdgeIf(default, null));
        }

        protected static void RemoveOutEdgeIf_Throws_Test<TVertex, TEdge>(
            [NotNull] IMutableIncidenceGraph<TVertex, TEdge> graph)
            where TVertex : class, new()
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.RemoveOutEdgeIf(null, edge => true));
            Assert.Throws<ArgumentNullException>(() => graph.RemoveOutEdgeIf(new TVertex(), null));
            Assert.Throws<ArgumentNullException>(() => graph.RemoveOutEdgeIf(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
        }

        protected static void RemoveOutEdgeIf_Throws_Test<TVertex, TEdge>(
            [NotNull] ClusteredAdjacencyGraph<TVertex, TEdge> graph)
            where TVertex : class, new()
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.RemoveOutEdgeIf(null, edge => true));
            Assert.Throws<ArgumentNullException>(() => graph.RemoveOutEdgeIf(new TVertex(), null));
            Assert.Throws<ArgumentNullException>(() => graph.RemoveOutEdgeIf(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
        }

        protected static void RemoveInEdgeIf_Test(
            [NotNull] IMutableBidirectionalGraph<int, Edge<int>> graph)
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            graph.VertexRemoved += v =>
            {
                Assert.IsNotNull(v);
                ++verticesRemoved;
            };
            graph.EdgeRemoved += e =>
            {
                Assert.IsNotNull(e);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            Assert.AreEqual(0, graph.RemoveInEdgeIf(1, edge => true));
            CheckCounters(0);
            AssertEmptyGraph(graph);

            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge13Bis = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge13Bis, edge14, edge24, edge31, edge33 });

            Assert.AreEqual(2, graph.RemoveInEdgeIf(3, edge => edge.Source == 1));
            CheckCounters(2);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge14, edge24, edge31, edge33 });

            Assert.AreEqual(0, graph.RemoveInEdgeIf(3, edge => edge.Target > 5));
            CheckCounters(0);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge14, edge24, edge31, edge33 });

            Assert.AreEqual(1, graph.RemoveInEdgeIf(2, edge => true));
            CheckCounters(1);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge14, edge24, edge31, edge33 });

            #region Local function

            void CheckCounters(int expectedRemovedEdges)
            {
                Assert.AreEqual(0, verticesRemoved);
                Assert.AreEqual(expectedRemovedEdges, edgesRemoved);
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveInEdgeIf_ImmutableVertices_Test(
            [NotNull] BidirectionalMatrixGraph<Edge<int>> graph)
        {
            int edgesRemoved = 0;

            graph.EdgeRemoved += e =>
            {
                Assert.IsNotNull(e);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            Assert.AreEqual(0, graph.RemoveInEdgeIf(6, edge => true));
            CheckCounter(0);
            AssertNoEdge(graph);

            var edge01 = new Edge<int>(0, 1);
            var edge02 = new Edge<int>(0, 2);
            var edge03 = new Edge<int>(0, 3);
            var edge13 = new Edge<int>(1, 3);
            var edge20 = new Edge<int>(2, 0);
            var edge22 = new Edge<int>(2, 2);
            graph.AddEdgeRange(new[] { edge01, edge02, edge03, edge13, edge20, edge22 });

            Assert.AreEqual(1, graph.RemoveInEdgeIf(2, edge => edge.Source == 0));
            CheckCounter(1);
            AssertHasEdges(graph, new[] { edge01, edge03, edge13, edge20, edge22 });

            Assert.AreEqual(0, graph.RemoveInEdgeIf(2, edge => edge.Target > 4));
            CheckCounter(0);
            AssertHasEdges(graph, new[] { edge01, edge03, edge13, edge20, edge22 });

            Assert.AreEqual(1, graph.RemoveInEdgeIf(1, edge => true));
            CheckCounter(1);
            AssertHasEdges(graph, new[] { edge03, edge13, edge20, edge22 });

            #region Local function

            void CheckCounter(int expectedRemovedEdges)
            {
                Assert.AreEqual(expectedRemovedEdges, edgesRemoved);
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveInEdgeIf_Throws_Test<TEdge>(
            [NotNull] BidirectionalMatrixGraph<TEdge> graph)
            where TEdge : class, IEdge<int>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.RemoveInEdgeIf(default, null));
        }

        protected static void RemoveInEdgeIf_Throws_Test(
            [NotNull] IMutableBidirectionalGraph<TestVertex, Edge<TestVertex>> graph)
        {
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.RemoveInEdgeIf(null, edge => true));
            Assert.Throws<ArgumentNullException>(() => graph.RemoveInEdgeIf(new TestVertex("v1"), null));
            Assert.Throws<ArgumentNullException>(() => graph.RemoveInEdgeIf(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
        }

        protected static void RemoveAdjacentEdgeIf_Test(
            [NotNull] IMutableUndirectedGraph<int, Edge<int>> graph)
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            graph.VertexRemoved += v =>
            {
                Assert.IsNotNull(v);
                ++verticesRemoved;
            };
            graph.EdgeRemoved += e =>
            {
                Assert.IsNotNull(e);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            Assert.AreEqual(0, graph.RemoveAdjacentEdgeIf(1, edge => true));
            CheckCounters(0);
            AssertEmptyGraph(graph);

            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge13Bis = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge13Bis, edge14, edge24, edge31, edge33 });

            Assert.AreEqual(4, graph.RemoveAdjacentEdgeIf(1, edge => edge.Source >= 3 || edge.Target >= 3));
            CheckCounters(4);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge24, edge33 });

            Assert.AreEqual(0, graph.RemoveAdjacentEdgeIf(3, edge => edge.Target > 5));
            CheckCounters(0);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge24, edge33 });

            Assert.AreEqual(1, graph.RemoveAdjacentEdgeIf(3, edge => true));
            CheckCounters(1);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge24 });

            #region Local function

            void CheckCounters(int expectedRemovedEdges)
            {
                Assert.AreEqual(0, verticesRemoved);
                Assert.AreEqual(expectedRemovedEdges, edgesRemoved);
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveAdjacentEdgeIf_Throws_Test(
            [NotNull] IMutableUndirectedGraph<TestVertex, Edge<TestVertex>> graph)
        {
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.RemoveAdjacentEdgeIf(null, edge => true));
            Assert.Throws<ArgumentNullException>(() => graph.RemoveAdjacentEdgeIf(new TestVertex("v1"), null));
            Assert.Throws<ArgumentNullException>(() => graph.RemoveAdjacentEdgeIf(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
        }

        #endregion
    }
}