using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using NUnit.Framework;

namespace QuikGraph.Serialization.Tests
{
    /// <summary>
    /// Tests relative to serialization via standard API.
    /// </summary>
    [TestFixture]
    internal class SystemSerializationTests
    {
        #region Helpers

        [Pure]
        [NotNull]
        private static TGraph SerializeDeserialize<TVertex, TEdge, TGraph>([NotNull] TGraph graph)
            where TGraph : IGraph<TVertex, TEdge>
            where TEdge : IEdge<TVertex>
        {
            Assert.IsNotNull(graph);

            // Serialize
            using (var stream = new MemoryStream())
            {
                graph.SerializeToBinary(stream);

                // Deserialize
                stream.Position = 0;

                TGraph deserializedGraph = stream.DeserializeFromBinary<TVertex, TEdge, TGraph>();
                Assert.IsNotNull(deserializedGraph);
                Assert.AreNotSame(graph, deserializedGraph);

                return deserializedGraph;
            }
        }

        #endregion

        [NotNull, ItemNotNull]
        private static IEnumerable<TestCaseData> BinarySerializationAdjacencyGraphTestCases
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

        [TestCaseSource(nameof(BinarySerializationAdjacencyGraphTestCases))]
        public void BinarySerialization_AdjacencyGraph([NotNull] AdjacencyGraph<int, EquatableEdge<int>> graph)
        {
            AdjacencyGraph<int, EquatableEdge<int>> deserializedGraph1 =
                SerializeDeserialize<int, EquatableEdge<int>, AdjacencyGraph<int, EquatableEdge<int>>>(graph);
            Assert.IsTrue(EquateGraphs.Equate(graph, deserializedGraph1));

            var arrayGraph = new ArrayAdjacencyGraph<int, EquatableEdge<int>>(graph);
            ArrayAdjacencyGraph<int, EquatableEdge<int>> deserializedGraph2 =
                SerializeDeserialize<int, EquatableEdge<int>, ArrayAdjacencyGraph<int, EquatableEdge<int>>>(arrayGraph);
            Assert.IsTrue(EquateGraphs.Equate(arrayGraph, deserializedGraph2));
        }

        [TestCaseSource(nameof(BinarySerializationAdjacencyGraphTestCases))]
        public void BinarySerialization_AdapterGraph([NotNull] AdjacencyGraph<int, EquatableEdge<int>> graph)
        {
            var bidirectionalAdapterGraph = new BidirectionalAdapterGraph<int, EquatableEdge<int>>(graph);
            BidirectionalAdapterGraph<int, EquatableEdge<int>> deserializedGraph =
                SerializeDeserialize<int, EquatableEdge<int>, BidirectionalAdapterGraph<int, EquatableEdge<int>>>(bidirectionalAdapterGraph);
            Assert.IsTrue(EquateGraphs.Equate(graph, deserializedGraph));
        }

        [NotNull, ItemNotNull]
        private static IEnumerable<TestCaseData> BinarySerializationClusteredAdjacencyGraphTestCases
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

        [TestCaseSource(nameof(BinarySerializationClusteredAdjacencyGraphTestCases))]
        public void BinarySerialization_ClusteredGraph([NotNull] ClusteredAdjacencyGraph<int, EquatableEdge<int>> graph)
        {
            ClusteredAdjacencyGraph<int, EquatableEdge<int>> deserializedGraph =
                SerializeDeserialize<int, EquatableEdge<int>, ClusteredAdjacencyGraph<int, EquatableEdge<int>>>(graph);
            Assert.IsTrue(EquateGraphs.Equate(graph, deserializedGraph));
        }

        [NotNull, ItemNotNull]
        private static IEnumerable<TestCaseData> BinarySerializationCompressedGraphTestCases
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

        [TestCaseSource(nameof(BinarySerializationCompressedGraphTestCases))]
        public void BinarySerialization_CompressedGraph([NotNull] CompressedSparseRowGraph<int> graph)
        {
            CompressedSparseRowGraph<int> deserializedGraph =
                SerializeDeserialize<int, SEquatableEdge<int>, CompressedSparseRowGraph<int>>(graph);
            Assert.IsTrue(EquateGraphs.Equate(graph, deserializedGraph));
        }

        [NotNull, ItemNotNull]
        private static IEnumerable<TestCaseData> BinarySerializationBidirectionalGraphTestCases
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

