#nullable enable

using Microsoft.Msagl.Drawing;
using NUnit.Framework;

namespace FastGraph.MSAGL.Tests
{
    /// <summary>
    /// Tests related to <see cref="MsaglEdgeEventArgs{TVertex,TEdge}"/>.
    /// </summary>
    internal sealed class MsaglEdgeEventArgsTests
    {
        [Test]
        public void Constructor()
        {
            var edge = new Edge<int>(1, 2);
            var msaglEdge = new Edge(new Node("1"), new Node("2"), ConnectionToGraph.Disconnected);
            var args = new MsaglEdgeEventArgs<int, Edge<int>>(edge, msaglEdge);

            args.Edge.Should().BeSameAs(edge);
            args.MsaglEdge.Should().BeSameAs(msaglEdge);
        }

        [Test]
        public void Constructor_Throws()
        {
            var edge = new Edge<int>(1, 2);
            var msaglEdge = new Edge(new Node("1"), new Node("2"), ConnectionToGraph.Disconnected);

            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new MsaglEdgeEventArgs<int, Edge<int>>(edge, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MsaglEdgeEventArgs<int, Edge<int>>(default, msaglEdge)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MsaglEdgeEventArgs<int, Edge<int>>(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }
    }
}
