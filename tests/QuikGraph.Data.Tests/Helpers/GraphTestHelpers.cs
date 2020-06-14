using System.Collections.Generic;
using System.Data;
using System.Linq;
using JetBrains.Annotations;
using NUnit.Framework;

namespace QuikGraph.Data.Tests
{
    internal static class GraphTestHelpers
    {
        public static void AssertHasRelations(
            [NotNull] IEdgeSet<DataTable, DataRelationEdge> graph,
            [NotNull, ItemNotNull] IEnumerable<DataRelationEdge> relations)
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