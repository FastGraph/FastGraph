using System;
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
            Assert.IsFalse(args.Reversed);
            Assert.AreSame(edge, args.Edge);
            Assert.AreEqual(1, args.Source);
            Assert.AreEqual(2, args.Target);

            args = new UndirectedEdgeEventArgs<int, Edge<int>>(edge, true);
            Assert.IsTrue(args.Reversed);
            Assert.AreSame(edge, args.Edge);
            Assert.AreEqual(2, args.Source);
            Assert.AreEqual(1, args.Target);
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => new UndirectedEdgeEventArgs<int, Edge<int>>(null, false));
            Assert.Throws<ArgumentNullException>(
                () => new UndirectedEdgeEventArgs<int, Edge<int>>(null, true));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }
    }
}