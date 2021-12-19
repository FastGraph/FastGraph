#nullable enable

using static FastGraph.Tests.GraphTestHelpers;

namespace FastGraph.Tests.Structures
{
    internal partial class GraphTestsBase
    {
        #region Remove Vertices

        protected static void RemoveVertex_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> graph)
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.VertexRemoved += v =>
            {
                // ReSharper disable once AccessToModifiedClosure
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
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge14, edge24, edge31, edge33 });

            graph.RemoveVertex(5).Should().BeFalse();
            CheckCounters(0, 0);

            graph.RemoveVertex(3).Should().BeTrue();
            CheckCounters(1, 3);
            AssertHasVertices(graph, new[] { 1, 2, 4 });
            AssertHasEdges(graph, new[] { edge12, edge14, edge24 });

            graph.RemoveVertex(1).Should().BeTrue();
            CheckCounters(1, 2);
            AssertHasVertices(graph, new[] { 2, 4 });
            AssertHasEdges(graph, new[] { edge24 });

            graph.RemoveVertex(2).Should().BeTrue();
            CheckCounters(1, 1);
            AssertHasVertices(graph, new[] { 4 });
            AssertNoEdge(graph);

            graph.RemoveVertex(4).Should().BeTrue();
            CheckCounters(1, 0);
            AssertEmptyGraph(graph);

            #region Local function

            void CheckCounters(int expectedRemovedVertices, int expectedRemovedEdges)
            {
                verticesRemoved.Should().Be(expectedRemovedVertices);
                edgesRemoved.Should().Be(expectedRemovedEdges);
                verticesRemoved = 0;
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveVertex_Clusters_Test(
            ClusteredAdjacencyGraph<int, Edge<int>> graph)
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge14, edge24, edge31, edge33 });

            graph.RemoveVertex(5).Should().BeFalse();

            graph.RemoveVertex(3).Should().BeTrue();
            AssertHasVertices(graph, new[] { 1, 2, 4 });
            AssertHasEdges(graph, new[] { edge12, edge14, edge24 });

            graph.RemoveVertex(1).Should().BeTrue();
            AssertHasVertices(graph, new[] { 2, 4 });
            AssertHasEdges(graph, new[] { edge24 });

            graph.RemoveVertex(2).Should().BeTrue();
            AssertHasVertices(graph, new[] { 4 });
            AssertNoEdge(graph);

            graph.RemoveVertex(4).Should().BeTrue();
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
            IMutableVertexSet<TVertex> graph)
            where TVertex : notnull
        {
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8604
            Invoking(() => graph.RemoveVertex(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8604
        }

        protected static void RemoveVertex_Throws_Clusters_Test<TVertex, TEdge>(
            ClusteredAdjacencyGraph<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8604
            Invoking(() => graph.RemoveVertex(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8604
        }

        protected static void RemoveVertexIf_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> graph)
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.VertexRemoved += v =>
            {
                // ReSharper disable once AccessToModifiedClosure
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
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge14, edge24, edge31, edge33 });

            graph.RemoveVertexIf(vertex => vertex > 10).Should().Be(0);
            CheckCounters(0, 0);

            graph.RemoveVertexIf(vertex => vertex > 2).Should().Be(2);
            CheckCounters(2, 5);
            AssertHasVertices(graph, new[] { 1, 2 });
            AssertHasEdges(graph, new[] { edge12 });

            graph.RemoveVertexIf(_ => true).Should().Be(2);
            CheckCounters(2, 1);
            AssertEmptyGraph(graph);

            #region Local function

            void CheckCounters(int expectedRemovedVertices, int expectedRemovedEdges)
            {
                verticesRemoved.Should().Be(expectedRemovedVertices);
                edgesRemoved.Should().Be(expectedRemovedEdges);
                verticesRemoved = 0;
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveVertexIf_Test2(
            IMutableVertexAndEdgeSet<int, Edge<int>> graph)
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.VertexRemoved += v =>
            {
                // ReSharper disable once AccessToModifiedClosure
                ++verticesRemoved;
            };
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeRemoved += e =>
            {
                e.Should().NotBeNull();
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            var edge11 = new Edge<int>(1, 1);
            var edge13 = new Edge<int>(1, 3);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge32 = new Edge<int>(3, 2);
            var edge34 = new Edge<int>(3, 4);
            graph.AddVerticesAndEdgeRange(new[] { edge11, edge13, edge24, edge31, edge32, edge34 });

            graph.RemoveVertexIf(vertex => vertex == 1 || vertex == 3).Should().Be(2);
            CheckCounters(2, 5);
            AssertHasVertices(graph, new[] { 2, 4 });
            AssertHasEdges(graph, new[] { edge24 });

            #region Local function

            void CheckCounters(int expectedRemovedVertices, int expectedRemovedEdges)
            {
                verticesRemoved.Should().Be(expectedRemovedVertices);
                edgesRemoved.Should().Be(expectedRemovedEdges);
                verticesRemoved = 0;
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveVertexIf_Clusters_Test(
            ClusteredAdjacencyGraph<int, Edge<int>> graph)
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge14, edge24, edge31, edge33 });

            graph.RemoveVertexIf(vertex => vertex > 10).Should().Be(0);

            graph.RemoveVertexIf(vertex => vertex > 2).Should().Be(2);
            AssertHasVertices(graph, new[] { 1, 2 });
            AssertHasEdges(graph, new[] { edge12 });

            graph.RemoveVertexIf(_ => true).Should().Be(2);
            AssertEmptyGraph(graph);
        }

        protected static void RemoveVertexIf_Clusters_Test2(
            ClusteredAdjacencyGraph<int, Edge<int>> graph)
        {
            var edge11 = new Edge<int>(1, 1);
            var edge13 = new Edge<int>(1, 3);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge32 = new Edge<int>(3, 2);
            var edge34 = new Edge<int>(3, 4);
            graph.AddVerticesAndEdgeRange(new[] { edge11, edge13, edge24, edge31, edge32, edge34 });

            graph.RemoveVertexIf(vertex => vertex == 1 || vertex == 3).Should().Be(2);
            AssertHasVertices(graph, new[] { 2, 4 });
            AssertHasEdges(graph, new[] { edge24 });
        }

        protected static void RemoveVertexIf_Throws_Test<TVertex>(
            IMutableVertexSet<TVertex> graph)
            where TVertex : notnull
        {
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => graph.RemoveVertexIf(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        protected static void RemoveVertexIf_Throws_Clusters_Test<TVertex, TEdge>(
            ClusteredAdjacencyGraph<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => graph.RemoveVertexIf(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        #endregion
    }
}
