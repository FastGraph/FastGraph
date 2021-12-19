#nullable enable

using static FastGraph.Tests.GraphTestHelpers;

namespace FastGraph.Tests.Structures
{
    internal partial class GraphTestsBase
    {
        #region Remove Edges

        protected static void RemoveEdge_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> graph)
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.VertexRemoved += v =>
            {
                ++verticesRemoved;
            };
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeRemoved += e =>
            {
                e.Should().NotBeNull();
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

            graph.RemoveEdge(edgeNotInGraph).Should().BeFalse();
            CheckCounters(0);

            graph.RemoveEdge(edgeWithVertexNotInGraph1).Should().BeFalse();
            CheckCounters(0);

            graph.RemoveEdge(edgeWithVertexNotInGraph2).Should().BeFalse();
            CheckCounters(0);

            graph.RemoveEdge(edgeWithVerticesNotInGraph).Should().BeFalse();
            CheckCounters(0);

            graph.RemoveEdge(edgeNotEquatable).Should().BeFalse();
            CheckCounters(0);

            graph.RemoveEdge(edge13Bis).Should().BeTrue();
            CheckCounters(1);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge13, edge14, edge24, edge31, edge33 });

            graph.RemoveEdge(edge31).Should().BeTrue();
            CheckCounters(1);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge13, edge14, edge24, edge33 });

            graph.RemoveEdge(edge12).Should().BeTrue();
            graph.RemoveEdge(edge13).Should().BeTrue();
            graph.RemoveEdge(edge14).Should().BeTrue();
            graph.RemoveEdge(edge24).Should().BeTrue();
            graph.RemoveEdge(edge33).Should().BeTrue();
            CheckCounters(5);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertNoEdge(graph);

            #region Local function

            void CheckCounters(int expectedRemovedEdges)
            {
                verticesRemoved.Should().Be(0);
                edgesRemoved.Should().Be(expectedRemovedEdges);
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveEdge_EdgesOnly_Test(
            EdgeListGraph<int, Edge<int>> graph)
        {
            int edgesRemoved = 0;

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeRemoved += e =>
            {
                e.Should().NotBeNull();
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

            graph.RemoveEdge(edgeNotInGraph).Should().BeFalse();
            CheckCounter(0);

            graph.RemoveEdge(edgeNotEquatable).Should().BeFalse();
            CheckCounter(0);

            graph.RemoveEdge(edge13Bis).Should().BeTrue();
            CheckCounter(1);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge13, edge14, edge24, edge31, edge33 });

            graph.RemoveEdge(edge31).Should().BeTrue();
            CheckCounter(1);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge13, edge14, edge24, edge33 });

            graph.RemoveEdge(edge12).Should().BeTrue();
            graph.RemoveEdge(edge13).Should().BeTrue();
            graph.RemoveEdge(edge14).Should().BeTrue();
            graph.RemoveEdge(edge24).Should().BeTrue();
            graph.RemoveEdge(edge33).Should().BeTrue();
            CheckCounter(5);
            AssertEmptyGraph(graph);    // Vertices removed in the same time as edges

            #region Local function

            void CheckCounter(int expectedRemovedEdges)
            {
                edgesRemoved.Should().Be(expectedRemovedEdges);
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveEdge_ImmutableVertices_Test(
            BidirectionalMatrixGraph<Edge<int>> graph)
        {
            int edgesRemoved = 0;

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeRemoved += e =>
            {
                e.Should().NotBeNull();
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

            graph.RemoveEdge(edgeNotInGraph).Should().BeFalse();
            CheckCounter(0);

            graph.RemoveEdge(edgeWithVertexNotInGraph1).Should().BeFalse();
            CheckCounter(0);

            graph.RemoveEdge(edgeWithVertexNotInGraph2).Should().BeFalse();
            CheckCounter(0);

            graph.RemoveEdge(edgeWithVerticesNotInGraph).Should().BeFalse();
            CheckCounter(0);

            graph.RemoveEdge(edgeNotEquatable).Should().BeTrue();
            CheckCounter(1);
            AssertHasVertices(graph, new[] { 0, 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge02, edge03, edge13, edge20, edge22 });

            graph.RemoveEdge(edge02).Should().BeTrue();
            CheckCounter(1);
            AssertHasVertices(graph, new[] { 0, 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge03, edge13, edge20, edge22 });

            graph.RemoveEdge(edge20).Should().BeTrue();
            CheckCounter(1);
            AssertHasVertices(graph, new[] { 0, 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge03, edge13, edge22 });

            graph.RemoveEdge(edge03).Should().BeTrue();
            graph.RemoveEdge(edge13).Should().BeTrue();
            graph.RemoveEdge(edge22).Should().BeTrue();
            CheckCounter(3);
            AssertHasVertices(graph, new[] { 0, 1, 2, 3 });
            AssertNoEdge(graph);

            #region Local function

            void CheckCounter(int expectedRemovedEdges)
            {
                edgesRemoved.Should().Be(expectedRemovedEdges);
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveEdge_Clusters_Test(
            ClusteredAdjacencyGraph<int, Edge<int>> graph)
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

            graph.RemoveEdge(edgeNotInGraph).Should().BeFalse();
            graph.RemoveEdge(edgeWithVertexNotInGraph1).Should().BeFalse();
            graph.RemoveEdge(edgeWithVertexNotInGraph2).Should().BeFalse();
            graph.RemoveEdge(edgeWithVerticesNotInGraph).Should().BeFalse();
            graph.RemoveEdge(edgeNotEquatable).Should().BeFalse();

            graph.RemoveEdge(edge13Bis).Should().BeTrue();
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge13, edge14, edge24, edge31, edge33 });

            graph.RemoveEdge(edge31).Should().BeTrue();
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge13, edge14, edge24, edge33 });

            graph.RemoveEdge(edge12).Should().BeTrue();
            graph.RemoveEdge(edge13).Should().BeTrue();
            graph.RemoveEdge(edge14).Should().BeTrue();
            graph.RemoveEdge(edge24).Should().BeTrue();
            graph.RemoveEdge(edge33).Should().BeTrue();
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
            IMutableVertexAndEdgeSet<int, EquatableEdge<int>> graph)
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.VertexRemoved += v =>
            {
                ++verticesRemoved;
            };
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeRemoved += e =>
            {
                e.Should().NotBeNull();
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

            graph.RemoveEdge(edgeNotInGraph).Should().BeFalse();
            CheckCounters(0);

            graph.RemoveEdge(edgeWithVertexNotInGraph1).Should().BeFalse();
            CheckCounters(0);

            graph.RemoveEdge(edgeWithVertexNotInGraph2).Should().BeFalse();
            CheckCounters(0);

            graph.RemoveEdge(edgeWithVerticesNotInGraph).Should().BeFalse();
            CheckCounters(0);

            graph.RemoveEdge(edgeEquatable).Should().BeTrue();
            CheckCounters(1);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge13, edge13Bis, edge14, edge24, edge31, edge33 });

            graph.RemoveEdge(edge13Bis).Should().BeTrue();
            CheckCounters(1);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge13, edge14, edge24, edge31, edge33 });

            graph.RemoveEdge(edge31).Should().BeTrue();
            CheckCounters(1);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge13, edge14, edge24, edge33 });

            graph.RemoveEdge(edge13).Should().BeTrue();
            graph.RemoveEdge(edge14).Should().BeTrue();
            graph.RemoveEdge(edge24).Should().BeTrue();
            graph.RemoveEdge(edge33).Should().BeTrue();
            CheckCounters(4);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertNoEdge(graph);

            #region Local function

            void CheckCounters(int expectedRemovedEdges)
            {
                verticesRemoved.Should().Be(0);
                edgesRemoved.Should().Be(expectedRemovedEdges);
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveEdge_EquatableEdge_EdgesOnly_Test(
            EdgeListGraph<int, EquatableEdge<int>> graph)
        {
            int edgesRemoved = 0;

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeRemoved += e =>
            {
                e.Should().NotBeNull();
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

            graph.RemoveEdge(edgeNotInGraph).Should().BeFalse();
            CheckCounter(0);

            graph.RemoveEdge(edgeEquatable).Should().BeTrue();
            CheckCounter(1);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge13, edge14, edge24, edge31, edge33 });

            graph.RemoveEdge(edge13).Should().BeTrue();
            CheckCounter(1);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge14, edge24, edge31, edge33 });

            graph.RemoveEdge(edge31).Should().BeTrue();
            CheckCounter(1);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge14, edge24, edge33 });

            graph.RemoveEdge(edge14).Should().BeTrue();
            graph.RemoveEdge(edge24).Should().BeTrue();
            graph.RemoveEdge(edge33).Should().BeTrue();
            CheckCounter(3);
            AssertEmptyGraph(graph);    // Vertices removed in the same time as edges

            #region Local function

            void CheckCounter(int expectedRemovedEdges)
            {
                edgesRemoved.Should().Be(expectedRemovedEdges);
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveEdge_EquatableEdge_ImmutableVertices_Test(
            BidirectionalMatrixGraph<EquatableEdge<int>> graph)
        {
            int edgesRemoved = 0;

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeRemoved += e =>
            {
                e.Should().NotBeNull();
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

            graph.RemoveEdge(edgeNotInGraph).Should().BeFalse();
            CheckCounter(0);

            graph.RemoveEdge(edgeWithVertexNotInGraph1).Should().BeFalse();
            CheckCounter(0);

            graph.RemoveEdge(edgeWithVertexNotInGraph2).Should().BeFalse();
            CheckCounter(0);

            graph.RemoveEdge(edgeWithVerticesNotInGraph).Should().BeFalse();
            CheckCounter(0);

            graph.RemoveEdge(edgeNotEquatable).Should().BeTrue();
            CheckCounter(1);
            AssertHasVertices(graph, new[] { 0, 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge02, edge03, edge13, edge20, edge22 });

            graph.RemoveEdge(edge02).Should().BeTrue();
            CheckCounter(1);
            AssertHasVertices(graph, new[] { 0, 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge03, edge13, edge20, edge22 });

            graph.RemoveEdge(edge20).Should().BeTrue();
            CheckCounter(1);
            AssertHasVertices(graph, new[] { 0, 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge03, edge13, edge22 });

            graph.RemoveEdge(edge03).Should().BeTrue();
            graph.RemoveEdge(edge13).Should().BeTrue();
            graph.RemoveEdge(edge22).Should().BeTrue();
            CheckCounter(3);
            AssertHasVertices(graph, new[] { 0, 1, 2, 3 });
            AssertNoEdge(graph);

            #region Local function

            void CheckCounter(int expectedRemovedEdges)
            {
                edgesRemoved.Should().Be(expectedRemovedEdges);
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveEdge_EquatableEdge_Clusters_Test(
            ClusteredAdjacencyGraph<int, EquatableEdge<int>> graph)
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

            graph.RemoveEdge(edgeNotInGraph).Should().BeFalse();
            graph.RemoveEdge(edgeWithVertexNotInGraph1).Should().BeFalse();
            graph.RemoveEdge(edgeWithVertexNotInGraph2).Should().BeFalse();
            graph.RemoveEdge(edgeWithVerticesNotInGraph).Should().BeFalse();

            graph.RemoveEdge(edgeEquatable).Should().BeTrue();
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge13, edge13Bis, edge14, edge24, edge31, edge33 });

            graph.RemoveEdge(edge13Bis).Should().BeTrue();
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge13, edge14, edge24, edge31, edge33 });

            graph.RemoveEdge(edge31).Should().BeTrue();
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge13, edge14, edge24, edge33 });

            graph.RemoveEdge(edge13).Should().BeTrue();
            graph.RemoveEdge(edge14).Should().BeTrue();
            graph.RemoveEdge(edge24).Should().BeTrue();
            graph.RemoveEdge(edge33).Should().BeTrue();
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertNoEdge(graph);
        }

        protected static void RemoveEdge_Throws_Test<TVertex, TEdge>(
            IMutableEdgeListGraph<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : class, IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => graph.RemoveEdge(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        protected static void RemoveEdge_Throws_Clusters_Test<TVertex, TEdge>(
            ClusteredAdjacencyGraph<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : class, IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => graph.RemoveEdge(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        protected static void RemoveEdgeIf_Test<TGraph>(TGraph graph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, Edge<int>>
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.VertexRemoved += v =>
            {
                ++verticesRemoved;
            };
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeRemoved += e =>
            {
                e.Should().NotBeNull();
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

            graph.RemoveEdgeIf(edge => edge.Target == 5).Should().Be(0);
            CheckCounters(0);

            graph.RemoveEdgeIf(edge => edge.Source == 3).Should().Be(2);
            CheckCounters(2);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge13, edge13Bis, edge14, edge24 });

            graph.RemoveEdgeIf(_ => true).Should().Be(5);
            CheckCounters(5);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertNoEdge(graph);

            #region Local function

            void CheckCounters(int expectedRemovedEdges)
            {
                verticesRemoved.Should().Be(0);
                edgesRemoved.Should().Be(expectedRemovedEdges);
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveEdgeIf_EdgesOnly_Test(
            EdgeListGraph<int, Edge<int>> graph)
        {
            int edgesRemoved = 0;

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeRemoved += e =>
            {
                e.Should().NotBeNull();
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

            graph.RemoveEdgeIf(edge => edge.Target == 5).Should().Be(0);
            CheckCounter(0);

            graph.RemoveEdgeIf(edge => edge.Source == 3).Should().Be(2);
            CheckCounter(2);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge13, edge13Bis, edge14, edge24 });

            graph.RemoveEdgeIf(_ => true).Should().Be(5);
            CheckCounter(5);
            AssertEmptyGraph(graph);    // Vertices removed in the same time as edges

            #region Local function

            void CheckCounter(int expectedRemovedEdges)
            {
                edgesRemoved.Should().Be(expectedRemovedEdges);
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveEdgeIf_Clusters_Test(
            ClusteredAdjacencyGraph<int, Edge<int>> graph)
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

            graph.RemoveEdgeIf(edge => edge.Target == 5).Should().Be(0);

            graph.RemoveEdgeIf(edge => edge.Source == 3).Should().Be(2);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge13, edge13Bis, edge14, edge24 });

            graph.RemoveEdgeIf(_ => true).Should().Be(5);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertNoEdge(graph);
        }

        protected static void RemoveEdgeIf_Throws_Test<TVertex, TEdge>(
            IMutableEdgeListGraph<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => graph.RemoveEdgeIf(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        protected static void RemoveEdgeIf_Throws_Clusters_Test<TVertex, TEdge>(
            ClusteredAdjacencyGraph<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => graph.RemoveEdgeIf(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        protected static void RemoveOutEdgeIf_Test(
            IMutableVertexAndEdgeListGraph<int, Edge<int>> graph)
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.VertexRemoved += v =>
            {
                ++verticesRemoved;
            };
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeRemoved += e =>
            {
                e.Should().NotBeNull();
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            graph.RemoveOutEdgeIf(1, _ => true).Should().Be(0);
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

            graph.RemoveOutEdgeIf(1, edge => edge.Target >= 3).Should().Be(3);
            CheckCounters(3);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge24, edge31, edge33 });

            graph.RemoveOutEdgeIf(3, edge => edge.Target > 5).Should().Be(0);
            CheckCounters(0);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge24, edge31, edge33 });

            graph.RemoveOutEdgeIf(3, _ => true).Should().Be(2);
            CheckCounters(2);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge24 });

            #region Local function

            void CheckCounters(int expectedRemovedEdges)
            {
                verticesRemoved.Should().Be(0);
                edgesRemoved.Should().Be(expectedRemovedEdges);
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveOutEdgeIf_ImmutableVertices_Test(
            BidirectionalMatrixGraph<Edge<int>> graph)
        {
            int edgesRemoved = 0;

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeRemoved += e =>
            {
                e.Should().NotBeNull();
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            graph.RemoveOutEdgeIf(6, _ => true).Should().Be(0);
            CheckCounter(0);
            AssertNoEdge(graph);

            var edge01 = new Edge<int>(0, 1);
            var edge02 = new Edge<int>(0, 2);
            var edge03 = new Edge<int>(0, 3);
            var edge13 = new Edge<int>(1, 3);
            var edge20 = new Edge<int>(2, 0);
            var edge22 = new Edge<int>(2, 2);
            graph.AddEdgeRange(new[] { edge01, edge02, edge03, edge13, edge20, edge22 });

            graph.RemoveOutEdgeIf(0, edge => edge.Target >= 2).Should().Be(2);
            CheckCounter(2);
            AssertHasEdges(graph, new[] { edge01, edge13, edge20, edge22 });

            graph.RemoveOutEdgeIf(2, edge => edge.Target > 4).Should().Be(0);
            CheckCounter(0);
            AssertHasEdges(graph, new[] { edge01, edge13, edge20, edge22 });

            graph.RemoveOutEdgeIf(2, _ => true).Should().Be(2);
            CheckCounter(2);
            AssertHasEdges(graph, new[] { edge01, edge13 });

            #region Local function

            void CheckCounter(int expectedRemovedEdges)
            {
                edgesRemoved.Should().Be(expectedRemovedEdges);
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveOutEdgeIf_Clusters_Test(
            ClusteredAdjacencyGraph<int, Edge<int>> graph)
        {
            graph.RemoveOutEdgeIf(1, _ => true).Should().Be(0);
            AssertEmptyGraph(graph);

            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge13Bis = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge13Bis, edge14, edge24, edge31, edge33 });

            graph.RemoveOutEdgeIf(1, edge => edge.Target >= 3).Should().Be(3);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge24, edge31, edge33 });

            graph.RemoveOutEdgeIf(3, edge => edge.Target > 5).Should().Be(0);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge24, edge31, edge33 });

            graph.RemoveOutEdgeIf(3, _ => true).Should().Be(2);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge24 });
        }

        protected static void RemoveOutEdgeIf_Throws_Test<TEdge>(
            BidirectionalMatrixGraph<TEdge> graph)
            where TEdge : class, IEdge<int>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => graph.RemoveOutEdgeIf(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        protected static void RemoveOutEdgeIf_Throws_Test<TVertex, TEdge>(
            IMutableIncidenceGraph<TVertex, TEdge> graph)
            where TVertex : notnull, new()
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8604
#pragma warning disable CS8625
            Invoking(() => graph.RemoveOutEdgeIf(default, _ => true)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.RemoveOutEdgeIf(new TVertex(), default)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.RemoveOutEdgeIf(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
#pragma warning restore CS8604
            // ReSharper restore AssignNullToNotNullAttribute
        }

        protected static void RemoveOutEdgeIf_Throws_Test<TVertex, TEdge>(
            ClusteredAdjacencyGraph<TVertex, TEdge> graph)
            where TVertex : notnull, new()
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8604
#pragma warning disable CS8625
            Invoking(() => graph.RemoveOutEdgeIf(default, _ => true)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.RemoveOutEdgeIf(new TVertex(), default)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.RemoveOutEdgeIf(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
#pragma warning restore CS8604
            // ReSharper restore AssignNullToNotNullAttribute
        }

        protected static void RemoveInEdgeIf_Test(
            IMutableBidirectionalGraph<int, Edge<int>> graph)
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.VertexRemoved += v =>
            {
                ++verticesRemoved;
            };
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeRemoved += e =>
            {
                e.Should().NotBeNull();
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            graph.RemoveInEdgeIf(1, _ => true).Should().Be(0);
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

            graph.RemoveInEdgeIf(3, edge => edge.Source == 1).Should().Be(2);
            CheckCounters(2);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge14, edge24, edge31, edge33 });

            graph.RemoveInEdgeIf(3, edge => edge.Target > 5).Should().Be(0);
            CheckCounters(0);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge14, edge24, edge31, edge33 });

            graph.RemoveInEdgeIf(2, _ => true).Should().Be(1);
            CheckCounters(1);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge14, edge24, edge31, edge33 });

            #region Local function

            void CheckCounters(int expectedRemovedEdges)
            {
                verticesRemoved.Should().Be(0);
                edgesRemoved.Should().Be(expectedRemovedEdges);
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveInEdgeIf_ImmutableVertices_Test(
            BidirectionalMatrixGraph<Edge<int>> graph)
        {
            int edgesRemoved = 0;

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeRemoved += e =>
            {
                e.Should().NotBeNull();
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            graph.RemoveInEdgeIf(6, _ => true).Should().Be(0);
            CheckCounter(0);
            AssertNoEdge(graph);

            var edge01 = new Edge<int>(0, 1);
            var edge02 = new Edge<int>(0, 2);
            var edge03 = new Edge<int>(0, 3);
            var edge13 = new Edge<int>(1, 3);
            var edge20 = new Edge<int>(2, 0);
            var edge22 = new Edge<int>(2, 2);
            graph.AddEdgeRange(new[] { edge01, edge02, edge03, edge13, edge20, edge22 });

            graph.RemoveInEdgeIf(2, edge => edge.Source == 0).Should().Be(1);
            CheckCounter(1);
            AssertHasEdges(graph, new[] { edge01, edge03, edge13, edge20, edge22 });

            graph.RemoveInEdgeIf(2, edge => edge.Target > 4).Should().Be(0);
            CheckCounter(0);
            AssertHasEdges(graph, new[] { edge01, edge03, edge13, edge20, edge22 });

            graph.RemoveInEdgeIf(1, _ => true).Should().Be(1);
            CheckCounter(1);
            AssertHasEdges(graph, new[] { edge03, edge13, edge20, edge22 });

            #region Local function

            void CheckCounter(int expectedRemovedEdges)
            {
                edgesRemoved.Should().Be(expectedRemovedEdges);
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveInEdgeIf_Throws_Test<TEdge>(
            BidirectionalMatrixGraph<TEdge> graph)
            where TEdge : class, IEdge<int>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => graph.RemoveInEdgeIf(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        protected static void RemoveInEdgeIf_Throws_Test(
            IMutableBidirectionalGraph<TestVertex, Edge<TestVertex>> graph)
        {
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => graph.RemoveInEdgeIf(default, _ => true)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.RemoveInEdgeIf(new TestVertex("v1"), default)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.RemoveInEdgeIf(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
        }

        protected static void RemoveAdjacentEdgeIf_Test(
            IMutableUndirectedGraph<int, Edge<int>> graph)
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.VertexRemoved += v =>
            {
                ++verticesRemoved;
            };
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeRemoved += e =>
            {
                e.Should().NotBeNull();
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            graph.RemoveAdjacentEdgeIf(1, _ => true).Should().Be(0);
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

            graph.RemoveAdjacentEdgeIf(1, edge => edge.Source >= 3 || edge.Target >= 3).Should().Be(4);
            CheckCounters(4);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge24, edge33 });

            graph.RemoveAdjacentEdgeIf(3, edge => edge.Target > 5).Should().Be(0);
            CheckCounters(0);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge24, edge33 });

            graph.RemoveAdjacentEdgeIf(3, _ => true).Should().Be(1);
            CheckCounters(1);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge24 });

            #region Local function

            void CheckCounters(int expectedRemovedEdges)
            {
                verticesRemoved.Should().Be(0);
                edgesRemoved.Should().Be(expectedRemovedEdges);
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveAdjacentEdgeIf_Throws_Test(
            IMutableUndirectedGraph<TestVertex, Edge<TestVertex>> graph)
        {
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => graph.RemoveAdjacentEdgeIf(default, _ => true)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.RemoveAdjacentEdgeIf(new TestVertex("v1"), default)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.RemoveAdjacentEdgeIf(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
        }

        #endregion
    }
}
