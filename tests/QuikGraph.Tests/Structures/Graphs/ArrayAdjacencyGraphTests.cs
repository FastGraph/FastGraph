using System;
using System.Collections.Generic;
using NUnit.Framework;
using static QuikGraph.Tests.AssertHelpers;

namespace QuikGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="ArrayAdjacencyGraph{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class ArrayAdjacencyGraphTests : GraphTestsBase
    {
        [Test]
        public void Construction()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();

            var graph = new ArrayAdjacencyGraph<int, Edge<int>>(wrappedGraph);
            AssertGraphProperties(graph);
            AssertNoVertex(graph);
            AssertNoEdge(graph);

            wrappedGraph.AddVertexRange(new[] { 2, 3, 1 });
            graph = new ArrayAdjacencyGraph<int, Edge<int>>(wrappedGraph);
            AssertGraphProperties(graph);
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertNoEdge(graph);

            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(2, 2);
            var edge3 = new Edge<int>(3, 4);
            var edge4 = new Edge<int>(1, 4);
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4 });
            graph = new ArrayAdjacencyGraph<int, Edge<int>>(wrappedGraph);
            AssertGraphProperties(graph);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge1, edge2, edge3, edge4 });

            #region Local function

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            void AssertGraphProperties<TVertex, TEdge>(ArrayAdjacencyGraph<TVertex, TEdge> g)
                where TEdge : IEdge<TVertex>
            {
                Assert.IsTrue(g.IsDirected);
                Assert.IsTrue(g.AllowParallelEdges);
            }

            #endregion
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new ArrayAdjacencyGraph<int, Edge<int>>(null));
        }

        #region Contains Vertex

        [Test]
        public void ContainsVertex()
        {
            var wrappedGraph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ArrayAdjacencyGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);

            var vertex1 = new TestVertex("1");
            var vertex2 = new TestVertex("2");
            var otherVertex1 = new TestVertex("1");

            Assert.IsFalse(graph.ContainsVertex(vertex1));
            Assert.IsFalse(graph.ContainsVertex(vertex2));
            Assert.IsFalse(graph.ContainsVertex(otherVertex1));

            wrappedGraph.AddVertex(vertex1);
            graph = new ArrayAdjacencyGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            Assert.IsTrue(graph.ContainsVertex(vertex1));
            Assert.IsFalse(graph.ContainsVertex(otherVertex1));

            wrappedGraph.AddVertex(vertex2);
            graph = new ArrayAdjacencyGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            Assert.IsTrue(graph.ContainsVertex(vertex2));

            wrappedGraph.AddVertex(otherVertex1);
            graph = new ArrayAdjacencyGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            Assert.IsTrue(graph.ContainsVertex(vertex1));
            Assert.IsTrue(graph.ContainsVertex(otherVertex1));
        }

        [Test]
        public void ContainsVertex_EquatableVertex()
        {
            var wrappedGraph = new AdjacencyGraph<EquatableTestVertex, Edge<EquatableTestVertex>>();
            var graph = new ArrayAdjacencyGraph<EquatableTestVertex, Edge<EquatableTestVertex>>(wrappedGraph);

            var vertex1 = new EquatableTestVertex("1");
            var vertex2 = new EquatableTestVertex("2");
            var otherVertex1 = new EquatableTestVertex("1");

            Assert.IsFalse(graph.ContainsVertex(vertex1));
            Assert.IsFalse(graph.ContainsVertex(vertex2));
            Assert.IsFalse(graph.ContainsVertex(otherVertex1));

            wrappedGraph.AddVertex(vertex1);
            graph = new ArrayAdjacencyGraph<EquatableTestVertex, Edge<EquatableTestVertex>>(wrappedGraph);
            Assert.IsTrue(graph.ContainsVertex(vertex1));
            Assert.IsTrue(graph.ContainsVertex(otherVertex1));

            wrappedGraph.AddVertex(vertex2);
            graph = new ArrayAdjacencyGraph<EquatableTestVertex, Edge<EquatableTestVertex>>(wrappedGraph);
            Assert.IsTrue(graph.ContainsVertex(vertex2));

            wrappedGraph.AddVertex(otherVertex1);
            graph = new ArrayAdjacencyGraph<EquatableTestVertex, Edge<EquatableTestVertex>>(wrappedGraph);
            Assert.IsTrue(graph.ContainsVertex(vertex1));
            Assert.IsTrue(graph.ContainsVertex(otherVertex1));
        }

        [Test]
        public void ContainsVertex_Throws()
        {
            var wrappedGraph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ArrayAdjacencyGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            ContainsVertex_Throws_Test(graph);
        }

        #endregion

        #region Contains Edge

        [Test]
        public void ContainsEdge()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();
            var graph = new ArrayAdjacencyGraph<int, Edge<int>>(wrappedGraph);

            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var otherEdge1 = new Edge<int>(1, 2);

            Assert.IsFalse(graph.ContainsEdge(edge1));
            Assert.IsFalse(graph.ContainsEdge(edge2));
            Assert.IsFalse(graph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge1);
            graph = new ArrayAdjacencyGraph<int, Edge<int>>(wrappedGraph);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsFalse(graph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge2);
            graph = new ArrayAdjacencyGraph<int, Edge<int>>(wrappedGraph);
            Assert.IsTrue(graph.ContainsEdge(edge2));

            wrappedGraph.AddVerticesAndEdge(otherEdge1);
            graph = new ArrayAdjacencyGraph<int, Edge<int>>(wrappedGraph);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));
        }

        [Test]
        public void ContainsEdge_EquatableEdge()
        {
            var wrappedGraph = new AdjacencyGraph<int, EquatableEdge<int>>();
            var graph = new ArrayAdjacencyGraph<int, EquatableEdge<int>>(wrappedGraph);

            var edge1 = new EquatableEdge<int>(1, 2);
            var edge2 = new EquatableEdge<int>(1, 3);
            var otherEdge1 = new EquatableEdge<int>(1, 2);

            Assert.IsFalse(graph.ContainsEdge(edge1));
            Assert.IsFalse(graph.ContainsEdge(edge2));
            Assert.IsFalse(graph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge1);
            graph = new ArrayAdjacencyGraph<int, EquatableEdge<int>>(wrappedGraph);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge2);
            graph = new ArrayAdjacencyGraph<int, EquatableEdge<int>>(wrappedGraph);
            Assert.IsTrue(graph.ContainsEdge(edge2));

            wrappedGraph.AddVerticesAndEdge(otherEdge1);
            graph = new ArrayAdjacencyGraph<int, EquatableEdge<int>>(wrappedGraph);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));
        }

        [Test]
        public void ContainsEdge_SourceTarget()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();
            var graph = new ArrayAdjacencyGraph<int, Edge<int>>(wrappedGraph);

            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);

            Assert.IsFalse(graph.ContainsEdge(1, 2));
            Assert.IsFalse(graph.ContainsEdge(2, 1));

            wrappedGraph.AddVerticesAndEdge(edge1);
            graph = new ArrayAdjacencyGraph<int, Edge<int>>(wrappedGraph);
            Assert.IsTrue(graph.ContainsEdge(1, 2));
            Assert.IsFalse(graph.ContainsEdge(2, 1));

            wrappedGraph.AddVerticesAndEdge(edge2);
            graph = new ArrayAdjacencyGraph<int, Edge<int>>(wrappedGraph);
            Assert.IsTrue(graph.ContainsEdge(1, 3));
            Assert.IsFalse(graph.ContainsEdge(3, 1));
        }

        [Test]
        public void ContainsEdge_Throws()
        {
            var wrappedGraph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ArrayAdjacencyGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            ContainsEdge_Throws_Test(graph);
            ContainsEdge_SourceTarget_Throws_Test(graph);
        }

        #endregion

        #region Out Edges

        [Test]
        public void OutEdge()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();
            var edge11 = new Edge<int>(1, 1);
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge24 = new Edge<int>(2, 4);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge11, edge12, edge13, edge24 });
            var graph = new ArrayAdjacencyGraph<int, Edge<int>>(wrappedGraph);

            Assert.AreSame(edge11, graph.OutEdge(1, 0));
            Assert.AreSame(edge13, graph.OutEdge(1, 2));
            Assert.AreSame(edge24, graph.OutEdge(2, 0));
        }

        [Test]
        public void OutEdge_Throws()
        {
            var wrappedGraph1 = new AdjacencyGraph<int, Edge<int>>();
            var graph1 = new ArrayAdjacencyGraph<int, Edge<int>>(wrappedGraph1);

            const int vertex1 = 1;
            const int vertex2 = 2;

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<KeyNotFoundException>(() => graph1.OutEdge(vertex1, 0));

            wrappedGraph1.AddVertex(vertex1);
            wrappedGraph1.AddVertex(vertex2);
            graph1 = new ArrayAdjacencyGraph<int, Edge<int>>(wrappedGraph1);
            AssertIndexOutOfRange(() => graph1.OutEdge(vertex1, 0));

            wrappedGraph1.AddEdge(new Edge<int>(1, 2));
            graph1 = new ArrayAdjacencyGraph<int, Edge<int>>(wrappedGraph1);
            AssertIndexOutOfRange(() => graph1.OutEdge(vertex1, 5));


            var wrappedGraph2 = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph2 = new ArrayAdjacencyGraph<TestVertex, Edge<TestVertex>>(wrappedGraph2);
            // ReSharper disable once AssignNullToNotNullAttribute
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

            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();
            var graph = new ArrayAdjacencyGraph<int, Edge<int>>(wrappedGraph);

            AssertNoOutEdge(graph, 1);

            wrappedGraph.AddVertex(1);
            graph = new ArrayAdjacencyGraph<int, Edge<int>>(wrappedGraph);
            AssertNoOutEdge(graph, 1);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge14, edge24, edge31, edge33 });
            graph = new ArrayAdjacencyGraph<int, Edge<int>>(wrappedGraph);

            AssertHasOutEdges(graph, 1, new[] { edge12, edge13, edge14 });
            AssertHasOutEdges(graph, 2, new[] { edge24 });
            AssertHasOutEdges(graph, 3, new[] { edge31, edge33 });
            AssertNoOutEdge(graph, 4);
        }

        [Test]
        public void OutEdges_Throws()
        {
            var wrappedGraph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ArrayAdjacencyGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            OutEdges_Throws_Test(graph);
        }

        #endregion

        #region Try Get Edges

        [Test]
        public void TryGetEdge()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();

            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });
            var graph = new ArrayAdjacencyGraph<int, Edge<int>>(wrappedGraph);

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
            var wrappedGraph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ArrayAdjacencyGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            TryGetEdge_Throws_Test(graph);
        }

        [Test]
        public void TryGetEdges()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();

            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });
            var graph = new ArrayAdjacencyGraph<int, Edge<int>>(wrappedGraph);

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
            var wrappedGraph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ArrayAdjacencyGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            TryGetEdges_Throws_Test(graph);
        }

        [Test]
        public void TryGetOutEdges()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();

            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);

            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3, edge4, edge5, edge6 });
            var graph = new ArrayAdjacencyGraph<int, Edge<int>>(wrappedGraph);

            Assert.IsFalse(graph.TryGetOutEdges(0, out IEnumerable<Edge<int>> _));

            Assert.IsTrue(graph.TryGetOutEdges(3, out IEnumerable<Edge<int>> gotEdges));
            CollectionAssert.AreEqual(new[] { edge6 }, gotEdges);

            Assert.IsTrue(graph.TryGetOutEdges(1, out gotEdges));
            CollectionAssert.AreEqual(new[] { edge1, edge2, edge3 }, gotEdges);
        }

        [Test]
        public void TryGetOutEdges_Throws()
        {
            var wrappedGraph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ArrayAdjacencyGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            TryGetOutEdges_Throws_Test(graph);
        }

        #endregion

        [Test]
        public void Clone()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();

            var graph = new ArrayAdjacencyGraph<int, Edge<int>>(wrappedGraph);
            AssertEmptyGraph(graph);

            var clonedGraph = graph.Clone();
            Assert.IsNotNull(clonedGraph);
            AssertEmptyGraph(clonedGraph);

            clonedGraph = (ArrayAdjacencyGraph<int, Edge<int>>)((ICloneable)graph).Clone();
            Assert.IsNotNull(clonedGraph);
            AssertEmptyGraph(clonedGraph);

            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 3);
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3 });

            graph = new ArrayAdjacencyGraph<int, Edge<int>>(wrappedGraph);
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge1, edge2, edge3 });

            clonedGraph = graph.Clone();
            Assert.IsNotNull(clonedGraph);
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3 });
            AssertHasEdges(clonedGraph, new[] { edge1, edge2, edge3 });

            clonedGraph = (ArrayAdjacencyGraph<int, Edge<int>>)((ICloneable)graph).Clone();
            Assert.IsNotNull(clonedGraph);
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3 });
            AssertHasEdges(clonedGraph, new[] { edge1, edge2, edge3 });
        }
    }
}
