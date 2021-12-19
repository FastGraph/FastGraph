#nullable enable

using NUnit.Framework;

namespace FastGraph.Tests.Events
{
    /// <summary>
    /// Tests related to <see cref="VertexEventArgs{TVertex}"/>.
    /// </summary>
    internal sealed class VertexEventArgsTests
    {
        [Test]
        public void Constructor()
        {
            var vertex = new TestVertex("1");
            var args = new VertexEventArgs<TestVertex>(vertex);

            args.Vertex.Should().BeSameAs(vertex);
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new VertexEventArgs<TestVertex>(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }
    }
}
