using System;
using NUnit.Framework;
using FastGraph.Collections;

namespace FastGraph.Tests.Collections
{
    /// <summary>
    /// Tests for <see cref="EdgeEdgeDictionary{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class EdgeEdgeDictionaryTests
    {
        [Test]
        public void Constructors()
        {
            // ReSharper disable ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new EdgeEdgeDictionary<int, Edge<int>>());
            Assert.DoesNotThrow(() => new EdgeEdgeDictionary<int, Edge<int>>(12));
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        [Obsolete("BinaryFormatter serialization is obsolete and should not be used. See https://aka.ms/binaryformatter for more information.", error: true)]
        public void Serialization()
        {
            throw new NotSupportedException("BinaryFormatter serialization is obsolete and should not be used. See https://aka.ms/binaryformatter for more information.");
        }

        [Test]
        [Obsolete("BinaryFormatter serialization is obsolete and should not be used. See https://aka.ms/binaryformatter for more information.", error: true)]
        public void Clone()
        {
            throw new NotSupportedException("BinaryFormatter serialization is obsolete and should not be used. See https://aka.ms/binaryformatter for more information.");
        }
    }
}