#nullable enable

using NUnit.Framework;
using FastGraph.Collections;

namespace FastGraph.Tests.Collections
{
    /// <summary>
    /// Tests for <see cref="VertexList{TVertex}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class VertexListTests
    {
        [Test]
        public void Constructors()
        {
            // ReSharper disable ObjectCreationAsStatement
            Invoking((Func<VertexList<int>>)(() => new VertexList<int>())).Should().NotThrow();
            Invoking((Func<VertexList<int>>)(() => new VertexList<int>(12))).Should().NotThrow();
            var list = new VertexList<int> { 1, 2, 3 };
            var otherList = new VertexList<int>(list);
            list.Should().BeEquivalentTo(otherList);
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Clone()
        {
            var list = new VertexList<int>();

            VertexList<int> clonedList = list.Clone();
            clonedList.Should().BeEmpty();

            clonedList = (VertexList<int>)((ICloneable)list).Clone();
            clonedList.Should().BeEmpty();

            list.AddRange(new[] { 1, 2, 3 });
            clonedList = list.Clone();
            list.Should().BeEquivalentTo(clonedList);

            clonedList = (VertexList<int>)((ICloneable)list).Clone();
            list.Should().BeEquivalentTo(clonedList);
        }
    }
}
