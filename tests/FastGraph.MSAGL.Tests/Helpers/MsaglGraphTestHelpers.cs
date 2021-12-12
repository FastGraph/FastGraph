#nullable enable

using NUnit.Framework;
using Microsoft.Msagl.Drawing;

namespace FastGraph.MSAGL.Tests
{
    internal static class MsaglGraphTestHelpers
    {
        public static void AssertAreEquivalent<TVertex, TEdge>(
            IEdgeListGraph<TVertex, TEdge> graph,
            Graph msaglGraph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            Assert.AreEqual(graph.IsDirected, msaglGraph.Directed);
            Assert.AreEqual(graph.VertexCount, msaglGraph.NodeCount);
            Assert.AreEqual(graph.EdgeCount, msaglGraph.EdgeCount);
        }
    }
}
