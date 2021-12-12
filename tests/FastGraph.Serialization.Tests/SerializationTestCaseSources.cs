#nullable enable

using JetBrains.Annotations;
using NUnit.Framework;

namespace FastGraph.Serialization.Tests
{
    /// <summary>
    /// Test case sources for serialization tests.
    /// </summary>
    internal static class SerializationTestCaseSources
    {
        public static IEnumerable<TestCaseData> SerializationAdjacencyGraphTestCases
        {
            [UsedImplicitly]
            get
            {
                var emptyGraph = new AdjacencyGraph<int, EquatableEdge<int>>();
                yield return new TestCaseData(emptyGraph);

                var graph = new AdjacencyGraph<int, EquatableEdge<int>>();
                graph.AddVertexRange(new[] { 0, 1, 2, 3 });
                graph.AddEdgeRange(new[]
                {
                    new EquatableEdge<int>(0, 1),
                    new EquatableEdge<int>(1, 2),
                    new EquatableEdge<int>(2, 0),
                    new EquatableEdge<int>(2, 1),
                    new EquatableEdge<int>(2, 2)
                });
                yield return new TestCaseData(graph);
            }
        }

        public static IEnumerable<TestCaseData> SerializationClusteredAdjacencyGraphTestCases
        {
            [UsedImplicitly]
            get
            {
                var emptyGraph = new AdjacencyGraph<int, EquatableEdge<int>>();
                var clusterEmptyGraph = new ClusteredAdjacencyGraph<int, EquatableEdge<int>>(emptyGraph);
                yield return new TestCaseData(clusterEmptyGraph);

                var graph = new AdjacencyGraph<int, EquatableEdge<int>>();
                graph.AddVertexRange(new[] { 0, 1, 2, 3 });
                graph.AddEdgeRange(new[]
                {
                    new EquatableEdge<int>(0, 1),
                    new EquatableEdge<int>(1, 2),
                    new EquatableEdge<int>(2, 0),
                    new EquatableEdge<int>(2, 1),
                    new EquatableEdge<int>(2, 2)
                });
                var clusterGraph = new ClusteredAdjacencyGraph<int, EquatableEdge<int>>(graph);
                yield return new TestCaseData(clusterGraph);

                graph = new AdjacencyGraph<int, EquatableEdge<int>>();
                graph.AddVertexRange(new[] { 0, 1, 2, 3 });
                graph.AddEdgeRange(new[]
                {
                    new EquatableEdge<int>(0, 1),
                    new EquatableEdge<int>(1, 2),
                    new EquatableEdge<int>(2, 0),
                    new EquatableEdge<int>(3, 2)
                });
                clusterGraph = new ClusteredAdjacencyGraph<int, EquatableEdge<int>>(graph);
                ClusteredAdjacencyGraph<int, EquatableEdge<int>> subGraph = clusterGraph.AddCluster();
                subGraph.AddVertexRange(new[] { 4, 5, 6 });
                subGraph.AddEdge(new EquatableEdge<int>(4, 6));
                yield return new TestCaseData(clusterGraph);
            }
        }

        public static IEnumerable<TestCaseData> SerializationCompressedGraphTestCases
        {
            [UsedImplicitly]
            get
            {
                var emptyGraph = new AdjacencyGraph<int, Edge<int>>();
                var emptyCompressedGraph = CompressedSparseRowGraph<int>.FromGraph(emptyGraph);
                yield return new TestCaseData(emptyCompressedGraph);

                var graph = new AdjacencyGraph<int, EquatableEdge<int>>();
                graph.AddVertexRange(new[] { 0, 1, 2, 3 });
                graph.AddEdgeRange(new[]
                {
                    new EquatableEdge<int>(0, 1),
                    new EquatableEdge<int>(1, 2),
                    new EquatableEdge<int>(2, 0),
                    new EquatableEdge<int>(2, 1),
                    new EquatableEdge<int>(2, 2)
                });
                var compressedGraph = CompressedSparseRowGraph<int>.FromGraph(emptyGraph);
                yield return new TestCaseData(compressedGraph);
            }
        }

        public static IEnumerable<TestCaseData> SerializationBidirectionalGraphTestCases
        {
            [UsedImplicitly]
            get
            {
                var emptyGraph = new BidirectionalGraph<int, EquatableEdge<int>>();
                yield return new TestCaseData(emptyGraph);

                var graph = new BidirectionalGraph<int, EquatableEdge<int>>();
                graph.AddVertexRange(new[] { 0, 1, 2, 3 });
                graph.AddEdgeRange(new[]
                {
                    new EquatableEdge<int>(0, 1),
                    new EquatableEdge<int>(1, 2),
                    new EquatableEdge<int>(2, 0),
                    new EquatableEdge<int>(2, 1),
                    new EquatableEdge<int>(2, 2)
                });

                yield return new TestCaseData(graph);
            }
        }

        public static IEnumerable<TestCaseData> SerializationBidirectionalMatrixGraphTestCases
        {
            [UsedImplicitly]
            get
            {
                var emptyGraph = new BidirectionalMatrixGraph<EquatableEdge<int>>(10);
                yield return new TestCaseData(emptyGraph);

                var graph = new BidirectionalMatrixGraph<EquatableEdge<int>>(4);
                graph.AddEdgeRange(new[]
                {
                    new EquatableEdge<int>(0, 1),
                    new EquatableEdge<int>(1, 2),
                    new EquatableEdge<int>(2, 0),
                    new EquatableEdge<int>(2, 1),
                    new EquatableEdge<int>(2, 2)
                });

                yield return new TestCaseData(graph);
            }
        }

        public static IEnumerable<TestCaseData> SerializationUndirectedGraphTestCases
        {
            [UsedImplicitly]
            get
            {
                var emptyGraph = new UndirectedGraph<int, EquatableEdge<int>>();
                yield return new TestCaseData(emptyGraph);

                var graph = new UndirectedGraph<int, EquatableEdge<int>>();
                graph.AddVertexRange(new[] { 0, 1, 2, 3 });
                graph.AddEdgeRange(new[]
                {
                    new EquatableEdge<int>(0, 1),
                    new EquatableEdge<int>(1, 2),
                    new EquatableEdge<int>(2, 0),
                    new EquatableEdge<int>(2, 1),
                    new EquatableEdge<int>(2, 2)
                });

                yield return new TestCaseData(graph);
            }
        }

        public static IEnumerable<TestCaseData> SerializationEdgeListGraphTestCases
        {
            [UsedImplicitly]
            get
            {
                var emptyGraph = new EdgeListGraph<int, EquatableEdge<int>>();
                yield return new TestCaseData(emptyGraph);

                var graph = new EdgeListGraph<int, EquatableEdge<int>>();
                graph.AddEdgeRange(new[]
                {
                    new EquatableEdge<int>(0, 1),
                    new EquatableEdge<int>(1, 2),
                    new EquatableEdge<int>(2, 0),
                    new EquatableEdge<int>(2, 1),
                    new EquatableEdge<int>(2, 2)
                });

                yield return new TestCaseData(graph);
            }
        }
    }
}
