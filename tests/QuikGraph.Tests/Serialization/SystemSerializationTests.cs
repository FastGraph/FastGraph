using System.IO;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Serialization;
using QuikGraph.Contracts;

namespace QuikGraph.Tests.Serialization
{
    /// <summary>
    /// Tests relative to serialization via standard API.
    /// </summary>
    [TestFixture]
    internal class SystemSerializationTests
    {
        #region Helpers

        [Pure]
        [NotNull]
        private static TGraph SerializeDeserialize<TVertex, TEdge, TGraph>([NotNull] TGraph graph)
            where TGraph : IGraph<TVertex, TEdge>
            where TEdge : IEdge<TVertex>
        {
            Assert.IsNotNull(graph);

            // Serialize
            using (var stream = new MemoryStream())
            {
                graph.SerializeToBinary(stream);

                // Deserialize
                stream.Position = 0;

                TGraph result = stream.DeserializeFromBinary<TVertex, TEdge, TGraph>();
                Assert.IsNotNull(result);

                return result;
            }
        }

        private static void AssertGraphsEqual(
            [NotNull] IEdgeListGraph<int, EquatableEdge<int>> expected,
            [NotNull] IEdgeListGraph<int, EquatableEdge<int>> graph)
        {
            // Check equal
            Assert.IsTrue(expected.VertexCountEqual(graph));
            Assert.IsTrue(expected.EdgeCountEqual(graph));

            foreach (int vertex in expected.Vertices)
                Assert.IsTrue(graph.ContainsVertex(vertex));
            foreach (Edge<int> edge in graph.Edges)
                Assert.IsTrue(graph.ContainsEdge(new EquatableEdge<int>(edge.Source, edge.Target)));
        }

        #endregion

        [Test]
        public void AdjacencyGraph()
        {
            var graph = new AdjacencyGraph<int, EquatableEdge<int>>();
            // Populate
            graph.AddVertex(0);
            graph.AddVertex(1);
            graph.AddEdge(new EquatableEdge<int>(0, 1));

            AdjacencyGraph<int, EquatableEdge<int>> result =
                SerializeDeserialize<int, EquatableEdge<int>, AdjacencyGraph<int, EquatableEdge<int>>>(graph);
            AssertGraphsEqual(graph, result);
        }

        [Test]
        public void BidirectionalGraph()
        {
            var graph = new BidirectionalGraph<int, EquatableEdge<int>>();
            // Populate
            graph.AddVertex(0);
            graph.AddVertex(1);
            graph.AddEdge(new EquatableEdge<int>(0, 1));

            BidirectionalGraph<int, EquatableEdge<int>> result =
                SerializeDeserialize<int, EquatableEdge<int>, BidirectionalGraph<int, EquatableEdge<int>>>(graph);
            AssertGraphsEqual(graph, result);
        }

        [Test]
        public void UndirectedGraph()
        {
            var graph = new UndirectedGraph<int, EquatableEdge<int>>();
            // Populate
            graph.AddVertex(0);
            graph.AddVertex(1);
            graph.AddEdge(new EquatableEdge<int>(0, 1));

            UndirectedGraph<int, EquatableEdge<int>> result =
                SerializeDeserialize<int, EquatableEdge<int>, UndirectedGraph<int, EquatableEdge<int>>>(graph);
            AssertGraphsEqual(graph, result);
        }
    }
}
