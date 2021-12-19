#nullable enable

using JetBrains.Annotations;
using static FastGraph.Tests.GraphTestHelpers;

namespace FastGraph.Tests.Structures
{
    internal partial class GraphTestsBase
    {
        #region Add Vertices

        protected static void AddVertex_Test(
            IMutableVertexSet<TestVertex> graph)
        {
            int vertexAdded = 0;

            AssertNoVertex(graph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.VertexAdded += v =>
            {
                v.Should().NotBeNull();
                ++vertexAdded;
            };

            // Vertex 1
            var vertex1 = new TestVertex("1");
            graph.AddVertex(vertex1).Should().BeTrue();
            vertexAdded.Should().Be(1);
            AssertHasVertices(graph, new[] { vertex1 });

            // Vertex 2
            var vertex2 = new TestVertex("2");
            graph.AddVertex(vertex2).Should().BeTrue();
            vertexAdded.Should().Be(2);
            AssertHasVertices(graph, new[] { vertex1, vertex2 });

            // Vertex 1 bis
            graph.AddVertex(vertex1).Should().BeFalse();
            vertexAdded.Should().Be(2);
            AssertHasVertices(graph, new[] { vertex1, vertex2 });

            // Other "Vertex 1"
            var otherVertex1 = new TestVertex("1");
            graph.AddVertex(otherVertex1).Should().BeTrue();
            vertexAdded.Should().Be(3);
            AssertHasVertices(graph, new[] { vertex1, vertex2, otherVertex1 });
        }

        protected static void AddVertex_Clusters_Test<TEdge>(
            ClusteredAdjacencyGraph<TestVertex, TEdge> graph1,
            ClusteredAdjacencyGraph<TestVertex, TEdge> parent2,
            ClusteredAdjacencyGraph<TestVertex, TEdge> graph2)
            where TEdge : IEdge<TestVertex>
        {
            AssertNoVertex(graph1);

            // Graph without parent
            // Vertex 1
            var vertex1 = new TestVertex("1");
            graph1.AddVertex(vertex1).Should().BeTrue();
            AssertHasVertices(graph1, new[] { vertex1 });

            // Vertex 2
            var vertex2 = new TestVertex("2");
            graph1.AddVertex(vertex2).Should().BeTrue();
            AssertHasVertices(graph1, new[] { vertex1, vertex2 });

            // Vertex 1 bis
            graph1.AddVertex(vertex1).Should().BeFalse();
            AssertHasVertices(graph1, new[] { vertex1, vertex2 });

            // Other "Vertex 1"
            var otherVertex1 = new TestVertex("1");
            graph1.AddVertex(otherVertex1).Should().BeTrue();
            AssertHasVertices(graph1, new[] { vertex1, vertex2, otherVertex1 });

            // Graph with parent
            AssertNoVertex(parent2);
            AssertNoVertex(graph2);

            graph2.AddVertex(vertex1).Should().BeTrue();
            AssertHasVertices(parent2, new[] { vertex1 });
            AssertHasVertices(graph2, new[] { vertex1 });

            // Vertex 2
            parent2.AddVertex(vertex2).Should().BeTrue();
            AssertHasVertices(parent2, new[] { vertex1, vertex2 });
            AssertHasVertices(graph2, new[] { vertex1 });

            graph2.AddVertex(vertex2).Should().BeTrue();
            AssertHasVertices(parent2, new[] { vertex1, vertex2 });
            AssertHasVertices(graph2, new[] { vertex1, vertex2 });

            // Vertex 1 bis
            graph2.AddVertex(vertex1).Should().BeFalse();
            AssertHasVertices(parent2, new[] { vertex1, vertex2 });
            AssertHasVertices(graph2, new[] { vertex1, vertex2 });

            // Other "Vertex 1"
            graph2.AddVertex(otherVertex1).Should().BeTrue();
            AssertHasVertices(parent2, new[] { vertex1, vertex2, otherVertex1 });
            AssertHasVertices(graph2, new[] { vertex1, vertex2, otherVertex1 });
        }

        protected static void AddVertex_Throws_Test<TVertex>(
            IMutableVertexSet<TVertex> graph)
            where TVertex : notnull
        {
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8604
            Invoking(() => graph.AddVertex(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8604
            AssertNoVertex(graph);
        }

        protected static void AddVertex_Throws_Clusters_Test<TVertex, TEdge>(
            ClusteredAdjacencyGraph<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8604
            Invoking(() => graph.AddVertex(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8604
            AssertNoVertex(graph);
        }

        protected static void AddVertex_EquatableVertex_Test(
            IMutableVertexSet<EquatableTestVertex> graph)
        {
            int vertexAdded = 0;

            AssertNoVertex(graph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.VertexAdded += v =>
            {
                v.Should().NotBeNull();
                ++vertexAdded;
            };

            // Vertex 1
            var vertex1 = new EquatableTestVertex("1");
            graph.AddVertex(vertex1).Should().BeTrue();
            vertexAdded.Should().Be(1);
            AssertHasVertices(graph, new[] { vertex1 });

            // Vertex 2
            var vertex2 = new EquatableTestVertex("2");
            graph.AddVertex(vertex2).Should().BeTrue();
            vertexAdded.Should().Be(2);
            AssertHasVertices(graph, new[] { vertex1, vertex2 });

            // Vertex 1 bis
            graph.AddVertex(vertex1).Should().BeFalse();
            vertexAdded.Should().Be(2);
            AssertHasVertices(graph, new[] { vertex1, vertex2 });

            // Other "Vertex 1"
            var otherVertex1 = new EquatableTestVertex("1");
            graph.AddVertex(otherVertex1).Should().BeFalse();
            vertexAdded.Should().Be(2);
            AssertHasVertices(graph, new[] { vertex1, vertex2 });
        }

        protected static void AddVertex_EquatableVertex_Clusters_Test<TEdge>(
            ClusteredAdjacencyGraph<EquatableTestVertex, TEdge> graph1,
            ClusteredAdjacencyGraph<EquatableTestVertex, TEdge> parent2,
            ClusteredAdjacencyGraph<EquatableTestVertex, TEdge> graph2)
            where TEdge : IEdge<EquatableTestVertex>
        {
            AssertNoVertex(graph1);

            // Graph without parent
            // Vertex 1
            var vertex1 = new EquatableTestVertex("1");
            graph1.AddVertex(vertex1).Should().BeTrue();
            AssertHasVertices(graph1, new[] { vertex1 });

            // Vertex 2
            var vertex2 = new EquatableTestVertex("2");
            graph1.AddVertex(vertex2).Should().BeTrue();
            AssertHasVertices(graph1, new[] { vertex1, vertex2 });

            // Vertex 1 bis
            graph1.AddVertex(vertex1).Should().BeFalse();
            AssertHasVertices(graph1, new[] { vertex1, vertex2 });

            // Other "Vertex 1"
            var otherVertex1 = new EquatableTestVertex("1");
            graph1.AddVertex(otherVertex1).Should().BeFalse();
            AssertHasVertices(graph1, new[] { vertex1, vertex2 });

            // Graph with parent
            AssertNoVertex(parent2);
            AssertNoVertex(graph2);

            graph2.AddVertex(vertex1).Should().BeTrue();
            AssertHasVertices(parent2, new[] { vertex1 });
            AssertHasVertices(graph2, new[] { vertex1 });

            // Vertex 2
            parent2.AddVertex(vertex2).Should().BeTrue();
            AssertHasVertices(parent2, new[] { vertex1, vertex2 });
            AssertHasVertices(graph2, new[] { vertex1 });

            graph2.AddVertex(vertex2).Should().BeTrue();
            AssertHasVertices(parent2, new[] { vertex1, vertex2 });
            AssertHasVertices(graph2, new[] { vertex1, vertex2 });

            // Vertex 1 bis
            graph2.AddVertex(vertex1).Should().BeFalse();
            AssertHasVertices(parent2, new[] { vertex1, vertex2 });
            AssertHasVertices(graph2, new[] { vertex1, vertex2 });

            // Other "Vertex 1"
            graph2.AddVertex(otherVertex1).Should().BeFalse();
            AssertHasVertices(parent2, new[] { vertex1, vertex2 });
            AssertHasVertices(graph2, new[] { vertex1, vertex2 });
        }

        protected static void AddVertexRange_Test(
            IMutableVertexSet<TestVertex> graph)
        {
            int vertexAdded = 0;

            AssertNoVertex(graph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.VertexAdded += v =>
            {
                v.Should().NotBeNull();
                ++vertexAdded;
            };

            // Vertex 1, 2, 3
            var vertex1 = new TestVertex("1");
            var vertex2 = new TestVertex("2");
            var vertex3 = new TestVertex("3");
            graph.AddVertexRange(new[] { vertex1, vertex2, vertex3 }).Should().Be(3);
            vertexAdded.Should().Be(3);
            AssertHasVertices(graph, new[] { vertex1, vertex2, vertex3 });

            // Vertex 1, 4
            var vertex4 = new TestVertex("4");
            graph.AddVertexRange(new[] { vertex1, vertex4 }).Should().Be(1);
            vertexAdded.Should().Be(4);
            AssertHasVertices(graph, new[] { vertex1, vertex2, vertex3, vertex4 });
        }

        protected static void AddVertexRange_Clusters_Test<TEdge>(
            ClusteredAdjacencyGraph<TestVertex, TEdge> graph1,
            ClusteredAdjacencyGraph<TestVertex, TEdge> parent2,
            ClusteredAdjacencyGraph<TestVertex, TEdge> graph2)
            where TEdge : IEdge<TestVertex>
        {
            AssertNoVertex(graph1);

            // Graph without parent
            // Vertex 1, 2, 3
            var vertex1 = new TestVertex("1");
            var vertex2 = new TestVertex("2");
            var vertex3 = new TestVertex("3");
            graph1.AddVertexRange(new[] { vertex1, vertex2, vertex3 }).Should().Be(3);
            AssertHasVertices(graph1, new[] { vertex1, vertex2, vertex3 });

            // Vertex 1, 4
            var vertex4 = new TestVertex("4");
            graph1.AddVertexRange(new[] { vertex1, vertex4 }).Should().Be(1);
            AssertHasVertices(graph1, new[] { vertex1, vertex2, vertex3, vertex4 });

            // Graph with parent
            AssertNoVertex(parent2);
            AssertNoVertex(graph2);

            // Vertex 1, 2, 3
            graph2.AddVertexRange(new[] { vertex1, vertex2, vertex3 }).Should().Be(3);
            AssertHasVertices(parent2, new[] { vertex1, vertex2, vertex3 });
            AssertHasVertices(graph2, new[] { vertex1, vertex2, vertex3 });

            // Vertex 1, 4
            parent2.AddVertexRange(new[] { vertex1, vertex4 }).Should().Be(1);
            AssertHasVertices(parent2, new[] { vertex1, vertex2, vertex3, vertex4 });
            AssertHasVertices(graph2, new[] { vertex1, vertex2, vertex3 });

            graph2.AddVertexRange(new[] { vertex1, vertex4 }).Should().Be(1);
            AssertHasVertices(parent2, new[] { vertex1, vertex2, vertex3, vertex4 });
            AssertHasVertices(graph2, new[] { vertex1, vertex2, vertex3, vertex4 });
        }

        protected static void AddVertexRange_Throws_Test(
            IMutableVertexSet<TestVertex> graph)
        {
            int vertexAdded = 0;

            AssertNoVertex(graph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.VertexAdded += v =>
            {
                v.Should().NotBeNull();
                ++vertexAdded;
            };

            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => graph.AddVertexRange(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            AssertNoVertex(graph);
            vertexAdded.Should().Be(0);

            // Vertex 1, 2, 3
            var vertex1 = new TestVertex("1");
            var vertex3 = new TestVertex("3");
#pragma warning disable CS8620
            Invoking(() => graph.AddVertexRange(new[] { vertex1, default, vertex3 })).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8620
            AssertNoVertex(graph);
            vertexAdded.Should().Be(0);
        }

        protected static void AddVertexRange_Throws_Clusters_Test<TEdge>(
            ClusteredAdjacencyGraph<TestVertex, TEdge> graph)
            where TEdge : IEdge<TestVertex>
        {
            AssertNoVertex(graph);

            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => graph.AddVertexRange(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            AssertNoVertex(graph);

            // Vertex 1, 2, 3
            var vertex1 = new TestVertex("1");
            var vertex3 = new TestVertex("3");
#pragma warning disable CS8620
            Invoking(() => graph.AddVertexRange(new[] { vertex1, default, vertex3 })).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8620
            AssertNoVertex(graph);
        }


        protected static void AddVertex_ImmutableGraph_NoUpdate(
            IMutableVertexSet<int> wrappedGraph,
            [InstantHandle] Func<IVertexSet<int>> createGraph)
        {
            IVertexSet<int> graph = createGraph();

            wrappedGraph.AddVertex(1);
            AssertNoVertex(graph);  // Graph is not updated
        }

        protected static void AddVertex_ImmutableGraph_WithUpdate(
            IMutableVertexSet<int> wrappedGraph,
            [InstantHandle] Func<IVertexSet<int>> createGraph)
        {
            IVertexSet<int> graph = createGraph();

            wrappedGraph.AddVertex(1);
            AssertHasVertices(graph, new[] { 1 });  // Graph is updated
        }

        #endregion
    }
}
