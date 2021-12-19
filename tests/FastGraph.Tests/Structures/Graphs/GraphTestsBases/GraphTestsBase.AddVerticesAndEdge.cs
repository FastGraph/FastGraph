#nullable enable

using static FastGraph.Tests.GraphTestHelpers;

namespace FastGraph.Tests.Structures
{
    internal partial class GraphTestsBase
    {
        #region Add Vertices & Edges

        protected static void AddVerticesAndEdge_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> graph)
        {
            int vertexAdded = 0;
            int edgeAdded = 0;

            AssertEmptyGraph(graph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.VertexAdded += v =>
            {
                ++vertexAdded;
            };
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeAdded += e =>
            {
                e.Should().NotBeNull();
                ++edgeAdded;
            };

            // Edge 1
            var edge1 = new Edge<int>(1, 2);
            graph.AddVerticesAndEdge(edge1).Should().BeTrue();
            vertexAdded.Should().Be(2);
            edgeAdded.Should().Be(1);
            AssertHasVertices(graph, new[] { 1, 2 });
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 2
            var edge2 = new Edge<int>(1, 3);
            graph.AddVerticesAndEdge(edge2).Should().BeTrue();
            vertexAdded.Should().Be(3);
            edgeAdded.Should().Be(2);
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge1, edge2 });

