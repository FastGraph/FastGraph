using JetBrains.Annotations;
using NUnit.Framework;
using Microsoft.Msagl.Drawing;

namespace QuikGraph.MSAGL.Tests
{
    internal static class MsaglGraphTestHelpers
    {
        public static void AssertAreEquivalent<TVertex, TEdge>(
            [NotNull] IEdgeListGraph<TVertex, TEdge> graph,
            [NotNull] Graph msaglGraph)
            where TEdge : IEdge<TVertex>
        {
            Assert.AreEqual(graph.IsDirected, msaglGraph.Directed);
            Assert.AreEqual(graph.VertexCount, msaglGraph.NodeCount);
            Assert.AreEqual(graph.EdgeCount, msaglGraph.EdgeCount);
        }
    }
}