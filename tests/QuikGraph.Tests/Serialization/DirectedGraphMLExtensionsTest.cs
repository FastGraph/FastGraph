using System;
using System.Diagnostics;
using NUnit.Framework;
using QuikGraph.Serialization;
using QuikGraph.Tests;

namespace QuikGraph.Tests.Serialization
{
    [TestFixture]
    internal class DirectedGraphMLExtensionsTest : QuikGraphUnitTests
    {
        [Test]
        public void SimpleGraph()
        {
            using (SetTemporaryTestWorkingDirectory())
            {
                int[][] edges = { new int[]{ 1, 2, 3 }, 
                              new int[]{ 2, 3, 1 } };
                edges.ToAdjacencyGraph()
                    .ToDirectedGraphML()
                    .WriteXml("simple.dgml");

                if (Debugger.IsAttached)
                { 
                    Process.Start("simple.dgml");
                }

                edges.ToAdjacencyGraph()
                    .ToDirectedGraphML()
                    .WriteXml(Console.Out);
            }
        }

        [Test]
        public void ToDirectedGraphML()
        {
            foreach (var g in TestGraphFactory.GetAdjacencyGraphs())
            {
                var dg = g.ToDirectedGraphML();
                Assert.IsNotNull(g);
                Assert.AreEqual(dg.Nodes.Length, g.VertexCount);
                Assert.AreEqual(dg.Links.Length, g.EdgeCount);
            }
        }
    }
}
