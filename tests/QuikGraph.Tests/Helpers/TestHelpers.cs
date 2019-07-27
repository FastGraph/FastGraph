using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace QuikGraph.Tests
{
    /// <summary>
    /// A collection of utility methods for unit tests.
    /// </summary>
    public static class TestHelpers
    {
        /// <summary>
        /// Creates the set of all Edges {(left, right)}
        /// such that every element in <paramref name="leftVertices"/> is matched with
        /// every element in <paramref name="rightVertices"/>.
        /// This is especially useful for creating bipartite graphs.
        /// </summary>
        /// <param name="leftVertices">A collection of vertices.</param>
        /// <param name="rightVertices">A collection of vertices.</param>
        /// <param name="edgeFactory">An object to use for creating edges.</param>
        /// <returns>List of edges.</returns>
        [NotNull, ItemNotNull]
        public static List<Edge<TVertex>> CreateAllPairwiseEdges<TVertex>(
            [NotNull, ItemNotNull] IEnumerable<TVertex> leftVertices,
            [NotNull, ItemNotNull] IEnumerable<TVertex> rightVertices,
            [NotNull] EdgeFactory<TVertex, Edge<TVertex>> edgeFactory)
        {
            var edges = new List<Edge<TVertex>>();
            var rightVerticesArray = rightVertices.ToArray();

            foreach (TVertex left in leftVertices)
            {
                foreach (TVertex right in rightVerticesArray)
                {
                    edges.Add(edgeFactory(left, right));
                }
            }

            return edges;
        }
    }
}
