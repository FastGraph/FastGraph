#nullable enable

using Microsoft.Msagl.Drawing;
using NUnit.Framework;
using FastGraph.Tests;

namespace FastGraph.MSAGL.Tests
{
    /// <summary>
    /// Tests related to <see cref="MsaglVertexEventArgs{TVertex}"/>.
    /// </summary>
    internal sealed class MsaglVertexEventArgsTests
    {
        [Test]
        public void Constructor()
        {
            var vertex = new TestVertex("1");
            var node = new Node("1");
            var args = new MsaglVertexEventArgs<TestVertex>(vertex, node);

            Assert.AreSame(vertex, args.Vertex);
            Assert.AreSame(node, args.Node);
        }

        [Test]
        public void Constructor_Throws()
        {
            var vertex = new TestVertex("1");
            var node = new Node("1");

            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Assert.Throws<ArgumentNullException>(
                () => new MsaglVertexEventArgs<TestVertex>(vertex, default));
            Assert.Throws<ArgumentNullException>(
                () => new MsaglVertexEventArgs<TestVertex>(default, node));
            Assert.Throws<ArgumentNullException>(
                () => new MsaglVertexEventArgs<TestVertex>(default, default));
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }
    }
}
