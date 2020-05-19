using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Tests;

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
                graph.AddVertex(0);
                graph.AddVertex(1);
                graph.AddVertex(2);
                graph.AddVertex(3);
                graph.AddEdge(new EquatableEdge<int>(0, 1));
                graph.AddEdge(new EquatableEdge<int>(1, 2));
                graph.AddEdge(new EquatableEdge<int>(2, 0));
                graph.AddEdge(new EquatableEdge<int>(2, 2));

                yield return new TestCaseData(graph);
            }
        }

        [TestCaseSource(nameof(BinarySerializationAdjacencyGraphTestCases))]
        public void BinarySerialization_AdjacencyGraph([NotNull] AdjacencyGraph<int, EquatableEdge<int>> graph)
        {
            AdjacencyGraph<int, EquatableEdge<int>> deserializedGraph =
                SerializeDeserialize<int, EquatableEdge<int>, AdjacencyGraph<int, EquatableEdge<int>>>(graph);
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
                graph.AddVertex(0);
                graph.AddVertex(1);
                graph.AddVertex(2);
                graph.AddVertex(3);
                graph.AddEdge(new EquatableEdge<int>(0, 1));
                graph.AddEdge(new EquatableEdge<int>(1, 2));
                graph.AddEdge(new EquatableEdge<int>(2, 0));
                graph.AddEdge(new EquatableEdge<int>(2, 2));

                yield return new TestCaseData(graph);
            }
        }

        [TestCaseSource(nameof(BinarySerializationBidirectionalGraphTestCases))]
        public void BinarySerialization_BidirectionalGraph([NotNull] BidirectionalGraph<int, EquatableEdge<int>> graph)
        {
            BidirectionalGraph<int, EquatableEdge<int>> deserializedGraph =
                SerializeDeserialize<int, EquatableEdge<int>, BidirectionalGraph<int, EquatableEdge<int>>>(graph);
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
                graph.AddVertex(0);
                graph.AddVertex(1);
                graph.AddVertex(2);
                graph.AddVertex(3);
                graph.AddEdge(new EquatableEdge<int>(0, 1));
                graph.AddEdge(new EquatableEdge<int>(1, 2));
                graph.AddEdge(new EquatableEdge<int>(2, 0));
                graph.AddEdge(new EquatableEdge<int>(2, 2));

                yield return new TestCaseData(graph);
            }
        }

        [TestCaseSource(nameof(BinarySerializationUndirectedGraphTestCases))]
        public void BinarySerialization_UndirectedGraph([NotNull] UndirectedGraph<int, EquatableEdge<int>> graph)
        {
            UndirectedGraph<int, EquatableEdge<int>> deserializedGraph =
                SerializeDeserialize<int, EquatableEdge<int>, UndirectedGraph<int, EquatableEdge<int>>>(graph);
            AssertGraphsEqual(graph, deserializedGraph);

            #region Local function

            void AssertGraphsEqual(
                IEdgeListGraph<int, EquatableEdge<int>> expected,
                IEdgeListGraph<int, EquatableEdge<int>> actual)
            {
                expected.AssertVertexCountEqual(actual);
                expected.AssertEdgeCountEqual(actual);

                foreach (int vertex in expected.Vertices)
                    Assert.IsTrue(actual.ContainsVertex(vertex));
                foreach (Edge<int> edge in expected.Edges)
                    Assert.IsTrue(actual.ContainsEdge(new EquatableEdge<int>(edge.Source, edge.Target)));
            }

            #endregion
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