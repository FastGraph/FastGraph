using System;
using JetBrains.Annotations;
using NUnit.Framework;

namespace QuikGraph.Tests
{
    /// <summary>
    /// Tests for <see cref="EdgeEventArgs{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class EdgeEventArgsTests
    {
        #region Helpers

        public EdgeEventArgs<TVertex, TEdge> Constructor<TVertex, TEdge>([NotNull] TEdge edge)
            where TEdge : IEdge<TVertex>
        {
            // TODO: add assertions to method EdgeEventArgsTVertexTEdgeTest.Constructor(!!1)
            EdgeEventArgs<TVertex, TEdge> target = new EdgeEventArgs<TVertex, TEdge>(edge);
            return target;
        }

        #endregion

        [Test]
        public void Constructor1()
        {
            Edge<int> edge = EdgeFactory.Create(0, 0);
            EdgeEventArgs<int, Edge<int>> edgeEventArgs = Constructor<int, Edge<int>>(edge);
            Assert.IsNotNull(edgeEventArgs);
            Assert.IsNotNull(edgeEventArgs.Edge);
        }

        [Test]
        public void Constructor2()
        {
            EdgeEventArgs<int, SEdge<int>> edgeEventArgs = Constructor<int, SEdge<int>>(default);
            Assert.IsNotNull(edgeEventArgs);
        }

        [Test]
        public void ConstructorThrowsContractException()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => Constructor<int, Edge<int>>(null));
        }
    }
}
