using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;

namespace QuikGraph.Tests
{
    /// <summary>
    /// Test helpers for graphs.
    /// </summary>
    internal static class GraphTestHelpers
    {
        public static void AssertVertexCountEqual<TVertex>(
            [NotNull] this IVertexSet<TVertex> left,
            [NotNull] IVertexSet<TVertex> right)
        {
            Assert.IsNotNull(left);
            Assert.IsNotNull(right);
            Assert.AreEqual(left.VertexCount, right.VertexCount);
        }

        public static void AssertEdgeCountEqual<TVertex, TEdge>(
            [NotNull] this IEdgeListGraph<TVertex, TEdge> left,
            [NotNull] IEdgeListGraph<TVertex, TEdge> right)
            where TEdge : IEdge<TVertex>
        {
            Assert.IsNotNull(left);
            Assert.IsNotNull(right);
            Assert.AreEqual(left.EdgeCount, right.EdgeCount);
        }

        public static bool InVertexSet<TVertex>(
            [NotNull] IVertexSet<TVertex> graph,
            [NotNull] TVertex vertex)
        {
            return graph.ContainsVertex(vertex);
        }

        public static bool InVertexSet<TVertex, TEdge>(
            [NotNull] IEdgeListGraph<TVertex, TEdge> graph,
            [NotNull] TEdge edge)
            where TEdge : IEdge<TVertex>
        {
            return InVertexSet(graph, edge.Source)
                   && InVertexSet(graph, edge.Target);
        }

        public static bool InEdgeSet<TVertex, TEdge>(
            IEdgeListGraph<TVertex, TEdge> graph,
            TEdge edge)
            where TEdge : IEdge<TVertex>
        {
            return InVertexSet(graph, edge) && graph.ContainsEdge(edge);
        }

        [Pure]
        public static bool IsDescendant<TVertex>(
            [NotNull] Dictionary<TVertex, TVertex> parents,
            [NotNull] TVertex u,
            [NotNull] TVertex v)
        {
            TVertex t;
            TVertex current = u;
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
    }
}