#nullable enable

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
    }
}
