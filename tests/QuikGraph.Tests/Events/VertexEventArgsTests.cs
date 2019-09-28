using System;
using NUnit.Framework;

namespace QuikGraph.Tests.Events
{
    /// <summary>
    /// Tests related to <see cref="VertexEventArgs{TVertex}"/>.
    /// </summary>
    internal class VertexEventArgsTests
    {
        [Test]
        public void Constructor()
        {
            var vertex = new TestVertex("1");
            var args = new VertexEventArgs<TestVertex>(vertex);

            Assert.AreSame(vertex, args.Vertex);
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => new VertexEventArgs<TestVertex>(null));
        }
    }
}