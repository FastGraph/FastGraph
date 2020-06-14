using System;
using NUnit.Framework;

namespace QuikGraph.Tests.Events
{
    /// <summary>
    /// Tests related to <see cref="EdgeEventArgs{TVertex,TEdge}"/>.
    /// </summary>
    internal class EdgeEventArgsTests
    {
        [Test]
        public void Constructor()
        {
            var edge = new Edge<int>(1, 2);
            var args = new EdgeEventArgs<int, Edge<int>>(edge);

            Assert.AreSame(edge, args.Edge);
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => new EdgeEventArgs<int, Edge<int>>(null));
        }
    }
}