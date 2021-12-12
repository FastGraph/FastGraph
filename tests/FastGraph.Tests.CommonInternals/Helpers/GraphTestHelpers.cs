#nullable enable

using JetBrains.Annotations;
using NUnit.Framework;

namespace FastGraph.Tests
{
    /// <summary>
    /// Test helpers for graphs.
    /// </summary>
    internal static class GraphTestHelpers
    {
        public static void AssertVertexCountEqual<TVertex>(
            this IVertexSet<TVertex> left,
            IVertexSet<TVertex> right)
            where TVertex : notnull
        {
            Assert.IsNotNull(left);
            Assert.IsNotNull(right);
            Assert.AreEqual(left.VertexCount, right.VertexCount);
        }

        public static void AssertEdgeCountEqual<TVertex, TEdge>(
            this IEdgeSet<TVertex, TEdge> left,
            IEdgeSet<TVertex, TEdge> right)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            Assert.IsNotNull(left);
            Assert.IsNotNull(right);
            Assert.AreEqual(left.EdgeCount, right.EdgeCount);
        }

        public static bool InVertexSet<TVertex>(
            IVertexSet<TVertex> graph,
            TVertex vertex)
            where TVertex : notnull
        {
            return graph.ContainsVertex(vertex);
        }

        public static bool InVertexSet<TVertex, TEdge>(
            IEdgeListGraph<TVertex, TEdge> graph,
            TEdge edge)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            return InVertexSet(graph, edge.Source)
                   && InVertexSet(graph, edge.Target);
        }

        public static bool InEdgeSet<TVertex, TEdge>(
            IEdgeListGraph<TVertex, TEdge> graph,
            TEdge edge)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            return InVertexSet(graph, edge) && graph.ContainsEdge(edge);
        }

        [Pure]
        public static bool IsDescendant<TValue>(
            Dictionary<TValue, TValue> parents,
            TValue u,
            TValue v)
            where TValue : notnull
        {
            TValue t;
            TValue current = u;
            do
            {
                t = current;
                current = parents[t];
                if (current.Equals(v))
                    return true;
            }
            while (!t.Equals(current));

            return false;
        }

        #region Vertices helpers

        public static void AssertNoVertex<TVertex>(IVertexSet<TVertex> graph)
            where TVertex : notnull
        {
            Assert.IsTrue(graph.IsVerticesEmpty);
            Assert.AreEqual(0, graph.VertexCount);
            CollectionAssert.IsEmpty(graph.Vertices);
        }

        public static void AssertHasVertices<TVertex>(
            IVertexSet<TVertex> graph,
            IEnumerable<TVertex> vertices)
            where TVertex : notnull
        {
            TVertex[] vertexArray = vertices.ToArray();
            CollectionAssert.IsNotEmpty(vertexArray);

            Assert.IsFalse(graph.IsVerticesEmpty);
            Assert.AreEqual(vertexArray.Length, graph.VertexCount);
            CollectionAssert.AreEquivalent(vertexArray, graph.Vertices);
        }

        public static void AssertNoVertices<TVertex>(
            IImplicitVertexSet<TVertex> graph,
            IEnumerable<TVertex> vertices)
            where TVertex : notnull
        {
            AssertImplicitHasVertices(graph, vertices, false);
        }

        public static void AssertHasVertices<TVertex>(
            IImplicitVertexSet<TVertex> graph,
            IEnumerable<TVertex> vertices)
            where TVertex : notnull
        {
            AssertImplicitHasVertices(graph, vertices, true);
        }

        private static void AssertImplicitHasVertices<TVertex>(
            IImplicitVertexSet<TVertex> graph,
            IEnumerable<TVertex> vertices,
            bool expectedContains)
            where TVertex : notnull
        {
            TVertex[] vertexArray = vertices.ToArray();
            CollectionAssert.IsNotEmpty(vertexArray);

            foreach (TVertex vertex in vertexArray)
            {
                Assert.AreEqual(expectedContains, graph.ContainsVertex(vertex));
            }
        }

        #endregion

        #region Edges helpers

        public static void AssertNoEdge<TVertex, TEdge>(IEdgeSet<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            Assert.IsTrue(graph.IsEdgesEmpty);
            Assert.AreEqual(0, graph.EdgeCount);
            CollectionAssert.IsEmpty(graph.Edges);
        }

        public static void AssertHasEdges<TVertex, TEdge>(
            IEdgeSet<TVertex, TEdge> graph,
            IEnumerable<TEdge> edges)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            TEdge[] edgeArray = edges.ToArray();
            CollectionAssert.IsNotEmpty(edgeArray);

            Assert.IsFalse(graph.IsEdgesEmpty);
            Assert.AreEqual(edgeArray.Length, graph.EdgeCount);
            CollectionAssert.AreEquivalent(edgeArray, graph.Edges);
        }

        public static void AssertHasEdges<TVertex, TEdge>(
            IEdgeSet<TVertex, SReversedEdge<TVertex, TEdge>> graph,
            IEnumerable<TEdge> edges)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            AssertHasEdges(
                graph,
                edges.Select(edge => new SReversedEdge<TVertex, TEdge>(edge)));
        }

        public static void AssertSameReversedEdge(
            Edge<int> edge,
            SReversedEdge<int, Edge<int>> reversedEdge)
        {
            Assert.AreEqual(new SReversedEdge<int, Edge<int>>(edge), reversedEdge);
            Assert.AreSame(edge, reversedEdge.OriginalEdge);
        }

        public static void AssertSameReversedEdges(
            IEnumerable<Edge<int>> edges,
            IEnumerable<SReversedEdge<int, Edge<int>>> reversedEdges)
        {
            var edgesArray = edges.ToArray();
            var reversedEdgesArray = reversedEdges.ToArray();
            Assert.AreEqual(edgesArray.Length, reversedEdgesArray.Length);
            for (int i = 0; i < edgesArray.Length; ++i)
                AssertSameReversedEdge(edgesArray[i], reversedEdgesArray[i]);
        }

        #endregion

        #region Graph helpers

        public static void AssertEmptyGraph<TVertex, TEdge>(
            IEdgeListGraph<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            AssertNoVertex(graph);
            AssertNoEdge(graph);
        }

        public static void AssertEmptyGraph<TVertex>(
            CompressedSparseRowGraph<TVertex> graph)
            where TVertex : notnull
        {
            AssertNoVertex(graph);
            AssertNoEdge(graph);
        }

        public static void AssertNoInEdge<TVertex, TEdge>(IBidirectionalIncidenceGraph<TVertex, TEdge> graph, TVertex vertex)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            Assert.IsTrue(graph.IsInEdgesEmpty(vertex));
            Assert.AreEqual(0, graph.InDegree(vertex));
            CollectionAssert.IsEmpty(graph.InEdges(vertex));
        }

        public static void AssertHasInEdges<TVertex, TEdge>(
            IBidirectionalIncidenceGraph<TVertex, TEdge> graph,
            TVertex vertex,
            IEnumerable<TEdge> edges)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            TEdge[] edgeArray = edges.ToArray();
            CollectionAssert.IsNotEmpty(edgeArray);

            Assert.IsFalse(graph.IsInEdgesEmpty(vertex));
            Assert.AreEqual(edgeArray.Length, graph.InDegree(vertex));
            CollectionAssert.AreEquivalent(edgeArray, graph.InEdges(vertex));
        }

        public static void AssertHasReversedInEdges<TVertex, TEdge>(
            IBidirectionalIncidenceGraph<TVertex, SReversedEdge<TVertex, TEdge>> graph,
            TVertex vertex,
            IEnumerable<TEdge> edges)
            where TVertex : notnull
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

        public static void AssertNoOutEdge<TVertex, TEdge>(IImplicitGraph<TVertex, TEdge> graph, TVertex vertex)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            Assert.IsTrue(graph.IsOutEdgesEmpty(vertex));
            Assert.AreEqual(0, graph.OutDegree(vertex));
            CollectionAssert.IsEmpty(graph.OutEdges(vertex));
        }

        public static void AssertHasOutEdges<TVertex, TEdge>(
            IImplicitGraph<TVertex, TEdge> graph,
            TVertex vertex,
            IEnumerable<TEdge> edges)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            TEdge[] edgeArray = edges.ToArray();
            CollectionAssert.IsNotEmpty(edgeArray);

            Assert.IsFalse(graph.IsOutEdgesEmpty(vertex));
            Assert.AreEqual(edgeArray.Length, graph.OutDegree(vertex));
            CollectionAssert.AreEquivalent(edgeArray, graph.OutEdges(vertex));
        }

        public static void AssertHasReversedOutEdges<TVertex, TEdge>(
            IImplicitGraph<TVertex, SReversedEdge<TVertex, TEdge>> graph,
            TVertex vertex,
            IEnumerable<TEdge> edges)
            where TVertex : notnull
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



        public static void AssertNoAdjacentEdge<TVertex, TEdge>(IImplicitUndirectedGraph<TVertex, TEdge> graph, TVertex vertex)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            Assert.IsTrue(graph.IsAdjacentEdgesEmpty(vertex));
            Assert.AreEqual(0, graph.AdjacentDegree(vertex));
            CollectionAssert.IsEmpty(graph.AdjacentEdges(vertex));
        }

        public static void AssertHasAdjacentEdges<TVertex, TEdge>(
            IImplicitUndirectedGraph<TVertex, TEdge> graph,
            TVertex vertex,
            IEnumerable<TEdge> edges,
            int degree = -1)    // If not set => equals the count of edges
            where TVertex : notnull
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



        public static void AssertEquivalentGraphs<TVertex, TEdge>(
            IEdgeListGraph<TVertex, TEdge> expected,
            IEdgeListGraph<TVertex, TEdge> actual)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            Assert.AreEqual(expected.IsDirected, actual.IsDirected);
            Assert.AreEqual(expected.AllowParallelEdges, actual.AllowParallelEdges);

            if (expected.IsVerticesEmpty)
            {
                AssertNoVertex(actual);
            }
            else
            {
                AssertHasVertices(actual, expected.Vertices);
            }

            if (expected.IsEdgesEmpty)
            {
                AssertNoEdge(actual);
            }
            else
            {
                AssertHasEdges(actual, expected.Edges);
            }
        }

        #endregion
    }
}
