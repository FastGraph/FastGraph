using System.Collections.Generic;
using NUnit.Framework;
using QuikGraph.Serialization;
using QuikGraph.Tests;

namespace QuikGraph.Algorithms.Condensation
{
    [TestFixture]
    internal partial class StronglyConnectedCondensationGraphAlgorithmTest : QuikGraphUnitTests
    {
        [Test]
        public void StronglyConnectedCondensateAll()
        {
            foreach (var g in TestGraphFactory.GetAdjacencyGraphs())
                this.StronglyConnectedCondensate(g);
        }

        public void StronglyConnectedCondensate<TVertex, TEdge>(
            IVertexAndEdgeListGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            var cg = g.CondensateStronglyConnected<TVertex, TEdge, AdjacencyGraph<TVertex,TEdge>>();

            CheckVertexCount(g, cg);
            CheckEdgeCount(g, cg);
            CheckComponentCount(g, cg);
            CheckDAG(g, cg);
        }

        private void CheckVertexCount<TVertex, TEdge>(
            IVertexAndEdgeListGraph<TVertex, TEdge> g,
            IMutableBidirectionalGraph<AdjacencyGraph<TVertex, TEdge>, CondensedEdge<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>> cg)
            where TEdge : IEdge<TVertex>
        {
            int count = 0;
            foreach (var vertices in cg.Vertices)
                count += vertices.VertexCount;
            Assert.AreEqual(g.VertexCount, count, "VertexCount does not match");
        }

        private void CheckEdgeCount<TVertex, TEdge>(
            IVertexAndEdgeListGraph<TVertex, TEdge> g,
            IMutableBidirectionalGraph<AdjacencyGraph<TVertex, TEdge>, CondensedEdge<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>> cg)
            where TEdge : IEdge<TVertex>
        {
            // check edge count
            int count = 0;
            foreach (var edges in cg.Edges)
                count += edges.Edges.Count;
            foreach (var vertices in cg.Vertices)
                count += vertices.EdgeCount;
            Assert.AreEqual(g.EdgeCount, count, "EdgeCount does not match");
        }


        private void CheckComponentCount<TVertex, TEdge>(
            IVertexAndEdgeListGraph<TVertex, TEdge> g,
            IMutableBidirectionalGraph<AdjacencyGraph<TVertex, TEdge>, CondensedEdge<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>> cg)
            where TEdge : IEdge<TVertex>
        {
            // check number of vertices = number of strongly connected components
            var components = new Dictionary<TVertex, int>();
            int componentCount = g.StronglyConnectedComponents(components);
            Assert.AreEqual(componentCount, cg.VertexCount, "ComponentCount does not match");
        }

        private void CheckDAG<TVertex, TEdge>(
            IVertexAndEdgeListGraph<TVertex, TEdge> g,
            IMutableBidirectionalGraph<AdjacencyGraph<TVertex, TEdge>, CondensedEdge<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>> cg)
            where TEdge : IEdge<TVertex>
        {
            // Check it's a dag
            try
            {
                cg.TopologicalSort();
            }
            catch (NonAcyclicGraphException)
            {
                Assert.Fail("Graph is not a DAG.");
            }
        }
    }
}
