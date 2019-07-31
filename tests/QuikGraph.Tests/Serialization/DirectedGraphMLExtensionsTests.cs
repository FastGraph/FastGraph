using System;
using NUnit.Framework;
using QuikGraph.Serialization;
using QuikGraph.Serialization.DirectedGraphML;

namespace QuikGraph.Tests.Serialization
{
    /// <summary>
    /// Tests for <see cref="DirectedGraphMLExtensions"/>.
    /// </summary>
    [TestFixture]
    internal class DirectedGraphMLExtensionsTests : QuikGraphUnitTests
    {
        [Test]
        public void SimpleGraph()
        {
            using (SetTemporaryTestWorkingDirectory())
            {
                int[][] edges =
                {
                    new[]{ 1, 2, 3 },
                    new[]{ 2, 3, 1 }
                };

                edges
                    .ToAdjacencyGraph()
                    .ToDirectedGraphML()
                    .WriteXml("simple.dgml");

                edges
                    .ToAdjacencyGraph()
                    .ToDirectedGraphML()
                    .WriteXml(Console.Out);
            }
        }

        [Test]
        public void ToDirectedGraphML()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs())
            {
                DirectedGraph directedGraph = graph.ToDirectedGraphML();
                Assert.IsNotNull(graph);
                Assert.AreEqual(directedGraph.Nodes.Length, graph.VertexCount);
                Assert.AreEqual(directedGraph.Links.Length, graph.EdgeCount);
            }
        }
    }
}
