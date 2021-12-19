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

            args.Vertex.Should().BeSameAs(vertex);
            args.Node.Should().BeSameAs(node);
        }

        [Test]
        public void Constructor_Throws()
        {
            var vertex = new TestVertex("1");
            var node = new Node("1");

            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new MsaglVertexEventArgs<TestVertex>(vertex, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MsaglVertexEventArgs<TestVertex>(default, node)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MsaglVertexEventArgs<TestVertex>(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }
    }
}
