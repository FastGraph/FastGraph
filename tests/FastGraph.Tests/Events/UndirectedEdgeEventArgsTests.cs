#nullable enable

using NUnit.Framework;

namespace FastGraph.Tests.Events
{
    /// <summary>
    /// Tests related to <see cref="UndirectedEdgeEventArgs{TVertex,TEdge}"/>.
    /// </summary>
    internal sealed class UndirectedEdgeEventArgsTests
    {
        [Test]
        public void Constructor()
        {
            var edge = new Edge<int>(1, 2);

            var args = new UndirectedEdgeEventArgs<int, Edge<int>>(edge, false);
            args.Reversed.Should().BeFalse();
            args.Edge.Should().BeSameAs(edge);
            args.Source.Should().Be(1);
            args.Target.Should().Be(2);

            args = new UndirectedEdgeEventArgs<int, Edge<int>>(edge, true);
            args.Reversed.Should().BeTrue();
            args.Edge.Should().BeSameAs(edge);
            args.Source.Should().Be(2);
            args.Target.Should().Be(1);
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new UndirectedEdgeEventArgs<int, Edge<int>>(default, false)).Should().Throw<ArgumentNullException>();
            Invoking(() => new UndirectedEdgeEventArgs<int, Edge<int>>(default, true)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }
    }
}
