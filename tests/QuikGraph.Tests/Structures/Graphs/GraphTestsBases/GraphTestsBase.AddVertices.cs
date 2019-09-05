using System;
using JetBrains.Annotations;
using NUnit.Framework;

namespace QuikGraph.Tests.Structures
{
    internal partial class GraphTestsBase
    {
        #region Add Vertices

        protected static void AddVertex_Test(
            [NotNull] IMutableVertexSet<TestVertex> graph)
        {
            int vertexAdded = 0;

            AssertNoVertex(graph);
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

        protected static void AddVertex_Throws_Test<TVertex>(
            [NotNull] IMutableVertexSet<TVertex> graph)
            where TVertex : class
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.AddVertex(null));
            AssertNoVertex(graph);
        }

        protected static void AddVertex_EquatableVertex_Test(
            [NotNull] IMutableVertexSet<EquatableTestVertex> graph)
        {
            int vertexAdded = 0;

            AssertNoVertex(graph);
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

        protected static void AddVertexRange_Test(
            [NotNull] IMutableVertexSet<TestVertex> graph)
        {
            int vertexAdded = 0;

            AssertNoVertex(graph);
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

        protected static void AddVertexRange_Throws_Test(
            [NotNull] IMutableVertexSet<TestVertex> graph)
        {
            int vertexAdded = 0;

            AssertNoVertex(graph);
            graph.VertexAdded += v =>
            {
                Assert.IsNotNull(v);
                ++vertexAdded;
            };

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.AddVertexRange(null));
            AssertNoVertex(graph);
            Assert.AreEqual(0, vertexAdded);

            // Vertex 1, 2, 3
            var vertex1 = new TestVertex("1");
            var vertex3 = new TestVertex("3");
            Assert.Throws<ArgumentNullException>(() => graph.AddVertexRange(new[] { vertex1, null, vertex3 }));
            AssertNoVertex(graph);
            Assert.AreEqual(0, vertexAdded);
        }


        protected static void AddVertex_ImmutableGraph_NoUpdate(
            [NotNull] IMutableVertexSet<int> wrappedGraph,
            [NotNull, InstantHandle] Func<IVertexSet<int>> createGraph)
        {
            IVertexSet<int> graph = createGraph();

            wrappedGraph.AddVertex(1);
            AssertNoVertex(graph);  // Graph is not updated
        }

        protected static void AddVertex_ImmutableGraph_WithUpdate(
            [NotNull] IMutableVertexSet<int> wrappedGraph,
            [NotNull, InstantHandle] Func<IVertexSet<int>> createGraph)
        {
            IVertexSet<int> graph = createGraph();

            wrappedGraph.AddVertex(1);
            AssertHasVertices(graph, new[] { 1 });  // Graph is updated
        }

        #endregion
    }
}
