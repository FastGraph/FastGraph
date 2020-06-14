using System;
using Microsoft.Msagl.Drawing;
using NUnit.Framework;
using QuikGraph.Tests;

namespace QuikGraph.MSAGL.Tests
{
    /// <summary>
    /// Tests related to <see cref="MsaglVertexEventArgs{TVertex}"/>.
    /// </summary>
    internal class MsaglVertexEventArgsTests
    {
        [Test]
        public void Constructor()
        {
            var vertex = new TestVertex("1");
            var node = new Node("1");
            var args = new MsaglVertexEventArgs<TestVertex>(vertex, node);

            Assert.AreSame(vertex, args.Vertex);
            Assert.AreSame(node, args.Node);
        }

        [Test]
        public void Constructor_Throws()
        {
            var vertex = new TestVertex("1");
            var node = new Node("1");
            
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => new MsaglVertexEventArgs<TestVertex>(vertex, null));
            Assert.Throws<ArgumentNullException>(
                () => new MsaglVertexEventArgs<TestVertex>(null, node));
            Assert.Throws<ArgumentNullException>(
                () => new MsaglVertexEventArgs<TestVertex>(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }
    }
}