            // Edge 3
            var edge3 = new Edge<int>(2, 3);
            graph.AddVerticesAndEdge(edge3).Should().BeTrue();
            vertexAdded.Should().Be(3);
            edgeAdded.Should().Be(3);
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge1, edge2, edge3 });
        }

        protected static void AddVerticesAndEdge_Clusters_Test(
            ClusteredAdjacencyGraph<int, Edge<int>> graph1,
            ClusteredAdjacencyGraph<int, Edge<int>> parent2,
            ClusteredAdjacencyGraph<int, Edge<int>> graph2)
        {
            // Graph without parent
            AssertEmptyGraph(graph1);

            // Edge 1
            var edge1 = new Edge<int>(1, 2);
            graph1.AddVerticesAndEdge(edge1).Should().BeTrue();
            AssertHasVertices(graph1, new[] { 1, 2 });
            AssertHasEdges(graph1, new[] { edge1 });

            // Edge 2
            var edge2 = new Edge<int>(1, 3);
            graph1.AddVerticesAndEdge(edge2).Should().BeTrue();
            AssertHasVertices(graph1, new[] { 1, 2, 3 });
            AssertHasEdges(graph1, new[] { edge1, edge2 });

            // Edge 3
            var edge3 = new Edge<int>(2, 3);
            graph1.AddVerticesAndEdge(edge3).Should().BeTrue();
            AssertHasVertices(graph1, new[] { 1, 2, 3 });
            AssertHasEdges(graph1, new[] { edge1, edge2, edge3 });


            // Graph with parent
            AssertEmptyGraph(parent2);
            AssertEmptyGraph(graph2);

            // Edge 1
            graph2.AddVerticesAndEdge(edge1).Should().BeTrue();
            AssertHasVertices(parent2, new[] { 1, 2 });
            AssertHasVertices(graph2, new[] { 1, 2 });
            AssertHasEdges(parent2, new[] { edge1 });
            AssertHasEdges(graph2, new[] { edge1 });

            // Edge 2
            parent2.AddVerticesAndEdge(edge2).Should().BeTrue();
            AssertHasVertices(parent2, new[] { 1, 2, 3 });
            AssertHasVertices(graph2, new[] { 1, 2 });
            AssertHasEdges(parent2, new[] { edge1, edge2 });
            AssertHasEdges(graph2, new[] { edge1 });

            graph2.AddVerticesAndEdge(edge2).Should().BeTrue();
            AssertHasVertices(parent2, new[] { 1, 2, 3 });
            AssertHasVertices(graph2, new[] { 1, 2, 3 });
            AssertHasEdges(parent2, new[] { edge1, edge2 });
            AssertHasEdges(graph2, new[] { edge1, edge2 });

            // Edge 3
            graph2.AddVerticesAndEdge(edge3).Should().BeTrue();
            AssertHasVertices(parent2, new[] { 1, 2, 3 });
            AssertHasVertices(graph2, new[] { 1, 2, 3 });
            AssertHasEdges(parent2, new[] { edge1, edge2, edge3 });
            AssertHasEdges(graph2, new[] { edge1, edge2, edge3 });
        }

        protected static void AddVerticesAndEdge_Throws_Test<TVertex, TEdge>(
            IMutableVertexAndEdgeSet<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : class, IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => graph.AddVerticesAndEdge(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            AssertEmptyGraph(graph);
        }

        protected static void AddVerticesAndEdge_Throws_EdgesOnly_Test<TVertex, TEdge>(
            EdgeListGraph<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : class, IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => graph.AddVerticesAndEdge(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            AssertEmptyGraph(graph);
        }

        protected static void AddVerticesAndEdge_Throws_Clusters_Test<TVertex, TEdge>(
            ClusteredAdjacencyGraph<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : class, IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => graph.AddVerticesAndEdge(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            AssertEmptyGraph(graph);
        }

        protected static void AddVerticesAndEdgeRange_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> graph)
        {
            int vertexAdded = 0;
            int edgeAdded = 0;

            AssertEmptyGraph(graph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.VertexAdded += v =>
            {
                ++vertexAdded;
            };
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeAdded += e =>
            {
                e.Should().NotBeNull();
                ++edgeAdded;
            };

            // Edge 1, 2
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            graph.AddVerticesAndEdgeRange(new[] { edge1, edge2 }).Should().Be(2);
            vertexAdded.Should().Be(3);
            edgeAdded.Should().Be(2);
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge1, edge2 });

            // Edge 1, 3
            var edge3 = new Edge<int>(2, 3);
            graph.AddVerticesAndEdgeRange(new[] { edge1, edge3 }).Should().Be(1); // Showcase the add of only one edge
            vertexAdded.Should().Be(3);
            edgeAdded.Should().Be(3);
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge1, edge2, edge3 });
        }

        protected static void AddVerticesAndEdgeRange_EdgesOnly_Test(
            EdgeListGraph<int, Edge<int>> graph)
        {
            int edgeAdded = 0;

            AssertEmptyGraph(graph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeAdded += e =>
            {
                e.Should().NotBeNull();
                ++edgeAdded;
            };

            // Edge 1, 2
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            graph.AddVerticesAndEdgeRange(new[] { edge1, edge2 }).Should().Be(2);
            edgeAdded.Should().Be(2);
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge1, edge2 });

            // Edge 1, 3
            var edge3 = new Edge<int>(2, 3);
            graph.AddVerticesAndEdgeRange(new[] { edge1, edge3 }).Should().Be(1); // Showcase the add of only one edge
            edgeAdded.Should().Be(3);
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge1, edge2, edge3 });
        }

        protected static void AddVerticesAndEdgeRange_Clusters_Test(
            ClusteredAdjacencyGraph<int, Edge<int>> graph1,
            ClusteredAdjacencyGraph<int, Edge<int>> parent2,
            ClusteredAdjacencyGraph<int, Edge<int>> graph2)
        {
            // Graph without parent
            AssertEmptyGraph(graph1);

            // Edge 1, 2
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            graph1.AddVerticesAndEdgeRange(new[] { edge1, edge2 }).Should().Be(2);
            AssertHasVertices(graph1, new[] { 1, 2, 3 });
            AssertHasEdges(graph1, new[] { edge1, edge2 });

            // Edge 1, 3
            var edge3 = new Edge<int>(2, 3);
            graph1.AddVerticesAndEdgeRange(new[] { edge1, edge3 }).Should().Be(1); // Showcase the add of only one edge
            AssertHasVertices(graph1, new[] { 1, 2, 3 });
            AssertHasEdges(graph1, new[] { edge1, edge2, edge3 });


            // Graph with parent
            AssertEmptyGraph(parent2);
            AssertEmptyGraph(graph2);

            // Edge 1, 2
            graph2.AddVerticesAndEdgeRange(new[] { edge1, edge2 }).Should().Be(2);
            AssertHasVertices(parent2, new[] { 1, 2, 3 });
            AssertHasVertices(graph2, new[] { 1, 2, 3 });
            AssertHasEdges(parent2, new[] { edge1, edge2 });
            AssertHasEdges(graph2, new[] { edge1, edge2 });

            // Edge 1, 3
            parent2.AddVerticesAndEdgeRange(new[] { edge1, edge3 }).Should().Be(1); // Showcase the add of only one edge
            AssertHasVertices(parent2, new[] { 1, 2, 3 });
            AssertHasVertices(graph2, new[] { 1, 2, 3 });
            AssertHasEdges(parent2, new[] { edge1, edge2, edge3 });
            AssertHasEdges(graph2, new[] { edge1, edge2 });

            graph2.AddVerticesAndEdgeRange(new[] { edge1, edge3 }).Should().Be(1); // Showcase the add of only one edge
            AssertHasVertices(parent2, new[] { 1, 2, 3 });
            AssertHasVertices(graph2, new[] { 1, 2, 3 });
            AssertHasEdges(parent2, new[] { edge1, edge2, edge3 });
            AssertHasEdges(graph2, new[] { edge1, edge2, edge3 });
        }

        protected static void AddVerticesAndEdgeRange_Throws_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> graph)
        {
            int vertexAdded = 0;
            int edgeAdded = 0;

            AssertEmptyGraph(graph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.VertexAdded += v =>
            {
                ++vertexAdded;
            };
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeAdded += e =>
            {
                e.Should().NotBeNull();
                ++edgeAdded;
            };

            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => graph.AddVerticesAndEdgeRange(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625

            // Edge 1, 2, 3
            var edge1 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
#pragma warning disable CS8620
            Invoking(() => graph.AddVerticesAndEdgeRange(new[] { edge1, default, edge3 })).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8620
            vertexAdded.Should().Be(0);
            edgeAdded.Should().Be(0);
            AssertEmptyGraph(graph);
        }

        protected static void AddVerticesAndEdgeRange_Throws_EdgesOnly_Test(
            EdgeListGraph<int, Edge<int>> graph)
        {
            int edgeAdded = 0;

            AssertEmptyGraph(graph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeAdded += e =>
            {
                e.Should().NotBeNull();
                ++edgeAdded;
            };

            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => graph.AddVerticesAndEdgeRange(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625

            // Edge 1, 2, 3
            var edge1 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
#pragma warning disable CS8620
            Invoking(() => graph.AddVerticesAndEdgeRange(new[] { edge1, default, edge3 })).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8620
            edgeAdded.Should().Be(0);
            AssertEmptyGraph(graph);
        }

        protected static void AddVerticesAndEdgeRange_Throws_Clusters_Test(
            ClusteredAdjacencyGraph<int, Edge<int>> graph)
        {
            AssertEmptyGraph(graph);

            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => graph.AddVerticesAndEdgeRange(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625

            // Edge 1, 2, 3
            var edge1 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
#pragma warning disable CS8620
            Invoking(() => graph.AddVerticesAndEdgeRange(new[] { edge1, default, edge3 })).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8620
            AssertEmptyGraph(graph);
        }

        #endregion
    }
}
