#nullable enable

using System.Data;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace FastGraph.Data.Tests
{
    internal static class GraphTestHelpers
    {
        [CustomAssertion]
        public static void HasRelations<TGraph>(this AndWhichConstraint<ObjectAssertions, TGraph> graph, IReadOnlyList<DataRelation> expectedRelations)
            where TGraph : IEdgeSet<DataTable, DataRelationEdge>

        {
            expectedRelations.Should().NotBeEmpty();

            using (_ = new AssertionScope())
            {
                graph.Which.IsEdgesEmpty.Should().BeFalse();
                graph.Which.EdgeCount.Should().Be(expectedRelations.Count);
                graph.Which.Edges.Select(r => r.Relation).Should().BeEquivalentTo(expectedRelations);
            }
        }
    }
}
