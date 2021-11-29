using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FastGraph.Graphviz;

namespace FastGraph.Samples
{
    [TestClass]
    public class GraphvizSamples
    {
        [TestMethod]
        public void RenderGraphWithGraphviz()
        {
            var edges = new SEdge<int>[] { 
                new SEdge<int>(1, 2), 
                new SEdge<int>(0, 1),
                new SEdge<int>(0, 3),
                new SEdge<int>(2, 3)
            };
            var graph = edges.ToAdjacencyGraph<int, SEdge<int>>();
            Console.WriteLine(graph.ToGraphviz());
        }
    }
}
