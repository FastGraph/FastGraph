using System;
using NUnit.Framework;

namespace QuikGraph.Tests
{
    /// <summary>
    /// Tests for <see cref="UndirectedEdge{TVertex}"/> and <see cref="SEquatableUndirectedEdge{TVertex}"/>.
    ///</summary>
    [TestFixture]
    internal class UndirectedEquatableEdgeTests
    {
        [Test]
        public void UndirectedEdge_WrongSource()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentException>(() => new SEquatableUndirectedEdge<int>(2, 1));
        }
    }
}
