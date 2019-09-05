using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NUnit.Framework;

namespace QuikGraph.Tests.Structures
{
    /// <summary>
    /// Base class for graph tests.
    /// </summary>
    [TestFixture]
    internal partial class GraphTestsBase
    {
        #region Vertices helpers

        protected static void AssertNoVertex<TVertex>([NotNull] IVertexSet<TVertex> graph)
        {
            Assert.IsTrue(graph.IsVerticesEmpty);
            Assert.AreEqual(0, graph.VertexCount);
            CollectionAssert.IsEmpty(graph.Vertices);
        }

        protected static void AssertHasVertices<TVertex>(
            [NotNull] IVertexSet<TVertex> graph,
            [NotNull, ItemNotNull] IEnumerable<TVertex> vertices)
        {
            TVertex[] vertexArray = vertices.ToArray();
            CollectionAssert.IsNotEmpty(vertexArray);

            Assert.IsFalse(graph.IsVerticesEmpty);
            Assert.AreEqual(vertexArray.Length, graph.VertexCount);
            CollectionAssert.AreEquivalent(vertexArray, graph.Vertices);
        }

        #endregion

        #region Edges helpers

        protected static void AssertNoEdge<TVertex, TEdge>([NotNull] IEdgeSet<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            Assert.IsTrue(graph.IsEdgesEmpty);
            Assert.AreEqual(0, graph.EdgeCount);
            CollectionAssert.IsEmpty(graph.Edges);
        }

        protected static void AssertHasEdges<TVertex, TEdge>(
            [NotNull] IEdgeSet<TVertex, TEdge> graph,
            [NotNull, ItemNotNull] IEnumerable<TEdge> edges)
            where TEdge : IEdge<TVertex>
        {
            TEdge[] edgeArray = edges.ToArray();
            CollectionAssert.IsNotEmpty(edgeArray);

            Assert.IsFalse(graph.IsEdgesEmpty);
            Assert.AreEqual(edgeArray.Length, graph.EdgeCount);
            CollectionAssert.AreEquivalent(edgeArray, graph.Edges);
        }

        protected static void AssertHasEdges<TVertex, TEdge>(
            [NotNull] IEdgeSet<TVertex, SReversedEdge<TVertex, TEdge>> graph,
            [NotNull, ItemNotNull] IEnumerable<TEdge> edges)
            where TEdge : IEdge<TVertex>
        {
            AssertHasEdges(
                graph,
                edges.Select(edge => new SReversedEdge<TVertex, TEdge>(edge)));
        }

        protected static void AssertSameReversedEdge(
            [NotNull] Edge<int> edge,
            SReversedEdge<int, Edge<int>> reversedEdge)
        {
            Assert.AreEqual(new SReversedEdge<int, Edge<int>>(edge), reversedEdge);
            Assert.AreSame(edge, reversedEdge.OriginalEdge);
        }

        protected static void AssertSameReversedEdges(
            [NotNull, ItemNotNull] IEnumerable<Edge<int>> edges,
            [NotNull] IEnumerable<SReversedEdge<int, Edge<int>>> reversedEdges)
        {
            var edgesArray = edges.ToArray();
            var reversedEdgesArray = reversedEdges.ToArray();
            Assert.AreEqual(edgesArray.Length, reversedEdgesArray.Length);
            for (int i = 0; i < edgesArray.Length; ++i)
                AssertSameReversedEdge(edgesArray[i], reversedEdgesArray[i]);
        }

        #endregion

        #region Graph helpers

        protected static void AssertEmptyGraph<TVertex, TEdge>([NotNull] IEdgeListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            AssertNoVertex(graph);
            AssertNoEdge(graph);
        }

        protected static void AssertNoInEdge<TVertex, TEdge>([NotNull] IBidirectionalIncidenceGraph<TVertex, TEdge> graph, [NotNull] TVertex vertex)
            where TEdge : IEdge<TVertex>
        {
            Assert.IsTrue(graph.IsInEdgesEmpty(vertex));
            Assert.AreEqual(0, graph.InDegree(vertex));
            CollectionAssert.IsEmpty(graph.InEdges(vertex));
        }

        protected static void AssertHasInEdges<TVertex, TEdge>(
            [NotNull] IBidirectionalIncidenceGraph<TVertex, TEdge> graph,
            [NotNull] TVertex vertex,
            [NotNull, ItemNotNull] IEnumerable<TEdge> edges)
            where TEdge : IEdge<TVertex>
        {
            TEdge[] edgeArray = edges.ToArray();
            CollectionAssert.IsNotEmpty(edgeArray);

            Assert.IsFalse(graph.IsInEdgesEmpty(vertex));
            Assert.AreEqual(edgeArray.Length, graph.InDegree(vertex));
            CollectionAssert.AreEquivalent(edgeArray, graph.InEdges(vertex));
        }

