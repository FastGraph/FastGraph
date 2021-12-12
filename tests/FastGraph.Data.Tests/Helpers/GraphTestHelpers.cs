#nullable enable

using System.Data;
using NUnit.Framework;

namespace FastGraph.Data.Tests
{
    internal static class GraphTestHelpers
    {
        public static void AssertHasRelations(
            IEdgeSet<DataTable, DataRelationEdge> graph,
            IEnumerable<DataRelationEdge> relations)
        {
            DataRelation[] relationArray = relations
                .Select(r => r.Relation)
                .ToArray();
            CollectionAssert.IsNotEmpty(relationArray);

            Assert.IsFalse(graph.IsEdgesEmpty);
            Assert.AreEqual(relationArray.Length, graph.EdgeCount);
            CollectionAssert.AreEquivalent(
                relationArray,
                graph.Edges.Select(r => r.Relation));
        }
    }
}
