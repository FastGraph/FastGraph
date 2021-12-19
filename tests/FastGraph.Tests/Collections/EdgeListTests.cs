#nullable enable

using NUnit.Framework;
using FastGraph.Collections;

namespace FastGraph.Tests.Collections
{
    /// <summary>
    /// Tests for <see cref="EdgeList{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class EdgeListTests
    {
        [Test]
        public void Constructors()
        {
            // ReSharper disable ObjectCreationAsStatement
            Invoking((Func<EdgeList<int, Edge<int>>>)(() => new EdgeList<int, Edge<int>>())).Should().NotThrow();
            Invoking((Func<EdgeList<int, Edge<int>>>)(() => new EdgeList<int, Edge<int>>(12))).Should().NotThrow();
            var list = new EdgeList<int, EquatableEdge<int>>
            {
                new EquatableEdge<int>(1, 2),
                new EquatableEdge<int>(2, 3)
            };
            var otherList = new EdgeList<int, EquatableEdge<int>>(list);
            list.Should().BeEquivalentTo(otherList);
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Clone()
        {
            var list = new EdgeList<int, EquatableEdge<int>>();

            EdgeList<int, EquatableEdge<int>> clonedList = list.Clone();
            clonedList.Should().BeEmpty();

            clonedList = (EdgeList<int, EquatableEdge<int>>)((IEdgeList<int, EquatableEdge<int>>)list).Clone();
            clonedList.Should().BeEmpty();

            clonedList = (EdgeList<int, EquatableEdge<int>>)((ICloneable)list).Clone();
            clonedList.Should().BeEmpty();

            list.AddRange(new[]
            {
                new EquatableEdge<int>(1, 2),
                new EquatableEdge<int>(2, 3)
            });
            clonedList = list.Clone();
            list.Should().BeEquivalentTo(clonedList);

            clonedList = (EdgeList<int, EquatableEdge<int>>)((IEdgeList<int, EquatableEdge<int>>)list).Clone();
            list.Should().BeEquivalentTo(clonedList);

            clonedList = (EdgeList<int, EquatableEdge<int>>)((ICloneable)list).Clone();
            list.Should().BeEquivalentTo(clonedList);
        }
    }
}
