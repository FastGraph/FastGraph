using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using JetBrains.Annotations;
using QuikGraph.Tests.Algorithms;

namespace QuikGraph.Tests
{
    /// <summary>
    /// A collection of utility methods for unit tests.
    /// </summary>
    internal static class TestHelpers
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
        [Pure]
        [NotNull, ItemNotNull]
        public static IEnumerable<Edge<TVertex>> CreateAllPairwiseEdges<TVertex>(
            [NotNull, ItemNotNull] IEnumerable<TVertex> leftVertices,
            [NotNull, ItemNotNull] IEnumerable<TVertex> rightVertices,
            [NotNull] EdgeFactory<TVertex, Edge<TVertex>> edgeFactory)
        {
            TVertex[] rightVerticesArray = rightVertices.ToArray();

            foreach (TVertex left in leftVertices)
            {
                foreach (TVertex right in rightVerticesArray)
                {
                    yield return edgeFactory(left, right);
                }
            }
        }

        [Pure]
        [NotNull]
        public static UndirectedGraph<int, UndirectedEdge<int>> CreateUndirectedGraph(
            [NotNull] IEnumerable<Vertices> vertices)
        {
            var graph = new UndirectedGraph<int, UndirectedEdge<int>>();
            foreach (Vertices pair in vertices)
            {
                graph.AddVerticesAndEdge(new UndirectedEdge<int>(pair.Source, pair.Target));
            }

            return graph;
        }

        [Pure]
        [NotNull]
        public static UndirectedGraph<TVertex, TEdge> CreateUndirectedGraph<TVertex, TEdge>(
            [NotNull, ItemNotNull] IEnumerable<TEdge> edges)
            where TEdge : IEdge<TVertex>, ITagged<double>
        {
            var graph = new UndirectedGraph<TVertex, TEdge>(true);
            foreach (TEdge edge in edges)
            {
                graph.AddVerticesAndEdge(edge);
            }

            return graph;
        }

        [Pure]
        [NotNull]
        public static T SerializeAndDeserialize<T>([NotNull] T @object)
        {
            // Round-trip the exception: Serialize and de-serialize with a BinaryFormatter
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                // "Save" object state
                bf.Serialize(ms, @object);

                // Re-use the same stream for de-serialization
                ms.Seek(0, 0);

                // Replace the original exception with de-serialized one
                return (T)bf.Deserialize(ms);
            }
        }
    }
}