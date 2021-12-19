#nullable enable

using NUnit.Framework;

namespace FastGraph.Tests.Events
{
    /// <summary>
    /// Tests related to <see cref="EdgeEventArgs{TVertex,TEdge}"/>.
    /// </summary>
    internal sealed class EdgeEventArgsTests
    {
        [Test]
        public void Constructor()
        {
            var edge = new Edge<int>(1, 2);
            var args = new EdgeEventArgs<int, Edge<int>>(edge);

            args.Edge.Should().BeSameAs(edge);
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new EdgeEventArgs<int, Edge<int>>(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }
    }
}
