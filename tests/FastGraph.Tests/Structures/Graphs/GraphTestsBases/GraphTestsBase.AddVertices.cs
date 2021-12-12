#nullable enable

using JetBrains.Annotations;
using NUnit.Framework;
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
                Assert.IsNotNull(v);
                ++vertexAdded;
            };

            // Vertex 1
            var vertex1 = new TestVertex("1");
            Assert.IsTrue(graph.AddVertex(vertex1));
            Assert.AreEqual(1, vertexAdded);
            AssertHasVertices(graph, new[] { vertex1 });

            // Vertex 2
            var vertex2 = new TestVertex("2");
            Assert.IsTrue(graph.AddVertex(vertex2));
            Assert.AreEqual(2, vertexAdded);
            AssertHasVertices(graph, new[] { vertex1, vertex2 });

            // Vertex 1 bis
            Assert.IsFalse(graph.AddVertex(vertex1));
            Assert.AreEqual(2, vertexAdded);
            AssertHasVertices(graph, new[] { vertex1, vertex2 });

            // Other "Vertex 1"
            var otherVertex1 = new TestVertex("1");
            Assert.IsTrue(graph.AddVertex(otherVertex1));
            Assert.AreEqual(3, vertexAdded);
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
            Assert.IsTrue(graph1.AddVertex(vertex1));
            AssertHasVertices(graph1, new[] { vertex1 });

            // Vertex 2
            var vertex2 = new TestVertex("2");
            Assert.IsTrue(graph1.AddVertex(vertex2));
            AssertHasVertices(graph1, new[] { vertex1, vertex2 });

            // Vertex 1 bis
            Assert.IsFalse(graph1.AddVertex(vertex1));
            AssertHasVertices(graph1, new[] { vertex1, vertex2 });

            // Other "Vertex 1"
            var otherVertex1 = new TestVertex("1");
            Assert.IsTrue(graph1.AddVertex(otherVertex1));
            AssertHasVertices(graph1, new[] { vertex1, vertex2, otherVertex1 });

            // Graph with parent
            AssertNoVertex(parent2);
            AssertNoVertex(graph2);

            Assert.IsTrue(graph2.AddVertex(vertex1));
            AssertHasVertices(parent2, new[] { vertex1 });
            AssertHasVertices(graph2, new[] { vertex1 });

            // Vertex 2
            Assert.IsTrue(parent2.AddVertex(vertex2));
            AssertHasVertices(parent2, new[] { vertex1, vertex2 });
            AssertHasVertices(graph2, new[] { vertex1 });

            Assert.IsTrue(graph2.AddVertex(vertex2));
            AssertHasVertices(parent2, new[] { vertex1, vertex2 });
            AssertHasVertices(graph2, new[] { vertex1, vertex2 });

            // Vertex 1 bis
            Assert.IsFalse(graph2.AddVertex(vertex1));
            AssertHasVertices(parent2, new[] { vertex1, vertex2 });
            AssertHasVertices(graph2, new[] { vertex1, vertex2 });

            // Other "Vertex 1"
            Assert.IsTrue(graph2.AddVertex(otherVertex1));
            AssertHasVertices(parent2, new[] { vertex1, vertex2, otherVertex1 });
            AssertHasVertices(graph2, new[] { vertex1, vertex2, otherVertex1 });
        }

        protected static void AddVertex_Throws_Test<TVertex>(
            IMutableVertexSet<TVertex> graph)
            where TVertex : notnull
        {
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8604
            Assert.Throws<ArgumentNullException>(() => graph.AddVertex(default));
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
            Assert.Throws<ArgumentNullException>(() => graph.AddVertex(default));
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
                Assert.IsNotNull(v);
                ++vertexAdded;
            };

            // Vertex 1
            var vertex1 = new EquatableTestVertex("1");
            Assert.IsTrue(graph.AddVertex(vertex1));
            Assert.AreEqual(1, vertexAdded);
            AssertHasVertices(graph, new[] { vertex1 });

            // Vertex 2
            var vertex2 = new EquatableTestVertex("2");
            Assert.IsTrue(graph.AddVertex(vertex2));
            Assert.AreEqual(2, vertexAdded);
            AssertHasVertices(graph, new[] { vertex1, vertex2 });

            // Vertex 1 bis
            Assert.IsFalse(graph.AddVertex(vertex1));
            Assert.AreEqual(2, vertexAdded);
            AssertHasVertices(graph, new[] { vertex1, vertex2 });

            // Other "Vertex 1"
            var otherVertex1 = new EquatableTestVertex("1");
            Assert.IsFalse(graph.AddVertex(otherVertex1));
            Assert.AreEqual(2, vertexAdded);
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
            Assert.IsTrue(graph1.AddVertex(vertex1));
            AssertHasVertices(graph1, new[] { vertex1 });

            // Vertex 2
            var vertex2 = new EquatableTestVertex("2");
            Assert.IsTrue(graph1.AddVertex(vertex2));
            AssertHasVertices(graph1, new[] { vertex1, vertex2 });

            // Vertex 1 bis
            Assert.IsFalse(graph1.AddVertex(vertex1));
            AssertHasVertices(graph1, new[] { vertex1, vertex2 });

            // Other "Vertex 1"
            var otherVertex1 = new EquatableTestVertex("1");
            Assert.IsFalse(graph1.AddVertex(otherVertex1));
            AssertHasVertices(graph1, new[] { vertex1, vertex2 });

            // Graph with parent
            AssertNoVertex(parent2);
            AssertNoVertex(graph2);

            Assert.IsTrue(graph2.AddVertex(vertex1));
            AssertHasVertices(parent2, new[] { vertex1 });
            AssertHasVertices(graph2, new[] { vertex1 });

            // Vertex 2
            Assert.IsTrue(parent2.AddVertex(vertex2));
            AssertHasVertices(parent2, new[] { vertex1, vertex2 });
            AssertHasVertices(graph2, new[] { vertex1 });

            Assert.IsTrue(graph2.AddVertex(vertex2));
            AssertHasVertices(parent2, new[] { vertex1, vertex2 });
            AssertHasVertices(graph2, new[] { vertex1, vertex2 });

            // Vertex 1 bis
            Assert.IsFalse(graph2.AddVertex(vertex1));
            AssertHasVertices(parent2, new[] { vertex1, vertex2 });
            AssertHasVertices(graph2, new[] { vertex1, vertex2 });

            // Other "Vertex 1"
            Assert.IsFalse(graph2.AddVertex(otherVertex1));
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
                Assert.IsNotNull(v);
                ++vertexAdded;
            };

            // Vertex 1, 2, 3
            var vertex1 = new TestVertex("1");
            var vertex2 = new TestVertex("2");
            var vertex3 = new TestVertex("3");
            Assert.AreEqual(3, graph.AddVertexRange(new[] { vertex1, vertex2, vertex3 }));
            Assert.AreEqual(3, vertexAdded);
            AssertHasVertices(graph, new[] { vertex1, vertex2, vertex3 });

            // Vertex 1, 4
            var vertex4 = new TestVertex("4");
            Assert.AreEqual(1, graph.AddVertexRange(new[] { vertex1, vertex4 }));
            Assert.AreEqual(4, vertexAdded);
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
            Assert.AreEqual(3, graph1.AddVertexRange(new[] { vertex1, vertex2, vertex3 }));
            AssertHasVertices(graph1, new[] { vertex1, vertex2, vertex3 });

            // Vertex 1, 4
            var vertex4 = new TestVertex("4");
            Assert.AreEqual(1, graph1.AddVertexRange(new[] { vertex1, vertex4 }));
            AssertHasVertices(graph1, new[] { vertex1, vertex2, vertex3, vertex4 });

            // Graph with parent
            AssertNoVertex(parent2);
            AssertNoVertex(graph2);

            // Vertex 1, 2, 3
            Assert.AreEqual(3, graph2.AddVertexRange(new[] { vertex1, vertex2, vertex3 }));
            AssertHasVertices(parent2, new[] { vertex1, vertex2, vertex3 });
            AssertHasVertices(graph2, new[] { vertex1, vertex2, vertex3 });

            // Vertex 1, 4
            Assert.AreEqual(1, parent2.AddVertexRange(new[] { vertex1, vertex4 }));
            AssertHasVertices(parent2, new[] { vertex1, vertex2, vertex3, vertex4 });
            AssertHasVertices(graph2, new[] { vertex1, vertex2, vertex3 });

            Assert.AreEqual(1, graph2.AddVertexRange(new[] { vertex1, vertex4 }));
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
                Assert.IsNotNull(v);
                ++vertexAdded;
            };

            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Assert.Throws<ArgumentNullException>(() => graph.AddVertexRange(default));
#pragma warning restore CS8625
            AssertNoVertex(graph);
            Assert.AreEqual(0, vertexAdded);

            // Vertex 1, 2, 3
            var vertex1 = new TestVertex("1");
            var vertex3 = new TestVertex("3");
#pragma warning disable CS8620
            Assert.Throws<ArgumentNullException>(() => graph.AddVertexRange(new[] { vertex1, default, vertex3 }));
#pragma warning restore CS8620
            AssertNoVertex(graph);
            Assert.AreEqual(0, vertexAdded);
        }

        protected static void AddVertexRange_Throws_Clusters_Test<TEdge>(
            ClusteredAdjacencyGraph<TestVertex, TEdge> graph)
            where TEdge : IEdge<TestVertex>
        {
            AssertNoVertex(graph);

            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Assert.Throws<ArgumentNullException>(() => graph.AddVertexRange(default));
#pragma warning restore CS8625
            AssertNoVertex(graph);

            // Vertex 1, 2, 3
            var vertex1 = new TestVertex("1");
            var vertex3 = new TestVertex("3");
#pragma warning disable CS8620
            Assert.Throws<ArgumentNullException>(() => graph.AddVertexRange(new[] { vertex1, default, vertex3 }));
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
