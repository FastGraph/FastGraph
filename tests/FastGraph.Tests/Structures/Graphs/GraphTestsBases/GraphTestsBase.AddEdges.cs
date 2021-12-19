#nullable enable

using JetBrains.Annotations;
using static FastGraph.Tests.GraphTestHelpers;

namespace FastGraph.Tests.Structures
{
    internal partial class GraphTestsBase
    {
        #region Add Edges

        protected static void AddEdge_ParallelEdges_Test<TGraph>(TGraph graph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, Edge<int>>
        {
            int edgeAdded = 0;

            graph.AddVertex(1);
            graph.AddVertex(2);

            AssertNoEdge(graph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeAdded += e =>
            {
                e.Should().NotBeNull();
                ++edgeAdded;
            };

            // Edge 1
            var edge1 = new Edge<int>(1, 2);
            graph.AddEdge(edge1).Should().BeTrue();
            edgeAdded.Should().Be(1);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 2
            var edge2 = new Edge<int>(1, 2);
            graph.AddEdge(edge2).Should().BeTrue();
            edgeAdded.Should().Be(2);
            AssertHasEdges(graph, new[] { edge1, edge2 });

            // Edge 3
            var edge3 = new Edge<int>(2, 1);
            graph.AddEdge(edge3).Should().BeTrue();
            edgeAdded.Should().Be(3);
            AssertHasEdges(graph, new[] { edge1, edge2, edge3 });

            // Edge 1 bis
            graph.AddEdge(edge1).Should().BeTrue();
            edgeAdded.Should().Be(4);
            AssertHasEdges(graph, new[] { edge1, edge2, edge3, edge1 });

            // Edge 4 self edge
            var edge4 = new Edge<int>(2, 2);
            graph.AddEdge(edge4).Should().BeTrue();
            edgeAdded.Should().Be(5);
            AssertHasEdges(graph, new[] { edge1, edge2, edge3, edge1, edge4 });
        }

        protected static void AddEdge_ParallelEdges_Clusters_Test(
            ClusteredAdjacencyGraph<int, Edge<int>> graph1,
            ClusteredAdjacencyGraph<int, Edge<int>> parent2,
            ClusteredAdjacencyGraph<int, Edge<int>> graph2)
        {
            // Graph without parent
            graph1.AddVertex(1);
            graph1.AddVertex(2);

            AssertNoEdge(graph1);

            // Edge 1
            var edge1 = new Edge<int>(1, 2);
            graph1.AddEdge(edge1).Should().BeTrue();
            AssertHasEdges(graph1, new[] { edge1 });

            // Edge 2
            var edge2 = new Edge<int>(1, 2);
            graph1.AddEdge(edge2).Should().BeTrue();
            AssertHasEdges(graph1, new[] { edge1, edge2 });

            // Edge 3
            var edge3 = new Edge<int>(2, 1);
            graph1.AddEdge(edge3).Should().BeTrue();
            AssertHasEdges(graph1, new[] { edge1, edge2, edge3 });

            // Edge 1 bis
            graph1.AddEdge(edge1).Should().BeTrue();
            AssertHasEdges(graph1, new[] { edge1, edge2, edge3, edge1 });

            // Edge 4 self edge
            var edge4 = new Edge<int>(2, 2);
            graph1.AddEdge(edge4).Should().BeTrue();
            AssertHasEdges(graph1, new[] { edge1, edge2, edge3, edge1, edge4 });


            // Graph with parent
            graph2.AddVertex(1);
            graph2.AddVertex(2);

            AssertNoEdge(parent2);
            AssertNoEdge(graph2);

            // Edge 1
            graph2.AddEdge(edge1).Should().BeTrue();
            AssertHasEdges(parent2, new[] { edge1 });
            AssertHasEdges(graph2, new[] { edge1 });

            // Edge 2
            parent2.AddEdge(edge2).Should().BeTrue();
            AssertHasEdges(parent2, new[] { edge1, edge2 });
            AssertHasEdges(graph2, new[] { edge1 });

            graph2.AddEdge(edge2).Should().BeTrue();
            AssertHasEdges(parent2, new[] { edge1, edge2 });
            AssertHasEdges(graph2, new[] { edge1, edge2 });

            // Edge 3
            graph2.AddEdge(edge3).Should().BeTrue();
            AssertHasEdges(parent2, new[] { edge1, edge2, edge3 });
            AssertHasEdges(graph2, new[] { edge1, edge2, edge3 });

            // Edge 1 bis
            graph2.AddEdge(edge1).Should().BeTrue();
            AssertHasEdges(parent2, new[] { edge1, edge2, edge3 });
            AssertHasEdges(graph2, new[] { edge1, edge2, edge3, edge1 });

            // Edge 4 self edge
            graph2.AddEdge(edge4).Should().BeTrue();
            AssertHasEdges(parent2, new[] { edge1, edge2, edge3, edge4 });
            AssertHasEdges(graph2, new[] { edge1, edge2, edge3, edge1, edge4 });
        }

        protected static void AddEdge_ParallelEdges_EquatableEdge_Test<TGraph>(TGraph graph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, EquatableEdge<int>>
        {
            int edgeAdded = 0;

            graph.AddVertex(1);
            graph.AddVertex(2);

            AssertNoEdge(graph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeAdded += e =>
            {
                e.Should().NotBeNull();
                ++edgeAdded;
            };

            // Edge 1
            var edge1 = new EquatableEdge<int>(1, 2);
            graph.AddEdge(edge1).Should().BeTrue();
            edgeAdded.Should().Be(1);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 2
            var edge2 = new EquatableEdge<int>(1, 2);
            graph.AddEdge(edge2).Should().BeTrue();
            edgeAdded.Should().Be(2);
            AssertHasEdges(graph, new[] { edge1, edge2 });

            // Edge 3
            var edge3 = new EquatableEdge<int>(2, 1);
            graph.AddEdge(edge3).Should().BeTrue();
            edgeAdded.Should().Be(3);
            AssertHasEdges(graph, new[] { edge1, edge2, edge3 });

            // Edge 1 bis
            graph.AddEdge(edge1).Should().BeTrue();
            edgeAdded.Should().Be(4);
            AssertHasEdges(graph, new[] { edge1, edge2, edge3, edge1 });

            // Edge 4 self edge
            var edge4 = new EquatableEdge<int>(2, 2);
            graph.AddEdge(edge4).Should().BeTrue();
            edgeAdded.Should().Be(5);
            AssertHasEdges(graph, new[] { edge1, edge2, edge3, edge1, edge4 });
        }

        protected static void AddEdge_ParallelEdges_EquatableEdge_Clusters_Test(
            ClusteredAdjacencyGraph<int, EquatableEdge<int>> graph1,
            ClusteredAdjacencyGraph<int, EquatableEdge<int>> parent2,
            ClusteredAdjacencyGraph<int, EquatableEdge<int>> graph2)
        {
            // Graph without parent
            graph1.AddVertex(1);
            graph1.AddVertex(2);

            AssertNoEdge(graph1);

            // Edge 1
            var edge1 = new EquatableEdge<int>(1, 2);
            graph1.AddEdge(edge1).Should().BeTrue();
            AssertHasEdges(graph1, new[] { edge1 });

            // Edge 2
            var edge2 = new EquatableEdge<int>(1, 2);
            graph1.AddEdge(edge2).Should().BeTrue();
            AssertHasEdges(graph1, new[] { edge1, edge2 });

            // Edge 3
            var edge3 = new EquatableEdge<int>(2, 1);
            graph1.AddEdge(edge3).Should().BeTrue();
            AssertHasEdges(graph1, new[] { edge1, edge2, edge3 });

            // Edge 1 bis
            graph1.AddEdge(edge1).Should().BeTrue();
            AssertHasEdges(graph1, new[] { edge1, edge2, edge3, edge1 });

            // Edge 4 self edge
            var edge4 = new EquatableEdge<int>(2, 2);
            graph1.AddEdge(edge4).Should().BeTrue();
            AssertHasEdges(graph1, new[] { edge1, edge2, edge3, edge1, edge4 });


            // Graph with parent
            graph2.AddVertex(1);
            graph2.AddVertex(2);

            AssertNoEdge(parent2);
            AssertNoEdge(graph2);

            // Edge 1
            graph2.AddEdge(edge1).Should().BeTrue();
            AssertHasEdges(parent2, new[] { edge1 });
            AssertHasEdges(graph2, new[] { edge1 });

            // Edge 2
            parent2.AddEdge(edge2).Should().BeTrue();
            AssertHasEdges(parent2, new[] { edge1, edge2 });
            AssertHasEdges(graph2, new[] { edge1 });

            graph2.AddEdge(edge2).Should().BeTrue();
            AssertHasEdges(parent2, new[] { edge1, edge2 });
            AssertHasEdges(graph2, new[] { edge1, edge2 });

            // Edge 3
            graph2.AddEdge(edge3).Should().BeTrue();
            AssertHasEdges(parent2, new[] { edge1, edge2, edge3 });
            AssertHasEdges(graph2, new[] { edge1, edge2, edge3 });

            // Edge 1 bis
            graph2.AddEdge(edge1).Should().BeTrue();
            AssertHasEdges(parent2, new[] { edge1, edge2, edge3 });
            AssertHasEdges(graph2, new[] { edge1, edge2, edge3, edge1 });

            // Edge 4 self edge
            graph2.AddEdge(edge4).Should().BeTrue();
            AssertHasEdges(parent2, new[] { edge1, edge2, edge3, edge4 });
            AssertHasEdges(graph2, new[] { edge1, edge2, edge3, edge1, edge4 });
        }

        protected static void AddEdge_NoParallelEdges_Test<TGraph>(TGraph graph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, Edge<int>>
        {
            int edgeAdded = 0;

            graph.AddVertex(1);
            graph.AddVertex(2);

            AssertNoEdge(graph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeAdded += e =>
            {
                e.Should().NotBeNull();
                ++edgeAdded;
            };

            // Edge 1
            var edge1 = new Edge<int>(1, 2);
            graph.AddEdge(edge1).Should().BeTrue();
            edgeAdded.Should().Be(1);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 2
            var edge2 = new Edge<int>(1, 2);
            graph.AddEdge(edge2).Should().BeFalse();
            edgeAdded.Should().Be(1);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 3
            var edge3 = new Edge<int>(2, 1);
            graph.AddEdge(edge3).Should().BeTrue();
            edgeAdded.Should().Be(2);
            AssertHasEdges(graph, new[] { edge1, edge3 });

            // Edge 1 bis
            graph.AddEdge(edge1).Should().BeFalse();
            edgeAdded.Should().Be(2);
            AssertHasEdges(graph, new[] { edge1, edge3 });

            // Edge 4 self edge
            var edge4 = new Edge<int>(2, 2);
            graph.AddEdge(edge4).Should().BeTrue();
            edgeAdded.Should().Be(3);
            AssertHasEdges(graph, new[] { edge1, edge3, edge4 });
        }

        protected static void AddEdge_NoParallelEdges_UndirectedGraph_Test<TGraph>(TGraph graph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, Edge<int>>
        {
            int edgeAdded = 0;

            graph.AddVertex(1);
            graph.AddVertex(2);

            AssertNoEdge(graph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeAdded += e =>
            {
                e.Should().NotBeNull();
                ++edgeAdded;
            };

            // Edge 1
            var edge1 = new Edge<int>(1, 2);
            graph.AddEdge(edge1).Should().BeTrue();
            edgeAdded.Should().Be(1);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 2
            var edge2 = new Edge<int>(1, 2);
            graph.AddEdge(edge2).Should().BeFalse();
            edgeAdded.Should().Be(1);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 3
            var edge3 = new Edge<int>(2, 1);
            graph.AddEdge(edge3).Should().BeFalse();   // Parallel to edge 1
            edgeAdded.Should().Be(1);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 1 bis
            graph.AddEdge(edge1).Should().BeFalse();
            edgeAdded.Should().Be(1);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 4 self edge
            var edge4 = new Edge<int>(2, 2);
            graph.AddEdge(edge4).Should().BeTrue();
            edgeAdded.Should().Be(2);
            AssertHasEdges(graph, new[] { edge1, edge4 });
        }

        protected static void AddEdge_NoParallelEdges_Clusters_Test(
            ClusteredAdjacencyGraph<int, Edge<int>> graph1,
            ClusteredAdjacencyGraph<int, Edge<int>> parent2,
            ClusteredAdjacencyGraph<int, Edge<int>> graph2)
        {
            // Graph without parent
            graph1.AddVertex(1);
            graph1.AddVertex(2);

            AssertNoEdge(graph1);

            // Edge 1
            var edge1 = new Edge<int>(1, 2);
            graph1.AddEdge(edge1).Should().BeTrue();
            AssertHasEdges(graph1, new[] { edge1 });

            // Edge 2
            var edge2 = new Edge<int>(1, 2);
            graph1.AddEdge(edge2).Should().BeFalse();
            AssertHasEdges(graph1, new[] { edge1 });

            // Edge 3
            var edge3 = new Edge<int>(2, 1);
            graph1.AddEdge(edge3).Should().BeTrue();
            AssertHasEdges(graph1, new[] { edge1, edge3 });

            // Edge 1 bis
            graph1.AddEdge(edge1).Should().BeFalse();
            AssertHasEdges(graph1, new[] { edge1, edge3 });

            // Edge 4 self edge
            var edge4 = new Edge<int>(2, 2);
            graph1.AddEdge(edge4).Should().BeTrue();
            AssertHasEdges(graph1, new[] { edge1, edge3, edge4 });


            // Graph with parent
            graph2.AddVertex(1);
            graph2.AddVertex(2);

            AssertNoEdge(parent2);
            AssertNoEdge(graph2);

            // Edge 1
            graph2.AddEdge(edge1).Should().BeTrue();
            AssertHasEdges(parent2, new[] { edge1 });
            AssertHasEdges(graph2, new[] { edge1 });

            // Edge 2
            graph2.AddEdge(edge2).Should().BeFalse();
            AssertHasEdges(parent2, new[] { edge1 });
            AssertHasEdges(graph2, new[] { edge1 });

            // Edge 3
            parent2.AddEdge(edge3).Should().BeTrue();
            AssertHasEdges(parent2, new[] { edge1, edge3 });
            AssertHasEdges(graph2, new[] { edge1 });

            graph2.AddEdge(edge3).Should().BeTrue();
            AssertHasEdges(parent2, new[] { edge1, edge3 });
            AssertHasEdges(graph2, new[] { edge1, edge3 });

            // Edge 1 bis
            graph2.AddEdge(edge1).Should().BeFalse();
            AssertHasEdges(parent2, new[] { edge1, edge3 });
            AssertHasEdges(graph2, new[] { edge1, edge3 });

            // Edge 4 self edge
            graph2.AddEdge(edge4).Should().BeTrue();
            AssertHasEdges(parent2, new[] { edge1, edge3, edge4 });
            AssertHasEdges(graph2, new[] { edge1, edge3, edge4 });
        }

        protected static void AddEdge_NoParallelEdges_EquatableEdge_Test<TGraph>(TGraph graph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, EquatableEdge<int>>
        {
            int edgeAdded = 0;

            graph.AddVertex(1);
            graph.AddVertex(2);

            AssertNoEdge(graph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeAdded += e =>
            {
                e.Should().NotBeNull();
                ++edgeAdded;
            };

            // Edge 1
            var edge1 = new EquatableEdge<int>(1, 2);
            graph.AddEdge(edge1).Should().BeTrue();
            edgeAdded.Should().Be(1);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 2
            var edge2 = new EquatableEdge<int>(1, 2);
            graph.AddEdge(edge2).Should().BeFalse();
            edgeAdded.Should().Be(1);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 3
            var edge3 = new EquatableEdge<int>(2, 1);
            graph.AddEdge(edge3).Should().BeTrue();
            edgeAdded.Should().Be(2);
            AssertHasEdges(graph, new[] { edge1, edge3 });

            // Edge 1 bis
            graph.AddEdge(edge1).Should().BeFalse();
            edgeAdded.Should().Be(2);
            AssertHasEdges(graph, new[] { edge1, edge3 });

            // Edge 4 self edge
            var edge4 = new EquatableEdge<int>(2, 2);
            graph.AddEdge(edge4).Should().BeTrue();
            edgeAdded.Should().Be(3);
            AssertHasEdges(graph, new[] { edge1, edge3, edge4 });
        }

        protected static void AddEdge_NoParallelEdges_EquatableEdge_UndirectedGraph_Test<TGraph>(TGraph graph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, EquatableEdge<int>>
        {
            int edgeAdded = 0;

            graph.AddVertex(1);
            graph.AddVertex(2);

            AssertNoEdge(graph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeAdded += e =>
            {
                e.Should().NotBeNull();
                ++edgeAdded;
            };

            // Edge 1
            var edge1 = new EquatableEdge<int>(1, 2);
            graph.AddEdge(edge1).Should().BeTrue();
            edgeAdded.Should().Be(1);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 2
            var edge2 = new EquatableEdge<int>(1, 2);
            graph.AddEdge(edge2).Should().BeFalse();
            edgeAdded.Should().Be(1);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 3
            var edge3 = new EquatableEdge<int>(2, 1);
            graph.AddEdge(edge3).Should().BeFalse();   // Parallel to edge 1
            edgeAdded.Should().Be(1);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 1 bis
            graph.AddEdge(edge1).Should().BeFalse();
            edgeAdded.Should().Be(1);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 4 self edge
            var edge4 = new EquatableEdge<int>(2, 2);
            graph.AddEdge(edge4).Should().BeTrue();
            edgeAdded.Should().Be(2);
            AssertHasEdges(graph, new[] { edge1, edge4 });
        }

        protected static void AddEdge_NoParallelEdges_EquatableEdge_Clusters_Test(
            ClusteredAdjacencyGraph<int, EquatableEdge<int>> graph1,
            ClusteredAdjacencyGraph<int, EquatableEdge<int>> parent2,
            ClusteredAdjacencyGraph<int, EquatableEdge<int>> graph2)
        {
            // Graph without parent
            graph1.AddVertex(1);
            graph1.AddVertex(2);

            AssertNoEdge(graph1);

            // Edge 1
            var edge1 = new EquatableEdge<int>(1, 2);
            graph1.AddEdge(edge1).Should().BeTrue();
            AssertHasEdges(graph1, new[] { edge1 });

            // Edge 2
            var edge2 = new EquatableEdge<int>(1, 2);
            graph1.AddEdge(edge2).Should().BeFalse();
            AssertHasEdges(graph1, new[] { edge1 });

            // Edge 3
            var edge3 = new EquatableEdge<int>(2, 1);
            graph1.AddEdge(edge3).Should().BeTrue();
            AssertHasEdges(graph1, new[] { edge1, edge3 });

            // Edge 1 bis
            graph1.AddEdge(edge1).Should().BeFalse();
            AssertHasEdges(graph1, new[] { edge1, edge3 });

            // Edge 4 self edge
            var edge4 = new EquatableEdge<int>(2, 2);
            graph1.AddEdge(edge4).Should().BeTrue();
            AssertHasEdges(graph1, new[] { edge1, edge3, edge4 });


            // Graph with parent
            graph2.AddVertex(1);
            graph2.AddVertex(2);

            AssertNoEdge(parent2);
            AssertNoEdge(graph2);

            // Edge 1
            graph2.AddEdge(edge1).Should().BeTrue();
            AssertHasEdges(parent2, new[] { edge1 });
            AssertHasEdges(graph2, new[] { edge1 });

            // Edge 2
            graph2.AddEdge(edge2).Should().BeFalse();
            AssertHasEdges(parent2, new[] { edge1 });
            AssertHasEdges(graph2, new[] { edge1 });

            // Edge 3
            parent2.AddEdge(edge3).Should().BeTrue();
            AssertHasEdges(parent2, new[] { edge1, edge3 });
            AssertHasEdges(graph2, new[] { edge1 });

            graph2.AddEdge(edge3).Should().BeTrue();
            AssertHasEdges(parent2, new[] { edge1, edge3 });
            AssertHasEdges(graph2, new[] { edge1, edge3 });

            // Edge 1 bis
            graph2.AddEdge(edge1).Should().BeFalse();
            AssertHasEdges(parent2, new[] { edge1, edge3 });
            AssertHasEdges(graph2, new[] { edge1, edge3 });

            // Edge 4 self edge
            graph2.AddEdge(edge4).Should().BeTrue();
            AssertHasEdges(parent2, new[] { edge1, edge3, edge4 });
            AssertHasEdges(graph2, new[] { edge1, edge3, edge4 });
        }

        protected static void AddEdge_ForbiddenParallelEdges_Test(
            BidirectionalMatrixGraph<Edge<int>> graph)
        {
            int edgeAdded = 0;

            AssertNoEdge(graph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeAdded += e =>
            {
                e.Should().NotBeNull();
                ++edgeAdded;
            };

            // Edge 1
            var edge1 = new Edge<int>(1, 2);
            graph.AddEdge(edge1).Should().BeTrue();
            edgeAdded.Should().Be(1);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 2
            var edge2 = new Edge<int>(2, 1);
            graph.AddEdge(edge2).Should().BeTrue();
            edgeAdded.Should().Be(2);
            AssertHasEdges(graph, new[] { edge1, edge2 });

            // Edge 3 self edge
            var edge3 = new Edge<int>(2, 2);
            graph.AddEdge(edge3).Should().BeTrue();
            edgeAdded.Should().Be(3);
            AssertHasEdges(graph, new[] { edge1, edge2, edge3 });
        }

        protected static void AddEdge_EquatableEdge_ForbiddenParallelEdges_Test(
            BidirectionalMatrixGraph<EquatableEdge<int>> graph)
        {
            int edgeAdded = 0;

            AssertNoEdge(graph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeAdded += e =>
            {
                e.Should().NotBeNull();
                ++edgeAdded;
            };

            // Edge 1
            var edge1 = new EquatableEdge<int>(1, 2);
            graph.AddEdge(edge1).Should().BeTrue();
            edgeAdded.Should().Be(1);
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 2
            var edge2 = new EquatableEdge<int>(2, 1);
            graph.AddEdge(edge2).Should().BeTrue();
            edgeAdded.Should().Be(2);
            AssertHasEdges(graph, new[] { edge1, edge2 });

            // Edge 3 self edge
            var edge3 = new EquatableEdge<int>(2, 2);
            graph.AddEdge(edge3).Should().BeTrue();
            edgeAdded.Should().Be(3);
            AssertHasEdges(graph, new[] { edge1, edge2, edge3 });
        }

        protected static void AddEdge_Throws_EdgesOnly_Test(
            IMutableEdgeListGraph<int, Edge<int>> graph)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Invoking(() => graph.AddEdge(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            AssertNoEdge(graph);
        }

        protected static void AddEdge_Throws_Test<TGraph>(TGraph graph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, Edge<int>>
        {
            AddEdge_Throws_EdgesOnly_Test(graph);

            // Both vertices not in graph
            Invoking(() => graph.AddEdge(new Edge<int>(0, 1))).Should().Throw<VertexNotFoundException>();
            AssertNoEdge(graph);

            // Source not in graph
            graph.AddVertex(1);
            Invoking(() => graph.AddEdge(new Edge<int>(0, 1))).Should().Throw<VertexNotFoundException>();
            AssertNoEdge(graph);

            // Target not in graph
            Invoking(() => graph.AddEdge(new Edge<int>(1, 0))).Should().Throw<VertexNotFoundException>();
            AssertNoEdge(graph);
        }

        protected static void AddEdge_Throws_Clusters_Test(
            ClusteredAdjacencyGraph<int, Edge<int>> graph)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Invoking(() => graph.AddEdge(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            AssertNoEdge(graph);

            // Both vertices not in graph
            Invoking(() => graph.AddEdge(new Edge<int>(0, 1))).Should().Throw<VertexNotFoundException>();
            AssertNoEdge(graph);

            // Source not in graph
            graph.AddVertex(1);
            Invoking(() => graph.AddEdge(new Edge<int>(0, 1))).Should().Throw<VertexNotFoundException>();
            AssertNoEdge(graph);

            // Target not in graph
            Invoking(() => graph.AddEdge(new Edge<int>(1, 0))).Should().Throw<VertexNotFoundException>();
            AssertNoEdge(graph);
        }

        protected static void AddEdgeRange_EdgesOnly_Test(
            IMutableEdgeListGraph<int, Edge<int>> graph)
        {
            int edgeAdded = 0;

            AssertNoEdge(graph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeAdded += e =>
            {
                e.Should().NotBeNull();
                ++edgeAdded;
            };

            // Edge 1, 2, 3
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 3);
            graph.AddEdgeRange(new[] { edge1, edge2, edge3 }).Should().Be(3);
            edgeAdded.Should().Be(3);
            AssertHasEdges(graph, new[] { edge1, edge2, edge3 });

            // Edge 1, 4
            var edge4 = new Edge<int>(2, 2);
            graph.AddEdgeRange(new[] { edge1, edge4 }).Should().Be(1); // Showcase the add of only one edge
            edgeAdded.Should().Be(4);
            AssertHasEdges(graph, new[] { edge1, edge2, edge3, edge4 });
        }

        protected static void AddEdgeRange_Test<TGraph>(TGraph graph)
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
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeAdded += e =>
            {
                e.Should().NotBeNull();
                ++edgeAdded;
            };

            // Edge 1, 2, 3
            var edge1 = new Edge<int>(0, 1);
            var edge2 = new Edge<int>(0, 2);
            var edge3 = new Edge<int>(1, 2);
            graph.AddEdgeRange(new[] { edge1, edge2, edge3 }).Should().Be(3);
            edgeAdded.Should().Be(3);
            AssertHasEdges(graph, new[] { edge1, edge2, edge3 });

            // Edge 4
            var edge4 = new Edge<int>(2, 2);
            graph.AddEdgeRange(new[] { edge4 }).Should().Be(1);
            edgeAdded.Should().Be(4);
            AssertHasEdges(graph, new[] { edge1, edge2, edge3, edge4 });
        }

        protected static void AddEdgeRange_Clusters_Test(
            ClusteredAdjacencyGraph<int, Edge<int>> graph1,
            ClusteredAdjacencyGraph<int, Edge<int>> parent2,
            ClusteredAdjacencyGraph<int, Edge<int>> graph2)
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
            graph1.AddEdgeRange(new[] { edge1, edge2, edge3 }).Should().Be(3);
            AssertHasEdges(graph1, new[] { edge1, edge2, edge3 });

            // Edge 1, 4
            var edge4 = new Edge<int>(2, 2);
            graph1.AddEdgeRange(new[] { edge1, edge4 }).Should().Be(1); // Showcase the add of only one edge
            AssertHasEdges(graph1, new[] { edge1, edge2, edge3, edge4 });


            // Graph with parent
            graph2.AddVertex(1);
            graph2.AddVertex(2);
            graph2.AddVertex(3);

            AssertNoEdge(parent2);
            AssertNoEdge(graph2);

            // Edge 1, 2, 3
            graph2.AddEdgeRange(new[] { edge1, edge2, edge3 }).Should().Be(3);
            AssertHasEdges(parent2, new[] { edge1, edge2, edge3 });
            AssertHasEdges(graph2, new[] { edge1, edge2, edge3 });

            // Edge 1, 4
            parent2.AddEdgeRange(new[] { edge1, edge4 }).Should().Be(1); // Showcase the add of only one edge
            AssertHasEdges(parent2, new[] { edge1, edge2, edge3, edge4 });
            AssertHasEdges(graph2, new[] { edge1, edge2, edge3 });

            graph2.AddEdgeRange(new[] { edge1, edge4 }).Should().Be(1); // Showcase the add of only one edge
            AssertHasEdges(parent2, new[] { edge1, edge2, edge3, edge4 });
            AssertHasEdges(graph2, new[] { edge1, edge2, edge3, edge4 });
        }

        protected static void AddEdgeRange_Throws_EdgesOnly_Test(
            IMutableEdgeListGraph<int, Edge<int>> graph)
        {
            int edgeAdded = 0;

            AssertNoEdge(graph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeAdded += e =>
            {
                e.Should().NotBeNull();
                ++edgeAdded;
            };

            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Invoking(() => graph.AddEdgeRange(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            AssertNoEdge(graph);
            edgeAdded.Should().Be(0);

            // Edge 1, 2, 3
            var edge1 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(2, 3);
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
            Invoking(() => graph.AddEdgeRange(new[] { edge1, default, edge3 })).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
            edgeAdded.Should().Be(0);
            AssertNoEdge(graph);
        }

        protected static void AddEdgeRange_Throws_Test<TGraph>(TGraph graph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, Edge<int>>
        {
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);

            AddEdgeRange_Throws_EdgesOnly_Test(graph);
        }

        protected static void AddEdgeRange_Throws_Clusters_Test(
            ClusteredAdjacencyGraph<int, Edge<int>> graph)
        {
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);

            AssertNoEdge(graph);

            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => graph.AddEdgeRange(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            AssertNoEdge(graph);

            // Edge 1, 2, 3
            var edge1 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(2, 3);
#pragma warning disable CS8620
            Invoking(() => graph.AddEdgeRange(new[] { edge1, default, edge3 })).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8620
            AssertNoEdge(graph);
        }

        protected static void AddEdgeRange_ForbiddenParallelEdges_Throws_Test(
            BidirectionalMatrixGraph<Edge<int>> graph)
        {
            int edgeAdded = 0;

            AssertNoEdge(graph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeAdded += e =>
            {
                e.Should().NotBeNull();
                ++edgeAdded;
            };

            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => graph.AddEdgeRange(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            AssertNoEdge(graph);
            edgeAdded.Should().Be(0);

            // Edge 1, 2, 3
            var edge1 = new Edge<int>(0, 1);
            var edge3 = new Edge<int>(1, 2);
#pragma warning disable CS8620
            Invoking(() => graph.AddEdgeRange(new[] { edge1, default, edge3 })).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8620
            edgeAdded.Should().Be(0);
            AssertNoEdge(graph);

            // Edge 1, 3, 4
            var edge4 = new Edge<int>(0, 1);
            Invoking(() => graph.AddEdgeRange(new[] { edge1, edge3, edge4 })).Should().Throw<ParallelEdgeNotAllowedException>();
            edgeAdded.Should().Be(2);
            AssertHasEdges(graph, new[] { edge1, edge3 });

            // Out of range => vertex not found
            Invoking(() => graph.AddEdgeRange(new[] { new Edge<int>(4, 5), })).Should().Throw<VertexNotFoundException>();
            edgeAdded.Should().Be(2);
            AssertHasEdges(graph, new[] { edge1, edge3 });
        }


        protected static void AddEdge_ParallelEdges_EdgesOnly_Test(
            EdgeListGraph<int, Edge<int>> directedGraph,
            EdgeListGraph<int, Edge<int>> undirectedGraph,
            [InstantHandle] Func<
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
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            directedGraph.EdgeAdded += e =>
            {
                e.Should().NotBeNull();
                ++directedEdgeAdded;
            };

            AssertNoEdge(undirectedGraph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            undirectedGraph.EdgeAdded += e =>
            {
                e.Should().NotBeNull();
                ++undirectedEdgeAdded;
            };

            // Edge 1
            var edge1 = new Edge<int>(1, 2);
            addEdge(directedGraph, edge1).Should().BeTrue();
            directedEdgeAdded.Should().Be(1);
            AssertHasEdges(directedGraph, new[] { edge1 });

            addEdge(undirectedGraph, edge1).Should().BeTrue();
            undirectedEdgeAdded.Should().Be(1);
            AssertHasEdges(undirectedGraph, new[] { edge1 });

            // Edge 2
            var edge2 = new Edge<int>(1, 2);
            addEdge(directedGraph, edge2).Should().BeTrue();
            directedEdgeAdded.Should().Be(2);
            AssertHasEdges(directedGraph, new[] { edge1, edge2 });

            addEdge(undirectedGraph, edge2).Should().BeTrue();
            undirectedEdgeAdded.Should().Be(2);
            AssertHasEdges(undirectedGraph, new[] { edge1, edge2 });

            // Edge 3
            var edge3 = new Edge<int>(2, 1);
            addEdge(directedGraph, edge3).Should().BeTrue();
            directedEdgeAdded.Should().Be(3);
            AssertHasEdges(directedGraph, new[] { edge1, edge2, edge3 });

            addEdge(undirectedGraph, edge3).Should().BeTrue();
            undirectedEdgeAdded.Should().Be(3);
            AssertHasEdges(undirectedGraph, new[] { edge1, edge2, edge3 });

            // Edge 1 bis
            addEdge(directedGraph, edge1).Should().BeFalse();
            directedEdgeAdded.Should().Be(3);
            AssertHasEdges(directedGraph, new[] { edge1, edge2, edge3 });

            addEdge(undirectedGraph, edge1).Should().BeFalse();
            undirectedEdgeAdded.Should().Be(3);
            AssertHasEdges(undirectedGraph, new[] { edge1, edge2, edge3 });

            // Edge 4 self edge
            var edge4 = new Edge<int>(2, 2);
            addEdge(directedGraph, edge4).Should().BeTrue();
            directedEdgeAdded.Should().Be(4);
            AssertHasEdges(directedGraph, new[] { edge1, edge2, edge3, edge4 });

            addEdge(undirectedGraph, edge4).Should().BeTrue();
            undirectedEdgeAdded.Should().Be(4);
            AssertHasEdges(undirectedGraph, new[] { edge1, edge2, edge3, edge4 });
        }

        protected static void AddEdge_ParallelEdges_EquatableEdge_EdgesOnly_Test(
            EdgeListGraph<int, EquatableEdge<int>> directedGraph,
            EdgeListGraph<int, EquatableEdge<int>> undirectedGraph,
            [InstantHandle] Func<
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
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            directedGraph.EdgeAdded += e =>
            {
                e.Should().NotBeNull();
                ++directedEdgeAdded;
            };

            AssertNoEdge(undirectedGraph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            undirectedGraph.EdgeAdded += e =>
            {
                e.Should().NotBeNull();
                ++undirectedEdgeAdded;
            };

            // Edge 1
            var edge1 = new EquatableEdge<int>(1, 2);
            addEdge(directedGraph, edge1).Should().BeTrue();
            directedEdgeAdded.Should().Be(1);
            AssertHasEdges(directedGraph, new[] { edge1 });

            addEdge(undirectedGraph, edge1).Should().BeTrue();
            undirectedEdgeAdded.Should().Be(1);
            AssertHasEdges(undirectedGraph, new[] { edge1 });

            // Edge 2
            var edge2 = new EquatableEdge<int>(1, 2);
            addEdge(directedGraph, edge2).Should().BeFalse();
            directedEdgeAdded.Should().Be(1);
            AssertHasEdges(directedGraph, new[] { edge1 });

            addEdge(undirectedGraph, edge2).Should().BeFalse();
            undirectedEdgeAdded.Should().Be(1);
            AssertHasEdges(undirectedGraph, new[] { edge1 });

            // Edge 3
            var edge3 = new EquatableEdge<int>(2, 1);
            addEdge(directedGraph, edge3).Should().BeTrue();
            directedEdgeAdded.Should().Be(2);
            AssertHasEdges(directedGraph, new[] { edge1, edge3 });

            addEdge(undirectedGraph, edge3).Should().BeTrue();
            undirectedEdgeAdded.Should().Be(2);
            AssertHasEdges(undirectedGraph, new[] { edge1, edge3 });

            // Edge 1 bis
            addEdge(directedGraph, edge1).Should().BeFalse();
            directedEdgeAdded.Should().Be(2);
            AssertHasEdges(directedGraph, new[] { edge1, edge3 });

            addEdge(undirectedGraph, edge1).Should().BeFalse();
            undirectedEdgeAdded.Should().Be(2);
            AssertHasEdges(undirectedGraph, new[] { edge1, edge3 });

            // Edge 4 self edge
            var edge4 = new EquatableEdge<int>(2, 2);
            addEdge(directedGraph, edge4).Should().BeTrue();
            directedEdgeAdded.Should().Be(3);
            AssertHasEdges(directedGraph, new[] { edge1, edge3, edge4 });

            addEdge(undirectedGraph, edge4).Should().BeTrue();
            undirectedEdgeAdded.Should().Be(3);
            AssertHasEdges(undirectedGraph, new[] { edge1, edge3, edge4 });
        }

        protected static void AddEdge_NoParallelEdges_EdgesOnly_Test(
            EdgeListGraph<int, Edge<int>> directedGraph,
            EdgeListGraph<int, Edge<int>> undirectedGraph,
            [InstantHandle] Func<
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
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            directedGraph.EdgeAdded += e =>
            {
                e.Should().NotBeNull();
                ++directedEdgeAdded;
            };

            AssertNoEdge(undirectedGraph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            undirectedGraph.EdgeAdded += e =>
            {
                e.Should().NotBeNull();
                ++undirectedEdgeAdded;
            };

            // Edge 1
            var edge1 = new Edge<int>(1, 2);
            addEdge(directedGraph, edge1).Should().BeTrue();
            directedEdgeAdded.Should().Be(1);
            AssertHasEdges(directedGraph, new[] { edge1 });

            addEdge(undirectedGraph, edge1).Should().BeTrue();
            undirectedEdgeAdded.Should().Be(1);
            AssertHasEdges(undirectedGraph, new[] { edge1 });

            // Edge 2
            var edge2 = new Edge<int>(1, 2);
            addEdge(directedGraph, edge2).Should().BeFalse();
            directedEdgeAdded.Should().Be(1);
            AssertHasEdges(directedGraph, new[] { edge1 });

            addEdge(undirectedGraph, edge2).Should().BeFalse();
            undirectedEdgeAdded.Should().Be(1);
            AssertHasEdges(undirectedGraph, new[] { edge1 });

            // Edge 3
            var edge3 = new Edge<int>(2, 1);
            addEdge(directedGraph, edge3).Should().BeTrue();
            directedEdgeAdded.Should().Be(2);
            AssertHasEdges(directedGraph, new[] { edge1, edge3 });

            addEdge(undirectedGraph, edge3).Should().BeFalse();
            undirectedEdgeAdded.Should().Be(1);
            AssertHasEdges(undirectedGraph, new[] { edge1 });

            // Edge 1 bis
            addEdge(directedGraph, edge1).Should().BeFalse();
            directedEdgeAdded.Should().Be(2);
            AssertHasEdges(directedGraph, new[] { edge1, edge3 });

            addEdge(undirectedGraph, edge1).Should().BeFalse();
            undirectedEdgeAdded.Should().Be(1);
            AssertHasEdges(undirectedGraph, new[] { edge1 });

            // Edge 4 self edge
            var edge4 = new Edge<int>(2, 2);
            addEdge(directedGraph, edge4).Should().BeTrue();
            directedEdgeAdded.Should().Be(3);
            AssertHasEdges(directedGraph, new[] { edge1, edge3, edge4 });

            addEdge(undirectedGraph, edge4).Should().BeTrue();
            undirectedEdgeAdded.Should().Be(2);
            AssertHasEdges(undirectedGraph, new[] { edge1, edge4 });
        }

        protected static void AddEdge_NoParallelEdges_EquatableEdge_EdgesOnly_Test(
            EdgeListGraph<int, EquatableEdge<int>> directedGraph,
            EdgeListGraph<int, EquatableEdge<int>> undirectedGraph,
            [InstantHandle] Func<
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
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            directedGraph.EdgeAdded += e =>
            {
                e.Should().NotBeNull();
                ++directedEdgeAdded;
            };

            AssertNoEdge(undirectedGraph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            undirectedGraph.EdgeAdded += e =>
            {
                e.Should().NotBeNull();
                ++undirectedEdgeAdded;
            };

            // Edge 1
            var edge1 = new EquatableEdge<int>(1, 2);
            addEdge(directedGraph, edge1).Should().BeTrue();
            directedEdgeAdded.Should().Be(1);
            AssertHasEdges(directedGraph, new[] { edge1 });

            addEdge(undirectedGraph, edge1).Should().BeTrue();
            undirectedEdgeAdded.Should().Be(1);
            AssertHasEdges(undirectedGraph, new[] { edge1 });

            // Edge 2
            var edge2 = new EquatableEdge<int>(1, 2);
            addEdge(directedGraph, edge2).Should().BeFalse();
            directedEdgeAdded.Should().Be(1);
            AssertHasEdges(directedGraph, new[] { edge1 });

            addEdge(undirectedGraph, edge2).Should().BeFalse();
            undirectedEdgeAdded.Should().Be(1);
            AssertHasEdges(undirectedGraph, new[] { edge1 });

            // Edge 3
            var edge3 = new EquatableEdge<int>(2, 1);
            addEdge(directedGraph, edge3).Should().BeTrue();
            directedEdgeAdded.Should().Be(2);
            AssertHasEdges(directedGraph, new[] { edge1, edge3 });

            addEdge(undirectedGraph, edge3).Should().BeFalse();
            undirectedEdgeAdded.Should().Be(1);
            AssertHasEdges(undirectedGraph, new[] { edge1 });

            // Edge 1 bis
            addEdge(directedGraph, edge1).Should().BeFalse();
            directedEdgeAdded.Should().Be(2);
            AssertHasEdges(directedGraph, new[] { edge1, edge3 });

            addEdge(undirectedGraph, edge1).Should().BeFalse();
            undirectedEdgeAdded.Should().Be(1);
            AssertHasEdges(undirectedGraph, new[] { edge1 });

            // Edge 4 self edge
            var edge4 = new EquatableEdge<int>(2, 2);
            addEdge(directedGraph, edge4).Should().BeTrue();
            directedEdgeAdded.Should().Be(3);
            AssertHasEdges(directedGraph, new[] { edge1, edge3, edge4 });

            addEdge(undirectedGraph, edge4).Should().BeTrue();
            undirectedEdgeAdded.Should().Be(2);
            AssertHasEdges(undirectedGraph, new[] { edge1, edge4 });
        }


        protected static void AddEdge_ImmutableGraph_NoUpdate<TGraph>(
            TGraph wrappedGraph,
            [InstantHandle] Func<IEdgeSet<int, Edge<int>>> createGraph)
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
            TGraph wrappedGraph,
            [InstantHandle] Func<IEdgeSet<int, Edge<int>>> createGraph)
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
