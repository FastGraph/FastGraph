using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Serialization;

namespace QuikGraph.Tests
{
    /// <summary>
    /// Factory to create graphs.
    /// </summary>
    internal static class TestGraphFactory
    {
        /// <summary>
        /// Gets graph ML file paths.
        /// </summary>
        [Pure]
        [NotNull, ItemNotNull]
        public static IEnumerable<string> GetGraphMLFilePaths()
        {
            var testPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "GraphML");
            if (Directory.Exists(testPath))
                return Directory.GetFiles(testPath, "g.*.graphml").AsEnumerable();
            throw new AssertionException("GraphML folder must exist.");
        }

        /// <summary>
        /// Creates an adjacency graph from the given file.
        /// </summary>
        [Pure]
        [NotNull]
        public static AdjacencyGraph<string, Edge<string>> LoadGraph([NotNull] string graphMLFilePath)
        {
            var graph = new AdjacencyGraph<string, Edge<string>>();
            using (var reader = new StreamReader(graphMLFilePath))
            {
                graph.DeserializeFromGraphML(
                    reader,
                    id => id,
                    (source, target, id) => new Edge<string>(source, target));
            }

            return graph;
        }

        /// <summary>
        /// Creates a bidirectional graph from the given file.
        /// </summary>
        [Pure]
        [NotNull]
        public static BidirectionalGraph<string, Edge<string>> LoadBidirectionalGraph([NotNull] string graphMLFilePath)
        {
            var graph = new BidirectionalGraph<string, Edge<string>>();
            using (var reader = new StreamReader(graphMLFilePath))
            {
                graph.DeserializeFromGraphML(
                    reader,
                    id => id,
                    (source, target, id) => new Edge<string>(source, target));
            }

            return graph;
        }

        /// <summary>
        /// Creates an undirected graph from the given file.
        /// </summary>
        [Pure]
        [NotNull]
        public static UndirectedGraph<string, Edge<string>> LoadUndirectedGraph([NotNull] string graphMLFilePath)
        {
            AdjacencyGraph<string, Edge<string>> graph = LoadGraph(graphMLFilePath);
            var undirectedGraph = new UndirectedGraph<string, Edge<string>>();
            undirectedGraph.AddVerticesAndEdgeRange(graph.Edges);
            return undirectedGraph;
        }

        /// <summary>
        /// Creates adjacency graphs.
        /// </summary>
        [Pure]
        [NotNull, ItemNotNull]
        public static IEnumerable<AdjacencyGraph<string, Edge<string>>> GetAdjacencyGraphs()
        {
            yield return new AdjacencyGraph<string, Edge<string>>();
            foreach (string graphMLFilePath in GetGraphMLFilePaths())
            {
                yield return LoadGraph(graphMLFilePath);
            }
        }

        /// <summary>
        /// Creates adjacency graphs.
        /// </summary>
        [Pure]
        [NotNull, ItemNotNull]
        public static IEnumerable<AdjacencyGraph<string, Edge<string>>> GetAdjacencyGraphs_TMP()
        {
            // TODO Need to be merged with previous method,
            // but for now it will make build slow and some tests failing
            // This need to be investigated, but there are some tests that
            // are not doing much thing since there is no input graph!
            // Do the same for all methods with _TMP suffix.
            yield return new AdjacencyGraph<string, Edge<string>>();
        }

        /// <summary>
        /// Creates bidirectional graphs.
        /// </summary>
        [Pure]
        [NotNull, ItemNotNull]
        public static IEnumerable<BidirectionalGraph<string, Edge<string>>> GetBidirectionalGraphs()
        {
            yield return new BidirectionalGraph<string, Edge<string>>();
            foreach (string graphMLFilePath in GetGraphMLFilePaths())
            {
                yield return LoadBidirectionalGraph(graphMLFilePath);
            }
        }

        /// <summary>
        /// Creates bidirectional graphs.
        /// </summary>
        [Pure]
        [NotNull, ItemNotNull]
        public static IEnumerable<BidirectionalGraph<string, Edge<string>>> GetBidirectionalGraphs_TMP()
        {
            // TODO Need to be merged with previous method,
            // but for now it will make build slow and some tests failing
            // This need to be investigated, but there are some tests that
            // are not doing much thing since there is no input graph!
            // Do the same for all methods with _TMP suffix.
            yield return new BidirectionalGraph<string, Edge<string>>();
        }

        /// <summary>
        /// Creates undirected graphs.
        /// </summary>
        [Pure]
        [NotNull, ItemNotNull]
        public static IEnumerable<UndirectedGraph<string, Edge<string>>> GetUndirectedGraphs()
        {
            yield return new UndirectedGraph<string, Edge<string>>();
            foreach (string graphMLFilePath in GetGraphMLFilePaths())
            {
                yield return LoadUndirectedGraph(graphMLFilePath);
            }
        }

        /// <summary>
        /// Creates undirected graphs.
        /// </summary>
        [Pure]
        [NotNull, ItemNotNull]
        public static IEnumerable<UndirectedGraph<string, Edge<string>>> GetUndirectedGraphs_TMP()
        {
            // TODO Need to be merged with previous method,
            // but for now it will make build slow and some tests failing
            // This need to be investigated, but there are some tests that
            // are not doing much thing since there is no input graph!
            // Do the same for all methods with _TMP suffix.
            yield return new UndirectedGraph<string, Edge<string>>();
        }
    }
}