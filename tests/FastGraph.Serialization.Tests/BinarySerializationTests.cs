using System;
using System.IO;
using JetBrains.Annotations;
using NUnit.Framework;
using static FastGraph.Serialization.Tests.SerializationTestCaseSources;

namespace FastGraph.Serialization.Tests
{
    /// <summary>
    /// Tests relative to serialization via standard API.
    /// </summary>
    [TestFixture]
    internal sealed class SystemSerializationTests
    {
        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationAdjacencyGraphTestCases))]
        [Obsolete("BinaryFormatter serialization is obsolete and should not be used. See https://aka.ms/binaryformatter for more information.", error: true)]
        public void BinarySerialization_AdjacencyGraph([NotNull] AdjacencyGraph<int, EquatableEdge<int>> graph)
        {
            throw new NotSupportedException("BinaryFormatter serialization is obsolete and should not be used. See https://aka.ms/binaryformatter for more information.");
        }

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationAdjacencyGraphTestCases))]
        [Obsolete("BinaryFormatter serialization is obsolete and should not be used. See https://aka.ms/binaryformatter for more information.", error: true)]
        public void BinarySerialization_AdapterGraph([NotNull] AdjacencyGraph<int, EquatableEdge<int>> graph)
        {
            throw new NotSupportedException("BinaryFormatter serialization is obsolete and should not be used. See https://aka.ms/binaryformatter for more information.");
        }

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationClusteredAdjacencyGraphTestCases))]
        [Obsolete("BinaryFormatter serialization is obsolete and should not be used. See https://aka.ms/binaryformatter for more information.", error: true)]
        public void BinarySerialization_ClusteredGraph([NotNull] ClusteredAdjacencyGraph<int, EquatableEdge<int>> graph)
        {
            throw new NotSupportedException("BinaryFormatter serialization is obsolete and should not be used. See https://aka.ms/binaryformatter for more information.");
        }

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationCompressedGraphTestCases))]
        [Obsolete("BinaryFormatter serialization is obsolete and should not be used. See https://aka.ms/binaryformatter for more information.", error: true)]
        public void BinarySerialization_CompressedGraph([NotNull] CompressedSparseRowGraph<int> graph)
        {
            throw new NotSupportedException("BinaryFormatter serialization is obsolete and should not be used. See https://aka.ms/binaryformatter for more information.");
        }

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationBidirectionalGraphTestCases))]
        [Obsolete("BinaryFormatter serialization is obsolete and should not be used. See https://aka.ms/binaryformatter for more information.", error: true)]
        public void BinarySerialization_BidirectionalGraph([NotNull] BidirectionalGraph<int, EquatableEdge<int>> graph)
        {
            throw new NotSupportedException("BinaryFormatter serialization is obsolete and should not be used. See https://aka.ms/binaryformatter for more information.");
        }

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationBidirectionalMatrixGraphTestCases))]
        [Obsolete("BinaryFormatter serialization is obsolete and should not be used. See https://aka.ms/binaryformatter for more information.", error: true)]
        public void BinarySerialization_BidirectionalMatrixGraph([NotNull] BidirectionalMatrixGraph<EquatableEdge<int>> graph)
        {
            throw new NotSupportedException("BinaryFormatter serialization is obsolete and should not be used. See https://aka.ms/binaryformatter for more information.");
        }

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationUndirectedGraphTestCases))]
        [Obsolete("BinaryFormatter serialization is obsolete and should not be used. See https://aka.ms/binaryformatter for more information.", error: true)]
        public void BinarySerialization_UndirectedGraph([NotNull] UndirectedGraph<int, EquatableEdge<int>> graph)
        {
            throw new NotSupportedException("BinaryFormatter serialization is obsolete and should not be used. See https://aka.ms/binaryformatter for more information.");
        }

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationEdgeListGraphTestCases))]
        [Obsolete("BinaryFormatter serialization is obsolete and should not be used. See https://aka.ms/binaryformatter for more information.", error: true)]
        public void BinarySerialization_EdgeListGraph([NotNull] EdgeListGraph<int, EquatableEdge<int>> graph)
        {
            throw new NotSupportedException("BinaryFormatter serialization is obsolete and should not be used. See https://aka.ms/binaryformatter for more information.");
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
        [Obsolete("BinaryFormatter serialization is obsolete and should not be used. See https://aka.ms/binaryformatter for more information.", error: true)]
        public void BinarySerialization_Throws()
        {
            throw new NotSupportedException("BinaryFormatter serialization is obsolete and should not be used. See https://aka.ms/binaryformatter for more information.");
        }
    }
}