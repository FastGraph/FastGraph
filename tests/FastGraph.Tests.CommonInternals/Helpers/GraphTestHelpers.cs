#nullable enable

using FluentAssertions.Execution;
using JetBrains.Annotations;

namespace FastGraph.Tests
{
    /// <summary>
    /// Test helpers for graphs.
    /// </summary>
    internal static class GraphTestHelpers
    {
        [CustomAssertion]
        public static void AssertVertexCountEqual<TVertex>(
            this IVertexSet<TVertex> left,
            IVertexSet<TVertex> right)
            where TVertex : notnull
        {
            using (_ = new AssertionScope())
            {
                left.Should().NotBeNull();
                right.Should().NotBeNull();
                right.VertexCount.Should().Be(left.VertexCount);
            }
        }

        [CustomAssertion]
        public static void AssertEdgeCountEqual<TVertex, TEdge>(
            this IEdgeSet<TVertex, TEdge> left,
            IEdgeSet<TVertex, TEdge> right)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            using (_ = new AssertionScope())
            {
                left.Should().NotBeNull();
                right.Should().NotBeNull();
                right.EdgeCount.Should().Be(left.EdgeCount);
            }
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
            graph.IsVerticesEmpty.Should().BeTrue();
            graph.VertexCount.Should().Be(0);
            graph.Vertices.Should().BeEmpty();
        }

        public static void AssertHasVertices<TVertex>(
            IVertexSet<TVertex> graph,
            IEnumerable<TVertex> vertices)
            where TVertex : notnull
        {
            TVertex[] vertexArray = vertices.ToArray();
            vertexArray.Should().NotBeEmpty();

            graph.IsVerticesEmpty.Should().BeFalse();
            graph.VertexCount.Should().Be(vertexArray.Length);
            graph.Vertices.Should().BeEquivalentTo(vertexArray);
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
            vertexArray.Should().NotBeEmpty();

            foreach (TVertex vertex in vertexArray)
            {
                graph.ContainsVertex(vertex).Should().Be(expectedContains);
            }
        }

        #endregion

        #region Edges helpers

        [CustomAssertion]
        public static void AssertNoEdge<TVertex, TEdge>(IEdgeSet<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            using (_ = new AssertionScope())
            {
                graph.IsEdgesEmpty.Should().BeTrue();
                graph.EdgeCount.Should().Be(0);
                graph.Edges.Should().BeEmpty();
            }
        }

        [CustomAssertion]
        public static void AssertHasEdges<TVertex, TEdge>(
            IEdgeSet<TVertex, TEdge> graph,
            IEnumerable<TEdge> edges)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            TEdge[] edgeArray = edges.ToArray();
            edgeArray.Should().NotBeEmpty();

