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
            Invoking((Func<EdgeEdgeDictionary<int, Edge<int>>>)(() => new EdgeEdgeDictionary<int, Edge<int>>())).Should().NotThrow();
            Invoking((Func<EdgeEdgeDictionary<int, Edge<int>>>)(() => new EdgeEdgeDictionary<int, Edge<int>>(12))).Should().NotThrow();
            // ReSharper restore ObjectCreationAsStatement
        }
    }
}
