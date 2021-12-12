#nullable enable

using NUnit.Framework;

namespace FastGraph.Tests.Structures
{
    /// <summary>
    /// Tests related to array graphs.
    /// </summary>
    [TestFixture]
    internal sealed class ArrayGraphTests
    {
        #region Test helpers

        private static void AssertSameProperties<TVertex, TEdge>(IVertexAndEdgeListGraph<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            ArrayAdjacencyGraph<TVertex, TEdge> adjacencyGraph = graph.ToArrayAdjacencyGraph();

            Assert.AreEqual(graph.VertexCount, adjacencyGraph.VertexCount);
            CollectionAssert.AreEqual(graph.Vertices, adjacencyGraph.Vertices);

            Assert.AreEqual(graph.EdgeCount, adjacencyGraph.EdgeCount);
            CollectionAssert.AreEqual(graph.Edges, adjacencyGraph.Edges);

            foreach (TVertex vertex in graph.Vertices)
                CollectionAssert.AreEqual(graph.OutEdges(vertex), adjacencyGraph.OutEdges(vertex));
        }

        private static void AssertSameProperties<TVertex, TEdge>(IBidirectionalGraph<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            ArrayBidirectionalGraph<TVertex, TEdge> bidirectionalGraph = graph.ToArrayBidirectionalGraph();

            Assert.AreEqual(graph.VertexCount, bidirectionalGraph.VertexCount);
            CollectionAssert.AreEqual(graph.Vertices, bidirectionalGraph.Vertices);

            Assert.AreEqual(graph.EdgeCount, bidirectionalGraph.EdgeCount);
            CollectionAssert.AreEqual(graph.Edges, bidirectionalGraph.Edges);

            foreach (TVertex vertex in graph.Vertices)
                CollectionAssert.AreEqual(graph.OutEdges(vertex), bidirectionalGraph.OutEdges(vertex));
        }

        private static void AssertSameProperties<TVertex, TEdge>(IUndirectedGraph<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            ArrayUndirectedGraph<TVertex, TEdge> undirectedGraph = graph.ToArrayUndirectedGraph();

            Assert.AreEqual(graph.VertexCount, undirectedGraph.VertexCount);
            CollectionAssert.AreEqual(graph.Vertices, undirectedGraph.Vertices);

            Assert.AreEqual(graph.EdgeCount, undirectedGraph.EdgeCount);
            CollectionAssert.AreEqual(graph.Edges, undirectedGraph.Edges);

            foreach (TVertex vertex in graph.Vertices)
                CollectionAssert.AreEqual(graph.AdjacentEdges(vertex), undirectedGraph.AdjacentEdges(vertex));
        }

        #endregion

        [Test]
        public void ConversionToArrayGraph()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs_All())
                AssertSameProperties(graph);
            foreach (BidirectionalGraph<string, Edge<string>> graph in TestGraphFactory.GetBidirectionalGraphs_All())
                AssertSameProperties(graph);
            foreach (UndirectedGraph<string, Edge<string>> graph in TestGraphFactory.GetUndirectedGraphs_All())
                AssertSameProperties(graph);
        }
    }
}
