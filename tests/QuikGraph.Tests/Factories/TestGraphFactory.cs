using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
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
        public static IEnumerable<string> GetFilePaths()
        {
            var filePaths = new List<string>();
            filePaths.AddRange(Directory.GetFiles(".", "g.*.graphml"));
            if (Directory.Exists("GraphML"))
                filePaths.AddRange(Directory.GetFiles("GraphML", "g.*.graphml"));
            return filePaths;
        }

        /// <summary>
        /// Creates adjacency graphs.
        /// </summary>
        [Pure]
        [NotNull, ItemNotNull]
        public static IEnumerable<AdjacencyGraph<string, Edge<string>>> GetAdjacencyGraphs()
        {
            yield return new AdjacencyGraph<string, Edge<string>>();
            foreach (string graphMLFilePath in GetFilePaths())
            {
                yield return LoadGraph(graphMLFilePath);
            }
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
        /// Creates undirected graphs.
        /// </summary>
        [Pure]
        [NotNull, ItemNotNull]
        public static IEnumerable<UndirectedGraph<string, Edge<string>>> GetUndirectedGraphs()
        {
            yield return new UndirectedGraph<string, Edge<string>>();
            foreach (AdjacencyGraph<string, Edge<string>> graph in GetAdjacencyGraphs())
            {
                var undirectedGraph = new UndirectedGraph<string, Edge<string>>();
                undirectedGraph.AddVerticesAndEdgeRange(graph.Edges);
                yield return undirectedGraph;
            }
        }

        /// <summary>
        /// Creates an undirected graph from the given file.
        /// </summary>
        [Pure]
        [NotNull]
        public static UndirectedGraph<string, Edge<string>> LoadUndirectedGraph([NotNull] string graphMLFilePath)
        {
            var graph = LoadGraph(graphMLFilePath);
            var undirectedGraph = new UndirectedGraph<string, Edge<string>>();
            undirectedGraph.AddVerticesAndEdgeRange(graph.Edges);
            return undirectedGraph;
        }

        /// <summary>
        /// Creates bidirectional graphs.
        /// </summary>
        [Pure]
        [NotNull, ItemNotNull]
        public static IEnumerable<BidirectionalGraph<string, Edge<string>>> GetBidirectionalGraphs()
        {
            yield return new BidirectionalGraph<string, Edge<string>>();
            foreach (string graphMLFilePath in GetFilePaths())
            {
                yield return LoadBidirectionalGraph(graphMLFilePath);
            }
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
    }
}