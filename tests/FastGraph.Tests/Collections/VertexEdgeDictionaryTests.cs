#nullable enable

using NUnit.Framework;
using FastGraph.Collections;

namespace FastGraph.Tests.Collections
{
    /// <summary>
    /// Tests for <see cref="VertexEdgeDictionary{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class VertexEdgeDictionaryTests
    {
        [Test]
        public void Constructors()
        {
            // ReSharper disable ObjectCreationAsStatement
            Invoking((Func<VertexEdgeDictionary<int, Edge<int>>>)(() => new VertexEdgeDictionary<int, Edge<int>>())).Should().NotThrow();
            Invoking((Func<VertexEdgeDictionary<int, Edge<int>>>)(() => new VertexEdgeDictionary<int, Edge<int>>(12))).Should().NotThrow();
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Clone()
        {
            var dictionary = new VertexEdgeDictionary<int, EquatableEdge<int>>();

            VertexEdgeDictionary<int, EquatableEdge<int>> clonedDictionary = dictionary.Clone();
            clonedDictionary.Should().BeEmpty();

            clonedDictionary = (VertexEdgeDictionary<int, EquatableEdge<int>>)((IVertexEdgeDictionary<int, EquatableEdge<int>>)dictionary).Clone();
            clonedDictionary.Should().BeEmpty();

            clonedDictionary = (VertexEdgeDictionary<int, EquatableEdge<int>>)((ICloneable)dictionary).Clone();
            clonedDictionary.Should().BeEmpty();

            dictionary.Add(1, new EdgeList<int, EquatableEdge<int>> { new EquatableEdge<int>(1, 2) });
            dictionary.Add(2, new EdgeList<int, EquatableEdge<int>> { new EquatableEdge<int>(2, 3) });
            dictionary.Add(3, new EdgeList<int, EquatableEdge<int>>());
            clonedDictionary = dictionary.Clone();
            dictionary.Should().BeEquivalentTo(clonedDictionary);

            clonedDictionary = (VertexEdgeDictionary<int, EquatableEdge<int>>)((IVertexEdgeDictionary<int, EquatableEdge<int>>)dictionary).Clone();
            dictionary.Should().BeEquivalentTo(clonedDictionary);

            clonedDictionary = (VertexEdgeDictionary<int, EquatableEdge<int>>)((ICloneable)dictionary).Clone();
            dictionary.Should().BeEquivalentTo(clonedDictionary);
        }
    }
}
