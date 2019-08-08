using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace QuikGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="AdjacencyGraph{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class AdjacencyGraphTests : GraphTestsBase
    {
        [Test]
        public void Construction()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            AssertEmptyGraph(graph);

            graph = new AdjacencyGraph<int, Edge<int>>(true);
            AssertEmptyGraph(graph);

            graph = new AdjacencyGraph<int, Edge<int>>(false);
            AssertEmptyGraph(graph, false);

            graph = new AdjacencyGraph<int, Edge<int>>(true, 12);
            AssertEmptyGraph(graph);

            graph = new AdjacencyGraph<int, Edge<int>>(false, 12);
            AssertEmptyGraph(graph, false);

            graph = new AdjacencyGraph<int, Edge<int>>(true, 42, 12);
            AssertEmptyGraph(graph, edgeCapacity: 12);

            graph = new AdjacencyGraph<int, Edge<int>>(false, 42, 12);
            AssertEmptyGraph(graph, false, 12);

            #region Local function

            void AssertEmptyGraph<TVertex, TEdge>(AdjacencyGraph<TVertex, TEdge> g, bool parallelEdges = true, int edgeCapacity = -1)
                where TEdge : IEdge<TVertex>
            {
                Assert.IsTrue(g.IsDirected);
                Assert.AreEqual(parallelEdges, g.AllowParallelEdges);
                AssertNoVertex(g);
                AssertNoEdge(g);
                Assert.AreEqual(edgeCapacity, g.EdgeCapacity);
                Assert.AreSame(typeof(int), g.VertexType);
                Assert.AreSame(typeof(Edge<int>), g.EdgeType);
            }

            #endregion
        }

        #region Add Vertices

        [Test]
        public void AddVertex()
        {
            int vertexAdded = 0;

            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();

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
            AssertHasVertices(graph, new []{ vertex1 });

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

        [Test]
        public void AddVertex_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.AddVertex(null));
            AssertNoVertex(graph);
        }

        [Test]
        public void AddVertex_EquatableVertex()
        {
            int vertexAdded = 0;

            var graph = new AdjacencyGraph<EquatableTestVertex, Edge<EquatableTestVertex>>();

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

        [Test]
        public void AddVertexRange()
        {
            int vertexAdded = 0;

            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();

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
            Assert.AreEqual(3, graph.AddVertexRange(new []{ vertex1, vertex2, vertex3 }));
            Assert.AreEqual(3, vertexAdded);
            AssertHasVertices(graph, new[] { vertex1, vertex2, vertex3 });

            // Vertex 1, 4
            var vertex4 = new TestVertex("4");
            Assert.AreEqual(1, graph.AddVertexRange(new[] { vertex1, vertex4 }));
            Assert.AreEqual(4, vertexAdded);
            AssertHasVertices(graph, new[] { vertex1, vertex2, vertex3, vertex4 });
        }

        [Test]
        public void AddVertexRange_Throws()
        {
            int vertexAdded = 0;

            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();

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
            Assert.Throws<ArgumentNullException>(() => graph.AddVertexRange(new []{ vertex1, null, vertex3 }));
            AssertNoVertex(graph);
            Assert.AreEqual(0, vertexAdded);
        }

        #endregion

        #region Add Edges

        [Test]
        public void AddEdge_ParallelEdges()
        {
            int edgeAdded = 0;

            var graph = new AdjacencyGraph<int, Edge<int>>();
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
            AssertHasEdges(graph, new []{ edge1 });

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
        }

        [Test]
        public void AddEdge_ParallelEdges_EquatableEdge()
        {
            int edgeAdded = 0;

            var graph = new AdjacencyGraph<int, EquatableEdge<int>>();
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
        }

        [Test]
        public void AddEdge_NoParallelEdges()
        {
            int edgeAdded = 0;

            var graph = new AdjacencyGraph<int, Edge<int>>(false);
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
        }

        [Test]
        public void AddEdge_NoParallelEdges_EquatableEdge()
        {
            int edgeAdded = 0;

            var graph = new AdjacencyGraph<int, EquatableEdge<int>>(false);
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
        }

        [Test]
        public void AddEdge_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.AddEdge(null));
            AssertNoEdge(graph);

            Assert.Throws<KeyNotFoundException>(() => graph.AddEdge(new Edge<int>(0, 1)));
            AssertNoEdge(graph);
        }

        [Test]
        public void AddEdgeRange()
        {
            int edgeAdded = 0;

            var graph = new AdjacencyGraph<int, Edge<int>>(false);
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);

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
            Assert.AreEqual(3, graph.AddEdgeRange(new []{ edge1, edge2, edge3 }));
            Assert.AreEqual(3, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge2, edge3 });

            // Edge 1, 4
            var edge4 = new Edge<int>(2, 2);
            Assert.AreEqual(1, graph.AddEdgeRange(new []{ edge1, edge4 })); // Showcase the add of only one edge
            Assert.AreEqual(4, edgeAdded);
            AssertHasEdges(graph, new[] { edge1, edge2, edge3, edge4 });
        }

        [Test]
        public void AddEdgeRange_Throws()
        {
            int edgeAdded = 0;

            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);

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

        #endregion

        #region Add Vertices & Edges

        [Test]
        public void AddVerticesAndEdge()
        {
            int vertexAdded = 0;
            int edgeAdded = 0;

            var graph = new AdjacencyGraph<int, Edge<int>>();

            AssertNoEdge(graph);
            graph.VertexAdded += v =>
            {
                Assert.IsNotNull(v);
                ++vertexAdded;
            };
            graph.EdgeAdded += e =>
            {
                Assert.IsNotNull(e);
                ++edgeAdded;
            };

            // Edge 1
            var edge1 = new Edge<int>(1, 2);
            Assert.IsTrue(graph.AddVerticesAndEdge(edge1));
            Assert.AreEqual(2, vertexAdded);
            Assert.AreEqual(1, edgeAdded);
            AssertHasVertices(graph, new[] { 1, 2 });
            AssertHasEdges(graph, new[] { edge1 });

            // Edge 2
            var edge2 = new Edge<int>(1, 3);
            Assert.IsTrue(graph.AddVerticesAndEdge(edge2));
            Assert.AreEqual(3, vertexAdded);
            Assert.AreEqual(2, edgeAdded);
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge1, edge2 });

            // Edge 3
            var edge3 = new Edge<int>(2, 3);
            Assert.IsTrue(graph.AddVerticesAndEdge(edge3));
            Assert.AreEqual(3, vertexAdded);
            Assert.AreEqual(3, edgeAdded);
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge1, edge2, edge3 });
        }

        [Test]
        public void AddVerticesAndEdge_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.AddVerticesAndEdge(null));
            AssertNoVertex(graph);
            AssertNoEdge(graph);
        }

        [Test]
        public void AddVerticesAndEdgeRange()
        {
            int vertexAdded = 0;
            int edgeAdded = 0;

            var graph = new AdjacencyGraph<int, Edge<int>>(false);

            AssertNoEdge(graph);
            graph.VertexAdded += v =>
            {
                Assert.IsNotNull(v);
                ++vertexAdded;
            };
            graph.EdgeAdded += e =>
            {
                Assert.IsNotNull(e);
                ++edgeAdded;
            };

            // Edge 1, 2
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            Assert.AreEqual(2, graph.AddVerticesAndEdgeRange(new []{ edge1, edge2 }));
            Assert.AreEqual(3, vertexAdded);
            Assert.AreEqual(2, edgeAdded);
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge1, edge2 });

            // Edge 1, 3
            var edge3 = new Edge<int>(2, 3);
            Assert.AreEqual(1, graph.AddVerticesAndEdgeRange(new []{ edge1, edge3 })); // Showcase the add of only one edge
            Assert.AreEqual(3, vertexAdded);
            Assert.AreEqual(3, edgeAdded);
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge1, edge2, edge3 });
        }

        [Test]
        public void AddVerticesAndEdgeRange_Throws()
        {
            int vertexAdded = 0;
            int edgeAdded = 0;

            var graph = new AdjacencyGraph<int, Edge<int>>(false);

            AssertNoEdge(graph);
            graph.VertexAdded += v =>
            {
                Assert.IsNotNull(v);
                ++vertexAdded;
            };
            graph.EdgeAdded += e =>
            {
                Assert.IsNotNull(e);
                ++edgeAdded;
            };

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.AddVerticesAndEdgeRange(null));

            // Edge 1, 2, 3
            var edge1 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            Assert.Throws<ArgumentNullException>(() => graph.AddVerticesAndEdgeRange(new[] { edge1, null, edge3 }));
            Assert.AreEqual(0, vertexAdded);
            Assert.AreEqual(0, edgeAdded);
            AssertNoVertex(graph);
            AssertNoEdge(graph);
        }

        #endregion

        #region Contains Vertex

        [Test]
        public void ContainsVertex()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();

            var vertex1 = new TestVertex("1");
            var vertex2 = new TestVertex("2");
            var otherVertex1 = new TestVertex("1");

            Assert.IsFalse(graph.ContainsVertex(vertex1));
            Assert.IsFalse(graph.ContainsVertex(vertex2));
            Assert.IsFalse(graph.ContainsVertex(otherVertex1));

            graph.AddVertex(vertex1);
            Assert.IsTrue(graph.ContainsVertex(vertex1));
            Assert.IsFalse(graph.ContainsVertex(otherVertex1));

            graph.AddVertex(vertex2);
            Assert.IsTrue(graph.ContainsVertex(vertex2));

            graph.AddVertex(otherVertex1);
            Assert.IsTrue(graph.ContainsVertex(vertex1));
            Assert.IsTrue(graph.ContainsVertex(otherVertex1));
        }

        [Test]
        public void ContainsVertex_EquatableVertex()
        {
            var graph = new AdjacencyGraph<EquatableTestVertex, Edge<EquatableTestVertex>>();

            var vertex1 = new EquatableTestVertex("1");
            var vertex2 = new EquatableTestVertex("2");
            var otherVertex1 = new EquatableTestVertex("1");

            Assert.IsFalse(graph.ContainsVertex(vertex1));
            Assert.IsFalse(graph.ContainsVertex(vertex2));
            Assert.IsFalse(graph.ContainsVertex(otherVertex1));

            graph.AddVertex(vertex1);
            Assert.IsTrue(graph.ContainsVertex(vertex1));
            Assert.IsTrue(graph.ContainsVertex(otherVertex1));

            graph.AddVertex(vertex2);
            Assert.IsTrue(graph.ContainsVertex(vertex2));

            graph.AddVertex(otherVertex1);
            Assert.IsTrue(graph.ContainsVertex(vertex1));
            Assert.IsTrue(graph.ContainsVertex(otherVertex1));
        }

        [Test]
        public void ContainsVertex_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();

            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => graph.ContainsVertex(null));
        }

        #endregion

        #region Contains Edge

        [Test]
        public void ContainsEdge()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();

            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var otherEdge1 = new Edge<int>(1, 2);

            Assert.IsFalse(graph.ContainsEdge(edge1));
            Assert.IsFalse(graph.ContainsEdge(edge2));
            Assert.IsFalse(graph.ContainsEdge(otherEdge1));

            graph.AddVerticesAndEdge(edge1);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsFalse(graph.ContainsEdge(otherEdge1));

            graph.AddVerticesAndEdge(edge2);
            Assert.IsTrue(graph.ContainsEdge(edge2));

            graph.AddVerticesAndEdge(otherEdge1);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));
        }

        [Test]
        public void ContainsEdge_EquatableEdge()
        {
            var graph = new AdjacencyGraph<int, EquatableEdge<int>>();

            var edge1 = new EquatableEdge<int>(1, 2);
            var edge2 = new EquatableEdge<int>(1, 3);
            var otherEdge1 = new EquatableEdge<int>(1, 2);

            Assert.IsFalse(graph.ContainsEdge(edge1));
            Assert.IsFalse(graph.ContainsEdge(edge2));
            Assert.IsFalse(graph.ContainsEdge(otherEdge1));

            graph.AddVerticesAndEdge(edge1);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));

            graph.AddVerticesAndEdge(edge2);
            Assert.IsTrue(graph.ContainsEdge(edge2));

            graph.AddVerticesAndEdge(otherEdge1);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));
        }

        [Test]
        public void ContainsEdge_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();

            var vertex = new TestVertex("v1");

            // ReSharper disable AssignNullToNotNullAttribute
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => graph.ContainsEdge(null));
            Assert.Throws<ArgumentNullException>(() => graph.ContainsEdge(vertex, null));
            Assert.Throws<ArgumentNullException>(() => graph.ContainsEdge(null, vertex));
            Assert.Throws<ArgumentNullException>(() => graph.ContainsEdge(null, null));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            // ReSharper restore AssignNullToNotNullAttribute
        }

        #endregion

        #region Out Edges

        [Test]
        public void OutEdge()
        {
            var edge11 = new Edge<int>(1, 1);
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge24 = new Edge<int>(2, 4);

            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[] { edge11, edge12, edge13, edge24 });

            Assert.AreSame(edge11, graph.OutEdge(1, 0));
            Assert.AreSame(edge13, graph.OutEdge(1, 2));
            Assert.AreSame(edge24, graph.OutEdge(2, 0));
        }

        [Test]
        public void OutEdge_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            var graph1 = new AdjacencyGraph<int, Edge<int>>();
            const int vertex1 = 1;
            Assert.Throws<KeyNotFoundException>(() => graph1.OutEdge(vertex1, 0));

            graph1.AddVertex(vertex1);
            Assert.Throws<ArgumentOutOfRangeException>(() => graph1.OutEdge(vertex1, 0));

            graph1.AddEdge(new Edge<int>(1, 2));
            Assert.Throws<ArgumentOutOfRangeException>(() => graph1.OutEdge(vertex1, 5));

            var graph2 = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            Assert.Throws<ArgumentNullException>(() => graph2.OutEdge(null, 0));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void OutEdges()
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);

            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVertex(1);
            AssertNoOutEdge(graph, 1);

            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge14, edge24, edge31, edge33 });

            AssertHasOutEdge(graph, 1, new []{ edge12, edge13, edge14 });
            AssertHasOutEdge(graph, 2, new []{ edge24 });
            AssertHasOutEdge(graph, 3, new []{ edge31, edge33 });
            AssertNoOutEdge(graph, 4);
        }

        [Test]
        public void OutEdges_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            var graph1 = new AdjacencyGraph<int, Edge<int>>();
            const int vertex = 1;

            Assert.Throws<KeyNotFoundException>(() => graph1.IsOutEdgesEmpty(vertex));
            Assert.Throws<KeyNotFoundException>(() => graph1.OutDegree(vertex));
            Assert.Throws<KeyNotFoundException>(() => graph1.OutEdges(vertex));

            var graph2 = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            Assert.Throws<ArgumentNullException>(() => graph2.IsOutEdgesEmpty(null));
            Assert.Throws<ArgumentNullException>(() => graph2.OutDegree(null));
            Assert.Throws<ArgumentNullException>(() => graph2.OutEdges(null));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        #endregion

        #region Try Get Edges

        [Test]
        public void TryGetEdge()
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);

            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]{ edge1, edge2, edge3, edge4, edge5, edge6 });

            Assert.IsFalse(graph.TryGetEdge(0, 1, out Edge<int> _));

            Assert.IsTrue(graph.TryGetEdge(2, 4, out Edge<int> gotEdge));
            Assert.AreSame(edge5, gotEdge);

            Assert.IsTrue(graph.TryGetEdge(1, 2, out gotEdge));
            Assert.AreSame(edge1, gotEdge);

            Assert.IsFalse(graph.TryGetEdge(2, 1, out gotEdge));
        }

        [Test]
        public void TryGetEdge_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();

            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.TryGetEdge(null, new TestVertex("v2"), out _));
            Assert.Throws<ArgumentNullException>(() => graph.TryGetEdge(new TestVertex("v1"), null, out _));
            Assert.Throws<ArgumentNullException>(() => graph.TryGetEdge(null, null, out _));
            // ReSharper restore AssignNullToNotNullAttribute
        }

        [Test]
        public void TryGetEdges()
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);

            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });

            Assert.IsFalse(graph.TryGetEdges(0, 1, out IEnumerable<Edge<int>> _));

            Assert.IsTrue(graph.TryGetEdges(2, 4, out IEnumerable<Edge<int>> gotEdges));
            CollectionAssert.AreEqual(new[] { edge5 }, gotEdges);

            Assert.IsTrue(graph.TryGetEdges(1, 2, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge1, edge2 }, gotEdges);

            Assert.IsFalse(graph.TryGetEdges(2, 1, out gotEdges));
        }

        [Test]
        public void TryGetEdges_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();

            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.TryGetEdges(null, new TestVertex("v2"), out _));
            Assert.Throws<ArgumentNullException>(() => graph.TryGetEdges(new TestVertex("v1"), null, out _));
            Assert.Throws<ArgumentNullException>(() => graph.TryGetEdges(null, null, out _));
            // ReSharper restore AssignNullToNotNullAttribute
        }

        [Test]
        public void TryGetOutEdges()
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);

            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });

            Assert.IsFalse(graph.TryGetOutEdges(0, out IEnumerable<Edge<int>> _));

            Assert.IsTrue(graph.TryGetOutEdges(3, out IEnumerable<Edge<int>> gotEdges));
            CollectionAssert.AreEqual(new[] { edge6 }, gotEdges);

            Assert.IsTrue(graph.TryGetOutEdges(1, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge1, edge2, edge3 }, gotEdges);
        }

        [Test]
        public void TryGetOutEdges_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.TryGetOutEdges(null, out _));
        }

        #endregion

        #region Remove Vertices

        [Test]
        public void RemoveVertex()
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            var graph = new AdjacencyGraph<int, Edge<int>>();
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
            AssertHasVertices(graph, new []{ 1, 2, 4 });
            AssertHasEdges(graph, new []{ edge12, edge14, edge24 });

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

        [Test]
        public void RemoveVertex_Throws()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new AdjacencyGraph<TestVertex, Edge<TestVertex>>().RemoveVertex(null));
        }

        [Test]
        public void RemoveVertexIf()
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            var graph = new AdjacencyGraph<int, Edge<int>>();
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

        [Test]
        public void RemoveVertexIf_Throws()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new AdjacencyGraph<TestVertex, Edge<TestVertex>>().RemoveVertexIf(null));
        }

        #endregion

        #region Remove Edges

        [Test]
        public void RemoveEdge()
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            var graph = new AdjacencyGraph<int, Edge<int>>();
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
            var edge13Bis = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            var edgeNotInGraph = new Edge<int>(3, 4);
            var edgeNotEquatable = new Edge<int>(1, 2);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge13Bis, edge14, edge24, edge31, edge33 });

            Assert.IsFalse(graph.RemoveEdge(edgeNotInGraph));
            CheckCounters(0, 0);

            Assert.IsFalse(graph.RemoveEdge(edgeNotEquatable));
            CheckCounters(0, 0);

            Assert.IsTrue(graph.RemoveEdge(edge13Bis));
            CheckCounters(0, 1);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge13, edge14, edge24, edge31, edge33 });

            Assert.IsTrue(graph.RemoveEdge(edge31));
            CheckCounters(0, 1);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge13, edge14, edge24, edge33 });

            Assert.IsTrue(graph.RemoveEdge(edge12));
            Assert.IsTrue(graph.RemoveEdge(edge13));
            Assert.IsTrue(graph.RemoveEdge(edge14));
            Assert.IsTrue(graph.RemoveEdge(edge24));
            Assert.IsTrue(graph.RemoveEdge(edge33));
            CheckCounters(0, 5);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertNoEdge(graph);

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

        [Test]
        public void RemoveEdge_EquatableEdge()
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            var graph = new AdjacencyGraph<int, EquatableEdge<int>>();
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

            var edge12 = new EquatableEdge<int>(1, 2);
            var edge13 = new EquatableEdge<int>(1, 3);
            var edge13Bis = new EquatableEdge<int>(1, 3);
            var edge14 = new EquatableEdge<int>(1, 4);
            var edge24 = new EquatableEdge<int>(2, 4);
            var edge31 = new EquatableEdge<int>(3, 1);
            var edge33 = new EquatableEdge<int>(3, 3);
            var edgeNotInGraph = new EquatableEdge<int>(3, 4);
            var edgeEquatable = new EquatableEdge<int>(1, 2);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge13Bis, edge14, edge24, edge31, edge33 });

            Assert.IsFalse(graph.RemoveEdge(edgeNotInGraph));
            CheckCounters(0, 0);

            Assert.IsTrue(graph.RemoveEdge(edgeEquatable));
            CheckCounters(0, 1);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge13, edge13Bis, edge14, edge24, edge31, edge33 });

            Assert.IsTrue(graph.RemoveEdge(edge13Bis));
            CheckCounters(0, 1);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge13, edge14, edge24, edge31, edge33 });

            Assert.IsTrue(graph.RemoveEdge(edge31));
            CheckCounters(0, 1);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge13, edge14, edge24, edge33 });

            Assert.IsTrue(graph.RemoveEdge(edge13));
            Assert.IsTrue(graph.RemoveEdge(edge14));
            Assert.IsTrue(graph.RemoveEdge(edge24));
            Assert.IsTrue(graph.RemoveEdge(edge33));
            CheckCounters(0, 4);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertNoEdge(graph);

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

        [Test]
        public void RemoveEdge_Throws()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new AdjacencyGraph<TestVertex, Edge<TestVertex>>().RemoveEdge(null));
        }

        [Test]
        public void RemoveEdgeIf()
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            var graph = new AdjacencyGraph<int, Edge<int>>();
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
            var edge13Bis = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge13Bis, edge14, edge24, edge31, edge33 });

            Assert.AreEqual(0, graph.RemoveEdgeIf(edge => edge.Target == 5));
            CheckCounters(0, 0);

            Assert.AreEqual(2, graph.RemoveEdgeIf(edge => edge.Source == 3));
            CheckCounters(0, 2);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge13, edge13Bis, edge14, edge24 });

            Assert.AreEqual(5, graph.RemoveEdgeIf(edge => true));
            CheckCounters(0, 5);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertNoEdge(graph);

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

        [Test]
        public void RemoveEdgeIf_Throws()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new AdjacencyGraph<TestVertex, Edge<TestVertex>>().RemoveEdgeIf(null));
        }

        [Test]
        public void RemoveOutEdgeIf()
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            var graph = new AdjacencyGraph<int, Edge<int>>();
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
            var edge13Bis = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge13Bis, edge14, edge24, edge31, edge33 });

            Assert.AreEqual(3, graph.RemoveOutEdgeIf(1, edge => edge.Target >= 3));
            CheckCounters(0, 3);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge24, edge31, edge33 });

            Assert.AreEqual(0, graph.RemoveOutEdgeIf(3, edge => edge.Target > 5));
            CheckCounters(0, 0);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge24, edge31, edge33 });

            Assert.AreEqual(2, graph.RemoveOutEdgeIf(3, edge => true));
            CheckCounters(0, 2);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge12, edge24 });

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

        [Test]
        public void RemoveOutEdgeIf_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            Assert.Throws<ArgumentNullException>(() => graph.RemoveOutEdgeIf(null, edge => true));
            Assert.Throws<ArgumentNullException>(() => graph.RemoveOutEdgeIf(new TestVertex("v1"), null));
            Assert.Throws<ArgumentNullException>(() => graph.RemoveOutEdgeIf(null, null));
            Assert.Throws<KeyNotFoundException>(() => graph.RemoveOutEdgeIf(new TestVertex("v1"), edge => true));
        }

        #endregion

        #region Clear

        [Test]
        public void Clear()
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.VertexRemoved += v =>
            {
                Assert.IsNotNull(v);
                ++verticesRemoved;
            };
            graph.EdgeRemoved += e =>
            {
                Assert.IsNotNull(e);
                ++edgesRemoved;
            };

            AssertEmptyGraph(graph);

            graph.Clear();

            AssertEmptyGraph(graph);
            Assert.AreEqual(0, verticesRemoved);
            Assert.AreEqual(0, edgesRemoved);

            graph.AddVerticesAndEdge(new Edge<int>(1, 2));
            graph.AddVerticesAndEdge(new Edge<int>(2, 3));
            graph.AddVerticesAndEdge(new Edge<int>(3, 1));

            graph.Clear();

            AssertEmptyGraph(graph);
            Assert.AreEqual(3, verticesRemoved);
            Assert.AreEqual(3, edgesRemoved);
        }

        [Test]
        public void ClearOutEdges()
        {
            int edgesRemoved = 0;

            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.EdgeRemoved += e =>
            {
                Assert.IsNotNull(e);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            AssertEmptyGraph(graph);

            var edge12 = new Edge<int>(1, 2);
            var edge23 = new Edge<int>(2, 3);
            graph.AddVerticesAndEdge(edge12);
            graph.AddVerticesAndEdge(edge23);

            // Clear out 1
            graph.ClearOutEdges(1);

            AssertHasEdges(graph, new []{ edge23 });
            CheckCounter(1);

            var edge13 = new Edge<int>(1, 3);
            var edge31 = new Edge<int>(3, 1);
            var edge32 = new Edge<int>(3, 2);
            graph.AddVerticesAndEdge(edge12);
            graph.AddVerticesAndEdge(edge13);
            graph.AddVerticesAndEdge(edge31);
            graph.AddVerticesAndEdge(edge32);

            // Clear out 3
            graph.ClearOutEdges(3);

            AssertHasEdges(graph, new[] { edge12, edge13, edge23 });
            CheckCounter(2);

            // Clear out 1
            graph.ClearOutEdges(1);

            AssertHasEdges(graph, new []{ edge23 });
            CheckCounter(2);

            // Clear out 2 = Clear
            graph.ClearOutEdges(2);

            AssertNoEdge(graph);
            CheckCounter(1);

            #region Local function

            void CheckCounter(int expectedRemovedEdges)
            {
                Assert.AreEqual(expectedRemovedEdges, edgesRemoved);
                edgesRemoved = 0;
            }

            #endregion
        }

        [Test]
        public void ClearOutEdges_Throws()
        {
            Assert.Throws<KeyNotFoundException>(() => new AdjacencyGraph<int, Edge<int>>().ClearOutEdges(1));
            Assert.Throws<ArgumentNullException>(() => new AdjacencyGraph<TestVertex, Edge<TestVertex>>().ClearOutEdges(null));
        }

        #endregion

        [Test]
        public void Clone()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();

            var clonedGraph = graph.Clone();
            Assert.IsNotNull(clonedGraph);

            AssertEmptyGraph(graph);
            AssertEmptyGraph(clonedGraph);


            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 3);
            graph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3 });

            clonedGraph = graph.Clone();
            Assert.IsNotNull(clonedGraph);

            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge1, edge2, edge3 });

            AssertHasVertices(clonedGraph, new []{ 1, 2, 3 });
            AssertHasEdges(clonedGraph, new []{ edge1, edge2, edge3 });
        }

        [Test]
        public void TrimEdgeExcess()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>(true, 12, 50);
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(1, 3),
                new Edge<int>(1, 4)
            });

            Assert.DoesNotThrow(() => graph.TrimEdgeExcess());
        }
    }
}