            using (_ = new AssertionScope())
            {
                graph.IsEdgesEmpty.Should().BeFalse();
                graph.EdgeCount.Should().Be(edgeArray.Length);
                graph.Edges.Should().BeEquivalentTo(edgeArray);
            }
        }

        [CustomAssertion]
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

        [CustomAssertion]
        public static void AssertSameReversedEdge(
            Edge<int> edge,
            SReversedEdge<int, Edge<int>> reversedEdge)
        {
            using (_ = new AssertionScope())
            {
                reversedEdge.Should().Be(new SReversedEdge<int, Edge<int>>(edge));
                reversedEdge.OriginalEdge.Should().BeSameAs(edge);
            }
        }

        [CustomAssertion]
        public static void AssertSameReversedEdges(
            IEnumerable<Edge<int>> edges,
            IEnumerable<SReversedEdge<int, Edge<int>>> reversedEdges)
        {
            var edgesArray = edges.ToArray();
            var reversedEdgesArray = reversedEdges.ToArray();
            reversedEdgesArray.Length.Should().Be(edgesArray.Length);

            using (_ = new AssertionScope())
            {
                for (int i = 0; i < edgesArray.Length; ++i)
                    AssertSameReversedEdge(edgesArray[i], reversedEdgesArray[i]);
            }
        }

        #endregion

        #region Graph helpers

        [CustomAssertion]
        public static void AssertEmptyGraph<TVertex, TEdge>(
            IEdgeListGraph<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            using (_ = new AssertionScope())
            {
                AssertNoVertex(graph);
                AssertNoEdge(graph);
            }
        }

        [CustomAssertion]
        public static void AssertEmptyGraph<TVertex>(
            CompressedSparseRowGraph<TVertex> graph)
            where TVertex : notnull
        {
            using (_ = new AssertionScope())
            {
                AssertNoVertex(graph);
                AssertNoEdge(graph);
            }
        }

        [CustomAssertion]
        public static void AssertNoInEdge<TVertex, TEdge>(IBidirectionalIncidenceGraph<TVertex, TEdge> graph, TVertex vertex)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            using (_ = new AssertionScope())
            {
                graph.IsInEdgesEmpty(vertex).Should().BeTrue();
                graph.InDegree(vertex).Should().Be(0);
                graph.InEdges(vertex).Should().BeEmpty();
            }
        }

        [CustomAssertion]
        public static void AssertHasInEdges<TVertex, TEdge>(
            IBidirectionalIncidenceGraph<TVertex, TEdge> graph,
            TVertex vertex,
            IEnumerable<TEdge> edges)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            TEdge[] edgeArray = edges.ToArray();
            edgeArray.Should().NotBeEmpty();

            using (_ = new AssertionScope())
            {
                graph.IsInEdgesEmpty(vertex).Should().BeFalse();
                graph.InDegree(vertex).Should().Be(edgeArray.Length);
                graph.InEdges(vertex).Should().BeEquivalentTo(edgeArray);
            }
        }

        [CustomAssertion]
        public static void AssertHasReversedInEdges<TVertex, TEdge>(
            IBidirectionalIncidenceGraph<TVertex, SReversedEdge<TVertex, TEdge>> graph,
            TVertex vertex,
            IReadOnlyList<TEdge> edges)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            edges.Should().NotBeEmpty();

            using (_ = new AssertionScope())
            {
                graph.IsInEdgesEmpty(vertex).Should().BeFalse();
                graph.InDegree(vertex).Should().Be(edges.Count);
                graph.InEdges(vertex).Should()
                    .BeEquivalentTo(edges.Select(edge => new SReversedEdge<TVertex, TEdge>(edge)));
            }
        }

        [CustomAssertion]
        public static void AssertNoOutEdge<TVertex, TEdge>(IImplicitGraph<TVertex, TEdge> graph, TVertex vertex)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            using (_ = new AssertionScope())
            {
                graph.IsOutEdgesEmpty(vertex).Should().BeTrue();
                graph.OutDegree(vertex).Should().Be(0);
                graph.OutEdges(vertex).Should().BeEmpty();
            }
        }

        [CustomAssertion]
        public static void AssertHasOutEdges<TVertex, TEdge>(
            IImplicitGraph<TVertex, TEdge> graph,
            TVertex vertex,
            IEnumerable<TEdge> edges)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            TEdge[] edgeArray = edges.ToArray();
            edgeArray.Should().NotBeEmpty();

            using (_ = new AssertionScope())
            {
                graph.IsOutEdgesEmpty(vertex).Should().BeFalse();
                graph.OutDegree(vertex).Should().Be(edgeArray.Length);
                graph.OutEdges(vertex).Should().BeEquivalentTo(edgeArray);
            }
        }

        [CustomAssertion]
        public static void AssertHasReversedOutEdges<TVertex, TEdge>(
            IImplicitGraph<TVertex, SReversedEdge<TVertex, TEdge>> graph,
            TVertex vertex,
            IEnumerable<TEdge> edges)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            TEdge[] edgeArray = edges.ToArray();
            edgeArray.Should().NotBeEmpty();

            using (_ = new AssertionScope())
            {
                graph.IsOutEdgesEmpty(vertex).Should().BeFalse();
                graph.OutDegree(vertex).Should().Be(edgeArray.Length);
                graph.OutEdges(vertex).Should().BeEquivalentTo(edgeArray.Select(edge => new SReversedEdge<TVertex, TEdge>(edge)));
            }
        }

        [CustomAssertion]
        public static void AssertNoAdjacentEdge<TVertex, TEdge>(IImplicitUndirectedGraph<TVertex, TEdge> graph, TVertex vertex)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            using (_ = new AssertionScope())
            {
                graph.IsAdjacentEdgesEmpty(vertex).Should().BeTrue();
                graph.AdjacentDegree(vertex).Should().Be(0);
                graph.AdjacentEdges(vertex).Should().BeEmpty();
            }
        }

        [CustomAssertion]
        public static void AssertHasAdjacentEdges<TVertex, TEdge>(
            IImplicitUndirectedGraph<TVertex, TEdge> graph,
            TVertex vertex,
            IEnumerable<TEdge> edges,
            int degree = -1)    // If not set => equals the count of edges
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            TEdge[] edgeArray = edges.ToArray();
            edgeArray.Should().NotBeEmpty();

            using (_ = new AssertionScope())
            {
                graph.IsAdjacentEdgesEmpty(vertex).Should().BeFalse();
                graph.AdjacentDegree(vertex).Should().Be(degree < 0 ? edgeArray.Length : degree);
                graph.AdjacentEdges(vertex).Should().BeEquivalentTo(edgeArray);
            }
        }


        [CustomAssertion]
        public static void AssertEquivalentGraphs<TVertex, TEdge>(
            IEdgeListGraph<TVertex, TEdge> expected,
            IEdgeListGraph<TVertex, TEdge> actual)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            using (_ = new AssertionScope())
            {
                actual.IsDirected.Should().Be(expected.IsDirected);
                actual.AllowParallelEdges.Should().Be(expected.AllowParallelEdges);

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
        }

        #endregion
    }
}