        [TestCaseSource(nameof(BinarySerializationBidirectionalGraphTestCases))]
        public void BinarySerialization_BidirectionalGraph([NotNull] BidirectionalGraph<int, EquatableEdge<int>> graph)
        {
            BidirectionalGraph<int, EquatableEdge<int>> deserializedGraph =
                SerializeDeserialize<int, EquatableEdge<int>, BidirectionalGraph<int, EquatableEdge<int>>>(graph);
            Assert.IsTrue(EquateGraphs.Equate(graph, deserializedGraph));

            var arrayGraph = new ArrayBidirectionalGraph<int, EquatableEdge<int>>(graph);
            ArrayBidirectionalGraph<int, EquatableEdge<int>> deserializedGraph2 =
                SerializeDeserialize<int, EquatableEdge<int>, ArrayBidirectionalGraph<int, EquatableEdge<int>>>(arrayGraph);
            Assert.IsTrue(EquateGraphs.Equate(arrayGraph, deserializedGraph2));

            var reversedGraph = new ReversedBidirectionalGraph<int, EquatableEdge<int>>(graph);
            ReversedBidirectionalGraph<int, EquatableEdge<int>> deserializedGraph3 =
                SerializeDeserialize<int, SReversedEdge<int, EquatableEdge<int>>, ReversedBidirectionalGraph<int, EquatableEdge<int>>>(reversedGraph);
            Assert.IsTrue(EquateGraphs.Equate(reversedGraph, deserializedGraph3));

            var undirectedBidirectionalGraph = new UndirectedBidirectionalGraph<int, EquatableEdge<int>>(graph);
            UndirectedBidirectionalGraph<int, EquatableEdge<int>> deserializedGraph4 =
                SerializeDeserialize<int, EquatableEdge<int>, UndirectedBidirectionalGraph<int, EquatableEdge<int>>>(undirectedBidirectionalGraph);
            Assert.IsTrue(EquateGraphs.Equate(undirectedBidirectionalGraph, deserializedGraph4));
        }

        [NotNull, ItemNotNull]
        private static IEnumerable<TestCaseData> BinarySerializationBidirectionalMatrixGraphTestCases
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

        [TestCaseSource(nameof(BinarySerializationBidirectionalMatrixGraphTestCases))]
        public void BinarySerialization_BidirectionalMatrixGraph([NotNull] BidirectionalMatrixGraph<EquatableEdge<int>> graph)
        {
            BidirectionalMatrixGraph<EquatableEdge<int>> deserializedGraph =
                SerializeDeserialize<int, EquatableEdge<int>, BidirectionalMatrixGraph<EquatableEdge<int>>>(graph);
            Assert.IsTrue(EquateGraphs.Equate(graph, deserializedGraph));
        }

        [NotNull, ItemNotNull]
        private static IEnumerable<TestCaseData> BinarySerializationUndirectedGraphTestCases
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

        [TestCaseSource(nameof(BinarySerializationUndirectedGraphTestCases))]
        public void BinarySerialization_UndirectedGraph([NotNull] UndirectedGraph<int, EquatableEdge<int>> graph)
        {
            UndirectedGraph<int, EquatableEdge<int>> deserializedGraph1 =
                SerializeDeserialize<int, EquatableEdge<int>, UndirectedGraph<int, EquatableEdge<int>>>(graph);
            Assert.IsTrue(EquateGraphs.Equate(graph, deserializedGraph1));

            var arrayGraph = new ArrayUndirectedGraph<int, EquatableEdge<int>>(graph);
            ArrayUndirectedGraph<int, EquatableEdge<int>> deserializedGraph2 =
                SerializeDeserialize<int, EquatableEdge<int>, ArrayUndirectedGraph<int, EquatableEdge<int>>>(arrayGraph);
            Assert.IsTrue(EquateGraphs.Equate(graph, deserializedGraph2));
        }

        [NotNull, ItemNotNull]
        private static IEnumerable<TestCaseData> BinarySerializationEdgeListGraphTestCases
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

        [TestCaseSource(nameof(BinarySerializationEdgeListGraphTestCases))]
        public void BinarySerialization_EdgeListGraph([NotNull] EdgeListGraph<int, EquatableEdge<int>> graph)
        {
            EdgeListGraph<int, EquatableEdge<int>> deserializedGraph =
                SerializeDeserialize<int, EquatableEdge<int>, EdgeListGraph<int, EquatableEdge<int>>>(graph);
            Assert.IsTrue(EquateGraphs.Equate(graph, deserializedGraph));
        }

        #region Test classes

        public class TestStream : Stream
        {
            public override void Flush()
            {
                // Fake
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                return 0;   // Fake
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                return 0;   // Fake
            }

            public override void SetLength(long value)
            {
                // Fake
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                // Fake
            }

            public override bool CanRead => false;

            public override bool CanSeek => false;

            public override bool CanWrite => false;

            public override long Length => 0;

            public override bool CanTimeout => true;

            public override long Position
            {
                get => 0;
                set { }
            }
        }

        #endregion

        [Test]
        public void BinarySerialization_Throws()
        {
            // ReSharper disable AssignNullToNotNullAttribute
            using (var stream = new MemoryStream())
            {
                Assert.Throws<ArgumentNullException>(
                    () => ((AdjacencyGraph<int, Edge<int>>)null).SerializeToBinary(stream));
            }

            var graph = new AdjacencyGraph<int, Edge<int>>();
            using (var stream = new TestStream())
            {
                Assert.Throws<ArgumentException>(
                    () => graph.SerializeToBinary(stream));
            }

            Assert.Throws<ArgumentNullException>(
                () => graph.SerializeToBinary(null));

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(
                () => ((Stream)null).DeserializeFromBinary<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>());

            using (var stream = new TestStream())
            {
                Assert.Throws<ArgumentException>(
                    () => stream.DeserializeFromBinary<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>());
            }
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            // ReSharper restore AssignNullToNotNullAttribute
        }
    }
}