        protected static void AssertHasReversedInEdges<TVertex, TEdge>(
            [NotNull] IBidirectionalIncidenceGraph<TVertex, SReversedEdge<TVertex, TEdge>> graph,
            [NotNull] TVertex vertex,
            [NotNull, ItemNotNull] IEnumerable<TEdge> edges)
            where TEdge : IEdge<TVertex>
        {
            TEdge[] edgeArray = edges.ToArray();
            CollectionAssert.IsNotEmpty(edgeArray);

            Assert.IsFalse(graph.IsInEdgesEmpty(vertex));
            Assert.AreEqual(edgeArray.Length, graph.InDegree(vertex));
            CollectionAssert.AreEquivalent(
                edgeArray.Select(edge => new SReversedEdge<TVertex, TEdge>(edge)),
                graph.InEdges(vertex));
        }

        protected static void AssertNoOutEdge<TVertex, TEdge>([NotNull] IImplicitGraph<TVertex, TEdge> graph, [NotNull] TVertex vertex)
            where TEdge : IEdge<TVertex>
        {
            Assert.IsTrue(graph.IsOutEdgesEmpty(vertex));
            Assert.AreEqual(0, graph.OutDegree(vertex));
            CollectionAssert.IsEmpty(graph.OutEdges(vertex));
        }

        protected static void AssertHasOutEdges<TVertex, TEdge>(
            [NotNull] IImplicitGraph<TVertex, TEdge> graph,
            [NotNull] TVertex vertex,
            [NotNull, ItemNotNull] IEnumerable<TEdge> edges)
            where TEdge : IEdge<TVertex>
        {
            TEdge[] edgeArray = edges.ToArray();
            CollectionAssert.IsNotEmpty(edgeArray);

            Assert.IsFalse(graph.IsOutEdgesEmpty(vertex));
            Assert.AreEqual(edgeArray.Length, graph.OutDegree(vertex));
            CollectionAssert.AreEquivalent(edgeArray, graph.OutEdges(vertex));
        }

        protected static void AssertHasReversedOutEdges<TVertex, TEdge>(
            [NotNull] IImplicitGraph<TVertex, SReversedEdge<TVertex, TEdge>> graph,
            [NotNull] TVertex vertex,
            [NotNull, ItemNotNull] IEnumerable<TEdge> edges)
            where TEdge : IEdge<TVertex>
        {
            TEdge[] edgeArray = edges.ToArray();
            CollectionAssert.IsNotEmpty(edgeArray);

            Assert.IsFalse(graph.IsOutEdgesEmpty(vertex));
            Assert.AreEqual(edgeArray.Length, graph.OutDegree(vertex));
            CollectionAssert.AreEquivalent(
                edgeArray.Select(edge => new SReversedEdge<TVertex, TEdge>(edge)),
                graph.OutEdges(vertex));
        }

        protected static void AssertNoAdjacentEdge<TVertex, TEdge>([NotNull] IImplicitUndirectedGraph<TVertex, TEdge> graph, [NotNull] TVertex vertex)
            where TEdge : IEdge<TVertex>
        {
            Assert.IsTrue(graph.IsAdjacentEdgesEmpty(vertex));
            Assert.AreEqual(0, graph.AdjacentDegree(vertex));
            CollectionAssert.IsEmpty(graph.AdjacentEdges(vertex));
        }

        protected static void AssertHasAdjacentEdges<TVertex, TEdge>(
            [NotNull] IImplicitUndirectedGraph<TVertex, TEdge> graph,
            [NotNull] TVertex vertex,
            [NotNull, ItemNotNull] IEnumerable<TEdge> edges,
            int degree = -1)    // If not set => equals the count of edges
            where TEdge : IEdge<TVertex>
        {
            TEdge[] edgeArray = edges.ToArray();
            CollectionAssert.IsNotEmpty(edgeArray);

            Assert.IsFalse(graph.IsAdjacentEdgesEmpty(vertex));
            Assert.AreEqual(
                degree < 0 ? edgeArray.Length : degree,
                graph.AdjacentDegree(vertex));
            CollectionAssert.AreEquivalent(edgeArray, graph.AdjacentEdges(vertex));
        }

        #endregion
    }
}
