#nullable enable

using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Microsoft.Msagl.Drawing;

namespace FastGraph.MSAGL.Tests
{
    internal static class MsaglGraphTestHelpers
    {
        [CustomAssertion]
        public static void BeEquivalentTo<TVertex, TEdge>(this AndWhichConstraint<ObjectAssertions, Graph> actual, IEdgeListGraph<TVertex, TEdge> expected)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            using (_ = new AssertionScope())
            {
                actual.Which.Directed.Should().Be(expected.IsDirected);
                actual.Which.NodeCount.Should().Be(expected.VertexCount);
                actual.Which.EdgeCount.Should().Be(expected.EdgeCount);
            }
        }
    }
}
